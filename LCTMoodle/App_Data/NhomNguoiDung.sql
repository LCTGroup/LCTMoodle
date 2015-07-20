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
--Trigger xóa nhóm người dùng
--Xóa nhóm người dùng _ người dùng
--Xóa nhóm người dùng _ quyền
ALTER TRIGGER dbo.xoaNhomNguoiDung_KH_TRIGGER
ON dbo.NhomNguoiDung_KH
AFTER DELETE
AS
BEGIN
	--Xóa nhóm người dùng _ người dùng
	DELETE NND_ND
		FROM 
			dbo.NhomNguoiDung_KH_NguoiDung NND_ND
				INNER JOIN deleted d ON
					NND_ND.MaNhomNguoiDung = d.Ma

	--Xóa nhóm người dùng _ quyền
	DELETE NND_Q
		FROM
			dbo.NhomNguoiDung_KH_Quyen NND_Q
				INNER JOIN deleted d ON
					NND_Q.MaNhomNguoiDung = d.Ma
END

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
				N''' + @2 + ''' PhamVi
				FROM dbo.NhomNguoiDung_' + @2 + '
				WHERE Ma = @@IDENTITY
		')
	END
END

GO
--Cập nhật theo mã
ALTER PROC dbo.capNhatNhomNguoiDungTheoMa (
	@0 VARCHAR(MAX), --PhamVi
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
			UPDATE dbo.NhomNguoiDung_' + @0 + '
				SET ' + @query + '
				WHERE Ma = ' + @1 + '
		')
	END	
	
	EXEC('
		SELECT *
			FROM dbo.NhomNguoiDung_' + @0 + '
			WHERE Ma = ' + @1 + '
	')
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
ALTER PROC dbo.xoaNhomNguoiDungTheoMa (
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
--Lấy đối tượng mà quyền chung tác động
ALTER PROC dbo.layNhomNguoiDungTheoMaNguoiDungVaPhamViQuyenVaGiaTriQuyen_ChuoiMaDoiTuong (
	@0 VARCHAR(MAX), --Phạm vi
	@1 INT, --MaNguoiDung
	@2 VARCHAR(MAX), --PhamViQuyen
	@3 VARCHAR(MAX) --GiaTriQuyen
)
AS
BEGIN
	IF (@0 = 'HT')
	BEGIN
		IF (EXISTS(
			SELECT 1
				FROM dbo.NguoiDung
				WHERE
					Ma = @1 AND
					CoQuyenNhomHT = 1		
		))
		BEGIN
			DECLARE @dsMaDoiTuong VARCHAR(MAX) = ''
			SELECT @dsMaDoiTuong = '0'
				FROM
					--Lấy nhóm mà người dùng thuộc
					dbo.NhomNguoiDung_HT_NguoiDung NND_ND
						--Quyền mà nhóm có
						INNER JOIN dbo.NhomNguoiDung_HT_Quyen NND_Q ON
							NND_ND.MaNguoiDung = @1 AND
							NND_ND.MaNhomNguoiDung = NND_Q.MaNhomNguoiDung
						--Chi tiết quyền
						INNER JOIN dbo.Quyen Q ON
							NND_Q.MaQuyen = Q.Ma AND						
							(Q.GiaTri = @3 OR
								Q.GiaTri = 'QLDB') AND
							Q.PhamVi = @2
						INNER JOIN dbo.NhomNguoiDung_HT NND ON
							NND_ND.MaNhomNguoiDung = NND.Ma

			SELECT CASE
				WHEN @dsMaDoiTuong = '' THEN
					NULL
				ELSE
					@dsMaDoiTuong
				END
		END
		ELSE
		BEGIN
			SELECT NULL
		END
	END
	ELSE
	BEGIN
		EXEC('
			IF (EXISTS(
				SELECT 1
					FROM dbo.NguoiDung
					WHERE
						Ma = ' + @1 + ' AND
						CoQuyenNhom' + @0 + ' = 1		
			))
			BEGIN
				DECLARE @dsMaDoiTuong VARCHAR(MAX) = ''''
				SELECT @dsMaDoiTuong += CAST(NND.MaDoiTuong AS VARCHAR(MAX)) + ''|''
					FROM
						--Lấy nhóm mà người dùng thuộc
						dbo.NhomNguoiDung_' + @0 + '_NguoiDung NND_ND
							--Quyền mà nhóm có
							INNER JOIN dbo.NhomNguoiDung_' + @0 + '_Quyen NND_Q ON
								NND_ND.MaNguoiDung = ' + @1 + ' AND
								NND_ND.MaNhomNguoiDung = NND_Q.MaNhomNguoiDung
							--Chi tiết quyền
							INNER JOIN dbo.Quyen Q ON
								NND_Q.MaQuyen = Q.Ma AND
								(Q.GiaTri = ''' + @3 + ''' OR
									Q.GiaTri = ''QLDB'') AND
								Q.PhamVi = ''' + @2 + '''
							INNER JOIN dbo.NhomNguoiDung_' + @0 + ' NND ON
								NND_ND.MaNhomNguoiDung = NND.Ma
					GROUP BY NND.MaDoiTuong

				SELECT CASE
					WHEN @dsMaDoiTuong = '''' THEN
						NULL
					ELSE
						LEFT(@dsMaDoiTuong, LEN(@dsMaDoiTuong) - 1)
					END
			END
			ELSE
			BEGIN
				SELECT NULL
			END
		')
	END
END

GO
--Lấy nhóm quyền của người dùng
ALTER PROC dbo.layNhomNguoiDungTheoMaNguoiDung (
	@0 VARCHAR(MAX), --PhamVi
	@1 INT --MaNguoiDung
)
AS
BEGIN
	EXEC('
		SELECT NND.*
			FROM 
				NhomNguoiDung_' + @0 + '_NguoiDung NND_ND
					INNER JOIN NhomNguoiDung_' + @0 + ' NND ON
						NND_ND.MaNguoiDung = ' + @1 + ' AND
						NND_ND.MaNhomNguoiDung = NND.Ma
	')
END

GO
--Tạo nhóm mặc định
CREATE PROC dbo.themNhomNguoiDung_MacDinh (
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
				(@maNhom, 503, @1),
				(@maNhom, 504, @1),
				(@maNhom, 531, @1),
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
			(N'Quản lý', N'Quản lý chủ đề', @1, 0, NULL)
			
			SET @maNhom = @@IDENTITY
			INSERT INTO NhomNguoiDung_CD_Quyen (MaNhomNguoiDung, MaQuyen, MaDoiTuong) VALUES
				(@maNhom, 301, 0),
				(@maNhom, 302, 0),
				(@maNhom, 303, 0)

		INSERT INTO NhomNguoiDung_CD (Ten, MoTa, MaDoiTuong, MaNguoiTao, GiaTri) VALUES
			(N'Quản lý nội dung', N'Quản lý nội dung chủ đề', @1, 0, NULL)

			SET @maNhom = @@IDENTITY
			INSERT INTO NhomNguoiDung_CD_Quyen (MaNhomNguoiDung, MaQuyen, MaDoiTuong) VALUES
				(@maNhom, 302, 0),
				(@maNhom, 303, 0),
				(@maNhom, 405, 0),
				(@maNhom, 405, 0),
				(@maNhom, 404, 0),
				(@maNhom, 406, 0),
				(@maNhom, 407, 0),
				(@maNhom, 408, 0),
				(@maNhom, 520, 0),
				(@maNhom, 501, 0)

		INSERT INTO NhomNguoiDung_CD (Ten, MoTa, MaDoiTuong, MaNguoiTao, GiaTri) VALUES
			(N'Quản lý hỏi đáp', N'Quản lý hỏi đáp', @1, 0, NULL)

			SET @maNhom = @@IDENTITY
			INSERT INTO NhomNguoiDung_CD_Quyen (MaNhomNguoiDung, MaQuyen, MaDoiTuong) VALUES
				(@maNhom, 405, 0),
				(@maNhom, 405, 0),
				(@maNhom, 404, 0),
				(@maNhom, 406, 0),
				(@maNhom, 407, 0),
				(@maNhom, 408, 0)

		INSERT INTO NhomNguoiDung_CD (Ten, MoTa, MaDoiTuong, MaNguoiTao, GiaTri) VALUES
			(N'Quản lý khóa học', N'Quản lý khóa học', @1, 0, NULL)

			SET @maNhom = @@IDENTITY
			INSERT INTO NhomNguoiDung_CD_Quyen (MaNhomNguoiDung, MaQuyen, MaDoiTuong) VALUES
				(@maNhom, 520, 0),
				(@maNhom, 501, 0)
	END
	ELSE IF (@0 = 'HT')
	BEGIN
		INSERT INTO NhomNguoiDung_HT (Ten, MoTa, MaNguoiTao, GiaTri) VALUES
			(N'Quản lý hệ thống', N'Nhóm người này mặc định có tất cả quyền', 0, 'QuanLy')
			
			SET @maNhom = @@IDENTITY
			INSERT INTO NhomNguoiDung_HT_Quyen (MaNhomNguoiDung, MaQuyen, MaDoiTuong) VALUES
				(@maNhom, 102, 0), --Đặc biệt
				(@maNhom, 201, 0), --Đặc biệt
				(@maNhom, 304, 0), --Đặc biệt
				(@maNhom, 402, 0), --Đặc biệt
				(@maNhom, 530, 0) --Đặc biệt

		INSERT INTO NhomNguoiDung_HT (Ten, MoTa, MaNguoiTao, GiaTri) VALUES
			(N'Giảng viên', N'Nhóm giảng viên của hệ thống', 0, 'GiangVien')
			
		INSERT INTO NhomNguoiDung_HT (Ten, MoTa, MaNguoiTao, GiaTri) VALUES
			(N'Quản lý chức năng hệ thống', N'Quản lý chức năng hệ thống', 0, NULL)

			SET @maNhom = @@IDENTITY
			INSERT INTO NhomNguoiDung_HT_Quyen (MaNhomNguoiDung, MaQuyen, MaDoiTuong) VALUES
				(@maNhom, 101, 0)

		INSERT INTO NhomNguoiDung_HT (Ten, MoTa, MaNguoiTao, GiaTri) VALUES
			(N'Quản lý chức năng người dùng', N'Quản lý chức năng người dùng', 0, NULL)

			SET @maNhom = @@IDENTITY
			INSERT INTO NhomNguoiDung_HT_Quyen (MaNhomNguoiDung, MaQuyen, MaDoiTuong) VALUES
				(@maNhom, 202, 0)
			
		INSERT INTO NhomNguoiDung_HT (Ten, MoTa, MaNguoiTao, GiaTri) VALUES
			(N'Quản lý chức năng chủ đề', N'Quản lý chức năng chủ đề', 0, NULL)

			SET @maNhom = @@IDENTITY
			INSERT INTO NhomNguoiDung_HT_Quyen (MaNhomNguoiDung, MaQuyen, MaDoiTuong) VALUES
				(@maNhom, 301, 0),
				(@maNhom, 302, 0),
				(@maNhom, 303, 0)

		INSERT INTO NhomNguoiDung_HT (Ten, MoTa, MaNguoiTao, GiaTri) VALUES
			(N'Quản lý chức năng hỏi đáp', N'Quản lý chức năng hỏi đáp', 0, NULL)

			SET @maNhom = @@IDENTITY
			INSERT INTO NhomNguoiDung_HT_Quyen (MaNhomNguoiDung, MaQuyen, MaDoiTuong) VALUES
				(@maNhom, 405, 0),
				(@maNhom, 403, 0),
				(@maNhom, 404, 0),
				(@maNhom, 406, 0),
				(@maNhom, 407, 0),
				(@maNhom, 408, 0)

		INSERT INTO NhomNguoiDung_HT (Ten, MoTa, MaNguoiTao, GiaTri) VALUES
			(N'Quản lý chức năng khóa học', N'Quản lý chức năng khóa học', 0, NULL)

			SET @maNhom = @@IDENTITY
			INSERT INTO NhomNguoiDung_HT_Quyen (MaNhomNguoiDung, MaQuyen, MaDoiTuong) VALUES
				(@maNhom, 520, 0),
				(@maNhom, 501, 0)
	END
END