using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging.Demo.Models;

namespace MvcPaging.Demo.Areas.Area51.Controllers
{
    public class PagingController : Controller
    {
        private const int defaultPageSize = 10;
        private IList<Product> allProducts = new List<Product>();

        public PagingController()
        {
            InitializeProducts();
        }

        private void InitializeProducts()
        {
            // Create a list of 500 products. 250 are in the Shoes category, 125 in the Electronics 
            // category and 125 in the Food category.
            for (int i = 0; i < 500; i++)
            {
                var product = new Product();
                product.Name = "Product " + (i + 1);
                product.Category = "All products";
                allProducts.Add(product);
            }
        }

        public ActionResult Index(int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            return View(this.allProducts.ToPagedList(currentPageIndex, defaultPageSize));
        }

    }
}
