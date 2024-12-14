using QuanLyKyTucXa.Common.Const;
using QuanLyKyTucXa.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyKyTucXa.Areas.QLKTX.Controllers
{
    public class ThongBaoController : Controller
    {
        private readonly QLKyTucXa _db;

        public ThongBaoController(QLKyTucXa db)
        {
            _db = db;
        }

        // Danh sách báo thông báo
        public async Task<ActionResult> Index()
        {
            try
            {
                List<ThongBao> listThongBao = await _db.ThongBao.OrderByDescending(n => n.MaThongBao).ToListAsync();

                return View(listThongBao);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Xem danh sách thông báo thất bại.";
                return RedirectToAction("Index", "ThongBao");
            }
        }

        #region Thêm Mới Thông Báo
        public async Task<ActionResult> ThemMoi()
        {
            try
            {
                ViewBag.Phong = await _db.Phong.Where(n => n.DaO != 0).ToListAsync();
                ViewBag.LoaiThongBao = await _db.LoaiThongBao.ToListAsync();

                return View();
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Thêm báo cáo thất bại.";
                return RedirectToAction("Index", "BaoCao");
            }
        }

        [HttpPost]
        public async Task<ActionResult> ThemMoi(int MaLoaiThongBao, int MaPhong, string NoiDung)
        {
            try
            {
                // Lấy danh sách hợp đồng phù hợp
                var listHopDong = MaPhong == 0
                    ? await _db.HopDong.Where(n => n.Giuong.Phong.DaO != 0).ToListAsync()
                    : await _db.HopDong.Where(n => n.Giuong.MaPhong == MaPhong).ToListAsync();

                // Kiểm tra nếu danh sách hợp đồng rỗng
                if (listHopDong == null || !listHopDong.Any())
                {
                    TempData["ToastMessage"] = "error|Phòng không có sinh viên.";
                    return RedirectToAction("Index", "BaoCao");
                }

                // Tạo thông báo cho từng sinh viên trong danh sách hợp đồng
                var thongBaoList = listHopDong.Select(item => new ThongBao
                {
                    NoiDung = NoiDung,
                    NgayGui = DateTime.Now,
                    NgayXem = null,
                    TrangThai = Constant.DaDuyet,
                    MaSinhVien = item.MaSinhVien,
                    MaLoaiThongBao = MaLoaiThongBao
                }).ToList();

                // Thêm danh sách thông báo vào cơ sở dữ liệu
                _db.ThongBao.AddRange(thongBaoList);
                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Thêm thông báo thành công.";
                return RedirectToAction("Index", "BaoCao");
            }
            catch (Exception ex)
            {
                // Ghi log lỗi
                Console.WriteLine(ex);

                TempData["ToastMessage"] = "error|Thêm thông báo thất bại.";
                return RedirectToAction("Index", "BaoCao");
            }
        }

        #endregion

        #region Xem chi tiết
        public async Task<ActionResult> XemChiTiet(int iMaThongBao)
        {
            try
            {
                ThongBao thongBao = await _db.ThongBao.FindAsync(iMaThongBao);
                if (thongBao == null)
                {
                    TempData["ToastMessage"] = "error|Không tồn tại thông báo.";
                    return RedirectToAction("Index", "ThongBao");
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
        #endregion

        #region Xóa Thông báo
        public async Task<ActionResult> Xoa(int iMaThongBao)
        {
            try
            {
                ThongBao thongBao = await _db.ThongBao.FindAsync(iMaThongBao);
                if (thongBao == null)
                {
                    TempData["ToastMessage"] = "error|Không tồn tại thông báo.";
                    return RedirectToAction("Index", "ThongBao");
                }

                _db.ThongBao.Remove(thongBao);
                await _db.SaveChangesAsync();
                TempData["ToastMessage"] = "success|Xóa thông báo thành công.";

                return RedirectToAction("Index", "ThongBao");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Lỗi khóa ngoại.";
                return RedirectToAction("Index", "ThongBao");
            }
        }
        #endregion
    }
}