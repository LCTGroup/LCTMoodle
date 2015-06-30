select * from nhomnguoidung_ht 
update nhomnguoidung_ht_nguoidung set manhomnguoidung = 10
delete from nhomnguoidung_ht_nguoidung

<<<<<<< HEAD
select * from nhomnguoidung_HT_Quyen

select * from nhomnguoidung_CD_quyen

exec layNhomnguoiDung_QuyenTheoMaNhomNguoiDungVaMaDoiTuong 'KH', , 0

<<<<<<< HEAD
exec layQuyenTheoMaNguoiDungVaGiaTriVaMaDoiTuong_KiemTra 1, 'KH', 1, 'BT_QLBaiNop'
select top 1 * from nguoidung 

update nguoidung set coquyennhomkh = 1 where ma = 1

layQuyenTheoMaNguoiDungVaMaDoiTuong_ChuoiGiaTri 1, 'KH', 1
=======
exec layQuyenTheoMaNguoiDungVaGiaTriVaMaDoiTuong_KiemTra 1, 'HT', 0, 'QLaQuyen'
select top 1 * from nguoidung 

declare @cachSapXep NVARCHAR(MAX) = 'TraLoiNhieuNhat'
select *
from dbo.cauhoi
order by case @cachSapXep = 'TraLoiNhieuNhat'
WHEN
	SoLuongTraLoi
END
WHEN
	order by ThoiDiemTao
END
DESC
>>>>>>> d14fb15ee7d26dac86c7cf8ea89feedd646f16f5


select top 1 * from cauhoi
order by ThoiDiemTao DESC

select * from cauHoi
=======
select * from nhomnguoidung_ht_nguoidung
>>>>>>> 3280e4ba9f478c413b4a5f2c81d4a0235af02982
