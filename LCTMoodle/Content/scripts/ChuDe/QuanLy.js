var $_Khung, $_Cay, $_DanhSach;

//Xác định bằng PhamVi, <Mã phạm vi> (KhoaHoc, HoiDap, ...), <Mã chủ đề>
var mangNut = {};

//Khởi tạo
$(function () {
    $_Khung = $('#khung_quan_ly');
    $_Cay = $('#khung_cay');
    $_DanhSach = $('#khung_danh_sach');

    khoiTao_MoNut($_Khung.find('[data-chuc-nang="mo-nut"]'));
})

//#region Xử lý nút nhấn

/*
    Danh sách loại
*/
/*
    Các loại nút ở cây
        cay-he-thong
        cay-pham-vi
        cay-nut

        cay-con-pham-vi
        cay-con-nut

    Các loại nút ở danh sách
        ds-pham-vi
        ds-nut
*/

function khoiTao_MoNut($nutMo) {
    $nutMo.on('click', function () {
        moNut(this);
    });
}

var a = b = 0;
function moNut(obj) {
    //Kiểm tra xem có đang mở nút khác ko
    if ('dangMoNut' in mangTam && mangTam['dangMoNut']) {
        return;
    }
    mangTam['dangMoNut'] = true;

    var
        $muc = $(obj).closest('[data-doi-tuong="muc"]'),
        loai = $muc.attr('data-loai'),
        ma = $muc.attr('data-ma');
        
    //#region Lấy dữ liệu
    if (!(ma in mangNut)) {
        //#region Ajax lấy dữ liệu
        //Lấy tham số gửi đi
        data = null;
        switch (loai) {
            case 'cay-he-thong':
            case 'cay-pham-vi':
            case 'cay-con-pham-vi':
            case 'ds-pham-vi':
                data = {
                    phamVi: ma
                };
                break;
            case 'cay-nut':
            case 'cay-con-nut':
            case 'ds-nut':
                data = {
                    maChuDeCha: ma,
                    phamVi: $_Khung.attr('data-pham-vi')
                }
                break;
            default:
                return;
        }

        //Lấy dữ liệu
        //Để xác định lấy dữ liệu thành công hay thất bại
        var thanhCong;

        $.ajax({
            url: '/ChuDe/_Khung',
            data: data,
            contentType: 'JSON',
            async: false
        }).done(function (data) {
            if (data.trangThai == 0 || data.trangThai == 1) {
                //Lưu lại kết quả lấy được
                mangNut[ma] = data.ketQua;

                thanhCong = true;
            }
            else {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Mở nút thất bại',
                    bieuTuong: 'nguy-hiem'
                });

                thanhCong = false;
            }
        }).fail(function () {
            moPopup({
                tieuDe: 'Thông báo',
                thongBao: 'Mở nút thất bại',
                bieuTuong: 'nguy-hiem'
            });
            thanhCong = false;
        });

        if (!thanhCong) {
            return;
        }
        //#endregion
    }

    var duLieuNutCon = mangNut[ma],
        $cay = $(duLieuNutCon.cay),
        $danhSach = $(duLieuNutCon.danhSach);

    khoiTao_MoNut($cay.find('[data-chuc-nang="mo-nut"]'));
    khoiTao_MoNut($danhSach.find('[data-chuc-nang="mo-nut"]'));
    //#endregion

    //#region Hiển thị
    //Hiển thị cây
    switch (loai) {
        case 'cay-he-thong':
        case 'cay-pham-vi':
        case 'cay-nut':
            $muc.find('~ *').remove();
            break;
        case 'cay-con-pham-vi':
        case 'cay-con-nut':
            var $mucCha = $muc.parent().closest('[data-doi-tuong="muc"]');
            $mucCha.find('~ *').remove();
            $mucCha.after($cay)
            break;
        case 'ds-pham-vi':
        case 'ds-nut':
            $_Cay.append($cay);
            break;
        default:
            return;
    }

    //Điều chỉnh hiển thị cây con
    var $danhSachMuc = $_Cay.find(':nth-last-child(2) [data-doi-tuong="danh-sach-muc"]');
    $danhSachMuc.prepend($danhSachMuc.children('[data-ma="' + ma + '"]'));

    //Hiển thị danh sách
    if ($danhSach.length) {
        $_DanhSach.removeClass('rong');

        $_DanhSach.html($danhSach);
    }
    else {
        $_DanhSach.addClass('rong');
        $_DanhSach.html('');
    }

    //#endregion

    //#region Cập nhật giá trị cây
    switch (loai) {
        //Các loại nút ở trên cây
        case 'cay-he-thong':
            $_Khung.attr({
                'data-ma': '',
                'data-pham-vi': 'PhamVi'
            });
            break;
        case 'cay-pham-vi':
        case 'cay-con-pham-vi':
        case 'ds-pham-vi':
            $_Khung.attr({
                'data-ma': '0',
                'data-pham-vi': ma
            });
            break;
        case 'cay-nut':
        case 'cay-con-nut':
        case 'ds-nut':
            $_Khung.attr('data-ma', ma);
            break;
        default:
            return;
    }
    //#endregion

    mangTam['dangMoNut'] = false;
};

//#endregion