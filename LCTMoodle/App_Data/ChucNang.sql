use rtcmfraf_Moodle;

--Reset bảng
truncate table dbo.TapTin
DBCC CHECKIDENT('dbo.TapTin', RESEED, 1)

--Tiếng việt
--ALTER DATABASE rtcmfraf_Moodle
--	COLLATE Vietnamese_CI_AS