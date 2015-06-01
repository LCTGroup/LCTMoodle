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
	Han DATETIME DEFAULT NULL,
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
	@5 DATETIME, --Han
	@6 BIT, --CanDangKy
	@7 DATETIME, --HanDangKy
	@8 INT, --PhiThamGia
	@9 NVARCHAR(MAX) --CheDoRiengTu
)
AS
BEGIN
	INSERT INTO dbo.KhoaHoc (Ten, MoTa, MaHinhDaiDien, MaChuDe, MaNguoiTao, Han, CanDangKy, HanDangKy, PhiThamGia, CheDoRiengTu)
		VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8, @9)
		
	SELECT @@IDENTITY AS Ma
END

GO
--Lấy khóa học theo mã
CREATE PROC dbo.layKhoaHocTheoMa(
	@0 INT --Ma
)
AS
BEGIN
	SELECT 
		Ma,
		Ten,
		MoTa,
		MaHinhDaiDien,
		MaChuDe,
		MaNguoiTao,
		ThoiDiemTao,
		Han,
		CanDangKy,
		HanDangKy,
		PhiThamGia,
		CheDoRiengTu,
		CoBangDiem,
		CoBangDiemDanh,
		CanDuyetBaiViet
		FROM dbo.KhoaHoc
		WHERE Ma = @0
END