Create Database QL_DogilyPetShop
Go
Use QL_DogilyPetShop
Go

CREATE TABLE [dbo].[KhachHang]
(
    [makh] CHAR(10),
    [tenkh] NVARCHAR (30) NOT NULL,
    [diachi] NVARCHAR (100),
    [dthoai] VARCHAR (20),
    CONSTRAINT [PK_KhachHang] PRIMARY KEY ([makh])
)

CREATE TABLE [dbo].[ChucVu]
(
	[macv] INT,
	[tencv] NVARCHAR(20),
	CONSTRAINT [PK_Quyen] PRIMARY KEY ([macv])
)

CREATE TABLE [dbo].[NguoiDung]
(
    [mand] CHAR(10),
    [tennd] VARCHAR (50) NOT NULL,
    [dchi] NVARCHAR (50),
    [dienthoai] VARCHAR (50),
    [macv] INT,
    [ngsinh] DATE,
    [password] VARCHAR (50),
    CONSTRAINT [PK_NguoiDung] PRIMARY KEY ([mand]),
	Constraint [FK_NguoiDung_ChucVu] Foreign Key ([macv]) References [ChucVu]([macv]),
	CONSTRAINT [UNIQUE_NguoiDung] UNIQUE (tennd),
)

CREATE TABLE [dbo].[NhaCungCap]
(
	[mancc] CHAR(10),
	[tenncc] NVARCHAR(30) NOT NULL,
	[dchincc] NVARCHAR(50) NOT NULL,
	[dthoaincc] CHAR(12) NOT NULL,
	CONSTRAINT [PK_NhaCC] PRIMARY KEY ([mancc])
)

CREATE TABLE [dbo].[LoaiPet]
(
	[maloai] INT,
	[tenloai] NVARCHAR(30) NOT NULL,
	CONSTRAINT [PK_LoaiPet] PRIMARY KEY ([maloai]),
	CONSTRAINT [UNIQUE_LoaiPet] UNIQUE ([tenloai])
)

CREATE TABLE [dbo].[GiongPet]
(
	[magiong] INT,
	[tengiong] NVARCHAR(30) NOT NULL,
	CONSTRAINT [PK_GiongPet] PRIMARY KEY ([magiong]),
	CONSTRAINT [UNIQUE_GiongPet] UNIQUE ([tengiong])
)

CREATE TABLE [dbo].[PhieuNhap]
(
	[mapn] CHAR(10),
	[ngaynhap] DATE NOT NULL DEFAULT GETDATE(),
	[mancc] CHAR(10),
	[mand] CHAR(10),
	[tongtien] MONEY DEFAULT 0,
	CONSTRAINT [PK_PhieuNhap] PRIMARY KEY ([mapn]),
	Constraint [FK_PhieuNhap_NhaCC] Foreign Key ([mancc]) References [NhaCungCap]([mancc]),
	Constraint [FK_PhieuNhap_NguoiDung] Foreign Key ([mand]) References [NguoiDung]([mand])
)

CREATE TABLE [dbo].[SanPham]
(
    [masp] CHAR(10),
    [tensp] NVARCHAR (50) NOT NULL,
    [magiong] INT NOT NULL,
    [maloai] INT NOT NULL,
    [soluongton] INT DEFAULT 0,
    CONSTRAINT [PK_SanPham] PRIMARY KEY CLUSTERED ([masp]),
	Constraint FK_SanPham_LoaiPet Foreign Key ([maloai]) References [LoaiPet]([maloai]),
	Constraint FK_SanPham_GiongPet Foreign Key ([magiong]) References [GiongPet]([magiong])
)

CREATE TABLE [dbo].[CTPhieuNhap]
(
	[mapn] CHAR(10),
	[masp] CHAR(10),
	[soluong] INT,
	[gianhap] DECIMAL (18, 2) NOT NULL,
	[thanhtien] MONEY DEFAULT 0,
	Constraint [PK_ChiTietPN] Primary Key ([mapn],[masp]),
	Constraint [FK_CTPN_PhieuNhap] Foreign Key ([mapn]) References [PhieuNhap]([mapn]),
	Constraint [FK_CTPN_SanPham] Foreign Key ([masp]) References [SanPham]([masp])
)

