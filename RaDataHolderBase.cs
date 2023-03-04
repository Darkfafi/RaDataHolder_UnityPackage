using System;

namespace RaDataHolder
{
	public abstract class RaDataHolderBase<TData> : IRaDataHolder<TData>, IDisposable
	{
		public delegate void DataHandler(RaDataHolderBase<TData> holder);
		public event DataHandler DataSetEvent;
		public event DataHandler DataClearedEvent;
		public event DataHandler DataSetResolvedEvent;
		public event DataHandler DataClearResolvedEvent;

		private RaDataHolderCore<TData> _core = null;

		private bool _isDestroyed = false;

		public bool HasData => _core != null && _core.HasData;

		protected TData Data
		{
			get; private set;
		}

		public RaDataHolderBase()
		{
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

			_core.DataSetEvent += OnDataDisplayedEvent;
			_core.DataClearedEvent += OnDataClearedEvent;
		}

		public RaDataHolderBase(TData data, bool resolve)
			: base()
		{
			SetData(data, resolve);
		}

		public IRaDataSetResolver SetData(TData data, bool resolve)
		{
			_core.SetData(data, resolve);
			return this;
		}

		public IRaDataSetResolver SetRawData(object data, bool resolve)
		{
			_core.SetRawData(data, resolve);
			return this;
		}

		public IRaDataClearResolver ClearData(bool resolve)
		{
			_core.ClearData(resolve);
			return this;
		}

		public void Dispose()
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

		private void OnDataDisplayedEvent(RaDataHolderCore<TData> core)
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
			((IRaDataSetResolver)_core).Resolve();
			return this;
		}

		IRaDataClearResolver IRaDataClearResolver.Resolve()
		{
			((IRaDataClearResolver)_core).Resolve();
			return this;
		}
	}
}
