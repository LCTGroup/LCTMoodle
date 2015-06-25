<<<<<<< HEAD
﻿select * from nhomnguoidung_kh 
=> ma = 4
select * from nhomnguoidung_kh_quyen
510 511 512
SELECT 
	GiaTri
	FROM 
		dbo.NhomNguoiDung_KH_NguoiDung NND_ND
			INNER JOIN dbo.NhomNguoiDung_KH NND ON 
				NND_ND.MaNguoiDung = 1 AND
				NND_ND.MaNhomNguoiDung = NND.Ma
			INNER JOIN dbo.NhomNguoiDung_KH_Quyen NND_Q ON
				NND_Q.MaDoiTuong = 1 AND
				NND.Ma = NND_Q.MaNhomNguoiDung
			INNER JOIN dbo.Quyen Q ON 
				Q.GiaTri IS NOT NULL AND
				NND_Q.MaQuyen = Q.Ma
=======
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
>>>>>>> dbb32a95e737e249fce9383728c0dea11147b333