Create Table [dbo].[HoaDon]
(
	[mahd] INT IDENTITY (1, 1),
	[transno] VARCHAR(15),
	[masp] CHAR(10),
	[soluong] INT,
	[gia] DECIMAL (18, 2),
	[thanhtien] MONEY DEFAULT 0,
	[makh] CHAR(10),
	[thungan] VARCHAR(20) NOT NULL,
	CONSTRAINT [PK_HoaDon] PRIMARY KEY ([mahd]),
	CONSTRAINT [FK_HoaDon_KhachHang] Foreign Key ([makh]) References [KhachHang]([makh]),
	CONSTRAINT [FK_HoaDon_SanPham] Foreign Key ([masp]) References [SanPham]([masp])
)

Create Table DonGia
(
	[masp] CHAR(10),
	[giaban] MONEY,
	CONSTRAINT [PK_DonGia] PRIMARY KEY ([masp]),
	Constraint [FK_DonGia_SanPham] Foreign Key ([masp]) References SanPham([masp])
)

/*----------------------------------------------------Trigger------------------------------------------------------------------*/
/*Tính thành tiền khi nhập liệu vào CTHoaDon*/
Go
Create trigger thanhtien_HD on HoaDon
For Insert,Update
As
	Update HoaDon
	Set thanhtien = (Select soluong*gia)
Go

/*Trigger khi insert CTPhieuNhap*/
Create Trigger Trigger_Insert_CTPN on CTPhieuNhap
After Insert
As
	/*Tính thành tiền*/
	Update CTPhieuNhap
	Set thanhtien = (Select soluong*gianhap)

	/*Update lại soluongton = 0 khi nó = null */
	Update SanPham
	Set soluongton = 0
	Where soluongton is NULL

	/*Update lại tongtien = 0 khi nó = null*/
	Update PhieuNhap
	Set tongtien = 0
	Where tongtien is NULL

	/*Update soluongton SanPham bằng số lượng hiện tại cộng với số lượng vừa nhập vào CTPhieuNhap*/
	Update SanPham
	Set soluongton = soluongton  + (Select soluong From inserted)
	Where SanPham.masp = (Select masp From inserted)

	/*Update tongtien của mỗi phiếu nhập bằng tổng tiền hiện tại cộng với tiền vừa nhập vào CTPhieuNhap*/
	Update PhieuNhap
	Set tongtien = tongtien + (Select soluong*gianhap From inserted)
	Where PhieuNhap.mapn = (Select mapn from inserted)
Go

/*Trigger khi xóa CTPhieuNhap*/
Create Trigger Update_SLSP on CTPhieuNhap
For Delete
As
	Update SanPham
	Set soluongton = 0
	Where soluongton is NULL

	Update PhieuNhap
	Set tongtien = 0
	Where tongtien is NULL

	Update SanPham
	Set soluongton = soluongton - (Select Sum(soluong) From deleted)
	Where SanPham.masp = (Select masp From deleted)

	Update PhieuNhap
	Set tongtien = tongtien - (Select soluong*gianhap From deleted)
	Where PhieuNhap.mapn = (Select mapn from deleted)
Go

/*Trigger cập nhật số lượng mua của mỗi sản phẩm khi chọn giống nhau*/
Create Trigger UpdateSL_SameMaSP on HoaDon
After Insert
As
	/*Sau khi nhập 1 dòng dữ liệu nếu xuất hiện trên 1 dòng dữ liệu giống nhau*/
	/*Trigger sẽ Update soluong của HoaDon có masp và transno giống với masp và transno vừa nhập*/
	if (Select Count(*) From HoaDon Where masp = (Select masp From inserted) and transno = (Select transno From inserted)) > 1
		Update HoaDon
		Set soluong = soluong + (Select soluong from inserted)
		Where masp = (Select masp From inserted) and transno = (Select transno From inserted)

	/*Xóa những dòng hóa đơn trùng*/
	Delete from HoaDon
    where mahd in (select TOP((Select Count(*) From HoaDon Where masp = (Select masp From inserted) and transno = (Select transno From inserted)) - 1) mahd 
	From HoaDon
	Where masp = (Select masp From inserted) and transno = (Select transno From inserted) Order by soluong)
Go

SET DATEFORMAT DMY

/*Nhập khách hàng*/
Insert Into KhachHang
Values('KH001','Duong Van Tu','Thu Duc','0978063224')
Insert Into KhachHang
Values('KH002','Le Loi','Tan Phu','0378163224')
Insert Into KhachHang
Values('KH003','Tran Anh Tuan','Binh Thanh','0394521002')
Insert Into KhachHang
Values('KH004','Ta Phi Long','Thu Duc','0379964782')
Insert Into KhachHang
Values('KH005','Le Huynh Yen Nhi','Di An','0972001201')

