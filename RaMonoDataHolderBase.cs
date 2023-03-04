using UnityEngine;

namespace RaDataHolder
{
	public abstract class RaMonoDataHolderBase<TData> : MonoBehaviour, IRaDataHolder<TData>
	{
		public delegate void DataHandler(RaMonoDataHolderBase<TData> holder);
		public event DataHandler DataSetEvent;
		public event DataHandler DataClearedEvent;
		public event DataHandler DataSetResolvedEvent;
		public event DataHandler DataClearResolvedEvent;

		private bool _isDestroyed = false;

		public bool HasData => _core != null && _core.HasData;

		protected RaDataHolderCore<TData> Core
		{
			get
			{
				TryInitialize();
				return _core;
			}
		}
		private RaDataHolderCore<TData> _core = null;

		protected TData Data
		{
			get; private set;
		}

		protected void Awake()
		{
			TryInitialize();
		}

		protected void OnDestroy()
		{
			if(_isDestroyed)
			{
				return;
			}

			_isDestroyed = true;

			DataSetEvent = null;
			DataClearedEvent = null;
			DataSetResolvedEvent = null;
			DataClearResolvedEvent = null;

			_core.ClearData(true);
			
			OnDispose();
			
			_core.Dispose();
			_core = null;

			Data = default;
		}

		private void TryInitialize()
		{
			if(_isDestroyed || _core != null)
			{
				return;
			}

			_core = new RaDataHolderCore<TData>(
			(core) =>
			{
				Data = core.Data;
				OnSetData();
			},
			(core) =>
			{
				OnClearData();
				Data = default;
			},
			(core)=> 
			{
				OnSetDataResolved();
			}, 
			(core)=> 
			{
				OnClearDataResolved();
			});

			_core.DataSetEvent += OnDataSetEvent;
			_core.DataClearedEvent += OnDataClearedEvent;
			_core.DataSetResolvedEvent += OnDataSetResolvedEvent;
			_core.DataClearResolvedEvent += OnDataClearResolvedEvent;
		}

		public IRaDataSetResolver SetData(TData data, bool resolve)
		{
			Core.SetData(data, resolve);
			return this;
		}

		public IRaDataSetResolver SetRawData(object data, bool resolve)
		{
			Core.SetRawData(data, resolve);
			return this;
		}

		public IRaDataClearResolver ClearData(bool resolve = true)
		{
			Core.ClearData(resolve);
			return this;
		}

		protected abstract void OnSetData();
		
		protected abstract void OnClearData();

		protected virtual void OnSetDataResolved()
		{

		}

		protected virtual void OnClearDataResolved()
		{
		
		}

		protected virtual void OnDispose()
		{

		}

		private void OnDataSetEvent(RaDataHolderCore<TData> core)
		{
			DataSetEvent?.Invoke(this);
		}

		private void OnDataClearedEvent(RaDataHolderCore<TData> core)
		{
			DataClearedEvent?.Invoke(this);
		}

		private void OnDataSetResolvedEvent(RaDataHolderCore<TData> core)
		{
			DataSetResolvedEvent?.Invoke(this);
		}

		private void OnDataClearResolvedEvent(RaDataHolderCore<TData> core)
		{
			DataClearResolvedEvent?.Invoke(this);
		}

		IRaDataSetResolver IRaDataSetResolver.Resolve()
		{
			((IRaDataSetResolver)Core).Resolve();
			return this;
		}

		IRaDataClearResolver IRaDataClearResolver.Resolve()
		{
			((IRaDataClearResolver)Core).Resolve();
			return this;
		}
	}
}
