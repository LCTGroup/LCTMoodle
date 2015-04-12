$cay = null ;

$(function () {
    $cay = $('#cay-chu-de');

    khoiTaoNut();
})

//Khởi tạo các nút xử lý
function khoiTaoNut() {
    //Nút tạo
    $cay.find('a[data-chuc-nang="tao"]').on('click', function () {
        $doiTuong = $(this);

        $.ajax({
            type: 'GET',
            url: '/ChuDe/_Form',
            data: {
                maChuDeCha: $cay.attr('data-ma-hien-tai'),
                phamVi: $cay.attr('data-pham-vi')
            },
            contentType: 'JSON'
        }).done(function (data) {
            if (data.trangThai == 1) {
                $popupFull = $('#popup_full');

                if ($popupFull.length == 0) {
                    $popupFull = $(
                        '<article id="popup_full" class="popup-full">\
                            <section class="khung-tat"></section>\
                            <section id="noi_dung" class="khung-noi-dung">\
                            </section>\
                        </article>');

                    $popupFull.find('.khung-tat').on('click', function () {
                        $popupFull.hide();
                    })

                    $popupFull.on('HoanTat', function (e, data) {
                        $popupFull.hide();
                        console.log('OK');
                        console.log(data);
                    })

                    $('body').prepend($popupFull);
                }

                $popupFull.show();

                $html = $(data.ketQua);

                khoiTaoLCTForm($html.siblings('.lct-form'));

                $html.siblings('.lct-form').on('submit', function (e) {
                    e = e || window.event;
                    e.preventDefault();

                    if (mangTam.hasOwnProperty("DangTaoChuDe") && mangTam['DangTaoChuDe']) {
                        return;
                    }
                    mangTam['DangTaoChuDe'] = true;

                    $form = $(this);

                    $.ajax({
                        url: $form.attr('action'),
                        type: $form.attr('method'),
                        dataType: 'JSON',
                        data: $form.serialize()
                    }).done(function (data) {
                        $popupFull.trigger('HoanTat', data);
                    }).fail(function () {
                        $popupFull.trigger('HoanTat', false);
                    }).always(function () {
                        mangTam['DangTaoChuDe'] = false;
                    });
                });

                $popupFull.find('#noi_dung').html($html);
            }
            else {
                alert('Thất bại');
            }
        }).fail(function () {
            alert('Thất bại');
        });
    });
}