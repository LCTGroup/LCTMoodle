use rtcmfraf_Moodle;

GO
--Thành viên (KhoaHoc_NguoiDung)
CREATE TABLE dbo.KhoaHoc_NguoiDung (
	MaKhoaHoc INT NOT NULL,
	MaNguoiDung INT NOT NULL,
	TrangThai INT NOT NULL,
	ThoiDiemThamGia DATETIME NOT NULL DEFAULT(0),
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
	IF (@2 = 4)
	BEGIN
		DELETE FROM dbo.KhoaHoc_NguoiDung
			WHERE 
				MaKhoaHoc = @0 AND
				MaNguoiDung = @1
	END

	INSERT INTO dbo.KhoaHoc_NguoiDung (MaKhoaHoc, MaNguoiDung, TrangThai, MaNguoiThem)
		VALUES (@0, @1, @2, @3)
END