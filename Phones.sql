--CREATE DATABASE MobileWords
------ccccccccccccccccc---------
create table tblSuppliers
(
	SupplierID    int primary key  IDENTITY(1,1),
	CompanyName  nvarchar(250)	Not null, 
	ContactName nvarchar(30) not null,
	Address nvarchar(250),
	Phone char(10) Not null,
	Email char(30),
	Description	nvarchar(250)
);

create table tblCategories
(
	CategoryID    int primary key  IDENTITY(1,1),
	CategoryName  nvarchar(30)	Not null, 
	Description	nvarchar(250)
);

create table tblTrademarks
(
	TrademarkID    int primary key  IDENTITY(1,1),
	TrademarkName  nvarchar(30)	Not null, 
	Description	nvarchar(250)
);

create table tblCustomers 
(
	CustomerID    int primary key  IDENTITY(1,1),
	CustomerName  nvarchar(30)	Not null, 
	Address  nvarchar(50)	Not null, 
	Phone char(10) Not null,
	Email char(30),
	Identification char(12),
	Description	nvarchar(250)
);

create table tblUsers
(
	UserID    int primary key  IDENTITY(1,1),
	UserName  char(30)	Not null, 
	Password  char(30)	Not null,	
	FullName  nvarchar(30)   Not null,
	Identification char(12) Not null,
	Phone	  nvarchar(10)	Not null,
	Address  nvarchar(50)	Not null,
	Email	  nvarchar(30),
	BirthYear date Not null,
	Status	  int	        Not null,
	Description	nvarchar(250)
);

create table tblProducts
(
	ProductID    int primary key  IDENTITY(1,1),
	CategoryID    int Not null,
	ProductName nvarchar(50) Not null,
	TrademarkID  int Not null, 
	Year char(10) Not null,
	GuaranteeDay int Not null,
	MadeIn nvarchar(30) Not null,
	Price money Not null,
	Description	nvarchar(250)
	FOREIGN KEY (CategoryID) REFERENCES tblCategories  (CategoryID),
	FOREIGN KEY (TrademarkID) REFERENCES tblTrademarks  (TrademarkID),
);

create table tblColors
(
	ColorID    int primary key  IDENTITY(1,1),
	Color nvarchar(50) Not null,
	Description	nvarchar(250)
);

create table tblProductColors
(	
	ProductID    int,
	ColorID    int,
	Description	nvarchar(250)
	primary key(ProductID, ColorID),
	FOREIGN KEY (ColorID) REFERENCES tblColors  (ColorID),
	FOREIGN KEY (ProductID) REFERENCES tblProducts  (ProductID),
);

create table tblInsurances
(
	InsuranceID    char(10) primary key,
	UserID    int Not null,
	CustomerID  int Not null, 
	InsuranceDay datetime Not null,
	Description	nvarchar(250)
	FOREIGN KEY (UserID) REFERENCES tblUsers  (UserID),
	FOREIGN KEY (CustomerID) REFERENCES tblCustomers  (CustomerID),
);

create table tblInsuranceDetails
(
	InsuranceID    char(10),
	ProductID    int,
	Details nvarchar(350) Not null,
	Description	nvarchar(250),
	primary key (InsuranceID, ProductID),
	FOREIGN KEY (InsuranceID) REFERENCES tblInsurances  (InsuranceID),
	FOREIGN KEY (ProductID) REFERENCES tblProducts  (ProductID),
);

create table tblOrders
(
	OrderID    char(10) primary key,
	UserID    int Not null,
	SupplierID int Not null,
	OrderDate  Datetime Not null,
	Description	nvarchar(250),
	FOREIGN KEY (UserID) REFERENCES tblUsers  (UserID),
	FOREIGN KEY (SupplierID) REFERENCES tblSuppliers  (SupplierID),
);


create table tblOrderDetails
(
	OrderID    char(10),
	ProductID    int,
	Quantity int Not null,
	Description	nvarchar(250),
	primary key (OrderID, ProductID),
	FOREIGN KEY (OrderID) REFERENCES tblOrders  (OrderID),
	FOREIGN KEY (ProductID) REFERENCES tblProducts  (ProductID),
);

create table tblRentals 
(
	RentalID    char(10) primary key,
	UserID    int Not null,
	CustomerID int Not null,
	RentalDate datetime not null,
	Discount float,
	Description	nvarchar(250),
	FOREIGN KEY (UserID) REFERENCES tblUsers  (UserID),
	FOREIGN KEY (CustomerID) REFERENCES tblCustomers  (CustomerID),
);

create table tblRentalDetails 
(
	RentalID    char(10),
	ProductID    int,
	Quantity int Not null,
	Description	nvarchar(250),
	primary key (RentalID, ProductID),
	FOREIGN KEY (RentalID) REFERENCES tblRentals  (RentalID),
	FOREIGN KEY (ProductID) REFERENCES tblProducts  (ProductID),
);

