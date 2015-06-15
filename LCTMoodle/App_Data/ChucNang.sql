use rtcmfraf_Moodle;

GO
CREATE TYPE dbo.BangCapNhat
AS
TABLE (
	TenTruong NVARCHAR(MAX) NOT NULL,
	GiaTri NVARCHAR(MAX),
	Loai TINYINT NOT NULL
)

GO
CREATE FUNCTION dbo.taoChuoiCapNhat (
	@0 dbo.BangCapNhat READONLY
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @query NVARCHAR(MAX) = ''

	SELECT @query = 
		@query + ' ' +
		TenTruong + ' = ' + 
		CASE
			WHEN GiaTri IS NULL THEN
				'NULL'
			WHEN Loai = 1 THEN 
				GiaTri
			WHEN Loai = 2 THEN 
				'N''' + REPLACE(GiaTri, '''', '''''') + ''''
			WHEN Loai = 3 THEN 
				'CONVERT(DATETIME, ''' + GiaTri + ''', 103)'
			ELSE
				'NULL'
		END + ','
		FROM @0

	RETURN LEFT(@query, LEN(@query) - 1)
END

--Reset bảng
truncate table dbo.BaiTapNop
DBCC CHECKIDENT('dbo.BaiTapNop', RESEED, 1)

--Tiếng việt
ALTER DATABASE rtcmfraf_Moodle
	COLLATE Vietnamese_CI_A
