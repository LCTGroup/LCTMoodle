//#region Khởi tạo

$(function () {
    $form = $('#kich_hoat_tai_khoan');

    khoiTaoKichHoatTaiKhoan($form);
});

//#endregion

//#region Kích hoạt tài khoản

function khoiTaoKichHoatTaiKhoan($form) {
    khoiTaoLCTForm($form, {
        submit: function () {
            var $tai = moBieuTuongTai($form);
            $.ajax({
                url: '/NguoiDung/XuLyKichHoatTaiKhoan',
                method: 'POST',
                data: layDataLCTForm($form)
            }).always(function () {
                $tai.tat();
            }).done(function (data) {
                if (data.trangThai == 0) {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: 'Chức mừng bạn đã kích hoạt tài khoản thành công',
                        bieuTuong: 'thanh-cong',
                        nut: [{
                            ten: 'Về trang chủ',
                            href: '/TrangChu/'
                        }]
                    });
                } else {
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