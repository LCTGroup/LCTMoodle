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
	Ghim BIT,
	Diem INT
)

GO
--Trigger xóa
--Xóa tập tin
--Xóa bình luận
CREATE TRIGGER dbo.xoaBaiVietDienDan_TRIGGER
ON dbo.BaiVietDienDan
AFTER DELETE
AS
BEGIN
	--Xóa tập tin
	DELETE TT
		FROM 
			dbo.TapTin_BaiVietDienDan_TapTin TT
				INNER JOIN deleted d ON
					TT.Ma = d.MaTapTin

	--Xóa bình luận
	DELETE BL
		FROM
			dbo.BinhLuanBaiVietDienDan BL
				INNER JOIN deleted d ON
					BL.MaBaiVietDienDan = d.Ma
END

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

	SELECT *
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
	SELECT *
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
	SELECT *
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
	
	SELECT *
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

GO
--Cập nhật điểm theo mã bài viết
CREATE PROC dbo.capNhatBaiVietDienDanTheoMa_Diem (
	@0 INT, --Ma
	@1 INT --Diem
)
AS
BEGIN
	--Lấy điểm hiện tại, người tạo của bài viết, mã khóa học
	DECLARE @diem INT, @maNguoiTao INT, @maKhoaHoc INT
	SELECT 
		@diem = Diem,
		@maNguoiTao = MaNguoiTao,
		@maKhoaHoc = MaKhoaHoc
		FROM dbo.BaiVietDienDan
		WHERE Ma = @0

	IF (@diem IS NULL)
	BEGIN
		SET @diem = 0
	END

	--Cập nhật điểm của thành viên
	UPDATE dbo.KhoaHoc_NguoiDung
		SET DiemThaoLuan += @1 - @diem
		WHERE
			MaKhoaHoc = @maKhoaHoc AND
			MaNguoiDung = @maNguoiTao

	--Cập nhật điểm
	UPDATE dbo.BaiVietDienDan
		SET Diem = @1
		WHERE Ma = @0
END