INSERT INTO tblSuppliers VALUES
(N'Hệ Thống Phân Phối Chính Hãng Xiaomi', N'Công Ty TNHH', N'261 Lê Lợi, P. Lê Lợi, Q. Ngô Quyền,Tp. Hải Phòng', '0888888261', 'info@mihome.vn', 'NULL'),
(N'Văn Phòng Đại Diện - Công Ty Nokia', N'Công Ty Nokia', N'Phòng 703, Tầng7, Tòa Nhà Metropolitan, 235 Đồng Khởi, P. Bến Nghé, Q. 1,Tp. Hồ Chí Minh (TPHCM)', '38245123', 'chau.nguyen@nokia.com', 'NULL'),
(N'Công Ty TNHH Công Nghệ Di Động Mai Nguyên', N'Mai Nguyên', N'117 Nguyễn Bỉnh Khiêm, P. Đa Kao, Q. 1,Tp. Hồ Chí Minh (TPHCM)', '39100332', 'mainguyen117@gmail.com', 'NULL'),
(N'Công Ty TNHH Bao La', N'Công Ty TNHH Bao La', N'8/38 Đinh Bô Lĩnh, P.24, Q. Bình Thạnh,Tp. Hồ Chí Minh (TPHCM)', '0909577752', 'ngoc0909577752@gmail.com', 'NULL')


SELECT * FROM tblCategories

INSERT INTO tblCategories VALUES
(N'Điện thoại', NULL),
(N'Sạc, cáp sạc', NULL),
(N'Pin, sạc dự phòng', NULL),
(N'Ốp lưng bảo vệ', NULL),
(N'Giá đỡ điện thoại', NULL),
(N'Gậy chụp ảnh, Gimbal', NULL),
(N'Tai nghe', NULL),
(N'Miếng dán màn hình', NULL)


SELECT * FROM tblTrademarks

INSERT INTO tblTrademarks VALUES
(N'Apple', N'Apple đã tạo ra một làn sóng cách mạng, thay đổi hoàn toàn công nghệ và thúc đẩy thế giới phát triển'),
(N'Samsung', N'Samsung là một tập đoàn công nghệ lớn của Hàn Quốc và luôn duy trì vị trí thương hiệu hàng đầu châu Á kể từ năm 2012.'),
(N'Xiaomi', N'Xiaomi là hãng điện thoại Trung Quốc nổi tiếng được thành lập vào năm 2010. Hiện nay, Xiaomi đang là công ty điện thoại thông minh lớn thứ 3 trên thế giới.'),
(N'Oppo', N'Oppo đang cho thử nghiệm mẫu smartphone camera selfie ẩn dưới màn hình. Nếu thành công, hãng điện thoại này sẽ là người đầu tiên trong xu hướng thiết kế toàn màn hình trong tương lai gần.'),
(N'Huawei', N'là một công ty đa quốc gia đến từ Trung Quốc có vai trò là nhà sản xuất các thiết bị điện thoại di động cho chính thương hiệu mình. Huawei cũng từng là đối tác lớn của các hãng điện tử khác trên thế giới.'),
(N'Realme', N'Realme là “ông lớn” mới nổi trên thị trường điện thoại di động toàn cầu. Được biết, thương hiệu này được chính thức thành lập vào 06/05/2018 sau khi tách ra khỏi Oppo.'),
(N'Lenovo', N'Lenovo là một hãng điện thoạiphổ thông với mức giá hợp lý và cấu hình cao.'),
(N'Motorola', N'Motorola là một trong những hãng điện thoại lâu đời của nước Mỹ và từng rất phổ biến tại thị trường Việt Nam trong những năm trước đây'),
(N'TECNO', N'TECNO là hãng điện thoại cao cấp do tập đoàn Trassions Holdings sáng lập vào năm 2006 tại Hồng Kông'),
(N'Vivo', N'Vivo cũng là một hãng điện thoại lớn đến từ Trung Quốc. Thành lập vào năm 2009, đây là công ty điện thoại thông minh phát triển nhanh nhất trên thế giới có dấu ấn toàn cầu mạnh mẽ với phạm vi rộng lớn.')

SELECT * FROM tblCustomers

INSERT INTO tblCustomers VALUES
(N'Nguyễn Hữu Nhật', N'Nghệ An', '0396864749', 'nhatnh@vinhuni.edu.vn', '1871881888', NULL),
(N'Hồ Anh Hòa', N'Hà Tĩnh', '0902922003', 'hoaha@vinhuni.edu.vn', '1882883334', NULL),
(N'Tôn Ngộ Không', N'Đào Hoa', '0000000000', 'ngoko@gmail.com', '1882993884', NULL),
(N'Đường Tăng', N'Thập Lý', '0999500506', 'dt@gmail.com', '188888888', NULL),
(N'Sa Tăng', N'Nghệ An', '0396864749', 'nhatnh@vinhuni.edu.vn', '1871881888', NULL),
(N'Trư Bắt Giới', N'Hà Tĩnh', '0902922003', 'hoaha@vinhuni.edu.vn', '1882883334', NULL),
(N'Hello kity', N'Đào Hoa', '0000000000', 'ngoko@gmail.com', '1882993884', NULL),
(N'Doraemon', N'Thập Lý', '0999500506', 'dt@gmail.com', '188888888', NULL)


