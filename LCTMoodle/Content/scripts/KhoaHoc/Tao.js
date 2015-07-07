$(function () {
    var $form = $('#tao_khoa_hoc_form');

    khoiTaoSubmit($form);
});

function khoiTaoSubmit($form) {
    if ($form.is('[data-cap-nhat]')) {
        khoiTaoLCTForm($form, {
            submit: function () {
                $.ajax({
                    url: '/KhoaHoc/XuLyCapNhat',
                    type: 'POST',
                    data: layDataLCTForm($form),
                    dataType: 'JSON'
                }).done(function (data) {
                    if (data.trangThai == 0) {
                        window.location = '/KhoaHoc/' + data.ketQua.ma;
                    }
                    else {
                        moPopupThongBao(data);
                    }
                }).fail(function () {
                    moPopupThongBao('Sửa khóa học thất bại');
                });
            }
        });
    }
    else {
        khoiTaoLCTForm($form, {
            submit: function () {
                $.ajax({
                    url: '/KhoaHoc/XuLyThem',
                    type: 'POST',
                    data: layDataLCTForm($form),
                    dataType: 'JSON'
                }).done(function (data) {
                    if (data.trangThai == 0) {
                        moPopup({
                            tieuDe: 'Xác nhận',
                            thongBao: 'Tạo khóa học thành công.<br />Bạn có muốn thực hiện việc gì tiếp theo?',
                            nut: [{
                                ten: 'Vào khóa học',
                                xuLy: function () {
                                    window.location = '/KhoaHoc/' + data.ketQua.ma;
                                }
                            }, {
                                ten: 'Tạo chương trình',
                                xuLy: function () {
                                    window.location = '/KhoaHoc/ChuongTrinh/' + data.ketQua.ma;
                                }
                            }, {
                                ten: 'Tiếp tục tạo'
                            }],
                            tat: function () {
                                khoiTaoLCTFormMacDinh($form);
                            }
                        });
                    }
                    else {
                        moPopupThongBao(data);
                    }
                }).fail(function () {
                    moPopupThongBao('Tạo khóa học thất bại');
                });
            }
        });
    }
}