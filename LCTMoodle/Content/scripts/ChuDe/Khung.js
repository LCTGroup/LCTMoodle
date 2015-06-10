var $_Khung,
    $_Cay,
    $_DanhSach,
    mangNut = {}; //Xác định bằng mã chủ đề

//Khởi tạo
function khoiTaoKhungChuDe($khung) {
    $_Khung = $khung;
    $_Cay = $_Khung.find('#khung_cay');
    $_DanhSach = $_Khung.find('#khung_danh_sach');

    khoiTaoCay_Item($_Cay.find('[data-doi-tuong="muc"]'));
    khoiTaoDanhSach_Item($_DanhSach.find('[data-doi-tuong="muc"]'));
    khoiTaoNutTao($_Khung.find('[data-chuc-nang="tao"]'));

    khoiTaoNutChon($_DanhSach.find('[data-chuc-nang="chon"]'));
}

//#region Xử lý nút nhấn

//#region Chọn nút

function khoiTaoNutChon($nut) {
    $nut.on('click', function () {
        chonNut($(this).closest('[data-doi-tuong="muc"]'));
    })
}

function chonNut($muc) {
    $_Khung.trigger('chon', [{
        ma: $muc.attr('data-ma'),
        ten: $muc.children('[data-doi-tuong="ten"]').text()
    }])
}

//#endregion

//#region Xử lý mở nút

/*
    Danh sách loại
*/
/*
    Các loại nút ở cây
        cay-nut
        cay-con-nut

    Các loại nút ở danh sách
        ds-nut
*/

