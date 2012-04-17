using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;
using System.Collections.Generic;
using System.Linq;

namespace MvcPaging
{
	public class Pager
	{
		private ViewContext viewContext;
		private readonly int pageSize;
		private readonly int currentPage;
		private readonly int totalItemCount;
		private readonly RouteValueDictionary linkWithoutPageValuesDictionary;
		private readonly AjaxOptions ajaxOptions;

		public Pager(ViewContext viewContext, int pageSize, int currentPage, int totalItemCount, RouteValueDictionary valuesDictionary, AjaxOptions ajaxOptions)
		{
			this.viewContext = viewContext;
			this.pageSize = pageSize;
			this.currentPage = currentPage;
			this.totalItemCount = totalItemCount;
			this.linkWithoutPageValuesDictionary = valuesDictionary;
			this.ajaxOptions = ajaxOptions;
		}

        public IList<PaginationModel> BuildPaginationModel()
        {
            var pages = new List<PaginationModel>();

            var pageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
            const int nrOfPagesToDisplay = 10;

            // Previous
            pages.Add(currentPage > 1 ? new PaginationModel { Active = true, DisplayText = "«", PageIndex = currentPage - 1 } : new PaginationModel { Active = false, DisplayText = "«" });

            var start = 1;
            var end = pageCount;

            if (pageCount > nrOfPagesToDisplay)
            {
                var middle = (int)Math.Ceiling(nrOfPagesToDisplay / 2d) - 1;
                var below = (currentPage - middle);
                var above = (currentPage + middle);

                if (below < 4)
                {
                    above = nrOfPagesToDisplay;
                    below = 1;
                }
                else if (above > (pageCount - 4))
                {
                    above = pageCount;
                    below = (pageCount - nrOfPagesToDisplay + 1);
                }

                start = below;
                end = above;
            }

            if (start > 1)
            {
                pages.Add(new PaginationModel { Active = true, PageIndex = 1, DisplayText = "1" });
                if (start > 3)
                {
                    pages.Add(new PaginationModel { Active = true, PageIndex = 2, DisplayText = "2" });
                }
                if (start > 2)
                {
                    pages.Add(new PaginationModel { Active = true, DisplayText = "..." });
                }
            }

            for (var i = start; i <= end; i++)
            {
                if (i == currentPage || (currentPage <= 0 && i == 0))
                {
                    pages.Add(new PaginationModel { Active = true, PageIndex = i, IsCurrent = true, DisplayText = i.ToString() });
                }
                else
                {
                    pages.Add(new PaginationModel { Active = true, PageIndex = i, DisplayText = i.ToString() });
                }
            }
            if (end < pageCount)
            {
                if (end < pageCount - 1)
                {
                    pages.Add(new PaginationModel { Active = true });
                }
                if (pageCount - 2 > end)
                {
                    pages.Add(new PaginationModel { Active = true, PageIndex = pageCount - 1, DisplayText = (pageCount - 1).ToString() });
                }

                pages.Add(new PaginationModel { Active = true, PageIndex = pageCount, DisplayText = pageCount.ToString() });
            }

            // Next
            pages.Add(currentPage < pageCount ? new PaginationModel { Active = true, PageIndex = currentPage + 1, DisplayText = "»" } : new PaginationModel { Active = false, DisplayText = "»" });

            return pages;
        }

		public HtmlString RenderHtml()
		{			
			var sb = new StringBuilder();

            var pages = BuildPaginationModel();

            foreach (var page in pages)
            {
                if (page.Active)
                {
                    if (page.IsCurrent)
                    {
                        sb.AppendFormat("<span class=\"current\">{0}</span>", page.DisplayText);
                    }
                    else if (!page.PageIndex.HasValue)
                    {
                        sb.AppendFormat(page.DisplayText);
                    }
                    else
                    {
                        sb.Append(GeneratePageLink(page.DisplayText, page.PageIndex.GetValueOrDefault()));
                    }
                }
                else
                {
                    sb.AppendFormat("<span class=\"disabled\">{0}</span>", page.DisplayText);
                }
            }

			
			return new HtmlString(sb.ToString());
		}

		private string GeneratePageLink(string linkText, int pageNumber)
		{
			var routeDataValues = viewContext.RequestContext.RouteData.Values;
			RouteValueDictionary pageLinkValueDictionary;
			// Avoid canonical errors when page count is equal to 1.
			if (pageNumber == 1)
			{
				pageLinkValueDictionary = new RouteValueDictionary(this.linkWithoutPageValuesDictionary);
				if (routeDataValues.ContainsKey("page"))
				{
					routeDataValues.Remove("page");
				}
			}
			else
			{
				pageLinkValueDictionary = new RouteValueDictionary(this.linkWithoutPageValuesDictionary) {{"page", pageNumber}};
			}

			// To be sure we get the right route, ensure the controller and action are specified.
			if (!pageLinkValueDictionary.ContainsKey("controller") && routeDataValues.ContainsKey("controller"))
			{
				pageLinkValueDictionary.Add("controller", routeDataValues["controller"]);
			}
			if (!pageLinkValueDictionary.ContainsKey("action") && routeDataValues.ContainsKey("action"))
			{
				pageLinkValueDictionary.Add("action", routeDataValues["action"]);
			}

			// 'Render' virtual path.
			var virtualPathForArea = RouteTable.Routes.GetVirtualPathForArea(viewContext.RequestContext, pageLinkValueDictionary);

			if (virtualPathForArea == null)
				return null;

			var stringBuilder = new StringBuilder("<a");

			if (ajaxOptions != null)
				foreach (var ajaxOption in ajaxOptions.ToUnobtrusiveHtmlAttributes())
					stringBuilder.AppendFormat(" {0}=\"{1}\"", ajaxOption.Key, ajaxOption.Value);

			stringBuilder.AppendFormat(" href=\"{0}\">{1}</a>", virtualPathForArea.VirtualPath, linkText);

			return stringBuilder.ToString();
		}
	}
}