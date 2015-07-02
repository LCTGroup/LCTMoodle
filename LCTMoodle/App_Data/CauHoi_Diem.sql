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
--Cho điểm
ALTER PROC dbo.themCauHoi_Diem
(
	@0 INT, --Mã câu hỏi
	@1 INT, --Mã người tạo
	@2 BIT --Trạng thái cho điểm
)
AS
BEGIN
	EXEC xoaCauHoi_DiemTheoMaCauHoiVaMaNguoiTao @0, @1

	INSERT INTO dbo.CauHoi_Diem(MaCauHoi, MaNguoiTao, Diem)
	VALUES (@0, @1, @2)	
END

GO
--Thêm điểm cho Câu Hỏi khi thêm CauHoi_Diem thành công
ALTER TRIGGER dbo.themCauHoi_Diem_TRIGGER
ON dbo.CauHoi_Diem
AFTER INSERT
AS
BEGIN
	DECLARE @maCauHoi INT
	DECLARE @diem BIT
	DECLARE @maNguoiTaoCauHoiDuocVote INT

	SELECT @maCauHoi = MaCauHoi, @diem = Diem FROM INSERTED	

	UPDATE dbo.CauHoi
	SET Diem += CASE
		WHEN @diem = 1 THEN
			1
		ELSE
			(-1)
		END
	WHERE Ma = @maCauHoi

	SELECT  @maNguoiTaoCauHoiDuocVote = MaNguoiTao
	FROM dbo.CauHoi
	WHERE Ma = @maCauHoi

	UPDATE dbo.NguoiDung
	SET DiemHoiDap += CASE
		WHEN @diem = 1 THEN
			3
		ELSE
			(-3)
		END
	WHERE Ma = @maNguoiTaoCauHoiDuocVote
END

GO
--Bỏ cho điểm
ALTER PROC dbo.xoaCauHoi_DiemTheoMaCauHoiVaMaNguoiTao
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
--Giảm điểm cho Câu Hỏi khi xóa CauHoi_Diem thành công
ALTER TRIGGER dbo.xoaCauHoi_Diem_TRIGGER
ON dbo.CauHoi_Diem
AFTER DELETE
AS
BEGIN
	DECLARE @maCauHoi INT
	DECLARE @diem BIT
	DECLARE @maNguoiTaoCauHoiDuocVote INT

	SELECT @maCauHoi = MaCauHoi, @diem = Diem FROM DELETED

	UPDATE dbo.CauHoi
	SET Diem -= CASE
		WHEN @diem = 1 THEN
			1
		ELSE
			(-1)
		END
	WHERE Ma = @maCauHoi
	
	SELECT  @maNguoiTaoCauHoiDuocVote = MaNguoiTao
	FROM dbo.CauHoi
	WHERE Ma = @maCauHoi

	UPDATE dbo.NguoiDung
	SET DiemHoiDap -= CASE
		WHEN @diem = 1 THEN
			3
		ELSE
			(-3)
		END
	WHERE Ma = @maNguoiTaoCauHoiDuocVote
END

GO
--Lấy giá trị vote của người dùng
ALTER PROC dbo.layCauHoi_DiemTheomMaCauHoiVaMaNguoiTao_Diem
(
	@0 INT, --Mã câu hỏi
	@1 INT --Mã người dùng
)
AS
BEGIN
	SELECT Diem
	FROM dbo.CauHoi_Diem
	WHERE MaCauHoi = @0 AND MaNguoiTao = @1
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
