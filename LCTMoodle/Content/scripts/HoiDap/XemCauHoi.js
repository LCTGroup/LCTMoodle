var $diem;
var $diemSo = $('#diem_so');
var $khungDiemCauHoi;
var $nutCongDiem;
var $nutTruDiem;

//#region Khởi tạo

$(function () {
    //Khởi tạo chức năng trả lời câu hỏi
    $formTraLoi = $('[data-doi-tuong="form-tra-loi"]');
    khoiTaoChucNangTraLoi($formTraLoi);

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
                url: '/HoiDap/XuLyThemTraLoi',
                method: 'POST',
                data: layDataLCTForm($form)
            }).done(function (data) {
                if (data.trangThai == 0) {
                    var $traLoiMoi = $(data.ketQua);
                    $('#danh_sach_tra_loi').append($traLoiMoi);

                    khoiTaoLCTFormMacDinh($form);
                    khoiTaoTraLoi($traLoiMoi);
                }
                else {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: data.ketQua,
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
    var maCauHoi = $cauHoi.attr('data-ma');
    $khungDiemCauHoi = $cauHoi.find('[data-doi-tuong="diem-so-' + maCauHoi + '"]');
    var voteDiem = $khungDiemCauHoi.attr('data-nguoi-dung-vote-diem');

    khoiTaoVoteDiem(maCauHoi, voteDiem);

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

    $cauHoi.find('[data-chuc-nang="sua-cau-hoi"]').on('click', function () {
        var ma = $cauHoi.attr('data-ma');
        $.ajax({
            url: '/HoiDap/_Form_CauHoi/' + ma,
            method: 'POST',
            dataType: 'JSON'
        }).done(function (data) {
            if (data.trangThai == 0) {
                $formSuaCauHoi = $(data.ketQua);

                //Lưu câu hỏi trước khi sửa
                mangTam['CauHoi' + ma] = $cauHoi.html();

                $cauHoi.html($formSuaCauHoi);
                khoiTaoLCTForm($formSuaCauHoi, {
                    submit: function () {
                        $.ajax({
                            url: '/HoiDap/XuLyCapNhatCauHoi/',
                            method: 'POST',
                            dataType: 'JSON',
                            data: layDataLCTForm($formSuaCauHoi)
                        }).done(function (data) {                            
                            if (data.trangThai == 0) {                                
                                $cauHoi.html($(data.ketQua).html());
                                khoiTaoCauHoi($cauHoi);
                            }
                            else {
                                moPopup({
                                    tieuDe: 'Thông báo',
                                    noiDung: 'Cập nhật không thành công',
                                    bieuTuong: 'nguy-hiem'
                                })
                            }
                        });
                    },
                    custom: [{
                        input: $formSuaCauHoi.find('[data-chuc-nang="huy"]'),
                        event: {
                            'click': function () {                                
                                $cauHoi.html(mangTam['CauHoi' + ma]);
                                khoiTaoCauHoi($cauHoi);
                            }
                        }
                    }]
                });
            }
            else {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Lỗi lấy câu hỏi',
                    bieuTuong: 'nguy-hiem'
                });
            }
        });
    });

    $cauHoi.find('[data-chuc-nang="cong-diem"]').on('click', function () {
        $.ajax({
            url: '/HoiDap/XuLyDiemCauHoi',
            data: { maCauHoi: maCauHoi, diem: true }
        }).done(function (data) {
            if (data.trangThai == 0) {
                capNhatDiem(data.ketQua);
                capNhatVoteDiem(1);
            }
            else if (data.trangThai == 4)
            {
                moPopupDangNhap();
            }
            else {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: data.ketQua
                });
            }
        });
    });

    $cauHoi.find('[data-chuc-nang="tru-diem"]').on('click', function () {
        $.ajax({
            url: '/HoiDap/XuLyDiemCauHoi',
            data: { maCauHoi: maCauHoi, diem: false }
        }).done(function (data) {
            if (data.trangThai == 0) {
                capNhatDiem(data.ketQua);
                capNhatVoteDiem(0);
            }
            else if (data.trangThai == 4) {
                moPopupDangNhap();
            }
            else {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: data.ketQua
                });
            }
        });
    });
}

