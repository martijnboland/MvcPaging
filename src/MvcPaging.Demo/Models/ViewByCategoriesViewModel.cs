using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcPaging.Demo.Models
{
    public class ViewByCategoriesViewModel
    {
        public IPagedList<Product> Products { get; set; }
        public string[] AvailableCategories { get; set; }
        public string[] Categories { get; set; }
    }
}