SELECT * FROM tblUsers

INSERT INTO tblUsers VALUES
('nhatnh', '123',N'Nguyễn Hữu Nhật', '1878878888', '0396864749', N'Nghệ An', 'nhatnh@vinhuni.edu.vn', '2002-09-22', 1, N'Là một trong những thành viên của Team Mobile Word cần cù thì bù siêng năng'),
('hoaha', '123',N'Hồ Anh Hòa', '1878879999', '0396869999', N'Hà Tĩnh', 'hoaha@vinhuni.edu.vn', '2002-01-20', 1, N'Là một trong những thành viên của Team Mobile Word cần cù thì bù siêng năng'),
('nghiadn', '123',N'Nguyễn Đình Nghĩa', '1878877777', '0396866666', N'Nghệ An', 'nghiadn@vinhuni.edu.vn', '2002-09-09', 1, N'Là một trong những thành viên của Team Mobile Word cần cù thì bù siêng năng'),
('manh123', '123',N'Lê Văn Mạnh', '1878878888', '0396864749', N'Nghệ An', 'nhatnh@vinhuni.edu.vn', '2002-09-22', 1, N'Là một trong những thành viên của Team Mobile Word cần cù thì bù siêng năng'),
('a', 'a', N'Nguyễn Văn An', '1878879999', '0396869999', N'Hà Tĩnh', 'hoaha@vinhuni.edu.vn', '2002-01-20', 1, N'Là một trong những thành viên của Team Mobile Word cần cù thì bù siêng năng'),
('hong123', '123', N'Nguyễn Đình Hồng', '1878877777', '0396866666', N'Nghệ An', 'nghiadn@vinhuni.edu.vn', '2002-09-09', 1, N'Là một trong những thành viên của Team Mobile Word cần cù thì bù siêng năng'),
('cuong121', '123', N'Hồ Bá Cường', '1878878888', '0396864749', N'Nghệ An', 'nhatnh@vinhuni.edu.vn', '2002-09-22', 1, N'Là một trong những thành viên của Team Mobile Word cần cù thì bù siêng năng'),
('huyen1', '123', N'Chu Ngọc Huyền', '1878879999', '0396869999', N'Hà Tĩnh', 'hoaha@vinhuni.edu.vn', '2002-01-20', 1, N'Là một trong những thành viên của Team Mobile Word cần cù thì bù siêng năng'),
('long333', '123', N'Bùi Văn Long', '1878877777', '0396866666', N'Nghệ An', 'nghiadn@vinhuni.edu.vn', '2002-09-09', 1, N'Là một trong những thành viên của Team Mobile Word cần cù thì bù siêng năng'),
('cuong99', '123', N'Tạ Công Cường', '1878878888', '0396864749', N'Nghệ An', 'nhatnh@vinhuni.edu.vn', '2002-09-22', 1, N'Là một trong những thành viên của Team Mobile Word cần cù thì bù siêng năng'),
('lan', '123', N'Đào Thu Lan', '1878879999', '0396869999', N'Hà Tĩnh', 'hoaha@vinhuni.edu.vn', '2002-01-20', 1, N'Là một trong những thành viên của Team Mobile Word cần cù thì bù siêng năng'),
('thuong98', '123', N'Nguyễn Thị Thương', '1878877777', '0396866666', N'Nghệ An', 'nghiadn@vinhuni.edu.vn', '2002-09-09', 1, N'Là một trong những thành viên của Team Mobile Word cần cù thì bù siêng năng')


select * from tblCategories
select * from tblTrademarks
SELECT * FROM tblProducts

INSERT INTO tblProducts VALUES
(1, N'Samsung Galaxy S22 Ultra 5G', 2, '2022', 24, N'Trung Quốc', 30990000, NULL),
(1, N'Samsung Galaxy A03', 2, '2022', 24, N'Trung Quốc', 2890000, NULL),
(1, N'Samsung Galaxy A52s 5G 128GB', 2, '2019', 24, N'Trung Quốc', 9490000, NULL),
(1, N'Samsung Galaxy Z Fold3 5G', 2, '2022', 18, N'Trung Quốc', 41990000, NULL),
(1, N'Samsung Galaxy Note 20', 2, '2022', 12, N'Trung Quốc', 15990000, NULL),
(1, N'Samsung Galaxy S21 5G', 2, '2022', 24, N'Trung Quốc', 13990000, NULL),
(1, N'Samsung Galaxy A53 5G 128GB', 2, '2019', 48, N'Trung Quốc', 9790000, NULL),
(1, N'iPhone 13 Pro Max', 1, '2019', 24, N'Mỹ', 30990000, NULL),
(1, N'iPhone 12 Pro Max', 1, '2019', 24, N'Mỹ', 28990000, NULL),
(1, N'iPhone 12 mini', 1, '2020', 24, N'Mỹ', 18990000, NULL),
(1, N'iPhone SE (2022)', 1, '2022', 24, N'Mỹ', 12990000, NULL),
(1, N'iPhone XR 128GB', 1, '2021', 24, N'Mỹ', 13490000, NULL),
(1, N'Xiaomi Redmi Note 11S series', 3, '2021', 24, N'Trung Quốc', 6490000, NULL),
(1, N'Xiaomi Redmi Note 10S', 3, '2020', 24, N'Trung Quốc', 5990000, NULL),
(1, N'Xiaomi 11 Lite 5G NE', 3, '2021', 12, N'Trung Quốc', 9490000, NULL),
(3, N'Xiaomi Power Bank 3 Ultra Compact', 3, '2021', 12, N'Trung Quốc', 711000, NULL),
(3, N'Xiaomi VXN4286GL', 3, '2019', 6, N'Trung Quốc', 240000, NULL),
(3, N'Xiaomi Mi Wireless Power Bank Essential', 3, '2021', 12, N'Trung Quốc', 699000, NULL),
(7, N'Tai nghe Bluetooth True Wireless OPPO ENCO Air', 4, '2021', 12, N'Trung Quốc', 1390000, NULL),
(7, N'Tai nghe Có Dây EP OPPO MH151', 4, '2019', 12, N'Trung Quốc', 590000, NULL);

