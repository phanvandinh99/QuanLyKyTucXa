using QuanLyKyTucXa.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyKyTucXa.Areas.QLKTX.Controllers
{
    public class LoaiHoaDonController : Controller
    {
        private readonly QLKyTucXa _db;

        public LoaiHoaDonController(QLKyTucXa db)
        {
            _db = db;
        }

        // Trang danh sách LoaiHoaDon
        public async Task<ActionResult> Index()
        {
            try
            {
                List<LoaiHoaDon> listLoaiHoaDon = await _db.LoaiHoaDon.ToListAsync();

                return View(listLoaiHoaDon);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Xem danh sách loại hóa đơn thất bại.";
                return RedirectToAction("Index", "LoaiHoaDon");
            }
        }

        #region Thêm mới loại hóa đơn
        public async Task<ActionResult> ThemMoi()
        {
            try
            {
                ViewBag.LoaiHoaDon = await _db.LoaiHoaDon.ToListAsync();

                return View();
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Thêm loại hóa đơn thất bại.";
                return RedirectToAction("Index", "LoaiHoaDon");
            }
        }

        [HttpPost]
        public async Task<ActionResult> ThemMoi(LoaiHoaDon LoaiHoaDonModel)
        {
            try
            {
                _db.LoaiHoaDon.Add(LoaiHoaDonModel);
                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Thêm loại hóa đơn thành công.";
                return RedirectToAction("Index", "LoaiHoaDon");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Thêm loại hóa đơn thất bại.";
                return RedirectToAction("Index", "LoaiHoaDon");
            }
        }
        #endregion

        #region Cập nhật LoaiHoaDon
        public async Task<ActionResult> CapNhat(int iMaLoaiHoaDon)
        {
            try
            {
                LoaiHoaDon loaiHoaDon = await _db.LoaiHoaDon.FindAsync(iMaLoaiHoaDon);
                ViewBag.LoaiHoaDon = await _db.LoaiHoaDon.ToListAsync();

                return View(loaiHoaDon);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Cập nhật loại hóa đơn thất bại.";
                return RedirectToAction("Index", "LoaiHoaDon");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CapNhat(LoaiHoaDon LoaiHoaDonModel)
        {
            try
            {
                LoaiHoaDon LoaiHoaDon = await _db.LoaiHoaDon.FindAsync(LoaiHoaDonModel.MaLoaiHoaDon);
                if (LoaiHoaDon == null)
                {
                    TempData["ToastMessage"] = "error|Không tồn tại loại hóa đơn.";
                    return RedirectToAction("Index", "LoaiHoaDon");
                }
                LoaiHoaDon.TenLoaiHoaDon = LoaiHoaDonModel.TenLoaiHoaDon;
                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Cập nhật loại hóa đơn thành công.";
                return RedirectToAction("Index", "LoaiHoaDon");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Cập nhật loại hóa đơn thất bại.";
                return RedirectToAction("Index", "LoaiHoaDon");
            }
        }
        #endregion

        #region Xoa loại hóa đơn
        public async Task<ActionResult> Xoa(int iMaLoaiHoaDon)
        {
            try
            {
                LoaiHoaDon loaiHoaDon = await _db.LoaiHoaDon.FindAsync(iMaLoaiHoaDon);
                if (loaiHoaDon == null)
                {
                    TempData["ToastMessage"] = "error|Không tồn tại loại hóa đơn.";
                    return RedirectToAction("Index", "LoaiHoaDon");
                }

                _db.LoaiHoaDon.Remove(loaiHoaDon);
                await _db.SaveChangesAsync();
                TempData["ToastMessage"] = "success|Xóa loại hóa đơn thành công.";

                return RedirectToAction("Index", "LoaiHoaDon");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Lỗi khóa ngoại.";
                return RedirectToAction("Index", "LoaiHoaDon");
            }
        }
        #endregion
    }
}