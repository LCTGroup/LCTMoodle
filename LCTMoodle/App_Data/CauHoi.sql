use rtcmfraf_Moodle;

GO
--Tạo Câu Hỏi
CREATE TABLE dbo.CauHoi
(
	Ma INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	TieuDe NVARCHAR(MAX) NOT NULL,
	NoiDung NVARCHAR(MAX) NOT NULL,
	ThoiDiemTao DATETIME DEFAULT GETDATE(),
	ThoiDiemCapNhat DATETIME DEFAULT GETDATE(),
	MaNguoiTao INT NOT NULL,
	MaChuDe INT,
)

GO
--Thêm Câu Hỏi
ALTER PROC dbo.themCauHoi
(
	@0 NVARCHAR(MAX), --Tiêu đề
	@1 NVARCHAR(MAX), --Nội dung
	@2 INT, --Mã người tạo
	@3 INT --Mã chủ đề
)
AS
BEGIN
	INSERT INTO dbo.CauHoi(TieuDe, NoiDung, MaNguoiTao, MaChuDe) VALUES (@0, @1, @2, @3)

	SELECT @@IDENTITY Ma
END

GO
--Xóa Câu Hỏi
CREATE PROC dbo.xoaCauHoiTheoMa
(
	@0 INT --Mã Câu Hỏi
)
AS
BEGIN
	DELETE FROM dbo.CauHoi WHERE Ma = @0
END

GO
--Cập nhật Câu Hỏi theo mã
ALTER PROC dbo.capNhatCauHoiTheoMa 
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
		UPDATE dbo.CauHoi
			SET ' + @query + ', ThoiDiemCapNhat = GETDATE()
			WHERE Ma = ' + @0 + '
		')
	END	
	
	SELECT TOP 1
		Ma,
		TieuDe,
		NoiDung,
		ThoiDiemTao,
		MaNguoiTao,
		MaChuDe
		FROM dbo.CauHoi
		WHERE Ma = @0
END

GO
--Lấy Câu Hỏi
CREATE PROC dbo.layCauHoiTheoMa
(
	@0 INT --Mã Câu Hỏi
)
AS
BEGIN
	SELECT *
	FROM dbo.CauHoi
	WHERE Ma=@0
END

GO
--Lấy toàn bộ Câu Hỏi
CREATE PROC dbo.layToanBoCauHoi
AS
BEGIN
	SELECT *
	FROM dbo.CauHoi
END

GO
--Lấy Câu Hỏi theo từ khóa
ALTER PROC dbo.layCauHoi_TimKiem 
(
	@0 NVARCHAR(MAX) --Từ khóa
)
AS
BEGIN
	SET @0 = '%' + REPLACE(@0, ' ', '%') + '%'
	SELECT *
	FROM dbo.CauHoi
	WHERE TieuDe LIKE @0 OR NoiDung LIKE @0
END 

GO
--Lấy Câu Hỏi theo Chủ Đề
CREATE PROC dbo.layCauHoiTheoChuDe
(
	@0 INT --Mã Chủ Đề
)
AS
BEGIN
	SELECT *
	FROM dbo.CauHoi
	WHERE MaChuDe = @0
END

select * from dbo.CauHoi