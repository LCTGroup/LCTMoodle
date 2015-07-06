use rtcmfraf_Moodle;

GO
	
CREATE TABLE dbo.BinhLuanBaiVietDienDan (
	Ma INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
	NoiDung NVARCHAR(MAX) NOT NULL,
	MaTapTin INT DEFAULT NULL,
	MaBaiVietDienDan INT NOT NULL,
	MaNguoiTao INT NOT NULL,
	ThoiDiemTao DATETIME DEFAULT GETDATE(),
	Diem INT
);

GO
--Thêm bình luận
CREATE PROC dbo.themBinhLuanBaiVietDienDan (
	@0 NVARCHAR(MAX), --NoiDung       
	@1 INT, --MaTapTin
	@2 INT, --MaBaiVietDienDan
	@3 INT --MaNguoiTao
)
AS
BEGIN
	INSERT INTO dbo.BinhLuanBaiVietDienDan (NoiDung, MaTapTin, MaBaiVietDienDan, MaNguoiTao)
		VALUES (@0, @1, @2, @3)

	SELECT *
		FROM dbo.BinhLuanBaiVietDienDan
		WHERE Ma = @@IDENTITY
END

GO
--Lấy bình luận theo đối tượng
ALTER PROC dbo.layBinhLuanBaiVietDienDanTheoMaBaiVietDienDan (
	@0 INT --MaBaiVietDienDan
)
AS
BEGIN
	SELECT *
		FROM dbo.BinhLuanBaiVietDienDan
		WHERE MaBaiVietDienDan = @0
END

GO
--Xóa bình luận theo loại và mã
ALTER PROC dbo.xoaBinhLuanBaiVietDienDanTheoMa (
	@0 INT --Ma
)
AS
BEGIN
	DELETE FROM dbo.BinhLuanBaiVietDienDan
		WHERE Ma = @0
END

GO
--Cập nhật theo mã
ALTER PROC dbo.capNhatBinhLuanBaiVietDienDanTheoMa (
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
			UPDATE dbo.BinhLuanBaiVietDienDan
				SET ' + @query + '
				WHERE Ma = ' + @0 + '
		')
	END	

	SELECT *
		FROM dbo.BinhLuanBaiVietDienDan
		WHERE Ma = @0
END

GO
--Lấy theo mã
CREATE PROC dbo.layBinhLuanBaiVietDienDanTheoMa (
	@0 INT --Ma
)
AS
BEGIN
	SELECT *
		FROM dbo.BinhLuanBaiVietDienDan
		WHERE Ma = @0
END

GO
--Cập nhật điểm bình luận
CREATE PROC dbo.capNhatBinhLuanBaiVietDienDanTheoMa_Diem (
	@0 INT, --Ma
	@1 INT --Diem
)
AS
BEGIN
	--Lấy điểm hiện tại, người tạo của bài viết, mã khóa học
	DECLARE @diem INT, @maNguoiTao INT, @maKhoaHoc INT
	SELECT 
		@diem = BL.Diem,
		@maNguoiTao = BL.MaNguoiTao,
		@maKhoaHoc = BV.MaKhoaHoc
		FROM 
			dbo.BinhLuanBaiVietDienDan BL
				INNER JOIN dbo.BaiVietDienDan BV ON
					BL.Ma = @0 AND
					BL.MaBaiVietDienDan = BV.Ma

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
	UPDATE dbo.BinhLuanBaiVietDienDan
		SET Diem = @1
		WHERE Ma = @0
END