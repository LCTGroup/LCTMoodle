var $_KhungTieuDe, $_KhungDiem, htmlTruocKhiSua, $_Khung, tongHeSo;

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

        var $dsODiem = $_KhungDiem.find('[data-la-cot-diem]');

        $dsODiem.each(function () {
            var $oDiem = $(this);
            $oDiem.html('<article class="input"><input data-validate="so-thuc" class="diem-input" type="text" data-mac-dinh="' + ($oDiem.data('diem') || '') + '" /></article>');
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
                        
                        $o.text($o.data('diem') || '');

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
    var diem = 0;
    $dong.find('[data-loai="diem"]').each(function () {
        diem += parseFloat($(this).data('diem'));
    })

    diem = Math.round((diem / tongHeSo) * 100) / 100;
    $dong.find('[data-cot="tb"] span').text(diem);

    //Lấy điểm cộng
    var diem = 0;
    $dong.find('[data-loai="cong"]').each(function () {
        diem += parseFloat($(this).data('diem'));
    })
    
    dong.find('[data-loai="cong"] span').text(diem);
}

//#endregion
