function khoiTaoKhungTimKiemLCT($danhSach, $khungTim, duongDan, thamSo) {
    if (typeof (thamSo) === 'undefined') {
        thamSo = {};
    }

    var 
        maChuDeTim = 0,
        tuKhoaTim = '',
        htmlDanhSach = $danhSach.html();

    var $khungChuDe = $('<section style="margin-top: 5px; display: none;"></section>');
    $khungTim.after($khungChuDe);

    khoiTaoLCTForm($khungTim, {
        custom: [{
            input: $khungTim.find('[data-doi-tuong=chu-de-tim]'),
            event: {
                valueChanged: function () {
                    maChuDeTim = this.value || 0;

                    if (maChuDeTim != 0) {
                        timKiem();
                    }
                    else if (tuKhoaTim == '') {
                        $danhSach.html(htmlDanhSach);
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
                        }
                        else {
                            timKiem();
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
                        timKiem();
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
                                                timKiem();
                                            }
                                            else if (tuKhoaTim == '') {
                                                $danhSach.html(htmlDanhSach);
                                            }
                                        }
                                    }
                                });
                            }
                            else {
                                moPopup({
                                    tieuDe: 'Thông báo',
                                    thongBao: 'Mở khung chọn chủ đề thất bại',
                                    bieuTuong: 'nguy-hiem'
                                });
                            }
                        }).fail(function () {
                            moPopup({
                                tieuDe: 'Thông báo',
                                thongBao: 'Mở khung chọn chủ đề thất bại',
                                bieuTuong: 'nguy-hiem'
                            });
                        });
                    }

                    $khungChuDe.toggle(0);
                }
            }
        }]
    });

    //#region Hàm xử lý

    function timKiem() {
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
            data = { tuKhoa: tuKhoaTim, maChuDe: maChuDeTim }
        }
        else {
            data.tuKhoa = tuKhoaTim;
            data.maChuDe = maChuDeTim;
        }

        $.ajax({
            url: duongDan,
            data: data,
            dataType: 'JSON'
        }).done(function (data) {
            if (data.trangThai == 0) {
                $danhSach.html(data.ketQua);
            }
            else {
                $danhSach.empty();
            }
        }).fail(function () {
            $danhSach.empty();
        });
    }

    //#endregion
}