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
                    var nhanVien = await _db.NhanVien
                                            .SingleOrDefaultAsync(n => n.TaiKhoanNV == sTaiKhoan &&
                                                                  n.MatKhau == sMatKhau);
                    if (nhanVien == null)
                    {
                        TempData["ToastMessage"] = "error|Tài khoản hoặc mật khẩu không đúng.";
                        return View();
                    }
                    else
                    {
                        // Tạo cookie lưu thông tin đăng nhập
                        var cookie = new HttpCookie("NhanVien")
                        {
                            Values = {
                            ["TaiKhoanNV"] = nhanVien.TaiKhoanNV,
                            ["HoNhanVien"] = HttpUtility.UrlEncode( nhanVien.Ho, Encoding.UTF8),
                            ["TenNhanVien"] = HttpUtility.UrlEncode( nhanVien.Ten, Encoding.UTF8),
                            ["MaQuyen"] = nhanVien.MaQuyen.ToString(),
                        },
                            Expires = DateTime.Now.AddDays(1)
                        };
                        Response.Cookies.Add(cookie);

                        // Kiểm tra có cần thay đổi mật khẩu hay không
                        if (nhanVien.DoiMatKhau == Constant.DoiMatKhau)
                        {
                            return RedirectToAction("DoiMatKhau", "DangNhap", new { @sTaiKhoan = sTaiKhoan });
                        }

                        // Chuyển hướng tới URL ban đầu hoặc trang chính
                        string returnUrl = Session["ReturnUrl"] as string;
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            Session["ReturnUrl"] = null;
                            return Redirect(returnUrl);
                        }
                        return Redirect("/Admin/Home/Index");
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
    }
}