select * from tblColors
SELECT * FROM tblProducts

INSERT INTO tblColors VALUES
(N'Xanh dương', NULL),
(N'Đỏ', NULL),
(N'Đen', NULL),
(N'Trắng', NULL)

INSERT INTO tblProductColors VALUES
(1, 1, NULL),
(1, 2, NULL),
(1, 3, NULL),
(2, 1, NULL),
(2, 2, NULL),
(3, 2, NULL),
(4, 3, NULL),
(5, 1, NULL),
(6, 2, NULL),
(6, 4, NULL),
(7, 3, NULL),
(7, 1, NULL),
(8, 4, NULL),
(9, 2, NULL),
(9, 4, NULL),
(9, 1, NULL),
(10, 2, NULL),
(10, 4, NULL),
(11, 3, NULL),
(12, 1, NULL),
(12, 4, NULL),
(13, 2, NULL),
(14, 2, NULL),
(15, 3, NULL),
(15, 1, NULL),
(16, 2, NULL),
(16, 4, NULL),
(17, 3, NULL),
(18, 1, NULL),
(19, 2, NULL),
(20, 2, NULL),
(20, 3, NULL);

SELECT * FROM tblInsurances

INSERT INTO tblInsurances VALUES 
('BH01', 1, 4, '2022-01-01', NULL),
('BH02', 2, 3, '2021-02-10', NULL),
('BH03', 1, 2, '2021-02-20', NULL),
('BH04', 3, 2, '2019-05-15', NULL)

SELECT * FROM tblInsuranceDetails

INSERT INTO tblInsuranceDetails VALUES
('BH01', 3, N'Thay thế linh kiện: Thay màn hình', NULL),
('BH02', 2, N'Thay thế linh kiện: Thay pin', NULL),
('BH03', 4, N'Thay thế linh kiện: Thay màn hình', NULL),
('BH04', 1, N'Thay thế linh kiện: Thay camera trước', NULL)

SELECT * FROM tblOrders

INSERT INTO tblOrders VALUES 
('PN01', 1, 2, '2020-01-01', NULL),
('PN02', 2, 3, '2021-11-11', NULL),
('PN03', 3, 2, '2022-10-20', NULL),
('PN04', 1, 1, '2019-02-25', NULL),
('PN05', 3, 1, '2002-11-01', NULL)


SELECT * FROM tblOrderDetails

INSERT INTO tblOrderDetails VALUES 
('PN01', 1, 500, NULL),
('PN01', 2, 300, NULL),
('PN01', 3, 400, NULL),
('PN01', 4, 80, NULL),
('PN01', 5, 50, NULL),
('PN01', 6, 400, NULL),
('PN02', 7, 80, NULL),
('PN02', 8, 50, NULL),
('PN02', 9, 125, NULL),
('PN02', 10, 120, NULL),
('PN03', 11, 50, NULL),
('PN03', 12, 500, NULL),
('PN03', 13, 300, NULL),
('PN03', 14, 400, NULL),
('PN03', 3, 500, NULL),
('PN03', 4, 300, NULL),
('PN03', 5, 400, NULL),
('PN04', 15, 80, NULL),
('PN04', 16, 50, NULL),
('PN04', 17, 400, NULL),
('PN04', 18, 80, NULL),
('PN05', 19, 50, NULL),
('PN05', 20, 400, NULL),
('PN05', 1, 80, NULL),
('PN05', 2, 50, NULL),
('PN05', 15, 50, NULL),
('PN05', 17, 400, NULL),
('PN05', 5, 80, NULL),
('PN05', 7, 50, NULL);

SELECT * FROM tblRentals
SELECT * FROM tblCustomers

INSERT INTO tblRentals VALUES 
('PX01', 1, 3, '2022-01-02', 0, NULL),
('PX02', 2, 1, '2022-12-22', 0, NULL),
('PX03', 2, 4, '2021-01-06', 0, NULL),
('PX04', 4, 1, '2021-10-20', 0, NULL)


SELECT * FROM tblRentalDetails

