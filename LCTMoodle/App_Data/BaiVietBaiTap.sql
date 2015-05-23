use rtcmfraf_Moodle;

GO
--Tạo bài viết diễn đàn
CREATE TABLE dbo.BaiVietBaiTap (
	Ma INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
	TieuDe NVARCHAR(MAX) NOT NULL,
	NoiDung NVARCHAR(MAX) NOT NULL,
	MaTapTin INT DEFAULT NULL,
	ThoiDiemHetHan DATETIME DEFAULT NULL,
	ThoiDiemTao DATETIME DEFAULT GETDATE() NOT NULL,
	MaNguoiTao INT NOT NULL,
	MaKhoaHoc INT NOT NULL
)

GO
--Thêm bài viết bài tập
CREATE PROC dbo.themBaiVietBaiTap(
	@0 NVARCHAR(MAX), --TieuDe
	@1 NVARCHAR(MAX), --NoiDung
	@2 INT, --MaTapTin
	@3 DATETIME, --ThoiDiemHetHan
	@4 INT, --MaNguoiTao
	@5 INT --MaKhoaHoc
)
AS
BEGIN
	INSERT INTO dbo.BaiVietBaiTap(TieuDe, NoiDung, MaTapTin, ThoiDiemHetHan, MaNguoiTao, MaKhoaHoc)
		VALUES (@0, @1, @2, @3, @4, @5)

	SELECT 
		Ma,
		TieuDe,
		NoiDung,
		MaTapTin,
		ThoiDiemHetHan,
		ThoiDiemTao,
		MaNguoiTao,
		MaKhoaHoc
		FROM dbo.BaiVietBaiTap
		WHERE Ma = @@IDENTITY
END

GO
--Lấy bài viết bài tập theo mã khóa học
CREATE PROC dbo.layBaiVietBaiTapTheoMaKhoaHoc (
	@0 INT --MaKhoaHoc
)
AS
BEGIN
	SELECT 
		Ma,
		TieuDe,
		NoiDung,
		MaTapTin,
		ThoiDiemHetHan,
		ThoiDiemTao,
		MaNguoiTao,
		MaKhoaHoc
		FROM dbo.BaiVietBaiTap
		WHERE MaKhoaHoc = @0
		ORDER BY ThoiDiemTao DESC
END