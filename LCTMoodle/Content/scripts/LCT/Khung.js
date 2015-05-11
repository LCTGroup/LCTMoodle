function css_browser_selector(u) { var ua = u.toLowerCase(), is = function (t) { return ua.indexOf(t) > -1 }, g = 'gecko', w = 'webkit', s = 'safari', o = 'opera', m = 'mobile', h = document.documentElement, b = [(!(/opera|webtv/i.test(ua)) && /msie\s(\d)/.test(ua)) ? ('ie ie' + RegExp.$1) : is('firefox/2') ? g + ' ff2' : is('firefox/3.5') ? g + ' ff3 ff3_5' : is('firefox/3.6') ? g + ' ff3 ff3_6' : is('firefox/3') ? g + ' ff3' : is('gecko/') ? g : is('opera') ? o + (/version\/(\d+)/.test(ua) ? ' ' + o + RegExp.$1 : (/opera(\s|\/)(\d+)/.test(ua) ? ' ' + o + RegExp.$2 : '')) : is('konqueror') ? 'konqueror' : is('blackberry') ? m + ' blackberry' : is('android') ? m + ' android' : is('chrome') ? w + ' chrome' : is('iron') ? w + ' iron' : is('applewebkit/') ? w + ' ' + s + (/version\/(\d+)/.test(ua) ? ' ' + s + RegExp.$1 : '') : is('mozilla/') ? g : '', is('j2me') ? m + ' j2me' : is('iphone') ? m + ' iphone' : is('ipod') ? m + ' ipod' : is('ipad') ? m + ' ipad' : is('mac') ? 'mac' : is('darwin') ? 'mac' : is('webtv') ? 'webtv' : is('win') ? 'win' + (is('windows nt 6.0') ? ' vista' : '') : is('freebsd') ? 'freebsd' : (is('x11') || is('linux')) ? 'linux' : '', 'js']; c = b.join(' '); h.className += ' ' + c; return c; }; css_browser_selector(navigator.userAgent);

$body = null;
var mangTam = [];

/*
	Khởi tạo
*/
$(function () {
    $body = $('body');
    $body.removeClass('tai');
    khoiTao_TatMoDoiTuong($('[data-show-target]'));
});

