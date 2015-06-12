use rtcmfraf_Moodle;

GO
--Người dùng
CREATE TABLE dbo.NguoiDung (
	Ma INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
	TenTaiKhoan NVARCHAR(MAX) NOT NULL,
	MatKhau NVARCHAR(MAX) NOT NULL,
	Email NVARCHAR(MAX) NOT NULL,
	HoTen NVARCHAR(MAX) NOT NULL,
	NgaySinh DATETIME,
	DiaChi NVARCHAR(MAX),
	SoDienThoai NVARCHAR(MAX),
	MaHinhDaiDien INT
);

GO
--Thêm người dùng
CREATE PROC dbo.themNguoiDung (
	@0 NVARCHAR(MAX), --Tên tài khoản
	@1 NVARCHAR(MAX), --Mật khẩu
	@2 NVARCHAR(MAX), --Email
	@3 NVARCHAR(MAX), --Họ và tên
	@4 DATETIME, --Ngày Sinh
	@5 NVARCHAR(MAX), --Địa chỉ
	@6 NVARCHAR(MAX), --Số điện thoại
	@7 INT --Hình đại diện
)
AS
BEGIN
	INSERT INTO dbo.NguoiDung(TenTaiKhoan, MatKhau, Email, HoTen, NgaySinh, DiaChi, SoDienThoai, MaHinhDaiDien) VALUES (@0, @1, @2, @3, @4, @5, @6, @7);

	SELECT @@IDENTITY Ma
END

GO
--Lấy người dùng
CREATE PROC dbo.layNguoiDungTheoTenTaiKhoan (
	@0 NVARCHAR(MAX) --Tên tài khoản
)
AS
BEGIN
	SELECT *
	FROM dbo.NguoiDung
	WHERE TenTaiKhoan = @0
END

select * from dbo.NguoiDung

GO
--Lấy người dùng theo mã
CREATE PROC dbo.layNguoiDungTheoMa (
	@0 INT --Mã người dùng
)
AS
BEGIN
	SELECT *
	FROM dbo.NguoiDung
	WHERE Ma = @0
END

GO
--Lấy người dùng theo từ khóa
ALTER PROC dbo.layNguoiDungTheoTuKhoa (
	@0 NVARCHAR(MAX) --Từ khóa
)
AS
BEGIN
	SELECT 
		Ma,
		HoTen
		FROM dbo.nguoiDung
		WHERE 
			HoTen LIKE '%' + REPLACE(@0, ' ', '%') + '%'
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
			ND.HoTen
			FROM
				dbo.NguoiDung ND 
					INNER JOIN dbo.NhomNguoiDung_' + @0 + '_NguoiDung NND_ND
						ON 
							NND_ND.MaNhomNguoiDung = ' + @1 + ' AND
							ND.Ma = NND_ND.MaNguoiDung
	')
END