﻿use rtcmfraf_Moodle;

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
	MaChuDe INT DEFAULT 0,
	Diem INT DEFAULT 0
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
--Cập nhật điểm Câu hỏi theo Mã
ALTER PROC dbo.capNhatCauHoi_Diem
(
	@0 INT, --Mã câu hỏi
	@1 INT --Điểm số
)
AS
BEGIN
	UPDATE dbo.CauHoi
	SET Diem = @1
	WHERE Ma = @0

	SELECT @1
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
		ThoiDiemCapNhat,
		MaNguoiTao,
		MaChuDe,
		Diem
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
--Lây câu hỏi theo mã người tạo
CREATE PROC dbo.layCauHoiTheoMaNguoiTao
(
	@0 INT --Mã người tạo
)
AS
BEGIN
	SELECT *
	FROM dbo.CauHoi
	WHERE MaNguoiTao = @0
END

GO
--Lấy toàn bộ Câu Hỏi
CREATE PROC dbo.layCauHoi (
	@0 INT --So dong lay
)
AS
BEGIN
	IF (@0 IS NULL)
	BEGIN
		SELECT *
		FROM dbo.CauHoi	
	END
	ELSE
	BEGIN
		EXEC('
			SELECT TOP ' + @0 + ' *
			FROM dbo.CauHoi	
		')
	END
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
CREATE PROC dbo.layCauHoiTheoMaChuDe_TimKiem
(
	@0 INT, --Mã Chủ Đề
	@1 NVARCHAR(MAX) --Từ khóa
)
AS
BEGIN
	SELECT *
	FROM dbo.CauHoi
	WHERE 
		MaChuDe = @0 AND 
		TieuDe LIKE '%' + REPLACE(@1, ' ', '%') + '%'
END

GO
--Xóa Trả Lời thuộc Câu Hỏi
CREATE TRIGGER dbo.xoaCauHoi_TRIGGER
ON dbo.CauHoi
AFTER DELETE
AS
	DECLARE @a INT
	
	SELECT @a = Ma FROM deleted

	EXEC dbo.xoaTraLoiTheoMaCauHoi @a	