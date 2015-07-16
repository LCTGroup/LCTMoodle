var $_dsCauHoi;

//#region Khởi tạo

$(function () {
    $_dsCauHoi = $('[data-doi-tuong="item-cau-hoi"]');

    khoiTaoQuanLyCauHoi($_dsCauHoi);
});

//#endregion

//#region Khởi tạo quản lý câu hỏi

function khoiTaoQuanLyCauHoi($dsCauHoi) {

    $dsCauHoi.find('[data-chuc-nang="duyet"]').on('click', function () {
        var $itemCauHoi = $(this).closest('[data-doi-tuong="item-cau-hoi"]');
        var $maCauHoi = $itemCauHoi.attr('data-ma');
        var $tai = moBieuTuongTai($itemCauHoi);

        $.ajax({
            url: '/HoiDap/XuLyDuyetHienThiCauHoi',
            method: 'POST',
            data: { maCauHoi: $maCauHoi, trangThai: true }
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai == 0) {
                $itemCauHoi.remove();
            }
            else {
                moPopupThongBao(data);
            }
        }).fail(function () {
            moPopupThongBao("Lỗi ajax");
        });
    });

    $dsCauHoi.find('[data-chuc-nang="xoa"]').on('click', function () {
        var $itemCauHoi = $(this).closest('[data-doi-tuong="item-cau-hoi"]');
        var $maCauHoi = $itemCauHoi.attr('data-ma');
        var $tai = moBieuTuongTai($itemCauHoi);

        $.ajax({
            url: '/HoiDap/XuLyXoaCauHoi',
            method: 'POST',
            data: { ma: $maCauHoi }
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai == 0) {
                $itemCauHoi.remove();
            } else {
                moPopupThongBao(data);
            }
        }).fail(function () {
            moPopupThongBao("Lỗi ajax");
        });
    });

    $dsCauHoi.find('[data-chuc-nang="xem"]').on('click', function () {
        var $itemCauHoi = $(this).closest('[data-doi-tuong="item-cau-hoi"]');
        var $maCauHoi = $itemCauHoi.attr('data-ma');
        var $tai = moBieuTuongTai($itemCauHoi);

        $.ajax({
            url: '/HoiDap/XemMoPhongCauHoi',
            method: 'POST',
            data: { ma: $maCauHoi }
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai == 0) {
                moPopupFull({
                    html: data.ketQua                    
                });
            }
            else {
                moPopupThongBao(data);
            }
        })
    });

}

//#region