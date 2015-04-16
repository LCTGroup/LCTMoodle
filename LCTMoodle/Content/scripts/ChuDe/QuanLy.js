﻿$cay = null;
$danhSachNutCon = null;
$danhSachNut = null;
mangNutCon = [];

$(function () {
    $cay = $('#cay_chu_de');
    $danhSachNutCon = $('#danh_sach_chu_de');
    $danhSachNut = $('#danh_sach_nut');

    khoiTaoNut_Tao($cay.find('*[data-chuc-nang="tao"]'));
    khoiTaoNut_MoPhamVi($cay.find('*[data-chuc-nang="mo-pham-vi"]'));
    khoiTaoNut_MoNut($cay.find('*[data-chuc-nang="mo-pham-vi-goc"]'));
    khoiTaoNut_MoNutCon_Nut_PhamVi($cay.find('*[data-chuc-nang="mo-nut-con-nut-pham-vi"]'));
})

/*
    Khởi tạo các nút xử lý
*/
//Nút tạo
function khoiTaoNut_Tao($nutTao) {
    //Nút tạo
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
}

//Nút mở cho nút gốc của phạm vi
function khoiTaoNut_MoPhamVi($nutMo) {
    $nutMo.on('click', function () {
        $phanTu = $(this);
        $.ajax({
            url: '/ChuDe/XuLyLayDanhSach',
            data: {
                maChuDeCha: 0,
                phamVi: $phanTu.attr('data-value')
            },
            contentType: 'JSON'
        }).done(function (data) {
            if (data.trangThai == 0 || data.trangThai == 1) {
                if (data.trangThai == 0) {                    
                    //Lưu danh sách nút lại để sử dụng cho lần sau
                    mangNutCon[$phanTu.attr('data-value')] = data.ketQua;

                    var danhSachNutCon = data.ketQua;
                    var soLuongNut = danhSachNutCon.length;
                    var $htmlDanhSachNutCon = '';
                    for (var i = 0; i < soLuongNut; i++) {
                        $htmlDanhSachNutCon += taoNutCon(danhSachNutCon[i]);
                    }
                    $htmlDanhSachNutCon = $($htmlDanhSachNutCon);
                    khoiTaoNut_MoNutCon($htmlDanhSachNutCon.find('*[data-chuc-nang="mo-nut-con"]'));
                    $danhSachNutCon.html($htmlDanhSachNutCon);
                    $danhSachNutCon.removeClass('rong');
                }
                else {
                    $danhSachNutCon.html('');
                    $danhSachNutCon.addClass('rong');

                    mangNutCon[$phanTu.attr('data-value')] = [];
                }

                $nutMoi = $(taoNut($phanTu.text(), $phanTu.attr('data-value'), 'pham-vi'));
                khoiTaoNut_MoNut($nutMoi.find('*[data-chuc-nang="mo-nut"]'));
                khoiTaoNut_MoNutCon_Nut_Nut($nutMoi.find('*[data-chuc-nang="mo-nut-con-nut"]'));
                $danhSachNut.append($nutMoi);

                dieuChinhNutTrenCung($nutMoi.prev(), $phanTu.attr('data-value'));
                
                $cay.attr({
                    'data-ma': 0,
                    'data-pham-vi': $phanTu.attr('data-value')
                });
            }
            else {
                alert('Mở phạm vi thất bại');
            }
        }).fail(function () {
            alert('Mở phạm vi thất bại');
        });
    });
}

//Nút mở nút
function khoiTaoNut_MoNut($nutMo) {
    $nutMo.on('click', function () {
        $phanTu = $(this);
        
        //Lấy danh sách nút con trong mảng đã lưu trữ
        danhSachNutCon = mangNutCon[$phanTu.attr('data-value')];

        if (danhSachNutCon.length == 0) {
            $danhSachNutCon.addClass('rong');
        }
        else {
            $danhSachNutCon.removeClass('rong');
            var soLuongNut = danhSachNutCon.length;
            var $htmlDanhSachNutCon = '';

            if ($phanTu.attr('data-loai') == 'pham-vi-goc') {
                for (var i = 0; i < soLuongNut; i++) {
                    $htmlDanhSachNutCon += taoNutCon_PhamVi(danhSachNutCon[i]);
                }

                $htmlDanhSachNutCon = $($htmlDanhSachNutCon);
                khoiTaoNut_MoPhamVi($htmlDanhSachNutCon.find('*[data-chuc-nang="mo-pham-vi"]'));
            }
            else {
                for (var i = 0; i < soLuongNut; i++) {
                    $htmlDanhSachNutCon += taoNutCon(danhSachNutCon[i]);
                }

                $htmlDanhSachNutCon = $($htmlDanhSachNutCon);
                khoiTaoNut_MoNutCon($htmlDanhSachNutCon.find('*[data-chuc-nang="mo-nut-con"]'));
            }
        }

        $danhSachNutCon.html($htmlDanhSachNutCon);

        //Xóa toàn bộ các nút phía sau
        $phanTu.parent().find('~ li').remove();

        $cay.attr('data-ma', $phanTu.attr('data-value'));
    });
}

