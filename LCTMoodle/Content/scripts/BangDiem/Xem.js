﻿var $_KhungTieuDe, $_KhungDiem, htmlTruocKhiSua, $_Khung, tongHeSo;

$(function () {
    $_Khung = $('#khung');
    $_KhungTieuDe = $_Khung.find('#khung_tieu_de');
    $_KhungDiem = $_Khung.find('#khung_diem');

    khoiTaoNutCheDoSua($_Khung.find('[data-chuc-nang="sua-bang-diem"]'));
    khoiTaoNutHoanThanhSua($_Khung.find('[data-chuc-nang="hoan-thanh-sua"]'));
    khoiTaoNutLuuSua($_Khung.find('[data-chuc-nang="luu-sua"]'));
    khoiTaoNutHuySua($_Khung.find('[data-chuc-nang="huy-sua"]'));
});

//#region Chức năng

function khoiTaoNutCheDoSua($nuts) {
    $nuts.on('click', function () {
        chuyenCheDo(true);

        htmlTruocKhiSua = $_KhungDiem.html();

        var $dsODiem = $_KhungDiem.find('[data-la-cot-diem]');

        $dsODiem.each(function () {
            var $oDiem = $(this);
            var diem = $oDiem.data('diem');

            $oDiem.html('<article class="input"><input data-validate="so-thuc" class="diem-input" type="text" data-mac-dinh="' + (diem || diem == '0' ? diem : '') + '" /></article>');
        });

        khoiTaoLCTForm($_KhungDiem);

        $dsODiem.find('input').on('change', function () {
            var $input = $(this);
            var value = $input.val();
            
            if (value > 10 || value < 0 || isNaN(parseFloat(value))) {
                $input.val('');
                $input.closest('td').data('diem', '');
            }
            else {
                value = Math.round(value * 100) / 100;
                $input.closest('td').data('diem', value);
                $input.val(value);
            }

            capNhatDiemDong($input.closest('tr'));
        });
    });
}

function khoiTaoNutHoanThanhSua($nuts) {
    $nuts.hide();

    $nuts.on('click', function () {
        var dsCapNhat = [];
        $_KhungDiem.find('input:not([data-cu])').each(function () {
            var $o = $(this).closest('td');

            dsCapNhat.push({
                maCotDiem: $o.data('ma'),
                maNguoiDung: $o.closest('tr').data('ma'),
                diem: $o.data('diem')
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
                    $_KhungDiem.find('[data-la-cot-diem]').each(function () {
                        var $o = $(this);
                        var diem = $o.data('diem');

                        $o.html(diem || diem == '0' ? '<span>' + diem + '</span>' : '');

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
            $_KhungDiem.find('[data-la-cot-diem]').each(function () {
                var $o = $(this);

                $o.html('<span>' + ($o.data('diem') || '') + '</span>');

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

function capNhatDiemDong($dong) {
    //Lấy điểm trung bình
    var diem = 0, $o;
    $dong.find('[data-loai="diem"]').each(function () {
        $o = $(this);
        diem += (parseFloat($o.data('diem')) || 0) * $o.data('he-so');
    })

    diem = Math.round((diem / tongHeSo) * 100) / 100;
    $dong.find('[data-cot="tb"] span').text(diem || 0);

    //Lấy điểm cộng
    var diem = 0;
    $dong.find('[data-loai="cong"]').each(function () {
        diem += parseFloat($(this).data('diem')) || 0;
    })
    
    $dong.find('[data-cot="cong"] span').text(diem || 0);
}

//#endregion
