using QuanLyKyTucXa.Common.Const;
using QuanLyKyTucXa.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKyTucXa.Areas.QLKTX.Controllers
{
    public class BaoCaoController : Controller
    {
        private readonly QLKyTucXa _db;

        public BaoCaoController(QLKyTucXa db)
        {
            _db = db;
        }

        // Danh sách báo cáo sinh viên gửi
        public async Task<ActionResult> Index()
        {
            try
            {
                List<BaoCao> listBaoCao = await _db.BaoCao.OrderByDescending(n => n.MaBaoCao).ToListAsync();

                return View(listBaoCao);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Xem danh sách báo cáo thất bại.";
                return RedirectToAction("Index", "BaoCao");
            }
        }

        #region Cập nhật Báo Cáo
        public async Task<ActionResult> CapNhat(int iMaBaoCao)
        {
            try
            {
                BaoCao baoCao = await _db.BaoCao.FindAsync(iMaBaoCao);
                if (baoCao.TrangThai == Constant.DaDuyet)
                {
                    TempData["ToastMessage"] = "error|Không thể cập nhật báo cáo.";
                    return RedirectToAction("Index", "BaoCao");
                }

                ViewBag.MaLoaiBaoCao = baoCao.MaLoaiBaoCao;

                return View(baoCao);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Cập nhật báo cáo thất bại.";
                return RedirectToAction("Index", "BaoCao");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Duyet(int MaBaoCao)
        {
            try
            {
                BaoCao baoCao = await _db.BaoCao.FindAsync(MaBaoCao);
                if (baoCao == null)
                {
                    TempData["ToastMessage"] = "error|Không tồn tại báo cáo.";
                    return RedirectToAction("Index", "BaoCao");
                }
                if (baoCao.TrangThai == Constant.DaDuyet)
                {
                    TempData["ToastMessage"] = "error|Không thể cập nhật báo cáo.";
                    return RedirectToAction("Index", "BaoCao");
                }
                baoCao.NgayXem = DateTime.Now;
                baoCao.TrangThai = Constant.DaDuyet;
                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Cập nhật báo cáo thành công.";
                return RedirectToAction("Index", "BaoCao");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Cập nhật báo cáo thất bại.";
                return RedirectToAction("Index", "BaoCao");
            }
        }
        #endregion
    }
}