use rtcmfraf_Moodle;

GO
--Tạo Hỏi Đáp - Điểm
CREATE TABLE dbo.TraLoi_Diem
(
	MaTraLoi INT NOT NULL,
	MaNguoiTao INT NOT NULL,
	Diem BIT NOT NULL,
	PRIMARY KEY(MaTraLoi, MaNguoiTao)
)