CREATE TABLE Readers (
    MaDocGia nvarchar(50) NOT NULL PRIMARY KEY,
    HoTen nvarchar(255),
    NgaySinh date,
    DiaChi nvarchar(255),
    Email nvarchar(255),
    SoDienThoai nvarchar(20),
    LoaiDocGia nvarchar(50) CHECK (LoaiDocGia IN ('Học sinh/Sinh viên', 'Giáo viên', 'Khách')),
    NgayLapThe date NOT NULL,
    NgayHetHan date,
    TongNo decimal(18, 2),
    CONSTRAINT CK_NgaySinh_NgayLapThe CHECK (NgaySinh < NgayLapThe),
    CONSTRAINT CK_NgayLapThe_NgayHetHan CHECK (NgayLapThe < NgayHetHan)
);

INSERT INTO Readers (MaDocGia, HoTen, NgaySinh, DiaChi, Email, SoDienThoai, LoaiDocGia, NgayLapThe, NgayHetHan, TongNo) 
VALUES ('23521284', N'Nguyễn Cao Quang', '2005-04-28', 'abcxyz', 'rinquanq05@gmail.com', '0917031134', N'Học sinh/Sinh viên', '2024-10-1', '2025-10-1', 0);
GO

select * from Readers;