INSERT INTO tblRentalDetails VALUES 
('PX01', 2, 2, NULL),
('PX01', 4, 1, NULL),
('PX02', 11, 1, NULL),
('PX02', 20, 1, NULL),
('PX03', 7, 2, NULL),
('PX03', 1, 1, NULL),
('PX03', 17, 1, NULL),
('PX04', 2, 1, NULL),
('PX04', 4, 2, NULL)

-- SELECT 
SELECT * FROM tblCategories
SELECT * FROM tblCustomers
SELECT * FROM tblInsuranceDetails
SELECT * FROM tblInsurances
SELECT * FROM tblOrderDetails
SELECT * FROM tblOrders
SELECT * FROM tblProducts
SELECT * FROM tblRentalDetails
SELECT * FROM tblRentals
SELECT * FROM tblSuppliers
SELECT * FROM tblTrademarks
SELECT * FROM tblUsers
go

-- Tạo các thủ tục

-- Thủ tục hiển thị
create PROCEDURE DisplayUser
AS
BEGIN
	SELECT * FROM tblUsers
	ORDER BY UserID ASC
END
go

-- Thủ tục xóa
create PROCEDURE DeleteUser @UserID int
AS
BEGIN
	DELETE FROM tblUsers WHERE UserID = @UserID
END

SELECT * FROM tblUsers
go

-- Thủ tục thêm mới dữ liệu
CREATE PROCEDURE AddUser
	@UserName nvarchar(20), @Password nvarchar(30), @FullName nvarchar(35), @Identification char(20), @Phone char(11),
	@Address nvarchar(40), @Email char(30),  @BirthYear date, @Status int, @Description nvarchar(250)	   	
AS
BEGIN
	INSERT INTO tblUsers(UserName, Password , FullName , Identification, Phone , Address, Email,  BirthYear, Status, Description)
	VALUES (@UserName, @Password , @FullName , @Identification, @Phone, @Address, @Email,  @BirthYear, @Status, @Description)
END
go

-- Thủ tục Update tblUsers
CREATE PROCEDURE UpdateUser
	@UserID int, @UserName nvarchar(20), @Password nvarchar(30), @FullName nvarchar(35), @Identification char(20), @Phone char(11),
	@Address nvarchar(40), @Email char(30),  @BirthYear date, @Status int, @Description nvarchar(250)	 
AS
BEGIN
	UPDATE tblUsers
	SET UserName = @UserName,
		Password = @Password, 
		FullName = @FullName, 
		Identification = @Identification, 
		Phone = @Phone, 
		Address = @Address, 
		Email = @Email, 
		BirthYear = @BirthYear, 
		Status = @Status, 
		Description = @Description
	WHERE UserID = @UserID
END
SELECT * FROM tblUsers
go
--Thủ tục delete tblOrders
CREATE PROCEDURE Delete_tblOrders @s_OrderID int
as
begin
	delete tblOrderDetails where OrderID = @s_OrderID
	delete tblOrders where OrderID = @s_OrderID
end
go

--Thủ tục xóa sản phẩm.
create PROCEDURE Delete_tblProducts @s_ProductID int
as
begin
	delete tblOrderDetails where ProductID = @s_ProductID
	delete tblRentalDetails where ProductID = @s_ProductID
	delete tblInsuranceDetails where ProductID = @s_ProductID
	delete tblProductColors where ProductID = @s_ProductID
	delete tblProducts where ProductID = @s_ProductID
end
go

--Thủ tục xem số lượng tồn kho, form thống kê
create proc tt_TonKho --@s_number int
as
begin
		select vt.ProductName, c.CategoryName, t.TrademarkName, sum(Quantity) as TongXuat into #bt
		from tblProducts vt
			left join tblRentalDetails ctpx on ctpx.ProductID = vt.ProductID
			inner join tblCategories c on c.CategoryID = vt.CategoryID
			inner join tblTrademarks t on t.TrademarkID = vt.TrademarkID
		group by vt.ProductName, c.CategoryName, t.TrademarkName
		update #bt
		set TongXuat = 0
		where TongXuat is Null

		select vt.ProductName, c.CategoryName, t.TrademarkName, sum(Quantity) as TongNhap into #bt1
		from tblProducts vt
			left join tblOrderDetails ctpx on ctpx.ProductID = vt.ProductID
			inner join tblCategories c on c.CategoryID = vt.CategoryID
			inner join tblTrademarks t on t.TrademarkID = vt.TrademarkID
		group by vt.ProductName, c.CategoryName, t.TrademarkName

		select #bt.ProductName, #bt.CategoryName, #bt.TrademarkName, TongNhap-TongXuat as TonKho
		from #bt inner join #bt1 on #bt1.ProductName = #bt.ProductName
		--where (TongNhap-TongXuat) 
		order by (TongNhap-TongXuat) asc

		drop table #bt
		drop table #bt1
end
exec tt_TonKho 200
go

---- Thủ tục Xem tổng sô lượng bán theo ngày.
create proc tt_SumBan @s_DayStart char(10), @s_DayEnd char(10)
as
begin
	select sum(Quantity) from tblRentalDetails rd
	inner join tblRentals r on r.RentalID = rd.RentalID
	where r.RentalDate between @s_DayStart and @s_DayEnd
