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

    }
}