--Mỗi khi thay đổi => chạy toàn bộ file này
GO
truncate table dbo.HanhDong

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
--	Ma		LoiNhan

	--Thêm người dùng
	(100, N'{ND} đã tham gia vào hệ thống'),

--2xx: Người dùng
--	Ma		LoiNhan

	--Cập nhật lại thông tin cá nhân
	(200, N'{ND} đã cập nhật lại thông tin cá nhân'),

	--Phục hồi mật khẩu
	(201, N'{ND} đã phục hồi mật khẩu'),

	--Chặn người dùng
	(202, N'{BD} đã bị chặn bởi {ND}'),

	--Kích hoạt người dùng
	(203, N'{ND} đã được kích hoạt'),
	
	--Bị lủng lỗ
	(204, N''),

	--Đổi mật khẩu
	(205, N'{ND} đã đổi mật khẩu'),

--3xx: Chủ đề
--	Ma		LoiNhan
	(300, N''),	

--4xx: Hỏi đáp
--	Ma		LoiNhan

	--Cộng điểm câu hỏi
	(400, N'{ND} đã cho điểm cộng câu hỏi {BD}'),
		
	--Trừ điểm câu hỏi
	(401, N'{ND} đã cho điểm trừ câu hỏi {BD}'),

	--Bỏ cho điểm câu hỏi
	(402, N'{ND} đã bỏ cho điểm câu hỏi {BD}'),

	--Cộng điểm trả lời
	(403, N'{ND} đã cho điểm câu trả lời {BD}'),

	--Trừ điểm trả lời
	(404, N'{ND} đã trừ cho điểm câu trả lời {BD}'),

	--Bỏ cho điểm câu trả lời
	(405, N'{ND} đã bỏ cho điểm câu trả lời {BD}'),

	--Thêm câu hỏi
	(406, N'{ND} đã thêm câu hỏi {BD}'),

	--Xóa câu hỏi
	(407, N'{ND} đã xóa câu hỏi {BD}'),

	--Cập nhật câu hỏi
	(408, N'{ND} đã cập nhật câu hỏi {BD}'),

	--Duyệt hiển thị câu hỏi
	(409, N'{ND} đã duyệt hiển thị cho câu hỏi {BD}'),

	--Thêm câu trả lời
	(410, N'{ND} đã thêm câu trả lời {BD}'),

	--Xóa câu trả lời
	(411, N'{ND} đã xóa câu trả lời {BD}'),

	--Cập nhật câu trả lời
	(412, N'{ND} đã cập nhật câu trả lời {BD}'),

	--Duyệt hiển thị câu trả lời
	(413, N'{ND} đã duyệt hiển thị cho câu trả lời {BD}'),

	--Duyệt câu trả lời đúng
	(414, N'{ND} đã duyệt câu trả lời {BD}'),

--5xx: Khóa học
--	Ma		LoiNhan
	(500, N''),		

--6xx: Quyền
--	Ma		LoiNhan
	(600, N'')