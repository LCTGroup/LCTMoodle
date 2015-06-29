<<<<<<< HEAD
<<<<<<< HEAD
﻿SELECT Ho, TenLot, Ten FROM NguoiDung WHERE Ma > 3

UPDATE NguoiDung
	SET Ten = SUBSTRING(Ten , LEN(Ten) - CHARINDEX(' ', REVERSE(Ten)) + 2, LEN(Ten))
	WHERE Ma > 3

SELECT SUBSTRING('Le Binh Chieu' , LEN(Le Binh Chieu) - CHARINDEX(' ', REVERSE(Le Binh Chieu)) + 2, LEN(Ten))

SELECT SUBSTRING_INDEX(,' ',-1)

select * from dbo.NguoiDung
delete from dbo.NguoiDung where ma >= 288

update dbo.NguoiDung
set MatKhauCap2 = 'a346a38a656709fbb9618ad2976097a4'
where Ma=2

alter table dbo.NguoiDung
add MatKhauCap2 NVARCHAR(MAX)
﻿select * from quyen

select * from dbo.cauhoi
update dbo.CauHoi
set diem = 0
where ma>=1
select *
from dbo.nguoiDung
DECLARE @a INT = 5
EXEc ('
	SELECT TOP ' + @a + ' *
		FROM NguoiDung
')

select * from
select * from dbo.cauhoi
select * from dbo.cauHoi_Diem

drop table dbo.cauHoi_Diem

select * from ChuDe

--declare @a bit = 1, @b bit = 1

--SELECT @a + @b

--select * from hoidap_diem

--declare @a INT = 0, @b INT = 0
--select @a += CASE
--	WHEN Diem = 1 THEN
--		1
--	ELSE
--		-1
--	END,
--	@b += CASE
--	WHEN MaNguoiTao = 1 THEN
--		CASE 
--			WHEN Diem = 1 THEN
--				2
--			ELSE
--				1
--			END
--	ELSE
--		0
--	END
--	FROM CauHoi_Diem

--SELECT @a
--SELECT @b
--select CAST(@a AS VARCHAR(MAX)) + '|' + CAST(@b AS VARCHAR(MAX))

alter table dbo.traloi
add Diem int default 0

select * from dbo.traloi
select * from dbo.cauHoi
update dbo.cauHoi set diem=0 where ma=1
 table cauhoi_diem
=======
﻿SELECT * FROM khoaHoc_NguoiDung
>>>>>>> b220b336de8fba13da8bf1bbb5c2860bb1762df3
=======
﻿
>>>>>>> 0a9f2ac66c4be1f52a2f15aef8261e0c0a8c32f2
