$cay = null;
$danhSachNutCon = null;
$danhSachNut = null;
//Xác định bằng PhamVi, <Tên phạm vi> (KhoaHoc, HeThong, ...), <Mã cha>
mangNutCon = [];

$(function () {
    $cay = $('#cay_chu_de');
    $danhSachNutCon = $('#danh_sach_chu_de');
    $danhSachNut = $('#danh_sach_nut');

    khoiTao_NutTao($cay.find('[data-chuc-nang="tao"]'));
    khoiTao_MoNut($cay.find('[data-chuc-nang="mo-nut"]'));
})

/*
    Khởi tạo các nút xử lý
*/
//Nút tạo
function khoiTao_NutTao($nutTao) {
    khoiTaoPopupFull($nutTao, {
        url: '/ChuDe/_Form',
        data: function() {
            return {
                maChuDeCha: parseInt($cay.attr('data-value')) || 0,
                phamVi: $cay.attr('data-pham-vi')
            };
        },
        width: '450px',
        thanhCong: function ($noiDung) {
            khoiTaoLCTForm($noiDung.find('.lct-form'));
            $noiDung.find('.lct-form').on('submit', function (e) {
                e = e || window.event;
                e.preventDefault();

                if ('DangTaoChuDe' in mangTam && mangTam['DangTaoChuDe']) {
                    return;
                }
                mangTam['DangTaoChuDe'] = true;

                $form = $(this);

                $.ajax({
                    url: $form.attr('action'),
                    type: $form.attr('method'),
                    dataType: 'JSON',
                    data: $form.serialize()
                }).done(function (data) {
                    $popup.trigger('Tat');

                    $nutCon = $(taoNutCon(data.ketQua));
                    khoiTao_MoNut($nutCon.find('*[data-chuc-nang="mo-nut"]'));
                    $danhSachNutCon.prepend($nutCon);
                    $danhSachNutCon.removeClass('rong');

                    var key = parseInt($cay.attr('data-ma')) > 0 ? $cay.attr('data-ma') : $cay.attr('data-pham-vi');
                    if (key in mangNutCon) {
                        mangNutCon[key].push(data.ketQua);
                    }
                    else {
                        mangNutCon[key] = [data.ketQua];
                    }

                    taoNutConTrenNut($danhSachNut.find('li:last-child'), data.ketQua);
                }).fail(function () {
                    $popup.trigger('Tat');
                }).always(function () {
                    mangTam['DangTaoChuDe'] = false;
                });
            });
        }
    });

    //Nút tạo


    /*
    $nutTao.on('click', function () {
        $.ajax({
            type: 'GET',
            url: '/ChuDe/_Form',
            data: {
                maChuDeCha: parseInt($cay.attr('data-ma')) || 0,
                phamVi: $cay.attr('data-pham-vi')
            },
            contentType: 'JSON'
        }).done(function (data) {
            if (data.trangThai == 0) {
                $popup = layPopupFull();
                $popup.trigger('Mo');
                $noiDungPopup = $popup.find('#noi_dung');

                $noiDungPopup.html(data.ketQua);
                khoiTaoLCTForm($noiDungPopup.find('.lct-form'));                    
                $noiDungPopup.find('.lct-form').on('submit', function (e) {
                    e = e || window.event;
                    e.preventDefault();

                    if (mangTam.hasOwnProperty("DangTaoChuDe") && mangTam['DangTaoChuDe']) {
                        return;
                    }
                    mangTam['DangTaoChuDe'] = true;

                    $form = $(this);

                    $.ajax({
                        url: $form.attr('action'),
                        type: $form.attr('method'),
                        dataType: 'JSON',
                        data: $form.serialize()
                    }).done(function (data) {
                        $popup.trigger('Tat');

                        $nutCon = $(taoNutCon(data.ketQua));
                        khoiTaoNut_MoNutCon($nutCon.find('*[data-chuc-nang="mo-nut-con"]'));
                        $danhSachNutCon.prepend($nutCon);
                        $danhSachNutCon.removeClass('rong');

                        var key = parseInt($cay.attr('data-ma')) > 0 ? $cay.attr('data-ma') : $cay.attr('data-pham-vi');
                        if (key in mangNutCon) {
                            mangNutCon[key].push(data.ketQua);
                        }
                        else {
                            mangNutCon[key] = [data.ketQua];
                        }

                        taoNutConChoNut($danhSachNut.find('li:last-child'), data.ketQua);
                    }).fail(function () {
                        $popup.trigger('Tat');
                    }).always(function () {
                        mangTam['DangTaoChuDe'] = false;
                    });
                });
            }
            else {
                alert('Thất bại');
            }
        }).fail(function () {
            alert('Thất bại');
        });
    });
    */
}

