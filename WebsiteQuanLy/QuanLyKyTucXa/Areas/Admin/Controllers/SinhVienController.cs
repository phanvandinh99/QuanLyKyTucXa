using QuanLyKyTucXa.Common.Const;
using QuanLyKyTucXa.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace QuanLyKyTucXa.Areas.Admin.Controllers
{
    public class SinhVienController : Controller
    {
        private readonly QLKyTucXa _db;

        public SinhVienController(QLKyTucXa db)
        {
            _db = db;
        }

        // Danh sách sinh viên
        public ActionResult Index()
        {

            return View();
        }

        // Danh sách sinh viên cần xác thực tài khoản
        public async Task<ActionResult> XacThucSinhVien()
        {
            try
            {
                List<SinhVien> listSinhVien = await _db.SinhVien.Where(n => n.TrangThai == Constant.CanXacThucTaiKhoan)
                                                                .ToListAsync();

                return View(listSinhVien);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Tìm kiếm nâng cao thất bại.";
                return RedirectToAction("Index", "SinhVien");
            }
        }

        // Xóa sinh viên khỏi hệ thống
        public async Task<ActionResult> XoaSinhVien(String sMaSinhVien)
        {
            try
            {
                SinhVien sinhVien = await _db.SinhVien.FindAsync(sMaSinhVien);
                if (null == sinhVien)
                {
                    TempData["ToastMessage"] = "error|Không tìm thấy sinh viên.";
                    return RedirectToAction("XacThucSinhVien", "SinhVien");

                }
                //_db.SinhVien.Remove(sinhVien);
                //await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Xóa sinh viên thành công.";

                return RedirectToAction("XacThucSinhVien", "SinhVien");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Không thể xóa sinh viên.";
                return RedirectToAction("Index", "SinhVien");
            }
        }

    }
}