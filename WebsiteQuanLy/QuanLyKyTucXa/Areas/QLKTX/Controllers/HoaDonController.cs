﻿using QuanLyKyTucXa.Common.Const;
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
        [HttpPost]
        public async Task<ActionResult> ThemHoaDon(int iMaKhu, int iMaLoaiHoaDon, DateTime dThangThanhToan)
        {
            try
            {
                // Kiểm tra tháng này đã có hóa đơn chưa
                HoaDon hoaDon = await _db.HoaDon.FirstOrDefaultAsync(n => n.Thang.Month == dThangThanhToan.Month &&
                                                                     n.Thang.Year == dThangThanhToan.Year);
                if (hoaDon != null)
                {
                    TempData["ToastMessage"] = "error|Hóa đơn tháng " + hoaDon.Thang.Month + "/" + hoaDon.Thang.Year + " đã tồn tại";
                    return RedirectToAction("HoaDonKhu", "HoaDon", new { iMaLoaiHoaDon = iMaLoaiHoaDon });
                }

                // Kiểm tra khu này có số hóa đơn lần cuối là tháng mấy
                var lastHoaDon = await _db.HoaDon
                    .Where(n => n.Phong.Tang.Khu.MaKhu == iMaKhu)
                    .OrderByDescending(n => n.Thang) // Sắp xếp theo tháng giảm dần
                    .FirstOrDefaultAsync();

                if (lastHoaDon != null)
                {
                    int lastMonth = lastHoaDon.Thang.Month;
                    int lastYear = lastHoaDon.Thang.Year;

                    // So sánh năm và tháng của hóa đơn cuối cùng với tháng mới
                    if (lastYear > dThangThanhToan.Year || (lastYear == dThangThanhToan.Year && lastMonth > dThangThanhToan.Month))
                    {
                        TempData["ToastMessage"] = "error|Không thể thêm hóa đơn trước tháng " + lastMonth + "/" + lastYear;
                        return RedirectToAction("HoaDonKhu", "HoaDon", new { iMaLoaiHoaDon = iMaLoaiHoaDon });
                    }
                }


                // kiểm tra đã thêm Đơn giá cho loại hóa đơn đó hay chưa?
                DonGia donGia = await _db.DonGia.FirstOrDefaultAsync(n => n.MaLoaiHoaDon == iMaLoaiHoaDon);
                if (donGia == null)
                {
                    TempData["ToastMessage"] = "error|Bạn cần thêm đơn giá.";
                    return RedirectToAction("ThemMoi", "DonGia");
                }

                var listPhong = await (from p in _db.Phong
                                       join g in _db.Giuong on p.MaPhong equals g.MaPhong
                                       join h in _db.HopDong on g.MaGiuong equals h.MaGiuong
                                       join t in _db.ThoiHanDangKy on h.MaThoiHanDangKy equals t.MaThoiHanDangKy
                                       join hd in _db.HoaDon on p.MaPhong equals hd.MaPhong into HoaDonGroup
                                       where p.Tang.MaKhu == iMaKhu &&
                                             p.DaO != 0 &&
                                             h.NgayDuyet != null &&
                                             t.NgayBatDau <= dThangThanhToan &&
                                             t.NgayKetThuc >= dThangThanhToan
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
                                .FirstOrDefault()
                }).ToList();

                ViewBag.MaLoaiHoaDon = iMaLoaiHoaDon;
                ViewBag.ThangThanhToan = dThangThanhToan.ToString("yyyy-MM-dd");

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
        public async Task<ActionResult> TaoHoaDon(List<HoaDon> hoaDonModels, int? iMaLoaiHoaDon, DateTime dThangThanhToan)
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
                Thang = dThangThanhToan,
                HanCuoiThanhToan = dThangThanhToan.AddDays(5),
                MaPhong = viewModel.MaPhong,
                MaDonGia = donGia.MaDonGia,
                TrangThai = false,
                TaiKhoanNV = taiKhoanNV,
            }).ToList();

            try
            {
                // Lưu danh sách hóa đơn vào cơ sở dữ liệu
                _db.HoaDon.AddRange(hoaDons);
                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Thêm hóa đơn thành công!";

                // Lấy tất cả hợp đồng tương ứng với các phòng có trong danh sách hóa đơn
                var maPhongList = hoaDons.Select(hd => hd.MaPhong).Distinct().ToList();  // Lấy danh sách mã phòng duy nhất từ hoaDons
                var hopDongs = await _db.HopDong
                                        .Where(hd => maPhongList.Contains(hd.Giuong.Phong.MaPhong))  // Truy vấn hợp đồng theo mã phòng
                                        .ToListAsync();

                // Lặp qua từng hợp đồng để gửi email
                foreach (var hopDong in hopDongs)
                {
                    // Lấy email sinh viên từ hợp đồng
                    var email = hopDong.SinhVien?.Email;

                    // Nếu email không có giá trị, bỏ qua hợp đồng này
                    if (string.IsNullOrEmpty(email)) continue;

                    // Tìm hóa đơn của sinh viên này trong danh sách hóa đơn đã thêm
                    var hoaDon = hoaDons.FirstOrDefault(hd => hd.MaPhong == hopDong.Giuong.Phong.MaPhong);

                    if (hoaDon != null)
                    {
                        // Soạn nội dung email
                        string emailContent = $@"
                                                <p><strong>Chào bạn,</strong></p>
                                                <p>Hóa đơn điện nước của bạn đã được tạo. Dưới đây là thông tin chi tiết:</p>
                                                <p><strong>Chữ số đầu:</strong> {hoaDon.ChuSoDau}</p>
                                                <p><strong>Chữ số cuối:</strong> {hoaDon.ChuSoCuoi}</p>
                                                <p><strong>Đơn giá:</strong> {hoaDon.TongTien / (hoaDon.ChuSoCuoi - hoaDon.ChuSoDau)} VND</p>
                                                <p><strong>Thành tiền:</strong> {hoaDon.TongTien} VND</p>
                                                <p><strong>Hạn thanh toán:</strong> {hoaDon.HanCuoiThanhToan.ToString("dd/MM/yyyy")}</p>
                                                <p>Vui lòng thanh toán trước hạn thanh toán để tránh bị phạt trễ hạn.</p>
                                                <p>Chúc bạn một ngày tốt lành!</p>
                                                <p>Trân trọng,</p>
                                                <p>Quản lý ký túc xá</p>
                                            ";

                        // Gửi email
                        bool emailSent = await Common.SendMail.SendEmailAsync(
                            "Hóa đơn điện nước",  // Tiêu đề email
                            emailContent,          // Nội dung email
                            email                  // Địa chỉ email người nhận
                        );

                        if (!emailSent)
                        {
                            TempData["ToastMessage"] = "error|Lỗi khi gửi mail.";
                            return RedirectToAction("Index", "HoaDon");
                        }
                    }
                }

                TempData["ToastMessage"] = "success|Gửi mail thông báo thành công!";
                TempData["ToastMessage"] = "success|Thêm hóa đơn điện nước thành công!";
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                TempData["ToastMessage"] = $"error|Lỗi khi thêm hóa đơn: {ex.Message}";
                return RedirectToAction("Index", "HoaDon");
            }



            return RedirectToAction("Index", "HoaDon");
        }

        #region Xem hóa đơn sau khi thêm
        public async Task<ActionResult> XemHoaDon(int iMaLoaiHoaDon)
        {
            try
            {
                List<HoaDon> hoaDon = await _db.HoaDon.Where(n => n.DonGia.MaLoaiHoaDon == iMaLoaiHoaDon)
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

        #region Xem hóa đơn sau khi thêm
        public async Task<ActionResult> ThanhToan(int iMaHoaDon, int iMaLoaiHoaDon)
        {
            try
            {
                HoaDon hoaDon = await _db.HoaDon.FindAsync(iMaHoaDon);

                hoaDon.TrangThai = true;
                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Thanh toán hóa đơn điện nước thành công.";
                return RedirectToAction("XemHoaDon", "HoaDon", new { iMaLoaiHoaDon = iMaLoaiHoaDon });
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