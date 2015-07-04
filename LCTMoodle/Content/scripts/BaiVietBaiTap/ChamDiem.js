var htmlNhapDiem =
'<article class="hop hop-1-vien"><article class="noi-dung"><form id="nhap_form"class="lct-form">' +
    '<ul><li>' +
        '<section class="noi-dung">' +
            '<article class="input" style="width: 50px; margin: 0 auto;">' +
                '<input data-validate="so-thuc bat-buoc" type="text" style="text-align: center; font-size: 20px; font-family: Berlin" autofocus data-doi-tuong="diem" />' +
            '</article>' +
        '</section>' +
    '</li></ul>' +
    '<section class="khung-button">' +
        '<button type="submit">Xác nhận</button>' +
        '<button type="button" data-chuc-nang="huy">Hủy</button>' +
    '</section>' +
'</form></aritcle></aritcle>';

var htmlNhap =
'<article class="hop hop-1-vien"><article class="noi-dung"><form id="nhap_form"class="lct-form">' +
    '<ul><li>' +
        '<section class="noi-dung">' +
            '<article class="input">' +
                '<textarea placeholder=""></textarea>' +
            '</article>' +
        '</section>' +
    '</li></ul>' +
    '<section class="khung-button">' +
        '<button type="submit">Xác nhận</button>' +
        '<button type="button" data-chuc-nang="huy">Hủy</button>' +
    '</section>' +
'</form></aritcle></aritcle>';

var $_khungDSBaiNop, maBaiTap, $_khungChamDiem;

$(function () {
    $_khungChamDiem = $('#khung_cham_diem');
    $_khungDSBaiNop = $_khungChamDiem.find('#ds_bai_nop');

    khoiTaoNutXem($_khungDSBaiNop.find('[data-chuc-nang="xem"]'));
    khoiTaoNutChamDiem($_khungDSBaiNop.find('[data-chuc-nang="cham-diem"]'));
    khoiTaoNutGhiChu($_khungDSBaiNop.find('[data-chuc-nang="ghi-chu"]'));
    khoiTaoNutXoa($_khungDSBaiNop.find('[data-chuc-nang="xoa"]'));

    khoiTaoNutChuyenDiem($_khungChamDiem.find('[data-chuc-nang="chuyen-diem"]'));
    khoiTaoNutChonHet_Checkbox($_khungChamDiem.find('[data-chuc-nang="chon-het"]'));
    khoiTaoNutXoa_Nhieu($_khungChamDiem.find('[data-chuc-nang="xoa-nhieu"]'))
});

//#region Khởi tạo nút

function khoiTaoNutXem($dsNut) {
    $dsNut.on('click', function () {
        var $item = $(this).closest('tr');
        var ma = $item.data('ma');

        moPopupFull({
            url: '/BaiTapNop/_Xem/' + ma,
            thanhCong: function ($popup) {
                var $khungNoiDung = $popup.find('.khung-noi-dung');
                hienThiCode($khungNoiDung.find('pre code'));

                var $form = $popup.find('[data-doi-tuong="diem-form"]');

                $popup.find('[data-chuc-nang="diem"]').on('click', function () {
                    $form.submit();
                })

                khoiTaoLCTForm($form, {
                    submit: function () {
                        $popup.tat();
                        chamDiem(ma, $popup.find('[data-doi-tuong="diem"]').val());
                    },
                    custom: [
                        {
                            input: $form.find('input'),
                            event: {
                                change: function () {
                                    var $input = $(this);
                                    var value = $input.val();

                                    if (value > 10 || value < 0 || isNaN(parseFloat(value))) {
                                        $input.val('');
                                    }
                                    else {
                                        value = Math.round(value * 100) / 100;
                                        $input.val(value);
                                    }
                                }
                            }
                        }
                    ]
                });
            },
            thatBai: function () {
                moPopupThongBao('Lấy dữ liệu bài nộp thất bại');
            }
        })
    });
}

