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
        case 'baitap':
            hienThi_BaiTap();
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
                    $doiTuongTaoBaiViet.hide();
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

    $danhSach = $khungHienThi.find('#danh_sach_bai_viet');

    khoiTaoForm_BaiGiang($khungHienThi.find('#tao_bai_viet_form'));
    khoiTaoItem_BaiGiang($khungHienThi.find('[data-doi-tuong="muc-bai-viet"]'));
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
                    var $mucBaiViet = $(data.ketQua);

                    khoiTaoItem_BaiGiang($mucBaiViet);
                    $danhSach.append($mucBaiViet);

                    khoiTaoLCTFormMacDinh($form);
                    $doiTuongTaoBaiViet.hide();
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

function khoiTaoItem_BaiGiang($danhSachBaiGiang) {
    $danhSachBaiGiang.each(function () {
        var $baiGiang = $(this);

        $baiGiang.find('.tieu-de').on('click', function () {
            if ($baiGiang.hasClass('mo')) {
                $baiGiang.removeClass('mo');
            }
            else {
                $danhSach.find('.mo[data-doi-tuong="muc-bai-viet"]').removeClass('mo');
                $baiGiang.addClass('mo');
            }
        })
    });
}

//#endregion

//#region Bài tập

function hienThi_BaiTap() {
    var $khung = layKhung_BaiTap();

    $khungHienThi.html($khung);
    $khungHienThi.attr('data-hien-thi', 'bai-tap');

    $danhSach = $khungHienThi.find('#danh_sach_bai_viet');

    khoiTaoForm_BaiTap($khungHienThi.find('#tao_bai_viet_form'));
}

function layKhung_BaiTap() {
    var $khung;

    $.ajax({
        url: '/BaiVietBaiTap/_Khung',
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
                thongBao: 'Lấy bài tập thất bại',
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

function khoiTaoForm_BaiTap($form) {
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
                url: '/BaiVietBaiTap/XuLyThem',
                type: 'POST',
                data: $form.serialize(),
                dataType: 'JSON',
                processData: false
            }).done(function (data) {
                if (data.trangThai == 0) {
                    var $htmlBaiViet = $(data.ketQua);

                    $danhSach.prepend($htmlBaiViet);

                    khoiTaoLCTFormMacDinh($form);
                    $doiTuongTaoBaiViet.hide();
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