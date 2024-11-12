using QuanLyKyTucXa.Models;
using System;
using System.Collections.Generic;
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
           int? MaLoaiPhong,                                    // Table LoaiPhong
           int? MaDichVu                                        // Table LoaiPhong
        )
        {
            try
            {
                // Tìm kiếm phòng với điều kiện tham số có giá trị
                var phong = await _db.Phong.Include(n => n.DichVuPhong)
                                           .Where(n => (!MaLoaiKhu.HasValue || n.Tang.Khu.LoaiKhu.MaLoaiKhu == MaLoaiKhu) &&
                                                       (!MaKhu.HasValue || n.Tang.Khu.MaKhu == MaKhu) &&
                                                       (!MaTang.HasValue || n.Tang.MaTang == MaTang) &&
                                                       (!MaLoaiPhong.HasValue || n.LoaiPhong.MaLoaiPhong == MaLoaiPhong))
                                           .ToListAsync();

                // Dữ liệu tìm kiếm
                ViewBag.listLoaiKhu = await _db.LoaiKhu.ToListAsync();
                ViewBag.listKhu = await _db.Khu.ToListAsync();
                ViewBag.listTang = await _db.Tang.ToListAsync();

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


                // Tên tiêu chí đã chọn
                ViewBag.TenLoaiKhu = tenLoaiKhu;
                ViewBag.TenKhu = tenKhu;
                ViewBag.TenTang = tenTang;
                ViewBag.KetQua = phong.Count();

                TempData["ToastMessage"] = "success|Tìm kiếm thành công.";

                return View(phong);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Tìm kiếm thất bại.";
                return RedirectToAction("Index", "Home");
            }
        }

        // Tìm kiếm phòng nâng cao
        public ActionResult _TraCuuPhongPartial()
        {
            try
            {
                // Sử dụng ToList thay vì ToListAsync để tránh dùng async/await
                ViewBag.listLoaiKhu = _db.LoaiKhu.ToList();
                ViewBag.listKhu = _db.Khu.ToList();
                ViewBag.listTang = _db.Tang.ToList();
                ViewBag.listLoaiPhong = _db.LoaiPhong.ToList();
                ViewBag.listDichVu = _db.DichVu.ToList();

                return PartialView("_TraCuuPhongPartial");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Tìm kiếm nâng cao thất bại.";
                return RedirectToAction("Index", "Home");
            }
        }

        // Tìm kiếm phòng nâng cao
        public async Task<ActionResult> TraCuuPhongNamgCao(
           int? MaLoaiKhu,                                      // Table LoaiKhu
           int? MaKhu,                                          // Table Khu
           int? MaTang,                                         // Table Tang
           List<int?> MaLoaiPhong,                              // Table LoaiPhong
           List<int?> MaDichVu                                  // Table DichVuPhong
        )
        {
            try
            {
                MaLoaiPhong = MaLoaiPhong ?? new List<int?>(); // Default to empty list if null
                MaDichVu = MaDichVu ?? new List<int?>();

                // Tìm kiếm phòng với điều kiện tham số có giá trị
                var phongQuery = _db.Phong.Include(n => n.DichVuPhong.Select(d => d.DichVu))
                                  .Where(n =>
                                      // Filter by LoaiKhu if provided
                                      (!MaLoaiKhu.HasValue || n.Tang.Khu.LoaiKhu.MaLoaiKhu == MaLoaiKhu) &&
                                      // Filter by Khu if provided
                                      (!MaKhu.HasValue || n.Tang.Khu.MaKhu == MaKhu) &&
                                      // Filter by Tang if provided
                                      (!MaTang.HasValue || n.Tang.MaTang == MaTang) &&
                                      // Filter by MaLoaiPhong if the list is provided and not empty
                                      (MaLoaiPhong.Count == 0 || MaLoaiPhong.Contains(n.LoaiPhong.MaLoaiPhong)) &&
                                      // Filter by MaDichVu if the list is provided and not empty
                                      (MaDichVu.Count == 0 || n.DichVuPhong.Any(dp => MaDichVu.Contains(dp.MaDichVu)))
                                  );

                // Execute the query and get the results
                var phong = await phongQuery.ToListAsync();


                // Dữ liệu tìm kiếm
                ViewBag.listLoaiKhu = await _db.LoaiKhu.ToListAsync();
                ViewBag.listKhu = await _db.Khu.ToListAsync();
                ViewBag.listTang = await _db.Tang.ToListAsync();

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


                // Tên tiêu chí đã chọn
                ViewBag.TenLoaiKhu = tenLoaiKhu;
                ViewBag.TenKhu = tenKhu;
                ViewBag.TenTang = tenTang;
                ViewBag.KetQua = phong.Count();

                TempData["ToastMessage"] = "success|Tìm kiếm thành công.";

                return View(phong);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Tìm kiếm thất bại.";
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<JsonResult> GetKhuByLoaiKhu(int maLoaiKhu)
        {
            try
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
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Lọc loại khu thất bại.";
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetTangByKhu(int maKhu)
        {
            try
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
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Lọc tầng thất bại.";
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        // Xem chi tiết phòng
        public async Task<ActionResult> ChiTietPhong(int iMaPhong)
        {
            try
            {
                Phong phong = await _db.Phong.FindAsync(iMaPhong);

                // Dữ liệu tìm kiếm
                ViewBag.listLoaiKhu = await _db.LoaiKhu.ToListAsync();
                ViewBag.listKhu = await _db.Khu.ToListAsync();
                ViewBag.listTang = await _db.Tang.ToListAsync();
                ViewBag.listLoaiPhong = await _db.LoaiPhong.ToListAsync();

                return View(phong);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Xem chi tiết thất bại.";
                return RedirectToAction("Index", "Home");
            }
        }

    }
}