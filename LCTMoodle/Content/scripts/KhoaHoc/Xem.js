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
    $_KhungChua,
    //Khung chọn hiển thị
    $_KhungChonHienThi,
    //Danh sách bài viết chưa đọc
    $_DSChuaXem;

//#region Khởi tạo

$(function () {
    $_KhungHienThi = $('#khung_hien_thi');
    $_KhungChonHienThi = $('#khung_chon_hien_thi')
    $_KhungChua = $_KhungHienThi.parent();
    
    hienThi(layQueryString('hienthi') || 'khung', layQueryString('ma'));

    khoiTaoNutHienThi($_KhungChonHienThi.find('[data-chuc-nang="hien-thi"]'));
    khoiTaoQuayLai();
    
    //Khởi tạo nút
    khoiTaoNutDangKy($('[data-chuc-nang="dang-ky"]'));
    khoiTaoNutThamGia($('[data-chuc-nang="tham-gia"]'));
    khoiTaoNutHuyDangKy($('[data-chuc-nang="huy-dang-ky"]'));
    khoiTaoNutRoiKhoaHoc($('[data-chuc-nang="roi-kh"]'));
    khoiTaoNutXoaKhoaHoc($('[data-chuc-nang="xoa-kh"]'));
});

function hienThi(nhom, ma) {
    switch (nhom) {
        case 'diendan':
            hienThi_DienDan(ma);
            break;
        case 'baigiang':
            hienThi_BaiGiang(ma);
            break;
        case 'tailieu':
            hienThi_TaiLieu(ma);
            break;
        case 'baitap':
            hienThi_BaiTap(ma);
            break;
        default:
            hienThi_Khung();
            break;
    }
}

