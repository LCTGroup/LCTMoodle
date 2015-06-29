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
            if (coKiemTra("TieuDe", truong, kiemTra) && string.IsNullOrEmpty(baiViet.tieuDe))
            {
                loi.Add("Tiêu đề không được bỏ trống");
            }
            if (coKiemTra("NoiDung", truong, kiemTra) && string.IsNullOrEmpty(baiViet.noiDung))
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
            BaiVietTaiLieuDTO baiVietTaiLieu = new BaiVietTaiLieuDTO();
            gan(ref baiVietTaiLieu, form);
            
            KetQua ketQua = kiemTra(baiVietTaiLieu);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return BaiVietTaiLieuDAO.them(baiVietTaiLieu, new LienKet()
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

        public static KetQua xoaTheoMa(int ma)
        {
            return BaiVietTaiLieuDAO.xoaTheoMa(ma);
        }

        public static KetQua capNhatTheoMa(Form form)
        {
            int? maBaiViet = form.layInt("Ma");
            if (!maBaiViet.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 1
                };
            }

            KetQua ketQua = BaiVietTaiLieuDAO.layTheoMa(maBaiViet);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            BaiVietTaiLieuDTO baiViet = ketQua.ketQua as BaiVietTaiLieuDTO;

            gan(ref baiViet, form);

            ketQua = kiemTra(baiViet, form.Keys.ToArray());

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return BaiVietTaiLieuDAO.capNhatTheoMa(maBaiViet, layBangCapNhat(baiViet, form.Keys.ToArray()), new LienKet()
            {
                "NguoiTao",
                "TapTin"
            });
        }
    }
}
