using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LCTMoodle
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "MacDinh",
                url: "{controller}/{action}/{ma}",
                defaults: new { controller = "TrangChu", action = "Index", ma = UrlParameter.Optional }
            );
        }
    }
}
