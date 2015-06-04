//Global: maKhoaHoc, tieuDe
var $_KhungHienThi, $_DanhSach, $_KhungChua;

//#region Khởi tạo

$(function () {
    $_KhungHienThi = $('#khung_hien_thi');
    $_KhungChua = $_KhungHienThi.parent();

    hienThi(layQueryString('hienthi').toLowerCase());

    khoiTaoNutHienThi($_KhungChua.find('[data-chuc-nang="hien-thi"]'));
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

function khoiTaoNutHienThi($nut) {
    $nut.on('click', function () {
        var nhom = $(this).attr('data-value');
        hienThi(nhom);

        history.pushState({ hienThi: nhom }, '', '?hienthi=' + nhom);
    });
}

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

//#region Diễn đàn

function hienThi_DienDan() {
    var $khung = layKhung_DienDan();

    $_KhungHienThi.html($khung);
    $_KhungChua.attr('data-hien-thi', 'dien-dan');
    document.title = 'Diễn đàn - ' + tieuDe;

    $_DanhSach = $_KhungHienThi.find('#danh_sach_bai_viet');

    khoiTaoForm_DienDan($_KhungHienThi.find('#tao_bai_viet_form'));
    khoiTaoKhungBinhLuan($_KhungHienThi.find('[data-doi-tuong="khung-binh-luan"]'));
    khoiTaoItem_DienDan($_KhungHienThi.find('[data-doi-tuong="muc-bai-viet"]'));
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

                    $_DanhSach.prepend($htmlBaiViet);

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

function khoiTaoItem_DienDan($danhSachBaiViet) {
    khoiTaoTatMoDoiTuong($danhSachBaiViet.find('[data-chuc-nang="tat-mo"]'));

    $danhSachBaiViet.find('[data-chuc-nang="xoa-bai-viet"]').on('click', function () {
        var $nut = $(this);

        moPopup({
            tieuDe: 'Xác nhận',
            thongBao: 'Bạn có chắc muốn xóa bài viết này?',
            bieuTuong: 'hoi',
            nut: [
                {
                    ten: 'Có',
                    xuLy: function () {
                        $.ajax({
                            url: '/BaiVietDienDan/Xoa/' + $nut.closest('[data-doi-tuong="muc-bai-viet"]').attr('data-value'),
                            type: 'POST',
                            dataType: 'JSON'
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                $nut.closest('[data-doi-tuong="muc-bai-viet"]').remove();
                            }
                            else {
                                moPopup({
                                    tieuDe: 'Thông báo',
                                    thongBao: 'Xóa bài viết thất bại',
                                    bieuTuong: 'nguy-hiem'
                                })
                            }
                        }).fail(function () {
                            moPopup({
                                tieuDe: 'Thông báo',
                                thongBao: 'Xóa bài viết thất bại',
                                bieuTuong: 'nguy-hiem'
                            })
                        });
                    }
                },
                {
                    ten: 'Không',
                }
            ]
        });
    });

    $danhSachBaiViet.find('[data-chuc-nang="sua-bai-viet"]').on('click', function () {
        var $baiViet = $(this).closest('[data-doi-tuong="muc-bai-viet"]');

        $.ajax({
            url: '/BaiVietDienDan/_Form/' + $baiViet.attr('data-ma'),
            dataType: 'JSON'
        }).done(function (data) {
            if (data.trangThai == 0) {
                var $khungNoiDung = $baiViet.find('[data-doi-tuong="khung-noi-dung"]');

                var $form = $(data.ketQua);
                khoiTaoLCTForm($form, {
                    submit: function () {
                        $.ajax()
                    }
                });

                $form.css({
                    border: '1px solid #ddd'
                });

                $khungNoiDung.html($form);
            }
            else {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Sửa bài viết thất bại',
                    bieuTuong: 'nguy-hiem'
                });
            }
        }).fail(function () {
            moPopup({
                tieuDe: 'Thông báo',
                thongBao: 'Sửa bài viết thất bại',
                bieuTuong: 'nguy-hiem'
            });
        });
    });
}

//#endregion

//#region Bài giảng

