﻿var $_khungChonHienThi;
var $_khungHienThi;

//#region Khởi tạo

$(function () {
    $_khungChonHienThi = $('#khung_chon_hien_thi');
    $_khungHienThi = $('#khung_hien_thi');

    khoiTaoKhungChonHienThi($_khungChonHienThi);
});

//#endregion

//#region Khung

function khoiTaoKhungChonHienThi($khungChonHienThi)
{
    $khungChonHienThi.find('[data-chuc-nang="hien-thi"]').on('click', function (e) {
        e = e || window.event;

        var giaTriHienThi = $(this).attr('data-value');

        hienThiNutTab(this, giaTriHienThi);

        hienThiNoiDung(giaTriHienThi);
    });
}

//#region Hiển thị nút tab

function hienThiNutTab(obj, objValue) {
    $_khungChonHienThi.find('[data-chuc-nang="hien-thi"]').removeClass('clicked');
    $(obj).addClass('clicked');

    $_khungChonHienThi.find('li').removeClass('active');
    switch(objValue)
    {
        case "nhiemvumoi":
            $_khungChonHienThi.find('.nhiem-vu-moi').addClass('active');
            break;
        case "baigiangmoi":
            $_khungChonHienThi.find('.bai-giang-moi').addClass('active');
            break;
        case "baitapmoi":
            $_khungChonHienThi.find('.bai-tap-moi').addClass('active');
            break;
    }
}

//#endregion

//#region Hiển thị nội dung tab

function hienThiNoiDung(objValue) {
    $_khungHienThi.find('.noi-dung').removeClass('active');

    $('#' + objValue).addClass('active');
}

//#endregion

//#endregion