using System.Web.Mvc;
using System.Web.Routing;

namespace QuanLyKyTucXa
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { Controller = "DangNhap", action = "DangNhap", id = UrlParameter.Optional },
            namespaces: new string[] { "QuanLyKyTucXa.Areas.DangNhap.Controllers" }
        ).DataTokens.Add("area", "DangNhap");
        }
    }
}
