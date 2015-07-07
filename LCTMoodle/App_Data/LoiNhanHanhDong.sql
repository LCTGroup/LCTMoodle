GO
--Bảng lời nhắn hành động
CREATE TABLE dbo.LoiNhanHanhDong
(
	MaHanhDong INT PRIMARY KEY NOT NULL,
	ChuDong NVARCHAR(MAX) NOT NULL,
	BiDong NVARCHAR(MAX) NOT NULL,
	ChuDongNgoai NVARCHAR(MAX) NOT NULL,
	BiDongNgoai NVARCHAR(MAX) NOT NULL
)