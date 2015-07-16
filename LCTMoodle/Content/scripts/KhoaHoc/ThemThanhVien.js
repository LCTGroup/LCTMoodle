var $_khung, $_khungChuaXacNhan;

$(function () {
    $_khung = $('#khung_them');
    $_khungChuaXacNhan = $('#khung_chua_xac_nhan');
    
    khoiTaoInputFile($_khung.find('.lct-form'));
});

function khoiTaoInputFile($form) {
    khoiTaoLCTForm($form, {
        custom: [
            {
                input: $form.find('[type="file"]'),
                event: {
                    valueChange: function () {
                        var ma = $(this).val();

                        if (ma) {
                            var $tai = moBieuTuongTai($_khung);
                            $.ajax({
                                url: '/NguoiDung/_DanhSachXacNhanThem',
                                method: 'POST',
                                data: { maTapTin: ma },
                                dataType: 'JSON'
                            }).always(function () {
                                $tai.tat()
                            }).done(function (data) {
                                if (data.trangThai == 0) {
                                    var $khungXacNhan = $(data.ketQua);
                                    khoiTaoKhungDanhSachXacNhanThem($khungXacNhan, '/KhoaHoc/XuLyThemDanhSachThanhVien')
                                    $_khungChuaXacNhan.html($khungXacNhan);
                                }
                                else {
                                    moPopupThongBao(data);
                                }
                            }).fail(function () {
                                moPopupThongBao('Mở tập tin thất bại');
                            })
                        }
                    }
                }
            }
        ]
    });
}