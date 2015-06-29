use rtcmfraf_Moodle;

GO
--Tạo Hỏi Đáp - Điểm
CREATE TABLE dbo.TraLoi_Diem
(
	MaTraLoi INT NOT NULL,
	MaNguoiTao INT NOT NULL,
	Diem BIT NOT NULL,
)

GO
--Cho điểm Hỏi Đáp
ALTER PROC dbo.themTraLoi_Diem
(
	@0 INT, --Mã hỏi đáp (Câu hỏi hoặc trả lời)
	@1 INT, --Mã người tạo
	@2 BIT --Trạng thái cho điểm
)
AS
BEGIN
	EXEC xoaTraLoi_DiemTheoMaTraLoiVaMaNguoiTao @0, @1

	INSERT INTO dbo.TraLoi_Diem(MaTraLoi, MaNguoiTao, Diem)
	VALUES (@0, @1, @2)
END

GO
--Thêm điểm cho Trả lời khi thêm TraLoi_Diem thành công
ALTER TRIGGER dbo.themTraLoi_Diem_TRIGGER
ON dbo.TraLoi_Diem
AFTER INSERT
AS
BEGIN
	DECLARE @maTraLoi INT
	DECLARE @diem BIT

	SELECT @maTraLoi = MaTraLoi, @diem = Diem FROM INSERTED	

	UPDATE dbo.TraLoi
	SET Diem += CASE
		WHEN @diem = 1 THEN
			1
		ELSE
			(-1)
		END
	WHERE Ma = @maTraLoi
END

GO
--Bỏ cho điểm
ALTER PROC dbo.xoaTraLoi_DiemTheoMaTraLoiVaMaNguoiTao
(
	@0 INT, --Mã trả lời
	@1 INT --Mã người tạo
)
AS
BEGIN
	DELETE FROM dbo.TraLoi_Diem
	WHERE MaTraLoi = @0 AND MaNguoiTao = @1
END

GO
--Giảm điểm cho Trả Lời khi xóa TraLoi_Diem thành công
ALTER TRIGGER dbo.xoaTraLoi_Diem_TRIGGER
ON dbo.TraLoi_Diem
AFTER DELETE
AS
BEGIN
	DECLARE @maTraLoi INT

	SELECT @maTraLoi = MaTraLoi FROM DELETED
	DECLARE @diem BIT

	UPDATE dbo.TraLoi
	SET Diem -= CASE
		WHEN @diem = 1 THEN
			1
		ELSE
			(-1)
		END
	WHERE Ma = @maTraLoi
END

GO
--Lấy giá trị vote của người dùng
CREATE PROC dbo.layTraLoi_DiemTheomMaTraLoiVaMaNguoiTao_Diem
(
	@0 INT, --Mã trả lời
	@1 INT --Mã người dùng
)
AS
BEGIN
	SELECT Diem
	FROM dbo.TraLoi_Diem
	WHERE MaTraLoi = @0 AND MaNguoiTao = @1
END

GO
--Lấy điểm cho trả lời
ALTER PROC dbo.layTraLoi_DiemTheoMaTraLoi_Diem
(
	@0 INT --Mã trả lời
)
AS
BEGIN
	DECLARE @diem INT = 0

	SELECT @diem += CASE
		WHEN Diem = 1 THEN
			1
		ELSE
			-1
		END
	FROM dbo.TraLoi_Diem	
	WHERE MaTraLoi = @0

	SELECT @diem
END