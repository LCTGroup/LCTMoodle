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
			
		--Cập nhật CoQuyenNhom
		UPDATE dbo.NguoiDung
			SET CoQuyenNhom' + @0 + ' = 1
			WHERE Ma = ' + @2 + '
	')
END

GO
--Xóa theo mã nhóm người dùng, người dùng
CREATE PROC dbo.xoaNhomNguoiDung_NguoiDungTheoMaNhomNguoiDungVaMaNguoiDung (
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