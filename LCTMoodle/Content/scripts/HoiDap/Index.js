var $_danhSach;
var $_khungTim;
var $_cachHienThi = "MoiNhat";

//#region Khởi tạo

$(function () {
    $_danhSach = $('#danh_sach');
    $_khungTim = $('#khung_tim');

    khoiTaoLCTForm($('#tieu_chi_hien_thi'));

    capNhatCachHienThi();

    khoiTaoKhungTimKiemLCT($_danhSach, $_khungTim, '/HoiDap/_DanhSach_Tim', {
        data: function () {
            return { cachHienThi: $_cachHienThi };
        }
    })
});

//#endregion

//#region Cập nhật cách hiển thị

function capNhatCachHienThi() {
    $doiTuongRadio = $('input[name="cachHienThi"]:radio');
    
    $doiTuongRadio.on('click', function () {
        $_cachHienThi = $(this).val();
    });
}

//#endregion