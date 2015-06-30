﻿use rtcmfraf_Moodle

GO
--Quyền
CREATE TABLE dbo.Quyen (
	Ma INT PRIMARY KEY,
	Ten NVARCHAR(MAX) NOT NULL,
	MoTa NVARCHAR(MAX),
	GiaTri NVARCHAR(MAX),
	PhamVi NVARCHAR(MAX) NOT NULL,
	MaCha INT DEFAULT 0 NOT NULL,
	ThuTu INT NOT NULL,
	LaQuyenChung BIT DEFAULT 0 NOT NULL
)

GO
--Lấy theo phạm vi
CREATE PROC dbo.layQuyenTheoPhamViVaMaChaVaLaQuyenChung (
	@0 NVARCHAR(MAX), --PhamVi
	@1 INT, --MaCha
	@2 BIT --LaQuyenChung
)
AS
BEGIN
	SELECT 
		Ma,
		Ten,
		MoTa,
		GiaTri,
		PhamVi,
		LaQuyenChung
		FROM dbo.Quyen
		WHERE 
			PhamVi = @0 AND
			MaCha = @1 AND
			LaQuyenChung = @2
		ORDER BY ThuTu ASC
END

GO
--Lấy theo mã
ALTER PROC dbo.layQuyenTheoMa (
	@0 INT --Mã
)
AS
BEGIN
	SELECT TOP 1
		Ma,
		Ten,
		MoTa,
		GiaTri,
		PhamVi,
		MaCha,
		LaQuyenChung
		FROM dbo.Quyen
		WHERE Ma = @0
END

GO
--Lấy quyền lá
ALTER FUNCTION dbo.layQuyenLa_FUNCTION (
	@maCha INT --Mã quyền cha
)
RETURNS VARCHAR(MAX)
AS
BEGIN
	DECLARE @chuoiMaLa VARCHAR(MAX) = ''

	SELECT 
		@chuoiMaLa = CASE
			WHEN GiaTri IS NULL THEN 
				@chuoiMaLa + dbo.layQuyenLa_FUNCTION(Ma)
			ELSE
				@chuoiMaLa + CAST(Ma AS VARCHAR(MAX)) + '|'
			END
		FROM dbo.Quyen
		WHERE MaCha = @maCha

	RETURN @chuoiMaLa
END

