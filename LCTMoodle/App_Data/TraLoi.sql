use rtcmfraf_Moodle;

GO
--Tạo Trả Lời
CREATE TABLE dbo.TraLoi
(
	Ma INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	NoiDung NVARCHAR(MAX) NOT NULL,
	ThoiDiemTao DATETIME DEFAULT GETDATE(),
	ThoiDiemCapNhat DATETIME DEFAULT GETDATE(),
	Duyet BIT DEFAULT NULL,
	MaNguoiTao INT NOT NULL,
	MaCauHoi INT NOT NULL,
)	

GO
--Thêm Trả Lời
CREATE PROC dbo.themTraLoi
(
	@0 NVARCHAR(MAX), --Nội dung
	@1 INT, --Mã người tạo
	@2 INT --Mã câu hỏi
)
AS
BEGIN
	INSERT INTO dbo.TraLoi(NoiDung, MaNguoiTao, MaCauHoi) VALUES (@0, @1, @2)

	SELECT
		Ma,
		NoiDung,
		ThoiDiemTao,
		Duyet,
		MaNguoiTao,
		MaCauHoi
	FROM dbo.TraLoi
	WHERE Ma = @@Identity
END

GO
--Xóa Trả Lời
CREATE PROC dbo.xoaTraLoiTheoMa
(
	@0 INT --Mã trả lời
)
AS
BEGIN
	DELETE FROM dbo.TraLoi WHERE Ma = @0
END

GO
--Cập nhật trả lời theo mã
ALTER PROC dbo.capNhatTraLoiTheoMa 
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
		UPDATE dbo.TraLoi
			SET ' + @query + ', ThoiDiemCapNhat = GETDATE()
			WHERE Ma = ' + @0 + '
		')
	END	
	
	SELECT TOP 1
		Ma,
		NoiDung,
		ThoiDiemTao,
		Duyet,
		MaNguoiTao,
		MaCauHoi
		FROM dbo.TraLoi
		WHERE Ma = @0
END

GO
--Cập nhật duyệt Trả Lời
ALTER PROC dbo.capNhatDuyetTraLoiTheoMa
(
	@0 INT, --Mã Trả Lời
	@1 BIT --Trạng thái duyệt
)
AS
BEGIN
	UPDATE dbo.TraLoi		
	SET Duyet = @1
	WHERE Ma=@0
END

GO
--Lấy toàn bộ trả lời của câu hỏi
CREATE PROC dbo.layDanhSachTraLoiTheoMaCauHoi
(
	@0 INT --Mã câu hỏi	
)
AS
BEGIN
	SELECT * FROM dbo.TraLoi WHERE MaCauHoi=@0
END

GO
--Lấy trả lời theo mã
CREATE PROC dbo.layTraLoiTheoMa
(
	@0 INT --Mã trả lời
)
AS
BEGIN
	SELECT * FROM dbo.TraLoi WHERE Ma=@0
END