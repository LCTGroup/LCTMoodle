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
                    //window.location = '/KhoaHoc/' + data.ketQua.ma;
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