use rtcmfraf_Moodle;

GO
CREATE TYPE dbo.BangCapNhat
AS
TABLE (
	TenTruong NVARCHAR(MAX) NOT NULL,
	GiaTri NVARCHAR(MAX),
	LaChuoi BIT NOT NULL
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
			WHEN LaChuoi = 1 THEN 
				'N''' + REPLACE(GiaTri, '''', '''''') + ''''
			ELSE
				GiaTri
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
