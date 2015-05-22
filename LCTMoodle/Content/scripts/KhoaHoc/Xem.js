//Global: maKhoaHoc
var $khungHienThi, $danhSach;

//#region Khởi tạo

$(function () {
    $khungHienThi = $('#khung_hien_thi');

    hienThi();
});

function hienThi() {
    var nhom = layQueryString('hienthi').toLowerCase();

    switch (nhom) {
        case 'diendan':
            hienThi_DienDan();
            break;
        case 'baigiang':
            hienThi_BaiGiang();
            break;
    }
}

//#endregion

//#region Diễn đàn

function hienThi_DienDan() {
    var $khung = layKhung_DienDan();

    $khungHienThi.html($khung);
    $khungHienThi.attr('data-hien-thi', 'dien-dan');

    $danhSach = $khungHienThi.find('#danh_sach_bai_viet');

    khoiTaoForm_DienDan($khungHienThi.find('#tao_bai_viet_form'));
    khoiTaoKhungBinhLuan($khungHienThi.find('[data-doi-tuong="khung-binh-luan"]'));
}

function layKhung_DienDan() {
    var $khung;

    $.ajax({
        url: '/BaiVietDienDan/_Khung',
        data: { maKhoaHoc: maKhoaHoc },
        dataType: 'JSON',
        async: false
    }).done(function (data) {
        if (data.trangThai == 0) {
            $khung = $(data.ketQua);
        }
        else {
            moPopup({
                tieuDe: 'Thông báo',
                thongBao: 'Lấy diễn đàn thất bại',
                bieuTuong: 'nguy-hiem'
            })
        }
    }).fail(function () {
        moPopup({
            tieuDe: 'Thông báo',
            thongBao: 'Lấy diễn đàn thất bại',
            bieuTuong: 'nguy-hiem'
        })
    });

    return $khung;
}

function khoiTaoForm_DienDan($form) {
    var $doiTuongTaoBaiViet = $form.find('[data-doi-tuong="tao-bai-viet"]');
    var $doiTuongBatDauBaiViet = $form.find('[data-chuc-nang="bat-dau-tao-bai-viet"]');

    khoiTaoLCTForm($form, {
        khoiTao: function () {
            $doiTuongTaoBaiViet.hide();
        },
        validates: [
            {
                input: $doiTuongBatDauBaiViet,
                customEvent: {
                    focus: function () {
                        $doiTuongTaoBaiViet.show();
                    }
                }
            }
        ],
        submit: function () {
            $.ajax({
                url: '/BaiVietDienDan/XuLyThem',
                type: 'POST',
                data: $form.serialize(),
                dataType: 'JSON',
                processData: false
            }).done(function (data) {
                if (data.trangThai == 0) {
                    var $htmlBaiViet = $(data.ketQua);

                    khoiTaoKhungBinhLuan($htmlBaiViet.find('[data-doi-tuong="khung-binh-luan"]'));

                    $danhSach.prepend($htmlBaiViet);

                    khoiTaoLCTFormMacDinh($form);
                }
                else {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: 'Thêm bài viết thất bại'
                    });
                }
            }).fail(function () {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Thêm bài viết thất bại'
                });
            });
        }
    });
}

//#endregion

//#region Bài giảng

function hienThi_BaiGiang() {
    var $khung = layKhung_BaiGiang();

    $khungHienThi.html($khung);
    $khungHienThi.attr('data-hien-thi', 'bai-giang');

    khoiTaoForm_BaiGiang($khungHienThi.find('#tao_bai_viet_form'));
}

function layKhung_BaiGiang() {
    var $khung;

    $.ajax({
        url: '/BaiVietBaiGiang/_Khung',
        data: { maKhoaHoc: maKhoaHoc },
        dataType: 'JSON',
        async: false
    }).done(function (data) {
        if (data.trangThai == 0) {
            $khung = $(data.ketQua);
        }
        else {
            moPopup({
                tieuDe: 'Thông báo',
                thongBao: 'Lấy bài giảng thất bại',
                bieuTuong: 'nguy-hiem'
            })
        }
    }).fail(function () {
        moPopup({
            tieuDe: 'Thông báo',
            thongBao: 'Lấy bài giảng thất bại',
            bieuTuong: 'nguy-hiem'
        })
    });

    return $khung;
}

function khoiTaoForm_BaiGiang($form) {
    var $doiTuongTaoBaiViet = $form.find('[data-doi-tuong="tao-bai-viet"]');
    var $doiTuongBatDauBaiViet = $form.find('[data-chuc-nang="bat-dau-tao-bai-viet"]');

    khoiTaoLCTForm($form, {
        khoiTao: function () {
            $doiTuongTaoBaiViet.hide();
        },
        validates: [
            {
                input: $doiTuongBatDauBaiViet,
                customEvent: {
                    focus: function () {
                        $doiTuongTaoBaiViet.show();
                    }
                }
            }
        ],
        submit: function () {
            $.ajax({
                url: '/BaiVietBaiGiang/XuLyThem',
                type: 'POST',
                data: $form.serialize(),
                dataType: 'JSON',
                processData: false
            }).done(function (data) {
                if (data.trangThai == 0) {
                    khoiTaoLCTFormMacDinh($form);
                }
                else {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: 'Thêm bài viết thất bại'
                    });
                }
            }).fail(function () {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Thêm bài viết thất bại'
                });
            });
        }
    });
}

//#endregion