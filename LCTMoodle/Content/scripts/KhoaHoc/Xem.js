//Global: maKhoaHoc
var $khungHienThi, $danhSach;

//#region Khởi tạo

$(function () {
    $khungHienThi = $('#khung_hien_thi');

    hienThi(layQueryString('hienthi').toLowerCase());

    khoiTaoQuayLai();
});

function hienThi(nhom) {
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
        default:
            hienThi_Khung();
            break;
    }
}

function khoiTaoQuayLai() {
    history.replaceState({
        hienThi: layQueryString('hienthi').toLowerCase()
    }, document.title, window.location.href);

    window.onpopstate = function (e) {
        var state = e.state || null;
        if (state === null) {
            window.location = window.location.href;
        }
        else {
            hienThi(state.hienThi);
        }
    }
}

//#endregion

//#region Khung

function hienThi_Khung() {
    var $khung = layKhung_Khung();

    $khungHienThi.html($khung);
    $khungHienThi.attr('data-hien-thi', 'khung');
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
    $khung.find('[data-chuc-nang="hien-thi"]').on('click', function () {
        var nhom = $(this).attr('data-value');
        hienThi(nhom);

        history.pushState({ hienThi: nhom }, '', '?hienthi=' + nhom);
    });
}

//#endregion

//#region Diễn đàn

function hienThi_DienDan() {
    var $khung = layKhung_DienDan();

    $khungHienThi.html($khung);
    $khungHienThi.attr('data-hien-thi', 'dien-dan');
    document.title = 'Diễn đàn - ' + tieuDe;

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
    var $doiTuongAn = $form.find('[data-an]');
    var $doiTuongBatDauBaiViet = $form.find('[data-chuc-nang="bat-dau-tao-bai-viet"]');

    khoiTaoLCTForm($form, {
        khoiTao: function () {
            $doiTuongAn.hide();
        },
        validates: [
            {
                input: $doiTuongBatDauBaiViet,
                customEvent: {
                    focus: function () {
                        $doiTuongAn.show();
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
                    $doiTuongAn.hide();
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
    document.title = 'Bài giảng - ' + tieuDe;

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
    var $doiTuongAn = $form.find('[data-an]');
    var $doiTuongBatDauBaiViet = $form.find('[data-chuc-nang="bat-dau-tao-bai-viet"]');

    khoiTaoLCTForm($form, {
        khoiTao: function () {
            $doiTuongAn.hide();
        },
        validates: [
            {
                input: $doiTuongBatDauBaiViet,
                customEvent: {
                    focus: function () {
                        $doiTuongAn.show();
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
                    $doiTuongAn.hide();
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
    document.title = 'Bài tập - ' + tieuDe;

    $danhSach = $khungHienThi.find('#danh_sach_bai_viet');

    khoiTaoForm_BaiTap($khungHienThi.find('#tao_bai_viet_form'));
    khoiTaoItem_BaiTap($danhSach.find('[data-doi-tuong="muc-bai-viet"]'));
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
    var $doiTuongAn = $form.find('[data-an]');
    var $doiTuongBatDauBaiViet = $form.find('[data-chuc-nang="bat-dau-tao-bai-viet"]');

    khoiTaoLCTForm($form, {
        khoiTao: function () {
            $doiTuongAn.hide();
        },
        validates: [
            {
                input: $doiTuongBatDauBaiViet,
                customEvent: {
                    focus: function () {
                        $doiTuongAn.show();
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
                    $doiTuongAn.hide();
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

function khoiTaoItem_BaiTap($danhSachBaiTap) {
    $danhSachBaiTap.each(function () {
        var $baiTap = $(this);

        var $form = $baiTap.find('[data-doi-tuong="nop-bai-form"]');

        khoiTaoLCTForm($form, {
            validates: [{
                input: $form.find('[data-chuc-nang="thay-doi-input"]'),
                customEvent: {
                    click: function () {
                        var $doiTuong = $(this);

                        if ($doiTuong.attr('data-value') == 'duong-dan') {
                            $form.find('[data-doi-tuong="duong-dan"]').show().find('input').prop('disabled', false);
                            $form.find('[data-doi-tuong="tap-tin"]').hide().find('input').prop('disabled', true);
                            $doiTuong.attr({
                                'data-value': 'tap-tin',
                                'class': 'pe-7s-cloud-upload',
                                'title': 'Nộp bằng tập tin'
                            });
                        }
                        else {
                            $form.find('[data-doi-tuong="tap-tin"]').show().find('input').prop('disabled', false);;
                            $form.find('[data-doi-tuong="duong-dan"]').hide().find('input').prop('disabled', true);;
                            $doiTuong.attr({
                                'data-value': 'duong-dan',
                                'class': 'pe-7s-cloud',
                                'title': 'Nộp bằng đường dẫn'
                            });
                        }
                    }
                }
            }],
            submit: function () {
                $.ajax({
                    url: '/BaiTapNop/XuLyThem',
                    type: 'POST',
                    data: $form.serialize(),
                    dataType: 'JSON'
                }).done(function (data) {
                    if (data.trangThai == 0) {
                        moPopup({
                            tieuDe: 'Thông báo',
                            thongBao: 'Nộp bài thành công',
                            bieuTuong: 'thanh-cong'
                        });
                    }
                    else {
                        moPopup({
                            tieuDe: 'Thông báo',
                            thongBao: 'Nộp bài thất bại',
                            bieuTuong: 'nguy-hiem'
                        });
                    }
                }).fail(function () {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: 'Nộp bài thất bại',
                        bieuTuong: 'nguy-hiem'
                    });
                });
            }
        });
    });
}

//#endregion