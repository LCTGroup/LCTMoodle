GO
--Bảng giá trị hoạt động
CREATE TABLE dbo.GiaTriHoatDong
(
	MaHoatDong INT PRIMARY KEY NOT NULL,
	GiaTriCu NVARCHAR(MAX) NOT NULL,
	GiaTriMoi NVARCHAR(MAX) NOT NULL,
	GiaTri NVARCHAR(MAX) NOT NULL
)