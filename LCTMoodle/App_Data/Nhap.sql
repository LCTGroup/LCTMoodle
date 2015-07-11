
SELECT 0
	FROM
		--L?y nhóm mà ng??i dùng thu?c
		dbo.NhomNguoiDung_HT_NguoiDung NND_ND
			--Quy?n mà nhóm có
			INNER JOIN dbo.NhomNguoiDung_HT_Quyen NND_Q ON
				NND_ND.MaNguoiDung = 1 AND
				NND_ND.MaNhomNguoiDung = NND_Q.MaNhomNguoiDung
			--Chi ti?t quy?n
			INNER JOIN dbo.Quyen Q ON
				NND_Q.MaQuyen = Q.Ma AND
				Q.PhamVi = 'KH'
			INNER JOIN dbo.NhomNguoiDung_HT NND ON
				NND_ND.MaNhomNguoiDung = NND.Ma