use rtcmfraf_Moodle;

GO
--Người dùng
CREATE TABLE dbo.NguoiDung 
(
	Ma INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
	TenTaiKhoan NVARCHAR(MAX) NOT NULL,
	MatKhau NVARCHAR(MAX) NOT NULL,
	Email NVARCHAR(MAX) NOT NULL,
	GioiTinh INT DEFAULT 0, 
	Ho NVARCHAR(MAX) NOT NULL,
	TenLot NVARCHAR(MAX),
	Ten NVARCHAR(MAX) NOT NULL,
	NgaySinh DATETIME,
	DiaChi NVARCHAR(MAX),
	SoDienThoai NVARCHAR(MAX),
	MaHinhDaiDien INT,
	CoQuyenHT BIT DEFAULT 0,
	CoQuyenCD BIT DEFAULT 0,
	DaKichHoat BIT DEFAULT 0,
	MaKichHoat NVARCHAR(MAX),
	MatKhauCap2 NVARCHAR(MAX)
)

GO
--Thêm người dùng
ALTER PROC dbo.themNguoiDung 
(
	@0 NVARCHAR(MAX), --Tên tài khoản
	@1 NVARCHAR(MAX), --Mật khẩu
	@2 NVARCHAR(MAX), --Email
	@3 INT, --Giới tính
	@4 NVARCHAR(MAX), --Họ
	@5 NVARCHAR(MAX), --Tên lót
	@6 NVARCHAR(MAX), --Tên
	@7 DATETIME, --Ngày Sinh
	@8 NVARCHAR(MAX), --Địa chỉ
	@9 NVARCHAR(MAX), --Số điện thoại
	@10 INT, --Hình đại diện
	@11 NVARCHAR(MAX), --Mã kích hoạt
	@12 NVARCHAR(MAX) --Mật khẩu cấp 2
)
AS
BEGIN
	INSERT INTO dbo.NguoiDung(TenTaiKhoan, MatKhau, Email, GioiTinh, Ho, TenLot, Ten, NgaySinh, DiaChi, SoDienThoai, MaHinhDaiDien, MaKichHoat, MatKhauCap2) 
	VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12);

	SELECT @@IDENTITY Ma
END

GO
--Cập nhật Người Dùng theo mã
CREATE PROC dbo.capNhatNguoiDungTheoMa 
(
	@0 INT, --Mã
	@1 dbo.BangCapNhat READONLY
)
AS
BEGIN
	--Tạo chuỗi gán
	DECLARE @query NVARCHAR(MAX) = dbo.taoChuoiCapNhat(@1)
	IF (@query <> '')
	BEGIN
		EXEC('
		UPDATE dbo.NguoiDung
			SET ' + @query + '
			WHERE Ma = ' + @0 + '
		')
	END	
END

GO
--Cập nhật kích hoạt Người dùng theo tenTaiKhoan
ALTER PROC dbo.capNhatNguoiDungTheoTenTaiKhoan_KichHoat
(
	@0 NVARCHAR(MAX), --Tên tài khoản
	@1 NVARCHAR(MAX) --Giá trị kích hoạt
)
AS
BEGIN
	UPDATE dbo.NguoiDung
	SET MaKichHoat = @1
	WHERE TenTaiKhoan = @0
END

GO
--Lấy người dùng theo mã
CREATE PROC dbo.layNguoiDungTheoMa 
(
	@0 INT --Mã người dùng
)
AS
BEGIN
	SELECT *
	FROM dbo.NguoiDung
	WHERE Ma = @0
END

GO
--Lấy người dùng theo Tên Tài Khoản
CREATE PROC dbo.layNguoiDungTheoTenTaiKhoan 
(
	@0 NVARCHAR(MAX) --Tên tài khoản
)
AS
BEGIN
	SELECT *
	FROM dbo.NguoiDung
	WHERE TenTaiKhoan = @0
END

GO
--Lấy người dùng theo Email
CREATE PROC dbo.layNguoiDungTheoEmail
(
	@0 NVARCHAR(MAX) --Email
)
AS
BEGIN
	SELECT *
	FROM dbo.NguoiDung
	WHERE Email = @0
END

GO
--Lấy người dùng theo từ khóa
ALTER PROC dbo.layNguoiDung_TimKiem 
(
	@0 NVARCHAR(MAX) --Từ khóa
)
AS
BEGIN
	SELECT 
		Ma,
		Ho,
		Ten
		FROM dbo.NguoiDung
		WHERE 
			Ho + ' ' + Ten LIKE '%' + REPLACE(@0, ' ', '%') + '%'
END

GO
--Lấy người dùng theo mã khóa học và từ khóa (Chiêu)
ALTER PROC dbo.layNguoiDungTheoMaKhoaHoc_TimKiem (
	@0 INT, --MaKhoaHoc
	@1 NVARCHAR(MAX) --Từ khóa
)
AS
BEGIN
	SELECT
		ND.Ma,
		ND.Ho,
		ND.Ten
		FROM 
			dbo.KhoaHoc_NguoiDung KH_ND INNER JOIN
				dbo.NguoiDung ND ON
					KH_ND.MaKhoaHoc = @0 AND
					KH_ND.MaNguoiDung = ND.Ma AND
					ND.Ho + ' ' + ND.Ten LIKE '%' + REPLACE(@1, ' ', '%') + '%'
END

GO
--Lấy người dùng theo mã nhóm người dùng
ALTER PROC dbo.layNguoiDungTheoMaNhomNguoiDung (
	@0 NVARCHAR(MAX), --PhamVi
	@1 INT --MaNhomNguoiDung
)
AS
BEGIN
	EXEC('
		SELECT
			ND.Ma,
			ND.Ho,
			ND.Ten
			FROM
				dbo.NguoiDung ND 
					INNER JOIN dbo.NhomNguoiDung_' + @0 + '_NguoiDung NND_ND
						ON 
							NND_ND.MaNhomNguoiDung = ' + @1 + ' AND
							ND.Ma = NND_ND.MaNguoiDung
	')
END
