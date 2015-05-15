$(function () {
    khoiTaoXuLyForm();
})

//{ Khởi tạo xử lý form
function khoiTaoXuLyForm() {
    var $form = $('#khung-dang-cau-hoi');

    $form.on('submit', function (e) {
        kiemTraDangCauHoi($form, e);
    })
}

function kiemTraDangCauHoi($form, e) {
    e = e || window.event;

    if ($form.find('.chu-de-cau-hoi')[0].value == 0) {
        alert('Chọn chủ để');
        e.preventDefault();
        return;
    }
    if ($form.find('.title-editor')[0].value == "") {
        alert('Nhập tiêu đề');
        e.preventDefault();
        return;
    }
    if ($form.find('.f-placeholder').length > 0) {
        alert('Nhập nội dung');
        e.preventDefault();
        return;
    }
}
//} Khởi tạo xử lý form