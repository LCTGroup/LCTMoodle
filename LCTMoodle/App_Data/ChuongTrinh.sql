use rtcmfraf_Moodle;

GO
--Chương trình
CREATE TABLE dbo.ChuongTrinh (
	Ma INT PRIMARY KEY IDENTITY(1, 1),
	MaKhoaHoc INT NOT NULL,
	CongViec NVARCHAR(MAX) NOT NULL,
	MoTa NVARCHAR(MAX) NOT NULL,
	ThoiGian NVARCHAR(MAX),
	ThuTu INT NOT NULL
)

GO
--Trigger xóa chương trình
--Cập nhật thứ tự
ALTER TRIGGER dbo.xoaChuongTrinh_TRIGGER
ON dbo.ChuongTrinh
AFTER DELETE
AS
BEGIN
	--Cập nhật thứ tự
	UPDATE GT
		SET
			GT.ThuTu = GT.ThuTu - 1
		FROM
			dbo.ChuongTrinh GT
				INNER JOIN deleted d
				ON 
					GT.MaKhoaHoc = d.MaKhoaHoc AND
					GT.ThuTu > d.ThuTu
END

GO
--Lấy chương trình theo mã khóa học
ALTER PROC dbo.layChuongTrinhTheoMaKhoaHoc (
	@0 INT --MaKhoaHoc
)
AS
BEGIN
	SELECT
		Ma,
		MaKhoaHoc,
		BaiHoc,
		NoiDung,
		ThoiGian,
		ThuTu
		FROM dbo.ChuongTrinh
		WHERE MaKhoaHoc = @0
		ORDER BY ThuTu
END

GO
--Thêm chương trình
ALTER PROC dbo.themChuongTrinh (
	@0 INT, --MaKhoaHoc
	@1 NVARCHAR(MAX), --BaiHoc
	@2 NVARCHAR(MAX), --NoiDung
	@3 NVARCHAR(MAX) --ThoiGian
)
AS
BEGIN
	--Lấy thứ tự
	DECLARE @thuTu INT

	SELECT @thuTu = MAX(ThuTu)
		FROM dbo.ChuongTrinh
		WHERE MaKhoaHoc = @0

	SET @thuTu = 
		CASE
			WHEN @thuTu IS NULL
				THEN 1
		ELSE
			@thuTu + 1
		END

	INSERT INTO dbo.ChuongTrinh (MaKhoaHoc, BaiHoc, NoiDung, ThoiGian, ThuTu)
		VALUES (@0, @1, @2, @3, @thuTu)
		
	SELECT TOP 1
		Ma,
		MaKhoaHoc,
		BaiHoc,
		NoiDung,
		ThoiGian
		FROM dbo.ChuongTrinh
		WHERE Ma = @@IDENTITY
END

GO
--Xóa giáo trình
ALTER PROC dbo.xoaChuongTrinhTheoMa (
	@0 INT --Ma
)
AS
BEGIN
	DELETE FROM dbo.ChuongTrinh
		WHERE Ma = @0
END

GO
--Thay đổi thứ tự
ALTER PROC dbo.capNhatChuongTrinh_ThuTu (
	@0 INT, --ThuTu cũ
	@1 INT, --ThuTu mới
	@2 INT --MaKhoaHoc
)
AS
BEGIN
	IF (@0 = @1)
	BEGIN
		RETURN
	END

	--Biến để xác định giá trị thứ tự sẽ thay đổi như thế nào
	DECLARE @thayDoi INT, @gioiHanTren INT, @gioiHanDuoi INT
	
	IF (@0 < @1)
	BEGIN
		SET @thayDoi = -1
		SET @gioiHanTren = @1
		SET @gioiHanDuoi = @0
	END
	ELSE
	BEGIN
		SET @thayDoi = 1
		SET @gioiHanTren = @0
		SET @gioiHanDuoi = @1
	END

	UPDATE GT
		SET
			GT.ThuTu = CASE
				WHEN GT.ThuTu <> @0 THEN
					GT.ThuTu + @thayDoi
				ELSE
					@1
				END
		FROM dbo.ChuongTrinh GT
		WHERE 
			GT.MaKhoaHoc = @2 AND
			GT.ThuTu BETWEEN @gioiHanDuoi AND @gioiHanTren
END

GO
--Lấy theo mã
CREATE PROC dbo.layTheoMa(
	@0 INT --Ma
)
AS
BEGIN
	SELECT TOP 1
		Ma,
		MaKhoaHoc,
		BaiHoc,
		NoiDung,
		ThoiGian
		FROM dbo.ChuongTrinh
		WHERE Ma = @0
END