function khoiTaoMoNut($nutMo) {
    $nutMo.on('click', function () {
        //Kiểm tra xem có đang mở nút khác ko
        if ('dangMoNut' in mangTam && mangTam.dangMoNut) {
            return;
        }
        mangTam.dangMoNut = true;

        var
            $muc = $(this).closest('[data-doi-tuong="muc"]'),
            loai = $muc.attr('data-loai'),
            ma = $muc.attr('data-ma');

        //#region Lấy dữ liệu
        if (!(ma in mangNut)) {
            //#region Ajax lấy dữ liệu
            //Lấy dữ liệu
            //Để xác định lấy dữ liệu thành công hay thất bại
            var thanhCong;

            $.ajax({
                url: '/ChuDe/_Khung',
                data: { ma: ma },
                contentType: 'JSON',
                async: false
            }).done(function (data) {
                if (data.trangThai <= 1) {
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

        khoiTaoCay_Item($cay);
        khoiTaoDanhSach_Item($danhSach);
        khoiTaoNutChon($danhSach.find('[data-chuc-nang="chon"]'));
        //#endregion

        //#region Hiển thị
        //Hiển thị cây
        switch (loai) {
            case 'cay-nut':
                $muc.find('~ *').remove();
                break;
            case 'cay-con-nut':
                var $mucCha = $muc.parent().closest('[data-doi-tuong="muc"]');
                $mucCha.find('~ *').remove();
                $mucCha.after($cay)
                break;
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
        $_DanhSach.html($danhSach);

        //#endregion

        //#region Cập nhật giá trị cây
        $_Khung.attr('data-ma', ma);
        //#endregion

        mangTam.dangMoNut = false;
    });
}

function moNut($obj) {
};

//#endregion

//#region Xử lý nút tạo

function khoiTaoNutTao($nutTao) {
    $nutTao.on('click', function () {
        moPopupFull({
            url: '/ChuDe/_Form',
            data: function () {
                return {
                    ma: $_Khung.attr('data-ma')
                };
            },
            width: '450px',
            thanhCong: function ($khung) {
                var $form = $khung.find('#tao_chu_de_form');

                khoiTaoLCTForm($form, {
                    submit: function () {
                        $.ajax({
                            url: '/ChuDe/XuLyThem',
                            type: 'POST',
                            data: $form.serialize(),
                            dataType: 'JSON'
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                $khung.tat();
                                var ma = $_Khung.attr('data-ma');
                                
                                if (ma in mangNut)
                                {
                                    mangNut[ma].cay += data.ketQua.cayCon;
                                    mangNut[ma].danhSach += data.ketQua.danhSach_Item;
                                }
                                else {
                                    mangNut[ma] = {
                                        cay: data.ketQua.cayCon,
                                        danhSach: data.ketQua.danhSach_Item
                                    };
                                }

                                var $cayCon_Item = $(data.ketQua.cayCon),
                                    $danhSach_Item = $(data.ketQua.danhSach_Item);

                                khoiTaoCayCon_Item($cayCon_Item);
                                khoiTaoDanhSach_Item($danhSach_Item);

                                $_Cay.find(':last-child [data-doi-tuong="danh-sach-muc"]').append($cayCon_Item);
                                $_DanhSach.prepend($danhSach_Item);
                            }
                            else {
                                moPopup({
                                    tieuDe: 'Thông báo',
                                    thongBao: 'Tạo chủ đề thất bại',
                                    bieuTuong: 'nguy-hiem'
                                })
                            }
                        }).fail(function () {
                            moPopup({
                                tieuDe: 'Thông báo',
                                thongBao: 'Tạo chủ đề thất bại',
                                bieuTuong: 'nguy-hiem'
                            })
                        });
                    }
                });
            }
        })
    })
}
//#endregion

//#region Xử lý nút xóa

function khoiTaoNutXoa($nutXoa) {
    $nutXoa.on('click', function () {
        var $muc = $(this).closest('[data-doi-tuong="muc"]');

        moPopup({
            tieuDe: 'Xác nhận',
            thongBao: 'Bạn có chắc muốn xóa chủ đề này?',
            bieuTuong: 'hoi',
            nut: [
                {
                    ten: 'Có',
                    xuLy: function () {
                        var ma = $muc.attr('data-ma');

                        $.ajax({
                            url: '/ChuDe/XuLyXoa/' + ma,
                            type: 'POST',
                            dataType: 'JSON'
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                //Xóa hiển thị hiện tại
                                $muc.remove();
                                $_Cay.find(':last-child [data-ma="' + ma + '"]').remove();

                                //Xóa dữ liệu đã lưu trữ 
                                var maCha = $_Khung.attr('data-ma');
                                if (maCha === '0') {
                                    maCha = $_Khung.attr('data-pham-vi');
                                }

                                var duLieuChuDe = mangNut[maCha],
                                    $cay_fake = $('<fake></fake>').html(duLieuChuDe.cay),
                                    $danhSach_fake = $('<fake></fake>').html(duLieuChuDe.danhSach);

                                $cay_fake.find('[data-ma="' + ma + '"]').remove();
                                $danhSach_fake.find('[data-ma="' + ma + '"]').remove();

                                duLieuChuDe.cay = $cay_fake.html();
                                duLieuChuDe.danhSach = $danhSach_fake.html();
                            }
                            else {
                                moPopup({
                                    tieuDe: 'Thông báo',
                                    thongBao: 'Xóa chủ đề thất bại',
                                    bieuTuong: 'nguy-hiem'
                                });
                                return false;
                            }
                        }).fail(function () {
                            moPopup({
                                tieuDe: 'Thông báo',
                                thongBao: 'Xóa chủ đề thất bại',
                                bieuTuong: 'nguy-hiem'
                            });
                            return false;
                        });
                    }
                },
                {
                    ten: 'Không'
                }
            ]
        })
    })
}

//#endregion

//#endregion

//#region Khởi tạo

function khoiTaoCay_Item($danhSachItem) {
    //Khởi tạo mở nút
    khoiTaoMoNut($danhSachItem.find('[data-chuc-nang="mo-nut"]'));
}

function khoiTaoCayCon_Item($danhSachItem) {
    //Khởi tạo mở nút
    khoiTaoMoNut($danhSachItem.find('[data-chuc-nang="mo-nut"]'));
}

function khoiTaoDanhSach_Item($danhSachItem) {
    //Khởi tạo mở nút
    khoiTaoMoNut($danhSachItem.find('[data-chuc-nang="mo-nut"]'));

    //Khởi tạo nút tắt mở đối tương
    khoiTaoTatMoDoiTuong($danhSachItem.find('[data-chuc-nang="tat-mo"]'));
    
    //Khởi tạo nút xóa
    khoiTaoNutXoa($danhSachItem.find('[data-chuc-nang="xoa"]'));
}

//#endregion