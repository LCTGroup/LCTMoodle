GO
--Bảng hoạt động
CREATE TABLE dbo.HoatDong
(
	Ma INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	MaNguoiTacDong INT NOT NULL,
	LoaiDoiTuongTacDong NVARCHAR(MAX) NOT NULL,
	MaDoiTuongTacDong INT NOT NULL,
	LoaiDoiTuongBiTacDong NVARCHAR(MAX) NOT NULL,
	MaDoiTuongBiTacDong INT NOT NULL,
	MaHanhDong INT NOT NULL,
	ThoiDiem DATETIME DEFAULT GETDATE()
)