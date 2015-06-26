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
	DECLARE @coQuyenHT BIT, @coQuyenCD BIT
	SELECT 
		@coQuyenHT = CoQuyenHT,
		@coQuyenCD = CoQuyenCD
		FROM dbo.NguoiDung
		WHERE Ma = @0

	--Chuỗi giá trị chứa danh sách quyền
	DECLARE @giaTri VARCHAR(MAX) = ''

	--Lấy danh sách quyền ở khóa học đó
	SELECT 
		@giaTri += GiaTri + ','
		FROM 
			dbo.NhomNguoiDung_KH_NguoiDung NND_ND
				--Lấy nhóm người dùng mà người dùng thuộc
				INNER JOIN dbo.NhomNguoiDung_KH NND ON 
					NND_ND.MaNguoiDung = @0 AND
					NND_ND.MaNhomNguoiDung = NND.Ma
				--Lấy quyền mà nhóm quyền đó có TRÊN ĐỐI TƯỢNG
				INNER JOIN dbo.NhomNguoiDung_KH_Quyen NND_Q ON
					NND_Q.MaDoiTuong = @2 AND
					NND.Ma = NND_Q.MaNhomNguoiDung
				--Lấy giá trị quyền của nhóm quyền đó
				INNER JOIN dbo.Quyen Q ON 
					NND_Q.MaQuyen = Q.Ma AND
					Q.GiaTri IS NOT NULL AND
					Q.PhamVi = 'KH'

	--Lấy danh sách quyền ở chủ đề của khóa học đó
	IF (@coQuyenCD = 1)
	BEGIN
		--Lấy mã chủ đề của khóa học
		DECLARE @maChuDe INT
		SELECT @maChuDe = MaChuDe
			FROM dbo.KhoaHoc
			WHERE Ma = @2

		--Lấy quyền của người dùng trên chủ đề đó
		SELECT
			@giaTri += GiaTri + ','
			FROM
				dbo.NhomNguoiDung_CD_NguoiDung NND_ND 
					--Lấy nhóm người dùng mà người dùng đó thuộc
					INNER JOIN dbo.NhomNguoiDung_CD NND ON
						NND_ND.MaNguoiDung = @0 AND
						NND_ND.MaNhomNguoiDung = NND.Ma
					--Lấy quyền mà nhóm người dùng đó có TRÊN ĐỐI TƯỢNG
					INNER JOIN dbo.NhomNguoiDung_CD_Quyen NND_Q ON
						NND_Q.MaDoiTuong = @2 AND
						NND.Ma = NND_Q.MaNhomNguoiDung
					--Lấy giá trị quyền của nhóm quyền đó
					INNER JOIN dbo.Quyen Q ON
						NND_Q.MaQuyen = Q.Ma AND
						Q.GiaTri IS NOT NULL AND
						Q.PhamVi = 'KH'
	END

	IF (@coQuyenHT = 1)
	BEGIN
		--Lấy quyền của người dùng trên hệ thống
		SELECT
			@giaTri += GiaTri + ','
			FROM
				dbo.NhomNguoiDung_CD_NguoiDung NND_ND 
					--Lấy nhóm người dùng mà người dùng đó thuộc
					INNER JOIN dbo.NhomNguoiDung_HT NND ON
						NND_ND.MaNguoiDung = @0 AND
						NND_ND.MaNhomNguoiDung = NND.Ma
					--Lấy quyền mà nhóm người dùng đó
					INNER JOIN dbo.NhomNguoiDung_HT_Quyen NND_Q ON
						NND.Ma = NND_Q.MaNhomNguoiDung
					--Lấy giá trị quyền của nhóm quyền đó
					INNER JOIN dbo.Quyen Q ON
						NND_Q.MaQuyen = Q.Ma AND
						Q.GiaTri IS NOT NULL AND
						Q.PhamVi = 'KH'
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
CREATE PROC dbo.layQuyenTheoMaNguoiDungVaGiaTriVaMaDoiTuong_KiemTra (
	@0 INT, --MaNguoiDung
	@1 NVARCHAR(MAX), --GiaTri
	@2 NVARCHAR(MAX), --PhamVi
	@3 INT --MaDoiTuong
)
AS
BEGIN
	IF (@2 = 'HT')
	BEGIN
		SELECT 1
	END
	ELSE IF (@2 = 'KH')
	BEGIN
		SELECT CASE
			WHEN EXISTS(
				SELECT TOP 1 1
					FROM 
						dbo.NhomNguoiDung_KH_NguoiDung NND_ND
							INNER JOIN dbo.NhomNguoiDung_KH NND ON 
								NND_ND.MaNguoiDung = @0
							INNER JOIN dbo.NhomNguoiDung_KH_Quyen NND_Q ON
								NND_Q.MaDoiTuong = @3 AND
								NND.Ma = NND_Q.MaNhomNguoiDung
							INNER JOIN dbo.Quyen Q ON 
								Q.GiaTri = @1 AND
								NND_Q.MaQuyen = Q.Ma) THEN
				1
			ELSE 
				0
			END
	END
END