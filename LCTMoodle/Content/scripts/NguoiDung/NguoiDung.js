/*
    Khởi tạo
*/
$(function () {
    //Khởi tạo xử lý đăng ký    
    khoiTaoThemNguoiDung($('#dang_ky'));

    //Khởi tạo xử lý đăng nhập
    khoiTaoDangNhap($('#dang_nhap'));
});

//Xử lý đăng ký
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
                            thongBao: 'Đăng ký thành công',
                            nut: [{
                                ten: 'Về trang chủ',
                                href: '/TrangChu/'
                            }],
                            esc: false,
                            bieuTuong: 'thanh-cong'
                        });
                        break;
                    case 1:
                        moPopup({
                            tieuDe: 'Thông báo',
                            thongBao: 'Đăng ký chưa thành công',
                            bieuTuong: 'nguy-hiem'
                        });
                        break;
                    case 2:
                        moPopup({
                            tieuDe: 'Thông báo',
                            thongBao: 'Lỗi kết nối CSDL hoặc lỗi truy vấn.',
                            bieuTuong: 'nguy-hiem'
                        });
                        break;
                    case 3:
                        moPopup({
                            tieuDe: 'Thông báo',
                            thongBao: data.ketQua,
                            bieuTuong: 'nguy-hiem'
                        });
                        break;
                }
            }).fail(function () {               
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Lỗi ajax',
                    bieuTuong: 'nguy-hiem'
                });
            })
        },
        validates: [{
            input: $('#NhapLaiMatKhau'),
            thongBao: 'Mật khẩu chưa khớp',
            validate: function () {
                if ($('#NhapLaiMatKhau').val() != $('#MatKhau').val()) {
                    return false;
                }
            }            
        }]
    });
}

//Xử lý đăng nhập
function khoiTaoDangNhap($form) {
    khoiTaoLCTForm($form, {
        submit: function (e) {
            $.ajax({
                url: $form.attr('action'),
                method: $form.attr('method'),
                data: $form.serialize(),
                asyne: false
            }).done(function (data) {
                if (data.trangThai == 0) {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: data.ketQua,
                        nut: [{
                            ten: 'Về trang chủ',                            
                            xuLy: function () {
                                window.location = '/TrangChu/';
                            }
                        }],
                        esc: false,
                        bieuTuong: 'thanh-cong'
                    });
                }
                if (data.trangThai == 1) {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: data.ketQua,
                        bieuTuong: 'nguy-hiem'
                    });
                }
                
            }).fail(function () {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Lỗi ajax',                    
                    nut: [{
                        ten: 'Về trang chủ',
                        xuLy: function () {
                            window.location = '/TrangChu/';
                        }
                    }],
                    esc: false,
                    bieuTuong: 'nguy-hiem'
                })
            })
        }
    });
}