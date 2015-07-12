GO
--Bảng giá trị hành động
CREATE TABLE dbo.GiaTriHoatDong (
	MaHoatDong INT NOT NULL,
	--1 Hoạt động có thể thay đổi nhiều giá trị
	--GiaTri dùng để xác định
	GiaTri NVARCHAR(20) NOT NULL,
	GiaTriCu NVARCHAR(MAX) NOT NULL,
	GiaTriMoi NVARCHAR(MAX) NOT NULL
	PRIMARY KEY(MaHoatDong, GiaTri)
)

GO
--Thêm giá trị hành động
ALTER PROC dbo.ThemGiaTriHoatDong (
	@0 INT, --Mã hoạt động
	@1 NVARCHAR(MAX), --Giá trị
	@2 NVARCHAR(MAX), --Giá trị cũ
	@3 NVARCHAR(MAX) --Giá trị mới
)
AS
BEGIN
	INSERT INTO dbo.GiaTriHoatDong (MaHoatDong, GiaTri, GiaTriCu, GiaTriMoi)
		VALUES (@0, @1, @2, @3)
END

GO
--Lấy giá trị hoạt động
ALTER PROC dbo.layGiaTriHoatDongTheoMaHoatDong (
	@0 INT --Mã hoạt động
)
AS
BEGIN
	SELECT *
		FROM dbo.GiaTriHoatDong
		WHERE MaHoatDong = @0
END