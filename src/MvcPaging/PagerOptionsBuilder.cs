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

		public PagerOptionsBuilder(PagerOptions pagerOptions, HtmlHelper<TModel> htmlHelper) : base(pagerOptions)
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
	}
}
