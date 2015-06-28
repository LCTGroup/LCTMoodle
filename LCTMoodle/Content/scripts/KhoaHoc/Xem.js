var
    //Mã khóa học hiện tại
    maKhoaHoc,
    //Tiêu đề của khóa học (Lưu để lúc ajax hiển thị)
    tieuDe,
    //Khung hiển thị nội dung khóa học
    $_KhungHienThi,
    //Khung danh sách bài viết
    $_DanhSach,
    //Khung chưa toàn bộ
    $_KhungChua;

//#region Khởi tạo

$(function () {
    $_KhungHienThi = $('#khung_hien_thi');
    $_KhungChua = $_KhungHienThi.parent();

    hienThi(layQueryString('hienthi').toLowerCase());

    khoiTaoNutHienThi($_KhungChua.find('[data-chuc-nang="hien-thi"]'));
    khoiTaoQuayLai();

    //Khởi tạo nút
    khoiTaoNutDangKy($('[data-chuc-nang="dang-ky"]'));
    khoiTaoNutThamGia($('[data-chuc-nang="tham-gia"]'));
    khoiTaoNutHuyDangKy($('[data-chuc-nang="huy-dang-ky"]'));
    khoiTaoNutRoiKhoaHoc($('[data-chuc-nang="roi-kh"]'));
});

function hienThi(nhom) {
    switch (nhom) {
        case 'diendan':
            hienThi_DienDan();
            break;
        case 'baigiang':
            hienThi_BaiGiang();
            break;
        case 'tailieu':
            hienThi_TaiLieu();
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

//#region Chức năng

function khoiTaoNutDangKy($nuts) {
    $nuts.on('click', function () {
        var $nut = $(this);
        
        var $tai = moBieuTuongTai($nut);
        $.ajax({
            url: '/KhoaHoc/XuLyDangKyThamGia/' + maKhoaHoc,
            method: 'POST',
            dataType: 'JSON'
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai == 0) {
                //Thay đổi nút
                $nut.text('Hủy đăng ký');
                $nut.off('click');
                khoiTaoNutHuyDangKy($nut);
            }
            else {
                moPopupThongBao(data);
            }
        }).fail(function () {
            moPopupThongBao('Đăng ký vào khóa học thất bại');
        });
    })
}

function khoiTaoNutThamGia($nuts) {
    $nuts.on('click', function (e) {
        var $tai = moBieuTuongTai($(this));
        $.ajax({
            url: '/KhoaHoc/XuLyDangKyThamGia/' + maKhoaHoc,
            method: 'POST',
            dataType: 'JSON'
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai == 0) {
                location.reload();
            }
            else {
                moPopupThongBao(data);
            }
        }).fail(function () {
            moPopupThongBao('Tham gia khóa học thất bại');
        });
    });
}

function khoiTaoNutRoiKhoaHoc($nuts) {
    $nuts.on('click', function (e) {
        var $nut = $(this);
        var $tai = moBieuTuongTai($nut);
        $.ajax({
            url: '/KhoaHoc/XuLyRoiKhoaHoc/' + maKhoaHoc,
            method: 'POST',
            dataType: 'JSON'
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai == 0) {
                location.reload();
            }
            else {
                moPopupThongBao(data);
            }
        }).fail(function () {
            moPopupThongBao('Rời khóa học thất bại');
        });
    })
}

function khoiTaoNutHuyDangKy($nuts) {
    $nuts.on('click', function (e) {
        var $nut = $(this);
        var $tai = moBieuTuongTai($nut);
        $.ajax({
            url: '/KhoaHoc/XuLyHuyDangKy/' + maKhoaHoc,
            method: 'POST',
            dataType: 'JSON'
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai == 0) {
                //Thay đổi nút
                $nut.text('Đăng ký');
                $nut.off('click');
                khoiTaoNutDangKy($nut);
            }
            else {
                moPopupThongBao(data);
            }
        }).fail(function () {
            moPopupThongBao('Hủy đăng ký thất bại');
        });
    })
}

function khoiTaoNutXacNhanMoi() {

}

