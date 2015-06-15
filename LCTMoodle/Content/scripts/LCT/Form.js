// Lưu đối tượng vừa focus
$inputVuaFocus = {};

/*
    Khởi tạo input
*/
function khoiTaoLCTForm($form, thamSo) {
    thamSo = thamSo || {};

    //Khởi tạo
    if ('khoiTao' in thamSo) {
        thamSo.khoiTao();
    }

    // Thêm nút mặc định
    if ($form.is('[data-nut-mac-dinh]')) {
        khoiTaoNutMacDinh_LCT($form);
    }

    //Xử lý hiển thị đúng form
    khoiTaoHienThiInput_LCT($form);

    //Xử lý upload file = ajax
    khoiTaoTapTinInput_LCT($form);

    //Xử lý chọn chủ đề
    khoiTaoChuDeInput_LCT($form);

    //Xử lý gợi ý input
    khoiTaoGoiYInput_LCT($form);

    //Khởi tạo thời gian
    khoiTaoInputThoiGian_LCT($form.find('input[data-input-type="gio"]'), 'thoi-gian', 'dong_ho_form', khoiTaoForm_DongHo, layGiaTriMacDinh_DongHo, layGiaTri_DongHo);
    khoiTaoInputThoiGian_LCT($form.find('input[data-input-type="ngay"]'), 'lich', 'lich_form', khoiTaoForm_Lich, layGiaTriMacDinh_Lich, layGiaTri_Lich);

    //Xử lý lấy giá trị mặc định
    khoiTaoLCTFormMacDinh($form);

    //Khởi tạo tắt, mở đối tượng
    khoiTaoTatMo_LCT($form);

    //Khởi tạo bắt lỗi
    khoiTaoBatLoi_LCT($form, thamSo);

    //Xử lý ajax submit chung
    khoiTaoSubmit_LCT($form, thamSo);
}

function khoiTaoNutMacDinh_LCT($form) {
    $form.find('.input').append($('<i class="mac-dinh" title="Mặc định"></i>').on('click', function () {
        /*
            Khởi tạo sự kiện cho nó làm mới
        */
        //Thẻ chứa
        $chua = $(this).parent();

        // Text, thời gian
        $chua.find('input[type="text"], textarea, input[data-input-type="gio"], input[data-input-type="ngay"]').each(function () {
            this.value = this.getAttribute('data-mac-dinh');
        });
        $chua.find('textarea[data-input-type="editor"]').each(function () {
            CKEDITOR.instances[this.getAttribute('name')].setData(this.getAttribute('data-mac-dinh'));
        });
        $chua.find('select').each(function () {
            var $select = $(this);
            var $macDinh = $select.children('[data-mac-dinh]');

            if ($macDinh.length == 0) {
                $select.children()[0].selected = true;
            }
            else {
                $macDinh.prop('selected', true);
            }
        });

        // Checkbox, radio button
        $chua.find('input[type="checkbox"], input[type="radio"]').each(function () {
            this.checked = this.getAttribute('data-mac-dinh') != null;
        });

        // Tập tin
        $chua.find('input[type="file"]').each(function () {
            $phanTu = $(this);

            var maMacDinh = $phanTu.attr('data-ma-mac-dinh');
            var loaiMacDinh = $phanTu.attr('data-loai-mac-dinh');
            if (maMacDinh) {
                $phanTu.addClass('co');
                $phanTu.find('~ img').attr('src', '/LayTapTin/' + loaiMacDinh + '/' + maMacDinh);
            }
            else {
                $phanTu.removeClass('co');
                $phanTu.find('~ img').removeAttr('src');
            }
            $phanTu.find('~ input[type="hidden"]').val(maMacDinh);
        });

        // Chủ đề
        $chua.find('input[data-input-type="chu-de"], input[data-input-type="goi-y"]').each(function () {
            var $phanTu = $(this);

            $phanTu.val($phanTu.attr('data-ten-mac-dinh'));
            $phanTu.next().val($phanTu.attr('data-ma-mac-dinh'));
        })

        $chua.find('[data-chuc-nang="tat-mo"]').each(function () {
            xuLyTatMo($form, $(this));
        });

        if ($chua.is('[data-cap-nhat]')) {
            $chua.find('.input :input').attr('data-cu', '');
        }
    }));
}

