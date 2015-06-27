var $_danhSach;
var $_khungTim;

//#region Khởi tạo

$(function () {
    $_danhSach = $('#danh_sach');
    $_khungTim = $('#khung_tim');

    khoiTaoKhungTimKiemLCT($_danhSach, $_khungTim, '/HoiDap/_DanhSach_Tim')
});

//#endregion