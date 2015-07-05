use rtcmfraf_Moodle;

GO
--Tập tin
	--Tam
	--ChuDe_HinhDaiDien	
	--KhoaHoc_HinhDaiDien
	--BaiVietDienDan_TapTin
	--BaiVietBaiGiang_TapTin
	--BaiVietTaiLieu_TapTin
	--BaiVietBaiTap_TapTin
	--NguoiDung_HinhDaiDien
	--BinhLuanBaiVietDienDan_TapTin
	--BaiTapNop_TapTin
--Ghi chú cách đặt tên bảng: TapTin_[tên bảng]

CREATE TABLE dbo.TapTin_BinhLuanBaiVietDienDan_TapTin (
	Ma INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
	Ten NVARCHAR(MAX) NOT NULL,
	Loai NVARCHAR(MAX) NOT NULL,
	Duoi NVARCHAR(MAX) NOT NULL,
	ThoiDiemTao DATETIME DEFAULT GETDATE() NOT NULL,
	MaNguoiTao INT
);

ALTER TABLE dbo.TapTin_BaiTapNop_TapTin
	ALTER COLUMN MaNguoiTao INT

GO
--Thêm tập tin (chỉ thêm vào bảng tạm)
ALTER PROC dbo.themTapTin (
	@0 NVARCHAR(MAX), --Ten
	@1 NVARCHAR(MAX), --Loai
	@2 NVARCHAR(MAX), --Duoi
	@3 INT --MaNguoiTao
)
AS
BEGIN
	INSERT INTO dbo.TapTin_Tam (Ten, Loai, Duoi, MaNguoiTao) VALUES
		(@0, @1, @2, @3);

	SELECT 
		Ma,
		Ten,
		Loai,
		Duoi,
		ThoiDiemTao,
		MaNguoiTao
		FROM dbo.TapTin_Tam
		WHERE Ma = @@IDENTITY;
END

GO
--Chuyển tập tin tạm -> bảng chính thức
ALTER PROC dbo.chuyenTapTin (
	@0 NVARCHAR(MAX), --Loại
	@1 INT --Mã tập tin tạm
)
AS
BEGIN
	EXEC ('
		INSERT INTO dbo.TapTin_' + @0 + ' (Ten, Loai, Duoi, MaNguoiTao)
			SELECT TOP 1 Ten, Loai, Duoi, MaNguoiTao
				FROM dbo.TapTin_Tam
				WHERE Ma = ' + @1 + ';

		DELETE FROM dbo.TapTin_Tam
			WHERE Ma = ' + @1 + ';

		SELECT
			Ma,
			Ten,
			Loai,
			Duoi,
			ThoiDiemTao,
			MaNguoiTao
			FROM dbo.TapTin_' + @0 + '
			WHERE Ma = @@IDENTITY;
	')
END

GO
--Lấy tập tin
ALTER PROC dbo.layTapTinTheoMa (
	@0 NVARCHAR(MAX), --Loại
	@1 INT --Mã
)
AS
BEGIN
	EXEC ('
		SELECT 
			Ma,
			Ten,
			Loai,
			Duoi,
			ThoiDiemTao,
			MaNguoiTao
			FROM dbo.TapTin_' + @0 + '
			WHERE Ma = ' + @1 + '
	')
END