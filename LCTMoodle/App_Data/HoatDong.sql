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

GO
--Thêm hoạt động
CREATE PROC dbo.themHoatDong
(
	@0 INT, --Mã người tác động
	@1 NVARCHAR(MAX), --Loại đối tượng tác động
	@2 INT, --Mã đối tượng tác động
	@3 NVARCHAR(MAX), --Loại đối tượng bị tác động
	@4 INT, --Mã đối tượng bị tác động
	@5 INT --Mã hành động
)
AS
BEGIN
	INSERT INTO dbo.HoatDong (MaNguoiTacDong, LoaiDoiTuongTacDong, MaDoiTuongTacDong, LoaiDoiTuongBiTacDong, MaDoiTuongBiTacDong, MaHanhDong)
	VALUES (@0, @1, @2, @3, @4, @5)
END