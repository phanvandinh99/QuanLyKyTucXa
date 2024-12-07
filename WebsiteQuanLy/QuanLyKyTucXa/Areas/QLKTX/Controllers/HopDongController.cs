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
                // Kiểm tra cookie đăng nhập
                var cookie = Request.Cookies["NhanVienBQL"];
                if (cookie == null)
                {
                    TempData["ToastMessage"] = "error|Bạn cần đăng nhập vào hệ thống.";
                    return RedirectToAction("Login", "DangNhap", new { area = "DangNhap" });
                }
                // Lấy thông tin sinh viên từ cookie
                string taiKhoanNV = cookie.Values["TaiKhoanNV"];
                string ho = cookie.Values["Ho"];
                string Ten = cookie.Values["Ten"];

                HopDong hopDong = await _db.HopDong.FindAsync(iMaHopDong);
                if (hopDong == null)
                {
                    TempData["ToastMessage"] = "error|Không tồn tại hợp đồng.";
                    return RedirectToAction("HopDongCanDuyet", "HopDong");
                }

                hopDong.NgayDuyet = DateTime.Now;
                hopDong.TrangThai = Constant.DaDuyet;
                await _db.SaveChangesAsync();

                // Tạo thông báo đặt phòng thành công
                // Khởi tạo hợp đồng sinh viên
                ThongBao thongBao = new ThongBao
                {
                    NoiDung = "Đơn đặt phòng của bạn đã được duyệt.\nHợp đồng: " + hopDong.TenHopDong + "\nNgày duyệt:" + hopDong.NgayDuyet,
                    NgayGui = DateTime.Now,
                    NgayXem = null,
                    MaLoaiThongBao = Constant.DuyetPhong,
                    MaSinhVien = hopDong.MaSinhVien,
                    TaiKhoanNV = taiKhoanNV

                };

                _db.ThongBao.Add(thongBao);
                await _db.SaveChangesAsync();

                // Gửi thông báo duyệt phòng thành công qua email
                bool emailSent = await Common.SendMail.SendEmailAsync(
                                   Constant.TitleDatPhong,
                                   "<p>Hợp đồng của bạn đã được duyệt. <a href=\"https://QLKyTucXa.somee.vn\">https://QLKyTucXa.somee.vn</a> đã được cập nhật</p>" +
                                   $"<p><strong>Mã hợp đồng:</strong> {hopDong.MaHopDong}</p>" +
                                   $"<p><strong>Tên hợp đồng:</strong> {hopDong.TenHopDong}</p>" +
                                   $"<p><strong>Ngày ở:</strong> {hopDong.ThoiHanDangKy.NgayBatDau}</p>" +
                                   $"<p><strong>Ngày trả:</strong> {hopDong.ThoiHanDangKy.NgayKetThuc}</p>" +
                                   $"<p><strong>Phê duyệt bởi ban quản lý:</strong> {ho} {Ten}(Mã: {taiKhoanNV})</p>",
                                   hopDong.SinhVien.Email
                   );
                if (emailSent == false)
                {
                    TempData["ToastMessage"] = "error|Lỗi khi gửi mail.";
                    return RedirectToAction("HopDongCanDuyet", "HopDong");
                };

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