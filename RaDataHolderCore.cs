using System;

namespace RaDataHolder
{
	public class RaDataHolderCore<TData> : IRaDataHolder<TData>, IDisposable
	{
		public delegate void DataHandler(RaDataHolderCore<TData> core);

		public event DataHandler DataDisplayedEvent;
		public event DataHandler DataClearedEvent;

		private State _state = State.None;
		private DataHandler _displayLogics;
		private DataHandler _clearLogics;

		public bool HasData => _state == State.Displaying;

		public TData Data
		{
			get; private set;
		}

		public RaDataHolderCore(DataHandler displayLogics, DataHandler clearLogics)
		{
			_displayLogics = displayLogics;
			_clearLogics = clearLogics;
		}

		public void SetData(object data)
		{
			if(data is TData castedData)
			{
				SetData(castedData);
			}
			else
			{
				throw new InvalidCastException($"Data passed was not of type {typeof(TData)}");
			}
		}

		public void SetData(TData data)
		{
			if(_state != State.None)
			{
				return;
			}

			_state = State.Assembling;
			Data = data;

			_displayLogics?.Invoke(this);

			_state = State.Displaying;

			DataDisplayedEvent?.Invoke(this);
		}

		public void ClearData()
		{
			if(_state != State.Displaying)
			{
				return;
			}

			_state = State.Disassembling;

			_clearLogics?.Invoke(this);
			DataClearedEvent?.Invoke(this);

			Data = default;
			_state = State.None;
		}

		public void Dispose()
		{
			DataDisplayedEvent = null;
			DataClearedEvent = null;
			Data = default;
			_state = default;

			_clearLogics = default;
			_displayLogics = default;
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
