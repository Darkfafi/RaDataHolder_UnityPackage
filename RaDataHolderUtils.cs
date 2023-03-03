using System.Collections.Generic;

namespace RaDataHolder
{
	public static class RaDataHolderUtils
	{
		public static void ResolveAll(this IList<IRaDataSetResolver> group)
		{
			for(int i = 0, c = group.Count; i < c; i++)
			{
				IRaDataSetResolver entry = group[i];
				if(entry != null)
				{
					entry.Resolve();
				}
			}
		}

		public static void ResolveAll(this IList<IRaDataClearResolver> group)
		{
			for(int i = group.Count - 1; i >= 0; i--)
			{
				IRaDataClearResolver entry = group[i];
				if(entry != null)
				{
					entry.Resolve();
				}
			}
		}
	}
}
