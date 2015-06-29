select * from khoahoc where ma = 1

select * from nhomnguoidung_HT_Quyen

select * from nhomnguoidung_CD_quyen

exec layNhomnguoiDung_QuyenTheoMaNhomNguoiDungVaMaDoiTuong 'HT', , 0

exec layQuyenTheoMaNguoiDungVaGiaTriVaMaDoiTuong_KiemTra 1, 'HT', 0, 'QLaQuyen'
select top 1 * from nguoidung 