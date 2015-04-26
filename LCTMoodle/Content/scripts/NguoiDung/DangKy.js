/*
    Khởi tạo
*/
$(function () {
    khoiTaoNguoiDung($('#TaoNguoiDung'));
});

function khoiTaoNguoiDung($formObject) {
    $formObject.submit(function (e) {
        e.preventDefault();        

        $.ajax({
            url: $formObject.attr('action'),
            type: $formObject.attr('method'),
            dataType: 'JSON',
            data: $formObject.serialize(),
        }).done(function (data) {            
            alert('Người dùng ' + data.ketQua + ' đã được tạo.');
        });
    });
}