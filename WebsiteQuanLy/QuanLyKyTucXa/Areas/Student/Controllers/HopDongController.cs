using QuanLyKyTucXa.Common.Const;
using QuanLyKyTucXa.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKyTucXa.Areas.Student.Controllers
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
            ViewBag.listKhu = await _db.Khu.ToListAsync();
            ViewBag.listTang = await _db.Tang.ToListAsync();
            ViewBag.listLoaiPhong = await _db.LoaiPhong.ToListAsync();
            ViewBag.listThoiHanDangKy = await _db.ThoiHanDangKy.Where(n => n.NgayMo <= DateTime.Now &&
                                                                n.NgayDong >= DateTime.Now)
                                                         .ToListAsync();

            List<HopDong> listHopDong = await _db.HopDong.Where(n => n.MaSinhVien == maSinhVien).ToListAsync();

            return View(listHopDong);
        }

        #region Xem chi tiết hợp đồng
        public async Task<ActionResult> XemChiTiet(int iMaHopDong)
        {
            try
            {
                HopDong hopDong = await _db.HopDong.FindAsync(iMaHopDong);
                if (hopDong == null)
                {
                    TempData["ToastMessage"] = "error|Hợp đồng không tồn tại.";
                    return RedirectToAction("Index", "Home");
                }

                // Dữ liệu tìm kiếm
                ViewBag.listLoaiKhu = await _db.LoaiKhu.ToListAsync();
                ViewBag.listKhu = await _db.Khu.ToListAsync();
                ViewBag.listTang = await _db.Tang.ToListAsync();
                ViewBag.listLoaiPhong = await _db.LoaiPhong.ToListAsync();
                ViewBag.listThoiHanDangKy = await _db.ThoiHanDangKy.Where(n => n.NgayMo <= DateTime.Now &&
                                                                    n.NgayDong >= DateTime.Now)
                                                             .ToListAsync();

                return View(hopDong);
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Xem hợp đồng thất bại.";
                return RedirectToAction("Index", "Khu");
            }
        }
        #endregion

        #region Thêm mới hợp đồng
        [HttpPost]
        public async Task<ActionResult>  DangKy(int iGiuongChon, int iMaThoiHanDangKy)
        {
            // Lấy thông tin phòng từ giường
            Giuong giuong = await _db.Giuong.FindAsync(iGiuongChon);
            if (giuong == null)
            {
                if (giuong.TrangThai == Constant.DaDangKy)
                {
                    TempData["ToastMessage"] = "error|Giường đã được đăng ký.";
                }
                else
                {
                    TempData["ToastMessage"] = "error|Không tồn tại giường.";
                }
                return RedirectToAction("Index", "Home");
            }

            try
            {
                // Kiểm tra cookie đăng nhập
                var cookie = Request.Cookies["TKSinhVien"];
                if (cookie == null)
                {
                    TempData["ToastMessage"] = "error|Bạn cần đăng nhập trước khi đăng ký.";
                    return RedirectToAction("ChiTietPhong", "Phong", new { iMaPhong = giuong.Phong.MaPhong });
                }
                // Lấy thông tin sinh viên từ cookie
                string maSinhVien = cookie.Values["MaSinhVien"];

                // Kiểm tra xem sinh viên đã có hợp đồng nào chưa?
                HopDong hopDong = await _db.HopDong.FirstOrDefaultAsync(n => n.MaThoiHanDangKy == iMaThoiHanDangKy &&
                                                                        n.MaSinhVien == maSinhVien);
                if (hopDong != null)
                {
                    TempData["ToastMessage"] = "error|Bạn đã đăng ký phòng thời hạn này rồi.";
                    return RedirectToAction("ChiTietPhong", "Phong", new { iMaPhong = giuong.Phong.MaPhong });
                }

                // Lấy thông tin thời hạn đăng ký
                ThoiHanDangKy thoiHanDangKy = await _db.ThoiHanDangKy.FindAsync(iMaThoiHanDangKy);
                if (thoiHanDangKy == null)
                {
                    TempData["ToastMessage"] = "error|Không tồn tại thời hạn đăng ký.";
                    return RedirectToAction("ChiTietPhong", "Phong", new { iMaPhong = giuong.Phong.MaPhong });
                }

                #region Thời gian thuê và thành tiền
                // Số tháng tròn giữa NgayBatDau và NgayKetThuc
                int soThangTron = ((thoiHanDangKy.NgayKetThuc.Year - thoiHanDangKy.NgayBatDau.Year) * 12) +
                                  (thoiHanDangKy.NgayKetThuc.Month - thoiHanDangKy.NgayBatDau.Month);

                // Tính phần lẻ của tháng bắt đầu
                int soNgayTrongThangBatDau = DateTime.DaysInMonth(thoiHanDangKy.NgayBatDau.Year, thoiHanDangKy.NgayBatDau.Month);
                double phanLeThangBatDau = (soNgayTrongThangBatDau - thoiHanDangKy.NgayBatDau.Day + 1) / (double)soNgayTrongThangBatDau;

                // Tính phần lẻ của tháng kết thúc
                int soNgayTrongThangKetThuc = DateTime.DaysInMonth(thoiHanDangKy.NgayKetThuc.Year, thoiHanDangKy.NgayKetThuc.Month);
                double phanLeThangKetThuc = thoiHanDangKy.NgayKetThuc.Day / (double)soNgayTrongThangKetThuc;

                // Tổng số tháng thuê bao gồm cả phần lẻ
                double tongSoThangThue = soThangTron + phanLeThangBatDau + phanLeThangKetThuc - 1;
                tongSoThangThue = Math.Round(tongSoThangThue, 2);

                // Tính tổng tiền thuê chính xác
                double thanhTien = giuong.Phong.GiaThue * tongSoThangThue;
                thanhTien = Math.Round(thanhTien, 2);

                // Tính số ngày thuê
                TimeSpan khoangThoiGian = thoiHanDangKy.NgayKetThuc - thoiHanDangKy.NgayBatDau;
                int soNgayThue = khoangThoiGian.Days + 1;
                #endregion


                // Khởi tạo hợp đồng sinh viên
                HopDong hoopDongThue = new HopDong
                {
                    TenHopDong = thoiHanDangKy.TenThoiHanDangKy,
                    NgayDuyet = null,
                    NgayDenHanThanhToan = thoiHanDangKy.NgayKetThuc,
                    GiaThue = giuong.Phong.GiaThue,
                    SoNgayThue = soNgayThue,
                    SoThangThue = tongSoThangThue,
                    ThanhTien = thanhTien,

                    MaGiuong = giuong.MaGiuong,
                    MaSinhVien = maSinhVien,
                    MaThoiHanDangKy = iMaThoiHanDangKy,
                    TaiKhoanNV = null,

                    TrangThai = Constant.ChoDuyet,
                    ThanhToan = Constant.ChuaThanhToan,
                };


                _db.HopDong.Add(hoopDongThue);

                #region Cập nhật lại trạng thái giường và hủy số lượng ở trong phòng
                giuong.TrangThai = Constant.ChoXacNhan;

                Phong phong = await _db.Phong.FindAsync(giuong.Phong.MaPhong);
                if (phong == null)
                {
                    TempData["ToastMessage"] = "error|Không tồn tại phòng.";
                    return RedirectToAction("Index", "HopDong");
                }

                phong.DaO++;
                phong.ConTrong--;
                await _db.SaveChangesAsync();
                #endregion

                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Thêm hợp đồng thuê thành công.";
                return RedirectToAction("Index", "HopDong");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Đăng ký phòng thất bại.";
                return RedirectToAction("ChiTietPhong", "Phong", new { iMaPhong = giuong.Phong.MaPhong });
            }
        }
        #endregion


        #region Thanh Toán Hợp Đồng
        public async Task<ActionResult> ThanhToan(int iMaHopDong)
        {
            try
            {
                HopDong hopDong = await _db.HopDong.FindAsync(iMaHopDong);
                if (hopDong == null)
                {
                    TempData["ToastMessage"] = "error|Hợp đồng không tồn tại khu.";
                    return RedirectToAction("Index", "HopDong");
                }

                #region VNPAY
                // Thực hiện thanh toán bằng VNPAY
                string returnUrl = "https://localhost:44355/Student/HopDong/ReturnUrl";
                string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
                string vnp_TmnCode = "QV4AJ3NO";
                string vnp_HashSecret = "3CP0V5HCDJ6VFE1YPVYL85YUHK1SGLLP";
                string ngayThanhToan = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string vnpTxnRef = $"{ngayThanhToan}_{hopDong.MaHopDong}";

                // Tạo các thông tin cần thiết để gửi sang VNPAY
                SortedList<string, string> vnp_Params = new SortedList<string, string>();
                vnp_Params.Add("vnp_Version", "2.0.0");
                vnp_Params.Add("vnp_Command", "pay");
                vnp_Params.Add("vnp_TmnCode", vnp_TmnCode);
                vnp_Params.Add("vnp_Locale", "vn");
                vnp_Params.Add("vnp_CurrCode", "VND");
                vnp_Params.Add("vnp_TxnRef", vnpTxnRef);
                vnp_Params.Add("vnp_OrderInfo", "Thanh toan phong thue"); // Thông tin đơn hàng của bạn
                vnp_Params.Add("vnp_Amount", (hopDong.ThanhTien * 100).ToString()); // Số tiền cần thanh toán (phải nhân 100 vì VNPAY yêu cầu là số nguyên)
                vnp_Params.Add("vnp_ReturnUrl", returnUrl);
                vnp_Params.Add("vnp_IpAddr", Request.UserHostAddress); // IP của người thanh toán
                vnp_Params.Add("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));

                string vnp_SecureHashType = "SHA512"; // Loại mã hóa
                string vnp_SecureHash = HmacSHA512(vnp_HashSecret, string.Join("", vnp_Params.Select(kvp => kvp.Key + "=" + kvp.Value + "&").ToArray()).Trim('&'));

                vnp_Params.Add("vnp_SecureHashType", vnp_SecureHashType);
                vnp_Params.Add("vnp_SecureHash", vnp_SecureHash);

                string vnp_UrlEncode = vnp_Url + "?" + string.Join("&", vnp_Params.Select(kvp => kvp.Key + "=" + HttpUtility.UrlEncode(kvp.Value)).ToArray());
                return Redirect(vnp_UrlEncode);
                #endregion
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Lỗi khóa ngoại.";
                return RedirectToAction("Index", "HopDong");
            }
        }
        #endregion

        #region Thanh Toán VNPAY
        public async Task<ActionResult> ReturnUrl(string vnp_ResponseCode, string vnp_TransactionNo, string vnp_TxnRef)
        {
            // Xử lý kết quả từ VNPAY khi redirect về

            // Xử lý kết quả thanh toán tại đây
            if (vnp_ResponseCode == "00")
            {
                string maHopDongString = vnp_TxnRef.Split('_').Last();
                HopDong hopDong = await _db.HopDong.FindAsync(int.Parse(maHopDongString));
                if (hopDong == null)
                {
                    TempData["ToastMessage"] = "error|Hợp đồng không tồn tại.";
                    return RedirectToAction("Index", "HopDong");
                }

                hopDong.TrangThai = Constant.DaDuyet;
                hopDong.NgayDuyet = DateTime.Now;
                hopDong.ThanhToan = Constant.DaThanhToan;

                await _db.SaveChangesAsync();

                TempData["ToastMessage"] = "success|Thanh toán thành công.";
                return RedirectToAction("Index", "Home", new { area = "Student" });
            }
            else
            {
                HopDong hopDong = await _db.HopDong.FindAsync(int.Parse(vnp_TxnRef));
                if (hopDong == null)
                {
                    TempData["ToastMessage"] = "error|Hợp đồng không tồn tại.";
                    return RedirectToAction("Index", "HopDong");
                }

                return RedirectToAction("XemChiTiet", "HopDong", new { iMaHopDong = hopDong.MaHopDong });
            }
        }

        private string HmacSHA512(string key, string data)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashBytes = hmac.ComputeHash(dataBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }


        private string BuildQueryString(Dictionary<string, string> data)
        {
            List<string> queryString = new List<string>();
            foreach (var kvp in data.OrderBy(kvp => kvp.Key))
            {
                string encodedKey = HttpUtility.UrlEncode(kvp.Key);
                string encodedValue = HttpUtility.UrlEncode(kvp.Value);
                queryString.Add($"{encodedKey}={encodedValue}");
            }
            return string.Join("&", queryString);
        }
        #endregion

        #region Xóa Hợp Đồng
        public async Task<ActionResult> Xoa(int iMaHopDong)
        {
            try
            {
                HopDong hopDong = await _db.HopDong.FindAsync(iMaHopDong);
                if (hopDong == null)
                {
                    TempData["ToastMessage"] = "error|Hợp đồng không tồn tại.";
                    return RedirectToAction("Index", "HopDong");
                }

                #region Cập nhật lại trạng thái giường và hủy số lượng ở trong phòng
                Giuong giuong = await _db.Giuong.FindAsync(hopDong.MaGiuong);
                if (giuong == null)
                {
                    TempData["ToastMessage"] = "error|Không tồn tại khu.";
                    return RedirectToAction("Index", "HopDong");
                }
                giuong.TrangThai = Constant.GiuongTrong;


                Phong phong = await _db.Phong.FindAsync(giuong.MaPhong);
                if (phong == null)
                {
                    TempData["ToastMessage"] = "error|Không tồn tại phòng.";
                    return RedirectToAction("Index", "HopDong");
                }

                phong.DaO--;
                phong.ConTrong++;
                await _db.SaveChangesAsync();
                #endregion

                _db.HopDong.Remove(hopDong);
                await _db.SaveChangesAsync();
                TempData["ToastMessage"] = "success|Xóa hợp đồng thành công.";

                return RedirectToAction("Index", "HopDong");
            }
            catch (Exception ex)
            {
                // logerror
                Console.WriteLine(ex.ToString());

                TempData["ToastMessage"] = "error|Lỗi khóa ngoại.";
                return RedirectToAction("Index", "HopDong");
            }
        }
        #endregion
    }
}