using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace MvcPaging
{
    public static class PagingExtensions
    {
        #region AjaxHelper extensions

        public static HtmlString Pager(this AjaxHelper ajaxHelper, int pageSize, int currentPage, int totalItemCount, AjaxOptions ajaxOptions)
        {
            return Pager(ajaxHelper, pageSize, currentPage, totalItemCount, null, null, ajaxOptions);
        }

        public static HtmlString Pager(this AjaxHelper ajaxHelper, int pageSize, int currentPage, int totalItemCount, string actionName, AjaxOptions ajaxOptions)
        {
            return Pager(ajaxHelper, pageSize, currentPage, totalItemCount, actionName, null, ajaxOptions);
        }

        public static HtmlString Pager(this AjaxHelper ajaxHelper, int pageSize, int currentPage, int totalItemCount, object values, AjaxOptions ajaxOptions)
        {
            return Pager(ajaxHelper, pageSize, currentPage, totalItemCount, null, new RouteValueDictionary(values), ajaxOptions);
        }

        public static HtmlString Pager(this AjaxHelper ajaxHelper, int pageSize, int currentPage, int totalItemCount, string actionName, object values, AjaxOptions ajaxOptions)
        {
            return Pager(ajaxHelper, pageSize, currentPage, totalItemCount, actionName, new RouteValueDictionary(values), ajaxOptions);
        }

        public static HtmlString Pager(this AjaxHelper ajaxHelper, int pageSize, int currentPage, int totalItemCount, RouteValueDictionary valuesDictionary, AjaxOptions ajaxOptions)
        {
            return Pager(ajaxHelper, pageSize, currentPage, totalItemCount, null, valuesDictionary, ajaxOptions);
        }

        public static HtmlString Pager(this AjaxHelper ajaxHelper, int pageSize, int currentPage, int totalItemCount, string actionName, RouteValueDictionary valuesDictionary, AjaxOptions ajaxOptions)
        {
            if (valuesDictionary == null)
            {
                valuesDictionary = new RouteValueDictionary();
            }
            if (actionName != null)
            {
                if (valuesDictionary.ContainsKey("action"))
                {
                    throw new ArgumentException("The valuesDictionary already contains an action.", "actionName");
                }
                valuesDictionary.Add("action", actionName);
            }
            var pager = new Pager(ajaxHelper.ViewContext, pageSize, currentPage, totalItemCount, valuesDictionary, ajaxOptions);
            return pager.RenderHtml();
        }

        #endregion

        #region HtmlHelper extensions

        public static HtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount)
        {
            return Pager(htmlHelper, pageSize, currentPage, totalItemCount, null, null);
        }

        public static HtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, string actionName)
        {
            return Pager(htmlHelper, pageSize, currentPage, totalItemCount, actionName, null);
        }

        public static HtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, object values)
        {
            return Pager(htmlHelper, pageSize, currentPage, totalItemCount, null, new RouteValueDictionary(values));
        }

        public static HtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, string actionName, object values)
        {
            return Pager(htmlHelper, pageSize, currentPage, totalItemCount, actionName, new RouteValueDictionary(values));
        }

        public static HtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, RouteValueDictionary valuesDictionary)
        {
            return Pager(htmlHelper, pageSize, currentPage, totalItemCount, null, valuesDictionary);
        }

        public static HtmlString Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, string actionName, RouteValueDictionary valuesDictionary)
        {
            if (valuesDictionary == null)
            {
                valuesDictionary = new RouteValueDictionary();
            }
            if (actionName != null)
            {
                if (valuesDictionary.ContainsKey("action"))
                {
                    throw new ArgumentException("The valuesDictionary already contains an action.", "actionName");
                }
                valuesDictionary.Add("action", actionName);
            }
            var pager = new Pager(htmlHelper.ViewContext, pageSize, currentPage, totalItemCount, valuesDictionary, null);
            return pager.RenderHtml();
        }

        #endregion

        #region IQueryable<T> extensions

        public static IPagedList<T> ToPagedList<T>(this IQueryable<T> source, int pageIndex, int pageSize, int? totalCount = null)
        {
            return new PagedList<T>(source, pageIndex, pageSize, totalCount);
        }

        #endregion

        #region IEnumerable<T> extensions

        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int pageIndex, int pageSize, int? totalCount = null)
        {
            return new PagedList<T>(source, pageIndex, pageSize, totalCount);
        }

        #endregion
    }
}