using System.Collections.Generic;
using System.Web.Mvc.Ajax;

namespace MvcPaging
{
	public class PaginationModel
	{
		public IList<PaginationLink> PaginationLinks { get; private set; }
		public AjaxOptions AjaxOptions { get; internal set; }
		public PagerOptions Options { get; internal set; }

		public PaginationModel()
		{
			PaginationLinks = new List<PaginationLink>();
			AjaxOptions = null;
			Options = null;
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
