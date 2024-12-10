using System.Web.Mvc;
using System.Web.Routing;

namespace QuanLyKyTucXa
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Route Đăng Nhập
            routes.MapRoute(
                name: "DangNhap",
                url: "DangNhap/{controller}/{action}/{id}",
                defaults: new { controller = "DangNhap", action = "DangNhap", id = UrlParameter.Optional },
                namespaces: new string[] { "QuanLyKyTucXa.Areas.DangNhap.Controllers" }
            ).DataTokens.Add("area", "DangNhap");

            // Route cho sinh viên
            routes.MapRoute(
                name: "Student",
                url: "Student/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "QuanLyKyTucXa.Areas.Student.Controllers" }
            ).DataTokens.Add("area", "Student");

            // Route cho khu vực QLKTX
            routes.MapRoute(
                name: "QLKTX",
                url: "QLKTX/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "QuanLyKyTucXa.Areas.QLKTX.Controllers" }
            ).DataTokens.Add("area", "QLKTX");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { Controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "QuanLyKyTucXa.Areas.Admin.Controllers" }
            ).DataTokens.Add("area", "Admin");
        }
    }
}
