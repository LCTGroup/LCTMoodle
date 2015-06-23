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
            $.ajax({
                url: '/NguoiDung/XuLyKichHoatTaiKhoan',
                method: 'POST',
                data: layDataLCTForm($form)
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
                }
                else if (data.trangThai == 3)
                {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: data.ketQua,
                        bieuTuong: 'nguy-hiem'                        
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