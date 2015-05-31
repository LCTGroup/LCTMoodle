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

GO
--Thêm Câu hỏi
CREATE PROC dbo.themCauHoi
(
	@0 NVARCHAR(MAX), --Tiêu đề
	@1 NVARCHAR(MAX), --Nội dung
	@2 DATETIME, --Thời điểm tạo
	@3 INT --Mã người tạo
)
AS
BEGIN
	INSERT INTO dbo.CauHoi(TieuDe, NoiDung, ThoiDiemTao, MaNguoiTao) VALUES (@0, @1, @2, @3)

	SELECT @@IDENTITY Ma
END

GO
--Lấy Câu hỏi
CREATE PROC dbo.layCauHoiTheoMa
(
	@0 INT --Mã câu hỏi
)
AS
BEGIN
	SELECT *
	FROM dbo.CauHoi
	WHERE Ma=@0
END