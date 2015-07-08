use rtcmfraf_Moodle;

GO
--Tạo cột điểm
CREATE TABLE dbo.CotDiem (
	Ma INT PRIMARY KEY IDENTITY(1, 1),
	Ten NVARCHAR(MAX) NOT NULL,
	HeSo INT,
	MaKhoaHoc INT NOT NULL,
	MoTa NVARCHAR(MAX),
	Ngay DATE,
	ThuTu INT NOT NULL,
	LaDiemCong BIT,
	--Đối tượng phát sinh
	LoaiDoiTuong NVARCHAR(MAX),
	MaDoiTuong INT
)

GO
--Thêm cột điểm
ALTER PROC dbo.themCotDiem (
	@0 NVARCHAR(MAX), --Ten
	@1 NVARCHAR(MAX), --MoTa
	@2 INT, --HeSo
	@3 DATE, --Ngay
	@4 INT, --MaKhoaHoc
	@5 BIT, --LaDiemCong
	@6 NVARCHAR(MAX), --LoaiDoiTuong
	@7 INT --MaDoiTuong
)
AS
BEGIN
	--Lấy thứ tự
	DECLARE @thuTu INT

	SELECT @thuTu = MAX(ThuTu)
		FROM dbo.CotDiem
		WHERE MaKhoaHoc = @4

	SET @thuTu = 
		CASE
			WHEN @thuTu IS NULL
				THEN 1
		ELSE
			@thuTu + 1
		END

	INSERT INTO dbo.CotDiem (Ten, MoTa, HeSo, Ngay, MaKhoaHoc, LaDiemCong, LoaiDoiTuong, MaDoiTuong, ThuTu)
		VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @thuTu)

	SELECT TOP 1 *
		FROM dbo.CotDiem
		WHERE Ma = @@IDENTITY
END

GO
--Cập nhật theo mã
CREATE PROC dbo.capNhatCotDiemTheoMa (
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
			UPDATE dbo.CotDiem
				SET ' + @query + '
				WHERE Ma = ' + @0 + '
		')
	END	
	
	SELECT *
		FROM dbo.CotDiem
		WHERE Ma = @0
END

GO
--Lấy theo mã khóa học
ALTER PROC dbo.layCotDiemTheoMaKhoaHoc(
	@0 INT --MaKhoaHoc
)
AS
BEGIN
	SELECT *
		FROM dbo.CotDiem
		WHERE MaKhoaHoc = @0
		ORDER BY ThuTu
END

GO
--Xóa theo mã
CREATE PROC dbo.xoaCotDiemTheoMa(
	@0 INT --Ma
)
AS
BEGIN
	DELETE FROM dbo.CotDiem
		WHERE Ma = @0
END

GO
--[Trigger] Xóa cột điểm
CREATE TRIGGER dbo.xoaCotDiem_TRIGGER
	ON dbo.CotDiem
	AFTER DELETE
AS
BEGIN
	UPDATE CD
		SET
			CD.ThuTu = CD.ThuTu - 1
		FROM
			dbo.CotDiem CD
				INNER JOIN deleted d
				ON 
					CD.MaKhoaHoc = d.MaKhoaHoc AND
					CD.ThuTu > d.ThuTu
END

GO
--Thay đổi thứ tự
CREATE PROC dbo.capNhatCotDiem_ThuTu (
	@0 INT, --ThuTu cũ
	@1 INT, --ThuTu mới
	@2 INT --MaKhoaHoc
)
AS
BEGIN
	IF (@0 = @1)
	BEGIN
		RETURN
	END

	--Biến để xác định giá trị thứ tự sẽ thay đổi như thế nào
	DECLARE @thayDoi INT, @gioiHanTren INT, @gioiHanDuoi INT
	
	IF (@0 < @1)
	BEGIN
		SET @thayDoi = -1
		SET @gioiHanTren = @1
		SET @gioiHanDuoi = @0
	END
	ELSE
	BEGIN
		SET @thayDoi = 1
		SET @gioiHanTren = @0
		SET @gioiHanDuoi = @1
	END

	UPDATE CD
		SET
			CD.ThuTu = CASE
				WHEN CD.ThuTu <> @0 THEN
					CD.ThuTu + @thayDoi
				ELSE
					@1
				END
		FROM dbo.CotDiem CD
		WHERE 
			CD.MaKhoaHoc = @2 AND
			CD.ThuTu BETWEEN @gioiHanDuoi AND @gioiHanTren
END

GO
--Lấy theo mã
ALTER PROC dbo.layCotDiemTheoMa(
	@0 INT --Ma
)
AS
BEGIN
	SELECT *
		FROM dbo.CotDiem
		WHERE Ma = @0
END
GO
--Lấy theo đối tượng
CREATE PROC dbo.layCotDiemTheoLoaiDoiTuongVaMaDoiTuong (
	@0 NVARCHAR(MAX), --LoaiDoiTuong
	@1 INT --MaDoiTuong
)
AS
BEGIN
	SELECT TOP 1 *
		FROM dbo.CotDiem
		WHERE 
			LoaiDoiTuong = @0 AND 
			MaDoiTuong = @1
END