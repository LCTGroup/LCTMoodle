﻿use rtcmfraf_Moodle;

GO
--Tạo bài viết tài liệu
CREATE TABLE dbo.BaiVietTaiLieu(
	Ma INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
	TieuDe NVARCHAR(MAX) NOT NULL,
	NoiDung NVARCHAR(MAX) NOT NULL,
	MaTapTin INT DEFAULT NULL,
	ThoiDiemTao DATETIME DEFAULT GETDATE() NOT NULL,
	MaNguoiTao INT NOT NULL,
	MaKhoaHoc INT NOT NULL,
	DanhSachMaThanhVienDaXem VARCHAR(MAX) NOT NULL DEFAULT '|'
)

GO
--Trigger xóa
--Xóa tập tin
CREATE TRIGGER dbo.xoaBaiVietTaiLieu_TRIGGER
ON dbo.BaiVietTaiLieu
AFTER DELETE
AS
BEGIN
	--Xóa tập tin
	DELETE TT
		FROM 
			dbo.TapTin_BaiVietTaiLieu_TapTin TT
				INNER JOIN deleted d ON
					TT.Ma = d.MaTapTin
END

GO
--Trigger thêm
--Bổ sung trường danh sách mã thành viên đã xem
CREATE TRIGGER dbo.xoaBaiVietTaiLieu_TRIGGER
ON dbo.BaiVietTaiLieu
AFTER DELETE
AS
BEGIN
	--Xóa tập tin
	DELETE TT
		FROM 
			dbo.TapTin_BaiVietTaiLieu_TapTin TT
				INNER JOIN deleted d ON
					TT.Ma = d.MaTapTin
END

GO
--Thêm bài viết tài liệu
ALTER PROC dbo.themBaiVietTaiLieu(
	@0 NVARCHAR(MAX), --TieuDe
	@1 NVARCHAR(MAX), --NoiDung
	@2 INT, --MaTapTin
	@3 INT, --MaNguoiTao
	@4 INT --MaKhoaHoc
)
AS
BEGIN
	INSERT INTO dbo.BaiVietTaiLieu(TieuDe, NoiDung, MaTapTin, MaNguoiTao, MaKhoaHoc)
		VALUES (@0, @1, @2, @3, @4)

	SELECT *
		FROM dbo.BaiVietTaiLieu
		WHERE Ma = @@IDENTITY
END

GO
--Lấy bài viết tài liệu theo mã khóa học
ALTER PROC dbo.layBaiVietTaiLieuTheoMaKhoaHoc (
	@0 INT --MaKhoaHoc
)
AS
BEGIN
	SELECT *
		FROM dbo.BaiVietTaiLieu
		WHERE MaKhoaHoc = @0
		ORDER BY ThoiDiemTao ASC
END

GO
--Xóa bài viết tài liệu theo mã
ALTER PROC dbo.xoaBaiVietTaiLieuTheoMa (
	@0 INT --Ma
)
AS
BEGIN
	DELETE FROM dbo.BaiVietTaiLieu
		WHERE Ma = @0
END

GO
--Lấy theo mã
ALTER PROC dbo.layBaiVietTaiLieuTheoMa (
	@0 INT --Ma
)
AS
BEGIN
	SELECT *
		FROM dbo.BaiVietTaiLieu
		WHERE Ma = @0
END

GO
--Cập nhật theo mã
ALTER PROC dbo.capNhatBaiVietTaiLieuTheoMa (
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
		UPDATE dbo.BaiVietTaiLieu
			SET ' + @query + '
			WHERE Ma = ' + @0 + '
		')
	END	
	
	SELECT *
		FROM dbo.BaiVietTaiLieu
		WHERE Ma = @0
END

GO
--Cập nhật đã xem bài viết
CREATE PROC dbo.capNhatBaiVietTaiLieuTheoMa_Xem (
	@0 INT, --Ma
	@1 INT --MaNguoiDung
)
AS
BEGIN
	DECLARE @maNguoiDung VARCHAR(MAX) = CAST(@1 AS VARCHAR(MAX)) + '|'
	UPDATE dbo.BaiVietTaiLieu
		SET DanhSachMaThanhVienDaXem = REPLACE(DanhSachMaThanhVienDaXem, '|' + @maNguoiDung, '|') + @maNguoiDung
		WHERE Ma = @0
END