﻿var
    //Mã khóa học hiện tại
    maKhoaHoc;

$(function () {
    khoiTaoItem_ThanhVien($('#danh_sach_thanh_vien').find('.item'));
    khoiTaoItem_DangKy($('#danh_sach_dang_ky').find('.item'));
    khoiTaoitem_BiChan($('#danh_sach_bi_chan').find('.item'));
});

//#region Khởi tạo item

function khoiTaoItem_ThanhVien($items) {
    khoiTaoTatMoDoiTuong($items.find('[data-chuc-nang="tat-mo"]'));

    $items.find('[data-chuc-nang="xoa-thanh-vien"]').on('click', function () {
        var $item = $(this).closest('.item');

        $.ajax({
            url: '/KhoaHoc/XuLyXoaThanhVien/' + maKhoaHoc,
            method: 'POST',
            data: { maNguoiDung: $item.attr('data-ma') }
        }).done(function (data) {
            if (data.trangThai == 0) {
                $item.remove();
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Xóa thành viên thành công',
                    bieuTuong: 'thanh-cong'
                });
            }
            else {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Xóa thành viên thất bại',
                    bieuTuong: 'nguy-hiem'
                });
            }
        }).fail(function () {
            moPopup({
                tieuDe: 'Thông báo',
                thongBao: 'Xóa thành viên thất bại',
                bieuTuong: 'nguy-hiem'
            });
        });
    });

    khoiTaoTatMoHocVien($items.find('[data-chuc-nang="tat-hoc-vien"]').data('mo', false));

    khoiTaoTatMoHocVien($items.find('[data-chuc-nang="mo-hoc-vien"]').data('mo', true));

    function khoiTaoTatMoHocVien($nuts) {
        $nuts.on('click', function () {
            var $nut = $(this);
            var mo = $nut.data('mo');

            $.ajax({
                url: '/KhoaHoc/XuLyCapNhatHocVien/' + maKhoaHoc,
                method: 'POST',
                data: {
                    maNguoiDung: $nut.closest('.item').attr('data-ma'),
                    laHocVien: mo
                },
                dataType: 'JSON'
            }).done(function (data) {
                if (data.trangThai == 0) {
                    $nut.text(mo ? 'Xóa khỏi danh sách học viên' : 'Đưa vào danh sách học viên');
                    $nut.data('mo', !mo);
                }
                else {
                    moPopup({
                        thongBao: 'Cập nhật học viên thất bại'
                    });
                }
            }).fail(function () {
                moPopup({
                    thongBao: 'Cập nhật học viên thất bại'
                });
            })
        })
    }
}

function khoiTaoItem_DangKy($items) {
    $items.find('[data-chuc-nang="dong-y"]').on('click', function () {
        var $item = $(this).closest('.item');

        $.ajax({
            url: '/KhoaHoc/XuLyChapNhanDangKy/' + maKhoaHoc,
            method: 'POST',
            data: { maNguoiDung: $item.attr('data-ma') }
        }).done(function (data) {
            if (data.trangThai == 0) {
                $item.remove();
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Chấp nhận đăng ký thành công',
                    bieuTuong: 'thanh-cong'
                });
            }
            else {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Chấp nhận đăng ký thất bại',
                    bieuTuong: 'nguy-hiem'
                });
            }
        }).fail(function () {
            moPopup({
                tieuDe: 'Thông báo',
                thongBao: 'Chấp nhận đăng ký thất bại',
                bieuTuong: 'nguy-hiem'
            });
        });
    });

    $items.find('[data-chuc-nang="tu-choi"]').on('click', function () {
        var $item = $(this).closest('.item');

        $.ajax({
            url: '/KhoaHoc/XuLyTuChoiDangKy/' + maKhoaHoc,
            method: 'POST',
            data: { maNguoiDung: $item.attr('data-ma') }
        }).done(function (data) {
            if (data.trangThai == 0) {
                $item.remove();
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Từ chối đăng ký thành công',
                    bieuTuong: 'thanh-cong'
                });
            }
            else {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Từ chối đăng ký thất bại',
                    bieuTuong: 'nguy-hiem'
                });
            }
        }).fail(function () {
            moPopup({
                tieuDe: 'Thông báo',
                thongBao: 'Từ chối đăng ký thất bại',
                bieuTuong: 'nguy-hiem'
            });
        });
    });

    $items.find('[data-chuc-nang="chan"]').on('click', function () {
        var $item = $(this).closest('.item');

        $.ajax({
            url: '/KhoaHoc/XuLyChanNguoiDung/' + maKhoaHoc,
            method: 'POST',
            data: { maNguoiDung: $item.attr('data-ma') }
        }).done(function (data) {
            if (data.trangThai == 0) {
                $item.remove();
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Chặn người dùng thành công',
                    bieuTuong: 'thanh-cong'
                });
            }
            else {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Chặn người dùng thất bại',
                    bieuTuong: 'nguy-hiem'
                });
            }
        }).fail(function () {
            moPopup({
                tieuDe: 'Thông báo',
                thongBao: 'Chặn người dùng thất bại',
                bieuTuong: 'nguy-hiem'
            });
        });
    });
}

function khoiTaoitem_BiChan($items) {
    $items.find('[data-chuc-nang="bo-chan"]').on('click', function () {
        var $item = $(this).closest('.item');

        $.ajax({
            url: '/KhoaHoc/XuLyHuyChanNguoiDung/' + maKhoaHoc,
            method: 'POST',
            data: { maNguoiDung: $item.attr('data-ma') }
        }).done(function (data) {
            if (data.trangThai == 0) {
                $item.remove();
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Bỏ chặn thành công',
                    bieuTuong: 'thanh-cong'
                });
            }
            else {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Bỏ chặn thất bại',
                    bieuTuong: 'nguy-hiem'
                });
            }
        }).fail(function () {
            moPopup({
                tieuDe: 'Thông báo',
                thongBao: 'Bỏ chặn thất bại',
                bieuTuong: 'nguy-hiem'
            });
        });
    })
}

//#endregion