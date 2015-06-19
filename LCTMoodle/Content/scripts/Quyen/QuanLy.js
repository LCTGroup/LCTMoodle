var
    _PhamViQuanLy,
    _DoiTuongQuanLy,
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
    //Chỉ lưu quyền lá
    _MangQuyenNhom = {}, 
    //Quyền hiện tại (Ma, Global = 0)
    _DoiTuongHienTai;

$(function () {
    $_DanhSachQuyen = $('#danh_sach_quyen');
    $_DanhSachNhom = $('#danh_sach_nhom');
    $_MoTaPhamVi = $('#mo_ta_pham_vi');
    $_DanhSachNguoi = $('#danh_sach_nguoi');
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
        var giaTriTam = $input.val();

        if (giaTriTam.length == 0) {
            mangTam[$input + 'gt'] = giaTriTam;
            var $items = $(_MangHtmlNguoi[_NhomHienTai]);
            khoiTaoItem_NguoiDung($items);
            $_DanhSachNguoi.html($items);
            return;
        }

        if (mangTam[maTam + 'gt'] == giaTriTam) {
            return;
        }

        mangTam[maTam + 'td'] = true;
        clearTimeout(mangTam[$input + 'to']);
        mangTam[$input + 'to'] = setTimeout(function () {
            mangTam[$input + 'gt'] = giaTriTam;

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
        }, 500)
    })
}

//#endregion

//#region Khởi tạo nút

