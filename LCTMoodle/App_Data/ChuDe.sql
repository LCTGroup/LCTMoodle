use rtcmfraf_Moodle;

GO
--Tạo chủ đề
CREATE TABLE dbo.ChuDe (
	Ma INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
	Ten NVARCHAR(MAX) NOT NULL,
	MoTa NVARCHAR(MAX) NOT NULL,
	MaNguoiTao INT NOT NULL,
	ThoiDiemTao DATETIME DEFAULT GETDATE() NOT NULL,
	PhamVi NVARCHAR(MAX) DEFAULT 'HeThong' NOT NULL,
	MaChuDeCha INT DEFAULT 0 NOT NULL,
	MaHinhDaiDien INT DEFAULT NULL
);

GO
--Thêm chủ đề
ALTER PROC dbo.themChuDe (
	@0 NVARCHAR(MAX), --Tên chủ đề
	@1 NVARCHAR(MAX), --Mô tả chủ đề
	@2 INT, --Mã người tạo
	@3 NVARCHAR(MAX), --Phạm vi
	@4 INT, --Mã chủ đề cha
	@5 INT --Mã hình đại diện
)
AS
BEGIN
	INSERT INTO dbo.ChuDe (Ten, MoTa, MaNguoiTao, PhamVi, MaChuDeCha, MaHinhDaiDien)
		VALUES (@0, @1, @2, @3, @4, @5);

	SELECT
		Ma,
		Ten,
		MoTa,
		MaNguoiTao,
		ThoiDiemTao,
		PhamVi,
		MaChuDeCha,
		MaHinhDaiDien
		FROM dbo.ChuDe
		WHERE Ma = @@IDENTITY;
END

GO
--Lấy chủ đề theo mã chủ đề cha và phạm vi
ALTER PROC dbo.layChuDeTheoMaChuDeChaVaPhamVi (
	@0 INT, --Mã chủ đề cha
	@1 NVARCHAR(MAX) --Phạm vi
)
AS
BEGIN
	SELECT 
		Ma,
		Ten,
		MoTa,
		MaNguoiTao,
		ThoiDiemTao,
		PhamVi,
		MaChuDeCha,
		MaHinhDaiDien
		FROM dbo.ChuDe
		WHERE 
			MaChuDeCha = @0 AND
			PhamVi = @1
END