GO
--Lấy danh sách quyền theo mã người dùng và đối tượng
ALTER PROC dbo.layQuyenTheoMaNguoiDungVaMaDoiTuong_ChuoiGiaTri (
	@0 INT, --MaNguoiDung
	@1 NVARCHAR(MAX), --PhamVi
	@2 INT --MaDoiTuong
)
AS
BEGIN
	--Lấy các quyền mà người dùng có
	DECLARE 
		@coQuyenNhomHT BIT, 
		@coQuyenNhomCD BIT,
		@coQuyenNhomKH BIT

	SELECT 
		@coQuyenNhomHT = CoQuyenNhomHT, 
		@coQuyenNhomCD = CoQuyenNhomCD,
		@coQuyenNhomKH = CoQuyenNhomKH
		FROM dbo.NguoiDung
		WHERE Ma = @0

	--Chuỗi giá trị chứa danh sách quyền
	DECLARE @giaTri VARCHAR(MAX) = ''
	
	--Quyền khóa học chỉ xác định phạm vi quyền khóa học
	IF (
		@coQuyenNhomKH = 1 AND
		@1 = 'KH'
	)
	BEGIN
		SELECT @giaTri += Q.GiaTri + '|'
			FROM 
				dbo.NhomNguoiDung_KH NND
					INNER JOIN dbo.NhomNguoiDung_KH_NguoiDung NND_ND ON
						NND_ND.MaNguoiDung = @0 AND
						NND_ND.MaNhomNguoiDung = NND.Ma
					INNER JOIN dbo.NhomNguoiDung_KH_Quyen NND_Q ON
						NND_Q.MaNhomNguoiDung = NND.Ma AND
						(NND_Q.MaDoiTuong = @2 OR 
							NND_Q.MaDoiTuong = 0)
					INNER JOIN dbo.Quyen Q ON
						Q.PhamVi = @1 AND
						Q.Ma = NND_Q.MaQuyen
			GROUP BY Q.GiaTri
	END
	--Phạm vi chủ đề chỉ kiểm soát chủ đề, khóa học, hỏi đáp
	IF (
		@coQuyenNhomCD = 1 AND
		(@1 = 'CD' OR @1 = 'HD' OR @1 = 'KH')
	)
	BEGIN
		--Nếu là CD => Lấy mã chủ đề cha của chủ đề
		--Nếu là HD => Lấy mã chủ đề của câu hỏi
		--Nếu là KH => Lấy mã chủ đề của khóa học
		DECLARE @maChuDe INT
		IF (@1 = 'KH')
		BEGIN
			SELECT @maChuDe = MaChuDe
				FROM dbo.KhoaHoc
				WHERE Ma = @2
		END
		ELSE IF (@1 = 'HD')
		BEGIN
			SELECT @maChuDe = MaChuDe
				FROM dbo.CauHoi
				WHERE Ma = @2
		END
		ELSE
		BEGIN
			SELECT @maChuDe = MaCha
				FROM dbo.ChuDe
				WHERE Ma = @2

			--Khi là chủ đề thì lấy quyền trong chính nó nữa
			SELECT @giaTri += Q.GiaTri + '|'
			FROM 
				dbo.NhomNguoiDung_CD NND
					INNER JOIN dbo.NhomNguoiDung_CD_NguoiDung NND_ND ON
						NND_ND.MaNguoiDung = @0 AND
						NND_ND.MaNhomNguoiDung = NND.Ma
					INNER JOIN dbo.NhomNguoiDung_CD_Quyen NND_Q ON
						NND_Q.MaNhomNguoiDung = NND.Ma AND
						NND_Q.MaDoiTuong = @2
					INNER JOIN dbo.Quyen Q ON
						Q.PhamVi = @1 AND
						Q.Ma = NND_Q.MaQuyen
			GROUP BY Q.GiaTri
		END

		SELECT @giaTri += Q.GiaTri + '|'
			FROM 
				dbo.NhomNguoiDung_CD NND
					INNER JOIN dbo.NhomNguoiDung_CD_NguoiDung NND_ND ON
						NND.MaDoiTuong = @maChuDe AND
						NND_ND.MaNguoiDung = @0 AND
						NND_ND.MaNhomNguoiDung = NND.Ma
					INNER JOIN dbo.NhomNguoiDung_CD_Quyen NND_Q ON
						NND_Q.MaNhomNguoiDung = NND.Ma
					INNER JOIN dbo.Quyen Q ON
						Q.PhamVi = @1 AND
						Q.Ma = NND_Q.MaQuyen
			GROUP BY Q.GiaTri
	END
	--Phạm vi hệ thống kiểm soát tất cả phạm vi quyền
	IF (@coQuyenNhomHT = 1)
	BEGIN
		SELECT @giaTri += Q.GiaTri + '|'
			FROM 
				dbo.NhomNguoiDung_HT NND
					INNER JOIN dbo.NhomNguoiDung_HT_NguoiDung NND_ND ON
						NND_ND.MaNguoiDung = @0 AND
						NND_ND.MaNhomNguoiDung = NND.Ma
					INNER JOIN dbo.NhomNguoiDung_HT_Quyen NND_Q ON
						NND_Q.MaNhomNguoiDung = NND.Ma
					INNER JOIN dbo.Quyen Q ON
						Q.PhamVi = @1 AND
						Q.Ma = NND_Q.MaQuyen
			GROUP BY Q.GiaTri
	END

	SELECT CASE
		WHEN @giaTri = '' THEN
			''
		ELSE
			LEFT(@giaTri, LEN(@giaTri) - 1)
		END
END

