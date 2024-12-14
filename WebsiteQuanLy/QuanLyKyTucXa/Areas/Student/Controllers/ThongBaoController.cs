using QuanLyKyTucXa.Common.Const;
using QuanLyKyTucXa.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyKyTucXa.Areas.Student.Controllers
{
    public class ThongBaoController : Controller
    {
        private readonly QLKyTucXa _db;

        public ThongBaoController(QLKyTucXa db)
        {
            _db = db;
        }

        // Trang thông báo sinh viên
        public async Task<ActionResult> Index()
        {
            // Kiểm tra cookie đăng nhập
            var cookie = Request.Cookies["TKSinhVien"];
            if (cookie == null)
            {
                TempData["ToastMessage"] = "error|Bạn cần đăng nhập để xem phòng thuê.";
                return RedirectToAction("Index", "Home");
            }
            string maSinhVien = cookie.Values["MaSinhVien"];

            // Dữ liệu tìm kiếm
            ViewBag.listLoaiKhu = await _db.LoaiKhu.ToListAsync();
            ViewBag.listKhu = await _db.Khu.ToListAsync();
            ViewBag.listTang = await _db.Tang.ToListAsync();
            ViewBag.listLoaiPhong = await _db.LoaiPhong.ToListAsync();
            ViewBag.listThoiHanDangKy = await _db.ThoiHanDangKy.Where(n => n.NgayMo <= DateTime.Now &&
                                                                      n.NgayDong >= DateTime.Now)
                                                               .ToListAsync();

            List<ThongBao> listThongBao = await _db.ThongBao.Where(n => n.MaSinhVien == maSinhVien).ToListAsync();

            return View(listThongBao);
        }


        public async Task<ActionResult> XemChiTiet(int iMaThongBao)
        {
            try
            {
                ThongBao thongBao = await _db.ThongBao.FindAsync(iMaThongBao);
                if (thongBao.NgayXem == null)
                {
                    thongBao.NgayXem = DateTime.Now;
                    await _db.SaveChangesAsync();
                }

                return View(thongBao);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Xem chi tiết thất bại.";
                return RedirectToAction("Index", "ThongBao");
            }
        }
    }
}