//Nút mở nút
function khoiTao_MoNut($nutMo) {

    /*
        Danh sách loại
    */
    /*
    switch (loai) {        
        //Các loại nút ở trên cây
        case 'nut-pham-vi-goc':
            break;
        case 'nut-pham-vi':
            break;
        case 'nut':
            break;
        case 'nut-con-pham-vi-tren-nut':
            break;
        case 'nut-con-tren-nut':
            break;

        //Các loại nút con ở danh sách nút con
        case 'nut-con':
            break;
        case 'nut-con-pham-vi':
            break;

        default:
            alert('Dữ liệu không hợp lệ');
            return;
    }
    */

    $nutMo.on('click', function () {
        $phanTu = $(this);
        
        //Lấy một số dữ liệu cần thiết
        var text = $phanTu.text();
        var loai = $phanTu.attr('data-loai');
        var value = $phanTu.attr('data-value');

        /*
            Lấy danh sách nút
        */
        //Lấy danh sách nút nếu trước đó chưa lấy
        if (!(value in mangNutCon)) {
            /*
                Nếu chưa có => Ajax để lấy
            */
            //Lấy tham số gửi đi
            data = null;
            switch (loai) {
                /*
                Không cần xét trường hợp này vì không bao giờ xảy ra
                case 'nut-pham-vi-goc':
                    break;
                */
                case 'nut-pham-vi':
                case 'nut-con-pham-vi-tren-nut':
                case 'nut-con-pham-vi':
                    data = {
                        maChuDeCha: 0,
                        phamVi: value
                    };
                    break;
                case 'nut':
                case 'nut-con-tren-nut':
                case 'nut-con':
                    data = {
                        maChuDeCha: value,
                        phamVi: $cay.attr('data-pham-vi')
                    }
                    break;
                default:
                    alert('Dữ liệu không hợp lệ');
                    return;
            }
            
            //Lấy dữ liệu
            var thanhCong; //Để xác định lấy dữ liệu thành công hay thất bại
            $.ajax({
                url: '/ChuDe/XuLyLayDanhSach',
                data: data,
                contentType: 'JSON',
                async: false
            }).done(function (data) {
                if (data.trangThai == 0) {
                    //Lưu lại kết quả lấy được
                    mangNutCon[value] = data.ketQua;
                    thanhCong = true;
                }
                else if (data.trangThai == 1) {
                    //Lưu lại kết quả lấy được
                    mangNutCon[value] = [];
                    thanhCong = true;
                }
                else {
                    alert('Lấy danh sách nút con thất bại');
                    thanhCong = false;
                }
            }).fail(function () {
                alert('Lấy danh sách nút con thất bại');
                thanhCong = false;
            });

            if (!thanhCong) {
                return;
            }
        }

        var danhSachNutCon = mangNutCon[value];

        /*
            Hiển thị ra danh sách nút
        */
        if (danhSachNutCon.length == 0) {
            $danhSachNutCon.addClass('rong');
            $danhSachNutCon.html('');
        }
        else {
            $danhSachNutCon.removeClass('rong');
            switch (loai) {
                //Các loại nút ở trên cây
                case 'nut-pham-vi-goc':
                    $danhSachNutCon.html(taoNutCon_PhamVi(danhSachNutCon));
                    break;
                case 'nut-pham-vi':
                case 'nut':
                case 'nut-con-pham-vi-tren-nut':
                case 'nut-con-tren-nut':
                case 'nut-con':
                case 'nut-con-pham-vi':
                    $danhSachNutCon.html(taoNutCon(danhSachNutCon));
                    break;

                default:
                    alert('Dữ liệu không hợp lệ');
                    return;
            }
        }

        /*
            Hiển thị nút ở cây và
            Điều chỉnh hiển thị nút con trên cây
        */
        $nut = null;
        switch (loai) {
            case 'nut-pham-vi-goc':
            case 'nut-pham-vi':
            case 'nut':
                $phanTu.parent().find('~ li').remove();
                break;
            case 'nut-con-pham-vi-tren-nut':
                $($phanTu.parents('li')[1]).find('~ li').remove();
                $nut = taoNut(text, value, 'nut-pham-vi');
            case 'nut-con-tren-nut':
                $($phanTu.parents('li')[1]).find('~ li').remove();
                $nut = taoNut(text, value, 'nut');
                break;
            case 'nut-con-pham-vi':
                $nut = taoNut(text, value, 'nut-pham-vi');
                break;
            case 'nut-con':
                $nut = taoNut(text, value, 'nut');
                break;

            default:
                alert('Dữ liệu không hợp lệ');
                return;
        }
        if ($nut != null) {
            $danhSachNut.append($nut);
            dieuChinhNutConTrenNut($nut.prev(), value);
        }

        /*
            Cập nhật giá trị cây
        */
        switch (loai) {
            //Các loại nút ở trên cây
            case 'nut-pham-vi-goc':
                $cay.attr({
                    'data-value': '',
                    'data-pham-vi': ''
                });
                break;
            case 'nut-pham-vi':
            case 'nut-con-pham-vi-tren-nut':
            case 'nut-con-pham-vi':
                $cay.attr({
                    'data-value': '0',
                    'data-pham-vi': value
                });
                break;
            case 'nut':
            case 'nut-con-tren-nut':
            case 'nut-con':
                $cay.attr('data-value', value);
                break;

            default:
                alert('Dữ liệu không hợp lệ');
                return;
        }
    });
}