/*Nhập chức vụ*/
Insert into ChucVu
Values(1,N'Quản trị viên')
Insert into ChucVu
Values(2,N'Thu ngân')
Insert into ChucVu
Values(3,N'Nhân viên')

/*Nhập người dùng*/
Insert Into NguoiDung
Values('ND001','CTruong',N'Bình Tân','0339961120',1,'28/04/2002','truong')
Insert Into NguoiDung
Values('ND002','QMinh',N'Tân Phú','0339541120',2,'29/10/2002','minh')
Insert Into NguoiDung
Values('ND003','TBinh',N'Thủ Dức','0311254556',3,'20/01/2002','binh')

/*Nhập nhà cung cấp*/
Insert Into NhaCungCap
Values('NCC001','Trạm cứu hộ chó mèo CPAPS','Ha Noi','0834524650'),
      ('NCC002','Nhóm cứu hộ động vật SAR','Quan 10','0371163225'),
	  ('NCC003','Hội yêu động vật (YDV)','Hue','0394511202'),
	  ('NCC004','Cứu Trợ Động Vật Đà Nẵng','Da Nang','0979964582'),
	  ('NCC005','Trạm cứu hộ chó mèo Nha Trang','Nha Trang','0372001401')

/*Nhập loại pet*/
Insert Into LoaiPet
Values(1,N'Chó')
Insert Into LoaiPet
Values(2,N'Mèo')

/*Nhập giống pet*/
Insert Into GiongPet
Values(1,N'Chó ta')
Insert Into GiongPet
Values(2,N'Chó tây')
Insert Into GiongPet
Values(3,'Mèo ta')
Insert Into GiongPet
Values(4,'Mèo tây')

/*Nhập phiếu nhập*/
Insert Into PhieuNhap(mapn,mancc,mand)
Values('PN001','NCC001','ND001')
Insert Into PhieuNhap(mapn,mancc,mand)
Values('PN002','NCC002','ND001')
Insert Into PhieuNhap(mapn,mancc,mand)
Values('PN003','NCC003','ND001')
Insert Into PhieuNhap(mapn,mancc,mand)
Values('PN004','NCC001','ND001')

/*Nhập sản phẩm*/
Insert Into SanPham(masp,tensp,magiong,maloai)
Values('SP001',N'Chó Bắc Hà',1,1)
Insert Into SanPham(masp,tensp,magiong,maloai)
Values('SP002',N'Chó Lài',1,1)

Insert Into SanPham(masp,tensp,magiong,maloai)
Values('SP003',N'Chó HUSKY',2,1)
Insert Into SanPham(masp,tensp,magiong,maloai)
Values('SP004',N'Chó SIBA INU',2,1)
Insert Into SanPham(masp,tensp,magiong,maloai)
Values('SP005',N'Chó GOLDEN',2,1)
Insert Into SanPham(masp,tensp,magiong,maloai)
Values('SP006',N'Chó CORGI',2,1)

Insert Into SanPham(masp,tensp,magiong,maloai)
Values('SP007',N'Mèo Mướp',3,2)
Insert Into SanPham(masp,tensp,magiong,maloai)
Values('SP008',N'Mèo Vàng',3,2)
Insert Into SanPham(masp,tensp,magiong,maloai)
Values('SP009',N'Mèo Xiêm',3,2)

Insert Into SanPham(masp,tensp,magiong,maloai)
Values('SP010',N'Mèo Anh Lông Ngắn',4,2)
Insert Into SanPham(masp,tensp,magiong,maloai)
Values('SP011',N'Mèo Anh Ragdoll',4,2)
Insert Into SanPham(masp,tensp,magiong,maloai)
Values('SP012',N'Mèo Tai Cụp',4,2)

/*Nhập đơn giá*/
Insert Into DonGia
Values('SP001', 1600000)
Insert Into DonGia
Values('SP002', 1600000)
Insert Into DonGia
Values('SP003', 2100000)
Insert Into DonGia
Values('SP004', 2300000)
Insert Into DonGia
Values('SP005', 2200000)
Insert Into DonGia
Values('SP006', 2000000)
Insert Into DonGia
Values('SP007', 650000)
Insert Into DonGia
Values('SP008', 620000)
Insert Into DonGia
Values('SP009', 1100000)
Insert Into DonGia
Values('SP010', 1200000)
Insert Into DonGia
Values('SP011', 1150000)
Insert Into DonGia
Values('SP012', 1450000)

