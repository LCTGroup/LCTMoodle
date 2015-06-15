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
	MaNguoiTao INT NOT NULL
)

GO
--Thêm Câu Hỏi
CREATE PROC dbo.themCauHoi
(
	@0 NVARCHAR(MAX), --Tiêu đề
	@1 NVARCHAR(MAX), --Nội dung
	@2 INT --Mã người tạo
)
AS
BEGIN
	INSERT INTO dbo.CauHoi(TieuDe, NoiDung, MaNguoiTao) VALUES (@0, @1, @2)

	SELECT @@IDENTITY Ma
END

GO
--Xóa Câu Hỏi
CREATE PROC dbo.xoaCauHoiTheoMa(
	@0 INT --Mã câu hỏi
)
AS
BEGIN
	DELETE FROM dbo.CauHoi WHERE Ma = @0
END

GO
--Cập nhật câu hỏi theo mã
ALTER PROC dbo.capNhatCauHoiTheoMa (
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
		MaNguoiTao
		FROM dbo.CauHoi
		WHERE Ma = @0
END

GO
--Lấy Câu hỏi
CREATE PROC dbo.layCauHoiTheoMa
(
	@0 INT --Mã câu hỏi
)
AS
BEGIN
	SELECT *
	FROM dbo.CauHoi
	WHERE Ma=@0
END

GO
--Lấy toàn bộ Câu hỏi
CREATE PROC dbo.layToanBoCauHoi
AS
BEGIN
	SELECT *
	FROM dbo.CauHoi
END