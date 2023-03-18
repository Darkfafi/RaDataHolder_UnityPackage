namespace RaDataHolder
{
	public interface IRaDataHolder : IRaDataSetResolver, IRaDataClearResolver
	{
		bool HasData
		{
			get;
		}

		IRaDataSetResolver SetRawData(object data, bool resolve = true);
		IRaDataClearResolver ClearData(bool resolve = true);
	}

	public interface IRaDataHolder<TData> : IRaDataHolder
	{
		IRaDataSetResolver SetData(TData data, bool resolve = true);
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
