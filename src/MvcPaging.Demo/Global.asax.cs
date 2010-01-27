using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcPaging.Demo
{
	public class GlobalApplication : System.Web.HttpApplication
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			AreaRegistration.RegisterAllAreas();
			routes.MapRoute(
				"Default",                                              // Route name
				"{controller}/{action}/{id}",                           // URL with parameters
				new { controller = "Home", action = "Index", id = "" },  // Parameter defaults
				new[] { "MvcPaging.Demo.Controllers" } 
			);

		}

		protected void Application_Start()
		{
			RegisterRoutes(RouteTable.Routes);
		}
	}
}