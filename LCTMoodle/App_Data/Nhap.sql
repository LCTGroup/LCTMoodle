select * from nhomnguoidung_kh 
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