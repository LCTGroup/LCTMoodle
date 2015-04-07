// Lưu đối tượng vừa focus
$inputVuaFocus = [];


$(function () {
    khoiTaoInput();
    khoiTaoThoiGian();
})

/*
    Khởi tạo input
*/
function khoiTaoInput() {
    $('.lct-form .input').append($('<i class="mac-dinh" title="Mặc định"></i>').on('click', function () {
        //Thẻ chứa
        $chua = $(this).parent();

        // Text, thời gian
        $chua.find('input[type="text"], textarea, input[data-type="lct-thoi-gian"], input[data-type="lct-lich"]').val('');
    }));

    $('.lct-form input[type="checkbox"], .lct-form input[type="radio"]').each(function () {
        $element = $(this);
        $element.wrap('<label class="lct-checkbox-radio-label"></label>');
        $element.after('<u></u>' +$element.attr('data-text'));
    });

    $('.lct-form input[type="file"]').each(function () {
        $element = $(this);
        $element.wrap('<label class="lct-file-label"></label>');
        $element.after('<i></i><u></u><span>' + $element.attr('data-text') + '</span>');
    });
}

/*
    Khởi tạo thời gian
*/

function khoiTaoThoiGian() {
    khoiTaoInputThoiGian('lct-thoi-gian', 'dong_ho_form', khoiTaoForm_DongHo, layGiaTriMacDinh_DongHo, layGiaTri_DongHo);
    khoiTaoInputThoiGian('lct-lich', 'lich_form', khoiTaoForm_Lich, layGiaTriMacDinh_Lich, layGiaTri_Lich);
}

// loai Loại input (lct-thoi-gian, lct-lich)
// id của form nhập (đồng hồ hoặc lịch)
// hàm khởi tạo form lúc chưa có
// hàm xử lý giá trị mặc định
// hàm xử lý lấy giá trị khi đã có
function khoiTaoInputThoiGian(loai, idForm, hamKhoiTao, hamXuLyMacDinh, hamXuLyLayGiaTri) {
    //Lấy/tạo form nhập
    $form = $('#' + idForm);

    //Lấy tất cả input
    $inputs = $('.lct-form input[data-input-type="' + loai + '"]');

    //Không cho điền
    $inputs.on('keypress', function (e) {
        e = e || window.event;
        e.preventDefault();
    });

    //Xử lý focus => hiển thị
    $inputs.on('focus', function (e) {
        e = e || window.event();
        $input = $(e.target);

        //Lấy form
        $form = $('#' + idForm);
        if ($form.length == 0) {
            $form = hamKhoiTao(idForm);
            $('body').prepend($form)
        }

        //Nếu input vừa được focus & focus tiếp => ko xử lý
        if (e.target === $inputVuaFocus[0] && $form.attr('data-trang-thai') == 'hien') {
            return;
        }

        $inputVuaFocus = $input;

        //Lắng nghe sự kiện để ẩn
        //Nếu form ở trạng thái hiện => đang lắng nghe => ko cần
        if ($form.attr('data-trang-thai') == 'an') {
            $('body').on('mousedown.' + loai, function (e) {
                e = e || window.event;
                //Nếu nhấn vào input đang xử lý thì ko làm gì cả
                if (e.target != $input[0]) {
                    //Nếu không phải vừa nhấn vào đối tượng trên form => tắt
                    if ($form.has(e.target).length == 0) {
                        //Nếu đối tượng vừa được nhấn vào có cùng loại với input thì không làm gì cả
                        if ($(e.target).attr('data-input-type') != loai) {
                            $form.attr('data-trang-thai', 'an');
                            $('body').off('mouseup.' + loai);
                            $('body').off('mousedown.' + loai);
                        }
                    }
                }
            });
            $('body').on('mouseup.' + loai, function (e) {
                e = e || window.event;
                //Nếu nhấn vào input đang xử lý thì ko làm gì cả
                if (e.target != $input[0]) {
                    //Nếu không phải vừa nhấn vào đối tượng trên form => tắt
                    if ($form.has(e.target).length != 0) {
                        $input.focus();
                    }
                }
            });
        }

        //Khởi tạo giá trị cho form
        if ($input.val() == '') {
            hamXuLyMacDinh($form, $input);
        }
        else {
            hamXuLyLayGiaTri($form, $input);
        }

        //Đưa form vào vị trí hiển thị
        var viTri = $input.offset();

        //Nếu đang ẩn => hiện lên
        if ($form.attr('data-trang-thai') == 'an') {
            $form.attr('data-trang-thai', 'hien');
            $form.css({
                left: viTri.left + 15,
                top: viTri.top - $form.outerHeight() - 5
            });
        }
        else {
            $form.animate({
                left: viTri.left + 15,
                top: viTri.top - $form.outerHeight() - 5
            }, 500);
        }
    })

    //Xử lý khi nhấn tab
    $inputs.on('keydown', function (e) {
        e = e || window.event;

        if (e.keyCode == 9) {
            $form.attr('data-trang-thai', 'an');
            $('body').off('mouseup.' + loai);
            $('body').off('mousedown.' + loai);
        }
    });
}

