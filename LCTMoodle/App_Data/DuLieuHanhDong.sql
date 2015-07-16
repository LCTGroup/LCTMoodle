--Mỗi khi thay đổi => chạy toàn bộ file này
GO
truncate table dbo.LoiNhanHanhDong

--Mã
	--1xx: Hệ thống
	--2xx: Người dùng
	--3xx: Chủ đề
	--4xx: Hỏi đáp
	--5xx: Khóa học
	--6xx: Quyền

-- {BD}: Tên của đối tượng bị động
-- {CD}: Tên của đối tượng chủ động
-- {ND}: Tên của người dùng tác động
-- {GTC}: Giá trị cũ
-- {GTM}: Giá trị mới

GO
INSERT INTO dbo.HanhDong (Ma, LoiNhan) VALUES

--1xx: Hệ thống
	(100, N''),	
--2xx: Người dùng
--	MaHanhDong		LoiNhanChuDong,		LoiNhanBiDong
	(200, N''),	

--3xx: Chủ đề
--	MaHanhDong		LoiNhanChuDong,		LoiNhanBiDong
	(300, N''),	

--4xx: Hỏi đáp
--	MaHanhDong		LoiNhanChuDong,		LoiNhanBiDong

	--Cộng điểm câu hỏi
	(400, N'{ND} đã cho điểm cộng câu hỏi {BD}'),
		
	--Trừ điểm câu hỏi
	(401, N'{ND} đã cho điểm trừ câu hỏi {BD}'),

--5xx: Khóa học
--	MaHanhDong		LoiNhanChuDong,		LoiNhanBiDong
	(500, N''),		

--6xx: Quyền
--	MaHanhDong		LoiNhanChuDong,		LoiNhanBiDong
	(600, N'')