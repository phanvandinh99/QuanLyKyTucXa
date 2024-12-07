using QuanLyKyTucXa.Common.Const;
using QuanLyKyTucXa.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKyTucXa.Areas.QLKTX.Controllers
{
    public class HopDongController : Controller
    {
        private readonly QLKyTucXa _db;

        public HopDongController(QLKyTucXa db)
        {
            _db = db;
        }

        // Danh sách hợp đồng thuê phòng
        public async Task<ActionResult> Index()
        {
            try
            {
                List<HopDong> listHopDong = await _db.HopDong.Where(n => n.TrangThai == Constant.DaDuyet)
                                                             .ToListAsync();

                return View(listHopDong);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Xem danh sách hợp đồng thất bại.";
                return RedirectToAction("Index", "HopDong");
            }
        }

        // Danh sách hợp đồng Cần phe duyệt
        public async Task<ActionResult> HopDongCanDuyet()
        {
            try
            {
                List<HopDong> listHopDong = await _db.HopDong.Where(n => n.TrangThai == Constant.ChoDuyet)
                                                             .ToListAsync();

                return View(listHopDong);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Xem danh sách hợp đồng thất bại.";
                return RedirectToAction("Index", "HopDong");
            }
        }

        #region Duyệt hợp đồng
        public async Task<ActionResult> DuyetHopDong(int iMaHopDong)
        {
            try
            {
                HopDong hopDong = await _db.HopDong.FindAsync(iMaHopDong);
                if (hopDong == null)
                {
                    TempData["ToastMessage"] = "error|Không tồn tại hợp đồng.";
                    return RedirectToAction("HopDongCanDuyet", "HopDong");
                }


                //hopDong.
                return RedirectToAction("HopDongCanDuyet", "HopDong");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Duyệt hợp đồng thất bại.";
                return RedirectToAction("HopDongCanDuyet", "HopDong");
            }
        }
        #endregion
    }
}