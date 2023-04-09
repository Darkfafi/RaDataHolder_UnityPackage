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
			_core.DataSetResolvedEvent += OnDataSetResolvedEvent;
			_core.DataClearResolvedEvent += OnDataClearResolvedEvent;
		}

		public RaDataHolderBase(TData data, bool resolve = true)
			: base()
		{
			SetData(data, resolve);
		}

		public void ReplaceData(TData data)
		{
			ClearData();
			SetData(data);
		}

		public IRaDataSetResolver SetData(TData data, bool resolve = true)
		{
			_core.SetData(data, resolve);
			return this;
		}

		public IRaDataSetResolver SetRawData(object data, bool resolve = true)
		{
			_core.SetRawData(data, resolve);
			return this;
		}

		public IRaDataClearResolver ClearData(bool resolve = true)
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

			DataSetResolvedEvent = null;
			DataClearResolvedEvent = null;
			DataSetEvent = null;
			DataClearedEvent = null;

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

		public IRaDataSetResolver Resolve()
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
