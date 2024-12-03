using QuanLyKyTucXa.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyKyTucXa.Areas.Admin.Controllers
{
    public class khuController : Controller
    {
        private readonly QLKyTucXa _db;

        public khuController(QLKyTucXa db)
        {
            _db = db;
        }

        // Trang danh sách khu
        public async Task<ActionResult> Index()
        {
            try
            {
                List<Khu> listKhu = await _db.Khu.ToListAsync();

                return View(listKhu);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Xem danh sách khu thất bại.";
                return RedirectToAction("Index", "Khu");
            }
        }

        #region Thêm mới khu
        public async Task<ActionResult> ThemMoi()
        {
            try
            {
                ViewBag.LoaiKhu = await _db.LoaiKhu.ToListAsync();

                return View();
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Thêm khu thất bại.";
                return RedirectToAction("Index", "Khu");
            }
        }

        [HttpPost]
        public async Task<ActionResult> ThemMoi(Khu khuModel)
        {
            try
            {
                _db.Khu.Add(khuModel);
                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Thêm khu thành công.";
                return RedirectToAction("Index", "Khu");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Thêm khu thất bại.";
                return RedirectToAction("Index", "Khu");
            }
        }
        #endregion

        #region Cập nhật khu
        public async Task<ActionResult> CapNhat(int iMaKhu)
        {
            try
            {
                Khu khu = await _db.Khu.FindAsync(iMaKhu);
                ViewBag.LoaiKhu = await _db.LoaiKhu.ToListAsync();

                return View(khu);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Cập nhật khu thất bại.";
                return RedirectToAction("Index", "Khu");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CapNhat(Khu khuModel)
        {
            try
            {
                Khu khu = await _db.Khu.FindAsync(khuModel.MaKhu);
                if (khu == null)
                {
                    TempData["ToastMessage"] = "error|Không tồn tại khu.";
                    return RedirectToAction("Index", "Khu");
                }
                khu.TenKhu = khuModel.TenKhu;
                khu.MaLoaiKhu = khuModel.MaLoaiKhu;
                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Cập nhật khu thành công.";
                return RedirectToAction("Index", "Khu");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Cập nhật khu thất bại.";
                return RedirectToAction("Index", "Khu");
            }
        }
        #endregion

        #region Xoa khu
        public async Task<ActionResult> Xoa(int iMaKhu)
        {
            try
            {
                Khu khu = await _db.Khu.FindAsync(iMaKhu);
                if (khu == null)
                {
                    TempData["ToastMessage"] = "error|Không tồn tại khu.";
                    return RedirectToAction("Index", "Khu");
                }

                _db.Khu.Remove(khu);
                await _db.SaveChangesAsync();
                TempData["ToastMessage"] = "success|Xóa khu thành công.";

                return RedirectToAction("Index", "Khu");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Lỗi khóa ngoại.";
                return RedirectToAction("Index", "Khu");
            }
        }
        #endregion
    }
}