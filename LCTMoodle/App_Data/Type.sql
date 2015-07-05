use rtcmfraf_Moodle;

GO
--Bảng cập nhật chứa dữ liệu cập nhật cho các trường
CREATE TYPE dbo.BangCapNhat
AS
TABLE (
	TenTruong NVARCHAR(MAX) NOT NULL,
	GiaTri NVARCHAR(MAX),
	Loai TINYINT NOT NULL
)

GO
--Bảng cột điểm _ người dùng để cập nhật, thêm
CREATE TYPE dbo.BangCotDiem_NguoiDung
AS
TABLE (
	MaCotDiem INT NOT NULL,
	MaNguoiDung INT NOT NULL,
	Diem FLOAT,
	MaNguoiTao INT NOT NULL
)