//Nút mở nút con ở cây
function khoiTaoNut_MoNutCon_Nut_Nut($nutMo) {
    $nutMo.on('click', function () {
        $phanTu = $(this);
        $.ajax({
            url: '/ChuDe/XuLyLayDanhSach',
            data: {
                maChuDeCha: $phanTu.attr('data-value'),
                phamVi: $cay.attr('data-pham-vi')
            },
            contentType: 'JSON'
        }).done(function (data) {
            if (data.trangThai == 0 || data.trangThai == 1) {
                if (data.trangThai == 0) {
                    $danhSachNutCon.removeClass('rong');

                    mangNutCon[$phanTu.attr('data-value')] = data.ketQua;
                    var danhSachNutCon = data.ketQua;
                    var soLuongNut = danhSachNutCon.length;
                    var $htmlDanhSachNutCon = '';
                    for (var i = 0; i < soLuongNut; i++) {
                        $htmlDanhSachNutCon += taoNutCon(danhSachNutCon[i]);
                    }
                    $htmlDanhSachNutCon = $($htmlDanhSachNutCon);
                    khoiTaoNut_MoNutCon($htmlDanhSachNutCon.find('*[data-chuc-nang="mo-nut-con"]'));
                    $danhSachNutCon.html($htmlDanhSachNutCon);
                }
                else {
                    mangNutCon[$phanTu.attr('data-value')] = Array();
                    $danhSachNutCon.addClass('rong');
                    $danhSachNutCon.html('');
                }

                //Xóa toàn bộ các nút phía sau và thêm nút vừa lấy        
                $($phanTu.parents('li')[1]).find('~ li').remove();
                $nutMoi = $(taoNut($phanTu.text(), $phanTu.attr('data-value'), 'nut'));
                khoiTaoNut_MoNut($nutMoi.find('*[data-chuc-nang="mo-nut"]'));
                khoiTaoNut_MoNutCon_Nut_Nut($nutMoi.find('*[data-chuc-nang="mo-nut-con-nut"]'));
                $danhSachNut.append($nutMoi);

                dieuChinhNutTrenCung($nutMoi.prev(), $phanTu.attr('data-value'));

                $cay.attr('data-ma', $phanTu.attr('data-value'));
            }
            else {
                alert('Mở phạm vi thất bại');
            }
        }).fail(function () {
            alert('Mở phạm vi thất bại');
        });
    });
}

//Nút mở nút con ở cây (nút phạm vi)
function khoiTaoNut_MoNutCon_Nut_PhamVi($nutMo) {
    $nutMo.on('click', function () {
        $phanTu = $(this);
        $.ajax({
            url: '/ChuDe/XuLyLayDanhSach',
            data: {
                maChuDeCha: 0,
                phamVi: $phanTu.attr('data-value')
            },
            contentType: 'JSON'
        }).done(function (data) {
            if (data.trangThai == 0 || data.trangThai == 1) {
                if (data.trangThai == 0) {
                    //Lưu danh sách nút lại để sử dụng cho lần sau
                    mangNutCon[$phanTu.attr('data-value')] = data.ketQua;

                    var danhSachNutCon = data.ketQua;
                    var soLuongNut = danhSachNutCon.length;
                    var $htmlDanhSachNutCon = '';
                    for (var i = 0; i < soLuongNut; i++) {
                        $htmlDanhSachNutCon += taoNutCon(danhSachNutCon[i]);
                    }
                    $htmlDanhSachNutCon = $($htmlDanhSachNutCon);
                    khoiTaoNut_MoNutCon($htmlDanhSachNutCon.find('*[data-chuc-nang="mo-nut-con"]'));
                    $danhSachNutCon.html($htmlDanhSachNutCon);
                    $danhSachNutCon.removeClass('rong');
                }
                else {
                    $danhSachNutCon.html('');
                    $danhSachNutCon.addClass('rong');

                    mangNutCon[$phanTu.attr('data-value')] = [];
                }

                $($phanTu.parents('li')[1]).find('~ li').remove();
                $nutMoi = $(taoNut($phanTu.text(), $phanTu.attr('data-value'), 'nut'));
                khoiTaoNut_MoNut($nutMoi.find('*[data-chuc-nang="mo-nut"]'));
                khoiTaoNut_MoNutCon_Nut_Nut($nutMoi.find('*[data-chuc-nang="mo-nut-con-nut"]'));
                $danhSachNut.append($nutMoi);

                dieuChinhNutTrenCung($nutMoi.prev(), $phanTu.attr('data-value'));

                $cay.attr({
                    'data-ma': 0,
                    'data-pham-vi': $phanTu.attr('data-value')
                });
            }
            else {
                alert('Mở phạm vi thất bại');
            }
        }).fail(function () {
            alert('Mở phạm vi thất bại');
        });
    });
}

