/*
    Hàm tạo khung bình luận
    Truyền vào loại đối tượng & mã đối tượng
*/
function layKhungBinhLuan(loaiDoiTuong, maDoiTuong) {
    var $khungBinhLuan = $('\
        <section data-doi-tuong="khung-binh-luan" class="khung-binh-luan">\
            <section class="khung-danh-sach">\
                <ul data-doi-tuong="danh-sach">\
                </ul>\
            </section>\
            <section class="khung-form">\
                <form data-doi-tuong="binh-luan-form" class="lct-form">\
                    <input name="Loai" value="' + loaiDoiTuong + '" type="hidden" />\
                    <input name="DoiTuong" value="' + maDoiTuong + '" type="hidden" />\
                    <ul style="padding: 0 10px 10px 10px; margin-top: 10px;">\
                        <li style="margin: 0;">\
                            <article class="noi-dung">\
                                <article class="input">\
                                    <input data-validate="bat-buoc" type="text" name="NoiDung" placeholder="Viết bình luận của bạn" />\
                                </article>\
                            </article>\
                        </li>\
                    </ul>\
                </form>\
            </section>\
        </section>\
    ');

    $.ajax({
        url: '/BinhLuan/Lay',
        data: { loaiDoiTuong: loaiDoiTuong, maDoiTuong: maDoiTuong },
        dataType: 'JSON',
        async: false
    }).done(function (data) {
        if (data.trangThai == 0) {
            var htmlDanhSachBinhLuan = '';
            $(data.ketQua).each(function () {
                htmlDanhSachBinhLuan += layBinhLuan(this);
            });
            
            $khungBinhLuan.find('[data-doi-tuong="danh-sach"]').html(htmlDanhSachBinhLuan);
        }
        else if (data.trangThai != 1) {
            moPopup({
                tieuDe: 'Thông báo',
                thongBao: 'Lấy bình luận thất bại',
                bieuTuong: 'nguy-hiem'
            })
        }
    }).fail(function () {
        moPopup({
            tieuDe: 'Thông báo',
            thongBao: 'Lấy bình luận thất bại',
            bieuTuong: 'nguy-hiem'
        })
    });

    khoiTaoKhungBinhLuan($khungBinhLuan);

    return $khungBinhLuan;
}

function layBinhLuan(binhLuan) {
    return '\
        <li class="muc-binh-luan">\
            <section class="trai">\
                <article class="hinh-dai-dien">\
                    <img src="#" />\
                </article>\
            </section>\
            <section class="phai">\
                <section class="nguoi-dung">\
                    <a href="#">Lê Bình Chiêu</a>\
                </section>\
                <section class="noi-dung">\
                    ' + binhLuan.noiDung + '\
                </section>\
                <section class="thong-tin">\
                    <span class="thoi-gian">18 giờ trước</span>\
                </section>\
            </section>\
        </li>\
    ';
}

function khoiTaoKhungBinhLuan($khung) {
    khoiTaoForm($khung.find('[data-doi-tuong="binh-luan-form"]'));
}

function khoiTaoForm($form) {
    khoiTaoLCTForm($form, {
        submit: function () {
            $.ajax({
                url: '/BinhLuan/Them',
                type: 'POST',
                data: $form.serialize(),
                dataType: 'JSON'
            }).done(function (data) {
                if (data.trangThai == 0) {
                    $form.closest('[data-doi-tuong="khung-binh-luan"]').find('[data-doi-tuong="danh-sach"]').append(layBinhLuan(data.ketQua));

                    khoiTaoLCTFormMacDinh($form);
                }
                else {
                    moPopup({
                        tieuDe: 'Thông báo',
                        thongBao: 'Thêm bình luận thất bại',
                        bieuTuong: 'nguy-hiem'
                    });
                }
            }).fail(function () {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Thêm bình luận thất bại',
                    bieuTuong: 'nguy-hiem'
                });
            });
        }
    });
}