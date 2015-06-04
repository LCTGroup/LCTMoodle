use rtcmfraf_Moodle;

CREATE TYPE dbo.BangCapNhat
AS
TABLE (
	TenTruong NVARCHAR(MAX) NOT NULL,
	GiaTri NVARCHAR(MAX) NOT NULL,
	LaChuoi BIT NOT NULL
)

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
			WHEN LaChuoi = 1 THEN 
				'N''' + GiaTri + ''''
			ELSE
				GiaTri
		END		
		FROM @0

	RETURN @query
END

--Reset bảng
truncate table dbo.BaiTapNop
DBCC CHECKIDENT('dbo.BaiTapNop', RESEED, 1)

--Tiếng việt
ALTER DATABASE rtcmfraf_Moodle
	COLLATE Vietnamese_CI_AS
