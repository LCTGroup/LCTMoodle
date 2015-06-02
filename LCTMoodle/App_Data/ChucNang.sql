use rtcmfraf_Moodle;

--Reset bảng
truncate table dbo.BaiTapNop
DBCC CHECKIDENT('dbo.BaiTapNop', RESEED, 1)

--Tiếng việt
ALTER DATABASE rtcmfraf_Moodle
	COLLATE Vietnamese_CI_AS
