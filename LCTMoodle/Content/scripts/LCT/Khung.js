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

    khoiTaoLCTKhung_Lich($('#lich_aside'));

    thongBaoTrang_LCT();

    khoiTaoScroll($('#tin_nhan_lct'), 45);

    khoiTaoNhanTin();
});

function khoiTaoScroll($khung, nhay) {
    $khung.on('mousewheel', function (e) {
        e.preventDefault();
        e = e.originalEvent;
        var d = e.wheelDelta;

        this.scrollTop += (d < 0 ? 1 : -1) * (nhay || 30);
    })
}

function thongBaoTrang_LCT() {
    var tb = layQueryString('tb');
    
    if (tb) {
        moPopup({
            tieuDe: 'Thông báo',
            thongBao: decodeURIComponent(tb.replace(/\+/g, ' '))
        });
    }
}

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
    var danhSach = location.search.substr(1).split('&');
    
    var soLuong = danhSach.length;
    for (var i = 0; i < soLuong; i++) {
        var query = danhSach[i].split('=');

        if (query[0] == key) {
            return query[1] || null;
        }
    }
    return;
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
    var esc = !('esc' in thamSo) || thamSo.esc

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

        $body.append($popupFull);
    }

    if (esc) {
        $popupFull.attr('data-esc', '');
    }
    else {
        $popupFull.removeAttr('data-esc');
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
        id, z-index: Trường hợp muốn trùng
*/
function moPopupFull(thamSo) {
    if (typeof thamSo === 'undefined') {
        thamSo = {};
    }

    var popupData = {}
    if ('esc' in thamSo) {
        popupData.esc = thamSo.esc;
    }
    if ('id' in thamSo) {
        popupData.id = thamSo.id;
    }
    if ('z-index' in thamSo) {
        popupData['z-index'] = thamSo['z-index'];
    }    

    if ('html' in thamSo) {
        $popup = layPopupFull(popupData);

        if ('tat' in thamSo) {
            $popup.one('tat', function () {
                thamSo.tat();
            });
        }

        $noiDungPopup = $popup.find('#noi_dung_popup');

        $noiDungPopup.css('width', 'width' in thamSo ? thamSo.width : 'auto');
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
                    width: 'width' in thamSo ? thamSo.width : 'auto',
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
    id, z-index: Trường hợp muốn trùng
*/
function moPopup(thamSo) {
    if (typeof thamSo === 'undefined') {
        thamSo = {};
    }

    var popupData = {}
    popupData.esc = 'esc' in thamSo ? thamSo.esc : true;
    popupData.id = 'id' in thamSo ? thamSo.id : 'popup_thong_bao';
    popupData['z-index'] = 'z-index' in thamSo ? thamSo['z-index'] : '20';

    $popup = layPopupFull(popupData);

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
                    var $tai = moBieuTuongTai($form);
                    $.ajax({
                        url: $form.attr('action'),
                        method: $form.attr('method'),
                        data: layDataLCTForm($form)
                    }).always(function () {
                        $tai.tat();
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
}

function hienThiCode($khungCode) {
    $khungCode.each(function () {
        hljs.highlightBlock(this);
    })
}

function khoiTaoLCTKhung_Lich($lich) {
    $lich.find('.thang-nam .thang-truoc, .thang-nam .thang .truoc').on('click', function () {
        var thang = parseInt($lich.attr('data-thang-ht'));
        if (thang == 1) {
            $lich.attr('data-thang-ht', '12');
            $lich.attr('data-nam-ht', parseInt($lich.attr('data-nam-ht')) - 1);
        }
        else {
            $lich.attr('data-thang-ht', thang - 1);
        }
        capNhatLich($lich);
    });
    $lich.find('.thang-nam .thang-sau, .thang-nam .thang .sau').on('click', function () {
        var thang = parseInt($lich.attr('data-thang-ht'));
        if (thang == 12) {
            $lich.attr('data-thang-ht', '1');
            $lich.attr('data-nam-ht', parseInt($lich.attr('data-nam-ht')) + 1);
        }
        else {
            $lich.attr('data-thang-ht', thang + 1);
        }
        capNhatLich($lich);
    });
    $lich.find('.thang-nam .nam .truoc').on('click', function () {
        $lich.attr('data-nam-ht', parseInt($lich.attr('data-nam-ht')) - 1);
        capNhatLich($lich);
    });
    $lich.find('.thang-nam .nam .sau').on('click', function () {
        $lich.attr('data-nam-ht', parseInt($lich.attr('data-nam-ht')) + 1);
        capNhatLich($lich);
    });
    $lich.find('.khung-thang i').on('click', function () {
        var
            ngay = $(this).attr('data-value'),
            thang = $lich.attr('data-thang-ht'),
            nam = $lich.attr('data-nam-ht');

        $inputVuaFocus.val(ngay + '/' + thang + '/' + nam).change();
        $lich.attr('data-ngay', ngay);
        $lich.attr('data-thang', thang);
        $lich.attr('data-nam', nam);

        $lich.find('i.chon').removeClass('chon');
        $lich.find('i[data-value="' + ngay + '"]').addClass('chon');
    });

    var date = new Date();
    var ngayHienTai = date.getDate();
    var thangHienTai = date.getMonth() + 1;
    var namHienTai = date.getFullYear();

    $lich.attr('data-thang-ht', thangHienTai);
    $lich.attr('data-nam-ht', namHienTai);
    capNhatLich($lich);

    function capNhatLich($lich) {
        var
            thang = $lich.attr('data-thang-ht'),
            nam = $lich.attr('data-nam-ht');
        $lich.find('.thang-nam .thang').attr('data-value', thang);
        $lich.find('.thang-nam .nam').attr('data-value', nam);
        // Chọn ngày 1 -> thứ bắt đầu & xử lý để thứ bắt đầu là 2 (0), kết thúc là chủ nhật (6)
        var thuBatDau = new Date(nam, thang - 1, 1).getDay() - 1;
        if (thuBatDau == -1) {
            // Chủ nhật
            thuBatDau = 6
        }

        // Ngày âm 1 của tháng sau (0) là ngày cuối cùng của tháng cần tìm
        var soNgayTrongThang = new Date(nam, thang, 0).getDate();

        var
            viTri = 0, // Lưu vi trí
            ngay = 1, // Lưu ngày xuất ra
            mangNgay = []; // Lưu mảng ngày

        // Lấy dòng đầu
        // Kiểm tra số dòng dư ở dòng cuối
        // Nếu tổng số ô chiếm trên 5 dòng
        var soOChiem = (thuBatDau + soNgayTrongThang);
        if (soOChiem / 7 > 5) {
            // Xuất ngày dư ra ở hàng đầu
            // Lấy ngày bắt đầu ở dòng dư
            ngayBatDauDu = soNgayTrongThang - (soOChiem % 7) + 1;
            // Xuất các ngày dư
            for (var i = ngayBatDauDu; i <= soNgayTrongThang; i++) {
                mangNgay.push(i);
                viTri++;
            }
            // Thay thế số ngày trong tháng = (ngày bắt đầu dư - 1)
            // để phần còn lại chỉ xuất tới phần dư
            soNgayTrongThang = ngayBatDauDu - 1;
        }
        // Xuất phần còn lại của dòng
        // Xuất phần không có trong tháng
        while (viTri < thuBatDau) {
            mangNgay.push(0);
            viTri++;
        }

        // Xuất toàn bộ cho đến khi hết ngày
        while (ngay <= soNgayTrongThang) {
            mangNgay.push(ngay++);
        }

        viTri = 0;
        $lich.find('tbody i').each(function () {
            if (mangNgay[viTri] > 0) {
                this.setAttribute('data-value', mangNgay[viTri]);
            }
            else {
                this.removeAttribute('data-value');
            }
            viTri++;
        });

        $lich.find('i').removeClass('chon');

        if (thang == thangHienTai && nam == namHienTai) {
            $lich.find('i[data-value="' + ngayHienTai + '"]').addClass('chon');
        }
    }
}

function khoiTaoNutMoPopupTapTin($nuts) {
    $nuts.off('click.popup_tap_tin').on('click.popup_tap_tin', function (e) {
        e.preventDefault();
        var $nut = $(this);

        moPopupFull({
            url: '/Popup' + $nut.attr('href'),
            thanhCong: function ($popup) {
                var $khung = $popup.find('[data-doi-tuong="khung-noi-dung"]');
                
                switch ($khung.attr('data-loai')) {
                    case 'code':
                        hienThiCode($khung.find('pre code'));
                    default:
                        break;
                }
            }
        });
    })
}

function coHoTroXem(duoi) {
    return $.inArray(duoi.substr(1), [
        'jpg',
        'jpeg',
        'png',
        'txt',
        'cs',
        'css',
        'js',
        'rb',
        'php',
        'cshtml',
        'cpp',
        'html',
        'haml'
    ]) !== -1;
}

/*
    $danhSach: Danh sách chứa nội dung
    $khungPhanTrang: Khung phân trang
    thamSo:
        url: Bắt buộc
        data: 
            Dữ liệu truyền đi khi chuyển trang
            Có thể là {} hoặc function return {}
        khoiTaoDanhSach:
            Hàm khởi tạo danh sách khi chuyển trang
            Tham số là danh sách
*/
function khoiTaoPhanTrang_ChonTrang($danhSach, $khungPhanTrang, thamSo) {
    if (typeof (thamSo) === 'undefined' || typeof (thamSo.url) === 'undefined') {
        return;
    }

    $khungPhanTrang.find('[data-trang="' + (thamSo.trang || 1) + '"]').addClass('chon');

    $khungPhanTrang.find('[data-chuc-nang="chuyen-trang"]').on('click', function () {
        var $nut = $(this);

        if ($nut.hasClass('chon')) {
            return;
        }

        var data;
        if ('data' in thamSo) {
            if (typeof (thamSo.data) === 'function') {
                data = thamSo.data();
            }
            else {
                data = thamSo.data;
            }
        }

        if (typeof (data) !== 'object') {
            data = {}
        }

        data.trang = $nut.attr('data-trang');

        var $tai = moBieuTuongTai($danhSach);
        $.ajax({
            url: thamSo.url,
            data: data,
            dataType: 'JSON'
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai == 0) {
                $danhSach.html(data.ketQua);

                if ('khoiTaoDanhSach' in thamSo) {
                    thamSo.khoiTaoThamSo($danhSach);
                }

                $khungPhanTrang.find('.chon').removeClass('chon');
                $nut.addClass('chon');
            }
            else {
                moPopupThongBao(data);
            }
        }).fail(function () {
            moPopupThongBao('Mở trang thất bại');
        });
    });
}

/*
    $danhSach: Danh sách chứa nội dung
    thamSo:
        url: Bắt buộc
        data: 
            Dữ liệu truyền đi khi lấy trang
            Có thể là {} hoặc function return {}
        khoiTaoDanhSach:
            Hàm khởi tạo danh sách khi lấy trang về
            Tham số là danh sách
        suKien: Mặc định là scroll
            ('scroll' hoặc $...)
            Giá trị có thể là scroll hoặc 1 đối tượng nhấn
            Nếu là scroll, khi scroll xuống cuối danh sách sẽ load thêm
            Nếu là đối tượng nhấn, khi nhấn nút sẽ load thêm
            
*/
function khoiTaoPhanTrang_SuKien($danhSach, thamSo) {
    if (typeof (thamSo) === 'undefined' || typeof (thamSo.url) === 'undefined') {
        return;
    }

    $danhSach.attr('data-trang', 1);
    $danhSach.data('dang-lay', false);

    if ('suKien' in thamSo && thamSo.suKien != 'scroll') {
        thamSo.suKien.on('click', function () {
            layTrangMoi();
        })
    }
    else {
        $danhSach.on('scroll', function () {
            if ($danhSach.scrollTop() + $danhSach.innerHeight() >= this.scrollHeight) {
                layTrangMoi();
            }
        })
    }

    function layTrangMoi() {
        if ($danhSach.data('dang-lay')) {
            return;
        }

        var data;
        if ('data' in thamSo) {
            if (typeof (thamSo.data) === 'function') {
                data = thamSo.data();
            }
            else {
                data = thamSo.data;
            }
        }

        if (typeof (data) !== 'object') {
            data = {}
        }

        data.trang = $danhSach.attr('data-trang');

        $danhSach.data('dang-lay', true);
        $.ajax({
            url: thamSo.url,
            data: data,
            dataType: 'JSON'
        }).always(function () {
            $danhSach.data('dang-lay', false);
        }).done(function (data) {
            if (data.trangThai == 0) {
                var $ds = $(data.ketQua);

                if ('khoiTaoDanhSach' in thamSo) {
                    thamSo.khoiTaoThamSo($ds);
                }
                
                $danhSach.append($ds).attr('data-trang', data.trang + 1);
            }
            else {
                moPopupThongBao(data);
            }
        }).fail(function () {
            moPopupThongBao('Mở trang thất bại');
        });
    }
}

function khoiTaoNhanTin() {
    $('[data-chuc-nang="nhan-tin"]').on('click', function (e) {
        moPopupFull({
            url: '/NguoiDung/_NhanTin',
            width: '500px',
            thanhCong: function () {
                var $form = $('#nhan_tin');
                khoiTaoLCTForm($form, {
                    submit: function () {
                        $tenTaiKhoanNguoiDung = $('[name="TenTaiKhoanNguoiNhan"]').val();

                        window.location = "/NguoiDung/ChiTietTinNhan?tenTaiKhoanKhach=" + $tenTaiKhoanNguoiDung;
                    }
                });
            }
        });
    });
}

function coTheNhinThay($item) {
    var topW = $body.scrollTop();
    var heightW = window.innerHeight;
    var bottomW = topW + heightW;

    var topI = $item.offset().top;
    var heightI = $item.height()
    var bottomI = topI + heightI;

    if (heightW > heightI) {
        if ((topW < topI && topI < bottomW) ||
            (topW < bottomI && bottomI < bottomW)) {
            return true;
        }
    }
    else if ((topI < bottomW && bottomW < bottomI) ||
        (topI < topW && topW < bottomI)) {
        return true;
    }

    return false;
}