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
ALTER PROC dbo.themHoacXoaNhomNguoiDung_QuyenTheoMaNhomNguoiDungVaMaQuyen (
	@0 NVARCHAR(MAX), --PhamVi
	@1 INT, --MaNhomNguoiDung
	@2 INT, --MaQuyen
	@3 INT, --MaDoiTuong
	@4 BIT, --Them
	@5 BIT --Là nút lá
)
AS
BEGIN
	IF (@4 = 1)
	BEGIN
		EXEC('
			IF (' + @5 + ' = 1)
			BEGIN
				INSERT INTO dbo.NhomNguoiDung_' + @0 + '_Quyen (MaNhomNguoiDung, MaQuyen, MaDoiTuong)
					VALUES (' + @1 + ', ' + @2 + ', ' + @3 + ')
			END
			ELSE
			BEGIN
				DECLARE @chuoiMaLa VARCHAR(MAX) = dbo.layQuyenLa_FUNCTION(' + @2 + ')

				INSERT INTO dbo.NhomNguoiDung_' + @0 + '_Quyen (MaNhomNguoiDung, MaQuyen, MaDoiTuong)
					SELECT ' + @1 + ', Ma, ' + @3 + '
						FROM dbo.Quyen
						WHERE @chuoiMaLa LIKE ''%'' + CAST(Ma AS VARCHAR(MAX)) + ''%''
			END
		')
	END
	ELSE
	BEGIN
		EXEC('
			IF (' + @5 + ' = 1)
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
					@chuoiMaLa LIKE ''%'' + CAST(MaQuyen AS VARCHAR(MAX)) + ''%'' AND
					MaDoiTuong = ' + @3 + '
			END
		')			
	END
END
GO
--Lấy theo mã nhóm người dùng
ALTER PROC dbo.layNhomNguoiDung_QuyenTheoMaNhomNguoiDungVaMaDoiTuong (
	@0 NVARCHAR(MAX), --PhamVi
	@1 INT, --MaNhomNguoiDung
	@2 INT --MaDoiTuong
)
AS
BEGIN
	IF (@0 = 'HT')
	BEGIN
		SELECT 
			MaQuyen,
			MaDoiTuong,
			'HT' PhamViNhomNguoiDung
			FROM dbo.NhomNguoiDung_HT_Quyen NNQ_Q
			WHERE MaNhomNguoiDung = @1
	END
	ELSE
	BEGIN
		EXEC('
			SELECT 
				MaQuyen,
				MaDoiTuong,
				''' + @0 + ''' PhamViNhomNguoiDung
				FROM dbo.NhomNguoiDung_' + @0 + '_Quyen NNQ_Q
				WHERE 
					MaNhomNguoiDung = ' + @1 + ' AND
					MaDoiTuong = ' + @2 + '
		')
	END
END