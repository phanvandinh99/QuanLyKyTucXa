GO
USE master;
GO
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'Database_QuanLyKyTucXa')
BEGIN
    ALTER DATABASE Database_QuanLyKyTucXa SET SINGLE_USER WITH ROLLBACK IMMEDIATE; -- Đảm bảo không ai kết nối đến cơ sở dữ liệu
    DROP DATABASE Database_QuanLyKyTucXa; -- Xóa cơ sở dữ liệu
END
GO
CREATE DATABASE Database_QuanLyKyTucXa
GO
USE Database_QuanLyKyTucXa
GO
create table SinhVien
(
	MaSinhVien varchar(10) primary key,
	MatKhau varchar(50) not null,
	AnhChanDung nvarchar(255) not null,
	Ho nvarchar(100) not null,
	Ten nvarchar(50) not null,
	GioiTinh bit not null,
	NgaySinh DateTime not null,
	Email nvarchar(100) not null,
	SDT nvarchar(12) not null,
	DanToc varchar(100) not null,
	DiemUuTien float default(0),
	TrangThai bit not null,
	DaXoa bit not null,
)
go
INSERT SinhVien (MaSinhVien, MatKhau, AnhChanDung, Ho, Ten, GioiTinh, NgaySinh, Email, SDT, DanToc, DiemUuTien, TrangThai, DaXoa) 
VALUES (N'111', N'Abc123', N'AnhTestSinhVien.jpg', N'Sinh Viên', N'Test', 0, CAST(N'1999-11-12 00:00:00.000' AS DateTime), N'Test@gmail.com', N'0971010281', N'Kinh', 0, 1, 0)
go
create  table DienChinhSach
(
	MaDienChinhSach int identity(1,1) primary key,
	TenDienChinhSach nvarchar(255) not null,
	DiemDienChinhSach float not null,
)
go
insert into DienChinhSach (TenDienChinhSach, DiemDienChinhSach) values (N'Con liệt sĩ', 1);
insert into DienChinhSach (TenDienChinhSach, DiemDienChinhSach) values (N'Con thương binh', 1);
insert into DienChinhSach (TenDienChinhSach, DiemDienChinhSach) values (N'Con bệnh bênh', 1);
insert into DienChinhSach (TenDienChinhSach, DiemDienChinhSach) values (N'Khuyết tật', 1);
insert into DienChinhSach (TenDienChinhSach, DiemDienChinhSach) values (N'Hộ nghèo', 1);
insert into DienChinhSach (TenDienChinhSach, DiemDienChinhSach) values (N'Hộ cận nghèo', 1);
insert into DienChinhSach (TenDienChinhSach, DiemDienChinhSach) values (N'Mồ côi cha lẫn mẹ', 1);
insert into DienChinhSach (TenDienChinhSach, DiemDienChinhSach) values (N'Sinh viên mồ côi cha', 1);
insert into DienChinhSach (TenDienChinhSach, DiemDienChinhSach) values (N'Sinh viên mồ côi mẹ', 1);
insert into DienChinhSach (TenDienChinhSach, DiemDienChinhSach) values (N'Vùng kinh tế đặc biệt khó khăn', 1);
insert into DienChinhSach (TenDienChinhSach, DiemDienChinhSach) values (N'Gia đình đặc biệt khó khăn', 1);
insert into DienChinhSach (TenDienChinhSach, DiemDienChinhSach) values (N'Gia đình khó khăn', 1);
insert into DienChinhSach (TenDienChinhSach, DiemDienChinhSach) values (N'Ảnh hưởng chất độc da cam', 1);
insert into DienChinhSach (TenDienChinhSach, DiemDienChinhSach) values (N'Sinh viên mắc bệnh hiểm nghèo', 1);
insert into DienChinhSach (TenDienChinhSach, DiemDienChinhSach) values (N'Đảng viên', 1);
insert into DienChinhSach (TenDienChinhSach, DiemDienChinhSach) values (N'Đoàn viên', 1);
go
create table SinhVienChinhSach
(
	MaSVChinhSach int identity(1,1) primary key,

	MaSinhVien varchar(10),
	MaDienChinhSach int,
	TrangThai bit default(0),
	Constraint fk_SinhVienChinhSach Foreign Key (MaSinhVien) references SinhVien(MaSinhVien) ON DELETE CASCADE,
	Constraint fk_DienChinhSach Foreign Key (MaDienChinhSach) references DienChinhSach(MaDienChinhSach) ON DELETE CASCADE,
)
go
create table LoaiKhu
(
	MaLoaiKhu int identity(1,1) primary key,
	TenLoaiKhu nvarchar(100) not null,
)
go
insert into LoaiKhu (TenLoaiKhu) values (N'Dành cho Nam');
insert into LoaiKhu (TenLoaiKhu) values (N'Dành cho Nữ');
go
create table Khu
(
	MaKhu int identity(1,1) primary key,
	TenKhu nvarchar(100) not null,
	MaLoaiKhu int,

	Constraint fk_Khu_LoaiKhu Foreign Key (MaLoaiKhu) references LoaiKhu(MaLoaiKhu),
)
go
insert into Khu (TenKhu, MaLoaiKhu) values (N'Khu A', 1);
insert into Khu (TenKhu, MaLoaiKhu) values (N'Khu B', 1);
insert into Khu (TenKhu, MaLoaiKhu) values (N'Khu C', 1);
insert into Khu (TenKhu, MaLoaiKhu) values (N'Khu D', 2);
insert into Khu (TenKhu, MaLoaiKhu) values (N'Khu E', 2);
go
create table Tang
(
	MaTang int identity(1,1) primary key,
	TenTang nvarchar(100) not null,
	MaKhu int,

	Constraint fk_Tang_Khu Foreign Key (MaKhu) references Khu(MaKhu),
)
go
insert into Tang (TenTang, MaKhu) values (N'Tầng 1', 1);
insert into Tang (TenTang, MaKhu) values (N'Tầng 2', 1);
insert into Tang (TenTang, MaKhu) values (N'Tầng 3', 1);
insert into Tang (TenTang, MaKhu) values (N'Tầng 4', 1);
insert into Tang (TenTang, MaKhu) values (N'Tầng 5', 1);