/*
    Chức năng hỗ trợ
*/

//Tạo nút ở cây
//loai: Là loại của nút
function taoNut(text, value, loai) {
    /*
        Tạo danh sách nút con trên nút
    */
    var htmlDanhSachNutCon = '';
    var danhSachNutCon = mangNutCon[value];
    var soLuongNutCon = danhSachNutCon.length;

    $(danhSachNutCon).each(function () {
        //Không cần xử lý trường hợp nút con trên nút là phạm vi vì không xảy ra trường hợp này
        htmlDanhSachNutCon += '\
            <li>\
                <a data-chuc-nang="mo-nut" data-loai="nut-con-tren-nut" data-value="' + this.ma + '" href="javascript:void(0)">\
                    ' + this.ten + '\
                </a>\
            </li>\
        ';
    });

    $htmlNut = $('\
        <li>\
            <a data-chuc-nang="mo-nut" data-loai="' + loai + '" data-value="' + value + '" href="javascript:void(0)">\
                ' + text + '\
            </a>\
            <ul class="nut-con">\
                ' + htmlDanhSachNutCon + '\
            </ul>\
        </li>\
    ');

    khoiTao_MoNut($htmlNut.find('[data-chuc-nang="mo-nut"]'));

    return $htmlNut;
}

//Điều chỉnh nút trên cùng cho nút ở cây
function dieuChinhNutConTrenNut($nutCanChinh, value) {
    $khungChuaDanhSach = $nutCanChinh.find('.nut-con');
    $nutLenDau = $khungChuaDanhSach.find('*[data-value="' + value + '"]').parent();

    $khungChuaDanhSach.prepend($nutLenDau);
}

//Tạo nút con ở danh sách dạng phạm vi
function taoNutCon_PhamVi(danhSachNutCon) {
    var soLuongNut = danhSachNutCon.length;

    var $htmlDanhSachNutCon = '';
    $(danhSachNutCon).each(function () {
        $htmlDanhSachNutCon += '\
            <li>\
                <section class="mo-ta">\
                    <img src="/Content/images/LCT/ChuDe/' + this.hinh + '">\
                    <p>\
                        ' + this.moTa + '\
                    </p>\
                </section>\
                <a href="javascript:void(0)" class="ten" data-chuc-nang="mo-nut" data-loai="nut-con-pham-vi" data-value="' + this.ma + '">\
                    ' + this.ten + '\
                </a>\
            </li>\
        ';
    });

    $htmlDanhSachNutCon = $($htmlDanhSachNutCon);

    khoiTao_MoNut($htmlDanhSachNutCon.find('[data-chuc-nang="mo-nut"]'));

    return $htmlDanhSachNutCon;
}

//Thêm 1 nút con cho nút ở cây
function taoNutConTrenNut($nutCanTao, nutConCanThem) {
    $htmlNutCon = $('\
        <li>\
            <a data-chuc-nang="mo-nut" data-loai="nut-con-tren-nut" data-value="' + nutConCanThem.ma + '" href="javascript:void(0)">\
                ' + nutConCanThem.ten + '\
            </a>\
        </li>\
    ');

    khoiTao_MoNut($htmlNutCon.find('[data-chuc-nang="mo-nut"]'));

    $nutCanTao.find('.nut-con').append($htmlNutCon);
}

//Tạo nút con ở danh sách
function taoNutCon(danhSachNutCon) {
    var soLuongNut = danhSachNutCon.length;

    var $htmlDanhSachNutCon = '';
    $(danhSachNutCon).each(function () {
        $htmlDanhSachNutCon += '\
            <li>\
                <section class="mo-ta">\
                    <img src="/taptin/laytaptin/' + this.hinhDaiDien.ma + '">\
                    <p>\
                        ' + this.moTa + '\
                    </p>\
                </section>\
                <a href="javascript:void(0)" data-chuc-nang="mo-nut" data-loai="nut-con" data-value="' + this.ma + '" class="ten">\
                    ' + this.ten + '\
                </a>\
                <section class="chuc-nang">\
                    <i class="pe-7s-config"></i>\
                    <ul>\
                        <li>\
                            <a href="javascript:void(0)">\
                                Xoa chu de\
                            </a>\
                        </li>\
                        <li>\
                            <a href="javascript:void(0)">\
                                Xoa chu de\
                            </a>\
                        </li>\
                        <li>\
                            <a href="javascript:void(0)">\
                                Xoa chu de\
                            </a>\
                        </li>\
                    </ul>\
                </section>\
            </li>\
        ';
    });

    $htmlDanhSachNutCon = $($htmlDanhSachNutCon);

    khoiTao_MoNut($htmlDanhSachNutCon.find('[data-chuc-nang="mo-nut"]'));

    return $htmlDanhSachNutCon;
}