function khoiTaoHienThiInput_LCT($form) {
    $form.find('input[type="checkbox"], input[type="radio"]').each(function () {
        $element = $(this);
        $element.wrap('<label class="checkbox-radio-label"></label>');
        $element.after('<u></u>' + $element.attr('data-text'));
    });

    $form.find('input[type="file"]').each(function () {
        var $phanTu = $(this);
        $phanTu.wrap('<label class="file-label"></label>');
        var name = $phanTu.attr('name');
        var maMacDinh = $phanTu.attr('data-ma-mac-dinh');
        var loaiMacDinh = $phanTu.attr('data-loai-mac-dinh');
        $phanTu.removeAttr('name');

        $phanTu.after('<input type="hidden" name="' + name + '" value="' + maMacDinh + '"><img src="' + (maMacDinh ? '/LayTapTin/' + loaiMacDinh + '/' + maMacDinh : '') + '" /><i></i><u></u>');
        if (maMacDinh) {
            $phanTu.addClass('co');
        }
        else {
            $phanTu.removeClass('co');
        }
    });

    $form.find('input[data-input-type="ngay-gio"]').each(function () {
        var $phanTu = $(this);

        var thoiGianMacDinh = $phanTu.attr('data-thoi-gian-mac-dinh') || '',
            lichMacDinh = $phanTu.attr('data-lich-mac-dinh') || '',
            thoiGianPlaceholder = $phanTu.attr('data-thoi-gian-placeholder') || '',
            lichPlaceholder = $phanTu.attr('data-lich-placeholder') || '',
            validate = $phanTu.attr('data-validate');

        $phanTu.removeAttr('data-validate data-thoi-gian-mac-dinh data-lich-mac-dinh data-thoi-gian-placeholder data-lich-placeholder');

        $phanTu.after($phanTu.clone().attr({
            'name': '',
            'data-input-type': 'ngay',
            'data-mac-dinh': lichMacDinh,
            'placeholder': lichPlaceholder,
            'data-validate': validate
        }).css('width', 'calc(50% - 19px)')).after($phanTu.clone().attr({
            'name': '',
            'data-input-type': 'gio',
            'data-mac-dinh': thoiGianMacDinh,
            'placeholder': thoiGianPlaceholder,
            'data-validate': validate
        }).css('width', 'calc(50% - 19px)'));

        $phanTu.attr('type', 'hidden');
    })

    $form.find('input[data-input-type="chu-de"]').each(function () {
        $phanTu = $(this);

        $phanTu.after($('<input type="hidden" />').attr({
            'name': $phanTu.attr('name')
        }));

        $phanTu.removeAttr('name');
    })

    $form.find('input[data-input-type="goi-y"]').each(function () {
        var $phanTu = $(this);
        $phanTu.wrap('<article class="khung-input-goi-y"></article>');
        var name = $phanTu.attr('name');
        $phanTu.removeAttr('name');
        $phanTu.after('<input type="hidden" name="' + name + '"><section class="khung-danh-sach-goi-y"><ul class="danh-sach-goi-y"></ul><span>Không có giá trị trùng khớp từ khóa</span></section>');
    })

    //Trường hợp đặc biệt, xử lý validate riêng cho editor
    $form.find('textarea[data-input-type="editor"]').each(function () {
        CKEDITOR.replace(this);
        var $phanTu = $(this);

        CKEDITOR.on('instanceReady', function (e) {
            e.removeListener();

            var $htmlTag = $phanTu.find('~ div iframe').contents().find('html');

            $htmlTag.find('body').on({
                focus: function () {
                    $htmlTag.addClass('focus');
                },
                focusout: function () {
                    $htmlTag.removeClass('focus');
                }
            });
        });
    });

    if ($form.is('[data-cap-nhat]')) {
        $form.find('.input :input').on('change', function () {
            $(this).removeAttr('data-cu');
        })
    }
}

function khoiTaoTapTinInput_LCT($form) {
    $form.find('input[type="file"]').on('change', function () {
        //Kiểm tra tồn tại
        if (this.files.length == 0) {
            return;
        }

        $phanTu = $(this);

        //Lấy, xử lý thanh thể hiện xử lý
        var thanhTheHien = $phanTu.find('~ u')[0];
        thanhTheHien.style.height = '0%';
        $phanTu.addClass('dang');

        //Lấy data
        var data = new FormData();
        data.append('TapTin', this.files[0]);

        //post request
        $.ajax({
            url: '/TapTin/XuLyThem',
            type: 'POST',
            processData: false,
            contentType: false,
            data: data,
            dataType: 'JSON',
            xhr: function () {
                var xhr = $.ajaxSettings.xhr();
                if (xhr.upload) { //Kiểm tra nếu thuộc tính tồn tại
                    xhr.upload.addEventListener('progress', function (e) {
                        if (e.lengthComputable) {
                            thanhTheHien.style.height = Math.ceil(e.loaded / e.total) * 100 + '%';
                        }
                    }, false);
                }
                return xhr;
            }
        }).done(function (data) {
            if (data.trangThai == 0) {
                $phanTu.addClass('co');
                $phanTu.find('~ img').attr('src', '/LayTapTin/Tam/' + data.ketQua.ma);
                $phanTu.find('~ input[type="hidden"]').val(data.ketQua.ma).change();
            }
            else {
                alert('Thêm file thất bại');
            }
        }).fail(function () {
            alert('Thêm file thất bại')
        }).always(function () {
            thanhTheHien.style.height = '';
            $phanTu.removeClass('dang');
        });
    });
}

