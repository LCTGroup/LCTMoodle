use rtcmfraf_Moodle;

GO
--Tạo bài viết diễn đàn
CREATE TABLE dbo.BaiVietDienDan(
	Ma INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
	TieuDe NVARCHAR(MAX) NOT NULL,
	NoiDung NVARCHAR(MAX) NOT NULL,
	MaTapTin INT,
	ThoiDiemTao DATETIME DEFAULT GETDATE() NOT NULL,
	MaNguoiTao INT NOT NULL,
	MaKhoaHoc INT NOT NULL,
	Ghim BIT
)

GO
--Thêm bài viết diễn đàn
ALTER PROC dbo.themBaiVietDienDan(
	@0 NVARCHAR(MAX), --TieuDe
	@1 NVARCHAR(MAX), --NoiDung
	@2 INT, --MaTapTin
	@3 INT, --MaNguoiTao
	@4 INT --MaKhoaHoc
)
AS
BEGIN
	INSERT INTO dbo.BaiVietDienDan(TieuDe, NoiDung, MaTapTin, MaNguoiTao, MaKhoaHoc)
		VALUES (@0, @1, @2, @3, @4)

	SELECT 
		Ma,
		TieuDe,
		NoiDung,
		MaTapTin,
		ThoiDiemTao,
		MaNguoiTao,
		MaKhoaHoc
		FROM dbo.BaiVietDienDan
		WHERE Ma = @@IDENTITY
END

GO
--Lấy bài viết diễn đàn theo mã khóa học
ALTER PROC dbo.layBaiVietDienDanTheoMaKhoaHoc (
	@0 INT --MaKhoaHoc
)
AS
BEGIN
	SELECT 
		Ma,
		TieuDe,
		NoiDung,
		MaTapTin,
		ThoiDiemTao,
		MaNguoiTao,
		MaKhoaHoc,
		Ghim
		FROM dbo.BaiVietDienDan
		WHERE MaKhoaHoc = @0
		ORDER BY Ghim DESC, ThoiDiemTao DESC
END

GO
--Xóa bài viết diễn đàn theo mã
CREATE PROC dbo.xoaBaiVietDienDanTheoMa (
	@0 INT --Ma
)
AS
BEGIN
	DELETE FROM dbo.BaiVietDienDan
		WHERE Ma = @0
END

GO
--Lấy bài viết diễn đàn theo mã
ALTER PROC dbo.layBaiVietDienDanTheoMa (
	@0 INT --Ma
)
AS
BEGIN
	SELECT 
		Ma,
		TieuDe,
		NoiDung,
		MaTapTin,
		ThoiDiemTao,
		MaNguoiTao,
		MaKhoaHoc
		FROM dbo.BaiVietDienDan
		WHERE Ma = @0
END

GO
--Cập nhật theo mã
ALTER PROC dbo.capNhatBaiVietDienDanTheoMa (
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
			UPDATE dbo.BaiVietDienDan
				SET ' + @query + '
				WHERE Ma = ' + @0 + '
		')
	END	
	
	SELECT TOP 1
		Ma,
		TieuDe,
		NoiDung,
		MaTapTin,
		ThoiDiemTao,
		MaNguoiTao,
		MaKhoaHoc,
		Ghim
		FROM dbo.BaiVietDienDan
		WHERE Ma = @0
END

GO
--Cập nhật ghim bài viết
CREATE PROC dbo.capNhatBaiVietDienDanTheoMa_Ghim (
	@0 INT, --Ma
	@1 BIT --Ghim
)
AS
BEGIN
	UPDATE dbo.BaiVietDienDan
		SET Ghim = @1
		WHERE Ma = @0
END

GO
--Xóa tất cả ghim theo mã khóa học
CREATE PROC dbo.capNhatBaiVietDienDanTheoMaKhoaHoc_XoaGhim (
	@0 INT --MaKhoaHoc
)
AS
BEGIN
	UPDATE dbo.BaiVietDienDan
		SET Ghim = null
		WHERE MaKhoaHoc = @0
END