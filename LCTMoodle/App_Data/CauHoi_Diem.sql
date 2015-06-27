use rtcmfraf_Moodle;

GO
--Tạo Hỏi Đáp - Điểm
CREATE TABLE dbo.CauHoi_Diem
(
	MaCauHoi INT NOT NULL,
	MaNguoiTao INT NOT NULL,
	Diem BIT NOT NULL,	
)

GO
--Cho điểm Hỏi Đáp
ALTER PROC dbo.themCauHoi_Diem
(
	@0 INT, --Mã hỏi đáp (Câu hỏi hoặc trả lời)
	@1 INT, --Mã người tạo
	@2 BIT --Trạng thái cho điểm
)
AS
BEGIN
	INSERT INTO dbo.CauHoi_Diem(MaCauHoi, MaNguoiTao, Diem)
	VALUES (@0, @1, @2)
END

GO
--Bỏ cho điểm
CREATE PROC dbo.xoaCauHoi_DiemTheoMaCauHoiVaMaNguoiTao
(
	@0 INT, --Mã câu hỏi
	@1 INT --Mã người tạo
)
AS
BEGIN
	DELETE FROM dbo.CauHoi_Diem
	WHERE MaCauHoi = @0 AND MaNguoiTao = @1
END

GO
--Lấy thông tin người vote
CREATE PROC dbo.layCauHoi_DiemTheoMaNguoiTao
(
	@0 INT --Mã người dùng
)
AS
BEGIN
	SELECT diem
	FROM dbo.CauHoi_Diem
	WHERE MaNguoiTao = @0
END

GO
--Lấy điểm cho câu hỏi
ALTER PROC dbo.layCauHoi_DiemTheoMaCauHoi_Diem
(
	@0 INT --Mã câu hỏi
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
	FROM dbo.CauHoi_Diem	
	WHERE MaCauHoi = @0

	SELECT @diem
END