/*
    Hàm của đồng hồ
*/

function khoiTaoForm_DongHo(id) {
    $dongHo = $(
        '<article id="' + id + '" class="lct-dong-ho" data-trang-thai="an" data-buoi="" data-gio="" data-phut="">\
            <section class="dong-ho">\
		        <section class="buoi">\
			        <i title="Sáng/Chiều"></i>\
		        </section>\
		        <section class="gio">' +
                    taoTheI('gio') +
		        '</section>\
		        <section class="phut">' +
                    taoTheI('phut') +
		        '</section>\
	        </section>\
        </article>\
    ');

    // Sự kiện khi nhấn vào đồng hồ
    $dongHo.find('.buoi i').on('click', function () {
        if ($dongHo.attr('data-buoi') == 'toi') {
            $dongHo.attr('data-buoi',  'sang');
            $dongHo.find('.buoi i').attr('title', 'Tối');
        }
        else {
            $dongHo.attr('data-buoi', 'toi');
            $dongHo.find('.buoi i').attr('title', 'Sáng');
        }
        $inputVuaFocus.val(layThoiGian($dongHo));
    });
    $dongHo.find('.gio i').on('click', function () {
        $dongHo.attr('data-gio', $(this).attr('data-value'));
        $inputVuaFocus.val(layThoiGian($dongHo));
    });
    $dongHo.find('.phut i').on('click', function () {
        $dongHo.attr('data-phut', $(this).attr('data-value'));
        $inputVuaFocus.val(layThoiGian($dongHo));
    });
    return $dongHo;
}

function layGiaTriMacDinh_DongHo($dongHo, $thoiGianInput) {
    var hienTai = new Date();
    var gio = hienTai.getHours();
    //Kiểm tra sáng / chiều
    if (gio < 12) {
        $dongHo.attr('data-buoi', 'sang');
        $dongHo.attr('data-gio', gio);
    }
    else {
        $dongHo.attr('data-buoi', 'toi');
        $dongHo.attr('data-gio', gio - 12);
    }
    $dongHo.attr('data-phut', hienTai.getMinutes());
    $thoiGianInput.val(layThoiGian($dongHo));

}

function layGiaTri_DongHo($dongHo, $thoiGianInput) {
    var thoiGian = $thoiGianInput.val().split(':');
    //Kiểm tra sáng / chiều
    if (thoiGian[0] < 12) {
        $dongHo.attr('data-buoi', 'sang');
        $dongHo.attr('data-gio', thoiGian[0]);
    }
    else {
        $dongHo.attr('data-buoi', 'toi');
        $dongHo.attr('data-gio', thoiGian[0] - 12);
    }
    $dongHo.attr('data-phut', thoiGian[1]);
}

function taoTheI(loai) {
    var chuoiThe = '';
    var loai, soLuong;
    if (loai == 'gio') {
        loai = ' giờ';
        soLuong = 12;
    }
    else {
        loai = ' phút';
        soLuong = 60;
    }
    for (var i = 1; i <= soLuong; i++) {
        chuoiThe += '<i title="' + i + loai + '" data-value="' + i + '"></i>';
    }
    return chuoiThe;
}

function layThoiGian($dongHo) {
    // Nếu là tối thì + thêm 12 vào giờ
    var gio = parseInt($dongHo.attr('data-gio')) + ($dongHo.attr('data-buoi') == 'toi' ? 12 : 0);
    var phut = parseInt($dongHo.attr('data-phut'));

    //Nếu giờ là 24, phút là 60 thì trả về 0
    if (gio == 24) {
        gio = 0;
    }
    if (phut == 60) {
        phut = 0;
    }
    return gio + ':' + phut;
}