function khoiTaoNutChamDiem($dsNut) {
    $dsNut.on('click', function () {
        var $item = $(this).closest('tr');

        moPopupFull({
            html: htmlNhapDiem,
            width: '300px',
            thanhCong: function ($popup) {
                var $form = $popup.find('#nhap_form');

                khoiTaoLCTForm($form, {
                    khoiTao: function () {
                        $form.find('input').attr('data-mac-dinh', $item.find('[data-doi-tuong="diem"]').text())
                    },
                    submit: function () {
                        $popup.tat();
                        chamDiem($item.data('ma'), $popup.find('[data-doi-tuong="diem"]').val());
                    },
                    custom: [
                        {
                            input: $form.find('[data-chuc-nang="huy"]'),
                            event: {
                                'click': function () {
                                    $popup.tat();
                                }
                            }
                        }, {
                            input: $form.find('input'),
                            event: {
                                change: function () {
                                    var $input = $(this);
                                    var value = $input.val();

                                    if (value > 10 || value < 0 || isNaN(parseFloat(value))) {
                                        $input.val('');
                                    }
                                    else {
                                        value = Math.round(value * 100) / 100;
                                        $input.val(value);
                                    }
                                }
                            }
                        }
                    ]
                });
            }
        })
    })
}

function khoiTaoNutChuyenDiem($dsNut) {
    $dsNut.on('click', function () {
        var $tai = moBieuTuongTai();
        $.ajax({
            url: '/BaiVietBaiTap/XuLyChuyenDiem/' + maBaiTap,
            method: 'POST',
            dataType: 'JSON'
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai != 0) {
                moPopupThongBao(data);
            }
        }).fail(function () {
            moPopupThongBao('Chuyển điểm thất bại');
        })
    });
}

function khoiTaoNutGhiChu($dsNut) {
    $dsNut.on('click', function () {
        var $item = $(this).closest('tr');

        moPopupFull({
            html: htmlNhap,
            width: '400px',
            thanhCong: function ($popup) {
                var $form = $popup.find('#nhap_form');

                khoiTaoLCTForm($form, {
                    khoiTao: function () {
                        $form.find('textarea').attr({
                            'data-mac-dinh': $item.data('ghi-chu'),
                            'placeholder': 'Ghi chú'
                        });
                    },
                    submit: function () {
                        $popup.tat();
                        var ghiChu = $form.find('textarea').val();
                        var ma = $item.data('ma');

                        var $tai = moBieuTuongTai($item);
                        $.ajax({
                            url: '/BaiTapNop/XuLyGhiChu/' + ma,
                            method: 'POST',
                            data: { ghiChu: ghiChu },
                            dataType: 'JSON'
                        }).always(function () {
                            $tai.tat();
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                $item.data('ghi-chu', ghiChu);
                                if (ghiChu) {
                                    $item.attr('data-co-ghi-chu', '');
                                }
                                else {
                                    $item.removeAttr('data-co-ghi-chu');
                                }
                            }
                            else {
                                moPopupThongBao(data);
                            }
                        }).fail(function () {
                            moPopupThongBao('Ghi chú thất bại');
                        });
                    },
                    custom: [
                        {
                            input: $form.find('[data-chuc-nang="huy"]'),
                            event: {
                                'click': function () {
                                    $popup.tat();
                                }
                            }
                        }
                    ]
                });
            }
        })
    })
}

function khoiTaoNutXoa($dsNut) {
    $dsNut.on('click', function () {
        var $item = $(this).closest('tr');

        moPopupFull({
            html: htmlNhap,
            width: '400px',
            thanhCong: function ($popup) {
                var $form = $popup.find('#nhap_form');

                khoiTaoLCTForm($form, {
                    khoiTao: function () {
                        $form.find('textarea').attr({
                            'data-mac-dinh': $item.data('ghi-chu'),
                            'placeholder': 'Lý do'
                        });
                    },
                    submit: function () {
                        $popup.tat();
                        var lyDo = $form.find('textarea').val();
                        var ma = $item.data('ma');

                        var $tai = moBieuTuongTai($item);
                        $.ajax({
                            url: '/BaiTapNop/XuLyXoa_Mot/' + ma,
                            method: 'POST',
                            data: { lyDo: lyDo },
                            dataType: 'JSON'
                        }).always(function () {
                            $tai.tat();
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                $item.data('ghi-chu', lyDo);
                                $item.attr('data-da-xoa', '');
                                if (lyDo) {
                                    $item.attr('data-co-ghi-chu', '');
                                }
                                else {
                                    $item.removeAttr('data-co-ghi-chu');
                                }
                            }
                            else {
                                moPopupThongBao(data);
                            }
                        }).fail(function () {
                            moPopupThongBao('Xóa thất bại');
                        });
                    },
                    custom: [
                        {
                            input: $form.find('[data-chuc-nang="huy"]'),
                            event: {
                                'click': function () {
                                    $popup.tat();
                                }
                            }
                        }
                    ]
                });
            }
        })
    })
}

