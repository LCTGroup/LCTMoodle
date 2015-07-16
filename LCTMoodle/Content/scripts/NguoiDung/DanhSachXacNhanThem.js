var $_khungXacNhan, $_khungNoiDung;

$(function () {
    $_khungXacNhan = $('#khung_xac_nhan');
    $_khungNoiDung = $_khungXacNhan.find('tbody');

    capNhatThuTu();

    khoiTaoTatMoDoiTuong($_khungNoiDung.find('[data-chuc-nang="tat-mo"]'), true);

    $_khungNoiDung.find('[data-chuc-nang="bo-qua"]').on('click', function () {
        var $nut = $(this);
        var $dong = $nut.closest('tr');

        if ($nut.data('value') == 1) {
            $nut.data('value', 0);
            $dong.attr('data-thiet-lap', 'b');

            $nut.text('Mặc định').data('value', 0);

            $dong.find('[data-chuc-nang="lay-theo-tai-khoan"]').text('Lấy theo tài khoản').data('value', 1);
            $dong.find('[data-chuc-nang="lay-theo-email"]').text('Lấy theo email').data('value', 1);
        }
        else {
            $dong.removeAttr('data-thiet-lap');

            $nut.text('Bỏ qua').data('value', 1);
        }
    });

    $_khungNoiDung.find('[data-chuc-nang="lay-theo-tai-khoan"]').on('click', function () {
        var $nut = $(this);
        var $dong = $nut.closest('tr');

        if ($nut.data('value') == 1) {
            $nut.data('value', 0);
            $dong.attr('data-thiet-lap', 'tk');

            $nut.text('Mặc định').data('value', 0);

            $dong.find('[data-chuc-nang="bo-qua"]').text('Bỏ qua').data('value', 1);
            $dong.find('[data-chuc-nang="lay-theo-email"]').text('Lấy theo email').data('value', 1);
        }
        else {
            $dong.removeAttr('data-thiet-lap');

            $nut.text('Lấy theo tài khoản').data('value', 1);
        }
    });

    $_khungNoiDung.find('[data-chuc-nang="lay-theo-email"]').on('click', function () {
        var $nut = $(this);
        var $dong = $nut.closest('tr');

        if ($nut.data('value') == 1) {
            $nut.data('value', 0);
            $dong.attr('data-thiet-lap', 'e');

            $nut.text('Mặc định').data('value', 0);

            $dong.find('[data-chuc-nang="lay-theo-tai-khoan"]').text('Lấy theo tài khoản').data('value', 1);
            $dong.find('[data-chuc-nang="bo-qua"]').text('Bỏ qua').data('value', 1);
        }
        else {
            $dong.removeAttr('data-thiet-lap');

            $nut.text('Lấy theo email').data('value', 1);
        }
    });

    $_khungXacNhan.find('[data-chuc-nang="hoan-tat"]').on('click', function () {
        var $loi = $_khungNoiDung.find('tr:not([data-thiet-lap]):has(.loi)');

        if ($loi.length > 0) {
            moPopup({
                tieuDe: 'Xác nhận',
                thongBao: 'Có ' + $loi.length + ' người dùng bị lỗi nhưng chưa xử lý. Bạn có muốn quay lại kiểm tra hay bỏ qua các người dùng đó?',
                nut: [
                    {
                        ten: 'Bỏ qua',
                        xuLy: function () {
                            xuLyThem();
                        }
                    }, {
                        ten: 'Quay lại'
                    }
                ]
            })
        }
        else {
            xuLyThem();
        }
    });
});

//#region Chức năng

function xuLyThem() {
    var $dsBinhThuong = $_khungNoiDung.find('tr:not([data-thiet-lap]):not(:has(.loi))'),
        $dsTheoTaiKhoan = $_khungNoiDung.find('tr[data-thiet-lap="tk"]'),
        $dsTheoEmail = $_khungNoiDung.find('tr[data-thiet-lap="e"]');
    
    var dsBinhThuong = [],
        dsTaiKhoan = [],
        dsEmail = [];

    var $dong, dong, $o;
    $dsBinhThuong.each(function () {
        $dong = $(this);
        dong = { ma: $dong.find('[data-doi-tuong="dem"]').text() };
        $dong.find('[data-name]').each(function () {
            $o = $(this);
            dong[$o.attr('data-name')] = $o.text();
        });

        dsBinhThuong.push(dong)
    });

    $dsTheoTaiKhoan.each(function () {
        $dong = $(this);
        dsTaiKhoan.push({
            ma: $dong.find('[data-doi-tuong="dem"]').text(),
            tenTaiKhoan: $dong.find('[data-name="tenTaiKhoan"]').text()
        });
    });

    $dsTheoEmail.each(function () {
        $dong = $(this);
        dsTaiKhoan.push({
            ma: $dong.find('[data-doi-tuong="dem"]').text(),
            email: $dong.find('[data-name="email"]').text()
        });
    });

    console.log({
        dsBinhThuong: dsBinhThuong,
        dsTaiKhoan: dsTaiKhoan,
        dsEmail: dsEmail
    });
    var $tai = moBieuTuongTai($_khungXacNhan);
    $.ajax({
        url: '/NguoiDung/XuLyThemDanhSach',
        method: 'POST',
        data: {
            dsBinhThuong: dsBinhThuong,
            dsTaiKhoan: dsTaiKhoan,
            dsEmail: dsEmail
        },
        dataType: 'JSON'
    }).always(function () {
        $tai.tat();
    }).done(function (data) {
        if (data.trangThai == 0) {
            alert('ok');
        }
        else {
            moPopupThongBao(data);
        }
    }).fail(function () {
        moPopupThongBao('Thêm thất bại');
    });
}

//#endregion

//#region Hỗ trợ

function capNhatThuTu() {
    $_khungNoiDung.find('tr').each(function (index) {
        $(this).find('td:eq(0) [data-doi-tuong="dem"]').text(index + 1);
    });
}

//#endregion