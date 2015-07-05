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
    public class BaiVietTaiLieuBUS : BUS
    {
        public static KetQua kiemTra(BaiVietTaiLieuDTO baiViet, string[] truong = null, bool kiemTra = true)
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

        public static void gan(ref BaiVietTaiLieuDTO baiViet, Form form)
        {
            if (baiViet == null)
            {
                baiViet = new BaiVietTaiLieuDTO();
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
                    case "MaTapTin":
                        baiViet.tapTin = TapTinBUS.chuyen("BaiVietTaiLieu_TapTin", form.layInt(key)).ketQua as TapTinDTO;
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

        public static BangCapNhat layBangCapNhat(BaiVietTaiLieuDTO baiViet, string[] keys)
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
                    trangThai = 3,
                    ketQua = "Mã người sửa ko được bỏ trống"
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

            if (coQuyen("TL_Them", "KH", maKhoaHoc.Value, maNguoiTao))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền thêm tài liệu"
                };
            }
            #endregion

            BaiVietTaiLieuDTO taiLieu = new BaiVietTaiLieuDTO();
            gan(ref taiLieu, form);
            
            KetQua ketQua = kiemTra(taiLieu);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return BaiVietTaiLieuDAO.them(taiLieu, new LienKet()
            {
                "TapTin"
            });
        }

        public static KetQua layTheoMaKhoaHoc(int maKhoaHoc)
        {
            return BaiVietTaiLieuDAO.layTheoMaKhoaHoc(maKhoaHoc, new LienKet()
            {
                "TapTin"
            });
        }

        public static KetQua layTheoMa(int ma)
        {
            return BaiVietTaiLieuDAO.layTheoMa(ma);
        }

        public static KetQua xoaTheoMa(int ma, int maNguoiXoa)
        {
            #region Kiểm tra điều kiện
            //Lấy tài liệu
            var ketQua = BaiVietTaiLieuDAO.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return new KetQua()
                {
                    trangThai = 1,
                    ketQua = "Tài liệu không tồn tại"
                };
            }

            var taiLieu = ketQua.ketQua as BaiVietTaiLieuDTO;

            if (taiLieu.nguoiTao.ma != maNguoiXoa && !coQuyen("TL_Xoa", "KH", taiLieu.khoaHoc.ma.Value, maNguoiXoa))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền xóa tài liệu"
                };
            }
            #endregion

            return BaiVietTaiLieuDAO.xoaTheoMa(ma);
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
                    trangThai = 4
                };
            }

            //Lấy bài giảng
            var ma = form.layInt("Ma");
            if (!ma.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Mã tài liệu không thể bỏ trống"
                };
            }
            var ketQua = BaiVietTaiLieuDAO.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return new KetQua()
                {
                    trangThai = 1,
                    ketQua = "Tài liệu không tồn tại"
                };
            }
            var taiLieu = ketQua.ketQua as BaiVietTaiLieuDTO;

            if (taiLieu.nguoiTao.ma != maNguoiSua && !coQuyen("TL_Sua", "KH", taiLieu.khoaHoc.ma.Value))
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền sửa tài liệu"
                };
            }
            #endregion

            gan(ref taiLieu, form);

            ketQua = kiemTra(taiLieu, form.Keys.ToArray());

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            BangCapNhat bang = layBangCapNhat(taiLieu, form.Keys.ToArray());
            if (!bang.coDuLieu())
            {
                return new KetQua(1);
            }

            return BaiVietTaiLieuDAO.capNhatTheoMa(ma, bang, new LienKet()
            {
                "NguoiTao",
                "TapTin"
            });
        }
    }
}
