GO
--Bảng giá trị hành động
CREATE TABLE dbo.GiaTriHanhDong
(
	MaHanhDong INT NOT NULL,
	GiaTri NVARCHAR(100) NOT NULL,
	GiaTriCu NVARCHAR(MAX) NOT NULL,
	GiaTriMoi NVARCHAR(MAX) NOT NULL
	PRIMARY KEY(MaHanhDong, GiaTri)
)

GO
--Thêm giá trị hành động
CREATE PROC dbo.ThemGiaTriHanhDong
(
	@0 INT, --Mã hành động
	@1 NVARCHAR(MAX), --Giá trị cũ
	@2 NVARCHAR(MAX), --Giá trị mới
	@3 NVARCHAR(MAX) --Giá trị
)
AS
BEGIN
	INSERT INTO dbo.GiaTriHanhDong
	VALUES (@0, @1, @2, @3)
END

GO
--Lấy giá trị hành động
CREATE PROC dbo.layGiaTriHanhDongTheoMaHanhDong
(
	@0 INT --Mã hành động
)
AS
BEGIN
	SELECT *
	FROM dbo.GiaTriHanhDong
	WHERE MaHanhDong = @0
END