GO
--Bảng tin nhắn
CREATE TABLE dbo.TinNhan
(
	Ma INT IDENTITY(1,1) NOT NULL,
	MaNguoiGui INT,
	MaNguoiNhan INT,
	NoiDung NVARCHAR(MAX),
	ThoiDiemGui DATETIME DEFAULT GETDATE(),
	DaDoc BIT DEFAULT 0
)

GO
--Thêm tin nhắn
ALTER PROC dbo.themTinNhan
(
	@0 INT, --Mã người gửi
	@1 INT, --Mã người nhận
	@2 NVARCHAR(MAX) --Nội dung
)
AS
BEGIN
	INSERT INTO dbo.TinNhan (MaNguoiGui, MaNguoiNhan, NoiDung)
	VALUES (@0, @1, @2)

	SELECT * 
		FROM dbo.TinNhan
			WHERE Ma = @@IDENTITY
END

GO
--Lấy tin nhắn theo mã người gửi
ALTER PROC dbo.layTinNhanTheoMaNguoiGuiVaMaNguoiNhan
(
	@0 INT, --Mã người gửi
	@1 INT --Mã người nhận
)
AS
BEGIN
	SELECT *
	FROM dbo.TinNhan
	WHERE (MaNguoiGui = @0 AND MaNguoiNhan = @1) OR (MaNguoiGui = @1 AND MaNguoiNhan = @0)
	ORDER BY ThoiDiemGui ASC
END

GO
--Lấy danh sách tin nhắn của người dùng
ALTER PROC dbo.layTinNhanTheoMaNguoiDung
(
	@0 INT --Mã người dùng
)
AS
BEGIN
	SELECT *
		FROM
			(SELECT 
				*,
				ROW_NUMBER() OVER (PARTITION BY MaNguoiGiaoTiep ORDER BY ThoiDiemGui DESC) Dong
				FROM 
					(SELECT 
						*,
						(CASE 
							WHEN MaNguoiGui = @0 THEN
								MaNguoiNhan
							ELSE
								MaNguoiGui
						END) MaNguoiGiaoTiep
						FROM dbo.TinNhan
						WHERE 
							MaNguoiGui = @0 OR
							ManguoiNhan = @0) TN) TN
		WHERE Dong = 1
		ORDER BY ThoiDiemGui DESC
END

themTinNhan 1,2, "Làm báo cáo chưa?"