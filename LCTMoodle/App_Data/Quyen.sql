use rtcmfraf_Moodle

GO
--Quyền
CREATE TABLE dbo.Quyen (
	Ma INT IDENTITY(1, 1) PRIMARY KEY,
	Ten NVARCHAR(MAX) NOT NULL,
	MoTa NVARCHAR(MAX),
	GiaTri NVARCHAR(MAX),
	PhamVi NVARCHAR(MAX) NOT NULL,
	MaCha INT DEFAULT 0 NOT NULL
)

GO
--Lấy theo phạm vi
CREATE PROC dbo.layQuyenTheoPhamViVaMaCha (
	@0 NVARCHAR(MAX), --PhamVi
	@1 INT --MaCha
)
AS
BEGIN
	SELECT 
		Ma,
		Ten,
		MoTa,
		GiaTri,
		PhamVi,
		MaCha
		FROM dbo.Quyen
		WHERE 
			PhamVi = @0 AND
			MaCha = @1
END

GO
--Lấy theo mã
ALTER PROC dbo.layQuyenTheoMa (
	@0 INT --Mã
)
AS
BEGIN
	SELECT TOP 1
		Ma,
		Ten,
		MoTa,
		GiaTri,
		PhamVi,
		MaCha
		FROM dbo.Quyen
		WHERE Ma = @0
END

GO
--Lấy quyền lá
ALTER FUNCTION dbo.layQuyenLa_FUNCTION (
	@maCha INT --Mã quyền cha
)
RETURNS VARCHAR(MAX)
AS
BEGIN
	DECLARE @chuoiMaLa VARCHAR(MAX) = ''

	SELECT 
		@chuoiMaLa = CASE
			WHEN GiaTri IS NULL THEN 
				@chuoiMaLa + dbo.layQuyenLa_FUNCTION(Ma)
			ELSE
				@chuoiMaLa + CAST(Ma AS VARCHAR(MAX)) + '|'
			END
		FROM dbo.Quyen
		WHERE MaCha = @maCha

	RETURN @chuoiMaLa
END