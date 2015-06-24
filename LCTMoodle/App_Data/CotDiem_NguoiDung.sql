use rtcmfraf_Moodle;

GO
--Cột điểm _ Người dùng
CREATE TABLE dbo.CotDiem_NguoiDung (
	MaCotDiem INT NOT NULL,
	MaNguoiDung INT NOT NULL,
	Diem FLOAT(1) NOT NULL,
	NguoiTao INT NOT NULL,
	PRIMARY KEY (MaCotDiem, MaNguoiDung)
)