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

GO
--Lấy danh sách quyền theo mã người dùng và đối tượng
ALTER PROC dbo.layQuyenTheoMaDoiTuongVaMaNguoiDung_ChuoiGiaTri (
	@0 NVARCHAR(MAX), --PhamVi
	@1 INT, --MaDoiTuong
	@2 INT --MaNguoiDung
)
AS
BEGIN
	IF (@0 = 'HT')
	BEGIN
		SELECT 1
	END
	ELSE IF (@0 = 'KH')
	BEGIN
		DECLARE @giaTri VARCHAR(MAX) = ''
		SELECT 
			@giaTri += GiaTri + ','
			FROM 
				dbo.NhomNguoiDung_KH_NguoiDung NND_ND
					INNER JOIN dbo.NhomNguoiDung_KH NND ON 
						NND_ND.MaNguoiDung = @2
					INNER JOIN dbo.NhomNguoiDung_KH_Quyen NND_Q ON
						NND_Q.MaDoiTuong = @1 AND
						NND.Ma = NND_Q.MaNhomNguoiDung
					INNER JOIN dbo.Quyen Q ON 
						Q.GiaTri IS NOT NULL AND
						NND_Q.MaQuyen = Q.Ma

		SELECT CASE
			WHEN @giaTri = '' THEN
				''
			ELSE
				LEFT(@giaTri, LEN(@giaTri) - 1)
			END
	END
END