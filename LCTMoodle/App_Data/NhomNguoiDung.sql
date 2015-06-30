use rtcmfraf_Moodle

GO
--Nhóm người dùng
	--HT - Không có mã đối tượng
	--CD
	--KH
CREATE TABLE dbo.NhomNguoiDung_KH (
	Ma INT IDENTITY(1, 1) PRIMARY KEY,
	Ten NVARCHAR(MAX) NOT NULL,
	MoTa NVARCHAR(MAX) NOT NULL,
	MaDoiTuong INT NOT NULL,
	MaNguoiTao INT NOT NULL
)

GO
--Thêm
ALTER PROC dbo.themNhomNguoiDung (
	@0 NVARCHAR(MAX), --Tên
	@1 NVARCHAR(MAX), --MoTa
	@2 NVARCHAR(MAX), --PhamVi
	@3 INT, --MaDoiTuong
	@4 INT --MaNguoiTao
)
AS
BEGIN
	IF (@2 = 'HT')
	BEGIN
		INSERT INTO dbo.NhomNguoiDung_HT (Ten, MoTa, MaNguoiTao)
			VALUES (@0, @1, @4)

		SELECT 
			Ma,
			Ten,
			MoTa,
			'HT' PhamVi,
			MaNguoiTao
			FROM dbo.NhomNguoiDung_HT
			WHERE Ma = @@IDENTITY
	END
	ELSE
	BEGIN
		EXEC('
			INSERT INTO dbo.NhomNguoiDung_' + @2 + ' (Ten, MoTa, MaDoiTuong, MaNguoiTao)
				VALUES (N''' + @0 + ''', N''' + @1 + ''', ' + @3 + ', ' + @4 + ')

			SELECT
				Ma,
				Ten,
				MoTa,
				N''' + @2 + ''' PhamVi,
				MaDoiTuong,
				MaNguoiTao
				FROM dbo.NhomNguoiDung_' + @2 + '
				WHERE Ma = @@IDENTITY
		')
	END
END

GO
--Lấy theo phạm vi và mã đối tượng
ALTER PROC dbo.layNhomNguoiDungTheoMaDoiTuong (
	@0 NVARCHAR(MAX), --PhamVi
	@1 INT --MaDoiTuong
)
AS
BEGIN
	IF (@0 = 'HT')
	BEGIN
		SELECT 
			Ma,
			Ten,
			MoTa,
			'HT' PhamVi,
			MaNguoiTao
			FROM dbo.NhomNguoiDung_HT
	END
	ELSE
	BEGIN
		EXEC('
			SELECT 
				Ma,
				Ten,
				MoTa,
				N''' + @0 + ''' PhamVi,
				MaDoiTuong,
				MaNguoiTao
				FROM dbo.NhomNguoiDung_' + @0 + '
				WHERE MaDoiTuong = ' + @1 + '
		')
	END
END

GO
--Xóa theo mã
CREATE PROC dbo.xoaNhomNguoiDungTheoMa (
	@0 NVARCHAR(MAX), --PhamVi
	@1 INT --Ma
)
AS
BEGIN
	EXEC('
		DELETE FROM dbo.NhomNguoiDung_' + @0 + '
			WHERE Ma = ' + @1 + '
	')
END

GO
--Lấy theo mã
ALTER PROC dbo.layNhomNguoiDungTheoMa (
	@0 NVARCHAR(MAX), --PhamVi
	@1 INT --Ma
)
AS
BEGIN
	IF (@0 = 'HT')
	BEGIN
			SELECT TOP 1
				Ma,
				Ten,
				MoTa,
				'HT' PhamVi,
				MaNguoiTao
				FROM dbo.NhomNguoiDung_HT
				WHERE Ma = @1
	END
	ELSE
	BEGIN
		EXEC('
			SELECT TOP 1
				Ma,
				Ten,
				MoTa,
				N''' + @0 + ''' PhamVi,
				MaDoiTuong,
				MaNguoiTao
				FROM dbo.NhomNguoiDung_' + @0 + '
				WHERE Ma = ' + @1 + '
		')
	END
END