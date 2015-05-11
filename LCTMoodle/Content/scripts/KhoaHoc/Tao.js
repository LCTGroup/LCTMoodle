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
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Thêm khóa học thành công'
                });
            }).fail(function () {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Thêm khóa học thất bại'
                });
            });
        }
    })
}