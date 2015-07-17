function khoiTaoKhungTimKiemLCT2($danhSach, $khungTim, $khungPhanTrang, duongDan, thamSo) {
    if (typeof (thamSo) === 'undefined') {
        thamSo = {};
    }

    var
        maChuDeTim = 0,
        tuKhoaTim = '',
        trangTim = 1,
        htmlDanhSach = $danhSach.html();
        danhSachTrang = $khungPhanTrang.html();

    var $khungChuDe = $('<section style="margin-top: 5px; display: none;"></section>');
    $khungTim.after($khungChuDe);

    khoiTaoLCTForm($khungTim, {
        custom: [{
            input: $khungTim.find('[data-doi-tuong=chu-de-tim]'),
            event: {
                valueChange: function () {
                    maChuDeTim = this.value || 0;

                    if (maChuDeTim != 0) {
                        $khungTim.tim();
                    }
                    else if (tuKhoaTim == '') {
                        $danhSach.html(htmlDanhSach);
                        $khungPhanTrang.html(danhSachTrang);
                        trangTim = 1;
                        khoiTaoPhanTrang();
                    }
                }
            }
        }, {
            input: $khungTim.find('[data-doi-tuong="ten-tim"]'),
            event: {
                keyup: function () {
                    var $input = $(this);

                    var maTam = 'tim_';
                    var giaTriTam = $input.val();

                    if (giaTriTam.length == 0) {
                        tuKhoaTim = mangTam[maTam + 'gt'] = '';

                        if (maChuDeTim == 0) {
                            $danhSach.html(htmlDanhSach);
                            $khungPhanTrang.html(danhSachTrang);
                            trangTim = 1;
                            khoiTaoPhanTrang();
                        }
                        else {
                            $khungTim.tim();
                        }
                        return;
                    }

                    if (mangTam[maTam + 'gt'] == giaTriTam) {
                        return;
                    }

                    mangTam[maTam + 'td'] = true;
                    clearTimeout(mangTam[maTam + 'to']);
                    mangTam[maTam + 'to'] = setTimeout(function () {
                        tuKhoaTim = mangTam[maTam + 'gt'] = giaTriTam;
                        $khungTim.tim();
                    }, 500)
                }
            }
        }, {
            input: $khungTim.find('[data-chuc-nang="tat-mo-chu-de"]'),
            event: {
                click: function () {
                    var $nut = $(this);

                    //Nếu chưa có khung chủ đề thì load
                    if ($khungChuDe.is(':empty')) {
                        $.ajax({
                            url: '/ChuDe/_Chon',
                            dataType: 'JSON',
                            async: false
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                $khungChuDe.html(data.ketQua);

                                khoiTaoKhungChuDe($khungChuDe.find('#khung_chu_de'), {
                                    event: {
                                        'chon': function (e, data) {
                                            maChuDeTim = data.ma || 0;

                                            if (maChuDeTim != 0) {
                                                $khungTim.tim();
                                            }
                                            else if (tuKhoaTim == '') {
                                                $danhSach.html(htmlDanhSach);
                                                $khungPhanTrang.html(danhSachTrang);
                                                trangTim = 1;
                                                khoiTaoPhanTrang();
                                            }
                                        }
                                    }
                                });
                            }
                            else {
                                moPopupThongBao(data);
                            }
                        }).fail(function () {
                            moPopupThongBao('Mở khung chọn chủ đề thất bại');
                        });
                    }

                    $khungChuDe.toggle(0);
                }
            }
        }]
    });

    $khungPhanTrang.chon = function (trang) {
        $khungPhanTrang.find('.chon').removeClass('chon');
        $khungPhanTrang.find('[data-trang="' + trang + '"]').addClass('chon');
    }

    khoiTaoPhanTrang();

    function khoiTaoPhanTrang() {
        $khungPhanTrang.find('[data-chuc-nang="chuyen-trang"]').on('click', function () {
            var $nut = $(this);
            trangTim = $nut.data('trang');
            
            $khungTim.tim(trangTim);
        });
        $khungPhanTrang.chon(trangTim);
    }

    //#region Hàm xử lý

    $khungTim.tim = function (trang) {
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

        trangTim = trang || 1;

        data.tuKhoa = tuKhoaTim;
        data.maChuDe = maChuDeTim;
        data.trang = trangTim;

        if ('Tim_Ajax' in mangTam) {
            mangTam['Tim_Ajax'].abort();
        }

        var $tai = moBieuTuongTai($danhSach);
        mangTam['Tim_Ajax'] = $.ajax({
            url: duongDan,
            data: data,
            dataType: 'JSON'
        }).always(function () {
            $tai.tat();
        }).done(function (data) {
            if (data.trangThai == 0) {
                $danhSach.html(data.ketQua.danhSach);

                $khungPhanTrang.html($(data.ketQua.phanTrang).html());
                khoiTaoPhanTrang();
            }
            else {
                $danhSach.empty();
                $khungPhanTrang.html('');
            }
        }).fail(function (xhr, status) {
            if (status !== 'abort') {
                $danhSach.empty();
                $khungPhanTrang.html('');
            }
        });
    }

    //#endregion
}