//#endregion

//#region Khởi tạo Trả Lời

function khoiTaoTraLoi($danhSachTraLoi) {
    khoiTaoTatMoDoiTuong($danhSachTraLoi.find('[data-chuc-nang="tat-mo"]'), true);

    $danhSachTraLoi.find('[data-chuc-nang="xoa-tra-loi"]').on('click', function () {
        var $traLoi = $(this).closest('[data-doi-tuong="tra-loi"]');

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
        var $traLoi = $(this).closest('[data-doi-tuong="tra-loi"]');
        var ma = $traLoi.attr('data-ma');

        $.ajax({
            url: '/HoiDap/_Form_TraLoi/' + ma,
            method: 'POST',
            dataType: 'JSON'
        }).done(function (data) {
            if (data.trangThai == 0) {
                var $formSuaTraLoi = $(data.ketQua);

                //Lưu _item_TraLoi vào biến tạm để phục vụ chức năng Hủy
                mangTam['TraLoi' + ma] = $traLoi.html();

                $traLoi.html($formSuaTraLoi);
                
                khoiTaoLCTForm($formSuaTraLoi, {
                    submit: function () {
                        $.ajax({
                            url: '/HoiDap/XuLyCapNhatTraLoi',
                            method: 'POST',
                            dataType: 'JSON',
                            data: layDataLCTForm($formSuaTraLoi)
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                $ketQua = $(data.ketQua).html();
                                //Hiển thị kết quả
                                $traLoi.html($ketQua);
                                //Khởi tạo lại _item_TraLoi
                                khoiTaoTraLoi($traLoi);
                            }
                            else {
                                moPopup({
                                    tieuDe: 'Thông báo',
                                    thongBao: 'Cập nhật trả lời thất bại',
                                    bieuTuong: 'nguy-hiem'
                                })
                            }
                        });
                    },
                    custom: [
                        {
                            input: $traLoi.find('[data-chuc-nang="huy"]'),
                            event: {
                                'click': function () {
                                    $traLoi.html(mangTam['TraLoi' + ma]);
                                    khoiTaoTraLoi($traLoi);
                                }
                            }
                        }
                    ]
                });
            }
            else {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Lấy trả lời lỗi',
                    bieuTuong: 'nguy-hiem'
                })
            }
        });
    });

    $danhSachTraLoi.find('[data-chuc-nang="duyet-tra-loi"]').on('click', function () {
        var $traLoi = $(this).closest('[data-doi-tuong="tra-loi"]');        
        var ma = $traLoi.attr('data-ma');

        $.ajax({
            url: '/HoiDap/XuLyDuyetTraLoi/',
            method: 'POST',
            data: { maTraLoi: ma, trangThaiDuyet: $traLoi.hasClass('binh-thuong') ? true : false }
        }).done(function (data) {
            if (data.trangThai == 0) {
                $traLoi.toggleClass('binh-thuong');
                $traLoi.toggleClass('duyet');
            }
            else {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Lỗi duyệt trả lời'
                });
            }
        });
    });

}

//#endregion

//#region Hàm khác

function khoiTaoVoteDiem(maCauHoi, diemVote) {
    $nutCongDiem = $khungDiemCauHoi.find('[data-chuc-nang="cong-diem"]');
    $nutTruDiem = $khungDiemCauHoi.find('[data-chuc-nang="tru-diem"]');

    switch(diemVote)
    {
        case "1":
            {
                $nutCongDiem.hide();
            }
            break;
        case "2":
            {
                $nutTruDiem.hide();
            }
            break;
        default: break;
    }
}

//vote = 1: nút cộng | vote = 0: nút trừ
function capNhatVoteDiem(vote)
{
    if (vote == 1) {
        if ($nutTruDiem.is(':visible')) {
            $nutCongDiem.hide();
        }
        else {
            $nutTruDiem.show();
        }
    }
    else {
        if ($nutCongDiem.is(':visible')) {
            $nutTruDiem.hide();
        }
        else {
            $nutCongDiem.show();
        }
    }
}

function capNhatDiem(soDiem) {
    $diemSo.text(soDiem);
}

//#endregion
