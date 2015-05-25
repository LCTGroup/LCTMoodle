use rtcmfraf_Moodle;

GO
--Bài tập nộp
CREATE TABLE dbo.BaiTapNop (
	Ma INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
	MaTapTin INT DEFAULT NULL,
	DuongDan NVARCHAR(MAX) DEFAULT NULL,
	ThoiDiemTao DATETIME DEFAULT GETDATE() NOT NULL,
	MaNguoiTao INT NOT NULL,
	MaBaiVietBaiTap INT NOT NULL,
)

GO
--Thêm bài tập nộp
CREATE PROC dbo.themBaiTapNop (
	@0 INT, --MaTapTin
	@1 NVARCHAR(MAX), --DuongDan
	@2 INT, --MaNguoiTao
	@3 INT --MaBaiVietBaiTap
)
AS
BEGIN
	INSERT INTO dbo.BaiTapNop (MaTapTin, DuongDan, MaNguoiTao, MaBaiVietBaiTap)
		VALUES (@0, @1, @2, @3)

	SELECT
		Ma,
		MaTapTin,
		DuongDan,
		ThoiDiemTao,
		MaNguoiTao,
		MaBaiVietBaiTap
		FROM dbo.BaiTapNop
		WHERE Ma = @@IDENTITY
END