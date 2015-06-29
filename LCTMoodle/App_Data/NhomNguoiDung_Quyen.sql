use rtcmfraf_Moodle

GO
--Nhóm người dùng _ Quyền
	--HT
	--CD
	--KH
--Mã đối tượng là mã đối tượng mà quyền này ảnh hưởng
CREATE TABLE dbo.NhomNguoiDung_KH_Quyen (
	MaNhomNguoiDung INT NOT NULL,
	MaQuyen INT NOT NULL,
	--Đối tượng mà quyền tác động
	MaDoiTuong INT NOT NULL
)

GO
--Cập nhật quyền
ALTER PROC dbo.themNhomNguoiDung_QuyenTheoMaNhomNguoiDungVaMaQuyen (
	@0 NVARCHAR(MAX), --PhamVi
	@1 INT, --MaNhomNguoiDung
	@2 INT, --MaQuyen
	@3 INT, --MaDoiTuong
	@4 BIT --Là nút lá
)
AS
BEGIN
	EXEC('
		IF (' + @4 + ' = 1)
		BEGIN
			INSERT INTO dbo.NhomNguoiDung_' + @0 + '_Quyen (MaNhomNguoiDung, MaQuyen, MaDoiTuong)
				VALUES (' + @1 + ', ' + @2 + ', ' + @3 + ')
		END
		ELSE
		BEGIN
			DECLARE @chuoiMaLa VARCHAR(MAX) = ''|'' + dbo.layQuyenLa_FUNCTION(' + @2 + ')

			INSERT INTO dbo.NhomNguoiDung_' + @0 + '_Quyen (MaNhomNguoiDung, MaQuyen, MaDoiTuong)
				SELECT ' + @1 + ', Ma, ' + @3 + '
					FROM dbo.Quyen
					WHERE @chuoiMaLa LIKE ''%|'' + CAST(Ma AS VARCHAR(MAX)) + ''|%''
		END
			
		--Cập nhật CoQuyenNhom
		UPDATE ND
			SET ND.CoQuyenNhom' + @0 + ' = 1
			FROM
				NhomNguoiDung_' + @0 + '_NguoiDung NND_ND
					INNER JOIN dbo.NguoiDung ND ON
						NND_ND.MaNhomNguoiDung = ' + @1 + ' AND
						NND_ND.MaNguoiDung = ND.Ma
	')
END

GO
--Xóa quyền của nhóm người dung
CREATE PROC dbo.xoaNhomNguoiDung_QuyenTheoMaNhomNguoiDungVaMaQuyen (
	@0 NVARCHAR(MAX), --PhamVi
	@1 INT, --MaNhomNguoiDung
	@2 INT, --MaQuyen
	@3 INT, --MaDoiTuong
	@4 BIT --Là nút lá
)
AS
BEGIN
	EXEC('
		IF (' + @4 + ' = 1)
		BEGIN
			DELETE FROM dbo.NhomNguoiDung_' + @0 + '_Quyen
			WHERE
				MaNhomNguoiDung = ' + @1 + ' AND
				MaQuyen = ' + @2 + ' AND
				MaDoiTuong = ' + @3 + '
		END
		ELSE
		BEGIN
			DECLARE @chuoiMaLa VARCHAR(MAX) = dbo.layQuyenLa_FUNCTION(' + @2 + ')

			DELETE FROM dbo.NhomNguoiDung_' + @0 + '_Quyen
			WHERE
				MaNhomNguoiDung = ' + @1 + ' AND
				@chuoiMaLa LIKE ''%|'' + CAST(MaQuyen AS VARCHAR(MAX)) + ''|%'' AND
				MaDoiTuong = ' + @3 + '
		END
	')			
END

exec dbo.layNhomNguoiDung_QuyenTheoMaNhomNguoiDung 'CD', 1
GO
--Lấy theo mã nhóm người dùng
ALTER PROC dbo.layNhomNguoiDung_QuyenTheoMaNhomNguoiDung (
	@0 NVARCHAR(MAX), --PhamVi
	@1 INT --MaNhomNguoiDung
)
AS
BEGIN
	EXEC('
		SELECT 
			NND_Q.MaQuyen,
			NND_Q.MaDoiTuong
			FROM 
				dbo.NhomNguoiDung_' + @0 + ' NND
					INNER JOIN dbo.NhomNguoiDung_' + @0 + '_Quyen NND_Q ON
						NND.Ma = ' + @1 + ' AND
						NND_Q.MaNhomNguoiDung = NND.Ma
	')
END