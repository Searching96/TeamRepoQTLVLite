CREATE DATABASE QLTVLite;
DROP TABLE SACH;
--CREATE TABLE [dbo].[SACH](
--	[MaSach] [varchar](50) NOT NULL,
--	[TenSach] [nvarchar](100) NOT NULL,
--	[MaTheLoai] [varchar](50) NOT NULL,
--	[NamXuatBan] [int] NOT NULL,
--	[NhaXuatBan] [nvarchar](100) NULL,
--	[MaTacGia] [varchar](50) NOT NULL,
--	[NgayNhap] [date] NOT NULL,
--	[TriGia] [decimal](18, 0) NOT NULL,
--	[MaTinhTrang] [varchar](50) NOT NULL,
-- CONSTRAINT [PK_SACH] PRIMARY KEY CLUSTERED 
--(
--	[MaSach] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
--) ON [PRIMARY]

DROP TABLE SACH;
--CREATE TABLE SACH
--(
--    ID INT IDENTITY(1,1) PRIMARY KEY,
--    MaSach AS ('S' + RIGHT('00000' + CAST(ID AS NVARCHAR(5)), 5) ) PERSISTED,
--    TenSach NVARCHAR(50) NOT NULL,
--    TacGia NVARCHAR(50) NOT NULL,
--    TheLoai NVARCHAR(50) NOT NULL,
--    NamXuatBan INT NOT NULL
--)

-- User
DROP TABLE NGUOIDUNG;
CREATE TABLE NGUOIDUNG
(
	ID INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung AS ('ND' + RIGHT('00000' + CAST(ID AS NVARCHAR(5)), 5)) PERSISTED,
	TenNguoiDung NVARCHAR(50) NOT NULL,
	MatKhau VARCHAR(20) NOT NULL,
	PhanQuyen INT NOT NULL,
)

INSERT INTO NGUOIDUNG (TenNguoiDung, MatKhau, PhanQuyen) VALUES ('admin', 'admin123', 1);
INSERT INTO NGUOIDUNG (TenNguoiDung, MatKhau, PhanQuyen) VALUES ('thuthu', 'thuthu123', 2);
INSERT INTO NGUOIDUNG (TenNguoiDung, MatKhau, PhanQuyen) VALUES ('docgia', 'docgia123', 3);
INSERT INTO NGUOIDUNG (TenNguoiDung, MatKhau, PhanQuyen) VALUES ('troioilatroi', 'admin123', 1);
-- ('ad', 'ad', 1) -> đi

SELECT * FROM NGUOIDUNG;
-- Loi
--INSERT INTO NGUOIDUNG (TenNguoiDung, MatKhau, PhanQuyen)
--VALUES 
--('ad', 'ad123', 1);

-- Sach co tham chieu tac gia
CREATE TABLE SACH
(
    ID INT IDENTITY(1,1) PRIMARY KEY,
    MaSach AS ('S' + RIGHT('00000' + CAST(ID AS NVARCHAR(5)), 5)) PERSISTED,
    TenSach NVARCHAR(50) NOT NULL,
    TheLoai NVARCHAR(50) NOT NULL,
    NamXuatBan INT NOT NULL
);

CREATE TABLE TACGIA
(
    ID INT IDENTITY(1,1) PRIMARY KEY,
    MaTacGia AS ('TG' + RIGHT('00000' + CAST(ID AS NVARCHAR(5)), 5)) PERSISTED,
    TenTacGia NVARCHAR(100) NOT NULL
);

CREATE TABLE SACH_TACGIA
(
    IDSach INT NOT NULL,
    IDTacGia INT NOT NULL,
    PRIMARY KEY (IDSach, IDTacGia),
    FOREIGN KEY (IDSach) REFERENCES SACH(ID) ON DELETE CASCADE,
    FOREIGN KEY (IDTacGia) REFERENCES TACGIA(ID) ON DELETE CASCADE
);

INSERT INTO SACH (TenSach, TheLoai, NamXuatBan)
VALUES 
('The Godfather', 'Trinh Tham', 1950),
('To Kill a Mockingbird', 'Van Hoc', 1960),
('1984', 'Khoa Hoc Vien Tuong', 1949),
('Pride and Prejudice', 'Tinh Cam', 1813),
('The Great Gatsby', 'Van Hoc', 1925),
('War and Peace', 'Lich Su', 1869),
('The Hobbit', 'Fantasy', 1937),
('Brave New World', 'Khoa Hoc Vien Tuong', 1932);

INSERT INTO TACGIA (TenTacGia)
VALUES 
('Mario Puzo'),
('Harper Lee'),
('George Orwell'),
('Jane Austen'),
('F. Scott Fitzgerald'),
('Leo Tolstoy'),
('J.R.R. Tolkien'),
('Aldous Huxley');

INSERT INTO SACH_TACGIA (IDSach, IDTacGia)
VALUES 
(1, 1),  -- 'The Godfather' - 'Mario Puzo'
(2, 2),  -- 'To Kill a Mockingbird' - 'Harper Lee'
(3, 3),  -- '1984' - 'George Orwell'
(4, 4),  -- 'Pride and Prejudice' - 'Jane Austen'
(5, 5),  -- 'The Great Gatsby' - 'F. Scott Fitzgerald'
(6, 6),  -- 'War and Peace' - 'Leo Tolstoy'
(7, 7),  -- 'The Hobbit' - 'J.R.R. Tolkien'
(8, 8),  -- 'Brave New World' - 'Aldous Huxley'

-- Giả sử có sách với nhiều tác giả
(3, 8);  -- '1984' - 'Aldous Huxley' (Giả định)

INSERT INTO SACH VALUES (00001, 'The Godfather', 'Mario Puzo', 'Trinh Tham', 1950);
INSERT INTO SACH VALUES (00002, 'To Kill a Mockingbird', 'Harper Lee', 'Van Hoc', 1960);
INSERT INTO SACH VALUES (00003, '1984', 'George Orwell', 'Khoa Hoc Vien Tuong', 1949);
INSERT INTO SACH VALUES (00004, 'Pride and Prejudice', 'Jane Austen', 'Tinh Cam', 1813);
INSERT INTO SACH VALUES (00005, 'The Great Gatsby', 'F. Scott Fitzgerald', 'Van Hoc', 1925);
INSERT INTO SACH VALUES (00006, 'Moby Dick', 'Herman Melville', 'Phiêu Lưu', 1851);
INSERT INTO SACH VALUES (00007, 'War and Peace', 'Leo Tolstoy', 'Lich Su', 1869);
INSERT INTO SACH VALUES (00008, 'The Catcher in the Rye', 'J.D. Salinger', 'Thanh Thieu Nien', 1951);
INSERT INTO SACH VALUES (00009, 'The Hobbit', 'J.R.R. Tolkien', 'Fantasy', 1937);
INSERT INTO SACH VALUES (00010, 'Brave New World', 'Aldous Huxley', 'Khoa Hoc Vien Tuong', 1932);
INSERT INTO SACH VALUES (00011, 'Fahrenheit 451', 'Ray Bradbury', 'Khoa Hoc Vien Tuong', 1953);
INSERT INTO SACH VALUES (00012, 'The Picture of Dorian Gray', 'Oscar Wilde', 'Van Hoc', 1890);
INSERT INTO SACH VALUES (00013, 'The Alchemist', 'Paulo Coelho', 'Phat Trien Ban Than', 1988);
INSERT INTO SACH VALUES (00014, 'The Catch-22', 'Joseph Heller', 'Chien Tranh', 1961);
INSERT INTO SACH VALUES (00015, 'Jane Eyre', 'Charlotte Brontë', 'Van Hoc', 1847);
INSERT INTO SACH VALUES (00016, 'Wuthering Heights', 'Emily Brontë', 'Van Hoc', 1847);
INSERT INTO SACH VALUES (00017, 'The Grapes of Wrath', 'John Steinbeck', 'Van Hoc', 1939);
INSERT INTO SACH VALUES (00018, 'The Road', 'Cormac McCarthy', 'Hau Tuong Lai', 2006);
INSERT INTO SACH VALUES (00019, 'The Road to Serfdom', 'Friedrich Hayek', 'Kinh Te', 1944);
INSERT INTO SACH VALUES (00020, 'The Brothers Karamazov', 'Fyodor Dostoevsky', 'Van Hoc', 1880);
INSERT INTO SACH VALUES (00069, 'Brother Mat Hut', 'Mat Hut', 'Van Hoc', 1696);

SELECT * FROM SACH;