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
--Hoặc cập nhật nếu đã có
ALTER PROC dbo.themHoacCapNhatBaiTapNop (
	@0 INT, --MaTapTin
	@1 NVARCHAR(MAX), --DuongDan
	@2 INT, --MaNguoiTao
	@3 INT --MaBaiVietBaiTap
)
AS
BEGIN
	UPDATE dbo.BaiTapNop
		SET 
			MaTapTin = @0,
			DuongDan = @1,
			MaNguoiTao = @2,
			MaBaiVietBaiTap = @3
		WHERE 
			MaNguoiTao = @2 AND
			MaBaiVietBaiTap = @3

	IF (@@ROWCOUNT = 0)
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
	ELSE
	BEGIN
		SELECT
			Ma,
			MaTapTin,
			DuongDan,
			ThoiDiemTao,
			MaNguoiTao,
			MaBaiVietBaiTap
			FROM dbo.BaiTapNop
			WHERE 
				MaNguoiTao = @2 AND
				MaBaiVietBaiTap = @3
	END
END

GO
--Lấy bài tập nộp theo mã bài tập
CREATE PROC dbo.layBaiTapNopTheoMaBaiVietBaiTap (
	@0 INT --MaBaiVietBaiTap
)
AS
BEGIN
	SELECT
		Ma,
		MaTapTin,
		DuongDan,
		ThoiDiemTao,
		MaNguoiTao,
		MaBaiVietBaiTap
		FROM dbo.BaiTapNop
		WHERE MaBaiVietBaiTap = @0
END