var maCauHoi;

$(function () {
    khoiTaoSuKienCauHoi();
    layTraLoi();
    khoiTaoXuLyFormTraLoi();
})

//{ Khởi tạo sự kiện cho câu hỏi
function khoiTaoSuKienCauHoi() {
    $('.danh-gia > .tang').on('click', function () {
        var $obj = $(this);

        //Nếu đã đánh giá => dừng
        if ($obj.hasClass('da')) {
            return;
        }   

        _ajax(
            '/HoiDap/DanhGiaCauHoiTot/' + maCauHoi,
            null,
            'POST',
            'text',
            function (data) {
                if (data == 'OK') {                    
                    //Lấy điểm
                    var diem = parseInt($obj.siblings('.diem').text());

                    //Kiểm tra xem có đánh dấu xấu không
                    //Nếu có => xóa và tăng điểm thêm 1
                    if ($obj.siblings('.giam').hasClass('da')) {
                        $obj.siblings('.giam').removeClass('da');
                        diem++;
                    }
                    
                    diem++;
                    $obj.addClass('da');
                    $obj.siblings('.diem').text(diem);
                }
                else {
                    thongBaoLoi(data.trangThai);
                }
            },
            thongBaoLoi,
            '',
            'Câu hỏi - Đánh giá câu hỏi tốt thất bại'
        );
    });
    $('.danh-gia > .giam').on('click', function () {
        var $obj = $(this);

        //Nếu đã đánh giá => dừng
        if ($obj.hasClass('da')) {
            return;
        }

        _ajax(
            '/HoiDap/DanhGiaCauHoiXau/' + maCauHoi,
            null,
            'POST',
            'text',
            function (data) {
                if (data == 'OK') {
                    if (data == 'OK') {
                        //Lấy điểm
                        var diem = parseInt($obj.siblings('.diem').text());

                        //Kiểm tra xem có đánh dấu xấu không
                        //Nếu có => xóa và giảm điểm thêm 1
                        if ($obj.siblings('.tang').hasClass('da')) {
                            $obj.siblings('.tang').removeClass('da');
                            diem--;
                        }

                        diem--;
                        $obj.addClass('da');
                        $obj.siblings('.diem').text(diem);
                    }
                    else {
                        thongBaoLoi(data.trangThai);
                    }
                }
                else {
                    thongBaoLoi(data.trangThai);
                }
            },
            thongBaoLoi,
            '',
            'Câu hỏi - Đánh giá câu hỏi xấu thất bại'
        );
    });
}
//} Khởi tạo sự kiện cho câu hỏi

//{ Khởi tạo xử lý form
function khoiTaoXuLyFormTraLoi() {
    var $form = $('#khung-soan-tra-loi');

    $form.on('submit', function (e) {
        kiemTraDangCauHoi($form, e);
    })
}

function kiemTraDangCauHoi($form, e) {
    e = e || window.event;
    e.preventDefault();

    if ($form.find('.f-placeholder').length > 0) {
        alert('Nhập nội dung');
        return;
    }

    _ajaxForm(
        '/TraLoi/CauHoi_DangTraLoi',
        new FormData($form[0]),
        'POST',
        'json',
        'application/x-www-form-urlencoded',
        false,
        false,
        false,
        function (data) {
            if (data.trangThai == 'OK') {
                dangTraLoiThanhCong(data.ketQua);
            }
            else {
                thongBaoLoi(data.trangThai);
            }
        },
        thongBaoLoi,
        '',
        'Câu hỏi - Đăng trả lời thất bại'
    );

    $form[0].reset();
    $('#viet-bai').editable("setHTML", "");
}

function dangTraLoiThanhCong(html) {
    $('#danh-sach-tra-loi').append(khoiTaoTraLoi(html));
}
//} Khởi tạo xử lý form


//{ Lấy trả lời
function layTraLoi() {
    _ajax(
        '/TraLoi/CauHoi_LayTraLoi',
        { ma: maCauHoi, tu: 0, den: -1},
        'POST',
        'JSON',
        function (data) {
            if (data.trangThai == 'OK') {
                $('#danh-sach-tra-loi').append(khoiTaoTraLoi(data.ketQua));
            }
            else {
                thongBaoLoi(data.trangThai);
            }
        },
        thongBaoLoi,
        '',
        'Câu hỏi - Lấy danh sách trả lời thất bại'
    );
}

