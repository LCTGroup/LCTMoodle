/*
    Khởi tạo
*/
$(function () {
    $form = $('#tra_loi_cau_hoi');

    XuLyTraLoiCauHoi($form);
});

/*
    Xử lý trả lời câu hỏi
*/
function XuLyTraLoiCauHoi($form) {
    khoiTaoLCTForm($form, {
        submit: function () {
            $.ajax({
                url: $form.attr('action'),
                method: $form.attr('method'),
                data: $form.serialize(),
                asyne: false
            }).done(function (data) {
                alert('asd');
            }).fail(function () {
                alert('fail');
            })
        }
    });
}
