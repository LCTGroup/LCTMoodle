/*
    Khởi tạo
*/
$(function () {
    $form = $('#them_nguoi_dung');

    khoiTaoThemNguoiDung($form);
});

function khoiTaoThemNguoiDung($form) {    
    khoiTaoLCTForm($form, {
        submit: function () {
            $.ajax({
                url: $form.attr('action'),
                method: $form.attr('method'),            
                data: $form.serialize(),
                async: false
            }).done(function (data) {                
                switch(data.trangThai) {
                    case 0:
                        moPopup({
                            tieuDe: 'Thông báo',
                            thongBao: '\
                            <div style="width: 26px; height: 26px; background-position-x: -370px;" class="site-image"></div> \
                            <span style="padding-left: 10px; line-height: 26px;">Đăng ký thành công.</span>\
                        ',
                            nut: [{
                                ten: 'Về trang chủ',
                                xuLy: function () {
                                    window.location = '/TrangChu/';
                                }
                            }],
                            esc: false
                        });
                        break;
                    case 1:
                        moPopup({
                            tieuDe: 'Thông báo',
                            thongBao: '\
                            <div style="width: 26px; height: 26px; background-position-x: -370px;" class="site-image"></div> \
                            <span style="padding-left: 10px; line-height: 26px;">Đăng ký chưa thành công.</span>\
                        '
                        });
                        break;
                    case 2:
                        moPopup({
                            tieuDe: 'Thông báo',
                            thongBao: '\
                            <div style="width: 26px; height: 26px; background-position-x: -370px;" class="site-image"></div> \
                            <span style="padding-left: 10px; line-height: 26px;">Lỗi kết nối CSDL hoặc lỗi truy vấn.</span>\
                        '
                        });
                        break;
                    case 3:
                        moPopup({
                            tieuDe: 'Thông báo',
                            thongBao: '\
                            <div style="width: 26px; height: 26px; background-position-x: -370px;" class="site-image"></div> \
                            <span style="padding-left: 10px; line-height: 26px;">Lỗi ràng buộc dữ liệu.</span>\
                        '
                        });
                        break;
                }
            }).fail(function () {               
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: '\
                    <div style="width: 26px; height: 26px; background-position-x: -370px;" class="site-image"></div> \
                    <span style="padding-left: 10px; line-height: 26px;">Lỗi Ajax</span>\
                '
                });
            })
        }
    });
}