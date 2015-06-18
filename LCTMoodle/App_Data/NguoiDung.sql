﻿use rtcmfraf_Moodle;

GO
--Người dùng
CREATE TABLE dbo.NguoiDung (
	Ma INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
	TenTaiKhoan NVARCHAR(MAX) NOT NULL,
	MatKhau NVARCHAR(MAX) NOT NULL,
	Email NVARCHAR(MAX) NOT NULL,
	Ho NVARCHAR(MAX),
	Ten NVARCHAR(MAX),
	NgaySinh DATETIME,
	DiaChi NVARCHAR(MAX),
	SoDienThoai NVARCHAR(MAX),
	MaHinhDaiDien INT,
	CoQuyenHT BIT DEFAULT 0
);

GO
--Thêm người dùng
ALTER PROC dbo.themNguoiDung (
	@0 NVARCHAR(MAX), --Tên tài khoản
	@1 NVARCHAR(MAX), --Mật khẩu
	@2 NVARCHAR(MAX), --Email
	@3 NVARCHAR(MAX), --Họ
	@4 NVARCHAR(MAX), --Tên
	@5 DATETIME, --Ngày Sinh
	@6 NVARCHAR(MAX), --Địa chỉ
	@7 NVARCHAR(MAX), --Số điện thoại
	@8 INT --Hình đại diện
)
AS
BEGIN
	INSERT INTO dbo.NguoiDung(TenTaiKhoan, MatKhau, Email, Ho, Ten, NgaySinh, DiaChi, SoDienThoai, MaHinhDaiDien) VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8);

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
CREATE PROC dbo.layNguoiDung_TimKiem (
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
