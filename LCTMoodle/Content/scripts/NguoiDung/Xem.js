//#region Khởi tạo

$(function () {
    hienThi_Khung()
});

//#endregion

//#region Khung

function hienThi_Khung() {
    var $khung = layKhung_Khung();

    $_KhungHienThi.html($khung);
    $_KhungChua.attr('data-hien-thi', 'khung');
}

function layKhung_Khung() {
    var $khung;

    $.ajax({
        url: '/KhoaHoc/_Khung',
        data: { ma: maKhoaHoc },
        dataType: 'JSON',
        async: false
    }).done(function (data) {
        if (data.trangThai == 0) {
            $khung = $(data.ketQua);
        }
        else {
            moPopup({
                tieuDe: 'Thông báo',
                thongBao: 'Lấy khung thất bại',
                bieuTuong: 'nguy-hiem'
            })
        }
    }).fail(function () {
        moPopup({
            tieuDe: 'Thông báo',
            thongBao: 'Lấy khung thất bại',
            bieuTuong: 'nguy-hiem'
        })
    });

    khoiTaoKhung($khung);

    return $khung;
}

function khoiTaoKhung($khung) {
    khoiTaoNutHienThi($khung.find('[data-chuc-nang="hien-thi"]'));
}

//#endregion