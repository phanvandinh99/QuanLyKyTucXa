using QuanLyKyTucXa.Common.Const;
using QuanLyKyTucXa.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyKyTucXa.Areas.Student.Controllers
{
    public class BaoCaoController : Controller
    {
        private readonly QLKyTucXa _db;

        public BaoCaoController(QLKyTucXa db)
        {
            _db = db;
        }

        // Trang báo cáo sinh viên
        public async Task<ActionResult> Index()
        {
            // Kiểm tra cookie đăng nhập
            var cookie = Request.Cookies["TKSinhVien"];
            if (cookie == null)
            {
                TempData["ToastMessage"] = "error|Bạn cần đăng nhập để xem phòng thuê.";
                return RedirectToAction("Index", "Home");
            }
            string maSinhVien = cookie.Values["MaSinhVien"];

            // Dữ liệu tìm kiếm
            ViewBag.listLoaiKhu = await _db.LoaiKhu.ToListAsync();
            ViewBag.listKhu = await _db.BaoCao.ToListAsync();
            ViewBag.listTang = await _db.Tang.ToListAsync();
            ViewBag.listLoaiPhong = await _db.LoaiPhong.ToListAsync();
            ViewBag.listThoiHanDangKy = await _db.ThoiHanDangKy.Where(n => n.NgayMo <= DateTime.Now &&
                                                                      n.NgayDong >= DateTime.Now)
                                                               .ToListAsync();

            List<BaoCao> listBaoCao = await _db.BaoCao.Where(n => n.MaSinhVien == maSinhVien).ToListAsync();

            return View(listBaoCao);
        }

        #region Thêm mới báo cáo
        public async Task<ActionResult> ThemMoi()
        {
            try
            {
                ViewBag.MaLoaiThongBao = await _db.LoaiBaoCao.ToListAsync();

                return View();
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Thêm loại báo cáo thất bại.";
                return RedirectToAction("Index", "BaoCao");
            }
        }

        [HttpPost]
        public async Task<ActionResult> ThemMoi(string sNoiDung, int iMaLoaiBaoCao)
        {
            try
            {
                // Kiểm tra cookie đăng nhập
                var cookie = Request.Cookies["TKSinhVien"];
                if (cookie == null)
                {
                    TempData["ToastMessage"] = "error|Bạn cần đăng nhập.";
                    return RedirectToAction("Index", "Home");
                }
                string maSinhVien = cookie.Values["MaSinhVien"];


                // Kiểm tra đang thêm dơn giá loại nào
                LoaiBaoCao loaiBaoCao = await _db.LoaiBaoCao.FindAsync(iMaLoaiBaoCao);
                if (loaiBaoCao == null)
                {
                    TempData["ToastMessage"] = "error|Loại báo cáo không tồn tại.";
                    return RedirectToAction("Index", "BaoCao");
                }

                BaoCao baoCao = new BaoCao();
                baoCao.NoiDung = sNoiDung;
                baoCao.NgayGui = DateTime.Now;
                baoCao.NgayXem = null;
                baoCao.TrangThai = Constant.ChoDuyet;
                baoCao.MaLoaiBaoCao = iMaLoaiBaoCao;
                baoCao.MaSinhVien = maSinhVien;
                baoCao.TaiKhoanNV = Constant.sBanQuanLy;

                _db.BaoCao.Add(baoCao);
                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Thêm báo cáo thành công.";
                return RedirectToAction("Index", "BaoCao");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Thêm báo cáo thất bại.";
                return RedirectToAction("Index", "DonGia");
            }
        }
        #endregion

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
        public async Task<ActionResult> CapNhat(BaoCao baoCaoModel)
        {
            try
            {
                BaoCao baoCao = await _db.BaoCao.FindAsync(baoCaoModel.MaBaoCao);
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

                baoCao.NoiDung = baoCaoModel.NoiDung;
                baoCao.MaLoaiBaoCao = baoCaoModel.MaLoaiBaoCao;
                baoCao.NgayGui = DateTime.Now;
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

        #region Xóa Báo Cáo
        public async Task<ActionResult> Xoa(int iMaBaoCao)
        {
            try
            {
                BaoCao baoCao = await _db.BaoCao.FindAsync(iMaBaoCao);
                if (baoCao == null)
                {
                    TempData["ToastMessage"] = "error|Không tồn tại báo cáo.";
                    return RedirectToAction("Index", "BaoCao");
                }

                _db.BaoCao.Remove(baoCao);
                await _db.SaveChangesAsync();
                TempData["ToastMessage"] = "success|Xóa báo cáo thành công.";

                return RedirectToAction("Index", "BaoCao");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Lỗi khóa ngoại.";
                return RedirectToAction("Index", "BaoCao");
            }
        }
        #endregion
    }
}