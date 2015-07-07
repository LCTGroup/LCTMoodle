using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOLayer;
using DTOLayer;
using System.IO;
using System.Web;
using Data;

namespace BUSLayer
{
    public class BaiVietBaiGiangBUS : BUS
    {
        public static KetQua kiemTra(BaiVietBaiGiangDTO baiViet, string[] truong = null, bool kiemTra = true)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (coKiemTra("TieuDe", truong, kiemTra) && string.IsNullOrWhiteSpace(baiViet.tieuDe))
            {
                loi.Add("Tiêu đề không được bỏ trống");
            }
            if (coKiemTra("NoiDung", truong, kiemTra) && string.IsNullOrWhiteSpace(baiViet.noiDung))
            {
                loi.Add("Nội dung không được bỏ trống");
            }
            if (coKiemTra("MaNguoiTao", truong, kiemTra) && baiViet.nguoiTao == null)
            {
                loi.Add("Người tạo không được bỏ trống");
            }
            if (coKiemTra("MaKhoaHoc", truong, kiemTra) && baiViet.khoaHoc == null)
            {
                loi.Add("Khóa học không được bỏ trống");
            }
            #endregion

            if (loi.Count > 0)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = loi
                };
            }
            else
            {
                return new KetQua()
                {
                    trangThai = 0
                };
            }
        }

        public static void gan(ref BaiVietBaiGiangDTO baiViet, Form form)
        {
            if (baiViet == null)
            {
                baiViet = new BaiVietBaiGiangDTO();
            }

            foreach (string key in form.Keys.ToArray())
            {
                switch (key)
                {
                    case "TieuDe":
                        baiViet.tieuDe = form.layString(key);
                        break;
                    case "NoiDung":
                        baiViet.noiDung = form.layString(key);
                        break;
                    case "TomTat":
                        baiViet.tomTat = form.layString(key);
                        break;
                    case "MaTapTin":
                        baiViet.tapTin = TapTinBUS.chuyen("BaiVietBaiGiang_TapTin", form.layInt(key)).ketQua as TapTinDTO;
                        break;
                    case "MaNguoiTao":
                        baiViet.nguoiTao = form.layDTO<NguoiDungDTO>(key);
                        break;
                    case "MaKhoaHoc":
                        baiViet.khoaHoc = form.layDTO<KhoaHocDTO>(key);
                        break;
                    default:
                        break;
                }
            }
        }

        public static BangCapNhat layBangCapNhat(BaiVietBaiGiangDTO baiViet, string[] keys)
        {
            BangCapNhat bangCapNhat = new BangCapNhat();
            foreach (string key in keys)
            {
                switch (key)
                {
                    case "TieuDe":
                        bangCapNhat.Add(key, baiViet.tieuDe, 2);
                        break;
                    case "NoiDung":
                        bangCapNhat.Add(key, baiViet.noiDung, 2);
                        break;
                    case "TomTat":
                        bangCapNhat.Add(key, baiViet.tomTat, 2);
                        break;
                    case "MaTapTin":
                        bangCapNhat.Add(key, baiViet.tapTin == null ? null : baiViet.tapTin.ma.ToString(), 1);
                        break;
                    default:
                        break;
                }
            }
            return bangCapNhat;
        }

        public static KetQua them(Form form)
        {
            #region Kiểm tra điều kiện
            //Lấy mã người dùng
            var maNguoiTao = form.layInt("MaNguoiTao");
            if (!maNguoiTao.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 4
                };
            }

            //Lấy mã khóa học
            var maKhoaHoc = form.layInt("MaKhoaHoc");
            if (!maKhoaHoc.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Khóa học không thể bỏ trống"
                };
            }

            if (coQuyen("BG_Them", "KH", maKhoaHoc.Value, maNguoiTao))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền thêm bài giảng"
                };
            }
            #endregion

            BaiVietBaiGiangDTO baiVietBaiGiang = new BaiVietBaiGiangDTO();
            gan(ref baiVietBaiGiang, form);
            
            var ketQua = kiemTra(baiVietBaiGiang);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return BaiVietBaiGiangDAO.them(baiVietBaiGiang, new LienKet()
            {
                "TapTin"
            });
        }

        public static KetQua layTheoMaKhoaHoc(int maKhoaHoc, LienKet lienKet = null)
        {
            return BaiVietBaiGiangDAO.layTheoMaKhoaHoc(maKhoaHoc, lienKet);
        }

        public static KetQua layTheoMa(int ma, LienKet lienKet = null)
        {
            return BaiVietBaiGiangDAO.layTheoMa(ma, lienKet);
        }

        public static KetQua xoaTheoMa(int ma, int maNguoiXoa)
        {
            #region Kiểm tra điều kiện
            //Lấy bài giảng
            var ketQua = BaiVietBaiGiangDAO.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return new KetQua()
                {
                    trangThai = 1,
                    ketQua = "Bài giảng không tồn tại"
                };
            }

            var baiGiang = ketQua.ketQua as BaiVietBaiGiangDTO;

            if (baiGiang.nguoiTao.ma != maNguoiXoa && !coQuyen("BG_Xoa", "KH", baiGiang.khoaHoc.ma.Value, maNguoiXoa))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền xóa bài giảng"
                };
            }
            #endregion

            return BaiVietBaiGiangDAO.xoaTheoMa(ma);
        }

        public static KetQua capNhatTheoMa(Form form)
        {
            #region Kiểm tra điều kiện
            //Lấy mã người dùng
            var maNguoiSua = form.layInt("MaNguoiSua");
            if (!maNguoiSua.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Mã người sửa ko được bỏ trống"
                };
            }

            //Lấy bài giảng
            var ma = form.layInt("Ma");
            if (!ma.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Mã bài giảng không thể bỏ trống"
                };
            }
            var ketQua = BaiVietBaiGiangDAO.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return new KetQua()
                    {
                        trangThai = 1,
                        ketQua = "Bài giảng không tồn tại"
                    };
            }
            var baiGiang = ketQua.ketQua as BaiVietBaiGiangDTO;

            if (baiGiang.nguoiTao.ma != maNguoiSua && !coQuyen("BG_Sua", "KH", baiGiang.khoaHoc.ma.Value))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền sửa bài giảng"
                };
            }
            #endregion

            gan(ref baiGiang, form);

            ketQua = kiemTra(baiGiang, form.Keys.ToArray());

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            BangCapNhat bang = layBangCapNhat(baiGiang, form.Keys.ToArray());
            if (!bang.coDuLieu())
            {
                return new KetQua(baiGiang);
            }

            return BaiVietBaiGiangDAO.capNhatTheoMa(ma, bang, new LienKet()
            {
                "NguoiTao",
                "TapTin"
            });
        }
    }
}
