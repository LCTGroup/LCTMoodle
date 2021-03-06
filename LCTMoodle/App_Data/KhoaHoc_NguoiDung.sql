﻿use rtcmfraf_Moodle;

GO
--Thành viên (KhoaHoc_NguoiDung)
CREATE TABLE dbo.KhoaHoc_NguoiDung (
	MaKhoaHoc INT NOT NULL,
	MaNguoiDung INT NOT NULL,
	TrangThai INT NOT NULL,
	ThoiDiemThamGia DATETIME NOT NULL DEFAULT GETDATE(),
	MaNguoiThem INT,
	LaHocVien BIT DEFAULT 1,
	DiemThaoLuan INT
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

	EXEC capNhatKhoaHocTheoMa_SoLuongThanhVien @0
END

GO
--Thêm danh sách
ALTER PROC dbo.themKhoaHoc_NguoiDung_DanhSach (
	@0 INT, --MaKhoaHoc
	@1 dbo.BangMa READONLY, --Bảng MaNguoiDung
	@2 INT, --TrangThai
	@3 INT --MaNguoiThem
)
AS
BEGIN
	DELETE KH_ND
		FROM dbo.KhoaHoc_NguoiDung KH_ND
			INNER JOIN @1 B ON
				KH_ND.MaNguoiDung = B.Ma

	INSERT INTO dbo.KhoaHoc_NguoiDung (MaKhoaHoc, MaNguoiDung, TrangThai, MaNguoiThem)
		SELECT @0, Ma, @2, @3
			FROM @1

	EXEC capNhatKhoaHocTheoMa_SoLuongThanhVien @0
END

GO
--Lấy theo mã khóa học và mã người dùng
ALTER PROC dbo.layKhoaHoc_NguoiDungTheoMaKhoaHocVaMaNguoiDung (
	@0 INT, --MaKhoaHoc
	@1 INT --MaNguoiDung
)
AS
BEGIN
	SELECT *
		FROM dbo.KhoaHoc_NguoiDung
		WHERE
			MaNguoiDung = @1 AND
			MaKhoaHoc = @0
END

GO
--Xóa theo mã khóa học và mã người dùng
ALTER PROC dbo.xoaKhoaHoc_NguoiDungTheoMaKhoaHocVaMaNguoiDung (
	@0 INT, --MaKhoaHoc
	@1 INT --MaNguoiDung
)
AS
BEGIN
	DELETE FROM dbo.KhoaHoc_NguoiDung
		WHERE 
			MaNguoiDung = @1 AND
			MaKhoaHoc = @0

	EXEC capNhatKhoaHocTheoMa_SoLuongThanhVien @0
END

GO
--Lấy theo mã khóa hoc, trạng thái
ALTER PROC dbo.layKhoaHoc_NguoiDungTheoMaKhoaHocVaTrangThai (
	@0 INT, --MaKhoaHoc
	@1 INT --TrangThai
)
AS
BEGIN
	SELECT *
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

	EXEC capNhatKhoaHocTheoMa_SoLuongThanhVien @0
END

GO
--Cập nhật thuộc tính học viên theo mã người dùng và mã khóa học
CREATE PROC dbo.capNhatKhoaHoc_NguoiDungTheoMaKhoaHocVaMaNguoiDung_LaHocVien (
	@0 INT, --MaKhoaHoc
	@1 INT, --MaNguoiDung
	@2 BIT --LaHocVien
)
AS
BEGIN
	UPDATE dbo.KhoaHoc_NguoiDung
		SET LaHocVien = @2
		WHERE
			MaKhoaHoc = @0 AND
			MaNguoiDung = @1
END

GO
--Lấy theo mã người dùng
ALTER PROC dbo.layKhoaHoc_NguoiDungTheoMaNguoiDung (
	@0 INT --MaNguoiDung
)
AS
BEGIN
	SELECT *
		FROM dbo.KhoaHoc_NguoiDung
		WHERE
			MaNguoiDung = @0
END

GO
--Lấy theo mã người dùng và trạng thái
ALTER PROC dbo.layKhoaHoc_NguoiDungTheoMaNguoiDungVaTrangThai (
	@0 INT, --MaNguoiDung
	@1 INT --TrangThai
)
AS
BEGIN
	SELECT *
		FROM dbo.KhoaHoc_NguoiDung
		WHERE
			MaNguoiDung = @0 AND
			TrangThai = @1
END