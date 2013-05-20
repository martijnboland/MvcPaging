using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcPaging.Demo.Models;

namespace MvcPaging.Demo.Controllers
{
	public class PagingController : Controller
	{
		private const int DefaultPageSize = 10;
		private IList<Product> allProducts = new List<Product>();
		private readonly string[] allCategories = new string[3] { "Shoes", "Electronics", "Food" };

		public PagingController()
		{
			InitializeProducts();
		}

		private void InitializeProducts()
		{
			// Create a list of products. 50% of them are in the Shoes category, 25% in the Electronics 
			// category and 25% in the Food category.
			for (var i = 0; i < 527; i++)
			{
				var product = new Product();
				product.Name = "Product " + (i + 1);
				var categoryIndex = i % 4;
				if (categoryIndex > 2)
				{
					categoryIndex = categoryIndex - 3;
				}
				product.Category = allCategories[categoryIndex];
				allProducts.Add(product);
			}
		}

		public ActionResult Index(int? page)
		{
			int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
			return View(this.allProducts.ToPagedList(currentPageIndex, DefaultPageSize));
		}

		public ActionResult CustomPageRouteValueKey(SearchModel search)
		{
			int currentPageIndex = search.page.HasValue ? search.page.Value - 1 : 0;
			return View(this.allProducts.ToPagedList(currentPageIndex, DefaultPageSize));
		}

		public ActionResult ViewByCategory(string categoryName, int? page)
		{
			categoryName = categoryName ?? this.allCategories[0];
			int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

			var productsByCategory = this.allProducts.Where(p => p.Category.Equals(categoryName)).ToPagedList(currentPageIndex,
																											  DefaultPageSize);
			ViewBag.CategoryName = new SelectList(this.allCategories, categoryName);
			ViewBag.CategoryDisplayName = categoryName;
			return View("ProductsByCategory", productsByCategory);
		}

		public ActionResult ViewByCategories(string[] categories, int? page)
		{
			categories = categories ?? new string[0];
			int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

			var productsByCategories = this.allProducts.Where(p => categories.Contains(p.Category)).ToPagedList(currentPageIndex,
																											  DefaultPageSize);
			ViewBag.AllCategories = this.allCategories;
			ViewBag.SelectedCategories = categories;
			return View("ProductsByCategories", productsByCategories);
		}

		public ActionResult IndexAjax()
		{
			int currentPageIndex = 0;
			var products = this.allProducts.ToPagedList(currentPageIndex, DefaultPageSize);
			return View(products);
		}

		public ActionResult AjaxPage(int? page)
		{
			ViewBag.Title = "Browse all products";
			int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
			var products = this.allProducts.ToPagedList(currentPageIndex, DefaultPageSize);
			return PartialView("_ProductGrid", products);
		}

		public ActionResult Bootstrap(int? page)
		{
			int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
			return View(this.allProducts.ToPagedList(currentPageIndex, DefaultPageSize));
		}
	}
}