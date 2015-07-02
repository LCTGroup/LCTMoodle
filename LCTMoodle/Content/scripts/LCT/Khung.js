function css_browser_selector(u) { var ua = u.toLowerCase(), is = function (t) { return ua.indexOf(t) > -1 }, g = 'gecko', w = 'webkit', s = 'safari', o = 'opera', m = 'mobile', h = document.documentElement, b = [(!(/opera|webtv/i.test(ua)) && /msie\s(\d)/.test(ua)) ? ('ie ie' + RegExp.$1) : is('firefox/2') ? g + ' ff2' : is('firefox/3.5') ? g + ' ff3 ff3_5' : is('firefox/3.6') ? g + ' ff3 ff3_6' : is('firefox/3') ? g + ' ff3' : is('gecko/') ? g : is('opera') ? o + (/version\/(\d+)/.test(ua) ? ' ' + o + RegExp.$1 : (/opera(\s|\/)(\d+)/.test(ua) ? ' ' + o + RegExp.$2 : '')) : is('konqueror') ? 'konqueror' : is('blackberry') ? m + ' blackberry' : is('android') ? m + ' android' : is('chrome') ? w + ' chrome' : is('iron') ? w + ' iron' : is('applewebkit/') ? w + ' ' + s + (/version\/(\d+)/.test(ua) ? ' ' + s + RegExp.$1 : '') : is('mozilla/') ? g : '', is('j2me') ? m + ' j2me' : is('iphone') ? m + ' iphone' : is('ipod') ? m + ' ipod' : is('ipad') ? m + ' ipad' : is('mac') ? 'mac' : is('darwin') ? 'mac' : is('webtv') ? 'webtv' : is('win') ? 'win' + (is('windows nt 6.0') ? ' vista' : '') : is('freebsd') ? 'freebsd' : (is('x11') || is('linux')) ? 'linux' : '', 'js']; c = b.join(' '); h.className += ' ' + c; return c; }; css_browser_selector(navigator.userAgent);

$body = null;
var mangTam = [];

/*
	Khởi tạo
*/
$(function () {
    $body = $('body');
    $body.removeClass('tai');
    $body.find('[data-doi-tuong="bieu-tuong-tai_page"]').remove();
    $body.data('tai', 0);
    
    khoiTaoTatMoDoiTuong($('[data-mo-doi-tuong]'));

    $('[data-chuc-nang="dang-nhap"]').on('click', function () {
        moPopupDangNhap();
    });
    khoiTaoDangXuat($('[data-chuc-nang="dang-xuat"]'));
});

function moBieuTuongTai($item) {
    var offset, bottom, left, $khungTai;
    if (typeof ($item) === 'undefined') {
        $item = $body;
        $khungTai = $('<article class="bieu-tuong-tai-lct bieu-tuong-tai-lct_page"><i></i><i></i><i></i><i></i><i></i><i></i></article>');
    }
    else {
        var offset = $item.offset();
        var bottom = $body.height() - offset.top - $item.height();
        var left = offset.left;

        $khungTai = $('<article style="left: ' + left + 'px;bottom: ' + bottom + 'px" class="bieu-tuong-tai-lct"><i></i><i></i><i></i><i></i><i></i><i></i></article>');
    }

    $body.append($khungTai);
    $item.addClass('item-tai-lct');
    $body.data('tai', $body.data('tai') + 1);
    $body.addClass('dang-tai-lct');

    $khungTai.tat = function () {
        var slTai = $body.data('tai');
        if (slTai == 1) {
            $body.removeClass('dang-tai-lct');
        }

        $body.data('tai', slTai - 1);
        $item.removeClass('item-tai-lct');
        $khungTai.remove();
    }
    return $khungTai;
}

/*
	Bật tắt đối tượng Target
*/
function khoiTaoTatMoDoiTuong($danhSachNut, laChucNang) {
    $danhSachNut.off('click.tat_mo').on('click.tat_mo', function (e) {
        //Lấy đối tượng 
        // $nut: nút nhấn
        // $doiTuong: đối tượng popup sẽ được hiển thị
        var $nut = $(this);
        var $doiTuong = $('[data-doi-tuong="' + $nut.attr('data-mo-doi-tuong') + '"]');
        
        //Xử lý sự kiện click của nút nhấn
        if ($doiTuong.is(':visible')) {
            $doiTuong.hide().removeClass('mo');
            return;
        }
        
        $doiTuong.show().addClass('mo');
        if (laChucNang !== true) {
            $doiTuong.on('click', function (e) {
                e.stopPropagation();
            });
        }

        //Xử lý sự kiện nhấn chuột ra ngoài đối tượng
        setTimeout(function () {
            $(document).one('click', function (e) {
                $doiTuong.hide().removeClass('mo');
            })
        });
    });
}

