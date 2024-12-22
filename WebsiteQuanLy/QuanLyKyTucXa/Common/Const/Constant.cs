namespace QuanLyKyTucXa.Common.Const
{
    public class Constant
    {
        // Kiểm tra đổi mật khẩu
        public const bool DoiMatKhau = true;
        public const bool KhongDoiMatKhau = false;

        // Trạng thái giường
        public const string GiuongTrong = "A";
        public const string ChoXacNhan = "B";
        public const string DaDangKy = "C";

        // Kiểm tra xác thực tài khoản sinh viên
        public const bool CanXacThucTaiKhoan = false;
        public const bool DaXacThucTaiKhoan = true;

        // Kiểm tra xóa tài khoản sinh viên
        public const bool HoatDong = false;
        public const bool Xoa = true;

        // Quyền
        public const int Admin = 1;
        public const int BanQuanLy = 2;

        // Trạng Thái Phòng
        public const int DangHoatDong = 1;
        public const int DangSuaChua = 2;
        public const int DangXay = 3;

        // Xóa dịch vụ phòng
        public const bool DichVuPhong = true;
        public const bool XoaDichVuPhong = false;

        // Xóa đơn giá
        public const bool DangApDung = true;
        public const bool DaXoa = false;

        // Thời hạn đăng ký
        public const bool ApDungThoiHanDangKy = true;
        public const bool HuyApDungThoiHanDangKy = false;

        // User Nhân viên
        public const bool NhanVienHoatDong = true;
        public const bool NhanVienKhoa = false;

        // Trạng thái hợp đồng
        public const bool ChoDuyet = false;
        public const bool DaDuyet = true;
        public const bool DaThanhToan = true;
        public const bool ChuaThanhToan = false;

        // Thông báo đặt phòng thành công
        public const string NoiDungDatPhong= "Bạn đã đặt phòng thành công";
        public const int DuyetPhong = 1; // Loại thông báo: Duyệt phòng thành công
        public const string TitleDatPhong = "Thông Báo Xác Nhận Ở Phòng Ký Túc Xá";

        // Tài khoản Nhân Viên
        public const string sAdmin = "Admin";
        public const string sBanQuanLy = "BanQuanLy";

        // Thời gian đăng ký
        public const int NganNgay = 15;
        public const int DaiNgay = 16;

        // Thời gian đăng ký
        public const bool TheoNgay = false;
        public const bool TheoThang = true;
    }
}