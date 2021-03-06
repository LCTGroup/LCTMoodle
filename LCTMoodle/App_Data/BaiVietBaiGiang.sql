﻿use rtcmfraf_Moodle;

GO
--Tạo bài viết bài giảng
CREATE TABLE dbo.BaiVietBaiGiang (
	Ma INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
	TieuDe NVARCHAR(MAX) NOT NULL,
	NoiDung NVARCHAR(MAX) NOT NULL,
	TomTat NVARCHAR(MAX),
	MaTapTin INT,
	ThoiDiemTao DATETIME DEFAULT GETDATE() NOT NULL,
	MaNguoiTao INT NOT NULL,
	MaKhoaHoc INT NOT NULL,
	DanhSachMaThanhVienDaXem VARCHAR(MAX) NOT NULL DEFAULT '|'
)

GO
--Trigger xóa
--Xóa tập tin
CREATE TRIGGER dbo.xoaBaiVietBaiGiang_TRIGGER
ON dbo.BaiVietBaiGiang
AFTER DELETE
AS
BEGIN
	--Xóa tập tin
	DELETE TT
		FROM 
			dbo.TapTin_BaiVietBaiGiang_TapTin TT
				INNER JOIN deleted d ON
					TT.Ma = d.MaTapTin
END

GO
--Thêm bài viết bài giảng
ALTER PROC dbo.themBaiVietBaiGiang(
	@0 NVARCHAR(MAX), --TieuDe
	@1 NVARCHAR(MAX), --NoiDung
	@2 NVARCHAR(MAX), --TomTat
	@3 INT, --MaTapTin
	@4 INT, --MaNguoiTao
	@5 INT --MaKhoaHoc
)
AS
BEGIN
	INSERT INTO dbo.BaiVietBaiGiang(TieuDe, NoiDung, TomTat, MaTapTin, MaNguoiTao, MaKhoaHoc)
		VALUES (@0, @1, @2, @3, @4, @5)

	SELECT *
		FROM dbo.BaiVietBaiGiang
		WHERE Ma = @@IDENTITY
END

GO
--Lấy bài viết bài giảng theo mã khóa học
ALTER PROC dbo.layBaiVietBaiGiangTheoMaKhoaHoc (
	@0 INT --MaKhoaHoc
)
AS
BEGIN
	SELECT *
		FROM dbo.BaiVietBaiGiang
		WHERE MaKhoaHoc = @0
		ORDER BY ThoiDiemTao ASC
END

GO
--Xóa bài viết bài giảng theo mã
ALTER PROC dbo.xoaBaiVietBaiGiangTheoMa (
	@0 INT --Ma
)
AS
BEGIN
	DELETE FROM dbo.BaiVietBaiGiang
		WHERE Ma = @0
END

GO
--Lấy theo mã
ALTER PROC dbo.layBaiVietBaiGiangTheoMa (
	@0 INT --Ma
)
AS
BEGIN
	SELECT *
		FROM dbo.BaiVietBaiGiang
		WHERE Ma = @0
END

GO
--Cập nhật theo mã
ALTER PROC dbo.capNhatBaiVietBaiGiangTheoMa (
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
		UPDATE dbo.BaiVietBaiGiang
			SET ' + @query + '
			WHERE Ma = ' + @0 + '
		')
	END	
	
	SELECT *
		FROM dbo.BaiVietBaiGiang
		WHERE Ma = @0
END

GO
--Cập nhật đã xem bài viết
CREATE PROC dbo.capNhatBaiVietBaiGiangTheoMa_Xem (
	@0 INT, --Ma
	@1 INT --MaNguoiDung
)
AS
BEGIN
	DECLARE @maNguoiDung VARCHAR(MAX) = CAST(@1 AS VARCHAR(MAX)) + '|'
	UPDATE dbo.BaiVietBaiGiang
		SET DanhSachMaThanhVienDaXem = REPLACE(DanhSachMaThanhVienDaXem, '|' + @maNguoiDung, '|') + @maNguoiDung
		WHERE Ma = @0
END