function khoiTaoNutTuChoiMoi() {

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
            moPopupThongBao(data);
        }
    }).fail(function () {
        moPopupThongBao('Lấy khung thất bại');
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
        custom: [
            {
                input: $doiTuongBatDauBaiViet,
                event: {
                    focus: function () {
                        $doiTuongAn.show();
                    }
                }
            }
        ],
        submit: function () {
            var $tai = moBieuTuongTai($form);
            $.ajax({
                url: '/BaiVietDienDan/XuLyThem',
                type: 'POST',
                data: layDataLCTForm($form),
                dataType: 'JSON'
            }).always(function () {
                $tai.tat();
            }).done(function (data) {
                if (data.trangThai == 0) {
                    var $item = $(data.ketQua);

                    khoiTaoItem_DienDan($item)
                    $_DanhSach.prepend($item);

                    khoiTaoLCTFormMacDinh($form);
                    $doiTuongAn.hide();
                }
                else {
                    moPopupThongBao(data);
                }
            }).fail(function () {
                moPopupThongBao('Thêm bài viết thất bại');
            });
        }
    });
}

function khoiTaoItem_DienDan($danhSachBaiViet) {
    khoiTaoTatMoDoiTuong($danhSachBaiViet.find('[data-chuc-nang="tat-mo"]'), true);

    $danhSachBaiViet.find('[data-chuc-nang="xoa-bai-viet"]').on('click', function () {
        var $baiViet = $(this).closest('[data-doi-tuong="muc-bai-viet"]');

        moPopup({
            tieuDe: 'Xác nhận',
            thongBao: 'Bạn có chắc muốn xóa bài viết này?',
            bieuTuong: 'hoi',
            nut: [
                {
                    ten: 'Có',
                    xuLy: function () {
                        var $tai = moBieuTuongTai($baiViet);
                        $.ajax({
                            url: '/BaiVietDienDan/XuLyXoa/' + $baiViet.attr('data-ma'),
                            type: 'POST',
                            dataType: 'JSON'
                        }).always(function () {
                            $tai.tat();
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                $baiViet.remove();
                            }
                            else {
                                moPopupThongBao(data);
                            }
                        }).fail(function () {
                            moPopupThongBao('Xóa bài viết thất bại');
                        });
                    }
                },
                {
                    ten: 'Không'
                }
            ]
        });
    });

    $danhSachBaiViet.find('[data-chuc-nang="sua-bai-viet"]').on('click', function () {
        var $baiViet = $(this).closest('[data-doi-tuong="muc-bai-viet"]');

        var $tai = moBieuTuongTai($baiViet);
        $.ajax({
            url: '/BaiVietDienDan/_Form/' + $baiViet.attr('data-ma'),
            dataType: 'JSON'
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai == 0) {
                var $form = $(data.ketQua);
                khoiTaoLCTForm($form, {
                    submit: function () {
                        var $tai = moBieuTuongTai($form);
                        $.ajax({
                            url: '/BaiVietDienDan/XuLyCapNhat',
                            type: 'POST',
                            data: layDataLCTForm($form),
                            dataType: 'JSON'
                        }).always(function () {
                            $tai.tat();
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                var $item = $(data.ketQua);
                                khoiTaoItem_DienDan($item);

                                $baiViet.replaceWith($item);
                            }
                            else {
                                moPopupThongBao(data);
                            }
                        }).fail(function () {
                            moPopupThongBao('Cập nhật thất bại');
                        });
                    }
                });

                $form.css({
                    border: '1px solid #ddd'
                });

                $baiViet.html($form);
            }
            else {
                moPopupThongBao(data);
            }
        }).fail(function () {
            moPopupThongBao('Sửa bài viết thất bại');
        });
    });

    $danhSachBaiViet.find('[data-chuc-nang="ghim-bai-viet"]').on('click', function () {
        var $nut = $(this),
            $item = $nut.closest('[data-doi-tuong="muc-bai-viet"]'),
            ghim = !$item.is('[data-ghim]');

        var $tai = moBieuTuongTai($item);
        $.ajax({
            url: '/BaiVietDienDan/XuLyGhim/' + $item.attr('data-ma'),
            method: 'POST',
            data: { ghim: ghim},
            dataType: 'JSON'
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai == 0) {
                if (ghim) {
                    $_DanhSach.find('[data-ghim]').removeAttr('data-ghim');
                    $item.attr('data-ghim', '');
                    $nut.text('Bỏ ghim bài viết');
                }
                else {
                    $item.removeAttr('data-ghim');
                    $nut.text('Ghim bài viết');
                }
            }
            else {
                moPopupThongBao(data);
            }
        }).fail(function () {
            moPopupThongBao('Ghim thất bại');
        })
    })

    khoiTaoKhungBinhLuan($danhSachBaiViet.find('[data-doi-tuong="khung-binh-luan"]'));
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
        custom: [
            {
                input: $doiTuongBatDauBaiViet,
                event: {
                    focus: function () {
                        $doiTuongAn.show();
                    }
                }
            }
        ],
        submit: function () {
            var $tai = moBieuTuongTai($form);
            $.ajax({
                url: '/BaiVietBaiGiang/XuLyThem',
                type: 'POST',
                data: layDataLCTForm($form),
                dataType: 'JSON'
            }).always(function () {
                $tai.tat();
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
                    moPopupThongBao(data);
                }
            }).fail(function () {
                moPopupThongBao('Thêm bài giảng thất bại');
            });
        }
    });
}

