/*
    Khởi tạo
*/
$(function () {
    $form = $('#tra_loi_cau_hoi');

    XuLyTraLoiCauHoi($form);

    KhoiTaoXuLyTraLoi();
});

/*
    Xử lý thêm trả lời theo mã Câu hỏi
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

                    khoiTaoLCTFormMacDinh($form);

                    KhoiTaoXuLyTraLoi();
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

/*
    Xử lý xóa trả lời
*/
function KhoiTaoXuLyTraLoi() {
    khoiTaoTatMoDoiTuong($('.muc-hoi-dap').find('[data-chuc-nang="tat-mo"]'), true);

    $danhSachTraLoi = $('#danh_sach_tra_loi');

    $danhSachTraLoi.find('[data-chuc-nang="xoa-tra-loi"]').on('click', function () {
        $traLoi = $(this).closest('[data-doi-tuong="muc_tra_loi"]');

        moPopup({
            tieuDe: 'Thông báo',
            thongBao: 'Bạn có chắc xóa?',
            bieuTuong: 'hoi',
            nut: [
                {
                    ten: 'Có',
                    xuLy: function () {
                        $.ajax({
                            url: '/HoiDap/XuLyXoaTraLoi/' + $traLoi.attr('data-ma'),
                            type: 'POST',
                            dataType: 'JSON'
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                $traLoi.remove();
                            }
                            else {
                                moPopup({
                                    tieuDe: 'Thông báo',
                                    thongBao: 'Xóa bài viết thất bại',
                                    bieuTuong: 'nguy-hiem'
                                })
                            }
                        }).fail(function () {
                            moPopup({
                                tieuDe: 'Thông báo',
                                thongBao: 'Xóa bài viết thất bại',
                                bieuTuong: 'nguy-hiem'
                            })
                        });
                    }
                },
                {
                    ten: 'Không',
                }
            ]
        })
    });

    $danhSachTraLoi.find('[data-chuc-nang="sua-tra-loi"]').on('click', function () {
        $traLoi = $(this).closest('[data-doi-tuong="muc_tra_loi"]');

        console.log($traLoi.attr('data-ma'));
    });
}