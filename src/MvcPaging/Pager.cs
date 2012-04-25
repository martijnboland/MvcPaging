﻿using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace MvcPaging
{
	public class Pager : IHtmlString
	{
		private readonly HtmlHelper htmlHelper;
		private readonly int pageSize;
		private readonly int currentPage;
		private readonly int totalItemCount;
		private readonly PagerOptions pagerOptions;

		public Pager(HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount)
		{
			this.htmlHelper = htmlHelper;
			this.pageSize = pageSize;
			this.currentPage = currentPage;
			this.totalItemCount = totalItemCount;
			this.pagerOptions = new PagerOptions();
		}

		public Pager Options(Action<PagerOptionsBuilder> buildOptions)
		{
			buildOptions(new PagerOptionsBuilder(this.pagerOptions));
			return this;
		}

		public PaginationModel BuildPaginationModel(Func<int, string> generateUrl)
		{
			var model = new PaginationModel();

			var pageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);

			// Previous
			model.PaginationLinks.Add(currentPage > 1 ? new PaginationLink { Active = true, DisplayText = "«", PageIndex = currentPage - 1, Url = generateUrl(currentPage - 1) } : new PaginationLink { Active = false, DisplayText = "«" });

			var start = 1;
			var end = pageCount;
			var nrOfPagesToDisplay = this.pagerOptions.MaxNrOfPages;

			if (pageCount > nrOfPagesToDisplay)
			{
				var middle = (int)Math.Ceiling(nrOfPagesToDisplay / 2d) - 1;
				var below = (currentPage - middle);
				var above = (currentPage + middle);

				if (below < 2)
				{
					above = nrOfPagesToDisplay;
					below = 1;
				}
				else if (above > (pageCount - 2))
				{
					above = pageCount;
					below = (pageCount - nrOfPagesToDisplay + 1);
				}

				start = below;
				end = above;
			}

			if (start > 1)
			{
				model.PaginationLinks.Add(new PaginationLink { Active = true, PageIndex = 1, DisplayText = "1", Url = generateUrl(1) });
				if (start > 3)
				{
					model.PaginationLinks.Add(new PaginationLink { Active = true, PageIndex = 2, DisplayText = "2", Url = generateUrl(2) });
				}
				if (start > 2)
				{
					model.PaginationLinks.Add(new PaginationLink { Active = true, DisplayText = "..." });
				}
			}

			for (var i = start; i <= end; i++)
			{
				if (i == currentPage || (currentPage <= 0 && i == 0))
				{
					model.PaginationLinks.Add(new PaginationLink { Active = true, PageIndex = i, IsCurrent = true, DisplayText = i.ToString() });
				}
				else
				{
					model.PaginationLinks.Add(new PaginationLink { Active = true, PageIndex = i, DisplayText = i.ToString(), Url = generateUrl(i) });
				}
			}
			if (end < pageCount)
			{
				if (end < pageCount - 1)
				{
					model.PaginationLinks.Add(new PaginationLink { Active = true, DisplayText = "..." });
				}
				if (pageCount - 2 > end)
				{
					model.PaginationLinks.Add(new PaginationLink { Active = true, PageIndex = pageCount - 1, DisplayText = (pageCount - 1).ToString(), Url = generateUrl(pageCount - 1) });
				}

				model.PaginationLinks.Add(new PaginationLink { Active = true, PageIndex = pageCount, DisplayText = pageCount.ToString(), Url = generateUrl(pageCount) });
			}

			// Next
			model.PaginationLinks.Add(currentPage < pageCount ? new PaginationLink { Active = true, PageIndex = currentPage + 1, DisplayText = "»", Url = generateUrl(currentPage + 1) } : new PaginationLink { Active = false, DisplayText = "»" });
			
			// AjaxOptions
		        if (pagerOptions.AjaxOptions != null)
	                {
	                	model.AjaxOptions = pagerOptions.AjaxOptions;
	                }

			return model;
		}

		public string ToHtmlString()
		{
			var model = BuildPaginationModel(GeneratePageUrl);

			if (! String.IsNullOrEmpty(this.pagerOptions.DisplayTemplate))
			{
				var templatePath = string.Format("DisplayTemplates/{0}", this.pagerOptions.DisplayTemplate);
				return htmlHelper.Partial(templatePath, model).ToHtmlString();
			}
			else
			{
				var sb = new StringBuilder();		
				
				foreach (var paginationLink in model.PaginationLinks)
				{
					if (paginationLink.Active)
					{
						if (paginationLink.IsCurrent)
						{
							sb.AppendFormat("<span class=\"current\">{0}</span>", paginationLink.DisplayText);
						}
						else if (!paginationLink.PageIndex.HasValue)
						{
							sb.AppendFormat(paginationLink.DisplayText);
						}
						else
						{
							var linkBuilder = new StringBuilder("<a");
							if (pagerOptions.AjaxOptions != null)
								foreach (var ajaxOption in pagerOptions.AjaxOptions.ToUnobtrusiveHtmlAttributes())
									linkBuilder.AppendFormat(" {0}=\"{1}\"", ajaxOption.Key, ajaxOption.Value);

							linkBuilder.AppendFormat(" href=\"{0}\">{1}</a>", paginationLink.Url, paginationLink.DisplayText);
							sb.Append(linkBuilder.ToString());
						}
					}
					else
					{
						sb.AppendFormat("<span class=\"disabled\">{0}</span>", paginationLink.DisplayText);
					}
				}
				return sb.ToString();
			}
		}

		private string GeneratePageUrl(int pageNumber)
		{
			var viewContext = this.htmlHelper.ViewContext;
			var routeDataValues = viewContext.RequestContext.RouteData.Values;
			RouteValueDictionary pageLinkValueDictionary;
			// Avoid canonical errors when page count is equal to 1.
			if (pageNumber == 1)
			{
				pageLinkValueDictionary = new RouteValueDictionary(this.pagerOptions.RouteValues);
				if (routeDataValues.ContainsKey("page"))
				{
					routeDataValues.Remove("page");
				}
			}
			else
			{
				pageLinkValueDictionary = new RouteValueDictionary(this.pagerOptions.RouteValues) { { "page", pageNumber } };
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

			return virtualPathForArea == null ? null : virtualPathForArea.VirtualPath;
		}
	}
}