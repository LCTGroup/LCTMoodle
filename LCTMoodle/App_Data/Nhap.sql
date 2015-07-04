SELECT 
	CD.Ma,
	CD.Ten,
	CD.MoTa,
	CD.MaNguoiTao,
	CD.ThoiDiemTao,
	CD.MaCha,
	CD.MaHinhDaiDien,
	COUNT(DISTINCT CD_Con.Ma) 'SLChuDeCon',
	COUNT(DISTINCT KH.Ma) 'SLKhoaHocCon'
	FROM 
		dbo.ChuDe CD 
			LEFT JOIN dbo.ChuDe CD_Con ON
				CD_Con.MaCha = CD.Ma
			LEFT JOIN dbo.KhoaHoc KH ON
				KH.MaChuDe = CD.Ma
	WHERE 
		CD.MaCha = 0
	GROUP BY 
		CD.Ma,
		CD.Ten,
		CD.MoTa,
		CD.MaNguoiTao,
		CD.ThoiDiemTao,
		CD.MaCha,
		CD.MaHinhDaiDien
		
select * from chude where macha = 45
select * from khoahoc where machude = 45