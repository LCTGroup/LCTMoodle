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
	INSERT INTO dbo.ChuDe (Ten, MoTa, MaNguoiTao, MaCha, MaHinhDaiDien)
		VALUES (@0, @1, @2, @3, @4);

	SELECT *
		FROM dbo.ChuDe
		WHERE Ma = @@IDENTITY;
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
CREATE PROC dbo.capNhatChuDeTheoMa_MaCha (
	@0 INT, --Ma
	@1 INT --MaCha
)
AS
BEGIN
	UPDATE dbo.ChuDe
		SET MaCha = @1
		WHERE Ma = @0
END