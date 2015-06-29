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
--102
--	('HT', 1, 0,	N'','',0,1),		--|
--	  PV   Ma,  C	Tên									Giá trị						Cha		TT
	('HT', 101, 0,	N'Quản lý quyền',					'Rieng',					0,		1),		--|

--2xx: Người dùng
--203
--	('ND', 2, 0,	N'','',0,1),		--|
--	  PV   Ma,  C	Tên									Giá trị						Cha		TT
	('ND', 201, 1,	N'Chung',							'Chung',					0,		1),		--|

	('ND', 202, 0,	N'Riêng',							'Rieng',					0,		1),		--|
	
--3xx: Chủ đề
--305
--	('CD', 3, 0,	N'','',0,1),		--|
--	  PV   Ma,  C	Tên									Giá trị						Cha		TT
	('CD', 301, 1,	N'Thêm, xóa, sửa chủ đề',			'QLNoiDung',				0,		1),		--|
	('CD', 302, 1,	N'Duyệt chủ đề',					'Duyet',					0,		2),		--|
	
	('CD', 303, 0,	N'Thêm, xóa, sửa chủ đề',			'QLNoiDung',				0,		1),		--|
	('CD', 304, 0,	N'Duyệt chủ đề',					'Duyet',					0,		2),		--|
	
--4xx: Hỏi đáp
--403
--	('HD', 4, 0,	N'','',0,1),		--|
--	  PV   Ma,  C	Tên									Giá trị						Cha		TT
	('HD', 401, 1,	N'Chung',							'Chung',					0,		1),		--|

	('HD', 402, 0,	N'Riêng',							'Rieng',					0,		1),		--|
	
--5xx: Khóa học
--524
--	('KH', 5, 0,	N'','',0,1),		--|
--	  PV   Ma,  C	Tên									Giá trị						Cha		TT
	('KH', 520, 1,	N'Thêm, xóa, sửa khóa học',			'QLNoiDung',				0,		1),		--|
	('KH', 501, 1,	N'Quản lý quyền',					'QLQuyen',					0,		2),		--|

	('KH', 502, 0,	N'Quản lý',							NULL,						0,		1),		--|
	('KH', 503,	0,	N'Quản lý quyền',					'QLQuyen',					502,	1),		--| |
	('KH', 504,	0,	N'Quản lý thành viên',				'QLThanhVien',				502,	2),		--| |
	('KH', 505, 0,	N'Quản lý bài viết',				NULL,						502,	3),		--| |
	('KH', 506, 0,	N'Diễn đàn',						NULL,						505,	1),		--| | |
	('KH', 510, 0,	N'Sửa bài viết',					'DD_Sua',					506,	1),		--| | | |
	('KH', 511, 0,	N'Xóa bài viết',					'DD_Xoa',					506,	2),		--| | | |
	('KH', 512, 0,	N'Ghim bài viết',					'DD_Ghim',					506,	3),		--| | | |
	('KH', 507, 0,	N'Bài giảng',						NULL,						505,	2),		--| | |
	('KH', 513, 0,	N'Sửa bài viết',					'BG_Sua',					507,	1),		--| | | |
	('KH', 514, 0,	N'Xóa bài viết',					'BG_Xoa',					507,	2),		--| | | |
	('KH', 515, 0,	N'Thay đổi thứ tự bài viết',		'BG_ThuTu',					507,	3),		--| | | |
	('KH', 508, 0,	N'Tài liệu',						NULL,						505,	3),		--| | |
	('KH', 509, 0,	N'Bài tập',							NULL,						505,	4),		--| | |
	('KH', 516, 0,	N'Sửa bài viết',					'BT_Sua',					509,	1),		--| | | |
	('KH', 517, 0,	N'Xóa bài viết',					'BT_Xoa',					509,	2),		--| | | |
	('KH', 518, 0,	N'Quản lý bài nộp',					'BT_BaiNop',				509,	3),		--| | | |
	('KH', 519, 0,	N'Quản lý bảng điểm',				NULL,						502,	4),		--| |
	('KH', 521, 0,	N'Quản lý cột điểm',				'QLCotDiem',				519,	1),		--| | |
	('KH', 522, 0,	N'Quản lý điểm',					'QLDiem',					519,	2),		--| | |
	('KH', 523, 0,	N'Quản lý chương trình',			'QLChuongTrinh',			502,	5)		--| |

GO
SELECT * FROM dbo.Quyen