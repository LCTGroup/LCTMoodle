var $_KhungTieuDe, $_KhungDiem, htmlTruocKhiSua, $_Khung;

$(function () {
    $_Khung = $('#khung');
    $_KhungTieuDe = $('#khung_tieu_de');
    $_KhungDiem = $('#khung_diem');

    khoiTaoNutCheDoSua($('[data-chuc-nang="sua-bang-diem"]'));
    khoiTaoNutHoanThanhSua($('[data-chuc-nang="hoan-thanh-sua"]'));
    khoiTaoNutLuuSua($('[data-chuc-nang="luu-sua"]'));
    khoiTaoNutHuySua($('[data-chuc-nang="huy-sua"]'));
});

//#region Chức năng

function khoiTaoNutCheDoSua($nuts) {
    $nuts.on('click', function () {
        chuyenCheDo(true);

        htmlTruocKhiSua = $_KhungDiem.html();

        var $dsODiem = $_KhungDiem.find('td:not(:last-child)');

        $dsODiem.each(function () {
            var $oDiem = $(this);
            $oDiem.html('<article class="input"><input data-validate="so-thuc" class="diem-input" type="text" data-mac-dinh="' + ($oDiem.attr('data-diem') || '') + '" /></article>');
        });

        khoiTaoLCTForm($_KhungDiem);

        $dsODiem.find('input').on('change', function () {
            var $input = $(this);
            var value = $input.val();

            if (value > 10 || value < 0 || isNaN(parseFloat(value))) {
                $input.val('');
                $input.closest('td').attr('data-diem', '');
            }
            else {
                value = Math.round(value * 100) / 100;
                $input.closest('td').attr('data-diem', value);
                $input.val(value);
            }

            var $row = $input.closest('tr');
            var $oDiem = $row.find('td:not(:last-child)');
            var diem = 0;

            $oDiem.each(function () {
                var $item = $(this);
                if ($item.attr('data-diem')) {
                    diem += parseFloat($(this).attr('data-diem'));
                }
            });

            diem = Math.round((diem / $oDiem.length) * 100) / 100;

            $row.find('td:last-child').text(diem);
        });
    });
}

function khoiTaoNutHoanThanhSua($nuts) {
    $nuts.hide();

    $nuts.on('click', function () {
        var mangMa = [];

        $_KhungTieuDe.find('.ngay').each(function (index) {
            mangMa.push($(this).attr('data-ma'));
        })

        var dsCapNhat = [];
        $_KhungDiem.find('input:not([data-cu])').each(function () {
            var $o = $(this).closest('td');

            dsCapNhat.push({
                maCotDiem: mangMa[$o.index() - 2],
                maNguoiDung: $o.closest('tr').attr('data-ma'),
                diem: $o.attr('data-diem')
            });
        })

        if (dsCapNhat.length != 0) {
            var $tai = moBieuTuongTai($_Khung);
            $.ajax({
                url: '/BangDiem/XuLyCapNhatBangDiem',
                method: 'POST',
                data: { jsonDiem: JSON.stringify(dsCapNhat) },
                dataType: 'JSON'
            }).always(function () {
                $tai.tat();
            }).done(function (data) {
                if (data.trangThai == 0) {
                    $_KhungDiem.find('td:not(:last-child)').each(function () {
                        var $o = $(this);
                        
                        $o.text($o.attr('data-diem') || '');

                        chuyenCheDo(false);
                    });
                }
                else {
                    moPopupThongBao(data);
                }
            }).fail(function () {
                moPopupThongBao('Cập nhật điểm thất bại');
            });
        }
        else {
            $_KhungDiem.find('td:not(:last-child)').each(function () {
                var $o = $(this);

                $o.text($o.attr('data-diem') || '');

                chuyenCheDo(false);
            });
        }
    });
}

function khoiTaoNutLuuSua($nuts) {
    $nuts.hide();
}

function khoiTaoNutHuySua($nuts) {
    $nuts.hide();

    $nuts.on('click', function () {
        $_KhungDiem.html(htmlTruocKhiSua);

        chuyenCheDo(false);
    });
}

//#endregion

//#region Hỗ trợ

function chuyenCheDo(sua) {
    if (sua) {
        $('[data-chuc-nang="sua-bang-diem"]').hide();
        $('[data-chuc-nang="hoan-thanh-sua"]').show();
        //$('[data-chuc-nang="luu-sua"]').show();
        $('[data-chuc-nang="huy-sua"]').show();
    }
    else {
        $('[data-chuc-nang="sua-bang-diem"]').show();
        $('[data-chuc-nang="hoan-thanh-sua"]').hide();
        //$('[data-chuc-nang="luu-sua"]').hide();
        $('[data-chuc-nang="huy-sua"]').hide();
    }
}

//#endregion
