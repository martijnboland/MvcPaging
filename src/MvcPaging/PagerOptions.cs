using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace MvcPaging
{
	public class PagerOptions
	{
        public static class DefaultDefaults
        {
            public const int MaxNrOfPages = 10;
            public const string DisplayTemplate = null;
            public const bool AlwaysAddFirstPageNumber = false;
        }

        public static class Defaults
        {
            public static int MaxNrOfPages = DefaultDefaults.MaxNrOfPages;
            public static string DisplayTemplate = DefaultDefaults.DisplayTemplate;
            public static bool AlwaysAddFirstPageNumber = DefaultDefaults.AlwaysAddFirstPageNumber;

            public static void ResetToDefaults()
            {
                MaxNrOfPages = DefaultDefaults.MaxNrOfPages;
                DisplayTemplate = DefaultDefaults.DisplayTemplate;
                AlwaysAddFirstPageNumber = DefaultDefaults.AlwaysAddFirstPageNumber;
            }
        }

        public RouteValueDictionary RouteValues { get; internal set; }
		public string DisplayTemplate { get; internal set; }
		public int MaxNrOfPages { get; internal set; }
		public AjaxOptions AjaxOptions { get; internal set; }
		public bool AlwaysAddFirstPageNumber { get; internal set; }
		public string Action { get; internal set; }

		public PagerOptions()
		{
			RouteValues = new RouteValueDictionary();
            DisplayTemplate = Defaults.DisplayTemplate;
            MaxNrOfPages = Defaults.MaxNrOfPages;
            AlwaysAddFirstPageNumber = Defaults.AlwaysAddFirstPageNumber;
		}
	}
}
