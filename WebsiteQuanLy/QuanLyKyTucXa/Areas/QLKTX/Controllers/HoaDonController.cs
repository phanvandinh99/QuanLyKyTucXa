using QuanLyKyTucXa.Common.Const;
using QuanLyKyTucXa.Models;
using QuanLyKyTucXa.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace QuanLyKyTucXa.Areas.QLKTX.Controllers
{
    public class HoaDonController : Controller
    {
        private readonly QLKyTucXa _db;

        public HoaDonController(QLKyTucXa db)
        {
            _db = db;
        }

        // Trang danh sách Loại hóa đơn
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
                return RedirectToAction("Index", "HoaDon");
            }
        }

        #region Thêm mới hoá đơn
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

                TempData["ToastMessage"] = "error|Thêm hóa đơn thất bại.";
                return RedirectToAction("Index", "HoaDon");
            }
        }

        public async Task<ActionResult> HoaDonKhu(int iMaLoaiHoaDon)
        {
            try
            {
                LoaiHoaDon loaiHoaDon = await _db.LoaiHoaDon.FindAsync(iMaLoaiHoaDon);
                if (loaiHoaDon == null)
                {
                    TempData["ToastMessage"] = "error|Không tồn tại loại hóa đơn.";
                    return RedirectToAction("Index", "HoaDon");
                }

                ViewBag.TenLoaiHoaDon = loaiHoaDon.TenLoaiHoaDon;

                // Danh sách khu
                List<Khu> listKhu = await _db.Khu.ToListAsync();

                ViewBag.MaLoaiHoaDon = iMaLoaiHoaDon;

                return View(listKhu);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Thêm hóa đơn theo khu thất bại.";
                return RedirectToAction("Index", "HoaDon");
            }
        }
        #endregion

        #region Thêm hóa đơn theo loại
        public async Task<ActionResult> ThemHoaDon(int iMaKhu, int iMaLoaiHoaDon)
        {
            try
            {
                // kiểm tra đã thêm Đơn giá cho loại hóa đơn đó hay chưa?
                DonGia donGia = await _db.DonGia.FirstOrDefaultAsync(n => n.MaLoaiHoaDon == iMaLoaiHoaDon);
                if (donGia == null)
                {
                    TempData["ToastMessage"] = "error|Bạn cần thêm đơn giá.";
                    return RedirectToAction("ThemMoi", "DonGia");
                }

                DateTime currentDate = DateTime.Now;

                var listPhong = await (from p in _db.Phong
                                       join g in _db.Giuong on p.MaPhong equals g.MaPhong
                                       join h in _db.HopDong on g.MaGiuong equals h.MaGiuong
                                       join t in _db.ThoiHanDangKy on h.MaThoiHanDangKy equals t.MaThoiHanDangKy
                                       join hd in _db.HoaDon on p.MaPhong equals hd.MaPhong into HoaDonGroup
                                       where p.Tang.MaKhu == iMaKhu &&
                                             p.DaO != 0 &&
                                             h.NgayDuyet != null &&
                                             t.NgayBatDau <= currentDate &&
                                             t.NgayKetThuc >= currentDate
                                       select new
                                       {
                                           Phong = p,
                                           HoaDonGroup = HoaDonGroup
                                       })
                            .ToListAsync(); // Truy vấn cơ sở dữ liệu và đưa về bộ nhớ

                // Xử lý trong bộ nhớ để lấy hóa đơn có `MaHoaDon` lớn nhất
                var result = listPhong.Select(item => new PhongHoaDonViewModel
                {
                    Phong = item.Phong,
                    HoaDon = item.HoaDonGroup
                                .Where(hd => hd.DonGia.MaLoaiHoaDon == iMaLoaiHoaDon) // Áp dụng điều kiện
                                .OrderByDescending(hd => hd.MaHoaDon)
                                .FirstOrDefault() // Lấy hóa đơn có `MaHoaDon` lớn nhất hoặc null
                }).ToList();

                ViewBag.MaLoaiHoaDon = iMaLoaiHoaDon;

                return View(result);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Thêm hóa đơn thất bại.";
                return RedirectToAction("Index", "DonGia");
            }
        }

        // Tạo hóa đơn
        [HttpPost]
        public async Task<ActionResult> TaoHoaDon(List<HoaDon> hoaDonModels, int? iMaLoaiHoaDon)
        {
            if (hoaDonModels == null || hoaDonModels.Count == 0)
            {
                TempData["ToastMessage"] = "error|Hóa Đơn Trống.";
                return RedirectToAction("ThemMoi", "HoaDon");
            }

            // Kiểm tra đã thêm Đơn giá cho loại hóa đơn đó hay chưa
            var donGia = await _db.DonGia.FirstOrDefaultAsync(n => n.MaLoaiHoaDon == iMaLoaiHoaDon);
            if (donGia == null)
            {
                TempData["ToastMessage"] = "error|Bạn cần thêm đơn giá.";
                return RedirectToAction("ThemMoi", "DonGia");
            }

            // Kiểm tra cookie đăng nhập
            var cookie = Request.Cookies["NhanVienBQL"];
            if (cookie == null)
            {
                TempData["ToastMessage"] = "error|Bạn cần đăng nhập vào hệ thống.";
                return RedirectToAction("Index", "Home");
            }

            string taiKhoanNV = cookie.Values["TaiKhoanNV"];

            // Tạo danh sách hóa đơn
            var hoaDons = hoaDonModels.Select(viewModel => new HoaDon
            {
                ChuSoDau = viewModel.ChuSoDau,
                ChuSoCuoi = viewModel.ChuSoCuoi,
                TongSoChu = viewModel.ChuSoCuoi - viewModel.ChuSoDau,
                TongTien = (viewModel.ChuSoCuoi - viewModel.ChuSoDau) * donGia.DonGia1,
                Thang = DateTime.Now,
                HanCuoiThanhToan = DateTime.Now.AddDays(15),
                MaPhong = viewModel.MaPhong,
                MaDonGia = donGia.MaDonGia,
                TaiKhoanNV = taiKhoanNV,
            }).ToList();

            try
            {
                // Lưu danh sách hóa đơn vào cơ sở dữ liệu
                _db.HoaDon.AddRange(hoaDons);
                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Thêm hóa đơn thành công!";
            }
            catch (Exception ex)
            {
                // Ghi log hoặc xử lý lỗi
                TempData["ToastMessage"] = $"error|Lỗi khi thêm hóa đơn: {ex.Message}";
                return RedirectToAction("ThemMoi", "HoaDon");
            }

            return RedirectToAction("Index", "HoaDon");
        }

        #region Xem hóa đơn sau khi thêm
        public async Task<ActionResult> XemHoaDon(int iMaLoaiHoaDon)
        {
            try
            {
                List<HoaDon> hoaDon = await _db.HoaDon.Where(n => n.DonGia.MaLoaiHoaDon == iMaLoaiHoaDon &&
                                                             n.Thang.Month == DateTime.Now.Month)
                                                      .ToListAsync();

                return View(hoaDon);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Xem hóa đơn thất bại.";
                return RedirectToAction("Index", "HoaDon");
            }
        }
        #endregion


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
                donGia.DonVi = donGiaModel.DonVi;
                donGia.DonGia1 = donGiaModel.DonGia1;
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

        //#region xóa đơn giá
        //public async Task<ActionResult> Xoa(int iMaDonGia)
        //{
        //    try
        //    {
        //        DonGia DonGia = await _db.DonGia.FindAsync(iMaDonGia);
        //        if (DonGia == null)
        //        {
        //            TempData["ToastMessage"] = "error|Không tồn tại đơn giá.";
        //            return RedirectToAction("Index", "DonGia");
        //        }

        //        _db.DonGia.Remove(DonGia);
        //        await _db.SaveChangesAsync();
        //        TempData["ToastMessage"] = "success|Xóa đơn giá thành công.";

        //        return RedirectToAction("Index", "DonGia");
        //    }
        //    catch (Exception ex)
        //    {
        //        // logerror
        //        Console.WriteLine(ex.ToString());

        //        TempData["ToastMessage"] = "error|Lỗi khóa ngoại.";
        //        return RedirectToAction("Index", "DonGia");
        //    }
        //}
        //#endregion
    }
}