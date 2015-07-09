use rtcmfraf_Moodle;

GO
--Tạo chủ đề
CREATE TABLE dbo.ChuDe (
	Ma INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
	Ten NVARCHAR(MAX) NOT NULL,
	MoTa NVARCHAR(MAX) NOT NULL,
	MaNguoiTao INT NOT NULL,
	ThoiDiemTao DATETIME DEFAULT GETDATE() NOT NULL,
	MaCha INT DEFAULT 0 NOT NULL,
	MaHinhDaiDien INT,
	Cay NVARCHAR(MAX) NOT NULL DEFAULT '|0|'
)

GO
--Thêm chủ đề
ALTER PROC dbo.themChuDe (
	@0 NVARCHAR(MAX), --Tên chủ đề
	@1 NVARCHAR(MAX), --Mô tả chủ đề
	@2 INT, --Mã người tạo
	@3 INT, --Mã chủ đề cha
	@4 INT --Mã hình đại diện
)
AS
BEGIN
	--Lấy cây của chủ đề cha
	DECLARE @cay NVARCHAR(MAX)
	IF (@3 = 0)
	BEGIN
		SET @cay = '|0|'
	END
	ELSE
	BEGIN
		SELECT @cay = Cay
			FROM dbo.ChuDe
			WHERE Ma = @3
		
		SET @cay += CAST(@3 AS NVARCHAR(MAX)) + '|'
	END

	INSERT INTO dbo.ChuDe (Ten, MoTa, MaNguoiTao, MaCha, MaHinhDaiDien, Cay)
		VALUES (@0, @1, @2, @3, @4, @cay);

	EXEC layChuDeTheoMa @@IDENTITY
END

GO
--Lấy chủ đề theo mã chủ đề cha và phạm vi
ALTER PROC dbo.layChuDeTheoMaCha (
	@0 INT --MaCha
)
AS
BEGIN
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
			CD.MaCha = @0
		GROUP BY 
			CD.Ma,
			CD.Ten,
			CD.MoTa,
			CD.MaNguoiTao,
			CD.ThoiDiemTao,
			CD.MaCha,
			CD.MaHinhDaiDien
END

GO
--Lấy chủ đề theo mã chủ đề
ALTER PROC dbo.layChuDeTheoMa (
	@0 INT --Ma
)
AS
BEGIN
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
			CD.Ma = @0
		GROUP BY 
			CD.Ma,
			CD.Ten,
			CD.MoTa,
			CD.MaNguoiTao,
			CD.ThoiDiemTao,
			CD.MaCha,
			CD.MaHinhDaiDien
END

GO
--Xóa chủ đề theo mã chủ đề
CREATE PROC dbo.xoaChuDeTheoMa (
	@0 INT --Ma
)
AS
BEGIN
	DELETE FROM dbo.ChuDe
		WHERE Ma = @0
END

GO
--Tìm kiếm chủ đề
CREATE PROC dbo.layChuDe_TimKiem (
	@0 NVARCHAR(MAX) --Từ khóa
)
AS
BEGIN
	SELECT 
		Ma,
		Ten,
		MoTa,
		MaNguoiTao,
		ThoiDiemTao,
		MaCha,
		MaHinhDaiDien
		FROM dbo.ChuDe
		WHERE Ten LIKE '%' + REPLACE(@0, ' ', '%') + '%'
END

GO
--Cập nhật chủ đề
CREATE PROC dbo.capNhatChuDeTheoMa (
	@0 INT, --Ma
	@1 dbo.BangCapNhat READONLY
)
AS
BEGIN
	--Tạo chuỗi gán
	DECLARE @query NVARCHAR(MAX) = dbo.taoChuoiCapNhat(@1)
	IF (@query <> '')
	BEGIN
		EXEC('
		UPDATE dbo.ChuDe
			SET ' + @query + '
			WHERE Ma = ' + @0 + '
		')
	END	

	SELECT TOP 1
		Ma,
		Ten,
		MoTa,
		MaNguoiTao,
		ThoiDiemTao,
		MaCha,
		MaHinhDaiDien
		FROM dbo.ChuDe
		WHERE Ma = @0
END

GO
--Cập nhật chủ đề - mã cha
ALTER PROC dbo.capNhatChuDeTheoMa_MaCha (
	@0 INT, --Ma
	@1 INT --MaCha
)
AS
BEGIN
	--Lấy cây của chủ đề cha
	DECLARE @cay NVARCHAR(MAX)
	IF (@1 = 0)
	BEGIN
		SET @cay = '|0|'
	END
	ELSE
	BEGIN
		SELECT @cay = Cay
			FROM dbo.ChuDe
			WHERE Ma = @1
		
		SET @cay += CAST(@1 AS NVARCHAR(MAX)) + '|'
	END

	UPDATE dbo.ChuDe
		SET 
			MaCha = @1,
			Cay = @cay
		WHERE Ma = @0
END

GO
--Lấy phân trang
CREATE PROC dbo.layChuDe_TimKiemPhanTrang (
	@0 NVARCHAR(MAX), --WHERE
	@1 NVARCHAR(MAX), --ORDER
	@2 INT, --Trang
	@3 INT --Số lượng mỗi trang
)
AS
BEGIN
	SET @0 = CASE WHEN @0 IS NULL THEN '' ELSE 'WHERE ' + @0 END
	SET @1 = CASE WHEN @1 IS NULL THEN 'ThoiDiemTao' ELSE @1 END
	
	DECLARE @trang VARCHAR(MAX)
	IF (@2 IS NULL OR @3 IS NULL)
	BEGIN
		SET @trang = ''
	END
	ELSE
	BEGIN
		SET @trang = 'WHERE ' + CAST((@2 - 1) * @3 AS VARCHAR(MAX)) + ' < Dong AND Dong <= ' + CAST(@2 * @3 AS VARCHAR(MAX))
	END

	EXEC('
		SELECT *
			FROM (
				SELECT
					*,
					ROW_NUMBER() OVER (ORDER BY ' + @1 + ') AS Dong,
					COUNT(1) OVER () AS TongSoDong
					FROM dbo.ChuDe
					' + @0 + '
			) AS KH
			' + @trang + '
			ORDER BY ' + @1 + '
	')
END