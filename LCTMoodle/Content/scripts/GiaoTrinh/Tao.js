var $_Khung, $_DanhSach, maKhoaHoc;

$(function () {
    $_Khung = $('#khung_giao_trinh');
    $_DanhSach = $_Khung.find('.tbody');

    khoiTaoForm($_Khung.find('#tao_giao_trinh_form'));
    khoiTaoItem($_DanhSach.children());
})

function khoiTaoForm($form) {
    khoiTaoLCTForm($form, {
        submit: function () {
            $.ajax({
                url: '/GiaoTrinh/XuLyThem',
                type: 'POST',
                data: $form.serialize(),
                dataType: 'JSON',
                async: false
            }).done(function (data) {
                if (data.trangThai == 0) {
                    var $item = $(data.ketQua);
                    khoiTaoItem($item);

                    $_Khung.find('.tbody').append($item);

                    khoiTaoLCTFormMacDinh($form);
                }
                else {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: 'Thêm thất bại',
                        bieuTuong: 'nguy-hiem'
                    })
                }
            }).fail(function () {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Thêm thất bại',
                    bieuTuong: 'nguy-hiem'
                })
            });
        }
    });
}

function khoiTaoItem($item) {
    //Nút tắt mở
    khoiTaoTatMoDoiTuong($item.find('[data-chuc-nang="tat-mo"]'));

    //Nút xóa
    $item.find('[data-chuc-nang="xoa"]').on('click', function () {
        var $item = $(this).closest('[data-doi-tuong="item-giao-trinh"]');
        moPopup({
            tieuDe: 'Xác nhận',
            thongBao: 'Bạn có chắc muốn xóa công việc này?',
            bieuTuong: 'hoi',
            nut: [
                {
                    ten: 'Có',
                    loai: 'can-than',
                    xuLy: function () {
                        $.ajax({
                            url: '/GiaoTrinh/XuLyXoa/' + $item.attr('data-ma'),
                            type: 'POST',
                            dataType: 'JSON'
                        }).done(function () {
                            $item.remove();
                        }).fail(function () {
                            moPopup({
                                tieuDe: 'Thông báo',
                                thongBao: 'Xóa công việc thất bại',
                                bieuTuong: 'nguy-hiem'
                            })
                        })
                    }
                },
                {
                    ten: 'Không'
                }
            ]
        })
    });

    //Kéo thay đổi thứ tự
    $item.each(function () {
        var $itemKeo = $(this);

        $itemKeo.prop('draggable', true).on({
            'dragstart': function (e) {
                e = e.originalEvent;
                e.dataTransfer.setDragImage($('<span></span>')[0], 0, 0);
                $_Khung.add($itemKeo).addClass('keo');
                mangTam.keo = false;
                mangTam.viTriBatDau = $itemKeo.index();

                $itemKeo.siblings('[data-doi-tuong="item-giao-trinh"]').on({
                    'dragover.keo-thu-tu': function (e) {
                        e = e.originalEvent;
                        e.preventDefault();
                        var $item = $(this);

                        var y = $item.offset().top;

                        if (e.offsetY < this.offsetHeight / 2) {
                            $item.before($itemKeo);
                        }
                        else {
                            $item.after($itemKeo);
                        }
                    },
                    'drop.keo-thu-tu': function () {
                        mangTam.keo = true;
                    }
                });

                $(this).on({
                    'dragover.keo-thu-tu': function (e) {
                        e.preventDefault();
                    },
                    'drop.keo-thu-tu': function () {
                        mangTam.keo = true;
                    },
                    'dragend.keo-thu-tu': function () {
                        $_Khung.add($itemKeo).removeClass('keo');
                        $_Khung.find('[data-doi-tuong="item-giao-trinh"]').off('.keo-thu-tu');

                        if (mangTam.keo === true) {
                            $.ajax({
                                url: '/GiaoTrinh/XuLyCapNhatThuTu/',
                                type: 'POST',
                                data: { thuTuCu: mangTam.viTriBatDau + 1, thuTuMoi: $itemKeo.index() + 1, maKhoaHoc: maKhoaHoc },
                                dataType: 'JSON'
                            }).done(function (data) {
                                if (data.trangThai != 0) {
                                    moPopup({
                                        tieuDe: 'Thông báo',
                                        thongBao: 'Thay đổi thứ tự thất bại',
                                        bieuTuong: 'loi'
                                    });
                                    var viTriHienTai = $itemKeo.index();
                                    var viTriBatDau = mangTam.viTriBatDau;

                                    if (viTriHienTai < viTriBatDau) {
                                        $_DanhSach.children(':nth-child(' + (viTriBatDau + 1) + ')').after($itemKeo);
                                    }
                                    else if (viTriHienTai > viTriBatDau) {
                                        $_DanhSach.children(':nth-child(' + (viTriBatDau + 1) + ')').before($itemKeo);
                                    }
                                }
                            }).fail(function () {
                                moPopup({
                                    tieuDe: 'Thông báo',
                                    thongBao: 'Thay đổi thứ tự thất bại',
                                    bieuTuong: 'nguy-hiem'
                                });
                                var viTriHienTai = $itemKeo.index();
                                var viTriBatDau = mangTam.viTriBatDau;

                                if (viTriHienTai < viTriBatDau) {
                                    $_DanhSach.children(':nth-child(' + (viTriBatDau + 1) + ')').after($itemKeo);
                                }
                                else if (viTriHienTai > viTriBatDau) {
                                    $_DanhSach.children(':nth-child(' + (viTriBatDau + 1) + ')').before($itemKeo);
                                }
                            });
                        }
                        else {
                            var viTriHienTai = $itemKeo.index();
                            var viTriBatDau = mangTam.viTriBatDau;

                            if (viTriHienTai < viTriBatDau) {
                                $_DanhSach.children(':nth-child(' + (viTriBatDau + 1) + ')').after($itemKeo);
                            }
                            else if (viTriHienTai > viTriBatDau) {
                                $_DanhSach.children(':nth-child(' + (viTriBatDau + 1) + ')').before($itemKeo);
                            }
                        }
                    }
                });
            }
        })
    })
}