create database Demo_QLTV

create table NGUOIDUNG
(
UserID int identity(1,1) primary key,
Username varchar(50) not null, 
Password varchar(50) not null,
Status bit not null,
Role varchar(50),
)
create table SACH
(
	id int identity(1,1) primary key,
	bid as ('S' + right('00000' + cast(id as nvarchar(5)), 5)) persisted,
	bName nvarchar(150) not null,
	bPubl nvarchar(150) not null,
	bPDate varchar(250) not null,
	bPrice bigint not null,
	bQuan bigint not null,
	bStatus bit not null
)
CREATE TRIGGER trg_Update_bStatus
ON SACH
AFTER INSERT, UPDATE
AS
BEGIN
    UPDATE SACH
    SET bStatus = CASE WHEN s.bQuan > 0 THEN 1 ELSE 0 END
    FROM SACH s
    INNER JOIN inserted i ON s.bid = i.bid;
END;
create table TACGIA
(
	id int identity(1,1) primary key,
	aid as('TG' + right('00000' + cast(id as nvarchar(5)), 5)) persisted,
	aName nvarchar(150) not null,
)
create table SACH_TACGIA
(
	bookid int not null,
	authorid int not null,
	primary key(bookid, authorid),
	foreign key (bookid) references SACH(id) on delete cascade,
	foreign key(authorid) references TACGIA(id) on delete cascade
)
create table DOCGIA
(
	id int identity(1,1) primary key,
	rid as ('DG' + right('00000' + cast(id as nvarchar(5)), 5)) persisted,
	rName nvarchar(150) not null,
	rBOD nvarchar(150) not null,
	rAdd nvarchar(15) not null,
)
create table PMUON
(
	id int identity(1,1) primary key,
	pmid as ('PM' + right('00000' + cast(id as nvarchar(5)), 5)) persisted,
	bookbid int not null,
	readerbid int not null,
	pmRedate nvarchar(150) not null,
	pmBodate nvarchar(150) not null,
	foreign key(bookbid) references SACH(id) on delete cascade,
	foreign key(readerbid) references DOCGIA(id) on delete cascade 
)

alter table PMUON
add constraint

select SACH.id, bName, bPubl, bPDate, bPrice, bQuan, aName from
SACH
join SACH_TACGIA on SACH_TACGIA.bookid = SACH.id
join TACGIA on TACGIA.id = SACH_TACGIA.authorid


