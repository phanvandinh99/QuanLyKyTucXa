using QuanLyKyTucXa.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyKyTucXa.Areas.QLKTX.Controllers
{
    public class DonGiaController : Controller
    {
        private readonly QLKyTucXa _db;

        public DonGiaController(QLKyTucXa db)
        {
            _db = db;
        }

        // Trang danh sách DonGia
        public async Task<ActionResult> Index()
        {
            try
            {
                List<DonGia> listDonGia = await _db.DonGia.ToListAsync();

                return View(listDonGia);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Xem danh sách đơn giá thất bại.";
                return RedirectToAction("Index", "DonGia");
            }
        }

        #region Thêm mới đơn giá
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

                TempData["ToastMessage"] = "error|Thêm đơn giá thất bại.";
                return RedirectToAction("Index", "DonGia");
            }
        }

        [HttpPost]
        public async Task<ActionResult> ThemMoi(DonGia donGiaModel)
        {
            try
            {
                donGiaModel.NgayBatDau = DateTime.Now;
                donGiaModel.NgayKetThuc = null;
                _db.DonGia.Add(donGiaModel);
                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Thêm đơn giá thành công.";
                return RedirectToAction("Index", "DonGia");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Thêm đơn giá thất bại.";
                return RedirectToAction("Index", "DonGia");
            }
        }
        #endregion

        #region Cập nhật DonGia
        public async Task<ActionResult> CapNhat(int iMaDonGia)
        {
            try
            {
                DonGia donGia = await _db.DonGia.FindAsync(iMaDonGia);
                ViewBag.LoaiHoaDon = await _db.LoaiHoaDon.ToListAsync();

                return View(donGia);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Cập nhật đơn giá thất bại.";
                return RedirectToAction("Index", "DonGia");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CapNhat(DonGia donGiaModel)
        {
            try
            {
                DonGia donGia = await _db.DonGia.FindAsync(donGiaModel.MaDonGia);
                if (donGia == null)
                {
                    TempData["ToastMessage"] = "error|Không tồn tại đơn giá.";
                    return RedirectToAction("Index", "DonGia");
                }
                donGia.NgayKetThuc = null;
                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Cập nhật đơn giá thành công.";
                return RedirectToAction("Index", "DonGia");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Cập nhật đơn giá thất bại.";
                return RedirectToAction("Index", "DonGia");
            }
        }
        #endregion

        #region xóa đơn giá
        public async Task<ActionResult> Xoa(int iMaDonGia)
        {
            try
            {
                DonGia DonGia = await _db.DonGia.FindAsync(iMaDonGia);
                if (DonGia == null)
                {
                    TempData["ToastMessage"] = "error|Không tồn tại đơn giá.";
                    return RedirectToAction("Index", "DonGia");
                }

                _db.DonGia.Remove(DonGia);
                await _db.SaveChangesAsync();
                TempData["ToastMessage"] = "success|Xóa đơn giá thành công.";

                return RedirectToAction("Index", "DonGia");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Lỗi khóa ngoại.";
                return RedirectToAction("Index", "DonGia");
            }
        }
        #endregion
    }
}