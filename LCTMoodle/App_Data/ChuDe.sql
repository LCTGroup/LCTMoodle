use rtcmfraf_Moodle;

GO
--Tạo chủ đề
CREATE TABLE dbo.ChuDe (
	Ma INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
	Ten NVARCHAR(MAX) NOT NULL,
	MoTa NVARCHAR(MAX) NOT NULL,
	MaNguoiTao INT NOT NULL,
	ThoiDiemTao DATETIME DEFAULT GETDATE() NOT NULL,
	MaCha INT DEFAULT 0 NOT NULL,
	MaHinhDaiDien INT
);

GO
--Thêm chủ đề
ALTER PROC dbo.themChuDe (
	@0 NVARCHAR(MAX), --Tên chủ đề
	@1 NVARCHAR(MAX), --Mô tả chủ đề
	@2 INT, --Mã người tạo
	@3 INT, --Mã chủ đề cha
	@4 INT --Mã hình đại diện
)
AS
BEGIN
	INSERT INTO dbo.ChuDe (Ten, MoTa, MaNguoiTao, MaCha, MaHinhDaiDien)
		VALUES (@0, @1, @2, @3, @4);

	SELECT
		Ma,
		Ten,
		MoTa,
		MaNguoiTao,
		ThoiDiemTao,
		MaCha,
		MaHinhDaiDien
		FROM dbo.ChuDe
		WHERE Ma = @@IDENTITY;
END

GO
--Lấy chủ đề theo mã chủ đề cha và phạm vi
CREATE PROC dbo.layChuDeTheoMaCha (
	@0 INT --MaCha
)
AS
BEGIN
	SELECT 
		Ma,
		Ten,
		MoTa,
		MaNguoiTao,
		ThoiDiemTao,
		MaCha,
		MaHinhDaiDien
		FROM dbo.ChuDe
		WHERE 
			MaCha = @0
END

GO
--Lấy chủ đề theo mã chủ đề
ALTER PROC dbo.layChuDeTheoMa (
	@0 INT --Ma
)
AS
BEGIN
	SELECT 
		Ma,
		Ten,
		MoTa,
		MaNguoiTao,
		ThoiDiemTao,
		MaCha,
		MaHinhDaiDien
		FROM dbo.ChuDe
		WHERE Ma = @0
END

GO
--Xóa chủ đề theo mã chủ đề
CREATE PROC dbo.xoaChuDeTheoMa (
	@0 INT --Ma
)
AS
BEGIN
	DELETE FROM dbo.ChuDe
		WHERE Ma = @0
END