end
select * from tblRentals
exec tt_SumBan '2017-1-1','2024-12-22'
go

----Xem tổng doanh thu
select sum(p.Price*rd.Quantity) from tblRentalDetails rd  inner join tblProducts p on p.ProductID = rd.ProductID
go

---- Thủ tục Xem tổng doanh thu theo ngày
create proc tt_SumDoanhThu @s_DayStart char(10), @s_DayEnd char(10)
as
begin
	select sum(p.Price*rd.Quantity) 
	from tblRentalDetails rd  
		inner join tblProducts p on p.ProductID = rd.ProductID
		inner join tblRentals r on r.RentalID = rd.RentalID
	where r.RentalDate between @s_DayStart and @s_DayEnd
end
select * from tblRentals
exec tt_SumDoanhThu '2017-1-1','2021-12-22'
go

--Thủ tục tính lợi nhuận theo ngày tùy chọn, form thống kê
create proc tt_SumLoiNhuan @s_DayStart char(10), @s_DayEnd char(10)
as
begin
		select sum(p.Price * rd.Quantity) as TongTienXuat into #bt
		from tblRentalDetails rd  
			inner join tblProducts p on p.ProductID = rd.ProductID
			inner join tblRentals r on r.RentalID = rd.RentalID
		where r.RentalDate between @s_DayStart and @s_DayEnd

		declare @s_TongTienXuat money
		select @s_TongTienXuat = TongTienXuat from #bt
		ALTER TABLE #bt
		ADD ID int;
		Update #bt
		set ID = 1
		where TongTienXuat = @s_TongTienXuat

		---------------------------------------------------------------------------------

		select sum((p.Price - p.Price * 0.2 ) * rd.Quantity) as TongTienNhap into #bt1
		from tblRentalDetails rd  
			inner join tblProducts p on p.ProductID = rd.ProductID
			inner join tblRentals r on r.RentalID = rd.RentalID
		where r.RentalDate between @s_DayStart and @s_DayEnd and p.ProductID in (select rd.ProductID from tblRentalDetails rd)

		declare @s_TongTienNhap money
		select @s_TongTienNhap = TongTienNhap from #bt1
		ALTER TABLE #bt1
		ADD ID int;
		Update #bt1
		set ID = 1
		where TongTienNhap = @s_TongTienNhap

		--------------------------------------------------------------------------------------------

		select TongTienXuat - TongTienNhap
		from #bt inner join #bt1 on #bt1.ID = #bt.ID

		--------------------------------------------------------------------------------------------

		drop table #bt
		drop table #bt1
end
exec tt_SumLoiNhuan '2019-1-1','2024-12-22'
go

--Thủ tục tính lợi nhuận, form thống kê
create proc tt_LoiNhuan
as
begin
		select sum(p.Price * rd.Quantity) as TongTienXuat into #bt
		from tblRentalDetails rd  
			inner join tblProducts p on p.ProductID = rd.ProductID

		declare @s_TongTienXuat money
		select @s_TongTienXuat = TongTienXuat from #bt
		ALTER TABLE #bt
		ADD ID int;
		Update #bt
		set ID = 1
		where TongTienXuat = @s_TongTienXuat

		---------------------------------------------------------------------------------

		select sum((p.Price - p.Price * 0.2 ) * rd.Quantity) as TongTienNhap into #bt1
		from tblRentalDetails rd  
			inner join tblProducts p on p.ProductID = rd.ProductID
		where p.ProductID in (select rd.ProductID from tblRentalDetails rd)

		declare @s_TongTienNhap money
		select @s_TongTienNhap = TongTienNhap from #bt1
		ALTER TABLE #bt1
		ADD ID int;
		Update #bt1
		set ID = 1
		where TongTienNhap = @s_TongTienNhap

		--------------------------------------------------------------------------------------------

		select TongTienXuat - TongTienNhap
		from #bt inner join #bt1 on #bt1.ID = #bt.ID

		--------------------------------------------------------------------------------------------

		drop table #bt
		drop table #bt1
end
exec tt_LoiNhuan
go

-----Thủ tục xem sản phẩm bán chạy
create proc tt_BanChay @s_DayStart char(10) = null, @s_DayEnd char(10) = null
as
begin
	if (@s_DayStart is null and @s_DayEnd is null)
		begin
			select p.ProductName, sum(rd.Quantity) as SLBan into #bt
			from tblProducts p
				inner join tblRentalDetails rd on rd.ProductID = p.ProductID
			group by p.ProductName

			select top(20) PERCENT *
			from #bt
			order by SLBan desc

			drop table #bt
		end
	else
		begin
			select p.ProductName, sum(rd.Quantity) as SLBan into #bt1
			from tblProducts p
				inner join tblRentalDetails rd on rd.ProductID = p.ProductID
				inner join tblRentals r on r.RentalID = rd.RentalID
			where r.RentalDate between @s_DayStart and @s_DayEnd
			group by p.ProductName

			select top(20) PERCENT *
			from #bt1
			order by SLBan desc

			drop table #bt1
		end
