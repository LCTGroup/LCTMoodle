var
    _PhamViQuanLy = 'HT',
    $_DanhSachQuyen,
    $_DanhSachNhom,
    $_MoTaPhamVi,
    $_DanhSachNguoi,
    //Xác định = Mã phạm vi (HT, CD, ...)
    _MangHtmlQuyen = {},
    //Xác định = Mã nhóm (1, 2, 5, ...)
    _MangHtmlNguoi = {},
    //HT, CD,...
    _PhamViHienTai, 
    //Mã nhóm hiện tại
    _NhomHienTai, 
    //Mảng chứa danh sách quyền của nhóm { "MaNhom", { "PhamViMa", "ChuoiMaQuyen |1|2|3|" } }
    _MangQuyenNhom = {}, 
    //Quyền hiện tại (Ma, Global = 0)
    _DoiTuongHienTai;

$(function () {
    $_DanhSachQuyen = $('#danh_sach_quyen');
    $_DanhSachNhom = $('#danh_sach_nhom');
    $_MoTaPhamVi = $('#mo_ta_pham_vi');
    $_DanhSachNguoi = $('#danh_sach_nguoi');
    _PhamViHienTai = 'HT';
    _DoiTuongHienTai = '0';
    _MangHtmlQuyen[_PhamViHienTai] = $_DanhSachQuyen.html();

    khoiTaoTimKiemNguoiDung($('#tim_nguoi_input'));

    khoiTaoNutChonPhamVi($('[data-chuc-nang="chon-pham-vi"]'));
    khoiTaoNutThemNhom($('[data-chuc-nang="them-nhom"]'));
    
    khoiTaoItem_Nhom($_DanhSachNhom.find('[data-doi-tuong="item-nhom"]'));
    khoiTaoItem_Quyen($_DanhSachQuyen.find('[data-doi-tuong="item-quyen"]'));
});

//#region Khởi tạo chức năng

function khoiTaoTimKiemNguoiDung($inputs) {
    $inputs.on('keyup', function () {
        var $input = $(this);

        var maTam = 'tim_nguoi_';

        if (mangTam[maTam + 'gt'] == $input.val()) {
            return;
        }

        mangTam[maTam + 'td'] = true;
        clearTimeout(mangTam[$input + 'to']);
        mangTam[$input + 'to'] = setTimeout(function () {
            mangTam[$input + 'gt'] = $input.val();

            $.ajax({
                url: '/Quyen/_DanhSachNguoiDung_Tim',
                data: { tuKhoa: $input.val(), phamVi: _PhamViHienTai, maNhom: _NhomHienTai },
                dataType: 'JSON'
            }).done(function (data) {
                if (data.trangThai == 0) {
                    var $items = $(data.ketQua);

                    khoiTaoItem_NguoiDung($items)
                    $_DanhSachNguoi.html($items);
                }
                else {
                    $_DanhSachNguoi.html('');
                }
            }).fail(function () {
                $_DanhSachNguoi.html('');
            });
        }, 200)
    })
}

//#endregion

//#region Khởi tạo nút

