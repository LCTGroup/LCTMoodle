GO
--Bảng lời nhắn hành động
CREATE TABLE dbo.LoiNhanHanhDong (
	Ma INT PRIMARY KEY NOT NULL,
	LoiNhan NVARCHAR(MAX) NOT NULL
)

GO
--Lấy hành động theo mã
CREATE PROC dbo.layHanhDongTheoMa (
	@0 INT --Mã
)
AS
BEGIN
	SELECT *
		FROM dbo.HanhDong
		WHERE Ma = @0
END