function khoiTaoItem_BaiGiang($danhSachBaiGiang) {
    $danhSachBaiGiang.find('.tieu-de').on('click', function () {
        moItem_BaiGiang($(this).parent());
    });

    khoiTaoTatMoDoiTuong($danhSachBaiGiang.find('[data-chuc-nang="tat-mo"]'), true);

    $danhSachBaiGiang.find('[data-chuc-nang="xoa-bai-viet"]').on('click', function () {
        var $baiViet = $(this).closest('[data-doi-tuong="muc-bai-viet"]');

        moPopup({
            tieuDe: 'Xác nhận',
            thongBao: 'Bạn có chắc muốn xóa bài viết này?',
            bieuTuong: 'hoi',
            nut: [
                {
                    ten: 'Có',
                    xuLy: function () {
                        var $tai = moBieuTuongTai($baiViet);
                        $.ajax({
                            url: '/BaiVietBaiGiang/XuLyXoa/' + $baiViet.attr('data-ma'),
                            type: 'POST',
                            dataType: 'JSON'
                        }).always(function () {
                            $tai.tat();
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                $baiViet.remove();
                            }
                            else {
                                moPopupThongBao(data);
                            }
                        }).fail(function () {
                            moPopupThongBao('Xóa bài viết thất bại');
                        });
                    }
                },
                {
                    ten: 'Không',
                }
            ]
        });
    });

    $danhSachBaiGiang.find('[data-chuc-nang="sua-bai-viet"]').on('click', function () {
        var $baiViet = $(this).closest('[data-doi-tuong="muc-bai-viet"]');

        var $tai = moBieuTuongTai($baiViet);
        $.ajax({
            url: '/BaiVietBaiGiang/_Form/' + $baiViet.attr('data-ma'),
            dataType: 'JSON'
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai == 0) {
                var $form = $(data.ketQua);
                khoiTaoLCTForm($form, {
                    submit: function () {
                        var $tai = moBieuTuongTai($form);
                        $.ajax({
                            url: '/BaiVietBaiGiang/XuLyCapNhat',
                            type: 'POST',
                            data: layDataLCTForm($form),
                            dataType: 'JSON'
                        }).always(function () {
                            $tai.tat();
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                var $item = $(data.ketQua);
                                khoiTaoItem_BaiGiang($item);

                                $baiViet.replaceWith($item);
                            }
                            else {
                                moPopupThongBao(data);
                            }
                        }).fail(function () {
                            moPopupThongBao('Cập nhật thất bại');
                        });
                    }
                });

                $form.css({
                    border: '1px solid #ddd'
                });

                $baiViet.html($form);
            }
            else {
                moPopupThongBao(data);
            }
        }).fail(function () {
            moPopupThongBao('Sửa bài viết thất bại');
        });
    });
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

//#region Tài liệu

function hienThi_TaiLieu() {
    var $khung = layKhung_TaiLieu();

    $_KhungHienThi.html($khung);
    $_KhungChua.attr('data-hien-thi', 'tai-lieu');
    document.title = 'Bài giảng - ' + tieuDe;

    $_DanhSach = $_KhungHienThi.find('#danh_sach_bai_viet');

    khoiTaoForm_TaiLieu($_KhungHienThi.find('#tao_bai_viet_form'));
    khoiTaoItem_TaiLieu($_KhungHienThi.find('[data-doi-tuong="muc-bai-viet"]'));
}