function khoiTaoNutChonPhamVi($nuts) {
    $nuts.on('click', function () {
        var $nut = $(this);
        var phamVi = $nut.attr('data-ma');

        if (phamVi != 'CD') {
            $nut.parent().addClass('chon').siblings().removeClass('chon');
        
            _PhamViHienTai = phamVi;

            if (!(_PhamViHienTai in _MangHtmlQuyen)) {
                $.ajax({
                    url: '/Quyen/_DanhSachQuyen',
                    data: { phamVi: _PhamViHienTai },
                    dataType: 'JSON',
                    async: false
                }).done(function (data) {
                    if (data.trangThai == 0) {
                        _MangHtmlQuyen[_PhamViHienTai] = data.ketQua;
                    }
                    else {
                        moPopup({
                            tieuDe: 'Thông báo',
                            thongBao: 'Lấy danh sách quyền thất bại',
                            bieuTuong: 'nguy-hiem'
                        });
                    }
                }).fail(function () {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: 'Lấy danh sách quyền thất bại',
                        bieuTuong: 'nguy-hiem'
                    });
                });
            }

            $_DanhSachQuyen.html(_MangHtmlQuyen[_PhamViHienTai]);
            khoiTaoItem_Quyen($_DanhSachQuyen.find('[data-doi-tuong="item-quyen"]'));

            $_MoTaPhamVi.text('Tùy chỉnh quyền cho nhóm');
            _DoiTuongHienTai = '0';

            capNhatDanhSachQuyen();
        }
        else {
            moPopupFull({
                url: '/ChuDe/_Chon',
                thanhCong: function ($popup) {
                    var $khung = $popup.find('#khung_quan_ly');

                    khoiTaoKhungChuDe($khung);

                    $khung.on('chon', function (e, data) {
                        $popup.tat();

                        $nut.parent().addClass('chon').siblings().removeClass('chon');

                        _PhamViHienTai = phamVi;

                        if (!(_PhamViHienTai in _MangHtmlQuyen)) {
                            $.ajax({
                                url: '/Quyen/_DanhSachQuyen',
                                data: { phamVi: _PhamViHienTai },
                                dataType: 'JSON',
                                async: false
                            }).done(function (data) {
                                if (data.trangThai == 0) {
                                    _MangHtmlQuyen[_PhamViHienTai] = data.ketQua;
                                }
                                else {
                                    moPopup({
                                        tieuDe: 'Thông báo',
                                        thongBao: 'Lấy danh sách quyền thất bại',
                                        bieuTuong: 'nguy-hiem'
                                    });
                                }
                            }).fail(function () {
                                moPopup({
                                    tieuDe: 'Thông báo',
                                    thongBao: 'Lấy danh sách quyền thất bại',
                                    bieuTuong: 'nguy-hiem'
                                });
                            });
                        }

                        $_DanhSachQuyen.html(_MangHtmlQuyen[_PhamViHienTai]);
                        khoiTaoItem_Quyen($_DanhSachQuyen.find('[data-doi-tuong="item-quyen"]'));

                        $_MoTaPhamVi.text('Tùy chỉnh quyền cho nhóm - ' + data.ten);
                        _DoiTuongHienTai = data.ma;

                        capNhatDanhSachQuyen();
                    });
                }
            });
        }
    })
}

function khoiTaoNutThemNhom($nuts) {
    $nuts.on('click', function () {
        var $nut = $(this);

        moPopupFull({
            url: '/Quyen/_FormNhom',
            data: { phamVi: 'HT' },
            width: '400px',
            thanhCong: function ($khung) {
                var $form = $khung.find('#them_nhom_form');
                khoiTaoLCTForm($form, {
                    submit: function () {
                        $.ajax({
                            url: '/Quyen/XuLyThemNhom',
                            type: 'POST',
                            data: layDataLCTForm($form),
                            dataType: 'JSON'
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                $khung.tat();
                                var $item = $(data.ketQua);

                                khoiTaoItem_Nhom($item);

                                $_DanhSachNhom.prepend($item);
                            }
                            else {
                                moPopup({
                                    tieuDe: 'Thông báo',
                                    thongBao: 'Thêm nhóm thất bại',
                                    bieuTuong: 'nguy-hiem'
                                });
                            }
                        }).fail(function () {
                            moPopup({
                                tieuDe: 'Thông báo',
                                thongBao: 'Thêm nhóm thất bại',
                                bieuTuong: 'nguy-hiem'
                            });
                        });
                    }
                });
            }
        })
    });
}

//#endregion

//#region Khởi tạo Item