end
exec tt_BanChay '2022-1-1','2024-12-22'
go

-----Thủ tục xem sản phẩm bán được ít
create proc tt_BanIt  @s_DayStart char(10) = null, @s_DayEnd char(10) = null
as
begin
	if(@s_DayStart is null and @s_DayEnd is null)
		begin
			select p.ProductName, sum(rd.Quantity) as SLBan into #bt
			from tblProducts p
				inner join tblRentalDetails rd on rd.ProductID = p.ProductID
			group by p.ProductName

			select top(20) PERCENT *
			from #bt
			order by SLBan asc

			drop table #bt
		end
	else
		begin
			select p.ProductName, sum(rd.Quantity) as SLBan into #bt1
			from tblProducts p
				inner join tblRentalDetails rd on rd.ProductID = p.ProductID
				inner join tblRentals r on r.RentalID = rd.RentalID
			where r.RentalDate between @s_DayStart and @s_DayEnd
			group by p.ProductName

			select top(20) PERCENT *
			from #bt1
			order by SLBan asc

			drop table #bt1
		end
end
exec tt_BanIt '2022-1-1','2024-12-22'
go


-----Thủ tục xem thương hiệu ưa chuộng
create proc tt_ThuongHieu @s_DayStart char(10) = null, @s_DayEnd char(10) = null
as
begin
	if(@s_DayStart is null and @s_DayEnd is null)
		begin
			select t.TrademarkName, sum(rd.Quantity) as SLBan into #bt
			from tblProducts p
				inner join tblRentalDetails rd on rd.ProductID = p.ProductID
				inner join tblTrademarks t on t.TrademarkID = p.TrademarkID
			group by t.TrademarkName

			select top(20) PERCENT *
			from #bt
			order by SLBan desc

			drop table #bt
		end
	else
		begin
			select t.TrademarkName, sum(rd.Quantity) as SLBan into #bt1
			from tblProducts p
				inner join tblRentalDetails rd on rd.ProductID = p.ProductID
				inner join tblTrademarks t on t.TrademarkID = p.TrademarkID
				inner join tblRentals r on r.RentalID = rd.RentalID
			where r.RentalDate between @s_DayStart and @s_DayEnd
			group by t.TrademarkName

			select top(20) PERCENT *
			from #bt1
			order by SLBan desc

			drop table #bt1
		end
end
exec tt_ThuongHieu '2022-1-1','2024-12-22'
go

--Thủ tục delete tblRental
CREATE PROCEDURE DeleteRental @s_RentalID char(10)
AS
BEGIN
	DELETE tblRentalDetails WHERE RentalID = @s_RentalID
	DELETE tblRentals WHERE RentalID = @s_RentalID
END
go

----Thủ tục nạp hóa đơn bán hàng
create proc tt_Nap_HDBanHang @s_RentalID char(10)
as
begin
	select rd.RentalID, rd.ProductID, rd.Quantity, p.Price, Price*Quantity as ThanhTien
	from tblProducts p
		inner join tblRentalDetails rd on rd.ProductID = p.ProductID
		inner join tblRentals r on r.RentalID = rd.RentalID
	where rd.RentalID = @s_RentalID
end
exec tt_Nap_HDBanHang 'PX01'
go


------Thủ tục cập nhật hóa đơn bán hàng-------------
create proc tt_Update_HDBanHang @s_RentalID char(10), @s_UserID int, 
			@s_CustomerID int, @s_RentalDate datetime, @s_Discount float, @s_Description nvarchar(250)
as
begin
	update tblRentals
	set UserID = @s_UserID, CustomerID = @s_CustomerID,
	RentalDate = @s_RentalDate, Discount = @s_Discount, Description = @s_Description
	where RentalID = @s_RentalID
end
go


-------Thủ tục Insert Chi tiết hóa đơn bán hàng. tblRentalDetails
create proc tt_Insert_tblRentalDetails @RentalID char(10), @ProductID int, @Quantity int--, @Description nvarchar(250)
as
begin
	insert into tblRentalDetails(RentalID, ProductID, Quantity) 
	values (@RentalID, @ProductID, @Quantity)
end
go


-----Thủ tục xóa khách hàng
create proc tt_Delete_tblCustomer @CustomerID int
as
begin
	delete tblInsuranceDetails
	from tblInsuranceDetails id inner join tblInsurances i on i.InsuranceID = id.InsuranceID
	where i.CustomerID = @CustomerID
	delete tblInsurances
	where CustomerID = @CustomerID

	delete tblRentalDetails
	from tblRentalDetails id inner join tblRentals i on i.RentalID = id.RentalID
	where i.CustomerID = @CustomerID
	delete tblRentals
	where CustomerID = @CustomerID

	delete tblCustomers
	where CustomerID = @CustomerID
end
go

--Thủ tục xem chi tiết hóa đơn nhập
create proc tt_Nap_HDNhapHang @s_OrderID char(10)
as
begin
	select rd.OrderID, rd.ProductID, rd.Quantity, rd.Description, p.Price, Price*Quantity as ThanhTien
	from tblProducts p
		inner join tblOrderDetails rd on rd.ProductID = p.ProductID
		inner join tblOrders r on r.OrderID = rd.OrderID
	where rd.OrderID = @s_OrderID
