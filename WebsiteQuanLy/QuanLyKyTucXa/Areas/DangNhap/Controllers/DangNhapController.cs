using QuanLyKyTucXa.Models;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System;
using System.Web.Mvc;
using QuanLyKyTucXa.Common.Const;

namespace QuanLyKyTucXa.Areas.DangNhap.Controllers
{
    public class DangNhapController : Controller
    {
        private readonly QLKyTucXa _db;

        public DangNhapController(QLKyTucXa db)
        {
            _db = db;
        }

        // GET: DangNhap/DangNhap
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string sTaiKhoan, string sMatKhau)
        {
            try
            {
                if (string.IsNullOrEmpty(sTaiKhoan) || string.IsNullOrEmpty(sMatKhau))
                {
                    ModelState.AddModelError("", "Vui lòng nhập tài khoản & mật khẩu !");
                }
                else
                {
                    var nhanVien = await _db.NhanVien.SingleOrDefaultAsync(n => n.TaiKhoanNV == sTaiKhoan &&
                                                                           n.MatKhau == sMatKhau);
                    if (nhanVien == null)
                    {
                        TempData["ToastMessage"] = "error|Tài khoản không đúng.";
                        return View();
                    }
                    else if (nhanVien.TrangThai == Constant.NhanVienHoatDong)
                    {
                        TempData["ToastMessage"] = "error|Tài khoản đã bị khóa.";
                        return View();
                    }
                    else
                    {
                        if (nhanVien.MaQuyen == Constant.Admin)
                        {
                            // Tạo cookie lưu thông tin đăng nhập Admin
                            var cookie = new HttpCookie("NhanVienAdmin")
                            {
                                Values = {
                                            ["TaiKhoanNV"] = nhanVien.TaiKhoanNV,
                                            ["Ho"] = HttpUtility.UrlEncode( nhanVien.Ho, Encoding.UTF8),
                                            ["Ten"] = HttpUtility.UrlEncode( nhanVien.Ten, Encoding.UTF8),
                                            ["MaQuyen"] = nhanVien.MaQuyen.ToString(),
                                            ["AnhChanDung"] = nhanVien.AnhChanDung.ToString(),
                                            ["ExpiresAt"] = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss")
                                },
                                Expires = DateTime.Now.AddDays(1)
                            };
                            Response.Cookies.Add(cookie);

                            return Redirect("/Admin/Home/Index");
                        }
                        else if (nhanVien.MaQuyen == Constant.BanQuanLy)
                        {
                            // Tạo cookie lưu thông tin đăng nhập BQL
                            var cookie = new HttpCookie("NhanVienBQL")
                            {
                                Values = {
                                            ["TaiKhoanNV"] = nhanVien.TaiKhoanNV,
                                            ["Ho"] = HttpUtility.UrlEncode( nhanVien.Ho, Encoding.UTF8),
                                            ["Ten"] = HttpUtility.UrlEncode( nhanVien.Ten, Encoding.UTF8),
                                            ["MaQuyen"] = nhanVien.MaQuyen.ToString(),
                                            ["AnhChanDung"] = nhanVien.AnhChanDung.ToString(),
                                            ["ExpiresAt"] = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss")
                                },
                                Expires = DateTime.Now.AddDays(1)
                            };
                            Response.Cookies.Add(cookie);

                            return Redirect("/QLKTX/Home/Index");
                        }
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = "error|Đã xảy ra lỗi trong quá trình dăng nhập.";
                return View();
            }
        }

        public ActionResult DangXuatAdmin()
        {
            // Xóa cookie đăng nhập
            var cookie = Request.Cookies["NhanVienAdmin"];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-2);
                cookie.HttpOnly = true;
                cookie.Secure = Request.IsSecureConnection;
                Response.Cookies.Add(cookie);
            }
            return Redirect("/DangNhap/DangNhap/Login");
        }

        public ActionResult DangXuatBQL()
        {
            // Xóa cookie đăng nhập
            var cookie = Request.Cookies["NhanVienBQL"];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-2);
                cookie.HttpOnly = true;
                cookie.Secure = Request.IsSecureConnection;
                Response.Cookies.Add(cookie);
            }
            return Redirect("/DangNhap/DangNhap/Login");
        }
    }
}