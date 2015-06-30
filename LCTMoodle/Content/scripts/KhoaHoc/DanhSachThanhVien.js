var
    //Mã khóa học hiện tại
    maKhoaHoc;

$(function () {
    khoiTaoItem_ThanhVien($('#danh_sach_thanh_vien').find('.item'));
    khoiTaoItem_DangKy($('#danh_sach_dang_ky').find('.item'));
    khoiTaoitem_BiChan($('#danh_sach_bi_chan').find('.item'));
});

//#region Khởi tạo item

function khoiTaoItem_ThanhVien($items) {
    khoiTaoTatMoDoiTuong($items.find('[data-chuc-nang="tat-mo"]'), true);

    $items.find('[data-chuc-nang="xoa-thanh-vien"]').on('click', function () {
        var $item = $(this).closest('.item');

        var $tai = moBieuTuongTai($item);
        $.ajax({
            url: '/KhoaHoc/XuLyXoaThanhVien/' + maKhoaHoc,
            method: 'POST',
            data: { maNguoiDung: $item.attr('data-ma') }
        }).always(function () {
            $tai.tat();
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
                moPopupThongBao(data);
            }
        }).fail(function () {
            moPopupThongBao('Xóa thành viên thất bại');
        });
    });

    khoiTaoTatMoHocVien($items.find('[data-chuc-nang="tat-hoc-vien"]').data('mo', false));

    khoiTaoTatMoHocVien($items.find('[data-chuc-nang="mo-hoc-vien"]').data('mo', true));

    function khoiTaoTatMoHocVien($nuts) {
        $nuts.on('click', function () {
            var $nut = $(this);
            var $item = $nut.closest('.item');
            var mo = $nut.data('mo');

            var $tai = moBieuTuongTai($item);
            $.ajax({
                url: '/KhoaHoc/XuLyCapNhatHocVien/' + maKhoaHoc,
                method: 'POST',
                data: {
                    maNguoiDung: $item.attr('data-ma'),
                    laHocVien: mo
                },
                dataType: 'JSON'
            }).always(function () {
                $tai.tat();
            }).done(function (data) {
                if (data.trangThai == 0) {
                    $nut.text(mo ? 'Xóa khỏi danh sách học viên' : 'Đưa vào danh sách học viên');
                    $nut.data('mo', !mo);
                }
                else {
                    moPopupThongBao(data);
                }
            }).fail(function () {
                moPopupThongBao('Cập nhật học viên thất bại');
            });
        });
    }
}

function khoiTaoItem_DangKy($items) {
    $items.find('[data-chuc-nang="dong-y"]').on('click', function () {
        var $item = $(this).closest('.item');

        var $tai = moBieuTuongTai($item);
        $.ajax({
            url: '/KhoaHoc/XuLyChapNhanDangKy/' + maKhoaHoc,
            method: 'POST',
            data: { maNguoiDung: $item.attr('data-ma') }
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai == 0) {
                $item.remove();
            }
            else {
                moPopupThongBao(data);
            }
        }).fail(function () {
            moPopupThongBao('Chấp nhận đăng ký thất bại');
        });
    });

    $items.find('[data-chuc-nang="tu-choi"]').on('click', function () {
        var $item = $(this).closest('.item');

        var $tai = moBieuTuongTai($item);
        $.ajax({
            url: '/KhoaHoc/XuLyTuChoiDangKy/' + maKhoaHoc,
            method: 'POST',
            data: { maNguoiDung: $item.attr('data-ma') }
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai == 0) {
                $item.remove();
            }
            else {
                moPopupThongBao(data);
            }
        }).fail(function () {
            moPopupThongBao('Từ chối đăng ký thất bại');
        });
    });

    $items.find('[data-chuc-nang="chan"]').on('click', function () {
        var $item = $(this).closest('.item');

        var $tai = moBieuTuongTai($item);
        $.ajax({
            url: '/KhoaHoc/XuLyChanNguoiDung/' + maKhoaHoc,
            method: 'POST',
            data: { maNguoiDung: $item.attr('data-ma') }
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai == 0) {
                $item.remove();
            }
            else {
                moPopupThongBao(data);
            }
        }).fail(function () {
            moPopupThongBao('Chặn người dùng thất bại');
        });
    });
}

function khoiTaoitem_BiChan($items) {
    $items.find('[data-chuc-nang="bo-chan"]').on('click', function () {
        var $item = $(this).closest('.item');

        var $tai = moBieuTuongTai($item);
        $.ajax({
            url: '/KhoaHoc/XuLyHuyChanNguoiDung/' + maKhoaHoc,
            method: 'POST',
            data: { maNguoiDung: $item.attr('data-ma') }
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai == 0) {
                $item.remove();
            }
            else {
                moPopupThongBao(data);
            }
        }).fail(function () {
            moPopupThongBao('Bỏ chặn thất bại');
        });
    })
}

//#endregion