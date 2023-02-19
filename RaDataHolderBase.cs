using System;

namespace RaDataHolder
{
	public abstract class RaDataHolderBase<TData> : IRaDataHolder<TData>, IDisposable
	{
		public delegate void DataHandler(RaDataHolderBase<TData> holder);
		public event DataHandler DataDisplayedEvent;
		public event DataHandler DataClearedEvent;

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
			});

			_core.DataDisplayedEvent += OnDataDisplayedEvent;
			_core.DataClearedEvent += OnDataClearedEvent;
		}

		public RaDataHolderBase(TData data)
			: base()
		{
			SetData(data);
		}

		public void SetData(TData data)
		{
			_core.SetData(data);
		}

		public void SetData(object data)
		{
			_core.SetData(data);
		}

		public void ClearData()
		{
			_core.ClearData();
		}

		public void Dispose()
		{
			if(_isDestroyed)
			{
				return;
			}

			_isDestroyed = true;

			DataDisplayedEvent = null;
			DataClearedEvent = null;

			_core.ClearData();
			
			OnDispose();

			_core.Dispose();
			_core = null;

			Data = default;
		}

		protected abstract void OnSetData();
		protected abstract void OnClearData();

		protected virtual void OnDispose()
		{

		}

		private void OnDataDisplayedEvent(RaDataHolderCore<TData> core)
		{
			DataDisplayedEvent?.Invoke(this);
		}

		private void OnDataClearedEvent(RaDataHolderCore<TData> core)
		{
			DataClearedEvent?.Invoke(this);
		}
	}
}
