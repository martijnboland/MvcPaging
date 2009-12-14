using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcPaging
{
	public class Pager
	{
		private ViewContext viewContext;
		private readonly int pageSize;
		private readonly int currentPage;
		private readonly int totalItemCount;
		private readonly RouteValueDictionary linkWithoutPageValuesDictionary;

		public Pager(ViewContext viewContext, int pageSize, int currentPage, int totalItemCount, RouteValueDictionary valuesDictionary)
		{
			this.viewContext = viewContext;
			this.pageSize = pageSize;
			this.currentPage = currentPage;
			this.totalItemCount = totalItemCount;
			this.linkWithoutPageValuesDictionary = valuesDictionary;
		}

		public string RenderHtml()
		{
			int pageCount = (int)Math.Ceiling(this.totalItemCount / (double)this.pageSize);
			int nrOfPagesToDisplay = 10;

			var sb = new StringBuilder();

			// Previous
			if (this.currentPage > 1)
			{
				sb.Append(GeneratePageLink("&lt;", this.currentPage - 1));
			}
			else
			{
				sb.Append("<span class=\"disabled\">&lt;</span>");
			}

			int start = 1;
			int end = pageCount;

			if (pageCount > nrOfPagesToDisplay)
			{
				int middle = (int)Math.Ceiling(nrOfPagesToDisplay / 2d) - 1;
				int below = (this.currentPage - middle);
				int above = (this.currentPage + middle);

				if (below < 4)
				{
					above = nrOfPagesToDisplay;
					below = 1;
				}
				else if (above > (pageCount - 4))
				{
					above = pageCount;
					below = (pageCount - nrOfPagesToDisplay);
				}

				start = below;
				end = above;
			}

			if (start > 3)
			{
				sb.Append(GeneratePageLink("1", 1));
				sb.Append(GeneratePageLink("2", 2));
				sb.Append("...");
			}
			for (int i = start; i <= end; i++)
			{
				if (i == this.currentPage)
				{
					sb.AppendFormat("<span class=\"current\">{0}</span>", i);
				}
				else
				{
					sb.Append(GeneratePageLink(i.ToString(), i));
				}
			}
			if (end < (pageCount - 3))
			{
				sb.Append("...");
				sb.Append(GeneratePageLink((pageCount - 1).ToString(), pageCount - 1));
				sb.Append(GeneratePageLink(pageCount.ToString(), pageCount));
			}

			// Next
			if (this.currentPage < pageCount)
			{
				sb.Append(GeneratePageLink("&gt;", (this.currentPage + 1)));
			}
			else
			{
				sb.Append("<span class=\"disabled\">&gt;</span>");
			}
			return sb.ToString();
		}

		private string GeneratePageLink(string linkText, int pageNumber)
		{
			var pageLinkValueDictionary = new RouteValueDictionary(this.linkWithoutPageValuesDictionary);
			pageLinkValueDictionary.Add("page", pageNumber);
			//var virtualPathData = this.viewContext.RouteData.Route.GetVirtualPath(this.viewContext, pageLinkValueDictionary);
			var virtualPathData = RouteTable.Routes.GetVirtualPath(this.viewContext.RequestContext, pageLinkValueDictionary);

			if (virtualPathData != null)
			{
				string linkFormat = "<a href=\"{0}\">{1}</a>";
				return String.Format(linkFormat, virtualPathData.VirtualPath, linkText);
			}
			else
			{
				return null;
			}
		}
	}
}