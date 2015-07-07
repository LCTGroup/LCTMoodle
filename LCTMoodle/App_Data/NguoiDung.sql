use rtcmfraf_Moodle;

GO
--Người dùng
CREATE TABLE dbo.NguoiDung 
(
	Ma INT IDENTITY(1, 1) PRIMARY KEY NOT NULL,
	TenTaiKhoan NVARCHAR(MAX) NOT NULL,
	MatKhau NVARCHAR(MAX) NOT NULL,
	Email NVARCHAR(MAX) NOT NULL,
	GioiTinh INT DEFAULT 0, 
	Ho NVARCHAR(MAX) NOT NULL,
	TenLot NVARCHAR(MAX),
	Ten NVARCHAR(MAX) NOT NULL,
	NgaySinh DATETIME,
	DiaChi NVARCHAR(MAX),
	SoDienThoai NVARCHAR(MAX),
	MaHinhDaiDien INT,
	DaKichHoat BIT DEFAULT 0,
	MaKichHoat NVARCHAR(MAX),
	MatKhauCap2 NVARCHAR(MAX),
	CoQuyenNhomHT BIT,
	CoQuyenNhomCD BIT,
	CoQuyenNhomKH BIT,
	DiemHoiDap INT DEFAULT 0,
	ThoiDiemPhucHoiMatKhau DATETIME DEFAULT GETDATE(),
	DaDuyet BIT DEFAULT 1
)

GO
--Thêm người dùng
ALTER PROC dbo.themNguoiDung 
(
	@0 NVARCHAR(MAX), --Tên tài khoản
	@1 NVARCHAR(MAX), --Mật khẩu
	@2 NVARCHAR(MAX), --Email
	@3 INT, --Giới tính
	@4 NVARCHAR(MAX), --Họ
	@5 NVARCHAR(MAX), --Tên lót
	@6 NVARCHAR(MAX), --Tên
	@7 DATETIME, --Ngày Sinh
	@8 NVARCHAR(MAX), --Địa chỉ
	@9 NVARCHAR(MAX), --Số điện thoại
	@10 INT, --Hình đại diện
	@11 NVARCHAR(MAX) --Mã kích hoạt
)
AS
BEGIN
	INSERT INTO dbo.NguoiDung(TenTaiKhoan, MatKhau, Email, GioiTinh, Ho, TenLot, Ten, NgaySinh, DiaChi, SoDienThoai, MaHinhDaiDien, MaKichHoat) 
	VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11);

	SELECT @@IDENTITY Ma
END

GO
--Cập nhật người dùng - chặn
CREATE PROC dbo.capNhatNguoiDungTheoMa_Chan
(
	@0 INT, --Mã người dùng
	@1 BIT --Trạng Thái
)
AS
BEGIN
	UPDATE dbo.NguoiDung
	SET DaDuyet = @1
	WHERE Ma = @0
END

