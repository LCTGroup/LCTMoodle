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
	Diem FLOAT(1),
	GhiChu NVARCHAR(MAX),
	DaChuyenDiem BIT DEFAULT 0,
	DaXoa BIT DEFAULT 0
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
			MaBaiVietBaiTap = @3,
			DaXoa = 0,
			ThoiDiemTao = GETDATE()
		WHERE 
			MaNguoiTao = @2 AND
			MaBaiVietBaiTap = @3

	IF (@@ROWCOUNT = 0)
	BEGIN
		INSERT INTO dbo.BaiTapNop (MaTapTin, DuongDan, MaNguoiTao, MaBaiVietBaiTap)
			VALUES (@0, @1, @2, @3)

		SELECT TOP 1 *
			FROM dbo.BaiTapNop
			WHERE Ma = @@IDENTITY
	END
	ELSE
	BEGIN
		SELECT TOP 1 *
			FROM dbo.BaiTapNop
			WHERE 
				MaNguoiTao = @2 AND
				MaBaiVietBaiTap = @3
	END
END

GO
--Lấy bài tập nộp theo mã bài tập
ALTER PROC dbo.layBaiTapNopTheoMaBaiVietBaiTap (
	@0 INT --MaBaiVietBaiTap
)
AS
BEGIN
	SELECT *
		FROM dbo.BaiTapNop
		WHERE MaBaiVietBaiTap = @0
END

GO
--Lấy theo mã
CREATE PROC dbo.layBaiTapNopTheoMa (
	@0 INT --Ma
)
AS
BEGIN
	SELECT TOP 1 *
		FROM dbo.BaiTapNop
		WHERE Ma = @0
END

GO
--Cập nhật điểm
ALTER PROC dbo.capNhatBaiTapNopTheoMa_Diem (
	@0 INT, --Ma
	@1 FLOAT(1) --Diem
)
AS
BEGIN
	UPDATE dbo.BaiTapNop
		SET 
			Diem = @1,
			DaChuyenDiem = 0,
			DaXoa = 0
		WHERE Ma = @0
END

GO
--Cập nhật ghi chú
ALTER PROC dbo.capNhatBaiTapNopTheoMa_GhiChu (
	@0 INT, --Ma
	@1 NVARCHAR(MAX) --GhiChu
)
AS
BEGIN
	UPDATE dbo.BaiTapNop
		SET GhiChu = @1
		WHERE Ma = @0
END

GO
--Lấy theo đã chuyển điểm
CREATE PROC dbo.layBaiTapNopTheoMaBaiVietBaiTapVaDaChuyenDiem (
	@0 INT, --MaBaiVietBaiTap
	@1 BIT --DaChuyenDiem
)
AS
BEGIN
	SELECT *
		FROM dbo.BaiTapNop
		WHERE 
			MaBaiVietBaiTap = @0 AND
			DaChuyenDiem = @1
END

GO
--Cập nhật đã chuyển điểm
CREATE PROC dbo.capNhatBaiTapNopTheoMaBaiVietBaiTap_DaChuyenDiem (
	@0 INT --MaBaiVietBaiTap
)
AS
BEGIN
	UPDATE dbo.BaiTapNop
		SET DaChuyenDiem = 1
		WHERE MaBaiVietBaiTap = @0
END

GO
--Cập nhật xóa
CREATE PROC dbo.capNhatBaiTapNopTheoMa_DaXoa (
	@0 INT, --Ma
	@1 NVARCHAR(MAX) --GhiChu
)
AS
BEGIN
	UPDATE dbo.BaiTapNop
		SET 
			DaXoa = 1,
			GhiChu = @1
		WHERE Ma = @0
END

GO
--Cập nhật xóa
ALTER PROC dbo.capNhatBaiTapNopTheoMa_DaXoa_Nhieu (
	@0 VARCHAR(MAX), --Danh sach ma
	@1 NVARCHAR(MAX) --GhiChu
)
AS
BEGIN
	EXEC('
	UPDATE dbo.BaiTapNop
		SET 
			DaXoa = 1,
			GhiChu = N''' + @1 + '''
		WHERE Ma IN (' + @0 + ')
	')
END