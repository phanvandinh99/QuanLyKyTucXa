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
insert into Khu (TenKhu, MaLoaiKhu) values (N'Khu A', 2);
insert into Khu (TenKhu, MaLoaiKhu) values (N'Khu B', 2);
go
create table Tang
(
	MaTang int identity(1,1) primary key,
	TenTang nvarchar(100) not null,
	MaKhu int,

	Constraint fk_Tang_Khu Foreign Key (MaKhu) references Khu(MaKhu),
)
go
insert into Tang (TenTang, MaKhu) values (N'Tầng 1 (A101 -> A110)', 1);
insert into Tang (TenTang, MaKhu) values (N'Tầng 2 (A201 -> A210)', 1);
insert into Tang (TenTang, MaKhu) values (N'Tầng 3 (A301 -> A310)', 1);
insert into Tang (TenTang, MaKhu) values (N'Tầng 4 (A401 -> A410)', 1);
insert into Tang (TenTang, MaKhu) values (N'Tầng 5 (A501 -> A510)', 1);

insert into Tang (TenTang, MaKhu) values (N'Tầng 1 (A101 -> A110)', 2);
insert into Tang (TenTang, MaKhu) values (N'Tầng 2 (A201 -> A210)', 2);
insert into Tang (TenTang, MaKhu) values (N'Tầng 3 (A301 -> A310)', 2);
insert into Tang (TenTang, MaKhu) values (N'Tầng 4 (A401 -> A410)', 2);
insert into Tang (TenTang, MaKhu) values (N'Tầng 5 (A501 -> A510)', 2);

insert into Tang (TenTang, MaKhu) values (N'Tầng 1 (A101 -> A110)', 3);
insert into Tang (TenTang, MaKhu) values (N'Tầng 2 (A201 -> A210)', 3);
insert into Tang (TenTang, MaKhu) values (N'Tầng 3 (A301 -> A310)', 3);
insert into Tang (TenTang, MaKhu) values (N'Tầng 4 (A401 -> A410)', 3);
insert into Tang (TenTang, MaKhu) values (N'Tầng 5 (A501 -> A510)', 3);

insert into Tang (TenTang, MaKhu) values (N'Tầng 1 (A101 -> A110)', 3);
insert into Tang (TenTang, MaKhu) values (N'Tầng 2 (A201 -> A210)', 3);
insert into Tang (TenTang, MaKhu) values (N'Tầng 3 (A301 -> A310)', 3);
insert into Tang (TenTang, MaKhu) values (N'Tầng 4 (A401 -> A410)', 3);
insert into Tang (TenTang, MaKhu) values (N'Tầng 5 (A501 -> A510)', 3);

