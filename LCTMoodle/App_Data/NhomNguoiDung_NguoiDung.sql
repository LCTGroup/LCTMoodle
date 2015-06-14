use rtcmfraf_Moodle

GO
--Nhóm người dùng _ Người dùng
	--HT
	--CD
	--KH
CREATE TABLE dbo.NhomNguoiDung_CD_NguoiDung (
	MaNhomNguoiDung INT NOT NULL,
	MaNguoiDung INT NOT NULL
)

GO
--Lấy theo mã người dùng
CREATE PROC dbo.layNhomNguoiDung_NguoiDungTheoMaNhomNguoiDungVaMaNguoiDung (
	@0 NVARCHAR(MAX), --PhamVi
	@1 INT, --MaNhomNguoiDung
	@2 INT --MaNguoiDung
)
AS
BEGIN
	EXEC('
		SELECT TOP 1
			MaNhomNguoiDung,
			MaNguoiDung,
			''' + @0 + ''' PhamViNhomNguoiDung
			FROM dbo.NhomNguoiDung_' + @0 + '_NguoiDung
			WHERE 
				MaNhomNguoiDung = ' + @1 + ' AND
				MaNguoiDung = ' + @2 + '

	')
END

GO
--Thêm
ALTER PROC dbo.themNhomNguoiDung_NguoiDung (
	@0 NVARCHAR(MAX), --PhamVi
	@1 INT, --MaNhomNguoiDung
	@2 INT --MaNguoiDung
)
AS
BEGIN
	EXEC('
		INSERT INTO dbo.NhomNguoiDung_' + @0 + '_NguoiDung (MaNhomNguoiDung, MaNguoiDung)
			VALUES (' + @1 + ', ' + @2 + ')
	')
	IF (@0 = 'HT')
	BEGIN
		UPDATE dbo.NguoiDung
			SET CoQuyenHT = 1
			WHERE Ma = @2
	END
END

GO
--Xóa theo mã nhóm người dùng, người dùng
ALTER PROC dbo.xoaNhomNguoiDung_NguoiDungTheoMaNhomNguoiDungVaMaNguoiDung (
	@0 NVARCHAR(MAX), --PhamVi
	@1 INT, --MaNhomNguoiDung
	@2 INT --MaNguoiDung
)
AS
BEGIN
	EXEC('
		DELETE FROM dbo.NhomNguoiDung_' + @0 + '_NguoiDung
			WHERE 
				MaNhomnguoiDung = ' + @1 + ' AND
				MaNguoiDung = ' + @2 + '
	')
END