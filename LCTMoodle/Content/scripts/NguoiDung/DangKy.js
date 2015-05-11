/*
    Khởi tạo
*/
$(function () {
    khoiTaoLCTForm($('#them_nguoi_dung'));

    khoiTaoThemNguoiDung($('#them_nguoi_dung'));
});

function khoiTaoThemNguoiDung(idForm) {
    $formObject = $(idForm);

    $formObject.submit(function (e) {
        e.preventDefault();        

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
        });
        
    });
}