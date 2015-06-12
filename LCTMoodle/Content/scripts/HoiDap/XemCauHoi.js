/*
    Khởi tạo
*/
$(function () {
    $form = $('#tra_loi_cau_hoi');

    XuLyTraLoiCauHoi($form);
});

/*
    Xử lý trả lời câu hỏi
*/
function XuLyTraLoiCauHoi($form) {
    khoiTaoLCTForm($form, {
        submit: function () {
            $.ajax({
                url: $form.attr('action'),
                method: $form.attr('method'),
                data: $form.serialize(),
                asyne: false
            }).done(function (data) {
                if (data.trangThai == 0) {
                    $('#danh_sach_tra_loi').append(data.ketQua);
                    $('#thong_bao_chua_co_tra_loi').remove();
                }
                else if (data.trangThai == 3) {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: 'Vui lòng đăng nhập để trả lời',
                        bieuTuong: 'thong-tin'
                    });
                }
                else {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: 'Thêm trả lời thất bại',
                        bieuTuong: 'nguy-hiem'
                    });
                }
            }).fail(function () {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Thêm trả lời thất bại',
                    bieuTuong: 'nguy-hiem'
                });
            })
        }
    });
}
