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
            var $tai = moBieuTuongTai($form.closest('[data-doi-tuong="khung-binh-luan"]'));
            $.ajax({
                url: '/BinhLuan/XuLyThem',
                type: 'POST',
                data: $form.serialize(),
                dataType: 'JSON'
            }).always(function () {
                $tai.tat();
            }).done(function (data) {
                if (data.trangThai == 0) {
                    var $binhLuan = $(data.ketQua);
                    khoiTaoItem($binhLuan);

                    $form.closest('[data-doi-tuong="khung-binh-luan"]').find('[data-doi-tuong="danh-sach"]').append($binhLuan);

                    khoiTaoLCTFormMacDinh($form);
                }
                else {
                    moPopupThongBao(data);
                }
            }).fail(function () {
                moPopupThongBao('Thêm bình luận thất bại');
            });
        }
    });
}

function khoiTaoItem($items) {
    khoiTaoTatMoDoiTuong($items.find('[data-chuc-nang="tat-mo"]'), true);
    
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
                        var $tai = moBieuTuongTai($item);
                        $.ajax({
                            url: '/BinhLuan/XuLyXoa/' + $item.attr('data-ma'),
                            data: { loaiDoiTuong: $item.attr('data-loai-doi-tuong') },
                            type: 'POST',
                            dataType: 'JSON'
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
                            moPopupThongBao('Xóa bài viết thất bại');
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
        var $khung = $item.closest('[data-doi-tuong="khung-binh-luan"]');

        var $tai = moBieuTuongTai($khung);
        $.ajax({
            data: { ma: $item.data('ma'), loaiDoiTuong: $item.data('loai-doi-tuong') },
            dataType: 'JSON'
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai == 0) {
                var $form = $(data.ketQua);

                $item.html($form);
                khoiTaoLCTForm($form, {
                    submit: function () {
                        $tai = moBieuTuongTai($khung);
                        $.ajax({
                            url: '/BinhLuan/XuLyCapNhat',
                            method: 'POST',
                            data: layDataLCTForm($form),
                            dataType: 'JSON'
                        }).always(function () {
                            $tai.tat();
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