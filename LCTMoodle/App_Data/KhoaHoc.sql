use rtcmfraf_Moodle;

GO
--Tạo khóa học
CREATE TABLE dbo.KhoaHoc(
	Ma INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
	Ten NVARCHAR(MAX) NOT NULL,
	MoTa NVARCHAR(MAX) NOT NULL,
	MaHinhDaiDien INT DEFAULT NULL,
	MaChuDe INT NOT NULL,
	MaNguoiTao INT NOT NULL,
	ThoiDiemTao DATETIME DEFAULT GETDATE() NOT NULL,
	ThoiDiemHetHan DATETIME DEFAULT NULL,
	CanDangKy BIT DEFAULT NULL,
	HanDangKy DATETIME DEFAULT NULL,
	PhiThamGia INT DEFAULT NULL,
	CheDoRiengTu NVARCHAR(MAX) NOT NULL,
	CoBangDiem BIT DEFAULT NULL,
	CoBangDiemDanh BIT DEFAULT NULL,
	CanDuyetBaiViet BIT DEFAULT NULL
)

GO
--Thêm khóa học
ALTER PROC dbo.themKhoaHoc(
	@0 NVARCHAR(MAX), --Ten
	@1 NVARCHAR(MAX), --MoTa
	@2 INT, --MaHinhDaiDien
	@3 INT, --MaChuDe
	@4 INT, --MaNguoiTao
	@5 DATETIME, --ThoiDiemHetHan
	@6 BIT, --CanDangKy
	@7 DATETIME, --HanDangKy
	@8 INT, --PhiThamGia
	@9 NVARCHAR(MAX) --CheDoRiengTu
)
AS
BEGIN
	INSERT INTO dbo.KhoaHoc (Ten, MoTa, MaHinhDaiDien, MaChuDe, MaNguoiTao, ThoiDiemHetHan, CanDangKy, HanDangKy, PhiThamGia, CheDoRiengTu)
		VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8, @9)
		
	SELECT *
		FROM dbo.KhoaHoc
		WHERE Ma = @@IDENTITY
END

GO
--Cập nhật theo mã
ALTER PROC dbo.capNhatKhoaHocTheoMa (
	@0 INT, --Mã
	@1 dbo.BangCapNhat READONLY
)
AS
BEGIN
	--Tạo chuỗi gán
	DECLARE @query NVARCHAR(MAX) = dbo.taoChuoiCapNhat(@1)
	IF (@query <> '')
	BEGIN
		EXEC('
			UPDATE dbo.KhoaHoc
				SET ' + @query + '
				WHERE Ma = ' + @0 + '
		')
	END	
	
	SELECT *
		FROM dbo.KhoaHoc
		WHERE Ma = @0
END

GO
--Lấy khóa học theo mã
ALTER PROC dbo.layKhoaHocTheoMa (
	@0 INT --Ma
)
AS
BEGIN
	SELECT *
		FROM dbo.KhoaHoc
		WHERE Ma = @0
END

GO
--Lấy toàn bộ khóa học
ALTER PROC dbo.layKhoaHoc
AS
BEGIN
	SELECT *
		FROM dbo.KhoaHoc
		ORDER BY ThoiDiemTao DESC
END

GO
--Lấy theo từ khóa
ALTER PROC dbo.layKhoaHoc_TimKiem (
	@0 NVARCHAR(MAX) --Từ khóa
)
AS
BEGIN
	SELECT *
		FROM dbo.KhoaHoc
		WHERE Ten LIKE '%' + REPLACE(@0, ' ', '%') + '%'
END

GO
--Lấy theo chủ đề
ALTER PROC dbo.layKhoaHocTheoMaChuDe (
	@0 INT --MaChuDe
)
AS
BEGIN
	SELECT *
		FROM dbo.KhoaHoc
		WHERE MaChuDe = @0
		ORDER BY ThoiDiemTao DESC
END

GO
--Lấy theo từ khóa và mã chủ đề
CREATE PROC dbo.layKhoaHocTheoMaChuDe_TimKiem (
	@0 INT, --MaChuDe
	@1 NVARCHAR(MAX) --Từ khóa
)
AS
BEGIN
	SELECT *
		FROM dbo.KhoaHoc
		WHERE 
			MaChuDe = @0 AND
			Ten LIKE '%' + REPLACE(@1, ' ', '%') + '%'
END