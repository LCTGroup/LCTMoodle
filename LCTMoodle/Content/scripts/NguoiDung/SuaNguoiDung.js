//#region Khởi tạo

$(function () {
    var $form = $('[data-doi-tuong="form-sua-nguoi-dung"]');

    khoiTaoFormSuaNguoiDung($form);
});

//#endregion

//#region khởi tạo form sửa người dùng

function khoiTaoFormSuaNguoiDung($form) {
    var ma = $form.attr('data-ma');
    var tenTaiKhoan = $form.attr('data-ten-tai-khoan');

    khoiTaoLCTForm($form, {
        submit: function () {
            $.ajax({
                url: '/NguoiDung/XuLyCapNhat',
                method: 'POST',
                data: layDataLCTForm($form),
                async: false
            }).done(function (data) {
                if (data.trangThai == 0) {
                    window.location = '/NguoiDung/Xem/' + ma;
                }
                else {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: 'Cập nhật người dùng thất bại',
                        bieuTuong: 'nguy-hiem'
                    });
                }
            });
        },
        validates:
        [{
            input: $form.find('[data-chuc-nang="reset"]'),
            customEvent: {
                'click': function () {
                    khoiTaoLCTFormMacDinh($form);
                }
            }
        }]
    });

}

//#endregion