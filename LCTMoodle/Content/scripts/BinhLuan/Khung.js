function khoiTaoKhungBinhLuan($danhSachKhung) {
    $danhSachKhung.each(function () {
        var $khung = $(this);

        khoiTaoForm($khung.find('[data-doi-tuong="binh-luan-form"]'));
    });
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