function khoiTaoItem_Quyen($items) {
    var $checks = $items.find('input[type="checkbox"]');

    $checks.each(function () {
        $element = $(this);
        $element.wrap('<label class="checkbox-radio-label"></label>');
        $element.after('<u></u>' + $element.attr('data-text'));
    });

    $checks.on('change', function () {
        var maQuyen = this.value;
        var them = this.checked;
        $.ajax({
            url: '/Quyen/XuLyCapNhatQuyenNhom',
            type: 'POST',
            data: { 
                phamVi: 'HT', 
                maNhom: _NhomHienTai,
                maQuyen: maQuyen,
                maDoiTuong: _DoiTuongHienTai,
                them: them
            },
            dataType: 'JSON'
        }).done(function (data) {
            if (data.trangThai == 0) {
                var doiTuongPhamVi = _PhamViHienTai + _DoiTuongHienTai;
                if (them) {
                    if (!(doiTuongPhamVi in _MangQuyenNhom[_NhomHienTai])) {
                        _MangQuyenNhom[_NhomHienTai][doiTuongPhamVi] = '|';
                    }

                    _MangQuyenNhom[_NhomHienTai][doiTuongPhamVi] += maQuyen + '|';
                }
                else {
                    _MangQuyenNhom[_NhomHienTai][doiTuongPhamVi] = _MangQuyenNhom[_NhomHienTai][doiTuongPhamVi].replace('|' + maQuyen + '|', '|')
                }
            }
            else {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Cập nhật quyền thất bại',
                    bieuTuong: 'nguy-hiem'
                });
            }
        }).fail(function () {
            moPopup({
                tieuDe: 'Thông báo',
                thongBao: 'Cập nhật quyền thất bại',
                bieuTuong: 'nguy-hiem'
            });
        })
    })
}

function khoiTaoItem_Nhom($items) {
    khoiTaoTatMoDoiTuong($items.find('[data-chuc-nang="tat-mo"]'), true);

    $items.find('[data-chuc-nang="xoa-nhom"]').on('click', function () {
        $item = $(this).closest('[data-doi-tuong="item-nhom"]');

        moPopup({
            tieuDe: 'Xác nhận',
            thongBao: 'Bạn có chắc muốn xóa nhóm này?',
            bieuTuong: 'hoi',
            nut: [
                {
                    ten: 'Có',
                    loai: 'can-than',
                    xuLy: function () {
                        $.ajax({
                            url: '/Quyen/XuLyXoaNhom/' + $item.attr('data-ma'),
                            data: { phamVi: 'HT' },
                            type: 'POST',
                            dataType: 'JSON'
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                $item.remove();
                            }
                            else {
                                moPopup({
                                    tieuDe: 'Thông báo',
                                    thongBao: 'Xóa nhóm thất bại'
                                })
                            }
                        }).fail(function () {
                            moPopup({
                                tieuDe: 'Thông báo',
                                thongBao: 'Xóa nhóm thất bại'
                            })
                        });
                    }
                },
                {
                    ten: 'Không'
                }
            ]
        })
    });

    $items.find('[data-chuc-nang="chon-nhom"]').on('click', function () {
        var $item = $(this).closest('[data-doi-tuong="item-nhom"]');

        $item.addClass('chon').siblings().removeClass('chon');

        _NhomHienTai = $item.attr('data-ma');

        if (!(_NhomHienTai in _MangQuyenNhom)) {
            $.ajax({
                url: '/Quyen/XulyLayQuyenNhom',
                data: { phamVi: _PhamViQuanLy, maNhom: _NhomHienTai },
                dataType: 'JSON',
                async: false
            }).done(function (data) {
                if (data.trangThai == 0) {
                    _MangQuyenNhom[_NhomHienTai] = data.ketQua;
                }
                else if (data.trangThai == 1) {
                    _MangQuyenNhom[_NhomHienTai] = {};
                }
                else {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: 'Lấy quyền của nhóm thất bại',
                        bieuTuong: 'nguy-hiem'
                    });
                }
            }).fail(function () {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Lấy quyền của nhóm thất bại',
                    bieuTuong: 'nguy-hiem'
                });
            });
        }

        if (!(_NhomHienTai in _MangHtmlNguoi)) {
            $.ajax({
                url: '/Quyen/_DanhSachNguoiDung',
                data: { phamVi: _PhamViQuanLy, maNhom: _NhomHienTai },
                dataType: 'JSON',
            }).done(function (data) {
                if (data.trangThai == 0) {
                    var $items = $(data.ketQua);

                    khoiTaoItem_NguoiDung($items);

                    _MangHtmlNguoi[_NhomHienTai] = data.ketQua;
                    $_DanhSachNguoi.html($items);
                }
                else if (data.trangThai == 1) {
                    _MangHtmlNguoi[_NhomHienTai] = '';
                    $_DanhSachNguoi.html('');
                }
                else {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: 'Lấy danh sách người dùng thất bại',
                        bieuTuong: 'nguy-hiem'
                    });
                }
            }).fail(function () {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Lấy danh sách người dùng thất bại',
                    bieuTuong: 'nguy-hiem'
                });
            })
        }
        else {
            var $items = $(_MangHtmlNguoi[_NhomHienTai]);
            khoiTaoItem_NguoiDung($items);
            $_DanhSachNguoi.html($items);
        }

        capNhatDanhSachQuyen();
    });
}

