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
--Trigger xóa
--Xóa bài viết
--Xóa cột điểm
--Xóa chương trình
--Xóa nhóm người dùng
--Xóa thành viên
--Xóa hình đại diện
ALTER TRIGGER dbo.xoaKhoaHoc_TRIGGER
ON dbo.KhoaHoc
AFTER DELETE
AS
BEGIN
	--Xóa bài viết
	DELETE BVBG
		FROM 
			dbo.BaiVietBaiGiang BVBG
				INNER JOIN deleted d ON
					BVBG.MaKhoaHoc = d.Ma
	DELETE BVBT
		FROM 
			dbo.BaiVietBaiTap BVBT
				INNER JOIN deleted d ON
					BVBT.MaKhoaHoc = d.Ma
	DELETE BVDD
		FROM 
			dbo.BaiVietDienDan BVDD
				INNER JOIN deleted d ON
					BVDD.MaKhoaHoc = d.Ma
	DELETE BVTL
		FROM 
			dbo.BaiVietTaiLieu BVTL
				INNER JOIN deleted d ON
					BVTL.MaKhoaHoc = d.Ma

	--Xóa cột điểm
	DELETE CD
		FROM 
			dbo.CotDiem CD
				INNER JOIN deleted d ON
					CD.MaKhoaHoc = d.Ma

	--Xóa chương trình
	DELETE CT
		FROM 
			dbo.ChuongTrinh CT
				INNER JOIN deleted d ON
					CT.MaKhoaHoc = d.Ma

	--Xóa nhóm người dùng
	DELETE NND
		FROM
			dbo.NhomNguoiDung_KH NND
				INNER JOIN deleted d ON
					NND.MaDoiTuong = d.Ma

	--Xóa thành viên
	DELETE KH_ND
		FROM
			dbo.KhoaHoc_NguoiDung KH_ND
				INNER JOIN deleted d ON
					KH_ND.MaKhoaHoc = d.Ma

	--Xóa hình đại diện
	DELETE TT
		FROM
			dbo.TapTin_KhoaHoc_HinhDaiDien TT
				INNER JOIN deleted d ON
					TT.Ma = d.MaHinhDaiDien
END

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
	INSERT INTO dbo.KhoaHoc (Ten, MoTa, MaHinhDaiDien, MaChuDe, MaNguoiTao, ThoiDiemHetHan, CanDangKy, HanDangKy, PhiThamGia, CheDoRiengTu, SoLuongThanhVien)
		VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8, @9, 1)
		
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

GO
--Xóa theo mã
CREATE PROC dbo.xoaKhoaHocTheoMa (
	@0 INT --Ma
)
AS
BEGIN
	DELETE FROM dbo.KhoaHoc
		WHERE Ma = @0
END

GO
--Lấy phân trang
ALTER PROC dbo.layKhoaHoc_TimKiemPhanTrang (
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
					FROM dbo.KhoaHoc
					' + @0 + '
			) AS KH
			' + @trang + '
			ORDER BY ' + @1 + '
	')
END

GO
--Cập nhật số lượng thành viên của khóa học
ALTER PROC dbo.capNhatKhoaHocTheoMa_SoLuongThanhVien (
	@0 INT --MaKhoaHoc
)
AS
BEGIN
	DECLARE @soLuongThanhVien INT = 0
	SELECT @soLuongThanhVien = COUNT(1)
		FROM dbo.KhoaHoc_NguoiDung
		WHERE 
			MaKhoaHoc = @0 AND
			TrangThai = 0

	UPDATE dbo.KhoaHoc
		SET SoLuongThanhVien = @soLuongThanhVien
		WHERE Ma = @0
END

GO
--Lấy khóa học theo mã, đếm
CREATE PROC dbo.layKhoaHocTheoMa_DemBaiMoiVoiThanhVien (
	@0 INT, --Ma
	@1 INT --MaNguoiDung
)
AS
BEGIN
	DECLARE @maNguoiDung VARCHAR(MAX) = '|' + CAST(@1 AS VARCHAR(MAX)) + '|'
	DECLARE @slBaiMoi INT = 0
	
	SELECT @slBaiMoi += COUNT(1)
		FROM BaiVietDienDan
		WHERE 
			MaKhoaHoc = @0 AND
			DanhSachMaThanhVienDaXem NOT LIKE @maNguoiDung
	
	SELECT @slBaiMoi += COUNT(1)
		FROM BaiVietBaiTap
		WHERE 
			MaKhoaHoc = @0 AND
			DanhSachMaThanhVienDaXem NOT LIKE @maNguoiDung
	
	SELECT @slBaiMoi += COUNT(1)
		FROM BaiVietBaiGiang
		WHERE 
			MaKhoaHoc = @0 AND
			DanhSachMaThanhVienDaXem NOT LIKE @maNguoiDung
	
	SELECT @slBaiMoi += COUNT(1)
		FROM BaiVietTaiLieu
		WHERE 
			MaKhoaHoc = @0 AND
			DanhSachMaThanhVienDaXem NOT LIKE @maNguoiDung


	SELECT *, @slBaiMoi SoLuongBaiMoi
		FROM dbo.KhoaHoc
		WHERE Ma = @0
END