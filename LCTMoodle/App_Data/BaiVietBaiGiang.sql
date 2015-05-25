use rtcmfraf_Moodle;

GO
--Tạo bài viết bài giảng
CREATE TABLE dbo.BaiVietBaiGiang(
	Ma INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
	TieuDe NVARCHAR(MAX) NOT NULL,
	NoiDung NVARCHAR(MAX) NOT NULL,
	MaTapTin INT DEFAULT NULL,
	ThoiDiemTao DATETIME DEFAULT GETDATE() NOT NULL,
	MaNguoiTao INT NOT NULL,
	MaKhoaHoc INT NOT NULL
)

GO
--Thêm bài viết bài giảng
CREATE PROC dbo.themBaiVietBaiGiang(
	@0 NVARCHAR(MAX), --TieuDe
	@1 NVARCHAR(MAX), --NoiDung
	@2 INT, --MaTapTin
	@3 INT, --MaNguoiTao
	@4 INT --MaKhoaHoc
)
AS
BEGIN
	INSERT INTO dbo.BaiVietBaiGiang(TieuDe, NoiDung, MaTapTin, MaNguoiTao, MaKhoaHoc)
		VALUES (@0, @1, @2, @3, @4)

	SELECT 
		Ma,
		TieuDe,
		NoiDung,
		MaTapTin,
		ThoiDiemTao,
		MaNguoiTao,
		MaKhoaHoc
		FROM dbo.BaiVietBaiGiang
		WHERE Ma = @@IDENTITY
END

GO
--Lấy bài viết bài giảng theo mã khóa học
ALTER PROC dbo.layBaiVietBaiGiangTheoMaKhoaHoc (
	@0 INT --MaKhoaHoc
)
AS
BEGIN
	SELECT 
		Ma,
		TieuDe,
		NoiDung,
		MaTapTin,
		ThoiDiemTao,
		MaNguoiTao,
		MaKhoaHoc
		FROM dbo.BaiVietBaiGiang
		WHERE MaKhoaHoc = @0
		ORDER BY ThoiDiemTao ASC
END