function khoiTaoTraLoi($html) {
    $html = $($html);

    $html.find('.xoa-tra-loi').on('click', function (e) {
        e = e || window.event;
        e.preventDefault();

        if (!confirm('Bạn có chắc là muốn xóa?')) {
            return;
        }

        _ajax(
            e.target,
            null,
            'POST',
            'text',
            function (data) {
                if (data == 'OK') {
                    $(e.target).parents('.muc-tra-loi').remove();

                    //Kiểm tra xem có tồn tại trả lời nào đang duyệt không
                    //Nếu không => thay đổi trạng thái
                    if ($('#danh-sach-tra-loi .da-duyet').length == 0) {
                        $('.phan-cau-hoi .thong-tin-bai').removeClass('da-tra-loi').addClass('chua-tra-loi');
                    }
                }
                else {
                    thongBaoLoi(data);
                }
            },
            thongBaoLoi,
            '',
            'Câu hỏi - Xóa trả lời thất bại'
        );
    });

    $html.find('.danh-gia > .tang').on('click', function () {
        var $obj = $(this);

        //Nếu đã đánh giá => dừng
        if ($obj.hasClass('da')) {
            return;
        }

        _ajax(
            '/TraLoi/DanhGiaTraLoiTot/' + $obj[0].value,
            null,
            'POST',
            'text',
            function (data) {
                if (data == 'OK') {
                    //Lấy điểm
                    var diem = parseInt($obj.siblings('.diem').text());

                    //Kiểm tra xem có đánh dấu xấu không
                    //Nếu có => xóa và tăng điểm thêm 1
                    if ($obj.siblings('.giam').hasClass('da')) {
                        $obj.siblings('.giam').removeClass('da');
                        diem++;
                    }

                    diem++;
                    $obj.addClass('da');
                    $obj.siblings('.diem').text(diem);
                }
                else {
                    thongBaoLoi(data.trangThai);
                }
            },
            thongBaoLoi,
            '',
            'Câu hỏi - Đánh giá trả lời tốt thất bại'
        );
    });

    $html.find('.danh-gia > .giam').on('click', function () {
        var $obj = $(this);

        //Nếu đã đánh giá => dừng
        if ($obj.hasClass('da')) {
            return;
        }

        _ajax(
            '/TraLoi/DanhGiaTraLoiXau/' + $obj[0].value,
            null,
            'POST',
            'text',
            function (data) {
                if (data == 'OK') {
                    if (data == 'OK') {
                        //Lấy điểm
                        var diem = parseInt($obj.siblings('.diem').text());

                        //Kiểm tra xem có đánh dấu xấu không
                        //Nếu có => xóa và giảm điểm thêm 1
                        if ($obj.siblings('.tang').hasClass('da')) {
                            $obj.siblings('.tang').removeClass('da');
                            diem--;
                        }

                        diem--;
                        $obj.addClass('da');
                        $obj.siblings('.diem').text(diem);
                    }
                    else {
                        thongBaoLoi(data.trangThai);
                    }
                }
                else {
                    thongBaoLoi(data.trangThai);
                }
            },
            thongBaoLoi,
            '',
            'Câu hỏi - Đánh giá trả lời xấu thất bại'
        );
    });

    $html.find('.duyet-tra-loi').on('click', function (e) {
        e = e || window.event;
        e.preventDefault();

        _ajax(
            e.target,
            null,
            'POST',
            'text',
            function (data) {
                if (data == 'OK') {
                    $(e.target).parents('.muc-tra-loi').removeClass('chua-duyet').addClass('da-duyet');
                    $('.phan-cau-hoi .thong-tin-bai').removeClass('chua-tra-loi').addClass('da-tra-loi');
                }
                else {
                    thongBaoLoi(data);
                }
            },
            thongBaoLoi,
            '',
            'Câu hỏi - Duyệt trả lời thất bại'
        );
    });

    $html.find('.bo-duyet-tra-loi').on('click', function (e) {
        e = e || window.event;
        e.preventDefault();

        _ajax(
            e.target,
            null,
            'POST',
            'text',
            function (data) {
                if (data == 'OK') {
                    $(e.target).parents('.muc-tra-loi').removeClass('da-duyet').addClass('chua-duyet');

                    //Kiểm tra xem còn trả lời nào đã được duyệt hay không
                    //Nếu không => bỏ class
                    if ($('#danh-sach-tra-loi .da-duyet').length == 0) {
                        $('.phan-cau-hoi .thong-tin-bai').removeClass('da-tra-loi').addClass('chua-tra-loi');
                    }
                }
                else {
                    thongBaoLoi(data);
                }
            },
            thongBaoLoi,
            '',
            'Câu hỏi - Bỏ duyệt trả lời thất bại'
        );
    });

    return $html;
}
//} Lấy trả lời