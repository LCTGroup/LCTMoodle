$(function () {
    var $form = $('#tao_khoa_hoc_form');

    khoiTaoSubmit($form);
});

function khoiTaoSubmit($form) {
    khoiTaoLCTForm($form, {
        submit: function () {
            $.ajax({
                url: '/KhoaHoc/XuLyThem',
                type: 'POST',
                data: $form.serialize(),
                dataType: 'JSON'
            }).done(function (data) {
                if (data.trangThai == 0) {
                    moPopup({
                        tieuDe: 'Xác nhận',
                        thongBao: 'Tạo khóa học thành công.<br />Bạn có muốn thực hiện việc gì tiếp theo?',
                        nut: [{
                            ten: 'Vào khóa học',
                            xuLy: function () {
                                window.location = '/KhoaHoc/' + data.ketQua;
                            }
                        }, {
                            ten: 'Tạo chương trình',
                            xuLy: function () {
                                window.location = '/KhoaHoc/ChuongTrinh/' + data.ketQua;
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
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: 'Thêm khóa học thất bại',
                        bieuTuong: 'nguy-hiem'
                    });
                }
            }).fail(function () {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Thêm khóa học thất bại',
                    bieuTuong: 'nguy-hiem'
                });
            });
        }
    })
}