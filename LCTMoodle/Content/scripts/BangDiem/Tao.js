﻿var maKhoaHoc, $_Khung, $_DanhSach;

$(function () {
    $_Khung = $('#khung_cot_diem');
    $_DanhSach = $_Khung.find('.tbody');

    khoiTaoForm($_Khung.find('#tao_cot_diem_form'));
    khoiTaoItem($_DanhSach.children());
})

function khoiTaoForm($form) {
    khoiTaoLCTForm($form, {
        submit: function () {
            $.ajax({
                url: '/BangDiem/XuLyThemCotDiem',
                type: 'POST',
                data: $form.serialize(),
                dataType: 'JSON'
            }).done(function (data) {
                if (data.trangThai == 0) {
                    var $item = $(data.ketQua);
                    khoiTaoItem($item);

                    $_DanhSach.append($item);

                    khoiTaoLCTFormMacDinh($form);
                }
                else {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: 'Thêm cột thất bại',
                        bieuTuong: 'nguy-hiem'
                    });
                }
            }).fail(function () {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Thêm cột thất bại',
                    bieuTuong: 'nguy-hiem'
                });
            });
        }
    });
}

function khoiTaoItem($item) {
    //Nút tắt mở
    khoiTaoTatMoDoiTuong($item.find('[data-chuc-nang="tat-mo"]'));

    //Nút xóa
    $item.find('[data-chuc-nang="xoa"]').on('click', function () {
        var $item = $(this).closest('[data-doi-tuong="item-cot-diem"]');

        $.ajax({
            url: '/BangDiem/XuLyXoaCotDiem/' + $item.attr('data-ma'),
            type: 'POST',
            dataType: 'JSON'
        }).done(function (data) {
            if (data.trangThai == 0) {
                $item.remove();
            }
            else {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Xóa cột điểm thất bại',
                    bieuTuong: 'nguy-hiem'
                });
            }
        }).fail(function () {
            moPopup({
                tieuDe: 'Thông báo',
                thongBao: 'Xóa cột điểm thất bại',
                bieuTuong: 'nguy-hiem'
            });
        });
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

                $itemKeo.siblings('[data-doi-tuong="item-cot-diem"]').on({
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
                        $_Khung.find('[data-doi-tuong="item-cot-diem"]').off('.keo-thu-tu');

                        if (mangTam.keo === true) {
                            var viTriHienTai = $itemKeo.index();
                            var viTriBatDau = mangTam.viTriBatDau;

                            if (viTriHienTai == viTriBatDau) {
                                return;
                            }

                            $.ajax({
                                url: '/BangDiem/XuLyCapNhatThuTuCotDiem/',
                                type: 'POST',
                                data: { thuTuCu: viTriBatDau + 1, thuTuMoi: viTriHienTai + 1, maKhoaHoc: maKhoaHoc },
                                dataType: 'JSON'
                            }).done(function (data) {
                                if (data.trangThai != 0) {
                                    moPopup({
                                        tieuDe: 'Thông báo',
                                        thongBao: 'Thay đổi thứ tự thất bại',
                                        bieuTuong: 'loi'
                                    });

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