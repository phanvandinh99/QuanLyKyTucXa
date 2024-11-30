using QuanLyKyTucXa.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKyTucXa.Areas.Student.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly QLKyTucXa _db;

        public TaiKhoanController(QLKyTucXa db)
        {
            _db = db;
        }

        // GET: Student/TaiKhoan
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DangNhap(string sMaSinhVien, string sMatKhau)
        {
            try
            {
                var sinhVien = await _db.SinhVien.SingleOrDefaultAsync(n => n.MaSinhVien == sMaSinhVien &&
                                                                            n.MatKhau == sMatKhau);

                if (sinhVien == null)
                {
                    TempData["ToastMessage"] = "error|Tài khoản không tồn tại.";
                    return RedirectToAction("Index", "Home");
                }
                else if (sinhVien.TrangThai == false)
                {
                    TempData["ToastMessage"] = "error|Tài khoản chưa được phê duyệt.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Tạo cookie lưu thông tin đăng nhập
                    var cookie = new HttpCookie("TKSinhVien")
                    {
                        Values = {
                            ["MaSinhVien"] = sinhVien.MaSinhVien,
                            ["Ho"] = HttpUtility.UrlEncode( sinhVien.Ho, Encoding.UTF8),
                            ["Ten"] = HttpUtility.UrlEncode( sinhVien.Ten, Encoding.UTF8),
                        },
                        Expires = DateTime.Now.AddDays(1)
                    };
                    Response.Cookies.Add(cookie);

                    TempData["ToastMessage"] = "success|Đăng nhập thành công.";
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = "error|Đăng nhập thất bại.";
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult DangXuat()
        {
            // Xóa cookie đăng nhập
            var cookie = Request.Cookies["TKSinhVien"];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}