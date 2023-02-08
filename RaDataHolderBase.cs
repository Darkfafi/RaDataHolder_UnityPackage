using System;

namespace RaDataHolder
{
	public abstract class RaDataHolderBase<TData> : IRaDataHolder<TData>, IDisposable
	{
		public delegate void DataHandler(RaDataHolderBase<TData> holder);
		public event DataHandler DataDisplayedEvent;
		public event DataHandler DataClearedEvent;

		private RaDataHolderCore<TData> _core = null;

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
				OnDisplay();
			},
			(core) =>
			{
				OnClear();
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
			_core.Dispose();
			_core = null;

			OnClear();
			OnDispose();

			Data = default;
		}

		protected abstract void OnDisplay();
		protected abstract void OnClear();

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