/*Nhập chi tiết phiếu nhập*/
Insert Into CTPhieuNhap(mapn,masp,soluong,gianhap)
Values('PN001','SP001',10,2500000)
Insert Into CTPhieuNhap(mapn,masp,soluong,gianhap)
Values('PN001','SP002',10,1500000)
Insert Into CTPhieuNhap(mapn,masp,soluong,gianhap)
Values('PN002','SP003',10,1000000)
Insert Into CTPhieuNhap(mapn,masp,soluong,gianhap)
Values('PN002','SP011',10,1000000)
Insert Into CTPhieuNhap(mapn,masp,soluong,gianhap)
Values('PN003','SP004',10,1200000)
Insert Into CTPhieuNhap(mapn,masp,soluong,gianhap)
Values('PN003','SP007',10,1500000)
Insert Into CTPhieuNhap(mapn,masp,soluong,gianhap)
Values('PN004','SP008',10,1200000)
Insert Into CTPhieuNhap(mapn,masp,soluong,gianhap)
Values('PN004','SP012',10,1200000)

/*------------------------------------------Procedure--------------------------------------------------------------------------------------*/
/*Thủ tục khi thêm dữ liệu vào CTPhieuNhap*/
Go
Create Proc Insert_CTPhieuNhap @mapn char(10), @masp char(10), @sl int, @gianhap money
As
	Insert Into CTPhieuNhap(mapn,masp,soluong,gianhap)
	Values(@mapn,@masp,@sl,@gianhap)
Go
/*Thêm phiếu nhập*/
Create proc Insert_PN @mapn char(10), @mancc char(10), @mand char(10)
As
	INSERT INTO PhieuNhap(mapn,mancc,mand)
	VALUES(@mapn,@mancc,@mand)
Go
/*Thủ tục xóa 1 phiếu nhập*/
Create Proc Xoa_PN @mapn char(10)
As
	Delete From CTPhieuNhap Where mapn = @mapn
	Delete From PhieuNhap Where mapn = @mapn
Go
/*Xóa chi tiết phiếu nhập*/
Create proc Xoa_CTPN @mapn char(10), @masp char(10)
As
	Delete From CTPhieuNhap Where mapn = @mapn and masp = @masp
Go
/*Sửa sản phẩm*/
Create Proc Update_SanPham @masp char(10), @tensp nvarchar(30), @mg int, @ml int, @slt int, @gia money
As
	Update SanPham
	Set tensp = @tensp, magiong = @mg, maloai = @ml, soluongton = @slt
	Where masp = @masp

	Update DonGia
	Set giaban = @gia 
	Where masp = @masp
Go
/*Thêm sản phẩm*/
Create Proc Insert_SanPham @masp char(10), @tensp nvarchar(30), @mg int, @ml int, @gia money
As
	Insert Into SanPham(masp,tensp,magiong,maloai)
	Values(@masp,@tensp, @mg, @ml)

	Insert Into DonGia
	Values(@masp,@gia)
Go
/*Xóa 1 sản phẩm*/
Create Proc Xoa_SP @masp char(10)
As
	if (Select Count(*) From CTPhieuNhap WHere masp = @masp) > 0
		Commit tran
	Else
		Delete From DonGia Where masp = @masp
		Delete From SanPham Where masp = @masp
Go
/*Show bill (chi tiết những sản phẩm đã chọn mua)*/
Create Proc ShowBill @transno varchar(15)
As
	If (Select TOP(1) makh from HoaDon where transno LIKE @transno) is null
		SELECT DISTINCT mahd, hd.masp, tensp, soluong, gia, thanhtien, 'tenkh' = NULL,thungan
		FROM HoaDon hd, KhachHang kh, SanPham sp
		Where sp.masp = hd.masp and transno LIKE @transno
	Else
		SELECT mahd, hd.masp, tensp, soluong, gia, thanhtien,kh.tenkh,thungan
		FROM HoaDon hd, KhachHang kh, SanPham sp
		Where kh.makh = hd.makh and sp.masp = hd.masp and transno LIKE @transno
Go