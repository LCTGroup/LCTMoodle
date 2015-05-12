/*
    Khởi tạo
*/
$(function () {
    $form = $('#them_nguoi_dung')

    khoiTaoThemNguoiDung($form);
});

function khoiTaoThemNguoiDung($form) {    
    khoiTaoLCTForm($form, {
        submit: function () {      
            $.ajax({
                url: $(this).attr('action'),
                method: $(this).attr('method'),            
                data: $(this).serialize(),
                async: false
            }).done(function (data) {
                if (data.trangThai == 0) {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: '\
                        <div style="width: 26px; height: 26px; background-position-x: -370px;" class="site-image"></div> \
                        <span style="padding-left: 10px; line-height: 26px;">Bạn đã đăng ký thành công!</span>\
                    '
                    });
                }
            })
        }
    });
}