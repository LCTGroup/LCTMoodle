var $_KhungDiem;

$(function () {
    $_KhungDiem = $('#khung_diem');

    khoiTaoNutCheDoSua($('[data-chuc-nang="sua-bang-diem"]'));
});

//#region Chức năng

function khoiTaoNutCheDoSua($nuts) {
    $nuts.on('click', function () {
        var $nut = $(this);
        var $dsODiem = $('tbody').find('td:not(:last-child)');

        $dsODiem.each(function () {
            var $oDiem = $(this);
            $oDiem.html('<article class="input"><input data-validate="so-thuc" class="diem-input" type="text" data-mac-dinh="' + ($oDiem.attr('data-diem') || '') + '" /></article>');
        });

        khoiTaoLCTForm($_KhungDiem);

        $dsODiem.find('input').on('change', function () {
            var $input = $(this);
            $input.closest('td').attr('data-diem', $input.val());

            var $row = $input.closest('tr');
            var $oDiem = $row.find('td:not(:last-child)');
            var diem = 0;

            $oDiem.each(function () {
                diem += parseFloat($(this).attr('data-diem')) || 0
            });

            diem = (diem / $oDiem.length).toFixed(2);

            alert(diem);
        });
    });
}

//#endregion