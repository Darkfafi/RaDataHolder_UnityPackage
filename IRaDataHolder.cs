namespace RaDataHolder
{
	public interface IRaDataHolder
	{
		bool HasData
		{
			get;
		}

		void SetData(object data);
		void ClearData();
	}

	public interface IRaDataHolder<TData> : IRaDataHolder
	{
		void SetData(TData data);
	}
}
