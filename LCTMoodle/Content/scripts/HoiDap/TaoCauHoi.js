//#region Khởi tạo

$(function () {
    $form = $('[data-doi-tuong="form-cau-hoi"]');

    khoiTaoThemCauHoi($form);
});

//#endregion

//#region Tạo câu hỏi

function khoiTaoThemCauHoi($form) {
    khoiTaoLCTForm($form, {
        submit: function () {
            $.ajax({
                url: '/HoiDap/XuLyThemCauHoi',
                method: 'POST',
                data: layDataLCTForm($form),
                dataType: 'JSON',
                async: false
            }).done(function (data) {
                if (data.trangThai == 0) {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: 'Tạo câu hỏi thành công',
                        nut: [{
                            ten: 'Về danh sách',
                            href: '/HoiDap/'
                        }],
                        esc: false,
                        bieuTuong: 'thanh-cong'
                    });
                } else {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: 'Tạo câu hỏi thất bại',
                        bieuTuong: 'nguy-hiem'
                    });
                }
            }).fail(function () {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Lỗi xử lý Server',
                    bieuTuong: 'nguy-hiem'
                });
            })
        }
    });
}

//#endregion