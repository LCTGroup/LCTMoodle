use rtcmfraf_Moodle;

GO
--Bình luận
	--BaiVietDienDan

CREATE TABLE dbo.BinhLuan_BaiVietDienDan (
	Ma INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
	NoiDung NVARCHAR(MAX) NOT NULL,
	MaTapTin INT DEFAULT NULL,
	MaDoiTuong INT NOT NULL,
	MaNguoiTao INT NOT NULL,
	ThoiDiemTao DATETIME DEFAULT GETDATE()
);

GO
--Thêm bình luận
ALTER PROC dbo.themBinhLuan (
	@0 NVARCHAR(MAX), --NoiDung
	@1 INT, --MaTapTin
	@2 INT, --MaDoiTuong
	@3 INT, --MaNguoiTao
	@4 NVARCHAR(MAX) --Loại đối tượng
)
AS
BEGIN
	EXEC ('
		INSERT INTO dbo.BinhLuan_' + @4 + ' (NoiDung, MaTapTin, MaDoiTuong, MaNguoiTao)
			VALUES (N''' + @0 + ''', ' + @1 + ', ' + @2 + ', ' + @3 + ')

		SELECT 
			Ma,
			NoiDung,
			MaTapTin,
			MaDoiTuong,
			MaNguoiTao,
			ThoiDiemTao,
			''' + @4 + ''' AS LoaiDoiTuong
			FROM dbo.BinhLuan_' + @4 + '
			WHERE Ma = @@IDENTITY
	')
END

GO
--Lấy bình luận theo đối tượng
ALTER PROC dbo.layBinhLuanTheoDoiTuong(
	@0 NVARCHAR(MAX), --Loại đối tượng
	@1 INT --MaDoiTuong
)
AS
BEGIN
	EXEC('
		SELECT 
			Ma,
			NoiDung,
			MaTapTin,
			MaDoiTuong,
			MaNguoiTao,
			ThoiDiemTao,
			''' + @0 + ''' AS LoaiDoiTuong
			FROM dbo.BinhLuan_' + @0 + '
			WHERE MaDoiTuong = ' + @1 + '
	')
END

GO
--Xóa bình luận theo loại và mã
CREATE PROC dbo.xoaBinhLuanTheoMa(
	@0 NVARCHAR(MAX), --Loại đối tượng
	@1 INT --Ma
)
AS
BEGIN
	EXEC('
		DELETE FROM dbo.BinhLuan_' + @0 + '
			WHERE Ma = ' + @1 + '
	')
END