/*
    Hàm của lịch
*/

function khoiTaoForm_Lich(id) {
    $lich = $(
        '<article id="lich_form" class="lct-lich" data-trang-thai="an" data-ngay data-thang data-nam data-thang-ht data-nam-ht>\
		    <section class="thang-nam">\
			    <a href="javascript:void(0)" class="thang-truoc"></a>\
			    <span class="thang" data-value><i class="truoc"></i><i class="sau"></i></span>\
			    -\
			    <span class="nam" data-value><i class="truoc"></i><i class="sau"></i></span>\
			    <a href="javascript:void(0)" class="thang-sau"></a>\
		    </section>\
		    <section class="lich">\
			    <table>\
				    <thead>\
					    <tr>\
						    <th>T2</th>\
						    <th>T3</th>\
						    <th>T4</th>\
						    <th>T5</th>\
						    <th>T6</th>\
						    <th>T7</th>\
						    <th>C<span style="font-size: 0.8em">N</span></th>\
					    </tr>\
				    </thead>\
				    <tbody>\
                        <tr><td><i></i></td><td><i></i></td><td><i></i></td><td><i></i></td><td><i></i></td><td><i></i></td><td><i></i></td></tr>\
                        <tr><td><i></i></td><td><i></i></td><td><i></i></td><td><i></i></td><td><i></i></td><td><i></i></td><td><i></i></td></tr>\
                        <tr><td><i></i></td><td><i></i></td><td><i></i></td><td><i></i></td><td><i></i></td><td><i></i></td><td><i></i></td></tr>\
                        <tr><td><i></i></td><td><i></i></td><td><i></i></td><td><i></i></td><td><i></i></td><td><i></i></td><td><i></i></td></tr>\
                        <tr><td><i></i></td><td><i></i></td><td><i></i></td><td><i></i></td><td><i></i></td><td><i></i></td><td><i></i></td></tr>\
				    </tbody>\
			    </table>\
		    </section>\
	    </article>'
    );

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
    $lich.find('.lich i').on('click', function () {
        var
            ngay = $(this).attr('data-value'),
            thang = $lich.attr('data-thang-ht'),
            nam = $lich.attr('data-nam-ht');

        $inputVuaFocus.val(ngay + '.' + thang + '.' + nam);
        $lich.attr('data-ngay', ngay);
        $lich.attr('data-thang', thang);
        $lich.attr('data-nam', nam);

        $lich.find('i.chon').removeClass('chon');
        $lich.find('i[data-value="' + ngay + '"]').addClass('chon');
    });

    return $lich;
}

function layGiaTriMacDinh_Lich($lich, $lichInput) {
    var date = new Date();
    var
        ngay = date.getDate(),
        thang = date.getMonth() + 1,
        nam = date.getFullYear();

    $lich.attr('data-ngay', ngay);
    $lich.attr('data-thang', thang);
    $lich.attr('data-nam', nam);
    $lich.attr('data-thang-ht', thang);
    $lich.attr('data-nam-ht', nam);

    capNhatLich($lich);

    $inputVuaFocus.val(ngay + '.' + thang + '.' + nam);
}

function layGiaTri_Lich($lich, $lichInput) {
    var lich = $lichInput.val().split('.');
        
    $lich.attr('data-ngay', lich[0]);
    $lich.attr('data-thang', lich[1]);
    $lich.attr('data-nam', lich[2]);
    $lich.attr('data-thang-ht', lich[1]);
    $lich.attr('data-nam-ht', lich[2]);

    capNhatLich($lich);
}

function capNhatLich($lich) {
    var
        ngayChon = $lich.attr('data-ngay'),
        thangChon = $lich.attr('data-thang'),
        namChon = $lich.attr('data-nam'),
        thang = $lich.attr('data-thang-ht'),
        nam = $lich.attr('data-nam-ht');
    $lich.find('.thang-nam .thang').attr('data-value', thang);
    $lich.find('.thang-nam .nam').attr('data-value', nam);
    // Chọn ngày 1 -> thứ bắt đầu 
    // & xử lý để thứ bắt đầu là 2 (0), kết thúc là chủ nhật (6)
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
    if (thang == thangChon && nam == namChon) {
        $lich.find('i[data-value="' + ngayChon + '"]').addClass('chon');
    }
}