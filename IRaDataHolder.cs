namespace RaDataHolder
{
	public interface IRaDataHolder : IRaDataSetResolver, IRaDataClearResolver
	{
		bool HasData
		{
			get;
		}

		IRaDataSetResolver SetRawData(object data);
		IRaDataClearResolver ClearData();
	}

	public interface IRaDataHolder<TData> : IRaDataHolder
	{
		IRaDataSetResolver SetData(TData data);
	}

	public interface IRaDataSetResolver
	{
		IRaDataSetResolver Resolve();
	}

	public interface IRaDataClearResolver
	{
		IRaDataClearResolver Resolve();
	}
}
