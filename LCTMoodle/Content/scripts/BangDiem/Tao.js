var maKhoaHoc, $_Khung, $_DanhSach;

$(function () {
    $_Khung = $('#khung_cot_diem');
    $_DanhSach = $_Khung.find('tbody');

    khoiTaoItem($_DanhSach.children());
    khoiTaoNutTao($_Khung.find('[data-chuc-nang="tao"]'));

    capNhatThuTu();
})

function khoiTaoNutTao($nuts) {
    $nuts.on('click', function () {
        moPopupFull({
            url: '/BangDiem/_Form',
            data: {
                maKhoaHoc: maKhoaHoc
            },
            width: '500px',
            thanhCong: function ($popup) {
                var $form = $popup.find('#tao_cot_diem_form');

                khoiTaoLCTForm($form, {
                    submit: function () {
                        var $tai = moBieuTuongTai($form);
                        $.ajax({
                            url: '/BangDiem/XuLyThemCotDiem',
                            type: 'POST',
                            data: layDataLCTForm($form),
                            dataType: 'JSON'
                        }).always(function () {
                            $tai.tat();
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                $popup.tat();
                                var $item = $(data.ketQua);
                                khoiTaoItem($item);

                                $_DanhSach.append($item);
                                $item.find('.td:eq(0)').text($item.index() + 1);
                            }
                            else {
                                moPopupThongBao(data);
                            }
                        }).fail(function () {
                            moPopupThongBao('Thêm cột thất bại');
                        });
                    }
                });
            }
        });
    })
}

function khoiTaoItem($item) {
    //Nút tắt mở
    khoiTaoTatMoDoiTuong($item.find('[data-chuc-nang="tat-mo"]'));

    //Nút xóa
    $item.find('[data-chuc-nang="xoa"]').on('click', function () {
        var $item = $(this).closest('[data-doi-tuong="item-cot-diem"]');

        var $tai = moBieuTuongTai($_Khung);
        $.ajax({
            url: '/BangDiem/XuLyXoaCotDiem/' + $item.attr('data-ma'),
            type: 'POST',
            dataType: 'JSON'
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai == 0) {
                $item.remove();
            }
            else {
                moPopupThongBao(data);
            }
        }).fail(function () {
            moPopupThongBao('Xóa cột điểm thất bại');
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
                        $_Khung.find('[data-doi-tuong="item-cot-diem"]').off('.keo-thu-tu');

                        if (mangTam.keo === true) {
                            var viTriHienTai = $itemKeo.index();
                            var viTriBatDau = mangTam.viTriBatDau;

                            if (viTriHienTai == viTriBatDau) {
                                return;
                            }

                            var $tai = moBieuTuongTai($_Khung);
                            $.ajax({
                                url: '/BangDiem/XuLyCapNhatThuTuCotDiem/',
                                type: 'POST',
                                data: { thuTuCu: viTriBatDau + 1, thuTuMoi: viTriHienTai + 1, maKhoaHoc: maKhoaHoc },
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
                                moPopupThongBao('Thay đổi vị trí thất bại');
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

//#region Hỗ trợ

function capNhatThuTu() {
    $_DanhSach.find('tr').each(function (index) {
        $(this).find('td:eq(0)').text(index + 1);
    });
}

//#endregion