function hienThi_BaiGiang() {
    var $khung = layKhung_BaiGiang();

    $_KhungHienThi.html($khung);
    $_KhungChua.attr('data-hien-thi', 'bai-giang');
    document.title = 'Bài giảng - ' + tieuDe;

    $_DanhSach = $_KhungHienThi.find('#danh_sach_bai_viet');

    khoiTaoForm_BaiGiang($_KhungHienThi.find('#tao_bai_viet_form'));
    khoiTaoItem_BaiGiang($_KhungHienThi.find('[data-doi-tuong="muc-bai-viet"]'));
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
                    $_DanhSach.append($mucBaiViet);
                    moItem_BaiGiang($mucBaiViet);

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
    $danhSachBaiGiang.find('.tieu-de').on('click', function () {
        moItem_BaiGiang($(this).parent());
    });

    khoiTaoTatMoDoiTuong($danhSachBaiGiang.find('[data-chuc-nang="tat-mo"]'));

    $danhSachBaiGiang.find('[data-chuc-nang="xoa-bai-viet"]').on('click', function () {
        var $nut = $(this);

        moPopup({
            tieuDe: 'Xác nhận',
            thongBao: 'Bạn có chắc muốn xóa bài viết này?',
            bieuTuong: 'hoi',
            nut: [
                {
                    ten: 'Có',
                    xuLy: function () {
                        $.ajax({
                            url: '/BaiVietBaiGiang/Xoa/' + $nut.attr('data-value'),
                            type: 'POST',
                            dataType: 'JSON'
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                $nut.closest('[data-doi-tuong="muc-bai-viet"]').remove();
                            }
                            else {
                                moPopup({
                                    tieuDe: 'Thông báo',
                                    thongBao: 'Xóa bài viết thất bại',
                                    bieuTuong: 'nguy-hiem'
                                })
                            }
                        }).fail(function () {
                            moPopup({
                                tieuDe: 'Thông báo',
                                thongBao: 'Xóa bài viết thất bại',
                                bieuTuong: 'nguy-hiem'
                            })
                        });
                    }
                },
                {
                    ten: 'Không',
                }
            ]
        });
    })
}

function moItem_BaiGiang($baiGiang) {
    if ($baiGiang.hasClass('mo')) {
        $baiGiang.removeClass('mo');
    }
    else {
        $_DanhSach.find('.mo[data-doi-tuong="muc-bai-viet"]').removeClass('mo');
        $baiGiang.addClass('mo');
        $body.animate({
            scrollTop: $baiGiang.offset().top
        }, 200);
    }
}

//#endregion

//#region Bài tập

function hienThi_BaiTap() {
    var $khung = layKhung_BaiTap();

    $_KhungHienThi.html($khung);
    $_KhungChua.attr('data-hien-thi', 'bai-tap');
    document.title = 'Bài tập - ' + tieuDe;

    $_DanhSach = $_KhungHienThi.find('#danh_sach_bai_viet');

    khoiTaoForm_BaiTap($_KhungHienThi.find('#tao_bai_viet_form'));
    khoiTaoItem_BaiTap($_DanhSach.find('[data-doi-tuong="muc-bai-viet"]'));
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
            thongBao: 'Lấy bài tập thất bại',
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

                    khoiTaoItem_BaiTap($htmlBaiViet);
                    $_DanhSach.prepend($htmlBaiViet);

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
                        var $hienThi = $form.prev().children();
                        
                        switch($hienThi.attr('data-type')) {
                            case '1':
                                $hienThi.html('<span><b>Bạn</b></span> <span>đã nộp</span>').attr('data-type', '2');
                                break;
                            case '3':
                                $hienThi.html('<span><b>Bạn</b></span>' + $hienThi.html()).attr('data-type', '4');
                                break;
                            default:
                                break;
                        }

                        $nut = $form.find('button');
                        $nut.text('Cập nhật');
                        $nut.parent().addClass('nop-lai')
                        khoiTaoLCTFormMacDinh($form);
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

    khoiTaoTatMoDoiTuong($danhSachBaiTap.find('[data-chuc-nang="tat-mo"]'));

    $danhSachBaiTap.find('[data-chuc-nang="xoa-bai-viet"]').on('click', function () {
        var $nut = $(this);

        moPopup({
            tieuDe: 'Xác nhận',
            thongBao: 'Bạn có chắc muốn xóa bài viết này?',
            bieuTuong: 'hoi',
            nut: [
                {
                    ten: 'Có',
                    xuLy: function () {
                        $.ajax({
                            url: '/BaiVietBaiTap/Xoa/' + $nut.attr('data-value'),
                            type: 'POST',
                            dataType: 'JSON'
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                $nut.closest('[data-doi-tuong="muc-bai-viet"]').remove();
                            }
                            else {
                                moPopup({
                                    tieuDe: 'Thông báo',
                                    thongBao: 'Xóa bài viết thất bại',
                                    bieuTuong: 'nguy-hiem'
                                })
                            }
                        }).fail(function () {
                            moPopup({
                                tieuDe: 'Thông báo',
                                thongBao: 'Xóa bài viết thất bại',
                                bieuTuong: 'nguy-hiem'
                            })
                        });
                    }
                },
                {
                    ten: 'Không',
                }
            ]
        });
    });

    $danhSachBaiTap.find('[data-chuc-nang="xem-danh-sach-nop"]').on('click', function () {
        moPopupFull({
            url: '/BaiTapNop/_DanhSachNop',
            data: { maBaiTap: $(this).closest('[data-doi-tuong="muc-bai-viet"]').attr('data-ma') },
            width: '400px'
        });
    })
}

//#endregion