GO
--Lấy quyền theo mã người dùng và mã đối tượng và giá trị
ALTER PROC dbo.layQuyenTheoMaNguoiDungVaGiaTriVaMaDoiTuong_KiemTra (
	@0 INT, --MaNguoiDung
	@1 NVARCHAR(MAX), --PhamVi
	@2 INT, --MaDoiTuong
	@3 NVARCHAR(MAX) --GiaTri
)
AS
BEGIN
	--Lấy các quyền mà người dùng có
	DECLARE 
		@coQuyenNhomHT BIT, 
		@coQuyenNhomCD BIT,
		@coQuyenNhomKH BIT

	SELECT 
		@coQuyenNhomHT = CoQuyenNhomHT, 
		@coQuyenNhomCD = CoQuyenNhomCD,
		@coQuyenNhomKH = CoQuyenNhomKH
		FROM dbo.NguoiDung
		WHERE Ma = @0
	
	--Quyền khóa học chỉ xác định phạm vi quyền khóa học
	IF (
		@coQuyenNhomKH = 1 AND
		@1 = 'KH'
	)
	BEGIN
		IF EXISTS(
			SELECT TOP 1 1
				FROM 
					dbo.NhomNguoiDung_KH NND
						INNER JOIN dbo.NhomNguoiDung_KH_NguoiDung NND_ND ON
							NND_ND.MaNguoiDung = @0 AND
							NND_ND.MaNhomNguoiDung = NND.Ma
						INNER JOIN dbo.NhomNguoiDung_KH_Quyen NND_Q ON
							NND_Q.MaNhomNguoiDung = NND.Ma AND
							(NND_Q.MaDoiTuong = @2 OR 
								NND_Q.MaDoiTuong = 0)
						INNER JOIN dbo.Quyen Q ON
							Q.PhamVi = @1 AND
							Q.Ma = NND_Q.MaQuyen AND
							Q.GiaTri = @3
		)
		BEGIN
			SELECT 1
			RETURN
		END
	END
	--Phạm vi chủ đề chỉ kiểm soát chủ đề, khóa học, hỏi đáp
	IF (
		@coQuyenNhomCD = 1 AND
		(@1 = 'CD' OR @1 = 'HD' OR @1 = 'KH')
	)
	BEGIN
		--Nếu là CD => Mã đối tượng là mã chủ đề => khỏi xử lý
		--Nếu là HD => Lấy mã chủ đề của câu hỏi
		--Nếu là KH => Lấy mã chủ đề của khóa học
		DECLARE @maChuDe INT
		IF (@1 = 'KH')
		BEGIN
			SELECT @maChuDe = MaChuDe
				FROM dbo.KhoaHoc
				WHERE Ma = @2
		END
		ELSE IF (@1 = 'HD')
		BEGIN
			SELECT @maChuDe = MaChuDe
				FROM dbo.CauHoi
				WHERE Ma = @2
		END
		ELSE
		BEGIN
			SELECT @maChuDe = MaCha
				FROM dbo.ChuDe
				WHERE Ma = @2

			--Khi là chủ đề thì lấy quyền trong chính nó nữa
			IF EXISTS(
				SELECT TOP 1 1
					FROM 
						dbo.NhomNguoiDung_CD NND
							INNER JOIN dbo.NhomNguoiDung_CD_NguoiDung NND_ND ON
								NND_ND.MaNguoiDung = @0 AND
								NND_ND.MaNhomNguoiDung = NND.Ma
							INNER JOIN dbo.NhomNguoiDung_CD_Quyen NND_Q ON
								NND_Q.MaNhomNguoiDung = NND.Ma AND
								NND_Q.MaDoiTuong = @2
							INNER JOIN dbo.Quyen Q ON
								Q.PhamVi = @1 AND
								Q.Ma = NND_Q.MaQuyen AND
								Q.GiaTri = @3
			)
			BEGIN
				SELECT 1
				RETURN
			END
		END

		IF EXISTS(
			SELECT TOP 1 1
				FROM 
					dbo.NhomNguoiDung_CD NND
						INNER JOIN dbo.NhomNguoiDung_CD_NguoiDung NND_ND ON
							NND.MaDoiTuong = @maChuDe AND
							NND_ND.MaNguoiDung = @0 AND
							NND_ND.MaNhomNguoiDung = NND.Ma
						INNER JOIN dbo.NhomNguoiDung_CD_Quyen NND_Q ON
							NND_Q.MaNhomNguoiDung = NND.Ma
						INNER JOIN dbo.Quyen Q ON
							Q.PhamVi = @1 AND
							Q.Ma = NND_Q.MaQuyen AND
							Q.GiaTri = @3
		)
		BEGIN
			SELECT 1
			RETURN
		END
	END
	--Phạm vi hệ thống kiểm soát tất cả phạm vi quyền
	IF (@coQuyenNhomHT = 1)
	BEGIN
		IF EXISTS(
			SELECT TOP 1 1
				FROM 
					dbo.NhomNguoiDung_HT NND
						INNER JOIN dbo.NhomNguoiDung_HT_NguoiDung NND_ND ON
							NND_ND.MaNguoiDung = @0 AND
							NND_ND.MaNhomNguoiDung = NND.Ma
						INNER JOIN dbo.NhomNguoiDung_HT_Quyen NND_Q ON
							NND_Q.MaNhomNguoiDung = NND.Ma
						INNER JOIN dbo.Quyen Q ON
							Q.PhamVi = @1 AND
							Q.Ma = NND_Q.MaQuyen AND
							Q.GiaTri = @3
		)
		BEGIN
			SELECT 1
			RETURN
		END
	END

	SELECT 0
END