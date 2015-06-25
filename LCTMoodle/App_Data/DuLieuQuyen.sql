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
--101

--2xx: Người dùng
--201
	
--3xx: Chủ đề
--301
	
--4xx: Hỏi đáp
--401
	
--5xx: Khóa học
--519
--	('KH', 5, 0,	N'','',0,0),		--|
	('KH', 501, 1,	N'Quản lý quyền',					'QuanLyQuyen',				0,		2),		--|

	('KH', 502, 0,	N'Quản lý',							NULL,						0,		1),		--|
	('KH', 503,	0,	N'Quản lý quyền',					'QuanLyQuyen',				502,	1),		--| |
	('KH', 504,	0,	N'Quản lý thành viên',				'QuanLyThanhVien',			502,	2),		--| |
	('KH', 505, 0,	N'Quản lý bài viết',				NULL,						502,	2),		--| |
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
	('KH', 518, 0,	N'Quản lý bài nộp',					'BT_BaiNop',				509,	3)		--| | | |

GO
SELECT * FROM dbo.Quyen