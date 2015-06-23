SELECT Ho, TenLot, Ten FROM NguoiDung WHERE Ma > 3

UPDATE NguoiDung
	SET Ten = SUBSTRING(Ten , LEN(Ten) - CHARINDEX(' ', REVERSE(Ten)) + 2, LEN(Ten))
	WHERE Ma > 3

SELECT SUBSTRING('Le Binh Chieu' , LEN(Le Binh Chieu) - CHARINDEX(' ', REVERSE(Le Binh Chieu)) + 2, LEN(Ten))

SELECT SUBSTRING_INDEX(,' ',-1)

select * from dbo.NguoiDung
delete from dbo.NguoiDung where ma >= 288

update dbo.NguoiDung
set MaKichHoat = 675316
where Ma=311