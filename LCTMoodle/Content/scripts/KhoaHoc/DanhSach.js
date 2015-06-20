var $_DanhSach;

$(function () {
    $_DanhSach = $('#danh_sach');

    khoiTaoChucNangTimKiem($('#tim_kiem'));
});

//#region Chức năng

function khoiTaoChucNangTimKiem($inputs) {
    var htmlDanhSach = $_DanhSach.html();

    $inputs.on('keyup', function () {
        var $input = $(this);

        var maTam = 'tim_kh_';
        var giaTriTam = $input.val();

        if (giaTriTam.length == 0) {
            mangTam[maTam + 'gt'] = giaTriTam;
            $_DanhSach.html(htmlDanhSach);
            return;
        }

        if (mangTam[maTam + 'gt'] == giaTriTam) {
            return;
        }

        mangTam[maTam + 'td'] = true;
        clearTimeout(mangTam[maTam + 'to']);
        mangTam[maTam + 'to'] = setTimeout(function () {
            mangTam[maTam + 'gt'] = giaTriTam;

            $.ajax({
                url: '/KhoaHoc/_DanhSach_Tim',
                data: { tuKhoa: giaTriTam },
                dataType: 'JSON'
            }).done(function (data) {
                if (data.trangThai == 0) {
                    $_DanhSach.html(data.ketQua);
                }
                else {
                    $_DanhSach.html('');
                }
            }).fail(function () {
                $_DanhSach.html('');
            });
        }, 500)
    })
}

//#endregion