namespace RaDataHolder
{
	public interface IRaDataHolder : IRaDataSetResolver, IRaDataClearResolver
	{
		bool HasData
		{
			get;
		}

		IRaDataSetResolver SetRawData(object data, bool resolve);
		IRaDataClearResolver ClearData(bool resolve);
	}

	public interface IRaDataHolder<TData> : IRaDataHolder
	{
		IRaDataSetResolver SetData(TData data, bool resolve);
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
