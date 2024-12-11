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

        // Danh sách hợp đồng Cần phê duyệt
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

        #region Cập nhật hợp đồng
        public async Task<ActionResult> CapNhat(int iMaHopDong)
        {
            try
            {
                HopDong hopDong = await _db.HopDong.FindAsync(iMaHopDong);
                if (hopDong == null)
                {
                    TempData["ToastMessage"] = "error|Hợp đồng không tồn tại.";
                    return RedirectToAction("Index", "Home");
                }


                return View(hopDong);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Cập nhật hợp đồng thất bại.";
                return RedirectToAction("Index", "Khu");
            }
        }
        #endregion

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
                string ho = HttpUtility.UrlDecode(cookie["Ho"], System.Text.Encoding.UTF8);
                string ten = HttpUtility.UrlDecode(cookie["Ten"], System.Text.Encoding.UTF8);

                HopDong hopDong = await _db.HopDong.FindAsync(iMaHopDong);
                if (hopDong == null)
                {
                    TempData["ToastMessage"] = "error|Không tồn tại hợp đồng.";
                    return RedirectToAction("HopDongCanDuyet", "HopDong");
                }

                hopDong.NgayDuyet = DateTime.Now;
                hopDong.TrangThai = Constant.DaDuyet;

                // Cập nhật lại trạng thái giường và hủy số lượng ở trong phòng
                hopDong.Giuong.TrangThai = Constant.DaDangKy;

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
                                   "<p>Hợp đồng của bạn đã được duyệt. Truy cập <a href=\"https://qlkytucxa.somee.com/\">https://qlkytucxa.somee.com/</a> để sử dụng</p>" +
                                   $"<p><strong>Mã hợp đồng:</strong> {hopDong.MaHopDong}</p>" +
                                   $"<p><strong>Tên hợp đồng:</strong> {hopDong.TenHopDong}</p>" +
                                   $"<p><strong>Ngày ở:</strong> {hopDong.ThoiHanDangKy.NgayBatDau}</p>" +
                                   $"<p><strong>Ngày trả:</strong> {hopDong.ThoiHanDangKy.NgayKetThuc}</p>" +
                                   $"<p><strong>Phê duyệt bởi ban quản lý:</strong> {ho} {ten}(Mã: {taiKhoanNV})</p>",
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

        #region Xóa hợp đồng
        public async Task<ActionResult> Xoa(int iMaHopDong)
        {
            try
            {
                HopDong hopDong = await _db.HopDong.FindAsync(iMaHopDong);
                if (hopDong == null)
                {
                    TempData["ToastMessage"] = "error|Hợp đồng không tồn tại.";
                    return RedirectToAction("HopDongCanDuyet", "HopDong");
                }

                if (hopDong.Giuong == null || hopDong.Giuong.Phong == null)
                {
                    TempData["ToastMessage"] = "error|Thông tin giường hoặc phòng không hợp lệ.";
                    return RedirectToAction("HopDongCanDuyet", "HopDong");
                }

                // Cập nhật lại trạng thái giường và hủy số lượng ở trong phòng
                hopDong.Giuong.TrangThai = Constant.GiuongTrong;
                hopDong.Giuong.Phong.DaO--;
                hopDong.Giuong.Phong.ConTrong++;

                _db.HopDong.Remove(hopDong);
                await _db.SaveChangesAsync();

                return RedirectToAction("HopDongCanDuyet", "HopDong");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Xóa hợp đồng thất bại.";
                return RedirectToAction("HopDongCanDuyet", "HopDong");
            }
        }
        #endregion
    }
}