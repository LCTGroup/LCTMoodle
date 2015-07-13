var $_Khung, $_DanhSach, maKhoaHoc;

$(function () {
    $_Khung = $('#khung_giao_trinh');
    $_DanhSach = $_Khung.find('.tbody');

    khoiTaoForm($_Khung.find('#tao_giao_trinh_form'));
    khoiTaoItem($_DanhSach.children());
    capNhatThuTu();
})

function khoiTaoForm($form) {
    khoiTaoLCTForm($form, {
        submit: function () {
            var $tai = moBieuTuongTai($_Khung);
            $.ajax({
                url: '/ChuongTrinh/XuLyThem',
                type: 'POST',
                data: $form.serialize(),
                dataType: 'JSON'
            }).always(function () {
                $tai.tat();
            }).done(function (data) {
                if (data.trangThai == 0) {
                    var $item = $(data.ketQua);
                    khoiTaoItem($item);

                    $_Khung.find('.tbody').append($item);
                    $item.find('.td:eq(0)').text($item.index() + 1);

                    khoiTaoLCTFormMacDinh($form);
                }
                else {
                    moPopupThongBao(data);
                }
            }).fail(function () {
                moPopupThongBao('Thêm mục chương trình thất bại');
            });
        }
    });
}

function khoiTaoItem($item) {
    //Nút tắt mở
    khoiTaoTatMoDoiTuong($item.find('[data-chuc-nang="tat-mo"]'), true);

    //Nút xóa
    $item.find('[data-chuc-nang="xoa"]').on('click', function () {
        var $item = $(this).closest('[data-doi-tuong="item-chuong-trinh"]');
        moPopup({
            tieuDe: 'Xác nhận',
            thongBao: 'Bạn có chắc muốn xóa công việc này?',
            bieuTuong: 'hoi',
            nut: [
                {
                    ten: 'Có',
                    loai: 'can-than',
                    xuLy: function () {
                        var $tai = moBieuTuongTai($_Khung);
                        $.ajax({
                            url: '/ChuongTrinh/XuLyXoa/' + $item.attr('data-ma'),
                            type: 'POST',
                            dataType: 'JSON'
                        }).always(function () {
                            $tai.tat();
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                $item.remove();
                                capNhatThuTu();
                            }
                            else {
                                moPopupThongBao(data);
                            }
                        }).fail(function () {
                            moPopupThongBao('Xóa công việc thất bại');
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

                $itemKeo.siblings('[data-doi-tuong="item-chuong-trinh"]').on({
                    'dragover.keo-thu-tu': function (e) {
                        e = e.originalEvent;
                        e.preventDefault();
                        var $item = $(this);

                        var y = $item.offset().top;

                        if (e.offsetY < this.offsetHeight / 2) {
                            $item.before($itemKeo);
                            capNhatThuTu();
                        }
                        else {
                            $item.after($itemKeo);
                            capNhatThuTu();
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
                        $_Khung.find('[data-doi-tuong="item-chuong-trinh"]').off('.keo-thu-tu');

                        if (mangTam.keo === true) {
                            var $tai = moBieuTuongTai($_Khung);
                            $.ajax({
                                url: '/ChuongTrinh/XuLyCapNhatThuTu/',
                                type: 'POST',
                                data: { thuTuCu: mangTam.viTriBatDau + 1, thuTuMoi: $itemKeo.index() + 1, maKhoaHoc: maKhoaHoc },
                                dataType: 'JSON'
                            }).always(function () {
                                $tai.tat();
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
                                        capNhatThuTu();
                                    }
                                    else if (viTriHienTai > viTriBatDau) {
                                        $_DanhSach.children(':nth-child(' + (viTriBatDau + 1) + ')').before($itemKeo);
                                        capNhatThuTu();
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
                                capNhatThuTu();
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

//#region Hỗ trợ

function capNhatThuTu() {
    $_DanhSach.find('.tr').each(function (index) {
        $(this).find('.td:eq(0)').text(index + 1);
    });
}

//#endregion