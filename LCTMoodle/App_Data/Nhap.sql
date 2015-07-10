<<<<<<< HEAD
DECLARE @0 INT = 24
DECLARE @chuoiMa VARCHAR(MAX) = ''

--L?y chu?i ng??i dùng
SELECT @chuoiMa += CAST(MaNguoiDung AS VARCHAR) + ','
	FROM 
		KhoaHoc_NguoiDung KH_ND
			INNER JOIN NguoiDung ND ON
				KH_ND.MaKhoaHoc = @0 AND
				LaHocVien = 1 AND
				KH_ND.MaNguoiDung = ND.Ma
	ORDER BY ND.Ten
	
	SET @chuoiMa = LEFT(@chuoiMa, LEN(@chuoiMa) - 1) + '|'

SELECT @chuoiMa += CASE
	WHEN CD_ND.Diem IS NULL THEN
		','
	ELSE 
		CAST(CD_ND.Diem AS VARCHAR(MAX)) + ','
	END
FROM 
	NguoiDung ND
		--L?y ng??i dùng là h?c viên c?a khóa h?c
		INNER JOIN KhoaHoc_NguoiDung KH_ND ON
			KH_ND.MaKhoaHoc = @0 AND
			KH_ND.MaNguoiDung = ND.Ma
		--L?y c?t ?i?m c?a khóa h?c
		RIGHT JOIN CotDiem CD ON
			CD.MaKhoaHoc = @0
		--L?y ?i?m c?a c?t ?i?m
		LEFT JOIN CotDiem_NguoiDung CD_ND ON
			CD.Ma = CD_ND.MaCotDiem AND
			ND.Ma = CD_ND.MaNguoiDung
ORDER BY ND.Ten, CD.ThuTu

SELECT LEFT(@chuoiMa, LEN(@chuoiMa) - 1)

SELECT @chuoiMa

SELECT CD.MaKhoaHoc
FROM 
	NguoiDung ND
		--L?y ng??i dùng là h?c viên c?a khóa h?c
		INNER JOIN KhoaHoc_NguoiDung KH_ND ON
			KH_ND.MaKhoaHoc = 24 AND
			KH_ND.MaNguoiDung = ND.Ma
		--L?y c?t ?i?m c?a khóa h?c
		INNER JOIN CotDiem CD ON
			CD.MaKhoaHoc = 24
		--L?y ?i?m c?a c?t ?i?m
		LEFT JOIN CotDiem_NguoiDung CD_ND ON
			CD.Ma = CD_ND.MaCotDiem AND
			ND.Ma = CD_ND.MaNguoiDung
ORDER BY ND.Ten, CD.ThuTu
=======
<<<<<<< HEAD
select * from nguoiDung

alter table nguoiDung
drop column DaDuyet
=======
>>>>>>> 9f0eb73f2080ced61131943ddb11a0273f414bb4
>>>>>>> 3bcf48f6a70386934aef492804fe96c129ebed22
