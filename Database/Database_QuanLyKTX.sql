Create Database QLKyTucXa
go
use QLKyTucXa
go
create table SinhVien
(
	MaSinhVien varchar(12) primary key,
	MatKhau varchar(50) not null,
	AnhChanDung nvarchar(255) not null,
	Ho nvarchar(100) not null,
	Ten nvarchar(50) not null,
	GioiTinh bit not null,
	NgaySinh DateTime not null,
	Email nvarchar(100) not null,
	SDT nvarchar(12) not null,
	DanToc varchar(100) not null,
)
go
create  table DienChinhSach
(
	MaDienChinhSach int identity(1,1) primary key,
	TenDienChinhSach nvarchar(255) not null,
	DiemDienChinhSach float default(1),
)
go
create table SinhVien_DienChinhSach
(
	MaSV_ChinhSach int identity(1,1) primary key,

	MaSinhVien varchar(12),
	MaDienChinhSach int,
	Constraint fk_SinhVien Foreign Key (MaSinhVien) references SinhVien(MaSinhVien),
	Constraint fk_DienChinhSach Foreign Key (MaDienChinhSach) references DienChinhSach(MaDienChinhSach),
)