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
--Trigger xóa
--Cập nhật thuộc tính CoQuyenNhom.. của người dùng
CREATE TRIGGER dbo.xoaNhomNguoiDung_KH_NguoiDung_TRIGGER
ON dbo.NhomNguoiDung_KH_NguoiDung
AFTER DELETE
AS
BEGIN
	--Cập nhật thuộc tính CoQuyenNhom.. của người dùng
	UPDATE ND
		SET ND.CoQuyenNhomKH = 0
		FROM 
			(SELECT d.MaNguoiDung
				FROM
					deleted d
						LEFT JOIN dbo.NhomNguoiDung_KH_NguoiDung NND_ND ON
							d.MaNguoiDung = NND_ND.MaNguoiDung
				GROUP BY d.MaNguoiDung
				HAVING COUNT(NND_ND.MaNguoiDung) = 0) d
				INNER JOIN dbo.NguoiDung ND ON
					d.MaNguoiDung = ND.Ma
END

GO
--Trigger thêm
--Cập nhật thuộc tính CoQuyenNhom.. của người dùng
CREATE TRIGGER dbo.themNhomNguoiDung_KH_NguoiDung_TRIGGER
ON dbo.NhomNguoiDung_KH_NguoiDung
AFTER INSERT
AS
BEGIN
	--Cập nhật thuộc tính CoQuyenNhom.. của người dùng
	UPDATE ND
		SET ND.CoQuyenNhomKH = 1
		FROM
			inserted i
				INNER JOIN dbo.NguoiDung ND ON
					i.ManguoiDung = ND.Ma
END

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