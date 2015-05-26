function khoiTaoKhungBinhLuan($danhSachKhung) {
    $danhSachKhung.each(function () {
        var $khung = $(this);

        khoiTaoForm($khung.find('[data-doi-tuong="binh-luan-form"]'));
    });

    khoiTaoXoa($danhSachKhung.find('[data-chuc-nang="xoa-binh-luan"]'));
}

function khoiTaoForm($form) {
    khoiTaoLCTForm($form, {
        submit: function () {
            $.ajax({
                url: '/BinhLuan/Them',
                type: 'POST',
                data: $form.serialize(),
                dataType: 'JSON'
            }).done(function (data) {
                if (data.trangThai == 0) {
                    $form.closest('[data-doi-tuong="khung-binh-luan"]').find('[data-doi-tuong="danh-sach"]').append(data.ketQua);

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

function khoiTaoXoa($danhSachNut) {
    $danhSachNut.on('click', function () {
        var $nut = $(this);

        moPopup({
            tieuDe: 'Xác nhận',
            thongBao: 'Bạn có chắc muốn xóa bình luận này?',
            bieuTuong: 'hoi',
            nut: [
                {
                    ten: 'Có',
                    xuLy: function () {
                        var $mucBinhLuan = $nut.closest('[data-doi-tuong="muc-binh-luan"]');

                        $.ajax({
                            url: '/BinhLuan/Xoa/' + $nut.attr('data-value'),
                            data: { loaiDoiTuong: $mucBinhLuan.attr('data-loai-doi-tuong') },
                            type: 'POST',
                            dataType: 'JSON'
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                $mucBinhLuan.remove();
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
}