using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace MvcPaging
{
    /// <summary>
    /// Pager options builder class. Enables a fluent interface for adding options to the pager.
    /// </summary>
    public class PagerOptionsBuilder
    {
        protected PagerOptions pagerOptions;

        public PagerOptionsBuilder(PagerOptions pagerOptions)
        {
            this.pagerOptions = pagerOptions;
        }

        /// <summary>
        /// Set the action name for the pager links. Note that we're always using the current controller.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public PagerOptionsBuilder Action(string action)
        {
            if (action != null)
            {
                if (pagerOptions.RouteValues.ContainsKey("action"))
                {
                    throw new ArgumentException("The valuesDictionary already contains an action.", "action");
                }
                pagerOptions.RouteValues.Add("action", action);
                pagerOptions.Action = action;
            }
            return this;
        }

        public PagerOptionsBuilder Action(string action, string controller)
        {
            if (action != null && controller != null)
            {
                if (pagerOptions.RouteValues.ContainsKey("action"))
                {
                    throw new ArgumentException("The valuesDictionary already contains an action.", "action");
                }

                if (pagerOptions.RouteValues.ContainsKey("controller"))
                {
                    throw new ArgumentException("The valuesDictionary already contains an controller.", "controller");
                }

                pagerOptions.RouteValues.Add("action", action);
                pagerOptions.RouteValues.Add("controller", controller);

                pagerOptions.Action = action;
                pagerOptions.Controller = controller;
            }
            return this;
        }

        /// <summary>
        /// Add a custom route value parameter for the pager links.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public PagerOptionsBuilder AddRouteValue(string name, object value)
        {
            pagerOptions.RouteValues[name] = value;
            return this;
        }

        /// <summary>
        /// Set the text for previous page navigation.
        /// </summary>
        /// <param name="previousPageText"></param>
        /// <returns></returns>
        public PagerOptionsBuilder SetPreviousPageText(string previousPageText)
        {
            pagerOptions.PreviousPageText = previousPageText;
            return this;
        }

        /// <summary>
        /// Set the title for previous page navigation.
        /// </summary>
        /// <param name="previousPageTitle"></param>
        /// <returns></returns>
        public PagerOptionsBuilder SetPreviousPageTitle(string previousPageTitle)
        {
            pagerOptions.PreviousPageTitle = previousPageTitle;
            return this;
        }

        /// <summary>
        /// Set the text for next page navigation.
        /// </summary>
        /// <param name="nextPageText"></param>
        /// <returns></returns>
        public PagerOptionsBuilder SetNextPageText(string nextPageText)
        {
            pagerOptions.NextPageText = nextPageText;
            return this;
        }

        /// <summary>
        /// Set the title for next page navigation.
        /// </summary>
        /// <param name="nextPageTitle"></param>
        /// <returns></returns>
        public PagerOptionsBuilder SetNextPageTitle(string nextPageTitle)
        {
            pagerOptions.NextPageTitle = nextPageTitle;
            return this;
        }

        /// <summary>
        /// Set the text for first page navigation.
        /// </summary>
        /// <param name="firstPageText"></param>
        /// <returns></returns>
        public PagerOptionsBuilder SetFirstPageText(string firstPageText)
        {
            pagerOptions.FirstPageText = firstPageText;
            return this;
        }

        /// <summary>
        /// Set the title for first page navigation.
        /// </summary>
        /// <param name="firstPageTitle"></param>
        /// <returns></returns>
        public PagerOptionsBuilder SetFirstPageTitle(string firstPageTitle)
        {
            pagerOptions.FirstPageTitle = firstPageTitle;
            return this;
        }

        /// <summary>
        /// Set the text for last page navigation.
        /// </summary>
        /// <param name="lastPageText"></param>
        /// <returns></returns>
        public PagerOptionsBuilder SetLastPageText(string lastPageText)
        {
            pagerOptions.LastPageText = lastPageText;
            return this;
        }

        /// <summary>
        /// Set the title for last page navigation.
        /// </summary>
        /// <param name="lastPageTitle"></param>
        /// <returns></returns>
        public PagerOptionsBuilder SetLastPageTitle(string lastPageTitle)
        {
            pagerOptions.LastPageTitle = lastPageTitle;
            return this;
        }

        /// <summary>
        /// Displays both first and last navigation pages.
        /// </summary>
        /// <returns></returns>
        public PagerOptionsBuilder DisplayFirstAndLastPage()
        {
            pagerOptions.DisplayFirstPage = true;
            pagerOptions.DisplayLastPage = true;
            return this;
        }

        /// <summary>
        /// Displays the first navigation page but not the last.
        /// </summary>
        /// <returns></returns>
        public PagerOptionsBuilder DisplayFirstPage()
        {
            pagerOptions.DisplayFirstPage = true;
            return this;
        }

        /// <summary>
        /// Displays the last navigation page but not the first.
        /// </summary>
        /// <returns></returns>
        public PagerOptionsBuilder DisplayLastPage()
        {
            pagerOptions.DisplayLastPage = true;
            return this;
        }

        public PagerOptionsBuilder HidePreviousAndNextPage()
        {
            pagerOptions.HidePreviousAndNextPage = true;
            return this;
        }

		/// <summary>
		/// Set custom route value parameters for the pager links.
		/// </summary>
		/// <param name="routeValues"></param>
		/// <returns></returns>
		public PagerOptionsBuilder RouteValues(object routeValues)
		{
			RouteValues(new RouteValueDictionary(routeValues));
			return this;
		}

        /// <summary>
        /// Set custom route value parameters for the pager links.
        /// </summary>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public PagerOptionsBuilder RouteValues(RouteValueDictionary routeValues)
        {
            if (routeValues == null)
            {
                throw new ArgumentException("routeValues may not be null", "routeValues");
            }

            this.pagerOptions.RouteValues = routeValues;

            if (!string.IsNullOrWhiteSpace(pagerOptions.Action) && !pagerOptions.RouteValues.ContainsKey("action"))
            {
                pagerOptions.RouteValues.Add("action", pagerOptions.Action);
            }

            if (!string.IsNullOrEmpty(pagerOptions.Controller) && !pagerOptions.RouteValues.ContainsKey("controller"))
            {
                pagerOptions.RouteValues.Add("controller", pagerOptions.Controller);
            }

            return this;
        }

        /// <summary>
        /// Set the name of the DisplayTemplate view to use for rendering.
        /// </summary>
        /// <param name="displayTemplate"></param>
        /// <remarks>The view must have a model of IEnumerable&lt;PaginationModel&gt;</remarks>
        /// <returns></returns>
        public PagerOptionsBuilder DisplayTemplate(string displayTemplate)
        {
            this.pagerOptions.DisplayTemplate = displayTemplate;
            return this;
        }

        /// <summary>
        /// Set the maximum number of pages to show. The default is 10.
        /// </summary>
        /// <param name="maxNrOfPages"></param>
        /// <returns></returns>
        public PagerOptionsBuilder MaxNrOfPages(int maxNrOfPages)
        {
            this.pagerOptions.MaxNrOfPages = maxNrOfPages;
            return this;
        }

        /// <summary>
        /// Always add the page number to the generated link for the first page.
        /// </summary>
        /// <remarks>
        /// By default we don't add the page number for page 1 because it results in canonical links.
        /// Use this option to override this behaviour.
        /// </remarks>
        /// <returns></returns>
        public PagerOptionsBuilder AlwaysAddFirstPageNumber()
        {
            this.pagerOptions.AlwaysAddFirstPageNumber = true;
            return this;
        }

        /// <summary>
        /// Set the page routeValue key for pagination links
        /// </summary>
        /// <param name="pageRouteValueKey"></param>
        /// <returns></returns>
        public PagerOptionsBuilder PageRouteValueKey(string pageRouteValueKey)
        {
            if (pageRouteValueKey == null)
            {
                throw new ArgumentException("pageRouteValueKey may not be null", "pageRouteValueKey");
            }
            this.pagerOptions.PageRouteValueKey = pageRouteValueKey;
            return this;
        }

        /// <summary>
        /// Indicate that the total item count means total page count. This option is for scenario's 
        /// where certain backends don't return the number of total items, but the number of pages.
        /// </summary>
        /// <returns></returns>
        public PagerOptionsBuilder UseItemCountAsPageCount()
        {
            this.pagerOptions.UseItemCountAsPageCount = true;
            return this;
        }

        /// <summary>
        /// Use an explicit route instead of letting ASP.NET MVC figuring out the correct route.
        /// </summary>
        /// <param name="customRouteName"></param>
        /// <returns></returns>
        public PagerOptionsBuilder CustomRouteName(string customRouteName)
        {
            if (customRouteName == null)
            {
                throw new ArgumentException("customRouteName may not be null", "customRouteName");
            }
            this.pagerOptions.CustomRouteName = customRouteName;
            return this;
        }

        /// <summary>
        /// Set the AjaxOptions.
        /// </summary>
        /// <param name="ajaxOptions"></param>
        /// <returns></returns>
        internal PagerOptionsBuilder AjaxOptions(AjaxOptions ajaxOptions)
        {
            this.pagerOptions.AjaxOptions = ajaxOptions;
            return this;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class PagerOptionsBuilder<TModel> : PagerOptionsBuilder
    {
        private readonly HtmlHelper<TModel> htmlHelper;

        public PagerOptionsBuilder(PagerOptions pagerOptions, HtmlHelper<TModel> htmlHelper)
            : base(pagerOptions)
        {
            this.htmlHelper = htmlHelper;
        }

        /// <summary>
        /// Adds a strongly typed route value parameter based on the current model.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <example>AddRouteValueFor(m => m.SearchQuery)</example>
        /// <returns></returns>
        public PagerOptionsBuilder<TModel> AddRouteValueFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            AddRouteValue(name, metadata.Model);

            return this;
        }

        /// <summary>
        /// Set the action name for the pager links. Note that we're always using the current controller.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public new PagerOptionsBuilder<TModel> Action(string action)
        {
            base.Action(action);
            return this;
        }

        /// <summary>
        /// Set the action name and controller name for the pager links.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        public new PagerOptionsBuilder<TModel> Action(string action, string controller)
        {
            base.Action(action, controller);
            return this;
        }

        /// <summary>
        /// Add a custom route value parameter for the pager links.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public new PagerOptionsBuilder<TModel> AddRouteValue(string name, object value)
        {
            base.AddRouteValue(name, value);
            return this;
        }

        /// <summary>
        /// Set custom route value parameters for the pager links.
        /// </summary>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public new PagerOptionsBuilder<TModel> RouteValues(object routeValues)
        {
            base.RouteValues(routeValues);
            return this;
        }

        /// <summary>
        /// Set custom route value parameters for the pager links.
        /// </summary>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public new PagerOptionsBuilder<TModel> RouteValues(RouteValueDictionary routeValues)
        {
            base.RouteValues(routeValues);
            return this;
        }

        /// <summary>
        /// Set the name of the DisplayTemplate view to use for rendering.
        /// </summary>
        /// <param name="displayTemplate"></param>
        /// <remarks>The view must have a model of IEnumerable&lt;PaginationModel&gt;</remarks>
        /// <returns></returns>
        public new PagerOptionsBuilder<TModel> DisplayTemplate(string displayTemplate)
        {
            base.DisplayTemplate(displayTemplate);
            return this;
        }

        /// <summary>
        /// Set the maximum number of pages to show. The default is 10.
        /// </summary>
        /// <param name="maxNrOfPages"></param>
        /// <returns></returns>
        public new PagerOptionsBuilder<TModel> MaxNrOfPages(int maxNrOfPages)
        {
            base.MaxNrOfPages(maxNrOfPages);
            return this;
        }

        /// <summary>
        /// Always add the page number to the generated link for the first page.
        /// </summary>
        /// <remarks>
        /// By default we don't add the page number for page 1 because it results in canonical links.
        /// Use this option to override this behaviour.
        /// </remarks>
        /// <returns></returns>
        public new PagerOptionsBuilder<TModel> AlwaysAddFirstPageNumber()
        {
            base.AlwaysAddFirstPageNumber();
            return this;
        }

        /// <summary>
        /// Set the page routeValue key for pagination links
        /// </summary>
        /// <param name="pageRouteValueKey"></param>
        /// <returns></returns>
        public new PagerOptionsBuilder<TModel> PageRouteValueKey(string pageRouteValueKey)
        {
            if (pageRouteValueKey == null)
            {
                throw new ArgumentException("pageRouteValueKey may not be null", "pageRouteValueKey");
            }
            this.pagerOptions.PageRouteValueKey = pageRouteValueKey;
            return this;
        }

        /// <summary>
        /// Set the text for previous page navigation.
        /// </summary>
        /// <param name="previousPageText"></param>
        /// <returns></returns>
        public new PagerOptionsBuilder<TModel> SetPreviousPageText(string previousPageText)
        {
            base.SetPreviousPageText(previousPageText);
            return this;
        }

        /// <summary>
        /// Set the title for previous page navigation.
        /// </summary>
        /// <param name="previousPageTitle"></param>
        /// <returns></returns>
        public new PagerOptionsBuilder<TModel> SetPreviousPageTitle(string previousPageTitle)
        {
            base.SetPreviousPageTitle(previousPageTitle);
            return this;
        }

        /// <summary>
        /// Set the text for next page navigation.
        /// </summary>
        /// <param name="nextPageText"></param>
        /// <returns></returns>
        public new PagerOptionsBuilder<TModel> SetNextPageText(string nextPageText)
        {
            base.SetNextPageText(nextPageText);
            return this;
        }

        /// <summary>
        /// Set the title for next page navigation.
        /// </summary>
        /// <param name="nextPageTitle"></param>
        /// <returns></returns>
        public new PagerOptionsBuilder<TModel> SetNextPageTitle(string nextPageTitle)
        {
            base.SetNextPageTitle(nextPageTitle);
            return this;
        }

        /// <summary>
        /// Set the text for first page navigation.
        /// </summary>
        /// <param name="firstPageText"></param>
        /// <returns></returns>
        public new PagerOptionsBuilder<TModel> SetFirstPageText(string firstPageText)
        {
            base.SetFirstPageText(firstPageText);
            return this;
        }

        /// <summary>
        /// Set the title for first page navigation.
        /// </summary>
        /// <param name="firstPageTitle"></param>
        /// <returns></returns>
        public new PagerOptionsBuilder<TModel> SetFirstPageTitle(string firstPageTitle)
        {
            base.SetFirstPageTitle(firstPageTitle);
            return this;
        }

        /// <summary>
        /// Set the text for last page navigation.
        /// </summary>
        /// <param name="lastPageText"></param>
        /// <returns></returns>
        public new PagerOptionsBuilder<TModel> SetLastPageText(string lastPageText)
        {
            base.SetLastPageText(lastPageText);
            return this;
        }

        /// <summary>
        /// Set the title for last page navigation.
        /// </summary>
        /// <param name="lastPageTitle"></param>
        /// <returns></returns>
        public new PagerOptionsBuilder<TModel> SetLastPageTitle(string lastPageTitle)
        {
            base.SetLastPageTitle(lastPageTitle);
            return this;
        }

        /// <summary>
        /// Displays first and last navigation pages.
        /// </summary>
        /// <returns></returns>
        public new PagerOptionsBuilder<TModel> DisplayFirstAndLastPage()
        {
            base.DisplayFirstAndLastPage();
            return this;
        }

        /// <summary>
        /// Indicate that the total item count means total page count. This option is for scenario's 
        /// where certain backends don't return the number of total items, but the number of pages.
        /// </summary>
        /// <returns></returns>
        public new PagerOptionsBuilder<TModel> UseItemCountAsPageCount()
        {
            base.UseItemCountAsPageCount();
            return this;
        }

        /// <summary>
        /// Use an explicit route instead of letting ASP.NET MVC figuring out the correct route.
        /// </summary>
        /// <param name="customRouteName"></param>
        /// <returns></returns>
        public new PagerOptionsBuilder<TModel> CustomRouteName(string customRouteName)
        {
            base.CustomRouteName(customRouteName);
            return this;
        }
    }
}