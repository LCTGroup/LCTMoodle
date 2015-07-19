//#region Khởi Tạo

$(function () {
    $form = $('#them_tin_nhan');   

    khoiTaoThemTinNhan($form);
});

//#endregion

//#region Thêm tin nhắn

function khoiTaoThemTinNhan($form)
{
    var $khungChiTietTinNhan = $('.khung-chi-tiet-tin-nhan');

    khoiTaoLCTForm($form, {
        submit: function () {
            var $tai = moBieuTuongTai($form);

            $.ajax({
                url: '/NguoiDung/XuLyThemTinNhan',
                method: 'POST',
                data: layDataLCTForm($form)
            }).always(function () {
                $tai.tat();
            }).done(function (data) {
                if (data.trangThai == 0) {
                    $khungChiTietTinNhan.append(data.ketQua);
                    $khungChiTietTinNhan.find('.empty').remove();

                    khoiTaoLCTFormMacDinh($form);
                }
                else {
                    moPopupThongBao(data);
                }
            }).fail(function () {
                moPopupThongBao(data);
            })
        }
    });
}

//#endregion