function layKhung_TaiLieu() {
    var $khung;

    $.ajax({
        url: '/BaiVietTaiLieu/_Khung',
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

function khoiTaoForm_TaiLieu($form) {
    var $doiTuongAn = $form.find('[data-an]');
    var $doiTuongBatDauBaiViet = $form.find('[data-chuc-nang="bat-dau-tao-bai-viet"]');

    khoiTaoLCTForm($form, {
        khoiTao: function () {
            $doiTuongAn.hide();
        },
        custom: [
            {
                input: $doiTuongBatDauBaiViet,
                event: {
                    focus: function () {
                        $doiTuongAn.show();
                    }
                }
            }
        ],
        submit: function () {
            var $tai = moBieuTuongTai($form);
            $.ajax({
                url: '/BaiVietTaiLieu/XuLyThem',
                type: 'POST',
                data: layDataLCTForm($form),
                dataType: 'JSON'
            }).always(function () {
                $tai.tat();
            }).done(function (data) {
                if (data.trangThai == 0) {
                    var $mucBaiViet = $(data.ketQua);

                    khoiTaoItem_TaiLieu($mucBaiViet);
                    $_DanhSach.append($mucBaiViet);
                    moItem_TaiLieu($mucBaiViet);

                    khoiTaoLCTFormMacDinh($form);
                    $doiTuongAn.hide();
                }
                else {
                    moPopupThongBao(data);
                }
            }).fail(function () {
                moPopupThongBao('Thêm bài viết thất bại');
            });
        }
    });
}

function khoiTaoItem_TaiLieu($danhSachTaiLieu) {
    $danhSachTaiLieu.find('.tieu-de').on('click', function () {
        moItem_TaiLieu($(this).parent());
    });

    khoiTaoTatMoDoiTuong($danhSachTaiLieu.find('[data-chuc-nang="tat-mo"]'), true);

    $danhSachTaiLieu.find('[data-chuc-nang="xoa-bai-viet"]').on('click', function () {
        var $baiViet = $(this).closest('[data-doi-tuong="muc-bai-viet"]');

        moPopup({
            tieuDe: 'Xác nhận',
            thongBao: 'Bạn có chắc muốn xóa bài viết này?',
            bieuTuong: 'hoi',
            nut: [
                {
                    ten: 'Có',
                    xuLy: function () {
                        var $tai = moBieuTuongTai($baiViet);
                        $.ajax({
                            url: '/BaiVietTaiLieu/XuLyXoa/' + $baiViet.attr('data-ma'),
                            type: 'POST',
                            dataType: 'JSON'
                        }).always(function () {
                            $tai.tat();
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                $baiViet.remove();
                            }
                            else {
                                moPopupThongBao(data);
                            }
                        }).fail(function () {
                            moPopupThongBao('Xóa bài viết thất bại');
                        });
                    }
                },
                {
                    ten: 'Không',
                }
            ]
        });
    });

    $danhSachTaiLieu.find('[data-chuc-nang="sua-bai-viet"]').on('click', function () {
        var $baiViet = $(this).closest('[data-doi-tuong="muc-bai-viet"]');

        var $tai = moBieuTuongTai($baiViet);
        $.ajax({
            url: '/BaiVietTaiLieu/_Form/' + $baiViet.attr('data-ma'),
            dataType: 'JSON'
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai == 0) {
                var $form = $(data.ketQua);
                khoiTaoLCTForm($form, {
                    submit: function () {
                        var $tai = moBieuTuongTai($form);
                        $.ajax({
                            url: '/BaiVietTaiLieu/XuLyCapNhat',
                            type: 'POST',
                            data: layDataLCTForm($form),
                            dataType: 'JSON'
                        }).always(function () {
                            $tai.tat();
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                var $item = $(data.ketQua);
                                khoiTaoItem_TaiLieu($item);

                                $baiViet.replaceWith($item);
                            }
                            else {
                                moPopupThongBao(data);
                            }
                        }).fail(function () {
                            moPopupThongBao('Cập nhật thất bại');
                        });
                    }
                });

                $form.css({
                    border: '1px solid #ddd'
                });

                $baiViet.html($form);
            }
            else {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Sửa bài viết thất bại',
                    bieuTuong: 'nguy-hiem'
                });
            }
        }).fail(function () {
            moPopupThongBao('Sửa bài viết thất bại');
        });
    });
}

