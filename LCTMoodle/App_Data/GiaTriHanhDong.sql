GO
--Bảng giá trị hành động
CREATE TABLE dbo.GiaTriHoatDong
(
	MaHoatDong INT NOT NULL,
	GiaTri NVARCHAR(100) NOT NULL,
	GiaTriCu NVARCHAR(MAX) NOT NULL,
	GiaTriMoi NVARCHAR(MAX) NOT NULL
	PRIMARY KEY(MaHoatDong, GiaTri)
)

GO
--Thêm giá trị hành động
ALTER PROC dbo.ThemGiaTriHoatDong
(
	@0 INT, --Mã hoạt động
	@1 NVARCHAR(MAX), --Giá trị cũ
	@2 NVARCHAR(MAX), --Giá trị mới
	@3 NVARCHAR(MAX) --Giá trị
)
AS
BEGIN
	INSERT INTO dbo.GiaTriHoatDong
	VALUES (@0, @1, @2, @3)
END

GO
--Lấy giá trị hoạt động
CREATE PROC dbo.layGiaTriHoatDongTheoMaHoatDong
(
	@0 INT --Mã hoạt động
)
AS
BEGIN
	SELECT *
	FROM dbo.GiaTriHoatDong
	WHERE MaHoatDong = @0
END