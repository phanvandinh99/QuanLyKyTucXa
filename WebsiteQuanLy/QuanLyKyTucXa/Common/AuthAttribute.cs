using System;
using System.Web.Mvc;

namespace QuanLyKyTucXa.Common
{
    public class AuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var requestUrl = filterContext.HttpContext.Request.Url.AbsolutePath;

            // Bỏ qua các trang không yêu cầu xác thực
            if (requestUrl.Equals("/DangNhap/DangNhap/Login", StringComparison.OrdinalIgnoreCase))
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            // Xác định Area từ URL
            var isAdminArea = requestUrl.StartsWith("/Admin", StringComparison.OrdinalIgnoreCase);
            var isQLKTXArea = requestUrl.StartsWith("/QLKTX", StringComparison.OrdinalIgnoreCase);

            // Kiểm tra quyền truy cập cho Admin
            if (isAdminArea)
            {
                var adminCookie = filterContext.HttpContext.Request.Cookies["NhanVienAdmin"];
                if (adminCookie == null || string.IsNullOrEmpty(adminCookie["TaiKhoanNV"]))
                {
                    RedirectToLogin(filterContext);
                    return;
                }
            }
            // Kiểm tra quyền truy cập cho QLKTX
            else if (isQLKTXArea)
            {
                var bqlCookie = filterContext.HttpContext.Request.Cookies["NhanVienBQL"];
                if (bqlCookie == null || string.IsNullOrEmpty(bqlCookie["TaiKhoanNV"]))
                {
                    RedirectToLogin(filterContext);
                    return;
                }
            }
            // Trường hợp không xác định Area
            else
            {
                RedirectToLogin(filterContext);
                return;
            }

            base.OnActionExecuting(filterContext);
        }

        private void RedirectToLogin(ActionExecutingContext filterContext)
        {
            // Lưu URL hiện tại để chuyển hướng sau khi đăng nhập thành công
            var returnUrl = filterContext.HttpContext.Request.Url.AbsoluteUri;
            filterContext.HttpContext.Session["ReturnUrl"] = returnUrl;

            // Chuyển hướng về trang đăng nhập
            filterContext.Result = new RedirectResult("/DangNhap/DangNhap/Login");
        }
    }
}