function khoiTaoChuDeInput_LCT($form) {
    $form.find('input[data-input-type="chu-de"]').on('focus', function (e, mo) {
        var $phanTu = $(this);

        moPopupFull({
            url: '/ChuDe/_Chon',
            thanhCong: function ($popup) {
                var $khung = $popup.find('#khung_quan_ly');

                khoiTaoKhungChuDe($khung);

                $khung.on('chon', function (e, data) {
                    $popup.tat();
                    $phanTu.val(data.ten).focusout();
                    $phanTu.next().val(data.ma).change();
                });
            }
        });
    });
}

function khoiTaoGoiYInput_LCT($form) {
    var maTam = 'goi-y_';

    $form.find('input[data-input-type="goi-y"]').each(function () {
        var $inputGoiY = $(this);
        var $khungDanhSachGoiY = $inputGoiY.find('~ .khung-danh-sach-goi-y');
        var $danhSachGoiY = $khungDanhSachGoiY.children('ul');

        $inputGoiY.on({
            focusout: function () {
                if (mangTam[maTam + 'td'] === false) {
                    return;
                }

                var $giaTriChon = $danhSachGoiY.children('.chon');

                if ($giaTriChon.length != 0) {
                    layGoiY_LCT($danhSachGoiY.children('.chon'));
                }
                else {
                    $inputGoiY.next().val('');
                    $danhSachGoiY.removeClass('rong').html('');
                }
            },
            keyup: function () {
                if (mangTam[maTam + 'gt'] == $inputGoiY.val()) {
                    return;
                }

                mangTam[maTam + 'td'] = true;
                clearTimeout(mangTam[maTam + 'to']);
                mangTam[maTam + 'to'] = setTimeout(function () {
                    mangTam[maTam + 'gt'] = $inputGoiY.val();
                    var data = $.parseJSON($inputGoiY.attr('data-data') || '{}');
                    data.input = $inputGoiY.val();

                    $.ajax({
                        url: $inputGoiY.attr('data-url'),
                        data: data,
                        dataType: 'JSON'
                    }).done(function (data) {
                        if (data.trangThai != 0 || data.ketQua.length == 0) {
                            $danhSachGoiY.addClass('rong').html('');
                        }
                        else {
                            var html = taoGoiY_LCT(data.ketQua);
                            $danhSachGoiY.removeClass('rong').html(html);
                            $danhSachGoiY.find(':first-child').addClass('chon');
                        }
                    }).fail(function () {
                        $danhSachGoiY.addClass('rong').html('');
                    });
                }, 200)
            },
            keydown: function (e) {
                switch (e.which) {
                    case 38:
                        e.preventDefault();

                        var $goiYChon = $danhSachGoiY.children('.chon');
                        if ($goiYChon.length == 0) {
                            chonGoiY_LCT($danhSachGoiY.children(':last-child'));
                        }
                        else {
                            var $goiYTren = $goiYChon.prev();
                            if ($goiYTren.length == 0) {
                                chonGoiY_LCT($danhSachGoiY.children(':last-child'));
                            }
                            else {
                                chonGoiY_LCT($goiYTren);
                            }
                        }
                        break;
                    case 40:
                        e.preventDefault();

                        var $goiYChon = $danhSachGoiY.children('.chon');
                        if ($goiYChon.length == 0) {
                            chonGoiY_LCT($danhSachGoiY.children(':first-child'));
                        }
                        else {
                            var $goiYDuoi = $goiYChon.next();
                            if ($goiYDuoi.length == 0) {
                                chonGoiY_LCT($danhSachGoiY.children(':first-child'));
                            }
                            else {
                                chonGoiY_LCT($goiYDuoi);
                            }
                        }
                        break;
                    case 13:
                        e.preventDefault();
                        layGoiY_LCT($danhSachGoiY.children('.chon'));
                    case 27:
                        e.preventDefault();
                    default:
                        break;
                }
            }
        });


        //Tạm
        khoiTaoDanhSachGoiY_LCT($inputGoiY.find('~ .khung-danh-sach-goi-y .danh-sach-goi-y'));
    })

    function khoiTaoDanhSachGoiY_LCT($danhSach) {
        var $items = $danhSach.find('li');
        $items.on({
            mouseenter: function () {
                chonGoiY_LCT($(this));
            },
            click: function () {
                layGoiY_LCT($(this));
            }
        })
    }

    function chonGoiY_LCT($item) {
        $item.siblings('.chon').removeClass('chon');
        $item.addClass('chon');
    }

    function layGoiY_LCT($item) {
        var $chua = $item.closest('.khung-input-goi-y'),
            $giaTriChon = $item.children(),
            $input = $chua.children(':first-child'),
            ten = $giaTriChon.text() || '',
            ma = $giaTriChon.attr('data-value') || '';
        $input.val(ten);
        $input.next().val(ma).change();

        $chua.find('.danh-sach-goi-y').removeClass('rong').html('');

        mangTam[maTam + 'td'] = false;
        mangTam[maTam + 'gt'] = $input.val();
        clearTimeout(mangTam[maTam + 'to']);
    }

    function taoGoiY_LCT(data) {
        var html = '', soLuong = data.length;

        for (var i = 0; i < soLuong; i++) {
            html += '<li><span data-value="' + data[i].ma + '">' + data[i].ten + '</span></li>';
        }

        return html;
    }
}

