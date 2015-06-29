select * from khoahoc where ma = 1

select * from nhomnguoidung_HT_Quyen

select * from nhomnguoidung_CD_quyen

exec layNhomnguoiDung_QuyenTheoMaNhomNguoiDungVaMaDoiTuong 'HT', , 0

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