function moItem_TaiLieu($baiGiang) {
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
        custom: [
            {
                input: $doiTuongBatDauBaiViet,
                event: {
                    focus: function () {
                        $doiTuongAn.show();
                    }
                }
            }
        ],
        submit: function () {
            var $tai = moBieuTuongTai($form);
            $.ajax({
                url: '/BaiVietBaiTap/XuLyThem',
                type: 'POST',
                data: layDataLCTForm($form),
                dataType: 'JSON'
            }).always(function () {
                $tai.tat();
            }).done(function (data) {
                if (data.trangThai == 0) {
                    var $htmlBaiViet = $(data.ketQua);

                    khoiTaoItem_BaiTap($htmlBaiViet);
                    $_DanhSach.prepend($htmlBaiViet);

                    khoiTaoLCTFormMacDinh($form);
                    $doiTuongAn.hide();
                }
                else {
                    moPopupThongBao(data);
                }
            }).fail(function () {
                moPopupThongBao('Thêm bài viết thất bại');
            });
        }
    });
}

function khoiTaoItem_BaiTap($danhSachBaiTap) {
    $danhSachBaiTap.each(function () {
        var $baiTap = $(this);

        var $form = $baiTap.find('[data-doi-tuong="nop-bai-form"]');

        khoiTaoLCTForm($form, {
            custom: [{
                input: $form.find('[data-chuc-nang="thay-doi-input"]'),
                event: {
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
                var $tai = moBieuTuongTai($form);
                $.ajax({
                    url: '/BaiTapNop/XuLyThem',
                    type: 'POST',
                    data: layDataLCTForm($form),
                    dataType: 'JSON'
                }).always(function () {
                    $tai.tat();
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
                        moPopupThongBao(data);
                    }
                }).fail(function () {
                    moPopupThongBao('Nộp bài thất bại');
                });
            }
        });
    });

    khoiTaoTatMoDoiTuong($danhSachBaiTap.find('[data-chuc-nang="tat-mo"]'), true);

    $danhSachBaiTap.find('[data-chuc-nang="xoa-bai-viet"]').on('click', function () {
        var $baiViet = $(this).closest('[data-doi-tuong="muc-bai-viet"]');

        moPopup({
            tieuDe: 'Xác nhận',
            thongBao: 'Bạn có chắc muốn xóa bài viết này?',
            bieuTuong: 'hoi',
            nut: [
                {
                    ten: 'Có',
                    xuLy: function () {
                        var $tai = moBieuTuongTai($baiViet);
                        $.ajax({
                            url: '/BaiVietBaiTap/XuLyXoa/' + $baiViet.attr('data-ma'),
                            type: 'POST',
                            dataType: 'JSON'
                        }).always(function () {
                            $tai.tat();
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                $baiViet.remove();
                            }
                            else {
                                moPopupThongBao(data);
                            }
                        }).fail(function () {
                            moPopupThongBao('Xóa bài viết thất bại');
                        });
                    }
                },
                {
                    ten: 'Không',
                }
            ]
        });
    });

    $danhSachBaiTap.find('[data-chuc-nang="sua-bai-viet"]').on('click', function () {
        var $baiViet = $(this).closest('[data-doi-tuong="muc-bai-viet"]');

        var $tai = moBieuTuongTai($baiViet);
        $.ajax({
            url: '/BaiVietBaiTap/_Form/' + $baiViet.attr('data-ma'),
            dataType: 'JSON'
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai == 0) {
                var $form = $(data.ketQua);
                khoiTaoLCTForm($form, {
                    submit: function () {
                        var $tai = moBieuTuongTai($baiViet);
                        $.ajax({
                            url: '/BaiVietBaiTap/XuLyCapNhat',
                            type: 'POST',
                            data: layDataLCTForm($form),
                            dataType: 'JSON'
                        }).always(function () {
                            $tai.tat();
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                var $item = $(data.ketQua);
                                khoiTaoItem_BaiTap($item);

                                $baiViet.replaceWith($item);
                            }
                            else {
                                moPopupThongBao(data);
                            }
                        }).fail(function () {
                            moPopupThongBao('Cập nhật thất bại');
                        });
                    }
                });

                $form.css({
                    border: '1px solid #ddd'
                });

                $baiViet.html($form);
            }
            else {
                moPopupThongBao(data);
            }
        }).fail(function () {
            moPopupThongBao('Sửa bài viết thất bại');
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