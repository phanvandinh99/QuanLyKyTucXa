using QuanLyKyTucXa.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyKyTucXa.Areas.QLKTX.Controllers
{
    public class PhongController : Controller
    {
        private readonly QLKyTucXa _db;

        public PhongController(QLKyTucXa db)
        {
            _db = db;
        }

        // Danh sách Phòng Hiện Tại
        public async Task<ActionResult> Index()
        {
            try
            {
                List<Phong> listPhong = await _db.Phong.ToListAsync();

                return View(listPhong);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Xem danh sách phòng thất bại.";
                return RedirectToAction("Index", "Phong");
            }
        }

        // Xem danh sách sinh viên đang ở
        public async Task<ActionResult> XemChiTiet(int iMaPhong)
        {
            try
            {
                List<Giuong> listGiuong = await _db.Giuong.Where(n => n.MaPhong == iMaPhong).ToListAsync();

                ViewBag.HopDing = await _db.HopDong.ToListAsync();

                return View(listGiuong);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Chi tiết thất bại.";
                return RedirectToAction("Index", "Phong");
            }
        }
    }
}