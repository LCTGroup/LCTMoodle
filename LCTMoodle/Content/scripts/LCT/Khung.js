function css_browser_selector(u) { var ua = u.toLowerCase(), is = function (t) { return ua.indexOf(t) > -1 }, g = 'gecko', w = 'webkit', s = 'safari', o = 'opera', m = 'mobile', h = document.documentElement, b = [(!(/opera|webtv/i.test(ua)) && /msie\s(\d)/.test(ua)) ? ('ie ie' + RegExp.$1) : is('firefox/2') ? g + ' ff2' : is('firefox/3.5') ? g + ' ff3 ff3_5' : is('firefox/3.6') ? g + ' ff3 ff3_6' : is('firefox/3') ? g + ' ff3' : is('gecko/') ? g : is('opera') ? o + (/version\/(\d+)/.test(ua) ? ' ' + o + RegExp.$1 : (/opera(\s|\/)(\d+)/.test(ua) ? ' ' + o + RegExp.$2 : '')) : is('konqueror') ? 'konqueror' : is('blackberry') ? m + ' blackberry' : is('android') ? m + ' android' : is('chrome') ? w + ' chrome' : is('iron') ? w + ' iron' : is('applewebkit/') ? w + ' ' + s + (/version\/(\d+)/.test(ua) ? ' ' + s + RegExp.$1 : '') : is('mozilla/') ? g : '', is('j2me') ? m + ' j2me' : is('iphone') ? m + ' iphone' : is('ipod') ? m + ' ipod' : is('ipad') ? m + ' ipad' : is('mac') ? 'mac' : is('darwin') ? 'mac' : is('webtv') ? 'webtv' : is('win') ? 'win' + (is('windows nt 6.0') ? ' vista' : '') : is('freebsd') ? 'freebsd' : (is('x11') || is('linux')) ? 'linux' : '', 'js']; c = b.join(' '); h.className += ' ' + c; return c; }; css_browser_selector(navigator.userAgent);

$body = null;
var mangTam = [];

/*
	Khởi tạo
*/
$(function () {
    $body = $('body');
    init_OnOffTarget();
});

/*
	Bật tắt đối tượng Target
*/
function init_OnOffTarget() {
    $('[data-show-target]').on('click', function (e) {        
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
    Popup
*/
//Popup full
function layPopupFull() {
    $popupFull = $('#popup_full');

    if ($popupFull.length == 0) {
        $popupFull = $(
            '<section id="popup_full" class="popup-full">\
                <section class="khung-tat"></section>\
                <section id="noi_dung" class="khung-noi-dung">\
                </section>\
            </section>');

        $popupFull.find('.khung-tat').on('click', function () {
            $popupFull.trigger('Tat');
        });

        $popupFull.on('Mo', function () {
            $popupFull.show();
            $(document).on('keydown.tat_popup', function (e) {
                e = e || window.event;
                if (e.keyCode == 27) {
                    $popupFull.trigger('Tat');
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

    return $popupFull;
}

/*
    Tham số gồm:
        url hoặc href của phần tử (bắt buộc),
        width, height,
        data (mảng {} hoặc function),
        thanhCong (function (data: nội dung popup)),
        thatBai (function ()),
        hoanThanh (function ())
*/
function khoiTaoPopupFull($phanTu, thamSo) {
    if (typeof thamSo === 'undefined') {
        thamSo = {};
    }

    $phanTu.on('click', function () {
        $.ajax({
            type: 'method' in thamSo ? thamSo.method : $phanTu.is('[data-method]') ? $phanTu.attr('data-method') : 'GET',
            url: 'url' in thamSo ? thamSo.url : $phanTu.attr('href'),
            data: 'data' in thamSo ? (typeof thamSo.data == 'function' ? thamSo.data() : thamSo.data) : {},
            contentType: 'JSON'
        }).done(function (data) {
            if (data.trangThai == 0) {
                $popup = layPopupFull();

                $popup.trigger('Mo');
                
                $noiDungPopup = $popup.find('#noi_dung');

                $noiDungPopup.css('width', 'width' in thamSo ? thamSo.width : 'auto');
                $noiDungPopup.css('height', 'height' in thamSo ? thamSo.height : 'auto');

                $noiDungPopup.html(data.ketQua);

                if ('thanhCong' in thamSo) {
                    thamSo.thanhCong($noiDungPopup);
                }                
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
            if ('hoanThanh' in thamSo) {
                thamSo.hoanThanh();
            }
        });
    });
}