insert into Tang (TenTang, MaKhu) values (N'Tầng 1 (A101 -> A110)', 4);
insert into Tang (TenTang, MaKhu) values (N'Tầng 2 (A201 -> A210)', 4);
insert into Tang (TenTang, MaKhu) values (N'Tầng 3 (A301 -> A310)', 4);
insert into Tang (TenTang, MaKhu) values (N'Tầng 4 (A401 -> A410)', 4);
insert into Tang (TenTang, MaKhu) values (N'Tầng 5 (A501 -> A510)', 4);
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
	HinhAnh nvarchar(100) null,
	DonGia float not null,
)
go
insert into LoaiPhong (TenLoaiPhong, HinhAnh, DonGia) values (N'Phòng 2 sinh viên', N'img-2SV.jpg', 1000000);
insert into LoaiPhong (TenLoaiPhong, HinhAnh, DonGia) values (N'Phòng 4 sinh viên', N'img-4SV_1.jpg', 1100000);
insert into LoaiPhong (TenLoaiPhong, HinhAnh, DonGia) values (N'Phòng 6 sinh viên', N'img-6SV.jpg', 1200000);
insert into LoaiPhong (TenLoaiPhong, HinhAnh, DonGia) values (N'Phòng 8 sinh viên', N'img-8SV.jpeg', 1300000);
insert into LoaiPhong (TenLoaiPhong, HinhAnh, DonGia) values (N'Phòng 10 sinh viên', N'img-10SV.jpg', 1400000);
insert into LoaiPhong (TenLoaiPhong, HinhAnh, DonGia) values (N'Phòng 12 sinh viên', N'img-12SV.jpg', 1500000);
go
create table Phong
(
	MaPhong int identity(1,1) primary key,
	TenPhong nvarchar(100) not null,
	DaO int default(0),
	ConTrong int default(0),
	GiaDichVu float default(0),
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
values (N'Phòng A101', 1, 7, default, 1000000, default, 1, 1, 1);
insert into Phong (TenPhong, DaO, ConTrong, GiaDichVu, GiaThue, TrangThai, MaTang, MaLoaiPhong, MaTrangThai) 
values (N'Phòng A102', 1, 7, default, 1000000, default, 1, 1, 1);
insert into Phong (TenPhong, DaO, ConTrong, GiaDichVu, GiaThue, TrangThai, MaTang, MaLoaiPhong, MaTrangThai) 
values (N'Phòng A103', 1, 7, default, 1000000, default, 1, 1, 1);
insert into Phong (TenPhong, DaO, ConTrong, GiaDichVu, GiaThue, TrangThai, MaTang, MaLoaiPhong, MaTrangThai) 
values (N'Phòng A104', 1, 7, default, 1000000, default, 1, 1, 1);
insert into Phong (TenPhong, DaO, ConTrong, GiaDichVu, GiaThue, TrangThai, MaTang, MaLoaiPhong, MaTrangThai) 
values (N'Phòng A105', 1, 7, default, 1000000, default, 1, 1, 1);
insert into Phong (TenPhong, DaO, ConTrong, GiaDichVu, GiaThue, TrangThai, MaTang, MaLoaiPhong, MaTrangThai) 
values (N'Phòng A106', 1, 7, default, 1000000, default, 1, 1, 1);
insert into Phong (TenPhong, DaO, ConTrong, GiaDichVu, GiaThue, TrangThai, MaTang, MaLoaiPhong, MaTrangThai) 
values (N'Phòng A107', 1, 7, default, 1000000, default, 1, 1, 1);
insert into Phong (TenPhong, DaO, ConTrong, GiaDichVu, GiaThue, TrangThai, MaTang, MaLoaiPhong, MaTrangThai) 
values (N'Phòng A108', 1, 7, default, 1000000, default, 1, 1, 1);
insert into Phong (TenPhong, DaO, ConTrong, GiaDichVu, GiaThue, TrangThai, MaTang, MaLoaiPhong, MaTrangThai) 
values (N'Phòng A109', 1, 7, default, 1000000, default, 1, 1, 1);
insert into Phong (TenPhong, DaO, ConTrong, GiaDichVu, GiaThue, TrangThai, MaTang, MaLoaiPhong, MaTrangThai) 
values (N'Phòng A110-', 1, 7, default, 1000000, default, 1, 1, 1);
go
create table Giuong
(
	MaGiuong int identity(1,1) primary key,
	TenGiuong nvarchar(100) not null,
	TrangThai Char(1) default('C'), -- A: Đang Trống , B: Đang Đăng Ký, C: Đã Đăng Ký
	MaPhong int,

	Constraint fk_Duong_Phong Foreign Key (MaPhong) references Phong(MaPhong),
)
go
insert into Giuong (TenGiuong, TrangThai, MaPhong) 
values (N'Dường 1', 'C', 1);
insert into Giuong (TenGiuong, TrangThai, MaPhong) 
values (N'Dường 2', 'B', 1);
insert into Giuong (TenGiuong, TrangThai, MaPhong) 
values (N'Dường 3', 'A', 1);
insert into Giuong (TenGiuong, TrangThai, MaPhong) 
values (N'Dường 4', 'A', 1);
insert into Giuong (TenGiuong, TrangThai, MaPhong) 
values (N'Dường 5', 'A', 1);
insert into Giuong (TenGiuong, TrangThai, MaPhong) 
values (N'Dường 6', 'A', 1);
insert into Giuong (TenGiuong, TrangThai, MaPhong) 
values (N'Dường 7', 'A', 1);
go
create table DichVu
(
	MaDichVu int identity(1,1) primary key,
	TenDichVu nvarchar(100) not null,
	DonGia float,
)
go
insert into DichVu (TenDichVu, DonGia) values (N'Vệ sinh riêng', 0);
insert into DichVu (TenDichVu, DonGia) values (N'Vệ sinh trong', 10000);
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
values (default , default, 3, 1);
insert into DichVuPhong (Xoa, NgayThem, MaPhong, MaDichVu) 
values (default , default, 4, 1);
insert into DichVuPhong (Xoa, NgayThem, MaPhong, MaDichVu) 
values (default , default, 5, 1);
insert into DichVuPhong (Xoa, NgayThem, MaPhong, MaDichVu) 
values (default , default, 1, 2);
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
	DoiMatKhau bit,
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