insert into Tang (TenTang, MaKhu) values (N'Tầng 1', 2);
insert into Tang (TenTang, MaKhu) values (N'Tầng 2', 2);
insert into Tang (TenTang, MaKhu) values (N'Tầng 3', 2);
insert into Tang (TenTang, MaKhu) values (N'Tầng 4', 2);
insert into Tang (TenTang, MaKhu) values (N'Tầng 5', 2);

insert into Tang (TenTang, MaKhu) values (N'Tầng 1', 3);
insert into Tang (TenTang, MaKhu) values (N'Tầng 2', 3);
insert into Tang (TenTang, MaKhu) values (N'Tầng 3', 3);
insert into Tang (TenTang, MaKhu) values (N'Tầng 4', 3);
insert into Tang (TenTang, MaKhu) values (N'Tầng 5', 3);

insert into Tang (TenTang, MaKhu) values (N'Tầng 1', 3);
insert into Tang (TenTang, MaKhu) values (N'Tầng 2', 3);
insert into Tang (TenTang, MaKhu) values (N'Tầng 3', 3);
insert into Tang (TenTang, MaKhu) values (N'Tầng 4', 3);
insert into Tang (TenTang, MaKhu) values (N'Tầng 5', 3);

insert into Tang (TenTang, MaKhu) values (N'Tầng 1', 4);
insert into Tang (TenTang, MaKhu) values (N'Tầng 2', 4);
insert into Tang (TenTang, MaKhu) values (N'Tầng 3', 4);
insert into Tang (TenTang, MaKhu) values (N'Tầng 4', 4);
insert into Tang (TenTang, MaKhu) values (N'Tầng 5', 4);
go
create table TrangThai
(
	MaTrangThai int identity(1,1) primary key,
	TenTrangThai nvarchar(100) not null,
)
go
insert into TrangThai (TenTrangThai) values (N'Đang hoạy động');
insert into TrangThai (TenTrangThai) values (N'Đang sửa chữa');
insert into TrangThai (TenTrangThai) values (N'Đang xây');
go
create table LoaiPhong
(
	MaLoaiPhong int identity(1,1) primary key,
	TenLoaiPhong nvarchar(100) not null,
	SoGiuong int not null,
	HinhAnh nvarchar(100) null,
	DonGia float not null,
)
go
insert into LoaiPhong (TenLoaiPhong, SoGiuong, HinhAnh, DonGia) values (N'Phòng 2 sinh viên', 2, N'img-2SV.jpg', 700000);
insert into LoaiPhong (TenLoaiPhong, SoGiuong, HinhAnh, DonGia) values (N'Phòng 4 sinh viên', 4, N'img-4SV_1.jpg', 600000);
insert into LoaiPhong (TenLoaiPhong, SoGiuong, HinhAnh, DonGia) values (N'Phòng 6 sinh viên', 6, N'img-6SV.jpg', 500000);
insert into LoaiPhong (TenLoaiPhong, SoGiuong, HinhAnh, DonGia) values (N'Phòng 8 sinh viên', 8, N'img-8SV.jpeg', 400000);
insert into LoaiPhong (TenLoaiPhong, SoGiuong, HinhAnh, DonGia) values (N'Phòng 10 sinh viên', 10, N'img-10SV.jpg', 300000);
insert into LoaiPhong (TenLoaiPhong, SoGiuong, HinhAnh, DonGia) values (N'Phòng 12 sinh viên', 12, N'img-12SV.jpg', 200000);
go
create table Phong
(
	MaPhong int identity(1,1) primary key,
	TenPhong nvarchar(100) not null,
	DaO int default(0),
	ConTrong int default(0),
	GiaDichVu float not null,
	GiaThue float not null,
	TrangThai bit default(0),
	MaTang int,
	MaLoaiPhong int,
	MaTrangThai int,

	Constraint fk_Phong_Tang Foreign Key (MaTang) references Tang(MaTang),
	Constraint fk_Phong_LoaiPhong Foreign Key (MaLoaiPhong) references LoaiPhong(MaLoaiPhong),
	Constraint fk_Phong_TrangThai Foreign Key (MaTrangThai) references TrangThai(MaTrangThai),
)
go
insert into Phong (TenPhong, DaO, ConTrong, GiaDichVu, GiaThue, TrangThai, MaTang, MaLoaiPhong, MaTrangThai) 
values (N'Phòng A101', 0, 2, 0, 1100000, default, 1, 1, 1);
insert into Phong (TenPhong, DaO, ConTrong, GiaDichVu, GiaThue, TrangThai, MaTang, MaLoaiPhong, MaTrangThai) 
values (N'Phòng A102', 0, 2, 0, 1100000, default, 1, 1, 1);
go
create table Giuong
(
	MaGiuong int identity(1,1) primary key,
	TenGiuong nvarchar(100) not null,
	TrangThai Char(1) default('C'), -- A: Đang Trống , B: Chờ Xác Nhận, C: Đã Đăng Ký
	MaPhong int,

	Constraint fk_Duong_Phong Foreign Key (MaPhong) references Phong(MaPhong),
)
go
insert into Giuong (TenGiuong, TrangThai, MaPhong) 
values (N'Giường 1', 'A', 1);
insert into Giuong (TenGiuong, TrangThai, MaPhong) 
values (N'Giường 2', 'A', 1);
insert into Giuong (TenGiuong, TrangThai, MaPhong) 
values (N'Giường 1', 'A', 2);
insert into Giuong (TenGiuong, TrangThai, MaPhong) 
values (N'Giường 2', 'A', 2);
go
create table DichVu
(
	MaDichVu int identity(1,1) primary key,
	TenDichVu nvarchar(100) not null,
	DonGia float not null,
)
go
insert into DichVu (TenDichVu, DonGia) values (N'Vệ sinh chung', 0);
insert into DichVu (TenDichVu, DonGia) values (N'Vệ sinh riêng', 10000);
insert into DichVu (TenDichVu, DonGia) values (N'Máy nóng lạnh', 10000);
insert into DichVu (TenDichVu, DonGia) values (N'Máy điều hòa', 10000);
insert into DichVu (TenDichVu, DonGia) values (N'Lọc nước', 10000);
insert into DichVu (TenDichVu, DonGia) values (N'Tủ lạnh', 10000);
go
create table DichVuPhong
(
	MaDichVuPhong int identity(1,1) primary key,
	Xoa bit default(0),
	NgayThem Datetime default(getdate()),
	NgayXoa Datetime null,
	MaPhong int,
	MaDichVu int,

	Constraint fk_DichVuPhong_Phong Foreign Key (MaPhong) references Phong(MaPhong),
	Constraint fk_DichVuPhong_DichVu Foreign Key (MaDichVu) references DichVu(MaDichVu),
)
go
insert into DichVuPhong (Xoa, NgayThem, MaPhong, MaDichVu) 
values (default , default, 1, 1);
insert into DichVuPhong (Xoa, NgayThem, MaPhong, MaDichVu) 
values (default , default, 2, 1);
insert into DichVuPhong (Xoa, NgayThem, MaPhong, MaDichVu) 
values (default , default, 1, 1);
insert into DichVuPhong (Xoa, NgayThem, MaPhong, MaDichVu) 
values (default , default, 2, 1);
insert into DichVuPhong (Xoa, NgayThem, MaPhong, MaDichVu) 
values (default , default, 1, 1);
insert into DichVuPhong (Xoa, NgayThem, MaPhong, MaDichVu) 
values (default , default, 2, 2);
go
create table ThoiHanDangKy
(
	MaThoiHanDangKy int identity(1,1) primary key,
	TenThoiHanDangKy nvarchar(100) not null,
	NgayMo Datetime not null,
	NgayDong Datetime not null,
	NgayBatDau Datetime not null,
	NgayKetThuc Datetime not null,
	TrangThai bit not null,
)
go
create table LoaiHoaDon
(
	MaLoaiHoaDon int identity(1,1) primary key,
	TenLoaiHoaDon nvarchar(100) not null,
)
go
insert into LoaiHoaDon (TenLoaiHoaDon) values (N'Hóa Đơn Điện');
insert into LoaiHoaDon (TenLoaiHoaDon) values (N'Hóa Đơn Nước');
insert into LoaiHoaDon (TenLoaiHoaDon) values (N'Tiền Rác');
insert into LoaiHoaDon (TenLoaiHoaDon) values (N'Wifi');
go
create table DonGia
(
	MaDonGia int identity(1,1) primary key,
	NgayBatDau Datetime not null,
	NgayKetThuc Datetime null,
	DonVi nvarchar(100) null,
	DonGia float not null,
	MaLoaiHoaDon int,
	DaXoa bit not null,

	Constraint fk_DonGia_LoaiHoaDon Foreign Key (MaLoaiHoaDon) references LoaiHoaDon(MaLoaiHoaDon),
)
go
create table Quyen
(
	MaQuyen int identity(1,1) primary key,
	TenQuyen nvarchar(100) not null,
)
go
insert into Quyen (TenQuyen) values (N'Admin');
insert into Quyen (TenQuyen) values (N'Ban Quản Lý');
go
create table NhanVien
(
	TaiKhoanNV varchar(10) primary key,
	MatKhau varchar(50) not null,
	AnhChanDung nvarchar(255) not null,
	Ho nvarchar(100) not null,
	Ten nvarchar(50) not null,
	GioiTinh bit not null,
	NgaySinh DateTime not null,
	Email nvarchar(100) not null,
	SDT nvarchar(12) not null,
	DoiMatKhau bit,
	TrangThai bit not null,
	MaQuyen int,

	Constraint fk_NhanVien_Quyen Foreign Key (MaQuyen) references Quyen(MaQuyen),
)
go
insert into NhanVien (TaiKhoanNV, MatKhau, AnhChanDung, Ho, Ten, GioiTinh, NgaySinh, Email, SDT, TrangThai, MaQuyen) 
values ('Admin', 'Abc123', 'Admin.jpg', N'Nhân Viên', N'Admin', 0, '01/01/1990', 'Admin@gmail.com', '0971234567', 0, 1);
insert into NhanVien (TaiKhoanNV, MatKhau, AnhChanDung, Ho, Ten, GioiTinh, NgaySinh, Email, SDT, TrangThai, MaQuyen) 
values ('BanQuanLy', 'Abc123', 'BanQuanLy.png', N'Ban Quản', N'Lý', 0, '01/01/1991', 'BanQuanLy@gmail.com', '0971234568', 0, 2);
go
create table HopDong
(
	MaHopDong int identity(1,1) primary key,
	TenHopDong nvarchar(100) not null,
	NgayDuyet Datetime null,
	NgayDenHanThanhToan datetime null,
	GiaThue float not null,
	SoNgayThue int not null,
	SoThangThue float not null,
	ThanhTien float not null,

	MaGiuong int,
	MaSinhVien varchar(10),
	MaThoiHanDangKy int,
	TaiKhoanNV varchar(10) null,


	TrangThai bit not null, -- Chờ xác nhận / duyệt
	ThanhToan bit not null, -- Đã thanh toán/ chưa thanh toán

	Constraint fk_HopDong_Giuong Foreign Key (MaGiuong) references Giuong(MaGiuong),
	Constraint fk_HopDong_SinhVien Foreign Key (MaSinhVien) references SinhVien(MaSinhVien),
	Constraint fk_HopDong_ThoiHanDangKy Foreign Key (MaThoiHanDangKy) references ThoiHanDangKy(MaThoiHanDangKy),
)
go
create table HoaDon
(
	MaHoaDon int identity(1,1) primary key,
	ChuSoDau int not null,
	ChuSoCuoi int not null,
	TongSoChu int not null,
	TongTien float not null,
	Thang datetime not null,
	HanCuoiThanhToan datetime not null,

	MaPhong int,
	MaDonGia int,
	TaiKhoanNV varchar(10),

	Constraint fk_HoaDon_Phong Foreign Key (MaPhong) references Phong(MaPhong),
	Constraint fk_HoaDon_DonGia Foreign Key (MaDonGia) references DonGia(MaDonGia),
	Constraint fk_HoaDon_NhanVien Foreign Key (TaiKhoanNV) references NhanVien(TaiKhoanNV),
)
go
create table LoaiViPham
(
	MaLoaiViPham int identity(1,1) primary key,
	TenLoaiViPham nvarchar(150) not null,
)
go
insert into LoaiViPham (TenLoaiViPham) values (N'Vi Phạm Nội Quy');
insert into LoaiViPham (TenLoaiViPham) values (N'Quá hạn Thanh Toán');
insert into LoaiViPham (TenLoaiViPham) values (N'Hư Hỏng Vật Chất');
go
create table ViPham
(
	MaViPham int identity(1,1) primary key,
	NoiDung nvarchar(max) not null,
	NgayGui datetime not null,
	NgayXem datetime null,

	MaLoaiViPham int,
	MaSinhVien varchar(10),
	TaiKhoanNV varchar(10),

	Constraint fk_ViPham_LoaiViPham Foreign Key (MaLoaiViPham) references LoaiViPham(MaLoaiViPham),
	Constraint fk_ViPham_SinhVien Foreign Key (MaSinhVien) references SinhVien(MaSinhVien),
	Constraint fk_ViPham_NhanVien Foreign Key (TaiKhoanNV) references NhanVien(TaiKhoanNV),
)
go
create table LoaiThongBao
(
	MaLoaiThongBao int identity(1,1) primary key,
	TenLoaiThongBao nvarchar(150) not null,
)
go
insert into LoaiThongBao (TenLoaiThongBao) values (N'Duyệt Phòng');
insert into LoaiThongBao (TenLoaiThongBao) values (N'Đóng Tiền Điện');
insert into LoaiThongBao (TenLoaiThongBao) values (N'Đóng Tiền Nước');
insert into LoaiThongBao (TenLoaiThongBao) values (N'Hoàn Tất Thanh Toán');
go
create table ThongBao
(
	MaThongBao int identity(1,1) primary key,
	NoiDung nvarchar(max) not null,
	NgayGui datetime not null,
	NgayXem datetime null,

	MaLoaiThongBao int,
	MaSinhVien varchar(10),
	TaiKhoanNV varchar(10),

	Constraint fk_ThongBao_LoaiThongBao Foreign Key (MaLoaiThongBao) references LoaiThongBao(MaLoaiThongBao),
	Constraint fk_ThongBao_SinhVien Foreign Key (MaSinhVien) references SinhVien(MaSinhVien),
	Constraint fk_ThongBao_NhanVien Foreign Key (TaiKhoanNV) references NhanVien(TaiKhoanNV),
)
go