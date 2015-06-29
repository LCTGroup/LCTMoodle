use rtcmfraf_Moodle

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

		IF (@@ROWCOUNT = 0)
		BEGIN
			UPDATE dbo.NguoiDung
				SET CoQuyenNhomHT = NULL
				WHERE Ma = @0
		END
	END
	--Phạm vi chủ đề chỉ kiểm soát chủ đề, khóa học, hỏi đáp
	IF (
		@coQuyenNhomCD = 1 AND
		(@1 = 'CD' OR @1 = 'HD' OR @1 = 'KH')
	)
	BEGIN
		SELECT @giaTri += Q.GiaTri + '|'
			FROM 
				dbo.NhomNguoiDung_CD NND
					INNER JOIN dbo.NhomNguoiDung_CD_NguoiDung NND_ND ON
						NND_ND.MaNguoiDung = @0 AND
						NND_ND.MaNhomNguoiDung = NND.Ma
					INNER JOIN dbo.NhomNguoiDung_CD_Quyen NND_Q ON
						NND_Q.MaNhomNguoiDung = NND.Ma AND
						(NND_Q.MaDoiTuong = @2 OR 
							NND_Q.MaDoiTuong = 0)
					INNER JOIN dbo.Quyen Q ON
						Q.PhamVi = @1 AND
						Q.Ma = NND_Q.MaQuyen
			GROUP BY Q.GiaTri

		IF (@@ROWCOUNT = 0)
		BEGIN
			UPDATE dbo.NguoiDung
				SET CoQuyenNhomCD = NULL
				WHERE Ma = @0
		END
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
						NND_Q.MaNhomNguoiDung = NND.Ma AND
						(NND_Q.MaDoiTuong = @2 OR 
							NND_Q.MaDoiTuong = 0)
					INNER JOIN dbo.Quyen Q ON
						Q.PhamVi = @1 AND
						Q.Ma = NND_Q.MaQuyen
			GROUP BY Q.GiaTri

		IF (@@ROWCOUNT = 0)
		BEGIN
			UPDATE dbo.NguoiDung
				SET CoQuyenNhomHT = NULL
				WHERE Ma = @0
		END
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
		IF EXISTS(
			SELECT TOP 1 1
				FROM 
					dbo.NhomNguoiDung_CD NND
						INNER JOIN dbo.NhomNguoiDung_CD_NguoiDung NND_ND ON
							NND_ND.MaNguoiDung = @0 AND
							NND_ND.MaNhomNguoiDung = NND.Ma
						INNER JOIN dbo.NhomNguoiDung_CD_Quyen NND_Q ON
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

	SELECT 0
END