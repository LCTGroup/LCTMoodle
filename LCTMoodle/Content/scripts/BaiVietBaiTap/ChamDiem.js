var htmlNhapDiem =
'<article class="hop hop-1-vien"><article class="noi-dung"><form id="diem_form"class="lct-form" data-an-rang-buoc>' +
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

var htmlNhapGhiChu =
'<article class="hop hop-1-vien"><article class="noi-dung"><form id="ghi_chu_form"class="lct-form">' +
    '<ul><li>' +
        '<section class="noi-dung">' +
            '<article class="input">' +
                '<textarea placeholder="Ghi chú"></textarea>' +
            '</article>' +
        '</section>' +
    '</li></ul>' +
    '<section class="khung-button">' +
        '<button type="submit">Xác nhận</button>' +
        '<button type="button" data-chuc-nang="huy">Hủy</button>' +
    '</section>' +
'</form></aritcle></aritcle>';

var $_khungDSBaiNop, maBaiTap;

$(function () {
    $_khungDSBaiNop = $('#ds_bai_nop');

    khoiTaoNutXem($('[data-chuc-nang="xem"]'));
    khoiTaoNutChamDiem($('[data-chuc-nang="cham-diem"]'));
    khoiTaoNutChuyenDiem($('[data-chuc-nang="chuyen-diem"]'));
    khoiTaoNutGhiChu($('[data-chuc-nang="ghi-chu"]'));
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
                var $form = $popup.find('#diem_form');

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
            html: htmlNhapGhiChu,
            width: '400px',
            thanhCong: function ($popup) {
                var $form = $popup.find('#ghi_chu_form');

                khoiTaoLCTForm($form, {
                    khoiTao: function () {
                        $form.find('textarea').attr('data-mac-dinh', $item.data('ghi-chu'));
                    },
                    submit: function () {
                        $popup.tat();
                        var ghiChu = $form.find('textarea').val();
                        var ma = $item.data('ma');

                        $item.data('ghi-chu', ghiChu);

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
                            moPopupThongBao('Chấm điểm thất bại');
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
        }
        else {
            moPopupThongBao(data);
        }
    }).fail(function () {
        moPopupThongBao('Chấm điểm thất bại');
    });
}

//#endregion