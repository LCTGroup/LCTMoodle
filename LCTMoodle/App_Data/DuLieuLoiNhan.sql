--Mỗi khi thay đổi => chạy toàn bộ file này
GO
truncate table dbo.LoiNhanHoatDong

--Mã
	--1xx: Hệ thống
	--2xx: Người dùng
	--3xx: Chủ đề
	--4xx: Hỏi đáp
	--5xx: Khóa học
	--6xx: Quyền

GO
INSERT INTO dbo.LoiNhanHoatDong (MaHanhDong, LoiNhanChuDong, LoiNhanBiDong) VALUES
--	MaHanhDong		ChuDong,		BiDong,			ChuDongNgoai,		BiDongNgoai

--1xx: Hệ thống
	(100,			
			'',				
			'',
			'',
			''),	
--2xx: Người dùng
--	MaHanhDong		LoiNhanChuDong,		LoiNhanBiDong
	(200,			'b',				'b'),

--3xx: Chủ đề
--	MaHanhDong		LoiNhanChuDong,		LoiNhanBiDong
	(300,			'c',				'c'),

--4xx: Hỏi đáp
--	MaHanhDong		LoiNhanChuDong,		LoiNhanBiDong

--	Lời nhắn cho điểm cộng câu hỏi
	(400,
		'Bạn đã cho điểm cộng câu hỏi {BD}',
		'Câu hỏi {CD} đã được bạn cho điểm cộng',
		NULL,
		NULL),

	(401,
		'Bạn đã cho điểm trừ câu hỏi {BD}',
		'Câu hỏi {CD} đã được bạn cho điểm trừ',
		NULL,
		NULL),

--5xx: Khóa học
--	MaHanhDong		LoiNhanChuDong,		LoiNhanBiDong
	(500,			'e',				'e'),

--6xx: Quyền
--	MaHanhDong		LoiNhanChuDong,		LoiNhanBiDong
	(600,			'f',				'f')