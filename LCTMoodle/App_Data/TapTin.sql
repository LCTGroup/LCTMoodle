use rtcmfraf_Moodle;

GO
--Tập tin
CREATE TABLE dbo.TapTin (
	Ma INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
	Ten NVARCHAR(MAX) NOT NULL,
	Loai NVARCHAR(MAX) NOT NULL,
	ThuMuc NVARCHAR(MAX) NOT NULL,
	ThoiDiemTao DATETIME DEFAULT GETDATE() NOT NULL
);

GO
--Thêm tập tin
ALTER PROC dbo.themTapTin (
	@0 NVARCHAR(MAX), --Tên tập tin
	@1 NVARCHAR(MAX), --Loại tập tin
	@2 NVARCHAR(MAX) --Tên thư mục
)
AS
BEGIN
	INSERT INTO dbo.TapTin (Ten, Loai, ThuMuc) VALUES
		(@0, @1, @2);

	SELECT 
		Ma,
		Ten,
		Loai,
		ThuMuc,
		ThoiDiemTao
		FROM dbo.TapTin
		WHERE Ma = @@IDENTITY;
END;

GO
--Lấy tập tin
CREATE PROC dbo.layTapTinTheoMa (
	@0 INT --Mã tập tin
)
AS
BEGIN
	SELECT 
		Ma,
		Ten,
		Loai,
		ThuMuc,
		ThoiDiemTao
		FROM dbo.TapTin
		WHERE Ma = @0;
END