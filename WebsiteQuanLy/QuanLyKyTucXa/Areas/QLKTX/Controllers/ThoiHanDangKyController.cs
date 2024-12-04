using QuanLyKyTucXa.Common.Const;
using QuanLyKyTucXa.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyKyTucXa.Areas.QLKTX.Controllers
{
    public class ThoiHanDangKyController : Controller
    {
        private readonly QLKyTucXa _db;

        public ThoiHanDangKyController(QLKyTucXa db)
        {
            _db = db;
        }

        // Trang danh sách thời hạn đăng ký
        public async Task<ActionResult> Index()
        {
            try
            {
                List<ThoiHanDangKy> listThoiHanDangKy = await _db.ThoiHanDangKy.OrderByDescending(n=>n.MaThoiHanDangKy).ToListAsync();

                return View(listThoiHanDangKy);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Xem danh sách thời hạn đăng ký thất bại.";
                return RedirectToAction("Index", "ThoiHanDangKy");
            }
        }

        #region Thêm mới thời hạn đăng ký
        public async Task<ActionResult> ThemMoi()
        {
            try
            {
                ViewBag.ThoiHanDangKy = await _db.ThoiHanDangKy.ToListAsync();

                return View();
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Thêm thời hạn đăng ký thất bại.";
                return RedirectToAction("Index", "ThoiHanDangKy");
            }
        }

        [HttpPost]
        public async Task<ActionResult> ThemMoi(ThoiHanDangKy thoiHanDangKyModel)
        {
            try
            {
                thoiHanDangKyModel.TrangThai = Constant.ApDungThoiHanDangKy;
                _db.ThoiHanDangKy.Add(thoiHanDangKyModel);
                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Thêm thời hạn đăng ký thành công.";
                return RedirectToAction("Index", "ThoiHanDangKy");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Thêm thời hạn đăng ký thất bại.";
                return RedirectToAction("Index", "ThoiHanDangKy");
            }
        }
        #endregion

        #region Cập nhật thời hạn đăng ký
        public async Task<ActionResult> CapNhat(int iMaThoiHanDangKy)
        {
            try
            {
                ThoiHanDangKy thoiHanDangKy = await _db.ThoiHanDangKy.FindAsync(iMaThoiHanDangKy);

                return View(thoiHanDangKy);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Cập nhật thời hạn đăng ký thất bại.";
                return RedirectToAction("Index", "ThoiHanDangKy");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CapNhat(ThoiHanDangKy thoiHanDangKyModel)
        {
            try
            {
                ThoiHanDangKy thoiHanDangKy = await _db.ThoiHanDangKy.FindAsync(thoiHanDangKyModel.MaThoiHanDangKy);
                if (thoiHanDangKy == null)
                {
                    TempData["ToastMessage"] = "error|Không tồn tại thời hạn đăng ký.";
                    return RedirectToAction("Index", "ThoiHanDangKy");
                }

                thoiHanDangKy.TenThoiHanDangKy = thoiHanDangKyModel.TenThoiHanDangKy;
                thoiHanDangKy.NgayMo = thoiHanDangKyModel.NgayMo;
                thoiHanDangKy.NgayDong = thoiHanDangKyModel.NgayDong;
                thoiHanDangKy.NgayBatDau = thoiHanDangKyModel.NgayBatDau;
                thoiHanDangKy.NgayKetThuc = thoiHanDangKyModel.NgayKetThuc;
                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Cập nhật thời hạn đăng ký thành công.";
                return RedirectToAction("Index", "ThoiHanDangKy");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Cập nhật thời hạn đăng ký thất bại.";
                return RedirectToAction("Index", "ThoiHanDangKy");
            }
        }
        #endregion

        #region xóa thời hạn đăng ký
        public async Task<ActionResult> Xoa(int iMaThoiHanDangKy)
        {
            try
            {
                ThoiHanDangKy thoiHanDangKy = await _db.ThoiHanDangKy.FindAsync(iMaThoiHanDangKy);
                if (thoiHanDangKy == null)
                {
                    TempData["ToastMessage"] = "error|Không tồn tại thời hạn đăng ký.";
                    return RedirectToAction("Index", "ThoiHanDangKy");
                }

                _db.ThoiHanDangKy.Remove(thoiHanDangKy);
                await _db.SaveChangesAsync();
                TempData["ToastMessage"] = "success|Xóa thời hạn đăng ký thành công.";

                return RedirectToAction("Index", "ThoiHanDangKy");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Lỗi khóa ngoại.";
                return RedirectToAction("Index", "ThoiHanDangKy");
            }
        }
        #endregion
    }
}