/*
    Lấy giá trị querystring
*/
function layQueryString(key) {
    var danhSach = window.location.search.substr(1).split('&');
    
    var soLuong = danhSach.length;
    for (var i = 0; i < soLuong; i++) {
        var query = danhSach[i].split('=');

        if (query[0] == key) {
            return query[1] || null;
        }
    }
    return 'undefined';
}

/*
    Popup
*/
//Popup full
function layPopupFull(thamSo) {
    if (typeof (thamSo) === 'undefined')
    {
        thamSo = {};
    }
    var id = ('id' in thamSo) ? thamSo.id : 'popup_full';
    var zIndex = ('z-index' in thamSo) ? thamSo['z-index'] : '19';
    var esc = 'esc' in thamSo && !thamSo.esc

    var $popupFull = $('#' + id);

    if ($popupFull.length == 0) {
        $popupFull = $(
            '<article id="' + id + '" style="z-index: ' + zIndex + ';" class="popup-full-container">\
                <section class="khung-tat"></section>\
                <section class="popup-full">\
                    <article id="noi_dung_popup" class="khung-noi-dung">\
                    </article>\
                </section>\
            </article>');

        $popupFull.find('.khung-tat').on('click', function () {
            if ($popupFull.is('[data-esc]')) {
                $popupFull.tat();
            }
        });

        $body.prepend($popupFull);
    }

    if (esc) {
        $popupFull.removeAttr('data-esc');
    }
    else {
        $popupFull.attr('data-esc', '');
    }

    $popupFull.mo = function () {
        $popupFull.addClass('popup-mo');
        $(document).on('keydown.tat_popup', function (e) {
            if ($popupFull.is('[data-esc]')) {
                if (e.keyCode == 27) {
                    $popupFull.tat();
                }
            }
        });
        $body.addClass('khong-scroll');
    }

    $popupFull.tat = function () {
        $popupFull.removeClass('popup-mo');
        $(document).off('keydown.tat_popup');
        $body.removeClass('khong-scroll');
        $(this).trigger('tat');
    };

    return $popupFull;
}

/*
    Có 2 cách để đưa nội dung cho popup
        1. Chỉ định html cho popup
            html: Bắt buộc
                Hiển thị html trong popup
        2. Sử dụng ajax để load nội dung cho popup
            url: Bắt buộc
                Đường dẫn để lấy nội dung
            type: Mặc định: GET
                Phương thức request
            data: Mặc định: rỗng
                Dữ liệu request
                Có 2 loại dữ liệu truyền vào
                    Mảng
                    Hàm trả về mảng
            thanhCong: Mặc định: không thực hiện
                Hàm thực hiện lúc lấy nội dung thành công
                Tham số truyền cho hàm thành công là nội dung được lấy về
            thatBai: Mặc định: không thực hiện
                Hàm thực hiện lúc lấy nội dung thất bại
            hoanTat: Mặc định: không thực hiện
                Hàm thực hiện lúc lấy nội dung hoàn tất

    Một số thiết lập bổ sung
        width: Mặc định: 90% màn hình
            Chiều ngang của nội dung popup
        height: Mặc định: auto
            Chiều cao của nội dung popup
        esc: Mặc định: true
            Cho phép bấm ra ngoài là tắt popup
*/
function moPopupFull(thamSo) {
    if (typeof thamSo === 'undefined') {
        thamSo = {};
    }

    if ('html' in thamSo) {
        $popup = layPopupFull({
            esc: 'esc' in thamSo ? thamSo.esc : true
        });

        if ('tat' in thamSo) {
            $popup.one('tat', function () {
                thamSo.tat();
            });
        }

        $noiDungPopup = $popup.find('#noi_dung_popup');

        $noiDungPopup.css('width', 'width' in thamSo ? thamSo.width : '90vw');
        $noiDungPopup.css('height', 'height' in thamSo ? thamSo.height : 'auto');

        $noiDungPopup.html(thamSo.html);

        $noiDungPopup.tat = $popup.tat;
        $noiDungPopup.mo = $popup.mo;

        if ('thanhCong' in thamSo) {
            thamSo.thanhCong($noiDungPopup);
        }

        $popup.mo();
    }
    else if ('url' in thamSo) {
        var $tai = moBieuTuongTai();
        $.ajax({
            url: thamSo.url,
            type: 'type' in thamSo ? thamSo.method : 'GET',
            data: 'data' in thamSo ? (typeof thamSo.data == 'function' ? thamSo.data() : thamSo.data) : {},
            dataType: 'JSON'
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai == 0) {
                $popup = layPopupFull({
                    esc: 'esc' in thamSo ? thamSo.esc : true
                });

                $noiDungPopup = $popup.find('#noi_dung_popup');

                $noiDungPopup.css({
                    width: 'width' in thamSo ? thamSo.width : '90vw',
                    height: 'height' in thamSo ? thamSo.height : 'auto'
                });
                $noiDungPopup.html(data.ketQua);

                $noiDungPopup.tat = $popup.tat;
                $noiDungPopup.mo = $popup.mo;
                
                if ('thanhCong' in thamSo) {
                    thamSo.thanhCong($noiDungPopup);
                }

                $popup.mo();

                if ('tat' in thamSo) {
                    $popup.one('tat', function () {
                        thamSo.tat();
                    });
                }
            }
            else {
                if ('thatBai' in thamSo) {
                    thamSo.thatBai();
                }
                else {
                    moPopupThongBao(data);
                }
            }
        }).fail(function () {
            if ('thatBai' in thamSo) {
                thamSo.thatBai();
            }
            else {
                moPopupThongBao('Mở chức năng thất bại');
            }
        }).always(function () {
            if ('hoanTat' in thamSo) {
                thamSo.hoanTat();
            }
        });
    }
}

