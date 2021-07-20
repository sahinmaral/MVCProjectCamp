using System.Web.Mvc;
using System.Web.Routing;

namespace MVCProjeKampi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "AdminHomepage",
            //    url: "AdminAnasayfa",
            //    defaults: new { controller = "AdminHomepage", action = "Index" }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Homepage", action = "Index", id = UrlParameter.Optional }
            );
            
        }
    }
}
