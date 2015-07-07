--Mỗi khi thay đổi => chạy toàn bộ file này
GO
truncate table dbo.Quyen

--Mã
	--1xx: Hệ thống
	--2xx: Người dùng
	--3xx: Chủ đề
	--4xx: Hỏi đáp
	--5xx: Khóa học
--Cẩn thận, mỗi lần thay đổi mã sẽ ảnh hưởng đến bảng NhomNguoiDung_XX_Quyen

GO
INSERT INTO dbo.Quyen (PhamVi, Ma, LaQuyenChung, Ten, GiaTri, MaCha, ThuTu) VALUES
--1xx: Hệ thống
--103
--	('HT', 1, 0,	N'','',0,1),		--|
--	  PV   Ma,  C	Tên									Giá trị						Cha		TT
	('HT', 102, 1,	'',									'QLDB',						0,		-1),

	('HT', 101, 0,	N'Quản lý quyền',					'QLQuyen',					0,		1),		--|

--2xx: Người dùng
--203
--	('ND', 2, 0,	N'','',0,1),		--|
--	  PV   Ma,  C	Tên									Giá trị						Cha		TT
	('ND', 201, 1,	'',									'QLDB',						0,		-1),

	('ND', 202, 0,	N'Quản lý người dùng',				'QuanLyNguoiDung',			0,		1),		--|
	
--3xx: Chủ đề
--305
--	('CD', 3, 0,	N'','',0,1),		--|
--	  PV   Ma,  C	Tên									Giá trị						Cha		TT
	('CD', 304, 1,	'',									'QLDB',						0,		-1),	

	('CD', 301, 0,	N'Quản lý quyền',					'QLQuyen',					0,		1),		--|
	('CD', 302, 0,	N'Thêm, xóa, sửa chủ đề',			'QLNoiDung',				0,		2),		--|
	('CD', 303, 0,	N'Duyệt chủ đề',					'Duyet',					0,		3),		--|
	
--4xx: Hỏi đáp
--403
--	('HD', 4, 0,	N'','',0,1),		--|
--	  PV   Ma,  C	Tên									Giá trị						Cha		TT
	('HD', 402, 1,	'',									'QLDB',						0,		-1),

	('HD', 401, 0,	N'Quản lý hỏi đáp',					NULL,						0,		1),		--|

	('HD', 405, 0,	N'Duyệt câu hỏi',					'DuyetCauHoi',				401,	1),		--| |
	('HD', 403, 0,	N'Sửa câu hỏi',						'SuaCauHoi',				401,	2),		--| |
	('HD', 404, 0,	N'Xóa câu hỏi',						'XoaCauHoi',				401,	3),		--| |
	('HD', 406, 0,	N'Duyệt trả lời',					'DuyetTraLoi',				401,	4),		--| |
	('HD', 407, 0,	N'Sửa trả lời',						'SuaTraLoi',				401,	5),		--| |
	('HD', 408, 0,	N'Xóa trả lời',						'XoaTraLoi',				401,	6),		--| |
	
--5xx: Khóa học
--532
--	('KH', 5, 0,	N'','',0,1),		--|
--	  PV   Ma,  C	Tên									Giá trị						Cha		TT
	('KH', 530, 1,	'',									'QLDB',						0,		-1),

	('KH', 520, 1,	N'Thêm, xóa, sửa khóa học',			'QLNoiDung',				0,		1),		--|
	('KH', 501, 1,	N'Quản lý quyền',					'QLQuyen',					0,		2),		--|

	('KH', 502, 0,	N'Quản lý',							NULL,						0,		1),		--|
	('KH', 503,	0,	N'Quản lý quyền',					'QLQuyen',					502,	1),		--| |
	('KH', 504,	0,	N'Quản lý thành viên',				'QLThanhVien',				502,	2),		--| |
	('KH', 505, 0,	N'Quản lý nội dung',				NULL,						502,	3),		--| |

	('KH', 531, 0,	N'Quản lý thông tin',				'QLThongTin',				505,	1),		--| | |

	('KH', 506, 0,	N'Diễn đàn',						NULL,						505,	2),		--| | |
	('KH', 510, 0,	N'Quản lý nội dung bài viết',		'DD_QLNoiDung',				506,	1),		--| | | |
	('KH', 511, 0,	N'Quản lý điểm thảo luận',			'DD_QLDiem',				506,	2),		--| | | |

	('KH', 507, 0,	N'Bài giảng',						NULL,						505,	3),		--| | |
	('KH', 524, 0,	N'Đăng bài giảng',					'BG_Them',					507,	1),		--| | | |
	('KH', 513, 0,	N'Sửa bài giảng',					'BG_Sua',					507,	2),		--| | | |
	('KH', 514, 0,	N'Xóa bài giảng',					'BG_Xoa',					507,	3),		--| | | |
	('KH', 515, 0,	N'Thay đổi thứ tự bài giảng',		'BG_ThuTu',					507,	3),		--| | | |

	('KH', 508, 0,	N'Tài liệu',						NULL,						505,	4),		--| | |
	('KH', 526, 0,	N'Đăng tài liệu',					'TL_Them',					508,	1),		--| | | |
	('KH', 527, 0,	N'Sửa tài liệu',					'TL_Sua',					508,	2),		--| | | |
	('KH', 528, 0,	N'Xóa tài liệu',					'TL_Xoa',					508,	3),		--| | | |
	('KH', 529, 0,	N'Thay đổi thứ tự bài giảng',		'TL_ThuTu',					508,	3),		--| | | |

	('KH', 509, 0,	N'Bài tập',							NULL,						505,	5),		--| | |
	('KH', 525, 0,	N'Đăng bài tập',					'BT_Them',					509,	1),		--| | | |
	('KH', 516, 0,	N'Sửa bài tập',						'BT_Sua',					509,	2),		--| | | |
	('KH', 517, 0,	N'Xóa bài tập',						'BT_Xoa',					509,	3),		--| | | |
	('KH', 518, 0,	N'Quản lý bài nộp',					'BT_QLBaiNop',				509,	4),		--| | | |

	('KH', 519, 0,	N'Quản lý bảng điểm',				'QLBangDiem',				502,	4),		--| |

	('KH', 523, 0,	N'Quản lý chương trình',			'QLChuongTrinh',			502,	5)		--| |