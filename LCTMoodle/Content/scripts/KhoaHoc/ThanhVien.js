var
    //Mã khóa học hiện tại
    maKhoaHoc;

$(function () {

});

//#region Khởi tạo item

function khoiTaoItem_DangKy($items) {
    $items.find('[data-chuc-nang="dong-y"]').on('click', function () {
        var $item = $(this).closest('.item');

    });

    $items.find('[data-chuc-nang="tu-choi"]').on('click', function () {
        var $item = $(this).closest('.item');

    });

    $items.find('[data-chuc-nang="chan"]').on('click', function () {
        var $item = $(this).closest('.item');

    });
}

//#endregion