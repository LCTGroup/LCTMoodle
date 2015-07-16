var $_khung, $_khungChuaXacNhan;

$(function () {
    $_khung = $('#khung_them');
    $_khungChuaXacNhan = $_khung.find('#khung_xac_nhan');

    khoiTaoInputFile($_khung.find('.lct-form'));
});

function khoiTaoInputFile($form) {
    khoiTaoLCTForm($form, {
        custom: [
            {
                input: $form.find('[type="file"]'),
                event: {
                    change: function () {
                        var ma = $(this).next().val();

                        if (ma) {
                            $.ajax({
                                url: '/NguoiDung/_DanhSachXacNhanThem',
                                method: 'POST',
                                data: { maTapTin: ma },
                                dataType: 'JSON'
                            }).done()
                        }
                    }
                }
            }
        ]
    });
}