GO
--Cập nhật Người Dùng theo mã
CREATE PROC dbo.capNhatNguoiDungTheoMa 
(
	@0 INT, --Mã
	@1 dbo.BangCapNhat READONLY
)
AS
BEGIN
	--Tạo chuỗi gán
	DECLARE @query NVARCHAR(MAX) = dbo.taoChuoiCapNhat(@1)
	IF (@query <> '')
	BEGIN
		EXEC('
		UPDATE dbo.NguoiDung
			SET ' + @query + '
			WHERE Ma = ' + @0 + '
		')
	END	
END

GO
--Cập nhật kích hoạt Người dùng theo tenTaiKhoan
ALTER PROC dbo.capNhatNguoiDungTheoTenTaiKhoan_KichHoat
(
	@0 NVARCHAR(MAX), --Tên tài khoản
	@1 NVARCHAR(MAX) --Giá trị kích hoạt
)
AS
BEGIN
	UPDATE dbo.NguoiDung
	SET MaKichHoat = @1
	WHERE TenTaiKhoan = @0
END

GO
--Cập nhật thời điểm phục hồi mật khẩu
CREATE PROC dbo.capNhatNguoiDungTheoMaNguoiDung_ThoiDiemPhucHoiMatKhau
(
	@0 INT --Mã người phục hồi
)
AS
BEGIN
	UPDATE dbo.NguoiDung
	SET ThoiDiemPhucHoiMatKhau = GETDATE()
	WHERE Ma = @0
END

GO
--Lấy người dùng theo mã
CREATE PROC dbo.layNguoiDungTheoMa 
(
	@0 INT --Mã người dùng
)
AS
BEGIN
	SELECT *
	FROM dbo.NguoiDung
	WHERE Ma = @0
END

GO
--Lấy danh sách người dùng
CREATE PROC dbo.layNguoiDung
AS
BEGIN
	SELECT * FROM dbo.NguoiDung
END

GO
--Lấy danh sách người dùng bị chặn
CREATE PROC dbo.layNguoiDungBiChan
AS
BEGIN
	SELECT * 
	FROM dbo.NguoiDung
	WHERE DaDuyet = 0
END

GO
--Lấy người dùng theo Tên Tài Khoản
CREATE PROC dbo.layNguoiDungTheoTenTaiKhoan 
(
	@0 NVARCHAR(MAX) --Tên tài khoản
)
AS
BEGIN
	SELECT *
	FROM dbo.NguoiDung
	WHERE TenTaiKhoan = @0
END

GO
--Lấy người dùng theo Email
CREATE PROC dbo.layNguoiDungTheoEmail
(
	@0 NVARCHAR(MAX) --Email
)
AS
BEGIN
	SELECT *
	FROM dbo.NguoiDung
	WHERE Email = @0
END

GO
--Lấy người dùng theo từ khóa
ALTER PROC dbo.layNguoiDung_TimKiem 
(
	@0 NVARCHAR(MAX) --Từ khóa
)
AS
BEGIN
	SET @0 = '%' + REPLACE(@0, ' ', '%') + '%'
	SELECT TOP 20 *
		FROM dbo.NguoiDung
		WHERE 
			Ho + ' ' + TenLot + ' ' + Ten LIKE @0
END

GO
--Lấy người dùng theo mã khóa học và từ khóa (Chiêu)
ALTER PROC dbo.layNguoiDungTheoMaKhoaHoc_TimKiem (
	@0 INT, --MaKhoaHoc
	@1 NVARCHAR(MAX) --Từ khóa
)
AS
BEGIN
	SET @1 = '%' + REPLACE(@1, ' ', '%') + '%'
	SELECT TOP 20 *
		FROM 
			dbo.KhoaHoc_NguoiDung KH_ND INNER JOIN
				dbo.NguoiDung ND ON
					KH_ND.MaKhoaHoc = @0 AND
					KH_ND.MaNguoiDung = ND.Ma AND
					ND.Ho + ' ' + ND.TenLot + ' ' + ND.Ten LIKE @1
END

GO
--Lấy người dùng theo mã nhóm người dùng
ALTER PROC dbo.layNguoiDungTheoMaNhomNguoiDung (
	@0 NVARCHAR(MAX), --PhamVi
	@1 INT --MaNhomNguoiDung
)
AS
BEGIN
	EXEC('
		SELECT *
			FROM
				dbo.NguoiDung ND 
					INNER JOIN dbo.NhomNguoiDung_' + @0 + '_NguoiDung NND_ND
						ON 
							NND_ND.MaNhomNguoiDung = ' + @1 + ' AND
							ND.Ma = NND_ND.MaNguoiDung
	')
END

GO
--Lấy người dùng theo giá trị nhóm người dùng
CREATE PROC dbo.layNguoiDungTheoMaDoiTuongNhomNguoiDungVaGiaTriNhomNguoiDung (
	@0 VARCHAR(MAX), --PhamVi
	@1 INT, --MaDoiTuong
	@2 VARCHAR(MAX) --GiaTri
)
AS
BEGIN
	EXEC('
		SELECT ND.*
			FROM
				dbo.NhomNguoiDung_' + @0 + ' NND
					INNER JOIN dbo.NhomNguoiDung_' + @0 + '_NguoiDung NND_ND ON
						NND.MaDoiTuong = ' + @1 + ' AND
						NND.GiaTri = ''' + @2 + ''' AND
						NND.Ma = NND_ND.MaNhomNguoiDung
					INNER JOIN dbo.NguoiDung ND ON
						NND_ND.MaNguoiDung = ND.Ma
	')
END