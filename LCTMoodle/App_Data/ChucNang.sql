use rtcmfraf_Moodle;

GO
ALTER FUNCTION dbo.taoChuoiCapNhat (
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

	RETURN CASE 
		WHEN @query = '' THEN
			''
		ELSE
			LEFT(@query, LEN(@query) - 1)
		END
END

--Reset bảng
truncate table dbo.ChuDe
DBCC CHECKIDENT('dbo.ChuDe', RESEED, 1)

--Tiếng việt
ALTER DATABASE rtcmfraf_Moodle
	COLLATE Vietnamese_CI_A
	
--Đổi tên
EXEC sp_rename 'dbo.NguoiDung.TrangThaiHoatDong', 'DaDuyet', 'COLUMN';

--Đệ quy trigger
ALTER DATABASE rtcmfraf_Moodle
	SET RECURSIVE_TRIGGERS ON;