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
		ThoiGian,
		ThuTu
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
--Thay đổi thứ tự
ALTER PROC dbo.capNhatGiaoTrinh_ThuTu (
	@0 INT, --Ma
	@1 INT, --ThuTu
	@2 INT --MaKhoaHoc
)
AS
BEGIN
	UPDATE GT
		SET
			GT.ThuTu =
				CASE
					WHEN Dong >= @1
						THEN Dong + 1
					ELSE
						Dong
				END				
		FROM 
			(SELECT Ma, ThuTu, ROW_NUMBER() OVER (ORDER BY ThuTu) AS Dong
				FROM dbo.GiaoTrinh
				WHERE 
					MaKhoaHoc = @2 AND
					Ma <> @0) AS GT

	UPDATE dbo.GiaoTrinh
		SET ThuTu = @1
		WHERE Ma = @0
END