/*
    tieuDe: Không bắt buộc
        Tiêu đề của thông báo
    thongBao: Không bắt buộc
        Đoạn thông báo
    bieuTuong: Không bắt buộc (chỉ sử dụng được khi có thông báo)
        Biểu tượng trước thông báo
        Gồm: thanh-cong, nguy-hiem, thong-tin, canh-bao, hoi
    nut: Mặc định: Nút thoát
        Danh sách nút xử lý ở thông báo
        Gồm:
            ten: Mặc định: Nút xử lý
                Tên của nút
            loai: Mặc định: chap-nhan
                Loại nút (chap-nhan, thong-tin, can-than,....)
            * Có 2 loại xử lý:
                href: Không bắt buộc
                    Nút đường dẫn
                    Ưu tiên hơn nếu có cả href & xuLy
                xuLy: Mặc định: Tắt popup
                    Xử lý khi nhấn vào nút
                    Trả về false nếu muốn chặn tắt popup
    esc: Mặc định: true
        Cho phép bấm ra ngoài là tắt popup
*/
function moPopup(thamSo) {
    if (typeof thamSo === 'undefined') {
        thamSo = {};
    }

    $popup = layPopupFull({
        id: 'popup_thong_bao',
        'z-index': '20',
        esc: 'esc' in thamSo ? thamSo.esc : true
    });

    if ('tat' in thamSo) {
        $popup.one('tat', function () {
            thamSo.tat();
        });
    }

    $noiDungPopup = $popup.find('#noi_dung_popup');

    $noiDungPopup.css({
        width: 'auto',
        height: 'auto'
    })

    $noiDungPopup.html('\
        <article class="hop hop-1-vien">\
            <section class="tieu-de" style="padding: 0 5px;">\
            </section>\
            <article class="noi-dung lct-form">\
                <ul class="danh-sach-hien-thi">\
                </ul>\
                <section class="danh-sach-nut khung-button">\
                </section>\
            </article>\
        </article>\
    ');

    var
        $tieuDe = $noiDungPopup.find('.tieu-de'),
        $danhSachHienThi = $noiDungPopup.find('.danh-sach-hien-thi'),
        $danhSachNut = $noiDungPopup.find('.danh-sach-nut');

    if ('tieuDe' in thamSo) {
        $tieuDe.text(thamSo.tieuDe);
    }
    else {
        $tieuDe.remove();
    }

    if ('thongBao' in thamSo) {
        var htmlBieuTuong = '';

        if ('bieuTuong' in thamSo &&
            $.inArray(thamSo.bieuTuong, ['thanh-cong', 'nguy-hiem', 'thong-tin', 'canh-bao', 'hoi']) !== -1) {
            htmlBieuTuong = '<span class="bieu-tuong ' + thamSo.bieuTuong + '"></span>';
        }

        $danhSachHienThi.append(
            '<li class="thong-bao">' +
                htmlBieuTuong +
                '<p>' + thamSo.thongBao + '</p>' +
            '</li>'
        );
    }

    if ($danhSachHienThi.children().length == 0) {
        $danhSachHienThi.remove();
    }

    if ('nut' in thamSo) {
        $(thamSo.nut).each(function () {
            var n = this;

            var $nut;

            if ('href' in n) {
                $nut = '<a href="' + n.href + '" class="button ' + (n.loai || 'chap-nhan') + '">' + (n.ten || 'Nút xử lý') + '</a>'
            }
            else {
                $nut = $('<button class="' + (n.loai || 'chap-nhan') + '">' + (n.ten || 'Nút xử lý') + '</button>');

                $nut.on('click', function () {
                    if ('xuLy' in n) {
                        if (n.xuLy() == false) {
                            return;
                        }
                    }
                    $popup.tat();
                });
            }

            $danhSachNut.append($nut);
        })
    }

    if ($danhSachNut.children().length == 0) {
        var $nut = $('<button class="chap-nhan">Tắt</button>');

        $nut.on('click', function () {
            $popup.tat();
        });

        $danhSachNut.append($nut);
    }
    
    $popup.mo();
}

