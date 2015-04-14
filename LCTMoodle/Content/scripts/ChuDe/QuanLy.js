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
                $popup = layPopupFull();
                $popup.trigger('Mo');
                $noiDungPopup = $popup.find('#noi_dung');

                $noiDungPopup.html(data.ketQua);
                khoiTaoLCTForm($noiDungPopup.find('.lct-form'));                    
                $noiDungPopup.find('.lct-form').on('submit', function (e) {
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
                        $popup.trigger('Tat');
                    }).fail(function () {
                        $popup.trigger('Tat');
                    }).always(function () {
                        mangTam['DangTaoChuDe'] = false;
                    });
                });
            }
            else {
                alert('Thất bại');
                $popup.trigger('Tat');
            }
        }).fail(function () {
            alert('Thất bại');
            $popup.trigger('Tat');
        });
    });
}