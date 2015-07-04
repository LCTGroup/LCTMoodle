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
            var $tai = moBieuTuongTai($form);
            $.ajax({
                url: '/NguoiDung/XuLyPhucHoiMatKhau/',
                method: 'POST',
                data: layDataLCTForm($form)
            }).always(function () {
                $tai.tat();
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
                else if (data.trangThai == 1) {
                    moPopupThongBao("Email không tồn tại trong hệ thống.");
                }
                else {
                    moPopupThongBao(data);
                }
            });
        }
    });
}

//#endregion