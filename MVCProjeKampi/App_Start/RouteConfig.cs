using System.Security.Policy;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCProjeKampi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "AboutIndex",
                url: "site-hakkinda",
                defaults: new { controller = "About", action = "Index" }
            );

            routes.MapRoute(
                name: "LoginsLogin",
                url: "giris-yap",
                defaults: new { controller = "Logins", action = "Login" }
            );

            //routes.MapRoute(
            //    name: "HeadingByHeadingId",
            //    url: "baslik/{id}/{p}",
            //    defaults: new { controller = "Headings", action = "HeadingByHeadingId"}
            //);

            routes.MapRoute(
                name: "HeadingByHeadingNameForFriendlyUrl",
                url: "baslik/{headingNameForFriendlyUrl}/{p}",
                defaults: new { controller = "Headings", action = "HeadingByHeadingNameForFriendlyUrl", p = UrlParameter.Optional}
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Homepage", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
