$(function () {    
    khoiTaoThemCauHoi($('#tao_cau_hoi_form'));
});

function khoiTaoThemCauHoi($form) {
    khoiTaoLCTForm($form, {
        submit: function () {
            $.ajax({
                url: $form.attr('action'),
                method: $form.attr('method'),
                data: $form.serialize(),
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