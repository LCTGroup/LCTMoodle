use rtcmfraf_Moodle;

GO
--Tạo giáo trình
CREATE TABLE dbo.GiaoTrinh (
	Ma INT PRIMARY KEY IDENTITY(1, 1),
	MaKhoaHoc INT NOT NULL,
	CongViec NVARCHAR(MAX) NOT NULL,
	MoTa NVARCHAR(MAX) NOT NULL,
	ThoiGian NVARCHAR(MAX),
	ThuTu INT NOT NULL
)

GO
--Lấy giáo trình theo mã khóa học
ALTER PROC dbo.layGiaoTrinhTheoMaKhoaHoc (
	@0 INT --MaKhoaHoc
)
AS
BEGIN
	SELECT
		Ma,
		MaKhoaHoc,
		CongViec,
		MoTa,
		ThoiGian,
		ThuTu
		FROM dbo.GiaoTrinh
		WHERE MaKhoaHoc = @0
		ORDER BY ThuTu
END

GO
--Thêm giáo trình
ALTER PROC dbo.themGiaoTrinh (
	@0 INT, --MaKhoaHoc
	@1 NVARCHAR(MAX), --CongViec
	@2 NVARCHAR(MAX), --ThoiGian
	@3 NVARCHAR(MAX) --ThoiGian
)
AS
BEGIN
	--Lấy thứ tự
	DECLARE @thuTu INT

	SELECT @thuTu = MAX(ThuTu)
		FROM dbo.GiaoTrinh
		WHERE MaKhoaHoc = @0

	SET @thuTu = 
		CASE
			WHEN @thuTu IS NULL
				THEN 1
		ELSE
			@thuTu + 1
		END

	INSERT INTO dbo.GiaoTrinh (MaKhoaHoc, CongViec, MoTa, ThoiGian, ThuTu)
		VALUES (@0, @1, @2, @3, @thuTu)
		
	SELECT TOP 1
		Ma,
		MaKhoaHoc,
		CongViec,
		MoTa,
		ThoiGian
		FROM dbo.GiaoTrinh
		WHERE Ma = @@IDENTITY
END

GO
--Xóa giáo trình
CREATE PROC dbo.xoaGiaoTrinhTheoMa (
	@0 INT --Ma
)
AS
BEGIN
	DELETE FROM dbo.GiaoTrinh
		WHERE Ma = @0
END

GO
--[Trigger] Xóa giáo trinh
CREATE TRIGGER dbo.xoaGiaoTrinh_TRIGGER
	ON dbo.GiaoTrinh
	AFTER DELETE
AS
BEGIN
	UPDATE GT
		SET
			GT.ThuTu = GT.ThuTu - 1
		FROM
			dbo.GiaoTrinh GT
				INNER JOIN deleted d
				ON 
					GT.MaKhoaHoc = d.MaKhoaHoc AND
					GT.ThuTu > d.ThuTu
END

GO
--Thay đổi thứ tự
ALTER PROC dbo.capNhatGiaoTrinh_ThuTu (
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
		FROM dbo.GiaoTrinh GT
		WHERE 
			GT.MaKhoaHoc = @2 AND
			GT.ThuTu BETWEEN @gioiHanDuoi AND @gioiHanTren
END