use rtcmfraf_Moodle;

GO
	
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
	DECLARE @maTapTin VARCHAR(MAX) = CASE 
		WHEN @1 IS NULL THEN 
			'NULL'
		ELSE 
			CAST(@1 AS VARCHAR(MAX))
		END

	EXEC('
		INSERT INTO dbo.BinhLuan_' + @4 + ' (NoiDung, MaTapTin, MaDoiTuong, MaNguoiTao)
			VALUES (N''' + @0 + ''', ' + @maTapTin + ', ' + @2 + ', ' + @3 + ')

		SELECT
			Ma,
			''' + @4 + ''' LoaiDoiTuong,
			NoiDung,
			MaTapTin,
			MaDoiTuong,
			MaNguoiTao,
			ThoiDiemTao
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
			''' + @0 + ''' AS LoaiDoiTuong,
			NoiDung,
			MaTapTin,
			MaDoiTuong,
			MaNguoiTao,
			ThoiDiemTao
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

GO
--Cập nhật theo mã
CREATE PROC dbo.capNhatBinhLuanTheoMa (
	@0 NVARCHAR(MAX), --Loại đối tượng
	@1 INT, --Mã
	@2 dbo.BangCapNhat READONLY
)
AS
BEGIN
	--Tạo chuỗi gán
	DECLARE @query NVARCHAR(MAX) = dbo.taoChuoiCapNhat(@2)
	IF (@query <> '')
	BEGIN
		EXEC('
			UPDATE dbo.BinhLuan_' + @0 + '
				SET ' + @query + '
				WHERE Ma = ' + @1 + '

			SELECT
				Ma,
				''' + @0 + ''' LoaiDoiTuong,
				NoiDung,
				MaTapTin,
				MaDoiTuong,
				MaNguoiTao,
				ThoiDiemTao
				FROM dbo.BinhLuan_' + @0 + '
				WHERE Ma = ' + @1 + '
		')
	END	
END

GO
--Lấy theo mã
ALTER PROC dbo.layBinhLuanTheoMa(
	@0 NVARCHAR(MAX), --LoaiDoiTuong
	@1 INT --Ma
)
AS
BEGIN
	EXEC('
		SELECT TOP 1
			Ma,
			''' + @0 + ''' LoaiDoiTuong,
			NoiDung,
			MaTapTin,
			MaDoiTuong,
			MaNguoiTao,
			ThoiDiemTao
			FROM dbo.BinhLuan_' + @0 + '
			WHERE Ma = ' + @1 + '
	')
END