var $_dsNguoiDung;

//#region Khởi tạo

$(function () {
    $_dsNguoiDung = $('[data-doi-tuong="item-nguoi-dung"]');

    khoiTaoQuanLyCauHoi($_dsNguoiDung);
});

//#endregion

//#region Khởi tạo quản lý người dùng

function khoiTaoQuanLyCauHoi($dsNguoiDung) {

    $dsNguoiDung.find('[data-chuc-nang="chan"]').on('click', function () {
        var $nguoiDung = $(this).closest('[data-doi-tuong="item-nguoi-dung"]');
        var $maNguoiDung = $nguoiDung.attr('data-ma');
        var $tai = moBieuTuongTai($nguoiDung);

        $.ajax({
            url: '/NguoiDung/XuLyChanNguoiDung',
            method: 'POST',
            data: { maNguoiDung: $maNguoiDung, trangThai: false }
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai == 0) {
                $nguoiDung.remove();
            }
        })
    });

    $dsNguoiDung.find('[data-chuc-nang="bo-chan"]').on('click', function () {
        var $nguoiDung = $(this).closest('[data-doi-tuong="item-nguoi-dung"]');
        var $maNguoiDung = $nguoiDung.attr('data-ma');
        var $tai = moBieuTuongTai($nguoiDung);

        $.ajax({
            url: '/NguoiDung/XuLyChanNguoiDung',
            method: 'POST',
            data: { maNguoiDung: $maNguoiDung, trangThai: true }
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai == 0) {
                $nguoiDung.remove();
            }
        })
    });

}

//#region