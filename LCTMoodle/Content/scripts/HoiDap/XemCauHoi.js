//#region Khởi tạo

$(function () {
    //Khởi tạo chức năng trả lời câu hỏi
    $form = $('.lct-form');
    khoiTaoChucNangTraLoi($form);

    //Khởi tạo Câu Hỏi
    $cauHoi = $('[data-doi-tuong="cau-hoi"]');
    khoiTaoCauHoi($cauHoi);
    
    //Khởi tạo Danh sách Trả Lời
    $danhSachTraLoi = $('#danh_sach_tra_loi');
    khoiTaoTraLoi($danhSachTraLoi);
});

//#endregion

/*#region Khởi tạo chức năng trả lời*/

function khoiTaoChucNangTraLoi($form) {
    khoiTaoLCTForm($form, {
        submit: function () {
            $.ajax({
                url: $form.attr('action'),
                method: $form.attr('method'),
                data: $form.serialize(),
                asyne: false
            }).done(function (data) {
                if (data.trangThai == 0) {
                    $traLoiMoi = $(data.ketQua);
                    $('#danh_sach_tra_loi').append($traLoiMoi);

                    khoiTaoTraLoi($traLoiMoi);
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

/*#endregion*/

//#region Khởi tạo Câu hỏi

function khoiTaoCauHoi($cauHoi) {
    khoiTaoTatMoDoiTuong($cauHoi.find('[data-chuc-nang="tat-mo"]'), true);    

    $cauHoi.find('[data-chuc-nang="xoa-cau-hoi"]').on('click', function () {
        moPopup({
            tieuDe: 'Thông báo',
            thongBao: 'Bạn có chắc xóa?',
            bieuTuong: 'hoi',
            nut: [
                {
                    ten: 'Có',
                    xuLy: function () {
                        $.ajax({
                            url: '/HoiDap/XuLyXoaCauHoi/' + $cauHoi.attr('data-ma'),
                            type: 'POST',
                            dataType: 'JSON'
                        }).done(function (data) {
                            if (data.trangThai != 0) {
                                moPopup({
                                    tieuDe: 'Thông báo',
                                    thongBao: 'Xóa câu hỏi thất bại',
                                    bieuTuong: 'nguy-hiem'
                                })
                            }
                            else {
                                window.location = "/HoiDap/";
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

}

//#endregion

//#region Khởi tạo Trả Lời

function khoiTaoTraLoi($danhSachTraLoi) {
    khoiTaoTatMoDoiTuong($danhSachTraLoi.find('[data-chuc-nang="tat-mo"]'), true);

    $danhSachTraLoi.find('[data-chuc-nang="xoa-tra-loi"]').on('click', function () {
        $traLoi = $(this).closest('[data-doi-tuong="tra-loi"]');
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
                                thongBao: 'Xóa trả lời thất bại',
                                bieuTuong: 'nguy-hiem'
                            })
                        });
                    }
                },
                {
                    ten: 'Không',
                }
            ]
        });
    });

    $danhSachTraLoi.find('[data-chuc-nang="sua-tra-loi"]').on('click', function () {
        $traLoi = $(this).closest('[data-doi-tuong="muc_tra_loi"]');

        console.log($traLoi.attr('data-ma'));
    });
}

//#endregion
