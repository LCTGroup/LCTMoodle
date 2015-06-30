select * from khoahoc where ma = 1

select * from nhomnguoidung_HT_Quyen


exec layNhomnguoiDung_QuyenTheoMaNhomNguoiDungVaMaDoiTuong 'KH', , 0

<<<<<<< HEAD
select top 1 * from nguoidung 

update nguoidung set coquyennhomkh = 1 where ma = 1

select * from nhomnguoidung_ht_nguoidung
select * from nhomnguoidung_ht_quyen
select * from nhomnguoidung_ht

select * from nhomnguoidung_CD_nguoidung
select * from nhomnguoidung_CD_quyen
select * from nhomnguoidung_cd
layQuyenTheoMaNguoiDungVaGiaTriVaMaDoiTuong_KiemTra 1, 'CD', 0, 'QLQuyen'
layQuyenTheoMaNguoiDungVaMaDoiTuong_ChuoiGiaTri 1, 'CD', 0

SELECT TOP 1 1
	FROM 
		dbo.NhomNguoiDung_CD NND
			INNER JOIN dbo.NhomNguoiDung_CD_NguoiDung NND_ND ON
				NND_ND.MaNguoiDung = 1 AND
				NND_ND.MaNhomNguoiDung = NND.Ma
			INNER JOIN dbo.NhomNguoiDung_CD_Quyen NND_Q ON
				NND_Q.MaNhomNguoiDung = NND.Ma AND
				NND_Q.MaDoiTuong = 0
			INNER JOIN dbo.Quyen Q ON
				Q.PhamVi = 'CD' AND
				Q.Ma = NND_Q.MaQuyen AND
				Q.GiaTri = 'QLQuyen'

declare @maChuDe INT
SELECT TOP 1 1
	FROM 
		dbo.NhomNguoiDung_CD NND
			INNER JOIN dbo.NhomNguoiDung_CD_NguoiDung NND_ND ON
				NND.MaDoiTuong = @maChuDe AND
				NND_ND.MaNguoiDung = 1 AND
				NND_ND.MaNhomNguoiDung = NND.Ma
			INNER JOIN dbo.NhomNguoiDung_CD_Quyen NND_Q ON
				NND_Q.MaNhomNguoiDung = NND.Ma
			INNER JOIN dbo.Quyen Q ON
				Q.PhamVi = 'CD' AND
				Q.Ma = NND_Q.MaQuyen AND
				Q.GiaTri = 'QLQuyen'

SELECT *
	FROM 
		dbo.NhomNguoiDung_HT NND
			INNER JOIN dbo.NhomNguoiDung_HT_NguoiDung NND_ND ON
				NND_ND.MaNguoiDung = 1 AND
				NND_ND.MaNhomNguoiDung = NND.Ma
			INNER JOIN dbo.NhomNguoiDung_HT_Quyen NND_Q ON
				NND_Q.MaNhomNguoiDung = NND.Ma AND
				NND_Q.MaDoiTuong = 0
			INNER JOIN dbo.Quyen Q ON
				Q.PhamVi = 'CD' AND
				Q.Ma = NND_Q.MaQuyen AND
				Q.GiaTri = 'QLQuyen'