/*
	Bật tắt đối tượng Target
*/
function khoiTao_TatMoDoiTuong($doiTuong) {
    $doiTuong.on('click', function (e) {        
        //Lấy đối tượng 
        // $obj: đối tượng nút nhấn
        // $target: đối tượng popup sẽ được hiển thị
        $obj = $(this);
        $target = $('[data-target="' + $obj.attr('data-show-target') + '"');
        
        //Xử lý sự kiện click của nút nhấn
        if ($target.is(':visible')) {
            $target.hide();           
        } else {
            $target.show();
        }

        //Xử lý sự kiện nhấn chuột ra ngoài đối tượng
        $(document).on('click', function (e) {
            if ($target.has($(e.target)).length == 0 && !$(e.target).is($obj) && !$(e.target).is($target) && $obj.has($(e.target)).length == 0) {
                $target.hide();
            }
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
    $popupFull = $('#popup_full');

    if ($popupFull.length == 0) {
        $popupFull = $(
            '<article id="popup_full" class="popup-full-container">\
                <section class="khung-tat"></section>\
                <section class="popup-full">\
                    <article id="noi_dung_popup" class="khung-noi-dung">\
                    </article>\
                </section>\
            </article>');

        $popupFull.find('.khung-tat').on('click', function () {
            if ($popupFull.is('[data-esc]')) {
                $popupFull.trigger('Tat');
            }
        });

        $popupFull.on('Mo', function () {
            $popupFull.show();
            $(document).on('keydown.tat_popup', function (e) {
                if ($popupFull.is('[data-esc]')) {
                    e = e || window.event;
                    if (e.keyCode == 27) {
                        $popupFull.trigger('Tat');
                    }
                }
            });
            $body.addClass('khong-scroll');
        });

        $popupFull.on('Tat', function () {
            $popupFull.hide();
            $(document).off('keydown.tat_popup');
            $body.removeClass('khong-scroll');
        });

        $body.prepend($popupFull);
    }

    if ('esc' in thamSo && !thamSo.esc) {
        $popupFull.removeAttr('data-esc');
    }
    else {
        $popupFull.attr('data-esc', '');
    }

    return $popupFull;
}

/*
    Có 2 cách để đưa nội dung cho popup
        1. Chỉ định html cho popup
            html: 
                Bắt buộc
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

        $noiDungPopup = $popup.find('#noi_dung');

        $noiDungPopup.css('width', 'width' in thamSo ? thamSo.width : '90vw');
        $noiDungPopup.css('height', 'height' in thamSo ? thamSo.height : 'auto');

        $noiDungPopup.html(thamSo.html);

        $popup.trigger('Mo');
    }
    else if ('url' in thamSo) {
        $.ajax({
            url: thamSo.url,
            type: 'type' in thamSo ? thamSo.method : 'GET',
            data: 'data' in thamSo ? (typeof thamSo.data == 'function' ? thamSo.data() : thamSo.data) : {},
            contentType: 'JSON'
        }).done(function (data) {
            if (data.trangThai == 0) {
                $popup = layPopupFull({
                    esc: 'esc' in thamSo ? thamSo.esc : true
                });

                $noiDungPopup = $popup.find('#noi_dung_popup');

                $noiDungPopup.css('width', 'width' in thamSo ? thamSo.width : '90vw');
                $noiDungPopup.css('height', 'height' in thamSo ? thamSo.height : 'auto');

                $noiDungPopup.html(data.ketQua);

                if ('thanhCong' in thamSo) {
                    thamSo.thanhCong($noiDungPopup);
                }

                $popup.trigger('Mo');
            }
            else {
                if ('thatBai' in thamSo) {
                    thamSo.thatBai();
                }
                else {
                    alert('Thất bại');
                }
            }
        }).fail(function () {
            if ('thatBai' in thamSo) {
                thamSo.thatBai();
            }
            else {
                alert('Thất bại');
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
    nut: Mặc định: Nút thoát
        Danh sách nút xử lý ở thông báo
        Gồm:
            ten: Mặc định: Nút xử lý
                Tên của nút
            loai: Mặc định: chap-nhan
                Loại nút (chap-nhan, can-than, ...)
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
        esc: 'esc' in thamSo ? thamSo.esc : true
    });

    $noiDungPopup = $popup.find('#noi_dung_popup');

    $noiDungPopup.html('\
        <article class="hop hop-1-vien">\
            <section class="tieu-de">\
            </section>\
            <article class="noi-dung lct-form">\
                <ul class="danh-sach-hien-thi">\
                </ul>\
                <section class="danh-sach-nut lct-khung-button">\
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
        $danhSachHienThi.append('\
            <li class="thong-bao">\
                ' + thamSo.thongBao + '\
            </li>\
        ');
    }

    if ($danhSachHienThi.children().length == 0) {
        $danhSachHienThi.remove();
    }

    if ('nut' in thamSo) {
        $(thamSo.nut).each(function () {
            var n = this;

            var $nut = $('<button class="' + (n.loai || 'chap-nhan') + '">' + (n.ten || 'Nút xử lý') + '</button>');

            $nut.on('click', function () {
                if ('xuLy' in n) {
                    if (n.xuLy() == false) {
                        return;
                    }
                }
                $popup.trigger('Tat');
            });

            $danhSachNut.append($nut);
        })
    }

    if ($danhSachNut.children().length == 0) {
        var $nut = $('<button class="chap-nhan">Tắt</button>');

        $nut.on('click', function () {
            $popup.trigger('Tat');
        });

        $danhSachNut.append($nut);
    }
    
    $popup.trigger('Mo');
}