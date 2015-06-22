var 
    $_DanhSach,
    $_KhungChuDe,
    maChuDeTim = 0,
    tuKhoaTim = '';

$(function () {
    $_DanhSach = $('#danh_sach');
    $_KhungChuDe = $('#chu_de');

    khoiTaoFormTimKiem();
});

//#region Chức năng

function khoiTaoFormTimKiem() {
    var htmlDanhSach = $_DanhSach.html();

    $_KhungChuDe.hide();

    khoiTaoLCTForm($('#form_tim'), {
        custom: [{
            input: $('#tim_chu_de'),
            event: {
                valueChanged: function () {
                    maChuDeTim = this.value || 0;

                    if (maChuDeTim != 0) {
                        timKiem();
                    }
                    else if (tuKhoaTim == '') {
                        $_DanhSach.html(htmlDanhSach);
                    }
                }
            }
        }, {
            input: $('#tim_ten'),
            event: {
                keyup: function () {
                    var $input = $(this);

                    var maTam = 'tim_kh_';
                    var giaTriTam = $input.val();

                    if (giaTriTam.length == 0) {
                        tuKhoaTim = mangTam[maTam + 'gt'] = '';

                        if (maChuDeTim == 0) {
                            $_DanhSach.html(htmlDanhSach);
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
            input: $('[data-chuc-nang="tat-mo-chu-de"]'),
            event: {
                click: function () {
                    var $nut = $(this);

                    //Nếu chưa có khung chủ đề thì load
                    if ($_KhungChuDe.is(':empty')) {
                        $.ajax({
                            url: '/ChuDe/_Chon',
                            dataType: 'JSON',
                            async: false
                        }).done(function (data) {
                            if (data.trangThai == 0) {
                                $_KhungChuDe.html(data.ketQua);

                                khoiTaoKhungChuDe($_KhungChuDe.find('#khung_chu_de'), {
                                    event: {
                                        'chon': function (e, data) {
                                            maChuDeTim = data.ma || 0;

                                            if (maChuDeTim != 0) {
                                                timKiem();
                                            }
                                            else if (tuKhoaTim == '') {
                                                $_DanhSach.html(htmlDanhSach);
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

                    $_KhungChuDe.toggle(0);
                }
            }
        }]
    });

    //#region Hàm xử lý

    function timKiem() {
        $.ajax({
            url: '/KhoaHoc/_DanhSach_Tim',
            data: { tuKhoa: tuKhoaTim, maChuDe: maChuDeTim },
            dataType: 'JSON'
        }).done(function (data) {
            if (data.trangThai == 0) {
                $_DanhSach.html(data.ketQua);
            }
            else {
                $_DanhSach.empty();
            }
        }).fail(function () {
            $_DanhSach.empty();
        });
    }

    //#endregion
}

//#endregion