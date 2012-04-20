using System.Collections.Generic;

namespace MvcPaging
{
	public class PaginationModel
	{
		public IList<PaginationLink> PaginationLinks { get; private set; }

		public PaginationModel()
		{
			PaginationLinks = new List<PaginationLink>();
		}
	}


	public class PaginationLink
	{
		public bool Active { get; set; }

		public bool IsCurrent { get; set; }

		public int? PageIndex { get; set; }

		public string DisplayText { get; set; }

		public string Url { get; set; }
	}
}