function khoiTaoNutChonPhamVi($nuts) {
    $nuts.on('click', function () {
        var $nut = $(this);
        var phamVi = $nut.attr('data-ma');

        if (phamVi != 'CD') {
            thanhCong = true;

            if (!(phamVi in _MangHtmlQuyen)) {
                $.ajax({
                    url: '/Quyen/_DanhSachQuyen',
                    data: { phamVi: phamVi },
                    dataType: 'JSON',
                    async: false
                }).done(function (data) {
                    if (data.trangThai == 0) {
                        _MangHtmlQuyen[phamVi] = data.ketQua;
                    }
                    else {
                        moPopup({
                            tieuDe: 'Thông báo',
                            thongBao: 'Lấy danh sách quyền thất bại',
                            bieuTuong: 'nguy-hiem'
                        });
                        thanhCong = false;
                    }
                }).fail(function () {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: 'Lấy danh sách quyền thất bại',
                        bieuTuong: 'nguy-hiem'
                    });
                    thanhCong = false;
                });
            }

            if (thanhCong) {
                $nut.parent().addClass('chon').siblings().removeClass('chon');
                _PhamViHienTai = phamVi;

                $_DanhSachQuyen.html(_MangHtmlQuyen[_PhamViHienTai]);
                khoiTaoItem_Quyen($_DanhSachQuyen.find('[data-doi-tuong="item-quyen"]'));

                $_MoTaPhamVi.text('Tùy chỉnh quyền cho nhóm');
                if (phamVi == 'KH') {
                    _DoiTuongHienTai = _DoiTuongQuanLy;
                }
                else {
                    _DoiTuongHienTai = '0';
                }

                capNhatDanhSachQuyen();
            }
        }
        else {
            moPopupFull({
                url: '/ChuDe/_Chon',
                thanhCong: function ($popup) {
                    var $khung = $popup.find('#khung_quan_ly');

                    khoiTaoKhungChuDe($khung);

                    $khung.on('chon', function (e, data) {
                        $popup.tat();

                        thanhCong = true;

                        if (!(phamVi in _MangHtmlQuyen)) {
                            $.ajax({
                                url: '/Quyen/_DanhSachQuyen',
                                data: { phamVi: phamVi },
                                dataType: 'JSON',
                                async: false
                            }).done(function (data) {
                                if (data.trangThai == 0) {
                                    _MangHtmlQuyen[phamVi] = data.ketQua;
                                }
                                else {
                                    moPopup({
                                        tieuDe: 'Thông báo',
                                        thongBao: 'Lấy danh sách quyền thất bại',
                                        bieuTuong: 'nguy-hiem'
                                    });
                                    thanhCong = false;
                                }
                            }).fail(function () {
                                moPopup({
                                    tieuDe: 'Thông báo',
                                    thongBao: 'Lấy danh sách quyền thất bại',
                                    bieuTuong: 'nguy-hiem'
                                });
                                thanhCong = false;
                            });
                        }

                        if (thanhCong) {
                            $nut.parent().addClass('chon').siblings().removeClass('chon');
                            _PhamViHienTai = phamVi;

                            $_DanhSachQuyen.html(_MangHtmlQuyen[_PhamViHienTai]);
                            khoiTaoItem_Quyen($_DanhSachQuyen.find('[data-doi-tuong="item-quyen"]'));

                            $_MoTaPhamVi.text('Tùy chỉnh quyền cho nhóm - ' + data.ten);
                            _DoiTuongHienTai = data.ma;

                            capNhatDanhSachQuyen();
                        }
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
            data: { phamVi: _PhamViQuanLy, doiTuong: _DoiTuongQuanLy },
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
        var $item = $(this).closest('[data-doi-tuong="item-quyen"]'),
            maQuyen = this.value,
            them = this.checked,
            la = $item.is('[data-la]');

        $.ajax({
            url: '/Quyen/XuLyCapNhatQuyenNhom',
            type: 'POST',
            data: {
                phamVi: _PhamViQuanLy,
                maNhom: _NhomHienTai,
                maQuyen: maQuyen,
                maDoiTuong: _DoiTuongQuanLy,
                them: them,
                la: la
            },
            dataType: 'JSON'
        }).done(function (data) {
            if (data.trangThai <= 1) {
                capNhatCheckQuyenChaCon($item);
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
    });

    $items.on({
        mouseleave: function () {
            $(this).trigger('hover', {
                event: 'leave',
                len: true,
                xuong: true
            })
        },
        mouseenter: function () {
            $(this).trigger('hover', {
                event: 'enter',
                len: true,
                xuong: true
            })
        },
        'hover': function (e, data) {
            var $item = $(this);
            var $tam;

            if (data.event === 'enter') {
                $item.addClass('hover')

                if (data.len) {
                    $tam = $item.prevAll('[data-ma="' + $item.attr('data-cha') + '"]');
                    if ($tam.length != 0) {
                        $tam.trigger('hover', {
                            event: 'enter',
                            len: true
                        });
                    }
                }

                if (data.xuong) {
                    $tam = $item.nextAll('[data-cha="' + $item.attr('data-ma') + '"]');
                    if ($tam.length != 0) {
                        $tam.trigger('hover', {
                            event: 'enter',
                            xuong: true
                        });
                    }
                }
            }
            else {
                $item.removeClass('hover');

                if (data.len) {
                    $tam = $item.prevAll('[data-ma="' + $item.attr('data-cha') + '"]');
                    if ($tam.length != 0) {
                        $tam.trigger('hover', {
                            event: 'leave',
                            len: true
                        });
                    }
                }

                if (data.xuong) {
                    $tam = $item.nextAll('[data-cha="' + $item.attr('data-ma') + '"]');
                    if ($tam.length != 0) {
                        $tam.trigger('hover', {
                            event: 'leave',
                            xuong: true
                        });
                    }
                }
            }
        },
        chuyenAnHienCon: function (e, anCon) {
            var $item = $(this);

            if (anCon || $item.hasClass('mo')) {
                $item.removeClass('mo');
                $item.nextAll('[data-cha="' + $item.attr('data-ma') + '"]').addClass('an').trigger('chuyenAnHienCon', true);
            }
            else {
                $item.addClass('mo');
                $item.nextAll('[data-cha="' + $item.attr('data-ma') + '"]').removeClass('an');
            }
        }
    });

    $items.find('[data-chuc-nang="an-hien-con"]').on({
        click: function () {
            $(this).closest('[data-doi-tuong="item-quyen"]').trigger('chuyenAnHienCon');
        }
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
                            data: { phamVi: _PhamViQuanLy },
                            type: 'POST',
                            dataType: 'JSON'
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                $item.remove();
                                capNhatTatMoKhung(false);
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
                data: { phamVi: _PhamViQuanLy, maNhom: _NhomHienTai, maDoiTuong: _DoiTuongQuanLy },
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
        capNhatTatMoKhung(true);
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
            false;

    $_DanhSachQuyen.find('[type="checkbox"]').each(function () {
        this.checked = mangQuyen && mangQuyen.indexOf("|" + this.value + "|") != -1;
    });

    capNhatCheckQuyenChaCon();
}

function capNhatCheckQuyenChaCon($item) {
    if (typeof($item) !== 'undefined') {
        if ($item.find('input[type="checkbox"]').prop('checked')) {
            if ($item.is('[data-la]')) {
                var doiTuongPhamVi = _PhamViHienTai + _DoiTuongHienTai;
                if (!(doiTuongPhamVi in _MangQuyenNhom)) {
                    _MangQuyenNhom[_NhomHienTai][doiTuongPhamVi] = '|';
                }
                _MangQuyenNhom[_NhomHienTai][doiTuongPhamVi] += $item.find('input[type="checkbox"]').val() + '|';
            }
            else {
                var doiTuongPhamVi = _PhamViHienTai + _DoiTuongHienTai;
                if (!(doiTuongPhamVi in _MangQuyenNhom)) {
                    _MangQuyenNhom[_NhomHienTai][doiTuongPhamVi] = '|';
                }
                var mangQuyen = _MangQuyenNhom[_NhomHienTai][doiTuongPhamVi];
                layTatCaQuyenCon($item).find('input[type="checkbox"]').each(function () {
                    if (!this.checked) {
                        mangQuyen += this.value + '|';

                        this.checked = true;
                    }
                });
                _MangQuyenNhom[_NhomHienTai][doiTuongPhamVi] = mangQuyen;
            }

            if ($item.is('[data-cha!="0"]')) {
                var $danhSachCha = layTatCaQuyenCha($item);
                var $tam = $item;

                for (var i = $danhSachCha.length - 1; i >= 0; i--) {
                    var $cha = $($danhSachCha[i]);

                    if ($tam.siblings('[data-cha="' + $tam.attr('data-cha') + '"]').find('[type="checkbox"]:not(:checked)').length == 0) {
                        $cha.find('input[type="checkbox"]').prop('checked', true);
                    }
                    else {
                        break;
                    }

                    $tam = $cha;
                }
            }
        }
        else {
            if ($item.is('[data-la]')) {
                var doiTuongPhamVi = _PhamViHienTai + _DoiTuongHienTai;
                if (!(doiTuongPhamVi in _MangQuyenNhom)) {
                    _MangQuyenNhom[_NhomHienTai][doiTuongPhamVi] = '|';
                }
                _MangQuyenNhom[_NhomHienTai][doiTuongPhamVi] = _MangQuyenNhom[_NhomHienTai][doiTuongPhamVi].replace('|' + $item.find('input[type="checkbox"]').val() + '|', '|');
            }
            else {
                var doiTuongPhamVi = _PhamViHienTai + _DoiTuongHienTai;
                if (!(doiTuongPhamVi in _MangQuyenNhom)) {
                    _MangQuyenNhom[_NhomHienTai][doiTuongPhamVi] = '|';
                }
                var mangQuyen = _MangQuyenNhom[_NhomHienTai][doiTuongPhamVi];
                layTatCaQuyenCon($item).find('input[type="checkbox"]').each(function () {
                    if (this.checked) {
                        mangQuyen = mangQuyen.replace('|' + this.value + '|', '|')

                        this.checked = false;
                    }
                });
                _MangQuyenNhom[_NhomHienTai][doiTuongPhamVi] = mangQuyen;
            }
                
            if ($item.is('[data-cha!="0"]')) {
                layTatCaQuyenCha($item).find('input[type="checkbox"]').prop('checked', false);
            }
        }

        return;
    }

    var maChaDaCo = {};
    var capDangXet = '0';
    var $danhSach = $_DanhSachQuyen.find('[data-doi-tuong="item-quyen"]').filter(function () {
        var $item = $(this),
            cap = $item.attr('data-cap'),
            maCha = $(this).attr('data-cha');

        if (cap > capDangXet) {
            capDangXet = cap;
        }

        if (maChaDaCo[maCha]) {
            return false;
        }
        else {
            maChaDaCo[maCha] = true;
            return true;
        }
    });

    while (capDangXet > 1) {
        $danhSach.each(function () {
            if (this.getAttribute('data-cap') == capDangXet) {
                var $item = $(this);
                var cha = $item.attr('data-cha');
                
                $item.prevAll('[data-ma="' + cha + '"]').find('[type="checkbox"]').prop(
                    'checked',
                    $item.find('[type="checkbox"]').prop('checked') && $item.siblings('[data-cha="' + cha + '"]').find('[type="checkbox"]:not(:checked)').length == 0
                );
            }
        })
        
        capDangXet = capDangXet - 1;
    }
}

function capNhatTatMoKhung(mo) {
    if (mo) {
        $('#khung_nguoi, #khung_quyen').removeClass('tat')
    }
    else {
        $('#khung_nguoi, #khung_quyen').addClass('tat')
    }
}

//#endregion

//#region Hỗ trợ

function layTatCaQuyenCon($items) {
    if ($items.length == 0) {
        return;
    }

    var $tam = $();

    $items.each(function () {
        var $item = $(this);

        $tam = $tam.add($item.nextAll('[data-cha="' + $item.attr('data-ma') + '"]'));
    })

    return $tam.add(layTatCaQuyenCon($tam));
}

function layTatCaQuyenCha($items) {
    if ($items.length == 0) {
        return;
    }

    var $tam = $();

    $items.each(function () {
        var $item = $(this);

        $tam = $tam.add($item.prevAll('[data-ma="' + $item.attr('data-cha') + '"]'));
    })

    return $tam.add(layTatCaQuyenCha($tam));
}

//#endregion