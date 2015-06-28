function khoiTaoKhungBinhLuan($danhSachKhung) {
    $danhSachKhung.each(function () {
        var $khung = $(this);

        khoiTaoForm($khung.find('[data-doi-tuong="binh-luan-form"]'));
    });

    khoiTaoItem($danhSachKhung.find('[data-doi-tuong="muc-binh-luan"]'));
}

function khoiTaoForm($form) {
    khoiTaoLCTForm($form, {
        submit: function () {
            $.ajax({
                url: '/BinhLuan/XuLyThem',
                type: 'POST',
                data: $form.serialize(),
                dataType: 'JSON'
            }).done(function (data) {
                if (data.trangThai == 0) {
                    var $binhLuan = $(data.ketQua);
                    khoiTaoItem($binhLuan);

                    $form.closest('[data-doi-tuong="khung-binh-luan"]').find('[data-doi-tuong="danh-sach"]').append($binhLuan);

                    khoiTaoLCTFormMacDinh($form);
                }
                else {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: 'Thêm bình luận thất bại',
                        bieuTuong: 'nguy-hiem'
                    });
                }
            }).fail(function () {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Thêm bình luận thất bại',
                    bieuTuong: 'nguy-hiem'
                });
            });
        }
    });
}

function khoiTaoItem($items) {
    khoiTaoTatMoDoiTuong($items.find('[data-chuc-nang="tat-mo"]'));
    
    $items.find('[data-chuc-nang="xoa-binh-luan"]').on('click', function () {
        var $item = $(this).closest('[data-doi-tuong="muc-binh-luan"]');

        moPopup({
            tieuDe: 'Xác nhận',
            thongBao: 'Bạn có chắc muốn xóa bình luận này?',
            bieuTuong: 'hoi',
            nut: [
                {
                    ten: 'Có',
                    xuLy: function () {

                        $.ajax({
                            url: '/BinhLuan/XuLyXoa/' + $item.attr('data-ma'),
                            data: { loaiDoiTuong: $item.attr('data-loai-doi-tuong') },
                            type: 'POST',
                            dataType: 'JSON'
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                $item.remove();
                            }
                            else {
                                moPopup({
                                    tieuDe: 'Thông báo',
                                    thongBao: 'Xóa bài viết thất bại',
                                    bieuTuong: 'nguy-hiem'
                                })
                            }
                        }).fail(function () {
                            moPopup({
                                tieuDe: 'Thông báo',
                                thongBao: 'Xóa bài viết thất bại',
                                bieuTuong: 'nguy-hiem'
                            })
                        });
                    }
                },
                {
                    ten: 'Không',
                }
            ]
        });
    });

    $items.find('[data-chuc-nang="sua-binh-luan"]').on('click', function () {
        var $item = $(this).closest('[data-doi-tuong="muc-binh-luan"]');
        $.ajax({
            url: '/BinhLuan/_Form',
            data: { ma: $item.data('ma'), loaiDoiTuong: $item.data('loai-doi-tuong') },
            dataType: 'JSON'
        }).done(function (data) {
            if (data.trangThai == 0) {
                var $form = $(data.ketQua);

                $item.html($form);
                khoiTaoLCTForm($form, {
                    submit: function () {
                        $.ajax({
                            url: '/BinhLuan/XuLyCapNhat',
                            method: 'POST',
                            data: layDataLCTForm($form),
                            dataType: 'JSON'
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                var $newItem = $(data.ketQua);
                                khoiTaoItem($newItem);
                                $item.replaceWith($newItem);
                            }
                            else {
                                moPopupThongBao(data);
                            }
                        }).fail(function () {
                            moPopupThongBao('Cập nhật thất bại');
                        });
                    }
                });
            }
            else {
                moPopupThongBao(data);
            }
        }).fail(function () {
            moPopupThongBao('Lấy dữ liệu sửa thất bại');
        });
    })
}