//Nút mở nút con
function khoiTaoNut_MoNutCon($nutMo) {
    $nutMo.on('click', function () {
        $phanTu = $(this);
        $.ajax({
            url: '/ChuDe/XuLyLayDanhSach',
            data: {
                maChuDeCha: $phanTu.attr('data-value'),
                phamVi: $cay.attr('data-pham-vi')
            },
            contentType: 'JSON'
        }).done(function (data) {
            if (data.trangThai == 0 || data.trangThai == 1) {
                if (data.trangThai == 0) {
                    $danhSachNutCon.removeClass('rong');

                    mangNutCon[$phanTu.attr('data-value')] = data.ketQua;
                    var danhSachNutCon = data.ketQua;
                    var soLuongNut = danhSachNutCon.length;
                    var $htmlDanhSachNutCon = '';
                    for (var i = 0; i < soLuongNut; i++) {
                        $htmlDanhSachNutCon += taoNutCon(danhSachNutCon[i]);
                    }
                    $htmlDanhSachNutCon = $($htmlDanhSachNutCon);
                    khoiTaoNut_MoNutCon($htmlDanhSachNutCon.find('*[data-chuc-nang="mo-nut-con"]'));
                    $danhSachNutCon.html($htmlDanhSachNutCon);
                }
                else {
                    mangNutCon[$phanTu.attr('data-value')] = Array();
                    $danhSachNutCon.addClass('rong');
                    $danhSachNutCon.html('');
                }

                $nutMoi = $(taoNut($phanTu.text(), $phanTu.attr('data-value'), 'nut'));
                khoiTaoNut_MoNut($nutMoi.find('*[data-chuc-nang="mo-nut"]'));
                khoiTaoNut_MoNutCon_Nut_Nut($nutMoi.find('*[data-chuc-nang="mo-nut-con-nut"]'));
                $danhSachNut.append($nutMoi);

                dieuChinhNutTrenCung($nutMoi.prev(), $phanTu.attr('data-value'));

                $cay.attr('data-ma', $phanTu.attr('data-value'));
            }
            else {
                alert('Mở phạm vi thất bại');
            }
        }).fail(function () {
            alert('Mở phạm vi thất bại');
        });
    });
}

/*
    Chức năng hỗ trợ
*/
//Tạo nút ở cây
function taoNut(text, value, loai) {
    var htmlDanhSachNutCon = '';
    var danhSachNutCon = mangNutCon[value];
    var soLuongNutCon = danhSachNutCon.length;

    for (var i = 0; i < soLuongNutCon; i++) {
        htmlDanhSachNutCon += '\
            <li>\
                <a data-chuc-nang="mo-nut-con-nut" data-value="' + danhSachNutCon[i].ma + '" href="javascript:void(0)">\
                    ' + danhSachNutCon[i].ten + '\
                </a>\
            </li>\
        ';
    }

    return '\
        <li>\
            <a data-chuc-nang="mo-nut" data-loai="' + loai + '" data-value="' + value + '" href="javascript:void(0)">\
                ' + text + '\
            </a>\
            <u>\
                <ul class="nut-con">\
                    ' + htmlDanhSachNutCon + '\
                </ul>\
            </u>\
        </li>\
    ';
}

//Thêm 1 nút con cho nút ở cây
function taoNutConChoNut($nutCanTao, nutConCanThem) {
    htmlNutCon = '\
        <li>\
            <a data-chuc-nang="mo-nut-con-nut" data-value="' + nutConCanThem.ma + '" href="javascript:void(0)">\
                ' + nutConCanThem.ten + '\
            </a>\
        </li>\
    ';
    $nutCanTao.find('.nut-con').append(htmlNutCon);
}

//Điều chỉnh nút trên cùng cho nút ở cây
function dieuChinhNutTrenCung($nutCanChinh, value) {
    $khungChuaDanhSach = $nutCanChinh.find('.nut-con');
    $nutLenDau = $khungChuaDanhSach.find('*[data-value="' + value + '"]').parent();

    $khungChuaDanhSach.prepend($nutLenDau);
}

//Tạo nút con ở danh sách
function taoNutCon(nutCon) {
    return '\
        <li>\
            <section class="mo-ta">\
                <img src="/taptin/laytaptin/' + nutCon.hinhDaiDien.ma + '">\
                <p>\
                    ' + nutCon.moTa + '\
                </p>\
            </section>\
            <a href="javascript:void(0)" data-chuc-nang="mo-nut-con" data-value="' + nutCon.ma + '" class="ten">\
                ' + nutCon.ten + '\
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
}

//Tạo nút con ở danh sách dạng phạm vi
function taoNutCon_PhamVi(nutPhamVi) {
    return '\
        <li>\
            <section class="mo-ta">\
                <img src="/Content/images/LCT/ChuDe/' + nutPhamVi.hinh + '">\
                <p>\
                    ' + nutPhamVi.moTa + '\
                </p>\
            </section>\
            <a href="javascript:void(0)" class="ten" data-chuc-nang="mo-pham-vi" data-value="' + nutPhamVi.ma + '">\
                ' + nutPhamVi.ten + '\
            </a>\
        </li>\
    ';
}