// loai Loại input (thoi-gian, lich)
// id của form nhập (đồng hồ hoặc lịch)
// hàm khởi tạo form lúc chưa có
// hàm xử lý giá trị mặc định
// hàm xử lý lấy giá trị khi đã có
function khoiTaoInputThoiGian_LCT($inputs, loai, idInput, hamKhoiTao, hamXuLyMacDinh, hamXuLyLayGiaTri) {
    //Lấy form nhập
    $formInput = $('#' + idInput);

    //Xử lý focus => hiển thị
    $inputs.on('focus', function (e) {
        e = e || window.event();
        $input = $(e.target);

        //Lấy form
        $formInput = $('#' + idInput);
        if ($formInput.length == 0) {
            $formInput = hamKhoiTao(idInput);
            $('body').prepend($formInput)
        }

        //Nếu input vừa được focus & focus tiếp => ko xử lý
        if (e.target === $inputVuaFocus[0] && $formInput.attr('data-trang-thai') == 'hien') {
            return;
        }

        $inputVuaFocus = $input;

        //Lắng nghe sự kiện để ẩn
        //Nếu form ở trạng thái hiện => đang lắng nghe => ko cần
        if ($formInput.attr('data-trang-thai') == 'an') {
            $('body').on('mousedown.' + loai, function (e) {
                e = e || window.event;
                //Nếu nhấn vào input đang xử lý thì ko làm gì cả
                if (e.target != $input[0]) {
                    //Nếu không phải vừa nhấn vào đối tượng trên form => tắt
                    if ($formInput.has(e.target).length == 0) {
                        //Nếu đối tượng vừa được nhấn vào có cùng loại với input thì không làm gì cả
                        if ($(e.target).attr('data-input-type') != loai) {
                            $formInput.attr('data-trang-thai', 'an');
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
                    if ($formInput.has(e.target).length != 0) {
                        $input.focus();
                    }
                }
            });
        }

        //Khởi tạo giá trị cho form
        if ($input.val() == '') {
            hamXuLyMacDinh($formInput, $input);
        }
        else {
            hamXuLyLayGiaTri($formInput, $input);
        }

        //Đưa form vào vị trí hiển thị
        var viTri = $input.offset();

        //Nếu đang ẩn => hiện lên
        if ($formInput.attr('data-trang-thai') == 'an') {
            $formInput.attr('data-trang-thai', 'hien');
            $formInput.css({
                left: viTri.left + 15,
                top: viTri.top - $formInput.outerHeight() - 5
            });
        }
        else {
            $formInput.animate({
                left: viTri.left + 15,
                top: viTri.top - $formInput.outerHeight() - 5
            }, 500);
        }
    })

    //Xử lý khi nhấn tab
    $inputs.on('keydown', function (e) {
        e = e || window.event;

        if (e.keyCode == 9) {
            $formInput.attr('data-trang-thai', 'an');
            $('body').off('mouseup.' + loai);
            $('body').off('mousedown.' + loai);
        }
    });
}

function khoiTaoLCTFormMacDinh($form) {
    $form.find('input[type="text"], textarea, input[data-input-type="gio"], input[data-input-type="ngay"]').each(function () {
        this.value = this.getAttribute('data-mac-dinh');
    });
    $form.find('textarea[data-input-type="editor"]').each(function () {
        CKEDITOR.instances[this.getAttribute('name')].setData(this.getAttribute('data-mac-dinh'));
    });
    $form.find('select').each(function () {
        var $select = $(this);
        var $macDinh = $select.children('[data-mac-dinh]');

        if ($macDinh.length == 0) {
            $select.children()[0].selected = true;
        }
        else {
            $macDinh.prop('selected', true);
        }
    });
    $form.find('input[type="checkbox"], input[type="radio"]').each(function () {
        this.checked = this.getAttribute('data-mac-dinh') != null;
    });
    $form.find('input[type="file"]').each(function () {
        $phanTu = $(this);

        var maMacDinh = $phanTu.attr('data-ma-mac-dinh');
        var loaiMacDinh = $phanTu.attr('data-loai-mac-dinh');
        if (maMacDinh) {
            $phanTu.addClass('co');
            $phanTu.find('~ img').attr('src', '/LayTapTin/' + loaiMacDinh + '/' + maMacDinh);
        }
        else {
            $phanTu.removeClass('co');
            $phanTu.find('~ img').removeAttr('src');
        }
        $phanTu.find('~ input[type="hidden"]').val(maMacDinh);
    });
    $form.find('input[data-input-type="chu-de"], input[data-input-type="goi-y"]').each(function () {
        var $phanTu = $(this);

        $phanTu.val($phanTu.attr('data-ten-mac-dinh'));
        $phanTu.next().val($phanTu.attr('data-ma-mac-dinh'));
    });
    $form.find('[data-chuc-nang="tat-mo"]').each(function () {
        xuLyTatMo($form, $(this), true);
    });
    
    if ($form.is('[data-cap-nhat]')) {
        $form.find('.input :input').attr('data-cu', '');
    }
}

function xuLyTatMoDoiTuong($doiTuong, tat, dangKhoiTao) {
    if (!$doiTuong.is('textarea, input, select')) {
        $doiTuong.find('input, textarea, select').prop('disabled', tat);
        if (tat) {
            $doiTuong.addClass('tat');
        }
        else {
            $doiTuong.removeClass('tat');
        }
    }
    else if ($doiTuong.is('textarea[data-input-type="editor"]')) {
        $doiTuong.prop('disabled', tat);
        if (dangKhoiTao === true) {
            CKEDITOR.on('instanceReady', function (e) {
                e.removeListener();

                if (tat) {
                    $doiTuong.next().addClass('tat').find('iframe').attr('tabindex', '-1');
                }
                else {
                    $doiTuong.next().removeClass('tat').find('iframe').attr('tabindex', '0');
                }
            });
        }
        if (tat) {
            $doiTuong.next().addClass('tat').find('iframe').attr('tabindex', '-1');
        }
        else {
            $doiTuong.next().removeClass('tat').find('iframe').attr('tabindex', '0');
        }
    }
    else if ($doiTuong.is('input[type="checkbox"], input[type="radio"], input[type="file"]')) {
        $doiTuong.prop('disabled', tat);
        if (tat) {
            $doiTuong.parent().addClass('tat');
        }
        else {
            $doiTuong.parent().removeClass('tat');
        }
    }
    else {
        $doiTuong.prop('disabled', tat);
        if (tat) {
            $doiTuong.addClass('tat');
        }
        else {
            $doiTuong.removeClass('tat');
        }
    }
}

//tatHet: undefined, true, false
function xuLyTatMo($form, $input, dangKhoiTao, tatHet) {
    var doiTuongTat = '',
        doiTuongMo = '';

    if ($input.is('input[type="checkbox"], input[type="radio"]')) {
        if ($input[0].checked) {
            doiTuongMo = $input.attr('data-mo');
            doiTuongTat = $input.attr('data-tat');
        }
        else {
            doiTuongMo = $input.attr('data-tat');
            doiTuongTat = $input.attr('data-mo');
        }
    }
    else if ($input.is('select')) {
        var $option = $input.children(':selected');

        doiTuongMo = $option.attr('data-mo');
        doiTuongTat = $option.attr('data-tat');
    }

    $form.find('[data-doi-tuong~="' + doiTuongMo + '"]').each(function () {
        var $doiTuong = $(this);

        xuLyTatMoDoiTuong($doiTuong, tatHet || false, dangKhoiTao)

        if ($doiTuong.is('[data-chuc-nang="tat-mo"]')) {
            xuLyTatMo($form, $doiTuong, dangKhoiTao);
        }
        $doiTuong.find('[data-chuc-nang="tat-mo"]').each(function () {
            xuLyTatMo($form, $(this), dangKhoiTao);
        })
    });

    $form.find('[data-doi-tuong~="' + doiTuongTat + '"]').each(function () {
        var $doiTuong = $(this);

        xuLyTatMoDoiTuong($doiTuong, !tatHet || true, dangKhoiTao);

        if ($doiTuong.is('[data-chuc-nang="tat-mo"]')) {
            var khiAn = $doiTuong.attr('data-khi-tat');

            xuLyTatMo($form, $doiTuong, dangKhoiTao, khiAn != 'mo');
        }
        $doiTuong.find('[data-chuc-nang="tat-mo"]').each(function () {
            var $input = $(this);
            var khiAn = $input.attr('data-khi-tat');

            xuLyTatMo($form, $input, dangKhoiTao, khiAn != 'mo');
        })
    });
}

function khoiTaoTatMo_LCT($form) {
    $form.find('[data-chuc-nang="tat-mo"]').on('change', function () {
        xuLyTatMo($form, $(this));
    });
}

function baoLoi($input, loai, noiDung) {
    var $khungInput = $input.closest('.input');
    var $khungLoi = $khungInput.find('~ .loi');

    if ($khungLoi.length == 0) {
        $khungLoi = $('<section class="loi"></section>');
        $khungInput.after($khungLoi);
    }

    if ($khungLoi.children('[data-type="' + loai + '"]').length == 0) {
        if (typeof noiDung === 'undefined') {
            switch (loai) {
                case 'bat-buoc':
                    noiDung = 'Nội dung bắt buộc';
                    break;
                case 'so-nguyen':
                    noiDung = 'Nội dung chỉ cho phép số nguyên';
                    break;
                case 'so-thuc':
                    noiDung = 'Nội dung chỉ cho phép số thực';
                    break;
                case 'chu':
                    noiDung = 'Nội dung chỉ cho phép chữ';
                    break;
                case 'email':
                    noiDung = 'Email không hợp lệ';
                    break;
                default:
                    noiDung = 'Nội dung không hợp lệ';
                    break;
            }
        }

        $khungLoi.append('<p data-type="' + loai + '">' + noiDung + '</p>');
    }

    $input.addClass('loi-' + loai);
}

function tatLoi($input, loai) {
    var $khungLoi = $input.closest('.input').siblings('.loi');
    $khungLoi.children('[data-type="' + loai + '"]').remove();

    if ($khungLoi.children().length == 0) {
        $khungLoi.remove();
    }

    $input.removeClass('loi-' + loai);
}

function khoiTaoBatLoi_LCT($form, thamSo) {
    var auto = $form.is('[data-validate-auto]');

    //Bắt buộc
    $form.find('[data-validate~="bat-buoc"]').each(function () {
        var $input = $(this);

        if ($input.is('[data-input-type="editor"]')) {
            var name = $input.attr('name');

            CKEDITOR.on('instanceReady', function (e) {
                e.removeListener();

                var $htmlTag = $input.find('~ div iframe').contents().find('html');

                CKEDITOR.instances[name].on('blur', function () {
                    if (this.getData()) {
                        tatLoi($input, 'bat-buoc');
                    }
                    else {
                        if (auto) {
                            baoLoi($input, 'bat-buoc');
                        }
                    }
                });
            });
        }
        else if ($input.is('[type="file"]')) {
            $input.on('focusout', function () {
                if ($input.next().val()) {
                    tatLoi($input, 'bat-buoc');
                }
                else {
                    if (auto) {
                        baoLoi($input, 'bat-buoc');
                    }
                }
            });
        }
        else if ($input.is('[data-input-type="chu-de"], [data-input-type="goi-y"]')) {
            $input.on('focusout', function () {
                if ($input.next().val()) {
                    tatLoi($input, 'bat-buoc');
                }
                else {
                    if (auto) {
                        baoLoi($input, 'bat-buoc');
                    }
                }
            })
        }
        else {
            $input.on('focusout', function () {
                if (this.value) {
                    tatLoi($input, 'bat-buoc');
                }
                else {
                    if (auto) {                        
                        baoLoi($input, 'bat-buoc');
                    }                    
                }
            });
        }
    });

    //Chỉ số nguyên
    $form.find('[data-validate~="so-nguyen"]').on({
        'keydown': function (e) {
            e = e || window.event;
            var keyCode = e.keyCode;

            if (
                //number
                (!e.shiftKey &&
                ((48 <= keyCode && keyCode <= 57) ||
                (96 <= keyCode && keyCode <= 105))) ||

                //backspace, delete, tab, enter
                $.inArray(keyCode, [8, 46, 9, 13]) !== -1 ||

                //home, end, left, right, down, up
                (35 <= keyCode && keyCode <= 40) ||

                //ctrl A | Z | X | C | V |...
                e.ctrlKey) {
                return;
            }

            e.preventDefault();
        },
        'paste': function () {
            var $input = $(this);

            setTimeout(function () {
                $input.val($input.val().replace(/\D/g, ''));
            });
        },
        'focusout': function () {
            var $input = $(this);

            if (/\D/.test($input.val())) {
                if (auto) {
                    baoLoi($input, 'so-nguyen');
                }
            }
            else {
                tatLoi($input, 'so-nguyen');
            }
        }
    });

    //Chỉ số thực
    $form.find('[data-validate~="so-thuc"]').on({
        'keydown': function (e) {
            e = e || window.event;
            var keyCode = e.keyCode;

            if (
                //number
                (!e.shiftKey &&
                ((48 <= keyCode && keyCode <= 57) ||
                (96 <= keyCode && keyCode <= 105))) ||

                //dấu chấm thập phân
                keyCode == 190 && $input.val().indexOf('.') === -1 ||

                //backspace, delete, tab, enter
                $.inArray(keyCode, [8, 46, 9, 13]) !== -1 ||

                //home, end, left, right, down, up
                (35 <= keyCode && keyCode <= 40) ||

                //ctrl A | Z | X | C | V |...
                e.ctrlKey) {
                return;
            }

            e.preventDefault();
        },
        'paste': function () {
            var $input = $(this);

            setTimeout(function () {
                var chuoi = $input.val();

                var viTriDau = chuoi.indexOf('.');

                if (viTriDau === -1) {
                    chuoi = chuoi.replace(/\D/g, '');
                }
                else {
                    chuoi = chuoi.slice(0, viTriDau).replace(/\D/g, '') + '.' + chuoi.slice(viTriDau).replace(/\D/g, '');
                }

                $input.val(chuoi);
            });
        },
        'focusout': function () {
            var $input = $(this);

            var chuoi = $input.val();

            if (/\D&[^.]/.test(chuoi) || chuoi.indexOf('.') !== chuoi.lastIndexOf('.')) {
                if (auto) {
                    baoLoi($input, 'so-thuc');
                }
            }
            else {
                tatLoi($input, 'so-thuc');
            }
        }
    });

    //Chỉ chữ
    $form.find('[data-validate~="chu"]').on({
        'keydown': function (e) {
            e = e || window.event;
            var keyCode = e.keyCode;

            if (
                //number
                (65 < keyCode && keyCode < 90) ||

                //backspace, delete, tab, enter
                $.inArray(keyCode, [8, 46, 9, 13]) !== -1 ||

                //home, end, left, right, down, up
                (35 <= keyCode && keyCode <= 40) ||

                //ctrl A | Z | X | C | V |...
                e.ctrlKey) {
                return;
            }

            e.preventDefault();
        },
        'paste': function () {
            var $input = $(this);
            setTimeout(function () {
                $input.val($input.val().replace(/[^a-z\s]/ig, ''));
            });
        },
        'focusout': function () {
            var $input = $(this);
            if (/[^a-z\s]/i.test($input.val())) {
                if (auto) {
                    baoLoi($input, 'chu');
                }
            }
            else {
                tatLoi($input, 'chu');
            }
        }
    });

    //Chỉ email
    $form.find('[data-validate~="email"]').on({
        'focusout': function () {
            var $input = $(this);
            if ($input.val() && !/^([a-z0-9_\.\-])+\@(([a-z0-9\-])+\.)+([a-z0-9]{2,4})+$/i.test($input.val())) {
                if (auto) {
                    baoLoi($input, 'email');
                }
            }
            else {
                tatLoi($input, 'email');
            }
        }
    });

    //Regex
    $form.find('[data-regex-validate]').each(function () {
        $input.on('focusout', function () {
            var $input = $(this);
            var name = this.name;

            var duLieu = $input.attr('data-regex-validate').split('||');

            var tenLoi = duLieu[0];
            var pattern = duLieu[1];
            var flags = duLieu[2];

            var reg = new RegExp(pattern, flags);

            if (this.value && !reg.test(this.value)) {
                if (auto) {
                    baoLoi($input, 'regex-' + name, tenLoi);
                }
            }
            else {
                tatLoi($input, 'regex-' + name);
            }
        })
    });

    //Custom
        /*  */
    if ('validates' in thamSo) {
        $(thamSo.validates).each(function (index) {
            var $input = this.input;

            if ('validate' in this) {
                var validate = this;

                var loai = 'custom-' + index;
                var thongBao = validate.thongBao;

                $input.on('focusout', function () {
                    if (validate.validate() === false) {
                        if (auto) {
                            baoLoi($input, loai, thongBao);
                        }
                    }
                    else {
                        tatLoi($input, loai);
                    }
                });
            }

            if ('customEvent' in this) {
                $input.on(this.customEvent);
            }
        });
    }
}

function khoiTaoSubmit_LCT($form, thamSo) {
    $form.on('submit', function (e) {
        e.preventDefault();

        if (mangTam.dangSubmit === true) {
            moPopup({
                tieuDe: 'Thông báo',
                thongBao: 'Đang xử lý, vui lòng đợi vài giây',
                bieuTuong: 'can-than'
            })
            return;
        }
        mangTam.dangSubmit = true;

        $form.find('textarea[data-input-type="editor"]').each(function () {
            $(this).change();
            CKEDITOR.instances[this.getAttribute('name')].updateElement();
        });

        $form.find('input[data-input-type="ngay-gio"]').each(function () {
            var $phanTu = $(this),
                $thoiGian = $phanTu.next(),
                $lich = $thoiGian.next();

            if ($thoiGian.is('[data-cu]') && $lich.is('[data-cu]')) {
                return;
            }

            $phanTu.val($thoiGian.val() + ' ' + $lich.val()).change();
        });

        //Validate
        var coLoi = false;

        $form.find('[data-validate~="bat-buoc"]:not(:disabled)').each(function () {
            var $input = $(this);

            if ($input.is('[type="file"]')) {
                if (!$input.next().val()) {
                    baoLoi($input, 'bat-buoc');
                    coLoi = true;
                }
            }
            else if ($input.is('[data-input-type="chu-de"], [data-input-type="goi-y"]')) {
                if (!$input.next().val()) {
                    baoLoi($input, 'bat-buoc');
                    coLoi = true;
                }
            }
            else {
                if (!this.value) {
                    baoLoi($input, 'bat-buoc');
                    coLoi = true;
                }
            }
        });
        $form.find('[data-validate~="so-nguyen"]:not(:disabled)').each(function () {
            var $input = $(this);
            
            if (/\D/.test($input.val())) {
                baoLoi($input, 'so-nguyen');

                coLoi = true;
            }
        });
        $form.find('[data-validate~="so-thuc"]:not(:disabled)').each(function () {
            var $input = $(this);
            var chuoi = $input.val();

            if (/\D&[^.]/.test(chuoi) || chuoi.indexOf('.') !== chuoi.lastIndexOf('.')) {
                baoLoi($input, 'so-thuc');

                coLoi = true;
            }
        });
        $form.find('[data-validate~="chu"]:not(:disabled)').each(function () {
            var $input = $(this);

            if (/[^a-z\s]/i.test($input.val())) {
                baoLoi($input, 'chu');

                coLoi = true;
            }
        });
        $form.find('[data-validate~="email"]:not(:disabled)').each(function () {
            var $input = $(this);

            if ($input.val() && !/^([a-z0-9_\.\-])+\@(([a-z0-9\-])+\.)+([a-z0-9]{2,4})+$/i.test($input.val())) {
                baoLoi($input, 'email');

                coLoi = true;
            }
        });
        $form.find('[data-regex-validate]:not(:disabled)').each(function () {
            var $input = $(this);
            var name = this.name;

            var duLieu = $input.attr('data-regex-validate').split('||');

            var tenLoi = duLieu[0];
            var pattern = duLieu[1];
            var flags = duLieu[2];

            var reg = new RegExp(pattern, flags);

            if (this.value && !reg.test(this.value)) {
                baoLoi($input, 'regex-' + name, tenLoi);

                coLoi = true;
            }
        });

        //Custom
        if ('validates' in thamSo) {
            $(thamSo.validates).each(function (index) {
                if ('validate' in this) {
                    var $input = this.input;
                    var loai = 'custom-' + index;
                    var thongBao = this.thongBao;

                    if (this.validate() === false) {
                        baoLoi($input, loai, thongBao);
                        coLoi = true;
                    }
                    else {
                        tatLoi($input, loai);
                    }
                }
            });
        }

        if (!coLoi) {
            if ('submit' in thamSo) {
                $.when(thamSo.submit()).then(function () {
                    mangTam.dangSubmit = false;
                });
            }
        }
    });
}

function layDataLCTForm($form) {
    var data;

    var $inputs = $form.find(':input[name]:not(:disabled)' + ($form.is('[data-cap-nhat]') ? ':not([data-cu])' : ''));

    data = $inputs.not('[type="checkbox"]').serialize();

    $inputs.not('[type!="checkbox"]').each(function () {
        data += '&' + this.name + "=" + (this.checked ? '1' : '0');
    });

    return data;
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
        $inputVuaFocus.val(layThoiGian($dongHo)).change();
    });
    $dongHo.find('.gio i').on('click', function () {
        $dongHo.attr('data-gio', $(this).attr('data-value'));
        $inputVuaFocus.val(layThoiGian($dongHo)).change();
    });
    $dongHo.find('.phut i').on('click', function () {
        $dongHo.attr('data-phut', $(this).attr('data-value'));
        $inputVuaFocus.val(layThoiGian($dongHo)).change();
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
    $thoiGianInput.val(layThoiGian($dongHo)).change();

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
    if (gio >= 24) {
        gio = 0;
    }
    if (phut >= 60) {
        phut = 0;
    }
    return gio + ':' + (phut < 10 ? '0' + phut : phut);
}

/*
    Hàm của lịch
*/

function khoiTaoForm_Lich(id) {
    $lich = $(
        '<article id="lich_form" class="lich" data-trang-thai="an" data-ngay data-thang data-nam data-thang-ht data-nam-ht>\
		    <section class="thang-nam">\
			    <a href="javascript:void(0)" class="thang-truoc"></a>\
			    <span class="thang" data-value><i class="truoc"></i><i class="sau"></i></span>\
			    -\
			    <span class="nam" data-value><i class="truoc"></i><i class="sau"></i></span>\
			    <a href="javascript:void(0)" class="thang-sau"></a>\
		    </section>\
		    <section class="khung-thang">\
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

    $inputVuaFocus.val(ngay + '/' + thang + '/' + nam).change();
}

function layGiaTri_Lich($lich, $lichInput) {
    var lich = $lichInput.val().split('/');
        
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