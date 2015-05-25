use rtcmfraf_Moodle;

GO
--Tạo Câu Hỏi
CREATE TABLE dbo.CauHoi
(
	Ma INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	TieuDe NVARCHAR(MAX) NOT NULL,
	NoiDung NVARCHAR(MAX) NOT NULL,
	ThoiDiemTao DATETIME DEFAULT GETDATE(),
	MaNguoiTao INT NOT NULL
)

GO
--Tạo Trả Lời
CREATE TABLE dbo.TraLoi
(
	Ma INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	NoiDung NVARCHAR(MAX) NOT NULL,
	ThoiDiemTao DATETIME DEFAULT GETDATE(),
	Duyet BIT DEFAULT NULL,
	MaNguoiTao INT NOT NULL,
	MaCauHoi INT NOT NULL,
)

GO
--Tạo Hỏi Đáp - Điểm
CREATE TABLE dbo.HoiDap_Diem
(
	Ma INT NOT NULL,
	MaNguoiTao INT NOT NULL,
	Diem BIT NOT NULL,
	PRIMARY KEY(Ma, MaNguoiTao)
)