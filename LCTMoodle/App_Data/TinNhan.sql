GO
--Bảng tin nhắn
CREATE TABLE dbo.TinNhan
(
	Ma INT IDENTITY(1,1) NOT NULL,
	MaNguoiGui INT,
	MaNguoiNhan INT,
	NoiDung NVARCHAR(MAX),
	ThoiGianGui DATETIME DEFAULT GETDATE(),
	TrangThaiTinNhan BIT DEFAULT 0
)

GO
--Thêm tin nhắn
CREATE PROC dbo.themTinNhan
(
	@0 INT, --Mã người gửi
	@1 INT, --Mã người nhận
	@2 NVARCHAR(MAX), --Nội dung
	@3 DATETIME, --Thời gian gửi
	@4 BIT --Trạng thái tin nhắn
)
AS
BEGIN
	INSERT INTO dbo.TinNhan (MaNguoiGui, MaNguoiNhan, NoiDung, ThoiGianGui, TrangThaiTinNhan)
	VALUES (@0, @1, @2, @3, @4)
END