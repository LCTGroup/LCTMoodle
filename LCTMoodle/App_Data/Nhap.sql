select * from khoahoc_nguoidung
delete from khoahoc_nguoidung
update khoahoc_nguoidung set trangthai = 1 where makhoahoc = 3
select * from khoahoc

update khoahoc set chedoriengtu = 'CongKhai'

select * from dbo.nguoidung
select * from dbo.cauhoi
select * from dbo.traloi
select * from dbo.ChuDe

insert into dbo.NguoiDungTam (TenTaiKhoan, MatKhau, Email, Ho, Ten, NgaySinh, DiaChi, SoDienThoai, MaHinhDaiDien, CoQuyenHT)
select TenTaiKhoan, MatKhau, Email, Ho, Ten, NgaySinh, DiaChi, SoDienThoai, MaHinhDaiDien, CoQuyenHT from dbo.NguoiDung

UPDATE dbo.NguoiDung
SET TenLot = Right(