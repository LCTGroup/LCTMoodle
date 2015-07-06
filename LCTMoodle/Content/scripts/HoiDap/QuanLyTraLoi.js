var $_dsTraLoi;

//#region Khởi tạo

$(function () {
    $_dsTraLoi = $('[data-doi-tuong="item-tra-loi"]');

    khoiTaoQuanLyCauHoi($_dsTraLoi);
});

//#endregion

//#region Khởi tạo quản lý câu hỏi

function khoiTaoQuanLyCauHoi($dsTraLoi) {

    $dsTraLoi.find('[data-chuc-nang="duyet"]').on('click', function () {
        var $itemTraLoi = $(this).closest('[data-doi-tuong="item-tra-loi"]');
        var $maTraLoi = $itemTraLoi.attr('data-ma');
        var $tai = moBieuTuongTai($itemTraLoi);

        $.ajax({
            url: '/HoiDap/XuLyDuyetHienThiTraLoi',
            method: 'POST',
            data: { maTraLoi: $maTraLoi, trangThai: true }
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai == 0) {
                $itemTraLoi.remove();
            }
            else {
                moPopupThongBao(data);
            }
        }).fail(function () {
            moPopupThongBao("Lỗi ajax");
        });
    });

    $dsTraLoi.find('[data-chuc-nang="xoa"]').on('click', function () {
        var $itemTraLoi = $(this).closest('[data-doi-tuong="item-tra-loi"]');
        var $maTraLoi = $itemTraLoi.attr('data-ma');
        var $tai = moBieuTuongTai($itemTraLoi);

        $.ajax({
            url: '/HoiDap/XuLyXoaTraLoi',
            method: 'POST',
            data: { ma: $maTraLoi }
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai == 0) {
                $itemTraLoi.remove();
            } else {
                moPopupThongBao(data);
            }
        }).fail(function () {
            moPopupThongBao("Lỗi ajax");
        });
    });

    $dsTraLoi.find('[data-chuc-nang="xem"]').on('click', function () {
        var $itemTraLoi = $(this).closest('[data-doi-tuong="item-tra-loi"]');
        var $maTraLoi = $itemTraLoi.attr('data-ma');
        var $tai = moBieuTuongTai($itemTraLoi);

        $.ajax({
            url: '/HoiDap/XemChiTietTraLoi',
            method: 'POST',
            data: { maTraLoi: $maTraLoi }
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai == 0) {
                moPopupFull({
                    html: data.ketQua
                })
            }
            else {
                moPopupThongBao(data);
            }
        })
       
    });
}

//#region