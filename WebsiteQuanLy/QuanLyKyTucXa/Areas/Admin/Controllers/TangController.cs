using QuanLyKyTucXa.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyKyTucXa.Areas.Admin.Controllers
{
    public class TangController : Controller
    {
        private readonly QLKyTucXa _db;

        public TangController(QLKyTucXa db)
        {
            _db = db;
        }

        // Trang danh sách Tầng
        public async Task<ActionResult> Index()
        {
            try
            {
                List<Tang> listTang = await _db.Tang.OrderBy(n => n.MaKhu)
                                                    .ThenBy(n => n.MaTang)
                                                    .ToListAsync();

                return View(listTang);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Xem danh sách tàng thất bại.";
                return RedirectToAction("Index", "Tang");
            }
        }

        #region Thêm mới Tầng
        public async Task<ActionResult> ThemMoi()
        {
            try
            {
                ViewBag.Khu = await _db.Khu.ToListAsync();

                return View();
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Thêm tầng thất bại.";
                return RedirectToAction("Index", "Tang");
            }
        }

        [HttpPost]
        public async Task<ActionResult> ThemMoi(Tang TangModel)
        {
            try
            {
                _db.Tang.Add(TangModel);
                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Thêm tầng thành công.";
                return RedirectToAction("Index", "Tang");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Thêm tầng thất bại.";
                return RedirectToAction("Index", "Tang");
            }
        }
        #endregion

        #region Cập nhật Tầng
        public async Task<ActionResult> CapNhat(int iMaTang)
        {
            try
            {
                Tang tang = await _db.Tang.FindAsync(iMaTang);
                ViewBag.Khu = await _db.Khu.ToListAsync();

                return View(tang);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Cập nhật tầng thất bại.";
                return RedirectToAction("Index", "Tang");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CapNhat(Tang tangModel)
        {
            try
            {
                Tang tang = await _db.Tang.FindAsync(tangModel.MaKhu);
                if (tang == null)
                {
                    TempData["ToastMessage"] = "error|Không tồn tại tầng.";
                    return RedirectToAction("Index", "Tang");
                }
                tang.TenTang = tangModel.TenTang;
                tang.MaKhu = tangModel.MaKhu;
                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Cập nhật tầng thành công.";
                return RedirectToAction("Index", "Tang");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Cập nhật tầng thất bại.";
                return RedirectToAction("Index", "Tang");
            }
        }
        #endregion

        #region Xóa khu
        public async Task<ActionResult> Xoa(int iMaTang)
        {
            try
            {
                Tang tang = await _db.Tang.FindAsync(iMaTang);
                if (tang == null)
                {
                    TempData["ToastMessage"] = "error|Không tồn tại tầng.";
                    return RedirectToAction("Index", "Tang");
                }

                _db.Tang.Remove(tang);
                await _db.SaveChangesAsync();
                TempData["ToastMessage"] = "success|Xóa tầng thành công.";

                return RedirectToAction("Index", "Tang");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Lỗi khóa ngoại.";
                return RedirectToAction("Index", "Tang");
            }
        }
        #endregion
    }
}