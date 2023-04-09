namespace RaDataHolder
{
	public interface IRaDataHolder : IRaDataSetResolver, IRaDataClearResolver
	{
		bool HasData
		{
			get;
		}

		public bool IsReplacingData
		{
			get;
		}

		IRaDataSetResolver SetRawData(object data, bool resolve = true);
		IRaDataClearResolver ClearData(bool resolve = true);
	}

	public interface IRaDataHolder<TData> : IRaDataHolder
	{
		void ReplaceData(TData data, bool ignoreOnEqual = true);
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
