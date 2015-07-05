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
	--Lấy nhóm mà người dùng thuộc
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
	DECLARE @dsGiaTri TABLE (
		GiaTri VARCHAR(MAX)
	)

	--Kiểm tra ở phạm vi nhóm người dùng khóa học
	--Nhóm khóa học chỉ có quyền khóa học nên nếu cần lấy quyền ở khóa học
	--thì mới cần thực hiện block này
	IF (
		@coQuyenNhomKH = 1 AND
		@1 = 'KH'
	)
	BEGIN
		INSERT @dsGiaTri (GiaTri)
			SELECT Q.GiaTri
				FROM 
					--Để lấy nhóm cần kiểm tra
					dbo.NhomNguoiDung_KH NND
						--Để lấy nhóm mà người đó thuộc
						INNER JOIN dbo.NhomNguoiDung_KH_NguoiDung NND_ND ON
							NND.Ma = NND_ND.MaNhomNguoiDung AND
							NND.MaDoiTuong = @2 AND
							NND_ND.MaNguoiDung = @0
						--Để lấy quyền mà nhóm quyền đó có
						INNER JOIN dbo.NhomNguoiDung_KH_Quyen NND_Q ON
							NND_Q.MaNhomNguoiDung = NND_ND.MaNhomNguoiDung AND
							--Quyền tác động đến đối tượng đó
							NND_Q.MaDoiTuong = @2
						--Để lấy quyền theo phạm vi
						INNER JOIN dbo.Quyen Q ON
							NND_Q.MaQuyen = Q.Ma AND
							--Phạm vi cần tìm
							Q.PhamVi = @1
	END

	IF (@coQuyenNhomCD = 1 AND
		(@1 = 'KH' OR
			@1 = 'HD' OR
			@1 = 'CD'))
	BEGIN
		--Lấy cây để kiểm tra đối tượng nhóm người dùng thuộc đối với chủ đề
		--Lấy mã chủ đề bị tác động
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
			SET @maChuDe = @2
		END
			
		--Lấy cây của chủ đề
		DECLARE @cay VARCHAR(MAX)
		IF (@maChuDe = 0)
		BEGIN
			SET @cay = '|0|'
		END
		ELSE
		BEGIN
		SELECT @cay = Cay
			FROM dbo.ChuDe
			WHERE Ma = @maChuDe

			--Cộng thêm chủ đề hiện tại
			SET @cay += CAST(@maChuDe AS VARCHAR(MAX)) + '|'
		END

		--Kiểm tra ở phạm vi chủ đề
		--Phạm vi chủ đề chỉ quản lý khóa hoc, hỏi đáp, chủ đề
		IF (@coQuyenNhomCD = 1)
		BEGIN
			--Lấy giá trị quyền tác động đến nó
			INSERT @dsGiaTri (GiaTri)
				SELECT Q.GiaTri
					FROM 
						--Để lấy nhóm cần kiểm tra
						dbo.NhomNguoiDung_CD NND
							--Để lấy nhóm mà người đó thuộc
							INNER JOIN dbo.NhomNguoiDung_CD_NguoiDung NND_ND ON
								NND.Ma = NND_ND.MaNhomNguoiDung AND
								--Những nhóm trong cây
								@cay LIKE '%|' + CAST(NND.MaDoiTuong AS VARCHAR(MAX)) + '|%' AND
								NND_ND.MaNguoiDung = @0
							--Để lấy quyền mà nhóm quyền đó có
							INNER JOIN dbo.NhomNguoiDung_CD_Quyen NND_Q ON
								NND_Q.MaNhomNguoiDung = NND_ND.MaNhomNguoiDung AND
								--Quyền tác động đến đối tượng đó
								(NND_Q.MaDoiTuong = 0 OR 
									(@1 = 'CD' AND 
										@cay LIKE '%|' + CAST(NND_Q.MaDoiTuong AS VARCHAR(MAX)) + '|%'))
							--Để lấy quyền theo phạm vi
							INNER JOIN dbo.Quyen Q ON
								NND_Q.MaQuyen = Q.Ma AND
								--Phạm vi cần tìm
								Q.PhamVi = @1
		END
	END
	
	--Kiểm tra ở phạm vi hệ thống
	IF (
		@coQuyenNhomHT = 1
	)
	BEGIN
		--Lấy giá trị quyền tác động đến nó
		INSERT @dsGiaTri (GiaTri)
			SELECT Q.GiaTri
				FROM 
					--Để lấy nhóm mà người đó thuộc
					dbo.NhomNguoiDung_HT_NguoiDung NND_ND
						--Để lấy quyền mà nhóm quyền đó có
						INNER JOIN dbo.NhomNguoiDung_HT_Quyen NND_Q ON
							NND_ND.MaNguoiDung = @0 AND
							NND_Q.MaNhomNguoiDung = NND_ND.MaNhomNguoiDung
						--Để lấy quyền theo phạm vi
						INNER JOIN dbo.Quyen Q ON
							NND_Q.MaQuyen = Q.Ma AND
							--Phạm vi cần tìm
							Q.PhamVi = @1
	END

	DECLARE @giaTri VARCHAR(MAX) = ''
	SELECT @giaTri += GiaTri + '|'
		FROM @dsGiaTri
		GROUP BY GiaTri

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
	--Lấy nhóm mà người dùng thuộc
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

	--Kiểm tra ở phạm vi nhóm người dùng khóa học
	--Nhóm khóa học chỉ có quyền khóa học nên nếu cần lấy quyền ở khóa học
	--thì mới cần thực hiện block này
	IF (
		@coQuyenNhomKH = 1 AND
		@1 = 'KH'
	)
	BEGIN
		IF (EXISTS(
			SELECT 1
				FROM 
					--Để lấy nhóm cần kiểm tra
					dbo.NhomNguoiDung_KH NND
						--Để lấy nhóm mà người đó thuộc
						INNER JOIN dbo.NhomNguoiDung_KH_NguoiDung NND_ND ON
							NND.Ma = NND_ND.MaNhomNguoiDung AND
							NND.MaDoiTuong = @2 AND
							NND_ND.MaNguoiDung = @0
						--Để lấy quyền mà nhóm quyền đó có
						INNER JOIN dbo.NhomNguoiDung_KH_Quyen NND_Q ON
							NND_Q.MaNhomNguoiDung = NND_ND.MaNhomNguoiDung AND
							--Quyền tác động đến đối tượng đó
							NND_Q.MaDoiTuong = @2
						--Để lấy quyền theo phạm vi
						INNER JOIN dbo.Quyen Q ON
							NND_Q.MaQuyen = Q.Ma AND
							--Phạm vi, giá trị cần tìm
							Q.GiaTri = @3 AND
							Q.PhamVi = @1))
		BEGIN
			SELECT 1
			RETURN
		END
	END

	IF (@coQuyenNhomCD = 1 AND
		(@1 = 'KH' OR
			@1 = 'HD' OR
			@1 = 'CD'))
	BEGIN
		--Lấy cây để kiểm tra đối tượng nhóm người dùng thuộc đối với chủ đề
		--Lấy mã chủ đề bị tác động
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
			SET @maChuDe = @2
		END
			
		--Lấy cây của chủ đề
		DECLARE @cay VARCHAR(MAX)
		IF (@maChuDe = 0)
		BEGIN
			SET @cay = '|0|'
		END
		ELSE
		BEGIN
		SELECT @cay = Cay
			FROM dbo.ChuDe
			WHERE Ma = @maChuDe

			--Cộng thêm chủ đề hiện tại
			SET @cay += CAST(@maChuDe AS VARCHAR(MAX)) + '|'
		END

		--Cộng thêm chủ đề hiện tại
		SET @cay += CAST(@maChuDe AS VARCHAR(MAX)) + '|'

		--Kiểm tra ở phạm vi chủ đề
		--Phạm vi chủ đề chỉ quản lý khóa hoc, hỏi đáp, chủ đề
		IF (@coQuyenNhomCD = 1)
		BEGIN
			--Lấy giá trị quyền tác động đến nó
			IF (EXISTS(
				SELECT 1
					FROM 
						--Để lấy nhóm cần kiểm tra
						dbo.NhomNguoiDung_CD NND
							--Để lấy nhóm mà người đó thuộc
							INNER JOIN dbo.NhomNguoiDung_CD_NguoiDung NND_ND ON
								NND.Ma = NND_ND.MaNhomNguoiDung AND
								--Những nhóm trong cây
								@cay LIKE '%|' + CAST(NND.MaDoiTuong AS VARCHAR(MAX)) + '|%' AND
								NND_ND.MaNguoiDung = @0
							--Để lấy quyền mà nhóm quyền đó có
							INNER JOIN dbo.NhomNguoiDung_CD_Quyen NND_Q ON
								NND_Q.MaNhomNguoiDung = NND_ND.MaNhomNguoiDung AND
								--Quyền tác động đến đối tượng đó
								(NND_Q.MaDoiTuong = 0 OR 
									(@1 = 'CD' AND 
										@cay LIKE '%|' + CAST(NND_Q.MaDoiTuong AS VARCHAR(MAX)) + '|%'))
							--Để lấy quyền theo phạm vi
							INNER JOIN dbo.Quyen Q ON
								NND_Q.MaQuyen = Q.Ma AND
								--Phạm vi cần tìm
								Q.GiaTri = @3 AND
								Q.PhamVi = @1))
			BEGIN
				SELECT 1
				RETURN
			END
		END
	END
	
	--Kiểm tra ở phạm vi hệ thống
	IF (
		@coQuyenNhomHT = 1
	)
	BEGIN
		--Lấy giá trị quyền tác động đến nó
		IF (EXISTS(
			SELECT 1
				FROM 
					--Để lấy nhóm mà người đó thuộc
					dbo.NhomNguoiDung_HT_NguoiDung NND_ND
						--Để lấy quyền mà nhóm quyền đó có
						INNER JOIN dbo.NhomNguoiDung_HT_Quyen NND_Q ON
							NND_ND.MaNguoiDung = @0 AND
							NND_Q.MaNhomNguoiDung = NND_ND.MaNhomNguoiDung
						--Để lấy quyền theo phạm vi
						INNER JOIN dbo.Quyen Q ON
							NND_Q.MaQuyen = Q.Ma AND
							--Phạm vi cần tìm
							Q.GiaTri = @3 AND
							Q.PhamVi = @1))
		BEGIN
			SELECT 1
			RETURN
		END
	END

	SELECT 0
END