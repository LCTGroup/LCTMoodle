use rtcmfraf_Moodle;

GO
--Tập tin
	--Tam
	--ChuDe_HinhDaiDien	
	--BaiVietDienDan_TapTin
	--BaiVietBaiTap_TapTin
	--NguoiDung_HinhDaiDien
	--BinhLuan_BaiVietDienDan_TapTin
	--BinhLuan_BaiVietBaiGiang_TapTin

CREATE TABLE dbo.TapTin_BaiVietBaiTap_TapTin (
	Ma INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
	Ten NVARCHAR(MAX) NOT NULL,
	Loai NVARCHAR(MAX) NOT NULL,
	ThoiDiemTao DATETIME DEFAULT GETDATE() NOT NULL
);

GO
--Thêm tập tin (chỉ thêm vào bảng tạm)
CREATE PROC dbo.themTapTin (
	@0 NVARCHAR(MAX), --Tên tập tin
	@1 NVARCHAR(MAX) --Loại tập tin
)
AS
BEGIN
	INSERT INTO dbo.TapTin_Tam (Ten, Loai) VALUES
		(@0, @1);

	SELECT 
		Ma,
		Ten,
		Loai,
		ThoiDiemTao
		FROM dbo.TapTin_Tam
		WHERE Ma = @@IDENTITY;
END

GO
--Chuyển tập tin tạm -> bảng chính thức
ALTER PROC dbo.chuyenTapTin (
	@0 INT, --Mã tập tin tạm
	@1 NVARCHAR(MAX) --Tên loại
)
AS
BEGIN
	EXEC ('
		INSERT INTO dbo.TapTin_' + @1 + ' (Ten, Loai)
			SELECT TOP 1 Ten, Loai
				FROM dbo.TapTin_Tam
				WHERE Ma = ' + @0 + ';

		DELETE FROM dbo.TapTin_Tam
			WHERE Ma = ' + @0 + ';

		SELECT
			Ma,
			Ten,
			Loai,
			ThoiDiemTao
			FROM dbo.TapTin_' + @1 + '
			WHERE Ma = @@IDENTITY;
	')
END

GO
--Lấy tập tin
CREATE PROC dbo.layTapTin (
	@0 INT, --Mã tập tin
	@1 NVARCHAR(MAX) --Loại
)
AS
BEGIN
	EXEC ('
		SELECT 
			Ma,
			Ten,
			Loai,
			ThoiDiemTao
			FROM dbo.TapTin_' + @1 + '
			WHERE Ma = ' + @0 + '
	')
END