end
exec tt_Nap_HDNhapHang 'PN01'
go

--Thủ tục xóa tblRental
CREATE PROCEDURE DeleteOrder @s_OrderID char(10)
AS
BEGIN
	DELETE tblOrderDetails WHERE OrderID = @s_OrderID
	DELETE tblOrders WHERE OrderID = @s_OrderID
END
go


------Thủ tục cập nhật hóa đơn bán hàng-------------
create proc tt_Update_HDNhapHang @s_OrderID char(10), @s_UserID int, 
			@s_SupplierID int, @s_OrderDate datetime, @s_Description nvarchar(250)
as
begin
	update tblOrders
	set UserID = @s_UserID, SupplierID = @s_SupplierID,
	OrderDate= @s_OrderDate, Description = @s_Description
	where OrderID = @s_OrderID
end
go

select p.ProductName, rd.Quantity, rd.Description 
from tblRentalDetails rd inner join tblProducts p on p.ProductID = rd.ProductID
where rd.RentalID = 'PX01'
go


--Thủ tục xem chi tiết hóa đơn nhập
create proc tt_Nap_HDBaoHanh @s_InsuranceID char(10)
as
begin
	select rd.InsuranceID, rd.ProductID, rd.Details, rd.Description, p.Price
	from tblProducts p
		inner join tblInsuranceDetails rd on rd.ProductID = p.ProductID
		inner join tblInsurances r on r.InsuranceID = rd.InsuranceID
	where rd.InsuranceID = @s_InsuranceID
end
exec tt_Nap_HDBaoHanh 'BH03'
go

--Thủ tục xóa tblRental
CREATE PROCEDURE DeleteInsurance @s_InsuranceID char(10)
AS
BEGIN
	DELETE tblInsuranceDetails WHERE InsuranceID = @s_InsuranceID
	DELETE tblInsurances WHERE InsuranceID = @s_InsuranceID
END
go


------Thủ tục cập nhật hóa đơn bán hàng-------------
create proc tt_Update_HDBaoHanh @s_InsuranceID char(10), @s_UserID int, 
			@s_CustomerID int, @s_InsuranceDate datetime, @s_Description nvarchar(250)
as
begin
	update tblInsurances
	set UserID = @s_UserID, CustomerID = @s_CustomerID,
	InsuranceDay= @s_InsuranceDate, Description = @s_Description
	where InsuranceID = @s_InsuranceID
end
go

select *
from tblTrademarks
order by TrademarkName asc

select *
from tblCustomers

SELECT p.ProductID, p.ProductName FROM tblProducts p
inner join tblRentalDetails rd on rd.ProductID = p.ProductID
inner join tblRentals r on r.RentalID = rd.RentalID
inner join tblCustomers c on c.CustomerID = r.CustomerID
where c.CustomerName = N'Phan Hà Vy'
Order By p.ProductName

SELECT * FROM tblCustomers c
inner join tblRentals r on r.CustomerID = c.CustomerID
--Order By CustomerName
go

----Thủ tục in hóa đơn bán hàng.
create proc tt_In_HDBanHang @RentalID char(10)
as
begin
	select c.* ,rd.RentalID, p.ProductName, rd.Quantity, p.Price, Price*Quantity as ThanhTien
	from tblRentalDetails rd
		inner join tblProducts p on p.ProductID = rd.ProductID
		inner join tblRentals r on r.RentalID = rd.RentalID
		inner join tblCustomers c on c.CustomerID = r.CustomerID
	where rd.RentalID = @RentalID
end
exec tt_In_HDBanHang 'PX05'
go

----Thủ tục in hóa đơn nhập hàng.
create proc tt_In_HDNhapHang @OrderID char(10)
as
begin
	select u.FullName, c.CompanyName, c.Address, c.Phone , rd.OrderID, p.ProductName, rd.Quantity, p.Price, Price * Quantity as ThanhTien
	from tblOrderDetails rd
		inner join tblProducts p on p.ProductID = rd.ProductID
		inner join tblOrders r on r.OrderID = rd.OrderID
		inner join tblSuppliers c on c.SupplierID = r.SupplierID
		inner join tblUsers u on u.UserID = r.UserID
	where rd.OrderID = @OrderID
end
exec tt_In_HDNhapHang 'PN02'
go

----Thủ tục in hóa đơn bảo hành.
create proc tt_In_HDBaoHanh @InsuranceID char(10)
as
begin
	select u.FullName, c.CustomerName, c.Address, c.Phone , rd.InsuranceID, p.ProductName, rd.Details
	from tblInsuranceDetails rd
		inner join tblProducts p on p.ProductID = rd.ProductID
		inner join tblInsurances r on r.InsuranceID = rd.InsuranceID
		inner join tblCustomers c on c.CustomerID = r.CustomerID
		inner join tblUsers u on u.UserID = r.UserID
	where rd.InsuranceID = @InsuranceID
end
exec tt_In_HDBaoHanh 'BH01'
go