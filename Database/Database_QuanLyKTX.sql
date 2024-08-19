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
	DiemUuTien float default(0)
)
go
create  table DienChinhSach
(
	MaDienChinhSach int identity(1,1) primary key,
	TenDienChinhSach nvarchar(255) not null,
	DiemDienChinhSach float default(1),
)
go
create table SinhVienChinhSach
(
	MaSVChinhSach int identity(1,1) primary key,

	MaSinhVien varchar(10),
	MaDienChinhSach int,
	TrangThai bit default(0),
	Constraint fk_SinhVienChinhSach Foreign Key (MaSinhVien) references SinhVien(MaSinhVien),
	Constraint fk_DienChinhSach Foreign Key (MaDienChinhSach) references DienChinhSach(MaDienChinhSach),
)
go
create table LoaiKhu
(
	MaLoaiKhu int identity(1,1) primary key,
	TenLoaiKhu nvarchar(100) not null,
)
go
create table Khu
(
	MaKhu int identity(1,1) primary key,
	TenKhu nvarchar(100) not null,
	MaLoaiKhu int,

	Constraint fk_Khu_LoaiKhu Foreign Key (MaLoaiKhu) references LoaiKhu(MaLoaiKhu),
)
go
create table Tang
(
	MaTang int identity(1,1) primary key,
	TenTang nvarchar(100) not null,
	MaKhu int,

	Constraint fk_Tang_Khu Foreign Key (MaKhu) references Khu(MaKhu),
)
go
create table TrangThai
(
	MaTrangThai int identity(1,1) primary key,
	TenTrangThai nvarchar(100) not null,
)
go
create table LoaiPhong
(
	MaLoaiPhong int identity(1,1) primary key,
	TenLoaiPhong nvarchar(100) not null,
	DonGia float not null,
)
go
create table Phong
(
	MaPhong int identity(1,1) primary key,
	TenPhong nvarchar(100) not null,
	DaO int default(0),
	TrangThai bit,
	MaTang int,
	MaLoaiPhong int,
	MaTrangThai int,

	Constraint fk_Phong_Tang Foreign Key (MaTang) references Tang(MaTang),
	Constraint fk_Phong_LoaiPhong Foreign Key (MaLoaiPhong) references LoaiPhong(MaLoaiPhong),
	Constraint fk_Phong_TrangThai Foreign Key (MaTrangThai) references TrangThai(MaTrangThai),
)
go
create table Duong
(
	MaDuong int identity(1,1) primary key,
	TenDuong nvarchar(100) not null,
	TrangThai Char(1), -- A: Đã Đăng Ký, B: Đang Đăng Ký, C: Đang Trống
	MaPhong int,

	Constraint fk_Duong_Phong Foreign Key (MaPhong) references Phong(MaPhong),
)
go
go
create table ThoiHan
(
	MaThoiHan int identity(1,1) primary key,
	TenThoiHan nvarchar(100) not null,
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
create table DonGia
(
	MaDonGia int identity(1,1) primary key,
	NgayBatDau Datetime not null,
	NgayKetThuc Datetime null,
	DonVi float null,
	DonGia float null,
	MaLoaiHoaDon int,

	Constraint fk_DonGia_LoaiHoaDon Foreign Key (MaLoaiHoaDon) references LoaiHoaDon(MaLoaiHoaDon),
)
go
create table Quyen
(
	MaQuyen int identity(1,1) primary key,
	TenQuyen nvarchar(100) not null,
)
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
	MaQuyen int,

	Constraint fk_NhanVien_Quyen Foreign Key (MaQuyen) references Quyen(MaQuyen),
)
go
create table HopDong
(
	MaHopDong int identity(1,1) primary key,
	TenHopDong nvarchar(100) not null,
	NgayBatDau Datetime not null,
	NgayKetThuc Datetime not null,
	NgayDuyet Datetime not null,

	MaPhong int,
	MaSinhVien varchar(10),
	MaThoiHan int,
	TaiKhoanNV varchar(10),

	Constraint fk_HopDong_Phong Foreign Key (MaPhong) references Phong(MaPhong),
	Constraint fk_HopDong_SinhVien Foreign Key (MaSinhVien) references SinhVien(MaSinhVien),
	Constraint fk_HopDong_ThoiHan Foreign Key (MaThoiHan) references ThoiHan(MaThoiHan),
	Constraint fk_HopDong_NhanVien Foreign Key (TaiKhoanNV) references NhanVien(TaiKhoanNV),
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
