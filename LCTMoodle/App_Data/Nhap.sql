select * from khoahoc where ma = 1

select * from nhomnguoidung_HT_Quyen

select * from nhomnguoidung_CD_quyen

exec layNhomnguoiDung_QuyenTheoMaNhomNguoiDungVaMaDoiTuong 'KH', , 0

exec layQuyenTheoMaNguoiDungVaGiaTriVaMaDoiTuong_KiemTra 1, 'KH', 1, 'BT_QLBaiNop'
select top 1 * from nguoidung 

update nguoidung set coquyennhomkh = 1 where ma = 1

layQuyenTheoMaNguoiDungVaMaDoiTuong_ChuoiGiaTri 1, 'KH', 1