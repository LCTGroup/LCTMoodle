<<<<<<< HEAD
﻿select * from cauHOi_diem
select * from nguoidung

alter table nguoidung
add DiemHoiDap INT DEFAULT 0

update dbo.nguoiDung
set DiemHoiDap = 8
where ma=2

select TenTaiKhoan, MatKhau, DiemHoiDap from nguoidung where ma = 2

select * from dbo.CauHOi_DIEM
select * from dbo.CauHOi

update dbo.cauhoi
set diem = 0
where ma = 9
=======
﻿UPDATE cotdiem
	set heso = 1
>>>>>>> 99ff389b2dc3703f1f3f6efd01d18cb5e668eb77
