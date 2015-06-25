//#region Khởi Tạo

$(function () {
    $form = $('#doi_mat_khau');

    khoiTaoDoiMatKhau($form);
});

//#endregion

//#region Đổi mật khẩu

function khoiTaoDoiMatKhau($form) {
    var maNguoiDung = $form.attr('data-ma');

    khoiTaoLCTForm($form, {
        submit: function () {
            $.ajax({
                url: '/NguoiDung/XuLyDoiMatKhau/',
                method: 'POST',
                data: layDataLCTForm($form)
            }).done(function (data) {
                if (data.trangThai == 0) {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: 'Đổi mật khẩu thành công',
                        bieuTuong: 'thanh-cong',
                        nut: [{
                            ten: 'Về trang cá nhân',
                            href: '/NguoiDung/Xem/' + maNguoiDung
                        }]
                    });
                }
                else
                {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: data.ketQua,
                        bieuTuong: 'nguy-hiem'
                    });
                }
            });
        },
        custom: [
            {
                input: $('#NhapLaiMatKhauMoi'),
                thongBao: 'Nhập lại mật khẩu chưa khớp',
                validate: function () {
                    if ($('#MatKhauMoi').val() != $('#NhapLaiMatKhauMoi').val()) {
                        return false;
                    }
                }
            }
        ]
    });
}

//#endregion