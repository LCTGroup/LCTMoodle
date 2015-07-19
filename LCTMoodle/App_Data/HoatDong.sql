GO
--Bảng hoạt động
CREATE TABLE dbo.HoatDong (
	Ma INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	MaNguoiTacDong INT NOT NULL,
	LoaiDoiTuongTacDong NVARCHAR(MAX) NOT NULL,
	MaDoiTuongTacDong INT NOT NULL,
	LoaiDoiTuongBiTacDong NVARCHAR(MAX) NOT NULL,
	MaDoiTuongBiTacDong INT NOT NULL,
	LoaiDoiTuongPhamVi NVARCHAR(MAX),
	MaDoiTuongPhamVi INT,
	MaHanhDong INT NOT NULL,
	DuongDan NVARCHAR(MAX),
	ThoiDiem DATETIME DEFAULT GETDATE()
)

GO
--Thêm hoạt động
ALTER PROC dbo.themHoatDong (
	@0 INT, --Mã người tác động
	@1 NVARCHAR(MAX), --Loại đối tượng tác động
	@2 INT, --Mã đối tượng tác động
	@3 NVARCHAR(MAX), --Loại đối tượng bị tác động
	@4 INT, --Mã đối tượng bị tác động
	@5 NVARCHAR(MAX), --Loại đối tượng phạm vi
	@6 INT, --Mã đối tượng phạm vi
	@7 INT, --Mã hành động
	@8 NVARCHAR(MAX) --Đường dẫn
)
AS
BEGIN
	INSERT INTO dbo.HoatDong (MaNguoiTacDong, LoaiDoiTuongTacDong, MaDoiTuongTacDong, LoaiDoiTuongBiTacDong, MaDoiTuongBiTacDong, LoaiDoiTuongPhamVi, MaDoiTuongPhamVi, MaHanhDong, DuongDan)
		VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8)
END

GO
--Lấy hoạt động theo mã
CREATE PROC dbo.layHoatDongTheoMa (
	@0 INT --Mã 
)
AS
BEGIN
	SELECT *
		FROM dbo.HoatDong
		WHERE Ma = @0
END

GO
--Lấy của đối tượng
ALTER PROC dbo.layHoatDong_CuaDoiTuong (
	@0 VARCHAR(MAX), --LoaiDoiTuong
	@1 INT, --MaDoiTuong
	@2 INT, --Trang
	@3 INT --Số lượng mỗi trang
)
AS
BEGIN
	DECLARE @trang VARCHAR(MAX)
	IF (@2 IS NULL OR @3 IS NULL)
	BEGIN
		SET @trang = ''
	END
	ELSE
	BEGIN
		SET @trang = 'WHERE ' + CAST((@2 - 1) * @3 AS VARCHAR(MAX)) + ' < Dong AND Dong <= ' + CAST(@2 * @3 AS VARCHAR(MAX))
	END

	EXEC('
		SELECT *
			FROM (
				SELECT
					*,
					ROW_NUMBER() OVER (ORDER BY ThoiDiem DESC) AS Dong,
					COUNT(1) OVER () AS TongSoDong
					FROM dbo.HoatDong
					' + @0 + '
					WHERE
						(loaiDoiTuongTacDong = ''' + @0 + ''' AND
							maDoiTuongTacDong = ' + @1 + ') OR
						(loaiDoiTuongBiTacDong = ''' + @0 + ''' AND
							maDoiTuongBiTacDong = ' + @1 + ') OR
						(loaiDoiTuongPhamVi = ''' + @0 + ''' AND
							maDoiTuongPhamVi = ' + @1 + ')
			) AS KH
			' + @trang + '
			ORDER BY ThoiDiem DESC
	')
END

GO
--Lấy của danh sách đối tượng
ALTER PROC dbo.layHoatDong_CuaDanhSachDoiTuong (
	@0 VARCHAR(MAX), --LoaiDoiTuong
	@1 VARCHAR(MAX), --Danh sách mã đối tương (1,2,3)
	@2 INT, --Trang
	@3 INT --Số lượng mỗi trang
)
AS
BEGIN
	DECLARE @trang VARCHAR(MAX)
	IF (@2 IS NULL OR @3 IS NULL)
	BEGIN
		SET @trang = ''
	END
	ELSE
	BEGIN
		SET @trang = 'WHERE ' + CAST((@2 - 1) * @3 AS VARCHAR(MAX)) + ' < Dong AND Dong <= ' + CAST(@2 * @3 AS VARCHAR(MAX))
	END

	EXEC('
		SELECT *
			FROM (
				SELECT
					*,
					ROW_NUMBER() OVER (ORDER BY ThoiDiem DESC) AS Dong,
					COUNT(1) OVER () AS TongSoDong
					FROM dbo.HoatDong
					' + @0 + '
					WHERE
						(loaiDoiTuongTacDong = ''' + @0 + ''' AND
							maDoiTuongTacDong IN (' + @1 + ')) OR
						(loaiDoiTuongBiTacDong = ''' + @0 + ''' AND
							maDoiTuongBiTacDong IN (' + @1 + ')) OR
						(loaiDoiTuongPhamVi = ''' + @0 + ''' AND
							maDoiTuongPhamVi IN (' + @1 + '))
			) AS KH
			' + @trang + '
			ORDER BY ThoiDiem DESC
	')
END

exec layHoatDong_CuaDanhSachDoiTuong 'TL',1,1,5