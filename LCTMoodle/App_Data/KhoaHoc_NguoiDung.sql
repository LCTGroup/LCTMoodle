use rtcmfraf_Moodle;

GO
--Thành viên (KhoaHoc_NguoiDung)
CREATE TABLE dbo.KhoaHoc_NguoiDung (
	MaKhoaHoc INT NOT NULL,
	MaNguoiDung INT NOT NULL,
	TrangThai INT NOT NULL,
	ThoiDiemThamGia DATETIME NOT NULL DEFAULT GETDATE(),
	MaNguoiThem INT,
	LaHocVien BIT DEFAULT 1
)

GO
--Thêm
ALTER PROC dbo.themKhoaHoc_NguoiDung (
	@0 INT, --MaKhoaHoc
	@1 INT, --MaNguoiDung
	@2 INT, --TrangThai
	@3 INT --MaNguoiThem
)
AS
BEGIN
	INSERT INTO dbo.KhoaHoc_NguoiDung (MaKhoaHoc, MaNguoiDung, TrangThai, MaNguoiThem)
		VALUES (@0, @1, @2, @3)
END

GO
--Lấy theo mã khóa học và mã người dùng
ALTER PROC dbo.layKhoaHoc_NguoiDungTheoMaKhoaHocVaMaNguoiDung (
	@0 INT, --MaKhoaHoc
	@1 INT --MaNguoiDung
)
AS
BEGIN
	SELECT TOP 1
		MaKhoaHoc,
		MaNguoiDung,
		TrangThai,
		MaNguoiThem
		FROM dbo.KhoaHoc_NguoiDung
		WHERE
			MaNguoiDung = @1 AND
			MaKhoaHoc = @0
END

GO
--Xóa theo mã khóa học và mã người dùng
CREATE PROC dbo.xoaKhoaHoc_NguoiDungTheoMaKhoaHocVaMaNguoiDung (
	@0 INT, --MaKhoaHoc
	@1 INT --MaNguoiDung
)
AS
BEGIN
	DELETE FROM dbo.KhoaHoc_NguoiDung
		WHERE 
			MaNguoiDung = @1 AND
			MaKhoaHoc = @0
END

GO
--Lấy theo mã khóa hoc, trạng thái
ALTER PROC dbo.layKhoaHoc_NguoiDungTheoMaKhoaHocVaTrangThai (
	@0 INT, --MaKhoaHoc
	@1 INT --TrangThai
)
AS
BEGIN
	SELECT 
		MaKhoaHoc,
		MaNguoiDung,
		TrangThai,
		MaNguoiThem
		FROM dbo.KhoaHoc_NguoiDung
		WHERE
			MaKhoaHoc = @0 AND
			TrangThai = @1
END

GO
--Cập nhật trạng thái theo mã khóa học và mã người dùng
ALTER PROC dbo.capNhatKhoaHoc_NguoiDungTheoMaKhoaHocVaMaNguoiDung_TrangThai (
	@0 INT, --MaKhoaHoc
	@1 INT, --MaNguoiDung
	@2 INT, --TrangThai
	@3 INT --MaNguoiThem
)
AS
BEGIN
	UPDATE KhoaHoc_NguoiDung
		SET 
			TrangThai = @2,
			MaNguoiThem = @3
		WHERE 
			MaKhoaHoc = @0 AND
			MaNguoiDung = @1
END

GO
--Lấy theo mã người dùng
CREATE PROC dbo.layKhoaHoc_NguoiDungTheoMaNguoiDung (
	@0 INT --MaNguoiDung
)
AS
BEGIN
	SELECT 
		MaKhoaHoc,
		MaNguoiDung,
		TrangThai,
		MaNguoiThem
		FROM dbo.KhoaHoc_NguoiDung
		WHERE
			MaNguoiDung = @0
END

GO
--Lấy theo mã người dùng và trạng thái
CREATE PROC dbo.layKhoaHoc_NguoiDungTheoMaNguoiDungVaTrangThai (
	@0 INT, --MaNguoiDung
	@1 INT --TrangThai
)
AS
BEGIN
	SELECT 
		MaKhoaHoc,
		MaNguoiDung,
		TrangThai,
		MaNguoiThem
		FROM dbo.KhoaHoc_NguoiDung
		WHERE
			MaNguoiDung = @0 AND
			TrangThai = @1
END