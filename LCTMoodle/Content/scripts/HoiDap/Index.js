var $_danhSach;
var $_khungTim;
var _cachHienThi = "MoiNhat";

//#region Khởi tạo

$(function () {
    $_danhSach = $('#danh_sach');
    $_khungTim = $('#khung_tim');

    khoiTaoLCTForm($('#tieu_chi_hien_thi'));

    khoiTaoKhungTimKiemLCT($_danhSach, $_khungTim, '/HoiDap/_DanhSach_Tim', {
        data: function () {
            return { cachHienThi: _cachHienThi };
        }
    });
    
    $('[data-doi-tuong="tieu-chi"]').on('change', function () {
        _cachHienThi = this.value;
        $_khungTim.tim();
    });

});

//#endregion