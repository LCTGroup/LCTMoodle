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
CREATE PROC dbo.themTraLoi_Diem
(
	@0 INT, --Mã hỏi đáp (Câu hỏi hoặc trả lời)
	@1 INT, --Mã người tạo
	@2 BIT --Trạng thái cho điểm
)
AS
BEGIN
	INSERT INTO dbo.TraLoi_Diem(MaTraLoi, MaNguoiTao, Diem)
	VALUES (@0, @1, @2)
END

GO
--Bỏ cho điểm
CREATE PROC dbo.xoaTraLoi_DiemTheoMaTraLoiVaMaNguoiTao
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
--Lấy điểm cho trả lời
CREATE PROC dbo.layTraLoi_DiemTheoMaTraLoi_Diem
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