function xuatKetQua(obj, macDinh) {
    if (obj === null) {
        return macDinh;
    }
    if (typeof (obj) === 'object') {
        return obj.join('<br />');
    }
    return obj || macDinh;
}

function moPopupThongBao(ketQua) {
    if (typeof (ketQua) === 'object') {
        switch (ketQua.trangThai) {
            case 0:
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: xuatKetQua(ketQua.ketQua, 'Thực hiện thành công'),
                    bieuTuong: 'thanh-cong'
                });
                break;
            case 1:
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: xuatKetQua(ketQua.ketQua, 'Thực hiện thất bại. Không có dữ liệu trùng khớp'),
                    bieuTuong: 'canh-bao'
                });
                break;
            case 2:
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: xuatKetQua(ketQua.ketQua, 'Thực hiện thất bại. Gặp lỗi xử lý truy vấn'),
                    bieuTuong: 'nguy-hiem'
                });
                break;
            case 3:
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: xuatKetQua(ketQua.ketQua, 'Thực hiện thất bại. Dữ liệu không đúng ràng buộc'),
                    bieuTuong: 'nguy-hiem'
                });
                break;
            case 4:
                moPopupDangNhap();
                break;
            default:
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: xuatKetQua(ketQua.ketQua, 'Thực hiện thất bại. Không xác định lỗi'),
                    bieuTuong: 'nguy-hiem'
                });
                break;
        }
    }
    else {
        moPopup({
            tieuDe: 'Thông báo',
            thongBao: xuatKetQua(ketQua, 'Thực hiện thất bại. Không xác định lỗi'),
            bieuTuong: 'nguy-hiem'
        });
    }
}

/*
    Xử lý đăng xuất
*/
function khoiTaoDangXuat($btnDangXuat) {
   
}

function moPopupDangNhap(thamSo) {
    if (typeof (thamSo) === 'undefined') {
        thamSo = {};
    }

    moPopupFull({
        url: '/NguoiDung/_FormDangNhap/',        
        width: '500px',
        thanhCong: function ($popup) {
            var $form = $popup.find('.lct-form');
            khoiTaoLCTForm($form, {
                submit: function (e) {
                    $.ajax({
                        url: $form.attr('action'),
                        method: $form.attr('method'),
                        data: layDataLCTForm($form)
                    }).done(function (data) {
                        if (data.trangThai == 0) {
                            if ('thanhCong' in thamSo) {
                                thamSo.thanhCong();
                                $popup.tat();
                            }
                            else {
                                location.reload();
                            }
                        }
                        else if (data.trangThai == 5) {
                            moPopup({
                                tieuDe: 'Thông báo',
                                thongBao: data.ketQua,
                                bieuTuong: 'thong-tin'
                            });
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
                            thongBao: 'Lỗi ajax',
                            nut: [{
                                ten: 'Về trang chủ',
                                xuLy: function () {
                                    window.location = '/';
                                }
                            }],
                            esc: false,
                            bieuTuong: 'nguy-hiem'
                        })
                    })
                }
            });
        }
    });
<<<<<<< HEAD
=======
}

function hienThiCode($khungCode) {
    $khungCode.each(function () {
        hljs.highlightBlock(this);
    })
>>>>>>> 99ff389b2dc3703f1f3f6efd01d18cb5e668eb77
}