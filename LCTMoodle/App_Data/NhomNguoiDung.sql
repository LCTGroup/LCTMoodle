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
	MaNguoiTao INT NOT NULL,
	GiaTri NVARCHAR(MAX)
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
			*,
			'HT' PhamVi
			FROM dbo.NhomNguoiDung_HT
			WHERE Ma = @@IDENTITY
	END
	ELSE
	BEGIN
		EXEC('
			INSERT INTO dbo.NhomNguoiDung_' + @2 + ' (Ten, MoTa, MaDoiTuong, MaNguoiTao)
				VALUES (N''' + @0 + ''', N''' + @1 + ''', ' + @3 + ', ' + @4 + ')

			SELECT
				*,
				N''' + @2 + ''' PhamVi,
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
			*,
			'HT' PhamVi
			FROM dbo.NhomNguoiDung_HT
	END
	ELSE
	BEGIN
		EXEC('
			SELECT 
				*,
				N''' + @0 + ''' PhamVi
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
			SELECT 
				*,
				'HT' PhamVi
				FROM dbo.NhomNguoiDung_HT
				WHERE Ma = @1
	END
	ELSE
	BEGIN
		EXEC('
			SELECT
				*,
				N''' + @0 + ''' PhamVi
				FROM dbo.NhomNguoiDung_' + @0 + '
				WHERE Ma = ' + @1 + '
		')
	END
END

GO
--Tạo nhóm mặc định
ALTER PROC themNhomNguoiDung_MacDinh (
	@0 NVARCHAR(MAX), --PhamVi
	@1 INT --MaDoiTuong
)
AS
BEGIN
	DECLARE @maNhom INT

	IF (@0 = 'KH')
	BEGIN
		INSERT INTO NhomNguoiDung_KH (Ten, MoTa, MaDoiTuong, MaNguoiTao, GiaTri) VALUES
			(N'Giảng viên', N'Giảng viên của khóa học', @1, 0, 'GiangVien')
			
			SET @maNhom = @@IDENTITY
			INSERT INTO NhomNguoiDung_KH_Quyen (MaNhomNguoiDung, MaQuyen, MaDoiTuong) VALUES
				(@maNhom, 530, @1), --Đặc biệt
				(@maNhom, 503, @1),
				(@maNhom, 504, @1),
				(@maNhom, 510, @1),
				(@maNhom, 511, @1),
				(@maNhom, 524, @1),
				(@maNhom, 513, @1),
				(@maNhom, 514, @1),
				(@maNhom, 515, @1),
				(@maNhom, 526, @1),
				(@maNhom, 527, @1),
				(@maNhom, 528, @1),
				(@maNhom, 529, @1),
				(@maNhom, 525, @1),
				(@maNhom, 516, @1),
				(@maNhom, 517, @1),
				(@maNhom, 518, @1),
				(@maNhom, 519, @1),
				(@maNhom, 523, @1)

		INSERT INTO NhomNguoiDung_KH (Ten, MoTa, MaDoiTuong, MaNguoiTao, GiaTri) VALUES
			(N'Quản lý thành viên', N'Quản lý thành viên', @1, 0, NULL)

			SET @maNhom = @@IDENTITY
			INSERT INTO NhomNguoiDung_KH_Quyen (MaNhomNguoiDung, MaQuyen, MaDoiTuong) VALUES
				(@maNhom, 504, @1)

		INSERT INTO NhomNguoiDung_KH (Ten, MoTa, MaDoiTuong, MaNguoiTao, GiaTri) VALUES
			(N'Quản lý diễn đàn', N'Quản lý diễn đàn', @1, 0, NULL)

			SET @maNhom = @@IDENTITY
			INSERT INTO NhomNguoiDung_KH_Quyen (MaNhomNguoiDung, MaQuyen, MaDoiTuong) VALUES
				(@maNhom, 510, @1),
				(@maNhom, 511, @1)
	END
	ELSE IF (@0 = 'CD')
	BEGIN
		INSERT INTO NhomNguoiDung_CD (Ten, MoTa, MaDoiTuong, MaNguoiTao, GiaTri) VALUES
			(N'Quản lý', N'Quản lý chủ đề', @1, 0, 'QLChuDe')
			
			SET @maNhom = @@IDENTITY
			INSERT INTO NhomNguoiDung_CD_Quyen (MaNhomNguoiDung, MaQuyen, MaDoiTuong) VALUES
				(@maNhom, 304, 0), --Đặc biệt
				(@maNhom, 301, 0),
				(@maNhom, 302, 0),
				(@maNhom, 303, 0)

		INSERT INTO NhomNguoiDung_CD (Ten, MoTa, MaDoiTuong, MaNguoiTao, GiaTri) VALUES
			(N'Quản lý nội dung', N'Quản lý nội dung chủ đề', @1, 0, 'QLNoiDung')

			SET @maNhom = @@IDENTITY
			INSERT INTO NhomNguoiDung_CD_Quyen (MaNhomNguoiDung, MaQuyen, MaDoiTuong) VALUES
				(@maNhom, 302, 0),
				(@maNhom, 303, 0),
				(@maNhom, 530, 0), --Đặc biệt
				(@maNhom, 520, 0),
				(@maNhom, 501, 0)

		INSERT INTO NhomNguoiDung_CD (Ten, MoTa, MaDoiTuong, MaNguoiTao, GiaTri) VALUES
			(N'Quản lý hỏi đáp', N'Quản lý hỏi đáp', @1, 0, 'QLHoiDap')

		INSERT INTO NhomNguoiDung_CD (Ten, MoTa, MaDoiTuong, MaNguoiTao, GiaTri) VALUES
			(N'Quản lý khóa học', N'Quản lý khóa học', @1, 0, 'QLKhoaHoc')

			SET @maNhom = @@IDENTITY
			INSERT INTO NhomNguoiDung_CD_Quyen (MaNhomNguoiDung, MaQuyen, MaDoiTuong) VALUES
				(@maNhom, 530, 0), --Đặc biệt
				(@maNhom, 520, 0),
				(@maNhom, 501, 0)
	END
	ELSE IF (@0 = 'HT')
	BEGIN
		INSERT INTO NhomNguoiDung_HT (Ten, MoTa, MaNguoiTao, GiaTri) VALUES
			(N'Quản lý hệ thống', N'Quản lý hệ thống', 0, 'QL')
			
			SET @maNhom = @@IDENTITY
			INSERT INTO NhomNguoiDung_HT_Quyen (MaNhomNguoiDung, MaQuyen, MaDoiTuong) VALUES
				(@maNhom, 102, 0), --Đặc biệt
				(@maNhom, 101, 0)

		INSERT INTO NhomNguoiDung_HT (Ten, MoTa, MaNguoiTao, GiaTri) VALUES
			(N'Quản lý chức năng hệ thống', N'Quản lý chức năng hệ thống', 0, 'QLHeThong')

			SET @maNhom = @@IDENTITY
			INSERT INTO NhomNguoiDung_HT_Quyen (MaNhomNguoiDung, MaQuyen, MaDoiTuong) VALUES
				(@maNhom, 101, 0)

		INSERT INTO NhomNguoiDung_HT (Ten, MoTa, MaNguoiTao, GiaTri) VALUES
			(N'Quản lý chức năng người dùng', N'Quản lý chức năng người dùng', 0, 'QLNguoiDung')
			
		INSERT INTO NhomNguoiDung_HT (Ten, MoTa, MaNguoiTao, GiaTri) VALUES
			(N'Quản lý chức năng chủ đề', N'Quản lý chức năng chủ đề', 0, 'QLChuDe')

			SET @maNhom = @@IDENTITY
			INSERT INTO NhomNguoiDung_HT_Quyen (MaNhomNguoiDung, MaQuyen, MaDoiTuong) VALUES
				(@maNhom, 304, 0), --Đặc biệt
				(@maNhom, 301, 0),
				(@maNhom, 302, 0),
				(@maNhom, 303, 0)

		INSERT INTO NhomNguoiDung_HT (Ten, MoTa, MaNguoiTao, GiaTri) VALUES
			(N'Quản lý chức năng hỏi đáp', N'Quản lý chức năng hỏi đáp', 0, 'QLHoiDap')

		INSERT INTO NhomNguoiDung_HT (Ten, MoTa, MaNguoiTao, GiaTri) VALUES
			(N'Quản lý chức năng khóa học', N'Quản lý chức năng khóa học', 0, 'QLKhoaHoc')

			SET @maNhom = @@IDENTITY
			INSERT INTO NhomNguoiDung_HT_Quyen (MaNhomNguoiDung, MaQuyen, MaDoiTuong) VALUES
				(@maNhom, 530, 0), --Đặc biệt
				(@maNhom, 520, 0),
				(@maNhom, 501, 0)
	END
END