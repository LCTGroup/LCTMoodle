function khoiTaoKhungChuDe($khung, thamSo) {
    var $_Khung_CD,
        $_Cay_CD,
        $_DanhSach_CD,
        mangNut = {},
        coChon; //Xác định bằng mã chủ đề

    thamSo = thamSo || {};

    $_Khung_CD = $khung;
    $_Cay_CD = $_Khung_CD.find('#khung_cay');
    $_DanhSach_CD = $_Khung_CD.find('#khung_danh_sach');

    khoiTaoCay_Item($_Cay_CD.find('[data-doi-tuong="muc"]'));
    khoiTaoDanhSach_Item($_DanhSach_CD.find('[data-doi-tuong="muc"]'));
    khoiTaoNutTao($_Khung_CD.find('[data-chuc-nang="tao"]'));
    khoiTaoNutChon_HienTai($_Khung_CD.find('[data-chuc-nang="chon-nut-ht"]'));

    coChon = $_Khung_CD.is('[data-chon]');
    if ('event' in thamSo) {
        $_Khung_CD.on(thamSo.event);
    }

    //#region Xử lý nút nhấn

    //#region Chọn nút

    function khoiTaoNutChon($nut) {
        $nut.on('click', function () {
            var $item = $(this).closest('[data-doi-tuong="muc"]');
            
            $_Khung_CD.trigger('chon', [{
                ma: $item.attr('data-ma'),
                ten: $item.children('[data-doi-tuong="ten"]').text()
            }])
        })
    }

    //#endregion

    //#region Xử lý chọn chủ đề hiện tại

    function khoiTaoNutChon_HienTai($nut) {
        $nut.on('click', function () {
            $_Khung_CD.trigger('chon', [{
                ma: $_Khung_CD.attr('data-ma'),
                ten: $_Khung_CD.data('ten')
            }])
        })
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

                var $tai = moBieuTuongTai($_Khung_CD);
                $.ajax({
                    url: '/ChuDe/_Khung',
                    data: { ma: ma, coChon: coChon },
                    contentType: 'JSON',
                    async: false
                }).always(function () {
                    $tai.tat();
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
                    $_Cay_CD.append($cay);
                    break;
                default:
                    return;
            }

            //Điều chỉnh hiển thị cây con
            var $danhSachMuc = $_Cay_CD.find(':nth-last-child(2) [data-doi-tuong="danh-sach-muc"]');
            $danhSachMuc.prepend($danhSachMuc.children('[data-ma="' + ma + '"]'));

            //Hiển thị danh sách
            $_DanhSach_CD.html($danhSach);

            //#endregion

            //#region Cập nhật giá trị cây
            $_Khung_CD.attr('data-ma', ma);
            $_Khung_CD.data('ten', $muc.is('[data-doi-tuong="ten"]') ? $muc.text() : $muc.find('[data-doi-tuong="ten"]').text());
            //#endregion

            mangTam.dangMoNut = false;
        });
    }

    //#endregion

    //#region Xử lý nút tạo

    function khoiTaoNutTao($nutTao) {
        $nutTao.on('click', function () {
            moPopupFull({
                url: '/ChuDe/_FormTao',
                data: function () {
                    return {
                        maCha: $_Khung_CD.attr('data-ma')
                    };
                },
                width: '450px',
                thanhCong: function ($khung) {
                    var $form = $khung.find('#tao_chu_de_form');

                    khoiTaoLCTForm($form, {
                        submit: function () {
                            var $tai = moBieuTuongTai($_Khung_CD);
                            $.ajax({
                                url: '/ChuDe/XuLyThem',
                                type: 'POST',
                                data: layDataLCTForm($form) + '&CoChon=' + (coChon ? 1 : 0),
                                dataType: 'JSON'
                            }).always(function () {
                                $tai.tat();
                            }).done(function (data) {
                                if (data.trangThai == 0) {
                                    $khung.tat();
                                    var ma = $_Khung_CD.attr('data-ma');

                                    if (ma in mangNut) {
                                        var $fake = $('<fake>' + mangNut[ma].cay + '</fake>');
                                        $fake.find('[data-doi-tuong="danh-sach-muc"]').append(data.ketQua.cayCon_Item);

                                        mangNut[ma].cay = $fake.html();
                                        mangNut[ma].danhSach += data.ketQua.danhSach_Item;
                                    }

                                    var $cayCon_Item = $(data.ketQua.cayCon_Item),
                                        $danhSach_Item = $(data.ketQua.danhSach_Item);

                                    khoiTaoCayCon_Item($cayCon_Item);
                                    khoiTaoDanhSach_Item($danhSach_Item);

                                    $_Cay_CD.find(':last-child [data-doi-tuong="danh-sach-muc"]').append($cayCon_Item);
                                    $_DanhSach_CD.prepend($danhSach_Item);
                                }
                                else {
                                    moPopupThongBao(data);
                                }
                            }).fail(function () {
                                moPopupThongBao('Thêm chủ đề thất bại');
                            });
                        }
                    });
                }
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

        //Khởi tạo nút chọn
        khoiTaoNutChon($danhSachItem.find('[data-chuc-nang="chon"]'));

        //Khởi tạo nút tắt mở đối tương
        khoiTaoTatMoDoiTuong($danhSachItem.find('[data-chuc-nang="tat-mo"]'), true);

        //Khởi tạo nút xóa
        $danhSachItem.find('[data-chuc-nang="xoa"]').on('click', function () {
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

                            var $tai = moBieuTuongTai($_Khung_CD);
                            $.ajax({
                                url: '/ChuDe/XuLyXoa/' + ma,
                                type: 'POST',
                                dataType: 'JSON'
                            }).always(function () {
                                $tai.tat();
                            }).done(function (data) {
                                if (data.trangThai == 0) {
                                    //Xóa hiển thị hiện tại
                                    $muc.remove();
                                    $_Cay_CD.find(':last-child [data-ma="' + ma + '"]').remove();

                                    //Xóa dữ liệu đã lưu trữ 
                                    var maCha = $_Khung_CD.attr('data-ma');
                                    if (maCha === '0') {
                                        maCha = $_Khung_CD.attr('data-pham-vi');
                                    }

                                    if (maCha in mangNut) {
                                        var duLieuChuDe = mangNut[maCha],
                                            $cay_fake = $('<fake></fake>').html(duLieuChuDe.cay),
                                            $danhSach_fake = $('<fake></fake>').html(duLieuChuDe.danhSach);

                                        $cay_fake.find('[data-ma="' + ma + '"]').remove();
                                        $danhSach_fake.find('[data-ma="' + ma + '"]').remove();

                                        duLieuChuDe.cay = $cay_fake.html();
                                        duLieuChuDe.danhSach = $danhSach_fake.html();
                                    }
                                }
                                else {
                                    moPopupThongBao();
                                }
                            }).fail(function () {
                                moPopupThongBao('Xóa chủ đề thất bại');
                            });
                        }
                    },
                    {
                        ten: 'Không'
                    }
                ]
            })
        });

        //Khởi tạo nút sửa
        $danhSachItem.find('[data-chuc-nang="sua"]').on('click', function () {
            var $item = $(this).closest('[data-doi-tuong="muc"]');

            moPopupFull({
                url: '/ChuDe/_FormSua',
                data: { ma: $item.attr('data-ma') },
                width: '450px',
                thanhCong: function ($khung) {
                    var $form = $khung.find('#tao_chu_de_form');

                    khoiTaoLCTForm($form, {
                        submit: function () {
                            var $tai = moBieuTuongTai($_Khung_CD);
                            $.ajax({
                                url: '/ChuDe/XuLyCapNhat',
                                type: 'POST',
                                data: layDataLCTForm($form) + '&CoChon=' + (coChon ? 1 : 0),
                                dataType: 'JSON'
                            }).always(function () {
                                $tai.tat();
                            }).done(function (data) {
                                if (data.trangThai == 0) {
                                    $khung.tat();

                                    var ma = $item.attr('data-ma');
                                    var maCha = $_Khung_CD.attr('data-ma');

                                    //Thay thế item ở danh sách, cây con
                                    var $itemMoi_ds = $(data.ketQua.danhSach_Item);
                                    khoiTaoDanhSach_Item($itemMoi_ds);
                                    $item.replaceWith($itemMoi_ds);

                                    var $itemMoi_Cay = $(data.ketQua.cayCon_Item);
                                    khoiTaoCayCon_Item($itemMoi_Cay);
                                    $_Cay_CD.find(':last-child [data-doi-tuong="danh-sach-muc"][data-ma="' + ma + '"]').replaceWith($itemMoi_Cay);

                                    //Thay thế item ở danh sách, cây con
                                    if (maCha in mangNut) {
                                        var $fake = $('<fake>' + mangNut[maCha].danhSach + '</fake>');
                                        $fake.find('[data-doi-tuong="muc"][data-ma="' + ma + '"]').replaceWith(data.ketQua.danhSach_Item);
                                        mangNut[maCha].danhSach = $fake.html();

                                        var $fake = $('<fake>' + mangNut[maCha].cay + '</fake>');
                                        $fake.find('[data-doi-tuong="danh-sach-muc"] [data-ma="' + ma + '"]').replaceWith(data.ketQua.cayCon_Item);
                                        mangNut[maCha].cay = $fake.html();
                                    }

                                    //Xóa cây tạm của nút
                                    delete mangNut[ma];
                                }
                                else {
                                    moPopupThongBao(data);
                                }
                            }).fail(function () {
                                moPopupThongBao('Sửa chủ đề thất bại');
                            });
                        }
                    });
                }
            })
        });

        //Khởi tạo nút chuỷen
        $danhSachItem.find('[data-chuc-nang="chuyen"]').on('click', function () {
            var $item = $(this).closest('[data-doi-tuong="muc"]');

            moPopupFull({
                url: '/ChuDe/_Chon',
                thanhCong: function ($popup) {
                    var $khung = $popup.find('#khung_chu_de');

                    khoiTaoKhungChuDe($khung, {
                        event: {
                            'chon': function (e, data) {
                                $popup.tat();

                                var maChaMoi = data.ma;
                                var maChaCu = $_Khung_CD.attr('data-ma')
                                var ma = $item.data('ma');

                                if (maChaMoi == ma) {
                                    moPopupThongBao('Không thể di chuyển vào chính nó');
                                }

                                if (maChaMoi == maChaCu) {
                                    return;
                                }

                                var $tai = moBieuTuongTai($_Khung_CD);
                                $.ajax({
                                    url: '/ChuDe/XuLyChuyen/' + ma,
                                    method: 'POST',
                                    data: { maCha: maChaMoi },
                                    dataType: 'JSON'
                                }).always(function () {
                                    $tai.tat();
                                }).done(function (data) {
                                    if (data.trangThai == 0) {

                                        var $item_cay = $_Cay_CD.find(':last-child [data-doi-tuong="danh-sach-muc"] [data-ma="' + ma + '"]');

                                        var $item_ = $item.clone(true),
                                            $item_cay_ = $item_cay.clone(true);

                                        /*
                                            Xóa item ở cha hiện tại
                                        */
                                        //Danh sách, cây
                                        $item.remove();
                                        $item_cay.remove();

                                        //Danh sách, cây tạm
                                        if (maChaCu in mangNut) {
                                            var $fake = $('<fake>' + mangNut[maChaCu].danhSach + '</fake>');
                                            $fake.find('[data-doi-tuong="muc"][data-ma="' + ma + '"]').remove();
                                            mangNut[maChaCu].danhSach = $fake.html();

                                            var $fake = $('<fake>' + mangNut[maChaCu].cay + '</fake>');
                                            $fake.find('[data-doi-tuong="danh-sach-muc"] [data-ma="' + ma + '"]').remove();
                                            mangNut[maChaCu].cay = $fake.html();
                                        }

                                        /*
                                            Thêm ở cha mới
                                        */
                                        //Cây
                                        $_Cay_CD.find('[data-ma="' + maChaMoi + '"] [data-doi-tuong="danh-sach-muc"]').append($item_cay_);
                                        
                                        //Danh sách, cây tạm
                                        if (maChaMoi in mangNut) {
                                            var $fake = $('<fake>' + mangNut[maChaMoi].danhSach + '</fake>');
                                            $fake.append($item_);
                                            mangNut[maChaMoi].danhSach = $fake.html();

                                            var $fake = $('<fake>' + mangNut[maChaMoi].cay + '</fake>');
                                            $fake.find('[data-doi-tuong="danh-sach-muc"]').append($item_cay_);
                                            mangNut[maChaMoi].cay = $fake.html();
                                        }
                                    }
                                    else {
                                        moPopupThongBao(data);
                                    }
                                }).fail(function () {
                                    moPopupThongBao('Chuyển thất bại');
                                })
                            }
                        }
                    });
                }
            });
        })
    }

    //#endregion
}
