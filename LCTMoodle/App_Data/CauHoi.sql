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
	MaChuDe INT DEFAULT 0,
	Diem INT DEFAULT 0,
	SoLuongTraLoi INT DEFAULT 0,
	DuyetHienThi BIT DEFAULT 0
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
--Trigger thêm điểm Hỏi-đáp cho người đăng câu hỏi
CREATE TRIGGER dbo.themCauHoi_TRIGGER
ON dbo.CauHoi
AFTER INSERT
AS
BEGIN
	DECLARE @maNguoiTao INT
	
	SELECT @maNguoiTao = MaNguoiTao FROM INSERTED	

	UPDATE dbo.NguoiDung
	SET DiemHoiDap += 1
	WHERE Ma = @maNguoiTao	
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
--Xóa Trả Lời thuộc Câu Hỏi
ALTER TRIGGER dbo.xoaCauHoi_TRIGGER
ON dbo.CauHoi
AFTER DELETE
AS
BEGIN
	DECLARE @maCauHoi INT
	DECLARE @maNguoiTao INT
	
	SELECT @maCauHoi = Ma, @maNguoiTao = MaNguoiTao FROM deleted

	EXEC dbo.xoaTraLoiTheoMaCauHoi @maCauHoi	

	UPDATE dbo.NguoiDung
	SET DiemHoiDap -= 1
	WHERE Ma = @maNguoiTao
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
--Cập nhật Câu Hỏi theo mã - duyệt hiển thị
CREATE PROC dbo.capNhatCauHoiTheoMa_DuyetHienThi
(
	@0 INT,--Mã câu hỏi
	@1 BIT --Trạng thái
)
AS
BEGIN
	UPDATE dbo.CauHoi
	SET DuyetHienThi = @1
	WHERE Ma = @0
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
--Lây mã câu hỏi theo mã người tạo
ALTER PROC dbo.layCauHoi_DanhSachMaLienQuan
(
	@0 INT --Mã người tạo
)
AS
BEGIN
	DECLARE @dsMa VARCHAR(MAX) = ''

	SELECT @dsMa += CAST(Ma AS VARCHAR(MAX)) + ','
		FROM dbo.CauHoi
		WHERE MaNguoiTao = @0
		
	SELECT CASE
		WHEN @dsMa = '' THEN
			''
		ELSE
			LEFT(@dsMa, LEN(@dsMa) - 1)
		END
END

GO
--Lấy toàn bộ Câu Hỏi
ALTER PROC dbo.layCauHoi 
(
	@0 INT, --So dong lay
	@1 NVARCHAR(MAX) --Cách hiển thị
)
AS
BEGIN
	DECLARE @query NVARCHAR(MAX) =
	'
		SELECT ' + CASE 
			WHEN @0 IS NOT NULL THEN
				'TOP ' + CAST(@0 AS VARCHAR(MAX))
			ELSE
				''
			END + ' *
		FROM dbo.CauHoi
		WHERE DuyetHienThi = 1
		ORDER BY ' + CASE
			WHEN @1 = 'MoiNhat' THEN 'ThoiDiemTao'
			WHEN @1 = 'DiemCaoNhat' THEN 'Diem'
			WHEN @1 = 'TraLoiNhieuNhat' THEN 'SoLuongTraLoi'				
			ELSE 'Ma'
		END + ' DESC
	'

	EXEC(@query)
END

GO
--Lấy tất cả câu hỏi chưa được duyệt
ALTER PROC dbo.layCauHoi_ChuaDuyet
AS
BEGIN
	SELECT *
	FROM dbo.CauHoi
	WHERE DuyetHienThi = 0
END

GO
--Lấy Câu Hỏi theo từ khóa
ALTER PROC dbo.layCauHoi_TimKiem 
(
	@0 NVARCHAR(MAX), --Từ khóa
	@1 NVARCHAR(MAX) --Cách sắp xếp: "MoiNhat", "DiemCaoNhat", "TraLoiNhieuNhat"
)
AS
BEGIN
	SET @0 = '%' + REPLACE(@0, ' ', '%') + '%'
	SELECT *
	FROM dbo.CauHoi
	WHERE TieuDe LIKE @0 OR NoiDung LIKE @0	
	ORDER BY CASE
		WHEN @1 = 'MoiNhat'THEN ThoiDiemTao
		WHEN @1 = 'DiemCaoNhat' THEN Diem
		WHEN @1 = 'TraLoiNhieuNhat' THEN SoLuongTraLoi
		ELSE Ma
	END DESC
END 

GO
--Lấy Câu Hỏi theo Chủ Đề
ALTER PROC dbo.layCauHoiTheoMaChuDe_TimKiem
(
	@0 INT, --Mã Chủ Đề
	@1 NVARCHAR(MAX), --Từ khóa
	@2 NVARCHAR(MAX) --Cách hiển thị
)
AS
BEGIN
	SELECT *
	FROM dbo.CauHoi
	WHERE 
		MaChuDe = @0 AND 
		TieuDe LIKE '%' + REPLACE(@1, ' ', '%') + '%'
	ORDER BY CASE
		WHEN @2 = 'MoiNhat'THEN ThoiDiemTao
		WHEN @2 = 'DiemCaoNhat' THEN Diem
		WHEN @2 = 'TraLoiNhieuNhat' THEN SoLuongTraLoi
		ELSE Ma
	END DESC
END

GO
--Lấy phân trang
CREATE PROC dbo.layCauHoi_TimKiemPhanTrang 
(
	@0 NVARCHAR(MAX), --WHERE
	@1 NVARCHAR(MAX), --ORDER
	@2 INT, --Trang
	@3 INT --Số lượng mỗi trang
)
AS
BEGIN
	SET @0 = CASE WHEN @0 IS NULL THEN '' ELSE 'WHERE ' + @0 END
	SET @1 = CASE WHEN @1 IS NULL THEN 'ThoiDiemTao' ELSE @1 END
	
	DECLARE @trang VARCHAR(MAX)
	IF (@2 IS NULL OR @3 IS NULL)
	BEGIN
		SET @trang = ''
	END
	ELSE
	BEGIN
		SET @trang = 'WHERE ' + CAST((@2 - 1) * @3 AS VARCHAR(MAX)) + ' < Dong AND Dong <= ' + CAST(@2 * @3 AS VARCHAR(MAX))
	END

	EXEC('
		SELECT *
			FROM (
				SELECT
					*,
					ROW_NUMBER() OVER (ORDER BY ' + @1 + ') AS Dong,
					COUNT(1) OVER () AS TongSoDong
					FROM dbo.CauHoi
					' + @0 + '
			) AS KH
			' + @trang + '
			ORDER BY ' + @1 + '
	')
END