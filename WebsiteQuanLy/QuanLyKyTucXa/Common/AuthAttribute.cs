using System;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKyTucXa.Common
{
    public class AuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var requestUrl = filterContext.HttpContext.Request.Url.AbsolutePath;

            // Bỏ qua các trang không yêu cầu xác thực (trang đăng nhập)
            if (requestUrl.Equals("/DangNhap/DangNhap/Login", StringComparison.OrdinalIgnoreCase))
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            // Xác định Area từ URL
            var isAdminArea = requestUrl.StartsWith("/Admin", StringComparison.OrdinalIgnoreCase);
            var isQLKTXArea = requestUrl.StartsWith("/QLKTX", StringComparison.OrdinalIgnoreCase);
            var isStudentArea = requestUrl.StartsWith("/Student", StringComparison.OrdinalIgnoreCase);

            // Kiểm tra quyền truy cập cho Student (không xử lý ở đây)
            if (isStudentArea)
            {
                return;
            }

            // Kiểm tra quyền truy cập cho Admin
            // Kiểm tra quyền truy cập cho Admin
            if (isAdminArea)
            {
                var adminCookie = filterContext.HttpContext.Request.Cookies["NhanVienAdmin"];
                if (adminCookie == null || string.IsNullOrEmpty(adminCookie["TaiKhoanNV"]))
                {
                    RedirectToLogin(filterContext);
                    return;
                }
                else
                {
                    // Kiểm tra thời gian hết hạn của token
                    var expiresAtString = adminCookie["ExpiresAt"];
                    DateTime expiresAt;
                    if (DateTime.TryParse(expiresAtString, out expiresAt))
                    {
                        if (expiresAt < DateTime.Now)
                        {
                            RedirectToLogin(filterContext);
                            return;
                        }
                    }
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
                else
                {
                    var expiresAtString = bqlCookie["ExpiresAt"];
                    DateTime expiresAt;
                    if (DateTime.TryParse(expiresAtString, out expiresAt))
                    {
                        if (expiresAt < DateTime.Now)
                        {
                            RedirectToLogin(filterContext);
                            return;
                        }
                    }
                }
            }
            else
            {
                RedirectToLogin(filterContext);
                return;
            }

            base.OnActionExecuting(filterContext);
        }

        private void RedirectToLogin(ActionExecutingContext filterContext)
        {
            System.Diagnostics.Debug.WriteLine("[Auth] Chuyển hướng về trang đăng nhập");

            //// Chỉ xóa cookie của quyền hiện tại nếu hết hạn hoặc không hợp lệ
            //var adminCookie = filterContext.HttpContext.Request.Cookies["NhanVienAdmin"];
            //if (adminCookie != null)
            //{
            //    adminCookie.Expires = DateTime.Now.AddDays(-1);
            //    filterContext.HttpContext.Response.Cookies.Add(adminCookie);
            //}

            //var bqlCookie = filterContext.HttpContext.Request.Cookies["NhanVienBQL"];
            //if (bqlCookie != null)
            //{
            //    bqlCookie.Expires = DateTime.Now.AddDays(-1);
            //    filterContext.HttpContext.Response.Cookies.Add(bqlCookie);
            //}

            // Lưu URL hiện tại để chuyển hướng sau khi đăng nhập thành công
            var returnUrl = filterContext.HttpContext.Request.Url.AbsoluteUri;
            filterContext.HttpContext.Session["ReturnUrl"] = returnUrl;

            // Chuyển hướng về trang đăng nhập
            filterContext.Result = new RedirectResult("/DangNhap/DangNhap/Login");
        }

    }
}
