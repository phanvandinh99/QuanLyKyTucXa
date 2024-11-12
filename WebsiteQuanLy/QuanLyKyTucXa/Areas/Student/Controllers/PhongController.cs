using QuanLyKyTucXa.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyKyTucXa.Areas.Student.Controllers
{
    public class PhongController : Controller
    {
        private readonly QLKyTucXa _db;

        public PhongController(QLKyTucXa db)
        {
            _db = db;
        }

        // Tra cứu phòng
        public async Task<ActionResult> TraCuuPhong(
           int? MaLoaiKhu,                                      // Table LoaiKhu
           int? MaKhu,                                          // Table Khu
           int? MaTang,                                         // Table Tang
           int? MaLoaiPhong                                     // Table LoaiPhong
        )
        {
            try
            {
                // Tìm kiếm phòng với điều kiện tham số có giá trị
                var phong = await _db.Phong
                    .Include(n => n.DichVuPhong)
                    .Where(n => (!MaLoaiKhu.HasValue || n.Tang.Khu.LoaiKhu.MaLoaiKhu == MaLoaiKhu) &&
                                (!MaKhu.HasValue || n.Tang.Khu.MaKhu == MaKhu) &&
                                (!MaTang.HasValue || n.Tang.MaTang == MaTang) &&
                                (!MaLoaiPhong.HasValue || n.LoaiPhong.MaLoaiPhong == MaLoaiPhong))
                    .ToListAsync();

                // Dữ liệu tìm kiếm
                ViewBag.listLoaiKhu = await _db.LoaiKhu.ToListAsync();
                ViewBag.listKhu = await _db.Khu.ToListAsync();
                ViewBag.listTang = await _db.Tang.ToListAsync();
                ViewBag.listLoaiPhong = await _db.LoaiPhong.ToListAsync();

                // Lấy tên của các tiêu chí tìm kiếm
                var tenLoaiKhu = MaLoaiKhu.HasValue ?
                                 await _db.LoaiKhu.Where(x => x.MaLoaiKhu == MaLoaiKhu)
                                                  .Select(x => x.TenLoaiKhu)
                                                  .FirstOrDefaultAsync()
                                 : "Tất cả";

                var tenKhu = MaKhu.HasValue ?
                             await _db.Khu.Where(x => x.MaKhu == MaKhu)
                                          .Select(x => x.TenKhu)
                                          .FirstOrDefaultAsync()
                            : "Tất cả";

                var tenTang = MaTang.HasValue ?
                              await _db.Tang.Where(x => x.MaTang == MaTang)
                                            .Select(x => x.TenTang)
                                            .FirstOrDefaultAsync()
                              : "Tất cả";

                var tenLoaiPhong = MaLoaiPhong.HasValue ?
                                   await _db.LoaiPhong.Where(x => x.MaLoaiPhong == MaLoaiPhong)
                                                      .Select(x => x.TenLoaiPhong)
                                                      .FirstOrDefaultAsync()
                                   : "Tất cả";

                // Tên tiêu chí đã chọn
                ViewBag.TenLoaiKhu = tenLoaiKhu;
                ViewBag.TenKhu = tenKhu;
                ViewBag.TenTang = tenTang;
                ViewBag.TenLoaiPhong = tenLoaiPhong;
                ViewBag.KetQua = phong.Count();

                TempData["ToastMessage"] = "success|Tìm kiếm thành công.";

                return View(phong);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = "error|Tìm kiếm thất bại.";
                return RedirectToAction("DanhSachBan", "Ban");
            }
        }

        public async Task<JsonResult> GetKhuByLoaiKhu(int maLoaiKhu)
        {
            var listKhu = await _db.Khu
                                   .Where(k => k.MaLoaiKhu == maLoaiKhu)
                                   .Select(k => new
                                   {
                                       k.MaKhu,
                                       k.TenKhu
                                   })
                                   .ToListAsync();
            return Json(listKhu, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetTangByKhu(int maKhu)
        {
            var listTang = await _db.Tang
                                    .Where(t => t.MaKhu == maKhu)
                                    .Select(t => new
                                    {
                                        t.MaTang,
                                        t.TenTang
                                    })
                                    .ToListAsync();
            return Json(listTang, JsonRequestBehavior.AllowGet);
        }

        // Xem chi tiết phòng
        public async Task<ActionResult> ChiTietPhong(int iMaPhong)
        {
            Phong phong = await _db.Phong.FindAsync(iMaPhong);

            // Dữ liệu tìm kiếm
            ViewBag.listLoaiKhu = await _db.LoaiKhu.ToListAsync();
            ViewBag.listKhu = await _db.Khu.ToListAsync();
            ViewBag.listTang = await _db.Tang.ToListAsync();
            ViewBag.listLoaiPhong = await _db.LoaiPhong.ToListAsync();

            return View(phong);
        }

    }
}