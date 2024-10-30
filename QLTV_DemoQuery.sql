create database Demo_QLTV
create table THELOAI
(
	ID int identity(1,1) primary key,
	MaTheLoai as ('TL' + right('00000' + cast(ID as nvarchar(5)), 5)) persisted,
	TenTheLoai nvarchar(100) not null
)
create table TINHTRANG
(
	ID int identity(1,1) primary key,
	MaTheLoai as ('TT' + right('00000' + cast(ID as nvarchar(5)), 5)) persisted,
	TenTinhTrang nvarchar(100) not null
)
create table SACH
(
	ID int identity(1,1) primary key,
	MaSach as ('S' + right('00000' + cast(ID as nvarchar(5)), 5)) persisted,
	TenSach nvarchar(100) not null,
	IDTheLoai int not null,
	foreign key (IDTheLoai) references THELOAI(ID),
	NamXuatBan int not null,
	NhaXuatBan nvarchar not null,
	NgayNhap Date not null,
	TriGia Decimal(18,0) not null,
	IDTinhTrang int not null,
	foreign key(IDTinhTrang) references TINHTRANG(ID)
)

create table TACGIA
(
	ID int identity(1,1) primary key,
	MaTacGia as('TG' + right('00000' + cast(ID as nvarchar(5)), 5)) persisted,
	TenTacGia nvarchar(100) not null,
)
create table CHITIETSACH
(
	IDSACH int not null,
	IDTACGIA int not null,
	primary key(IDSACH, IDTACGIA),
	foreign key (IDSACH) references SACH(ID) on delete cascade,
	foreign key(IDTACGIA) references TACGIA(ID) on delete cascade
)
create table LOAIDOCGIA
(
	ID int identity(1,1) primary key,
	MaLoaiDocGia as ('LDG' + right('00000' + cast(ID as nvarchar(5)), 5)) persisted,
	TenLoaiDocGia nvarchar(100)
)
create table DOCGIA
(
	ID int identity(1,1) primary key,
	MaDocGia as ('DG' + right('00000' + cast(ID as nvarchar(5)), 5)) persisted,
	HoTen nvarchar(150) not null,
	NgaySinh nvarchar(150) not null,
	DiaChi nvarchar(15) not null,
	Email nvarchar(100) not null, 
	NgayLapThe Date not null,
	NgayHetHan Date not null,
	CONSTRAINT chk_NgayLapThe_NgayHetHan CHECK (NgayLapThe < NgayHetHan),
	IDLoaiDocGia int not null,
	foreign key (IDLoaiDocGia) references LOAIDOCGIA(ID) on delete cascade,
	IDAccount int not null,
	SDT varchar(50) not null,
	TongNo Decimal(18,0) not null
)






