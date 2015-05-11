var $khungHienThi, $danhSach;

$(function () {
    $khungHienThi = $('#khung_hien_thi');

    hienThiNhom();
});

function hienThiNhom() {
    var nhom = layQueryString('nhom').toLowerCase();

    switch (nhom) {
        case 'diendan':
            hienThiNhom_DienDan();
            break;
    }
}



/*
    Nhóm diễn đàn
*/
//Xử lý hiển thị
function hienThiNhom_DienDan() {
    var $khungFormTao = layKhungFormTao_DienDan();
    var $khungDanhSach = layKhungDanhSach_DienDan();

    $khungHienThi.html($khungFormTao).append($khungDanhSach);

    layDanhSach_DienDan();
}

/*
    Khung form tạo bài viết
*/
//Tạo
function layKhungFormTao_DienDan() {
    var $form = $(
        '<section class="hop hop-2-vien">' +
            '<section class="tieu-de">' +
                'Đăng bài viết' +
            '</section>' +
            '<form class="noi-dung lct-form" id="tao_bai_viet_form">' +
                '<input type="hidden" name="KhoaHoc" value="' + maKhoaHoc + '" />' +
                '<article>' +
                    '<ul>' +
                        '<li>' +
                            '<section class="noi-dung">' +
                                '<article class="input">' +
                                    '<input data-validate="bat-buoc" data-chuc-nang="bat-dau-tao-bai-viet" type="text" name="TieuDe" placeholder="Tiêu đề" />' +
                                '</article>' +
                            '</section>' +
                        '</li>' +
                        '<li data-doi-tuong="tao-bai-viet">' +
                            '<section class="noi-dung">' +
                                '<section class="input">' +
                                    '<textarea data-validate="bat-buoc" name="NoiDung" data-input-type="editor" placeholder="Nội dung"></textarea>' +
                                '</section>' +
                            '</section>' +
                        '</li>' +
                        '<li data-doi-tuong="tao-bai-viet">' +
                            '<section class="noi-dung">' +
                                '<article class="input">' +
                                    '<input type="file" name="TapTin" value="" data-thu-muc="HinhDaiDien_ChuDe" />' +
                                '</article>' +
                            '</section>' +
                        '</li>' +
                    '</ul>' +
                    '<section data-doi-tuong="tao-bai-viet" class="lct-khung-button">' +
                        '<button type="submit" class="chap-nhan">' +
                            'Đăng' +
                        '</button>' +
                    '</section>' +
                '</article>' +
            '</form>' +
        '</section>'
    );

    khoiTaoForm_DienDan($form.find('#tao_bai_viet_form'));

    return $form;
}

/*
    Form tạo bài viết
*/
//Khởi tạo
function khoiTaoForm_DienDan($form) {
    khoiTaoLCTForm($form);

    var $doiTuongTaoBaiViet = $form.find('[data-doi-tuong="tao-bai-viet"]');

    $doiTuongTaoBaiViet.hide();

    $form.find('[data-chuc-nang="bat-dau-tao-bai-viet"]').on('focus', function () {
        $doiTuongTaoBaiViet.show();
    });

    $form.on('submit', function () {
        $.ajax({
            url: '/BaiVietDienDan/XuLyThem',
            type: 'POST',
            data: $form.serialize(),
            dataType: 'JSON',
            processData: false
        }).done(function (data) {
            if (data.trangThai == 0) {
                $danhSach.prepend(layBaiViet_DienDan(data.ketQua));

                khoiTaoGiaTriMacDinh_LCT($form);
            }
            else {
                moPopup({
                    tieuDe: 'Thông báo',
                    thongBao: 'Thêm bài viết thất bại'
                });
            }
        }).fail(function () {
            moPopup({
                tieuDe: 'Thông báo',
                thongBao: 'Thêm bài viết thất bại'
            });
        });
    });
}

/*
    Khung danh sách bài viết
*/
//Lấy
function layKhungDanhSach_DienDan() {
    var $khungDanhSach = $(
        '<section class="hop">' +
            '<section class="tieu-de">' +
                'Dánh sách bài viết' +
            '</section>' +
            '<ul id="danh_sach_bai_viet" class="noi-dung danh-sach-bai-viet">' +
            '</ul>' +
        '</section>'
    );

    $danhSach = $khungDanhSach.find('#danh_sach_bai_viet');

    return $khungDanhSach;
}

//Khởi tạo
function khoiTaoKhungDanhSach_DienDan($khungDanhSach) {

}

/*
    Danh sách bài viết
*/
function layDanhSach_DienDan() {
    $.ajax({
        url: '/BaiVietDienDan/LayDanhSach',
        type: 'GET',
        data: { maKhoaHoc: maKhoaHoc },
        dataType: 'JSON',
        async: false
    }).done(function (data) {
        if (data.trangThai == 0) {
            $(data.ketQua).each(function () {
                $danhSach.append(layBaiViet_DienDan(this));
            })
        }
        else if (data.trangThai != 1) {
            moPopup({
                tieuDe: 'Thông báo',
                thongBao: 'Lấy danh sách bài viết thất bại'
            });
        }
    }).fail(function () {
        moPopup({
            tieuDe: 'Thông báo',
            thongBao: 'Lấy danh sách bài viết thất bại'
        });
    });
}

/*
    Bài viết
*/
//Lấy
function layBaiViet_DienDan(baiViet) {
    var t = new Date(parseInt(baiViet.thoiDiemTao.substr(6)));
    thoiDiemTao =
        t.getHours() + ' giờ ' +
        t.getMinutes() + ' phút ' +
        '... tạm';

    var $htmlBaiViet = $(
        '<li>' +
            '<section class="khung-bai-viet">' +
                '<section class="khung-chuc-nang"></section>' +
                '<section class="khung-thong-tin">' +
                    '<section class="trai">' +
                        '<article class="hinh-dai-dien">' +
                            '<img src="#" />' +
                        '</article>' +
                    '</section>' +
                    '<section class="phai">' +
                        '<section class="nguoi-dung">' +
                            '<a href="#">Lê Bình Chiêu</a>' +
                        '</section>' +
                        '<section class="bai-viet">' +
                            '<section class="thoi-gian">' +
                                 thoiDiemTao +
                            '</section>' +
                        '</section>' +
                    '</section>' +
                '</section>' +
                '<section class="khung-noi-dung">' +
                    '<h1 class="tieu-de">' +
                        baiViet.tieuDe +
                    '</h1>' +
                    '<section class="noi-dung noi-dung-ckeditor">' +
                        baiViet.noiDung +
                    '</section>' +
                '</section>' +
                '<section class="khung-dieu-khien"></section>' +
            '</section>' +
        '</li>'
    );

    khoiTaoBaiViet_DienDan($htmlBaiViet);

    return $htmlBaiViet;
}

//Khởi tạo
function khoiTaoBaiViet_DienDan($baiViet) {
    
}