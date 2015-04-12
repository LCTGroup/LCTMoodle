use rtcmfraf_Moodle;

--9/4/2015
GO
--Chủ đề
CREATE TABLE dbo.ChuDe (
	Ma INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
	Ten NVARCHAR(MAX) NOT NULL,
	MoTa NVARCHAR(MAX) NOT NULL,
	MaNguoiTao INT NOT NULL,
	ThoiDiemTao DATETIME DEFAULT GETDATE() NOT NULL,
	PhamVi NVARCHAR(MAX) DEFAULT 'HeThong' NOT NULL,
	MaChuDeCha INT DEFAULT 0 NOT NULL,
	MaHinhDaiDien INT
);

--10/4/2015
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
--Tập tin
CREATE TABLE dbo.TapTin (
	Ma INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
	Ten NVARCHAR(MAX) NOT NULL,
	Loai NVARCHAR(MAX) NOT NULL,
	ThuMuc NVARCHAR(MAX) NOT NULL,
	ThoiDiemTao DATETIME DEFAULT GETDATE() NOT NULL
);

--11/4/2015
GO
--Thêm tập tin
ALTER PROC dbo.themTapTin (
	@0 NVARCHAR(MAX), --Tên tập tin
	@1 NVARCHAR(MAX), --Loại tập tin
	@2 NVARCHAR(MAX) --Tên thư mục
)
AS
BEGIN
	INSERT INTO dbo.TapTin (Ten, Loai, ThuMuc) VALUES
		(@0, @1, @2);

	SELECT 
		Ma,
		Ten,
		Loai,
		ThuMuc,
		ThoiDiemTao
		FROM dbo.TapTin
		WHERE Ma = @@IDENTITY;
END;

GO
--Lấy tập tin
CREATE PROC dbo.layTapTinTheoMa (
	@0 INT --Mã tập tin
)
AS
BEGIN
	SELECT 
		Ma,
		Ten,
		Loai,
		ThuMuc,
		ThoiDiemTao
		FROM dbo.TapTin
		WHERE Ma = @0;
END