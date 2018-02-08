The MvcPaging library contains an ASP.NET MVC HTML helper that renders a pager based on given parameters.
A live demo can be found at http://demo.taiga.nl/mvcpaging.

### Important information for ASP.NET 5 / MVC 6 users

This library doesn't support ASP.NET 5. Go to https://github.com/joeaudette/cloudscribe.Web.Pagination for the logical successor of this library. Thanks Joe for the effort!

### Usage (Razor / pseudo code): 
	
	@Html.Pager(pageSize, pageNumber, totalItemCount)

Options are added via the Options method:

	@Html.Pager(pageSize, pageNumber, totalItemCount).Options(o => o
		.Action("action")
		.AddRouteValue("q", mySearchQuery)
	)

Possible options:

	Action(string action)
		Sets an alternative action for the pager that is different from the current action

	Action(string action, string controller)
		Sets an alternative action and controller for the pager that is different from the current

	AddRouteValue(string name, object value)
		Adds a single route value parameter that is added to page url's

	AddRouteValueFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
		Adds a strongly typed route value parameter based on the current model
		(e.g. AddRouteValueFor(m => m.SearchQuery))

	RouteValues(object routeValues)
		Adds route value parameters that are added to the page url's

	RouteValues(RouteValueDictionary routeValues)
		Adds route value parameters that are added to the page url's

	DisplayTemplate(string displayTemplate)
		When set, the internal HTML rendering is bypassed and a DisplayTemplate view with the given
		name is rendered instead. Note that the DisplayTemplate must have a model of type PaginationModel

	MaxNrOfPages(int maxNrOfPages)
		Sets the maximum number of pages to show	
	
	AlwaysAddFirstPageNumber
		By default we don't add the page number for page 1 because it results in canonical links. 
		Use this option to override this behaviour.	

	PageRouteValueKey
		Set the page routeValue key for pagination links

	DisplayFirstAndLastPage
		Displays first and last navigation pages

	DisplayFirstPage
		Displays the first navigation page

	DisplayLastPage
		Displays the last navigation page

	SetFirstPageText
		Set a custom text for the first page

	SetFirstPageTitle
		Set a custom text for title attribute of the first page link

	SetLastPageText
		Set a custom text for the last page

	SetLastPageTitle
		Set a custom text for title attribute of the last page link
		
	SetPreviousPageText
		Set a custom text for the previous page

	SetPreviousPageTitle
		Set a custom text for title attribute of the previous page link

	SetNextPageText
		Set a custom text for the next page

	SetNextPageTitle
		Set a custom text for title attribute of the next page link
		
	UseItemCountAsPageCount
		The totalItemCount parameter is (ab)used for the total number of pages
		instead of the total number of items to facilitate backends that return the total number
		of pages instead of the total number of items

	HidePreviousAndNextPage
		Don't show the 'previous' and 'next' links and only show the page numbers

	CustomRouteName
		Indicate that a specific named route must be used when generating page links

### PagedList

The library contains a PagedList class that makes it easy to work with paged data. Use it via an 
extension method on IEnumerable<> or IList<>:

	myList.ToPagedList(pageIndex, pageSize)

with any unpaged list or 

	myList.ToPagedList(pageIndex, pageSize, totalItemCount)

when the list already only contains the data for the page

### Contributing

Contributions via pull requests are great. We use 4 spaces for indentation :-).
