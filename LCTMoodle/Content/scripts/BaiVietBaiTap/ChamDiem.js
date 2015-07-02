var htmlNhapDiem =
'<article class="hop hop-1-vien"><article class="noi-dung"><form id="diem_form"class="lct-form">' +
    '<ul><li>' +
        '<section class="noi-dung">' +
            '<article class="input" style="width: 50px; margin: 0 auto;">' +
                '<input type="text" style="text-align: center;" autofocus data-mac-dinh="123" />' +
            '</article>' +
        '</section>' +
    '</li></ul>' +
    '<section class="khung-button">' +
        '<button type="submit">Xác nhận</button>' +
        '<button type="button" data-doi-tuong="huy">Hủy</button>' +
    '</section>' +
'</form></aritcle></aritcle>';


$(function () {
    khoiTaoNutXem($('[data-chuc-nang="xem"]'));
    khoiTaoNutChamDiem($('[data-chuc-nang="cham-diem"]'));
});

function khoiTaoNutXem($dsNut) {
    $dsNut.on('click', function () {
        var $nut = $(this);
        var $item = $nut.closest('li');
        var $khungNoiDung = $('[data-doi-tuong="noi-dung-bai-nop"]');

        if ($nut.data('da-lay')) {
            if ($nut.data('an')) {
                $khungNoiDung.show();
                $nut.data('an', false);
                $nut.text('Ẩn');
            }
            else {
                $khungNoiDung.hide();
                $nut.data('an', true);
                $nut.text('Xem');
            }
        }
        else {
            var ma = $nut.data('ma-tap-tin');

            var $tai = moBieuTuongTai($item)
            $.ajax({
                url: '/DocTapTin/BaiTapNop_TapTin/' + ma,
                dataType: 'JSON'
            }).always(function () {
                $tai.tat();
            }).done(function (data) {
                if (data.trangThai == 0) {
                    var loai = data.ketQua.loai;
                    var giaTri = data.ketQua.giaTri;

                    $nut.text('Ẩn');
                    $nut.data('an', false);
                    $nut.data('da-lay', true);

                    switch (loai) {
                        case 'hinh':
                            $khungNoiDung.html('<img src="/LayTapTin/BaiTapNop_TapTin/' + ma + '"></img>');
                            break;
                        case 'text':
                            $khungNoiDung.text(giaTri);
                        case 'code':
                            var $pre = $('<pre></pre>');
                            var $code = $('<code></code>');
                            $code.text(giaTri);
                            hienThiCode($code);
                            $pre.html($code);

                            $khungNoiDung.html($pre);
                            break;
                        default:
                            $khung.NoiDung.html(giaTri);
                    }
                }
                else {
                    moPopupThongBao(data);
                }
            }).fail(function () {
                moPopupThongBao('Lấy dữ liệu tập tin thất bại');
            })
        }
    });
}

function khoiTaoNutChamDiem($dsNut) {
    $dsNut.on('click', function () {
        moPopupFull({
            html: htmlNhapDiem,
            width: '300px',
            thanhCong: function ($popup) {
                var $form = $popup.find('#diem_form');

                khoiTaoLCTForm($form, {
                    submit: function () {
                        $popup.tat();
                    },
                    custom: [
                        {
                            input: $form.find('input'),
                            event: {
                                'focus': function () {
                                    this.select();
                                }
                            }
                        }
                    ]
                });
            }
        })
    })
}