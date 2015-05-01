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
	SoDienThoai NVARCHAR(MAX)
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
	@6 NVARCHAR(MAX) --Số điện thoại
)
AS
BEGIN
	INSERT INTO dbo.NguoiDung(TenTaiKhoan, MatKhau, Email, HoTen, NgaySinh, DiaChi, SoDienThoai) VALUES (@0, @1, @2, @3, @4, @5, @6);

	SELECT @@IDENTITY Ma
END

GO
--Lấy người dùng
CREATE PROC dbo.layNguoiDung (
	@0 NVARCHAR(MAX), --Tên tài khoản
	@1 NVARCHAR(MAX) --Mật khẩu
)
AS
BEGIN
	
END