//#region Khởi tạo

$(function () {
    //Khởi tạo xử lý đăng ký    
    khoiTaoDangKy($('#dang_ky'));

    //Khởi tạo xử lý đăng nhập
    khoiTaoDangNhap($('#dang_nhap'));
});

//#endregion

//#region Xử lý đăng ký

function khoiTaoDangKy($form) {
    khoiTaoLCTForm($form, {
        submit: function () {
            $.ajax({
                url: $form.attr('action'),
                method: $form.attr('method'),            
                data: layDataLCTForm($form),
                dataType: 'JSON',
                async: false
            }).done(function (data) {
                if (data.trangThai == 0) {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: 'Đăng ký thành công. Vui lòng kiểm tra email để kích hoạt tài khoản',
                        nut: [{
                            ten: 'Về trang chủ',
                            href: '/TrangChu/'
                        }],
                        esc: false,
                        bieuTuong: 'thanh-cong'
                    });
                }
                else {
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
                    bieuTuong: 'nguy-hiem'
                });
            })
        },
        custom: [
            {
                input: $('#NhapLaiMatKhau'),
                thongBao: 'Mật khẩu chưa khớp',
                validate: function () {
                    if ($('#NhapLaiMatKhau').val() != $('#MatKhau').val()) {
                        return false;
                    }
                }            
            },
            {
                input: $('#MatKhauCap2'),
                thongBao: 'Mật khẩu cấp 2 không được trùng với Mật khẩu cấp 1',
                validate: function () {
                    if ($('#MatKhauCap2').val() == $('#MatKhau').val()) {
                        return false;
                    }
                }            
            },
            {
                input: $('#TenTaiKhoan'),
                thongBao: 'Tài khoản đã tồn tại',
                validate: function () {
                    var ketQua;
                    $.ajax({
                        url: '/NguoiDung/KiemTraTenTaiKhoan',
                        data: { tenTaiKhoan: $('#TenTaiKhoan').val() },
                        async: false
                    }).done(function (data) {
                        ketQua = !data;
                    }).fail(function () {
                        moPopup({
                            tieuDe: 'Thông báo',
                            noiDung: 'Lỗi ajax'
                        })
                    });
                    return ketQua;
                }
            }]
    });
}

//#endregion

//#region Xử lý đăng nhập

function khoiTaoDangNhap($form) {
    khoiTaoLCTForm($form, {
        submit: function (e) {
            $.ajax({
                url: $form.attr('action'),
                method: $form.attr('method'),
                data: layDataLCTForm($form)
            }).done(function (data) {
                if (data.trangThai == 0) {
                    window.location = '/TrangChu/';
                }
                else if (data.trangThai == 5)
                {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: data.ketQua,
                        bieuTuong: 'thong-tin'
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

//#endregion