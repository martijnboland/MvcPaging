using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace MvcPaging
{
	public class PagerOptions
	{
		const int DefaultMaxNrOfPages = 10;
        const string DefaultPageRouteValueKey = "page";

		public RouteValueDictionary RouteValues { get; internal set; }
		public string DisplayTemplate { get; internal set; }
		public int MaxNrOfPages { get; internal set; }
		public AjaxOptions AjaxOptions { get; internal set; }
		public bool AlwaysAddFirstPageNumber { get; internal set; }
		public string PageRouteValueKey { get; set; }

		public PagerOptions()
		{
			this.RouteValues = new RouteValueDictionary();
			MaxNrOfPages = DefaultMaxNrOfPages;
            PageRouteValueKey = DefaultPageRouteValueKey;
		}
	}
}
