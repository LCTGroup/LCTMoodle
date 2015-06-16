use rtcmfraf_Moodle;

GO
--Thành viên (KhoaHoc_NguoiDung)
CREATE TABLE dbo.KhoaHoc_NguoiDung (
	MaKhoaHoc INT NOT NULL,
	MaNguoiDung INT NOT NULL,
	TrangThai INT NOT NULL,
	ThoiDiemThamGia DATETIME NOT NULL DEFAULT GETDATE(),
	MaNguoiThem INT
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
	--Xóa nếu mời hoặc chặn
	IF (@2 = 2 OR @2 = 3)
	BEGIN
		DELETE FROM dbo.KhoaHoc_NguoiDung
			WHERE 
				MaKhoaHoc = @0 AND
				MaNguoiDung = @1
	END

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