function khoiTaoQuayLai() {
    history.replaceState({
        hienThi: layQueryString('hienthi') || 'khung'
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
    $nut.on('click', function (e) {
        e.preventDefault();

        var $nut = $(this);
        var nhom = $nut.attr('data-value');
        var ma = $nut.attr('data-ma');
        hienThi(nhom, ma);

        history.pushState({ hienThi: nhom }, '', '?hienthi=' + nhom + (ma ? '&ma=' + ma : ''));
    });
}

function khoiTaoXuLyChuaXem(url) {
    $(window).off('scroll.chua_xem').on('scroll.chua_xem', function () {
        var sl = !$_DSChuaXem ? 0 : $_DSChuaXem.length;
        if (sl > 0) {
            for (var i = sl - 1; i >= 0; i--) {
                var $item = $($_DSChuaXem[i]);                     

                if (coTheNhinThay($item)) {
                    $_DSChuaXem.splice(i, 1);
                    
                    $.ajax({
                        url: url,
                        method: 'POST',
                        data: { ma: $item.attr('data-ma') }
                    });
                }
            }
        }
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

function khoiTaoNutXoaKhoaHoc($nuts) {
    $nuts.on('click', function () {
        moPopup({
            tieuDe: 'Xác nhận',
            thongBao: 'Bạn có chắc muốn xóa khóa học này?',
            bieuTuong: 'hoi',
            nut: [
                {
                    ten: 'Có',
                    loai: 'nguy-hiem',
                    xuLy: function () {
                        var $tai = moBieuTuongTai();
                        $.ajax({
                            url: '/KhoaHoc/XuLyXoa/' + maKhoaHoc,
                            method: 'POST',
                            dataType: 'JSON'
                        }).always(function () {
                            $tai.tat();
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                location = '/KhoaHoc/DanhSach'
                            }
                            else {
                                moPopupThongBao(data);
                            }
                        }).fail(function () {
                            moPopupThongBao('Xóa khóa học thất bại');
                        });
                    }
                }, {
                    ten: 'Không'
                }
            ]
        })
    });
}

//#endregion

//#region Khung

function hienThi_Khung() {
    var $tai = moBieuTuongTai($_KhungChonHienThi);
    $.ajax({
        url: '/KhoaHoc/_Khung',
        data: { ma: maKhoaHoc },
        dataType: 'JSON'
    }).always(function () {
        $tai.tat();
    }).done(function (data) {
        if (data.trangThai == 0) {
            var $khung = $(data.ketQua);

            khoiTaoKhung($khung);

            $_KhungHienThi.html($khung);
            $_KhungChua.attr('data-hien-thi', 'khung');

            khoiTaoNutHienThi($_KhungHienThi.find('[data-chuc-nang="hien-thi"]'));
        }
        else {
            moPopupThongBao(data);
        }
    }).fail(function () {
        moPopupThongBao('Lấy khung thất bại');
    });
}

function khoiTaoKhung($khung) {
    khoiTaoNutHienThi($khung.find('[data-chuc-nang="hien-thi"]'));
}

//#endregion

//#region Diễn đàn

function hienThi_DienDan(ma)   {
    var $tai = moBieuTuongTai($_KhungChonHienThi);
    $.ajax({
        url: '/BaiVietDienDan/_Khung/' + (ma ? ma : ''),
        data: { maKhoaHoc: maKhoaHoc },
        dataType: 'JSON'
    }).always(function () {
        $tai.tat();
    }).done(function (data) {
        if (data.trangThai == 0) {
            var $khung = $(data.ketQua);

            $_KhungHienThi.html($khung);
            $_KhungChua.attr('data-hien-thi', 'dien-dan');
            document.title = 'Diễn đàn - ' + tieuDe;

            $_DanhSach = $_KhungHienThi.find('#danh_sach_bai_viet');

            khoiTaoForm_DienDan($_KhungHienThi.find('#tao_bai_viet_form'));
            khoiTaoItem_DienDan($_KhungHienThi.find('[data-doi-tuong="muc-bai-viet"]'));

            $_DSChuaXem = $_DanhSach.find('[data-chua-xem]');
            khoiTaoXuLyChuaXem('/BaiVietDienDan/XuLyCapNhatDaXem');
        }
        else {
            moPopupThongBao(data);
        }
    }).fail(function () {
        moPopupThongBao('Lấy diễn đàn thất bại');
    });
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

                    $_DSChuaXem = $_DSChuaXem.add($item);

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
            data: { ghim: ghim },
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
    });
    
    $danhSachBaiViet.find('[data-chuc-nang="cho-diem-bai-viet"]').on('click', function () {
        $item = $(this).closest('[data-doi-tuong="muc-bai-viet"]');

        moPopupFull({
            html: '<article class="hop hop-1-vien"><article class="noi-dung"><form id="nhap_form"class="lct-form">' +
                        '<ul><li>' +
                            '<section class="noi-dung">' +
                                '<article class="input" style="width: 50px; margin: 0 auto;">' +
                                    '<input data-validate="so-nguyen bat-buoc" type="text" style="text-align: center; font-size: 20px; font-family: Berlin" autofocus data-doi-tuong="diem" />' +
                                '</article>' +
                            '</section>' +
                        '</li></ul>' +
                        '<section class="khung-button">' +
                            '<button type="submit">Xác nhận</button>' +
                            '<button type="button" data-chuc-nang="huy">Hủy</button>' +
                        '</section>' +
                    '</form></aritcle></aritcle>',
            width: '300px',
            thanhCong: function ($popup) {
                var diem = $item.data('diem') || '';
                var $form = $popup.find('form');
                
                khoiTaoLCTForm($form, {
                    khoiTao: function () {
                        $form.find('input').attr('data-mac-dinh', diem);
                    },
                    submit: function () {
                        $popup.tat();
                        var diem = $form.find('input').val();

                        var $tai = moBieuTuongTai($item);
                        $.ajax({
                            url: '/BaiVietDienDan/XuLyChoDiem/' + $item.data('ma'),
                            method: 'POST',
                            data: { diem: diem },
                            dataType: 'JSON'
                        }).always(function () {
                            $tai.tat();
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                $item.data('diem', diem);
                                $item.find('[data-doi-tuong="diem"]').text(diem == 0 ? '' : diem);
                            }
                            else {
                                moPopupThongBao(data);
                            }
                        }).fail(function () {
                            moPopupThongBao('Cho điểm thất bại');
                        });
                    }
                })
            }
        });
    });

    $danhSachBaiViet.find('[data-chuc-nang="binh-luan"]').on('click', function () {
        var $nut = $(this);
        var $item = $nut.closest('[data-doi-tuong="muc-bai-viet"]');

        $tai = moBieuTuongTai($nut)
        $.ajax({
            url: '/BinhLuanBaiVietDienDan/_Khung',
            data: { maBaiVietDienDan: $item.data('ma') },
            dataType: 'JSON'
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai == 0) {
                $nut.remove();
                var $khung = $(data.ketQua);
                khoiTaoKhungBinhLuan($khung);
                $item.append($khung);
            }
            else {
                moPopupThongBao(data);
            }
        }).fail(function () {
            moPopupThongBao('Lấy bình luận thất bại');
        });
    });

    khoiTaoNutMoPopupTapTin($danhSachBaiViet.find('[data-chuc-nang="mo-popup-tap-tin"]'));

    khoiTaoKhungBinhLuan($danhSachBaiViet.find('[data-doi-tuong="khung-binh-luan"]'));

    function khoiTaoKhungBinhLuan($khung) {
        khoiTaoForm($khung.find('[data-doi-tuong="binh-luan-form"]'));
        khoiTaoItem($khung.find('[data-doi-tuong="muc-binh-luan"]'));

        function khoiTaoForm($form) {
            khoiTaoLCTForm($form, {
                submit: function () {
                    var $tai = moBieuTuongTai($form.closest('[data-doi-tuong="khung-binh-luan"]'));
                    $.ajax({
                        url: '/BinhLuanBaiVietDienDan/XuLyThem',
                        type: 'POST',
                        data: $form.serialize(),
                        dataType: 'JSON'
                    }).always(function () {
                        $tai.tat();
                    }).done(function (data) {
                        if (data.trangThai == 0) {
                            var $binhLuan = $(data.ketQua);
                            khoiTaoItem($binhLuan);

                            $form.closest('[data-doi-tuong="khung-binh-luan"]').find('[data-doi-tuong="danh-sach"]').append($binhLuan);

                            khoiTaoLCTFormMacDinh($form);
                        }
                        else {
                            moPopupThongBao(data);
                        }
                    }).fail(function () {
                        moPopupThongBao('Thêm bình luận thất bại');
                    });
                }
            });
        }

        function khoiTaoItem($items) {
            khoiTaoTatMoDoiTuong($items.find('[data-chuc-nang="tat-mo"]'), true);

            $items.find('[data-chuc-nang="xoa-binh-luan"]').on('click', function () {
                var $item = $(this).closest('[data-doi-tuong="muc-binh-luan"]');

                moPopup({
                    tieuDe: 'Xác nhận',
                    thongBao: 'Bạn có chắc muốn xóa bình luận này?',
                    bieuTuong: 'hoi',
                    nut: [
                        {
                            ten: 'Có',
                            xuLy: function () {
                                var $tai = moBieuTuongTai($item);
                                $.ajax({
                                    url: '/BinhLuanBaiVietDienDan/XuLyXoa/' + $item.attr('data-ma'),
                                    type: 'POST',
                                    dataType: 'JSON'
                                }).always(function () {
                                    $tai.tat();
                                }).done(function (data) {
                                    if (data.trangThai == 0) {
                                        $item.remove();
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

            $items.find('[data-chuc-nang="sua-binh-luan"]').on('click', function () {
                var $item = $(this).closest('[data-doi-tuong="muc-binh-luan"]');
                var $khung = $item.closest('[data-doi-tuong="khung-binh-luan"]');

                var $tai = moBieuTuongTai($item);
                $.ajax({
                    url: '/BinhLuanBaiVietDienDan/_Form',
                    data: { ma: $item.data('ma') },
                    dataType: 'JSON'
                }).always(function () {
                    $tai.tat();
                }).done(function (data) {
                    if (data.trangThai == 0) {
                        var $form = $(data.ketQua);

                        var htmlTam = $item.html();
                        $item.html($form);
                        khoiTaoLCTForm($form, {
                            submit: function () {
                                $tai = moBieuTuongTai($item);
                                $.ajax({
                                    url: '/BinhLuanBaiVietDienDan/XuLyCapNhat',
                                    method: 'POST',
                                    data: layDataLCTForm($form),
                                    dataType: 'JSON'
                                }).always(function () {
                                    $tai.tat();
                                }).done(function (data) {
                                    if (data.trangThai == 0) {
                                        var $newItem = $(data.ketQua);
                                        khoiTaoItem($newItem);
                                        $item.replaceWith($newItem);
                                    }
                                    else {
                                        moPopupThongBao(data);
                                    }
                                }).fail(function () {
                                    moPopupThongBao('Cập nhật thất bại');
                                });
                            },
                            custom: [
                                {
                                    input: $form.find('[data-chuc-nang="huy"]'),
                                    event: {
                                        click: function () {
                                            $item.html(htmlTam);
                                            khoiTaoItem($item);
                                        }
                                    }
                                }
                            ]
                        });
                    }
                    else {
                        moPopupThongBao(data);
                    }
                }).fail(function () {
                    moPopupThongBao('Lấy dữ liệu sửa thất bại');
                });
            });

            $items.find('[data-chuc-nang="cho-diem-binh-luan"]').on('click', function () {
                $item = $(this).closest('[data-doi-tuong="muc-binh-luan"]');

                moPopupFull({
                    html: '<article class="hop hop-1-vien"><article class="noi-dung"><form id="nhap_form"class="lct-form">' +
                                '<ul><li>' +
                                    '<section class="noi-dung">' +
                                        '<article class="input" style="width: 50px; margin: 0 auto;">' +
                                            '<input data-validate="so-nguyen bat-buoc" type="text" style="text-align: center; font-size: 20px; font-family: Berlin" autofocus data-doi-tuong="diem" />' +
                                        '</article>' +
                                    '</section>' +
                                '</li></ul>' +
                                '<section class="khung-button">' +
                                    '<button type="submit">Xác nhận</button>' +
                                    '<button type="button" data-chuc-nang="huy">Hủy</button>' +
                                '</section>' +
                            '</form></aritcle></aritcle>',
                    width: '300px',
                    thanhCong: function ($popup) {
                        var diem = $item.data('diem') || '';
                        var $form = $popup.find('form');

                        khoiTaoLCTForm($form, {
                            khoiTao: function () {
                                $form.find('input').attr('data-mac-dinh', diem);
                            },
                            submit: function () {
                                $popup.tat();
                                var diem = $form.find('input').val();

                                var $tai = moBieuTuongTai($item);
                                $.ajax({
                                    url: '/BinhLuanBaiVietDienDan/XuLyChoDiem/' + $item.data('ma'),
                                    method: 'POST',
                                    data: { diem: diem },
                                    dataType: 'JSON'
                                }).always(function () {
                                    $tai.tat();
                                }).done(function (data) {
                                    if (data.trangThai == 0) {
                                        $item.data('diem', diem);
                                        $item.find('[data-doi-tuong="diem-binh-luan"]').text(diem == 0 ? '' : diem);
                                    }
                                    else {
                                        moPopupThongBao(data);
                                    }
                                }).fail(function () {
                                    moPopupThongBao('Cho điểm thất bại');
                                });
                            }
                        })
                    }
                });
            });

            khoiTaoNutMoPopupTapTin($items.find('[data-chuc-nang="mo-popup-tap-tin"]'));
        }
    }
}

//#endregion

//#region Bài giảng

function hienThi_BaiGiang(ma) {
    var $tai = moBieuTuongTai($_KhungChonHienThi);
    $.ajax({
        url: '/BaiVietBaiGiang/_Khung/' + (ma ? ma : ''),
        data: { maKhoaHoc: maKhoaHoc },
        dataType: 'JSON'
    }).always(function () {
        $tai.tat();
    }).done(function (data) {
        if (data.trangThai == 0) {
            var $khung = $(data.ketQua);

            $_KhungHienThi.html($khung);
            $_KhungChua.attr('data-hien-thi', 'bai-giang');
            document.title = 'Bài giảng - ' + tieuDe;

            $_DanhSach = $_KhungHienThi.find('#danh_sach_bai_viet');

            khoiTaoForm_BaiGiang($_KhungHienThi.find('#tao_bai_viet_form'));
            khoiTaoItem_BaiGiang($_KhungHienThi.find('[data-doi-tuong="muc-bai-viet"]'));

            $_DSChuaXem = $_DanhSach.find('[data-chua-xem]');
            khoiTaoXuLyChuaXem('/BaiVietBaiGiang/XuLyCapNhatDaXem');
        }
        else {
            moPopupThongBao(data);
        }
    }).fail(function () {
        moPopupThongBao('Lấy bài giảng thất bại');
    });
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

                    $_DSChuaXem = $_DSChuaXem.add($mucBaiViet);

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

    khoiTaoNutMoPopupTapTin($danhSachBaiGiang.find('[data-chuc-nang="mo-popup-tap-tin"]'));
}

function moItem_BaiGiang($baiGiang) {
    if ($baiGiang.hasClass('mo')) {
        $baiGiang.removeClass('mo');
    }
    else {
        $_DanhSach.find('.mo[data-doi-tuong="muc-bai-viet"]').removeClass('mo');
        $baiGiang.addClass('mo');
        $body.animate({
            scrollTop: $baiGiang.offset().top - 50
        }, 200);
    }
}

//#endregion

//#region Tài liệu

function hienThi_TaiLieu(ma) {
    var $tai = moBieuTuongTai($_KhungChonHienThi);
    $.ajax({
        url: '/BaiVietTaiLieu/_Khung/' + (ma ? ma : ''),
        data: { maKhoaHoc: maKhoaHoc },
        dataType: 'JSON'
    }).always(function () {
        $tai.tat();
    }).done(function (data) {
        if (data.trangThai == 0) {
            var $khung = $(data.ketQua);

            $_KhungHienThi.html($khung);
            $_KhungChua.attr('data-hien-thi', 'tai-lieu');
            document.title = 'Bài giảng - ' + tieuDe;

            $_DanhSach = $_KhungHienThi.find('#danh_sach_bai_viet');

            khoiTaoForm_TaiLieu($_KhungHienThi.find('#tao_bai_viet_form'));
            khoiTaoItem_TaiLieu($_KhungHienThi.find('[data-doi-tuong="muc-bai-viet"]'));

            $_DSChuaXem = $_DanhSach.find('[data-chua-xem]');
            khoiTaoXuLyChuaXem('/BaiVietTaiLieu/XuLyCapNhatDaXem');
        }
        else {
            moPopupThongBao(data);
        }
    }).fail(function () {
        moPopupThongBao('Lấy bài giảng thất bại');
    });
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

                    $_DSChuaXem = $_DSChuaXem.add($mucBaiViet);

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
                moPopupThongBao(data);
            }
        }).fail(function () {
            moPopupThongBao('Sửa bài viết thất bại');
        });
    });

    khoiTaoNutMoPopupTapTin($danhSachTaiLieu.find('[data-chuc-nang="mo-popup-tap-tin"]'));
}

function moItem_TaiLieu($taiLieu) {
    if ($taiLieu.hasClass('mo')) {
        $taiLieu.removeClass('mo');
    }
    else {
        $_DanhSach.find('.mo[data-doi-tuong="muc-bai-viet"]').removeClass('mo');
        $taiLieu.addClass('mo');
        $body.animate({
            scrollTop: $taiLieu.offset().top - 50
        }, 200);
    }
}

//#endregion

//#region Bài tập

function hienThi_BaiTap(ma) {
    var $tai = moBieuTuongTai($_KhungChonHienThi);
    $.ajax({
        url: '/BaiVietBaiTap/_Khung/' + (ma ? ma : ''),
        data: { maKhoaHoc: maKhoaHoc },
        dataType: 'JSON'
    }).always(function () {
        $tai.tat();
    }).done(function (data) {
        if (data.trangThai == 0) {
            var $khung = $(data.ketQua);

            $_KhungHienThi.html($khung);
            $_KhungChua.attr('data-hien-thi', 'bai-tap');
            document.title = 'Bài tập - ' + tieuDe;

            $_DanhSach = $_KhungHienThi.find('#danh_sach_bai_viet');

            khoiTaoForm_BaiTap($_KhungHienThi.find('#tao_bai_viet_form'));
            khoiTaoItem_BaiTap($_DanhSach.find('[data-doi-tuong="muc-bai-viet"]'));

            $_DSChuaXem = $_DanhSach.find('[data-chua-xem]');
            khoiTaoXuLyChuaXem('/BaiVietBaiTap/XuLyCapNhatDaXem');
        }
        else {
            moPopupThongBao(data);
        }
    }).fail(function () {
        moPopupThongBao('Lấy bài tập thất bại');
    });
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
                    var $item = $(data.ketQua);

                    $_DSChuaXem = $_DSChuaXem.add($item);

                    khoiTaoItem_BaiTap($item);
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
                                $hienThi.html('<span><b>Bạn</b></span> và ' + $hienThi.html()).attr('data-type', '4');
                                break;
                            default:
                                break;
                        }

                        var $baiCuaToi = $hienThi.next('[data-doi-tuong="bai-nop-cua-toi"]');
                        if ($baiCuaToi.length == 0) {
                            $baiCuaToi = $('<a data-doi-tuong="bai-nop-cua-toi" class="bai-nop-cua-toi"></a>')
                            $hienThi.after($baiCuaToi);
                        }

                        var baiNop = data.ketQua;
                        if (baiNop.duongDan) {
                            $baiCuaToi.attr('href', baiNop.duongDan);
                            $baiCuaToi.html('<span>' + baiNop.duongDan + '</span>');
                        }
                        else {
                            $baiCuaToi.html('<img src="/LayHinh/BaiTapNop_TapTin/' + baiNop.tapTin.ma + '" /><span>' + baiNop.tapTin.ten + '</span>');
                            if (coHoTroXem(baiNop.tapTin.duoi)) {
                                khoiTaoNutMoPopupTapTin($baiCuaToi);
                            }
                        }

                        var $nut = $form.find('button');
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
    });

    khoiTaoNutMoPopupTapTin($danhSachBaiTap.find('[data-chuc-nang="mo-popup-tap-tin"]'));
}

//#endregion