function khoiTaoNutChonHet_Checkbox($dsNut) {
    $dsNut.on('change', function () {
        $($_khungDSBaiNop.find('tr:not([data-da-xoa]) [data-doi-tuong="nut-chon"]')).prop('checked', this.checked);
    });
}

function khoiTaoNutXoa_Nhieu($dsNut) {
    $dsNut.on('click', function () {
        //Lấy danh sách chọn
        var $dsChon = $_khungDSBaiNop.find('tr:not([data-da-xoa]):has([data-doi-tuong="nut-chon"]:checked)');
        if ($dsChon.length == 0) {
            moPopup({
                tieuDe: 'Thông báo',
                thongBao: 'Hãy chọn bài nộp mà bạn muốn xóa',
                bieuTuong: 'thong-tin'
            });
            return;
        }

        moPopupFull({
            html: htmlNhap,
            width: '400px',
            thanhCong: function ($popup) {
                var $form = $popup.find('#nhap_form');

                khoiTaoLCTForm($form, {
                    khoiTao: function () {
                        $form.find('textarea').attr({
                            'data-mac-dinh': '',
                            'placeholder': 'Lý do'
                        });
                    },
                    submit: function () {
                        $popup.tat();
                        var lyDo = $form.find('textarea').val();
                        
                        //Lấy danh sách mã chọn
                        var dsMaChon = '';
                        $dsChon.each(function () {
                            dsMaChon += ',' + $(this).data('ma');
                        });
                        dsMaChon = dsMaChon.substr(1);

                        var $tai = moBieuTuongTai($dsChon.add($dsNut));
                        $.ajax({
                            url: '/BaiTapNop/XuLyXoa_Nhieu/',
                            method: 'POST',
                            data: { dsMa: dsMaChon, lyDo: lyDo },
                            dataType: 'JSON'
                        }).always(function () {
                            $tai.tat();
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                $dsChon.data('ghi-chu', lyDo);
                                $dsChon.attr('data-da-xoa', '');
                                if (lyDo) {
                                    $dsChon.attr('data-co-ghi-chu', '');
                                }
                                else {
                                    $dsChon.removeAttr('data-co-ghi-chu');
                                }
                            }
                            else {
                                moPopupThongBao(data);
                            }
                        }).fail(function () {
                            moPopupThongBao('Xóa thất bại');
                        });
                    },
                    custom: [
                        {
                            input: $form.find('[data-chuc-nang="huy"]'),
                            event: {
                                'click': function () {
                                    $popup.tat();
                                }
                            }
                        }
                    ]
                });
            }
        })
    })
}

//#endregion

//#region Thực hiện chức năng

function chamDiem(maBaiNop, diem) {
    var $item = $_khungDSBaiNop.find('tr[data-ma="' + maBaiNop + '"]');
    var $tai = moBieuTuongTai($item);
    $.ajax({
        url: '/BaiTapNop/XuLyChamDiem/' + maBaiNop,
        method: 'POST',
        data: { diem: diem },
        dataType: 'JSON'
    }).always(function () {
        $tai.tat();
    }).done(function (data) {
        if (data.trangThai == 0) {
            $item.find('[data-doi-tuong="diem"]').text(diem);
            $item.removeAttr('data-da-xoa');
        }
        else {
            moPopupThongBao(data);
        }
    }).fail(function () {
        moPopupThongBao('Chấm điểm thất bại');
    });
}

//#endregion