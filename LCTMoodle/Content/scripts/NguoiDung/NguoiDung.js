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
            var $tai = moBieuTuongTai($form);
            $.ajax({
                url: $form.attr('action'),
                method: $form.attr('method'),            
                data: layDataLCTForm($form),
                dataType: 'JSON'
            }).always(function () {
                $tai.tat();
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
                input: $('#nhap_lai_mat_khau'),
                thongBao: 'Mật khẩu chưa khớp',
                validate: function () {
                    if ($('#NhapLaiMatKhau').val() != $('#MatKhau').val()) {
                        return false;
                    }
                }            
            },
            {
                input: $('#ten_tai_khoan'),
                thongBao: 'Tài khoản đã tồn tại',
                validate: function () {
                    var ketQua;
                    $.ajax({
                        url: '/NguoiDung/KiemTraTenTaiKhoan',
                        data: { tenTaiKhoan: $('#TenTaiKhoan').val() }
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
            },
            {
                input: $('#email'),
                thongBao: 'Email đã tồn tại',
                validate: function () {
                    var ketQua;
                    $.ajax({
                        url: '/NguoiDung/KiemTraEmail',
                        data: { email: $('#email').val() }
                    }).done(function (data) {
                        ketQua = !data;
                    }).fail(function () {
                        moPopupThongBao(data)
                    });
                    return ketQua;
                }
            },
            {
                input: $('#ngay_sinh'),
                thongBao: 'Bạn chưa đủ tuổi để tham gia LCTMoodle',
                validate: function () {
                    var $ngaySinh = $('#ngay_sinh').val();
                    var $giaTriNgaySinh = $ngaySinh.split('/');
                    var $namHienTai = new Date().getFullYear();
                   
                    if (($namHienTai - $giaTriNgaySinh[2] >= 13) || $('#ngay_sinh').val() == '')
                    {
                        return true;
                    }
                    return false;
                }
            }]
    });

    //#region khởi tạo nút điều khoản

        $('#dieu_khoan').on('click', function () {
            moPopupFull({
                url: '/NguoiDung/_DieuKhoan'
            });
        })

    //#endregion

    //#region Khởi tạo nút reset

        $('[data-chuc-nang="reset"]').on('click', function () {
            khoiTaoLCTFormMacDinh($form);
        });

    //#endregion
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