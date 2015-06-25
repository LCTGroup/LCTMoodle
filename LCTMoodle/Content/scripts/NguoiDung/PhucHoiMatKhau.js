//#region Khởi tạo

$(function () {
    $form = $('#phuc_hoi_mat_khau');

    khoiTaoPhucHoiMatKhau($form);
});

//#endregion

//#region Khởi tạo Phục hồi mật khẩu

function khoiTaoPhucHoiMatKhau() {
    khoiTaoLCTForm($form, {
        submit: function () {
            $.ajax({
                url: '/NguoiDung/XuLyPhucHoiMatKhau/',
                method: 'POST',
                data: layDataLCTForm($form)
            }).done(function (data) {
                if (data.trangThai == 0) {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: 'Gửi yêu cầu thành công',
                        bieuTuong: 'thanh-cong',
                        nut: [{
                            ten: 'Về trang chủ',
                            href: '/TrangChu/'
                        }]
                    });
                }
                else {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: data.ketQua,
                        bieuTuong: 'nguy-hiem'
                    });
                }
            });
        }
    });
}

//#endregion