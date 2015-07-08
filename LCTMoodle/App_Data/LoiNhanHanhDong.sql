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

GO
--Lấy lời nhắn hành động theo mã hành động
CREATE PROC dbo.layLoiNhanHanhDongTheoMaHanhDong
(
	@0 INT --Mã hành động
)
AS
BEGIN
	SELECT *
	FROM dbo.LoiNhanHanhDong
	WHERE MaHanhDong = @0
END