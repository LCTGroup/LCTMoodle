use rtcmfraf_Moodle;

GO
--Tạo khóa học
CREATE TABLE dbo.KhoaHoc_NguoiDung (
	MaNguoiDung INT NOT NULL,
	MaKhoaHoc INT NOT NULL,
	TrangThai INT NOT NULL,
	ThoiDiemThamGia DATETIME NOT NULL DEFAULT(0),
	MaNguoiThem INT
)