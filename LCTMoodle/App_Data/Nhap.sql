select ThoiDiemCapNhat from traloi
group by ThoiDiemCapNhat

select * from traloi

alter table dbo.TraLoi
add DuyetHienThi BIT DEFAULT 0

update traloi set DuyetHienThi = 0

update cauhoi set duyethienthi = 0 where ma = 318

select * from NguoiDung where DaDuyet = 0

alter table nguoidung
add DaDuyet BIT DEFAULT 0

update nguoidung set DaDuyet = 0 where ma=318