function khoiTaoItem_NguoiDung($items) {
    khoiTaoTatMoDoiTuong($items.find('[data-chuc-nang="tat-mo"]'));

    $items.find('[data-chuc-nang="them-vao-nhom"]').on('click', function () {
        var $item = $(this).closest('[data-doi-tuong="item-nguoi-dung"]');

        $.ajax({
            url: '/Quyen/XuLyThemNguoiDung',
            type: 'POST',
            data: {
                phamVi: _PhamViQuanLy,
                maNhom: _NhomHienTai,
                maNguoiDung: $item.attr('data-ma')
            },
            dataType: 'JSON'
        }).done(function (data) {
            if (data.trangThai == 0) {
                var $newItem = $(data.ketQua);

                khoiTaoItem_NguoiDung($newItem);
                $item.replaceWith($newItem);

                _MangHtmlNguoi[_NhomHienTai] += data.ketQua;
            }
            else {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Thêm người dùng vào nhóm thất bại',
                    bieuTuong: 'nguy-hiem'
                });
            }
        }).fail(function () {
            moPopup({
                tieuDe: 'Thông báo',
                thongBao: 'Thêm người dùng vào nhóm thất bại',
                bieuTuong: 'nguy-hiem'
            });
        })
    });

    $items.find('[data-chuc-nang="xoa-khoi-nhom"]').on('click', function () {
        var $item = $(this).closest('[data-doi-tuong="item-nguoi-dung"]');
        var ma = $item.attr('data-ma');
        $.ajax({
            url: '/Quyen/XuLyXoaNguoiDung',
            type: 'POST',
            data: {
                phamVi: _PhamViQuanLy,
                maNhom: _NhomHienTai,
                maNguoiDung: ma
            },
            dataType: 'JSON'
        }).done(function (data) {
            if (data.trangThai == 0) {
                $item.remove();

                var $tam = $('<fake></fake>').html(_MangHtmlNguoi[_NhomHienTai]);
                $tam.children('[data-ma="' + ma + '"]').remove();
                _MangHtmlNguoi[_NhomHienTai] = $tam.html();
            }
            else {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Xóa người dùng khỏi nhóm thất bại',
                    bieuTuong: 'nguy-hiem'
                });
            }
        }).fail(function () {
            moPopup({
                tieuDe: 'Thông báo',
                thongBao: 'Xóa người dùng khỏi nhóm thất bại',
                bieuTuong: 'nguy-hiem'
            });
        })
    });
}

//#endregion

//#region Xử lý hiển thị

function capNhatDanhSachQuyen() {
    var doiTuongPhamVi = _PhamViHienTai + _DoiTuongHienTai;
    var mangQuyen = 
        _NhomHienTai in _MangQuyenNhom && doiTuongPhamVi in _MangQuyenNhom[_NhomHienTai] ?
            _MangQuyenNhom[_NhomHienTai][doiTuongPhamVi] :
            false   

    $_DanhSachQuyen.find('[type="checkbox"]').each(function () {
        this.checked = mangQuyen && mangQuyen.indexOf("|" + this.value + "|") != -1;
    });
}

//#endregion