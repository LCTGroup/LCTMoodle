use rtcmfraf_Moodle;

GO
--Tạo Trả Lời
CREATE TABLE dbo.TraLoi
(
	Ma INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	NoiDung NVARCHAR(MAX) NOT NULL,
	ThoiDiemTao DATETIME DEFAULT GETDATE(),
	ThoiDiemCapNhat DATETIME DEFAULT GETDATE(),
	Duyet BIT DEFAULT 0,
	MaNguoiTao INT NOT NULL,
	MaCauHoi INT NOT NULL,
	Diem INT DEFAULT 0
)	

GO
--Thêm Trả Lời
ALTER PROC dbo.themTraLoi
(
	@0 NVARCHAR(MAX), --Nội dung
	@1 INT, --Mã người tạo
	@2 INT --Mã câu hỏi
)
AS
BEGIN
	INSERT INTO dbo.TraLoi(NoiDung, MaNguoiTao, MaCauHoi) VALUES (@0, @1, @2)

	SELECT *		
	FROM dbo.TraLoi
	WHERE Ma = @@Identity
END

GO
--Tăng số lượng trả lời trong Câu hỏi khi thêm trả lời
ALTER TRIGGER dbo.themTraLoi_TRIGGER
ON dbo.TraLoi
AFTER INSERT
AS
BEGIN
	DECLARE @maCauHoi INT
	DECLARE @maNguoiTao INT
	
	SELECT @maCauHoi = MaCauHoi, @maNguoiTao = MaNguoiTao FROM inserted

	UPDATE dbo.CauHoi
	SET SoLuongTraLoi += 1
	WHERE Ma = @maCauHoi

	UPDATE dbo.NguoiDung
	SET DiemHoiDap += 1
	WHERE Ma = @maNguoiTao
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
--Giảm số lượng trả lời trong Câu hỏi khi thêm trả lời
ALTER TRIGGER dbo.xoaTraLoi_TRIGGER
ON dbo.TraLoi
AFTER DELETE
AS
BEGIN
	DECLARE @maCauHoi INT
	DECLARE @maNguoiTao INT

	SELECT @maCauHoi = MaCauHoi, @maNguoiTao = MaNguoiTao From DELETED

	UPDATE dbo.CauHoi
	SET SoLuongTraLoi -= 1
	WHERE Ma = @maCauHoi

	UPDATE dbo.NguoiDung
	SET DiemHoiDap -= 1
	WHERE Ma = @maNguoiTao
END

GO
--Xóa Trả Lời theo mã Câu Hỏi
CREATE PROC dbo.xoaTraLoiTheoMaCauHoi
(
	@0 INT --Mã Câu Hỏi
)
AS
BEGIN
	DELETE FROM dbo.TraLoi WHERE MaCauHoi = @0
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
		ThoiDiemCapNhat,
		Duyet,
		MaNguoiTao,
		MaCauHoi,
		Diem
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
ALTER PROC dbo.layDanhSachTraLoiTheoMaCauHoi
(
	@0 INT --Mã câu hỏi	
)
AS
BEGIN
	SELECT *
	FROM dbo.TraLoi 
	WHERE MaCauHoi=@0
	ORDER BY Duyet DESC
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

GO
--Lấy số lượng trả lời theo Câu hỏi
CREATE PROC dbo.layTraLoiTheoMaCauHoi_SoLuong
(
	@0 INT --Mã câu hỏi
)
AS
BEGIN
	SELECT COUNT(*)
	FROM dbo.TraLoi
	WHERE MaCauHoi = @0
END