use rtcmfraf_Moodle

GO
--Nhóm người dùng _ Quyền
	--HT - Không có mã đối tượng
	--CD
	--KH
CREATE TABLE dbo.NhomNguoiDung_HT_Quyen (
	MaNhomNguoiDung INT NOT NULL,
	MaQuyen INT NOT NULL,
	MaDoiTuong INT NOT NULL
)

GO
--Cập nhật quyền
ALTER PROC dbo.themHoacXoaNhomNguoiDung_QuyenTheoMaNhomNguoiDungVaMaQuyen (
	@0 NVARCHAR(MAX), --PhamVi
	@1 INT, --MaNhomNguoiDung
	@2 INT, --MaQuyen
	@3 INT, --MaDoiTuong
	@4 BIT --Them
)
AS
BEGIN
	IF (@4 = 1)
	BEGIN
		EXEC('
			INSERT INTO dbo.NhomNguoiDung_' + @0 + '_Quyen (MaNhomNguoiDung, MaQuyen, MaDoiTuong)
				VALUES (' + @1 + ', ' + @2 + ', ' + @3 + ')
		')
	END
	ELSE
	BEGIN
		EXEC('
			DELETE FROM dbo.NhomNguoiDung_' + @0 + '_Quyen
				WHERE
					MaNhomNguoiDung = ' + @1 + ' AND
					MaQuyen = ' + @2 + ' AND
					MaDoiTuong = ' + @3 + '
		')			
	END
END

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
			MaQuyen,
			MaDoiTuong,
			''' + @0 + ''' PhamViNhomNguoiDung
			FROM dbo.NhomNguoiDung_' + @0 + '_Quyen NNQ_Q
			WHERE MaNhomNguoiDung = ' + @1 + '
	')
END