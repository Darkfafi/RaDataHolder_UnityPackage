using System;
using System.Collections.Generic;

namespace RaDataHolder
{
	public class RaDataHolderCore<TData> : IRaDataHolder<TData>, IDisposable
	{
		public delegate void DataHandler(RaDataHolderCore<TData> core);
		public delegate void DataChangeHandler(TData newData, TData oldData, RaDataHolderCore<TData> core);

		public event DataHandler DataSetEvent;
		public event DataHandler DataClearedEvent;
		public event DataHandler DataSetResolvedEvent;
		public event DataHandler DataClearResolvedEvent;
		public event DataChangeHandler DataReplacedEvent;

		private State _state = State.None;
		private ResolveState _resolveState = ResolveState.None;
		
		private DataHandler _setLogics;
		private DataHandler _clearLogics;

		private DataHandler _setResolveLogics;
		private DataHandler _clearResolveLogics;

		private HashSet<object> _replacingFlags;

		public bool HasData => _state == State.Displaying;

		public TData Data
		{
			get; private set;
		}

		public bool IsReplacingData => _replacingFlags.Count > 0;

		public RaDataHolderCore(DataHandler setLogics, DataHandler clearLogics, DataHandler setResolveLogics, DataHandler clearResolveLogics)
		{
			_replacingFlags = new HashSet<object>();
			_setLogics = setLogics;
			_clearLogics = clearLogics;
			_setResolveLogics = setResolveLogics;
			_clearResolveLogics = clearResolveLogics;
		}

		public IRaDataSetResolver SetRawData(object data, bool resolve = true)
		{
			if(data is TData castedData)
			{
				SetData(castedData, resolve);
				return this;
			}
			else
			{
				throw new InvalidCastException($"Data passed was not of type {typeof(TData)}");
			}
		}

		public void ReplaceData(TData data, bool ignoreOnEqual = true)
		{
			if(ignoreOnEqual)
			{
				if(Data == null && data == null)
				{
					return;
				}

				if(Data != null && Data.Equals(data))
				{
					return;
				}
			}

			object flag = new object();

			_replacingFlags.Add(flag);
			{
				TData oldData = Data;
				TData newData = data;

				ClearData();

				if(data != null)
				{
					SetData(data);
				}

				DataReplacedEvent?.Invoke(newData, oldData, this);
			}
			_replacingFlags.Remove(flag);

		}

		public IRaDataSetResolver SetData(TData data, bool resolve = true)
		{
			if(_state != State.None)
			{
				return this;
			}

			_state = State.Assembling;
			Data = data;

			_setLogics?.Invoke(this);

			_resolveState = ResolveState.AwaitSet;
			_state = State.Displaying;

			DataSetEvent?.Invoke(this);

			if(resolve)
			{
				Resolve();
			}

			return this;
		}

		public IRaDataClearResolver ClearData(bool resolve = true)
		{
			if(_state != State.Displaying)
			{
				return this;
			}

			_state = State.Disassembling;

			_clearLogics?.Invoke(this);
			
			_resolveState = ResolveState.AwaitClear;
			_state = State.None;

			DataClearedEvent?.Invoke(this);
			Data = default;

			if(resolve)
			{
				((IRaDataClearResolver)this).Resolve();
			}

			return this;
		}

		public void Dispose()
		{
			DataReplacedEvent = null;

			DataSetEvent = null;
			DataClearedEvent = null;

			DataSetResolvedEvent = null;
			DataClearResolvedEvent = null;

			Data = default;
			_state = default;
			_resolveState = default;

			_clearLogics = default;
			_setLogics = default;

			_clearResolveLogics = default;
			_setResolveLogics = default;

			_replacingFlags.Clear();
			_replacingFlags = default;
		}

		public IRaDataSetResolver Resolve()
		{
			if(_resolveState != ResolveState.AwaitSet)
			{
				return this;
			}

			_resolveState = ResolveState.Set;
			_setResolveLogics?.Invoke(this);
			DataSetResolvedEvent?.Invoke(this);

			return this;
		}

		IRaDataClearResolver IRaDataClearResolver.Resolve()
		{
			if(_resolveState != ResolveState.AwaitClear)
			{
				return this;
			}

			_resolveState = ResolveState.Clear;
			_clearResolveLogics?.Invoke(this);
			DataClearResolvedEvent?.Invoke(this);

			return this;
		}

		private enum ResolveState
		{
			None,

			// Set
			AwaitSet,
			Set,

			// Clear
			AwaitClear,
			Clear,
		}

		private enum State
		{
			/// Specific
			None,
			Displaying,

			/// Transition
			Assembling,
			Disassembling
		}
	}
}
