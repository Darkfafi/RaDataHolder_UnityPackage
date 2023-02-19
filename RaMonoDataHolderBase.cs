using UnityEngine;

namespace RaDataHolder
{
	public abstract class RaMonoDataHolderBase<TData> : MonoBehaviour, IRaDataHolder<TData>
	{
		public delegate void DataHandler(RaMonoDataHolderBase<TData> holder);
		public event DataHandler DataDisplayedEvent;
		public event DataHandler DataClearedEvent;

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

			OnClearData();
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
			});

			_core.DataDisplayedEvent += OnDataDisplayedEvent;
			_core.DataClearedEvent += OnDataClearedEvent;
		}

		public void SetData(TData data)
		{
			Core.SetData(data);
		}

		public void SetData(object data)
		{
			Core.SetData(data);
		}

		public void ClearData()
		{
			Core.ClearData();
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
