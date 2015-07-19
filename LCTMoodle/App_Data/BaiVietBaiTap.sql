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
	MaKhoaHoc INT NOT NULL,
	Loai INT NOT NULL,
	ThoiDiemCapNhat DATETIME,
	CachNop INT NOT NULL DEFAULT 0,
	DanhSachMaThanhVienDaXem VARCHAR(MAX) NOT NULL DEFAULT '|'
)

GO
--Trigger xóa
--Xóa tập tin
--Xóa bài nộp
CREATE TRIGGER dbo.xoaBaiVietBaiTap_TRIGGER
ON dbo.BaiVietBaiTap
AFTER DELETE
AS
BEGIN
	--Xóa tập tin
	DELETE TT
		FROM 
			dbo.TapTin_BaiVietBaiTap_TapTin TT
				INNER JOIN deleted d ON
					TT.Ma = d.MaTapTin

	--Xóa bài nộp
	DELETE BTN
		FROM
			dbo.BaiTapNop BTN
				INNER JOIN deleted d ON
					BTN.MaBaiVietBaiTap = d.Ma
END

GO
--Lấy bài tập người dùng cần hoàn thành
CREATE PROC dbo.layBaiVietBaiTap_NguoiDungCanHoanThanh (
	@0 INT --Mã người dùng
)
AS
BEGIN
	DECLARE @thoiDiemHienTai DATETIME = GETDATE()

	SELECT BVBT.*
		FROM
			--Lấy khóa học mà người dùng là học viên
			dbo.KhoaHoc_NguoiDung KH_ND
				INNER JOIN dbo.BaiVietBaiTap BVBT ON
					KH_ND.MaNguoiDung = @0 AND
					BVBT.MaKhoaHoc = KH_ND.MaKhoaHoc AND
					BVBT.ThoiDiemHetHan IS NOT NULL AND
					BVBT.ThoiDiemHetHan > @thoiDiemHienTai AND
					KH_ND.TrangThai = 0 AND
					KH_ND.LaHocVien = 1 AND
					NOT EXISTS (
						SELECT 1
							FROM dbo.BaiTapNop BTN
							WHERE 
								BVBT.Ma = BTN.MaBaiVietBaiTap AND
								BTN.MaNguoiTao = @0 AND
								BTN.DaXoa <> 1
					)
END

GO
--Thêm bài viết bài tập
ALTER PROC dbo.themBaiVietBaiTap(
	@0 NVARCHAR(MAX), --TieuDe
	@1 NVARCHAR(MAX), --NoiDung
	@2 INT, --MaTapTin
	@3 INT, --Loai
	@4 INT, --CachNop
	@5 DATETIME, --ThoiDiemHetHan
	@6 INT, --MaNguoiTao
	@7 INT --MaKhoaHoc
)
AS
BEGIN
	INSERT INTO dbo.BaiVietBaiTap(TieuDe, NoiDung, MaTapTin, Loai, CachNop, ThoiDiemHetHan, MaNguoiTao, MaKhoaHoc)
		VALUES (@0, @1, @2, @3, @4, @5, @6, @7)

	SELECT *
		FROM dbo.BaiVietBaiTap
		WHERE Ma = @@IDENTITY
END

GO
--Lấy bài viết bài tập theo mã khóa học
ALTER PROC dbo.layBaiVietBaiTapTheoMaKhoaHoc (
	@0 INT --MaKhoaHoc
)
AS
BEGIN
	SELECT *
		FROM dbo.BaiVietBaiTap
		WHERE MaKhoaHoc = @0
		ORDER BY ThoiDiemTao DESC
END

GO
--Xóa bài viết bài tập theo mã
ALTER PROC dbo.xoaBaiVietBaiTapTheoMa (
	@0 INT --Ma
)
AS
BEGIN
	DELETE FROM dbo.BaiVietBaiTap
		WHERE Ma = @0
END

GO
--Lấy theo mã
ALTER PROC dbo.layBaiVietBaiTapTheoMa (
	@0 INT --Ma
)
AS
BEGIN
	SELECT *
		FROM dbo.BaiVietBaiTap
		WHERE Ma = @0
END

GO
--Cập nhật theo mã
ALTER PROC dbo.capNhatBaiVietBaiTapTheoMa (
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
		UPDATE dbo.BaiVietBaiTap
			SET ' + @query + '
			WHERE Ma = ' + @0 + '
		')
	END	
	
	SELECT *
		FROM dbo.BaiVietBaiTap
		WHERE Ma = @0
END

GO
--Cập nhật đã xem bài viết
ALTER PROC dbo.capNhatBaiVietBaiTapTheoMa_Xem (
	@0 INT, --Ma
	@1 INT --MaNguoiDung
)
AS
BEGIN
	DECLARE @maNguoiDung VARCHAR(MAX) = CAST(@1 AS VARCHAR(MAX)) + '|'
	UPDATE dbo.BaiVietBaiTap
		SET DanhSachMaThanhVienDaXem = REPLACE(DanhSachMaThanhVienDaXem, '|' + @maNguoiDung, '|') + @maNguoiDung
		WHERE Ma = @0
END