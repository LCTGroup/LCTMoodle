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
            if (coKiemTra("TieuDe", truong, kiemTra) && string.IsNullOrEmpty(baiViet.tieuDe))
            {
                loi.Add("Tiêu đề không được bỏ trống");
            }
            if (coKiemTra("NoiDung", truong, kiemTra) && string.IsNullOrEmpty(baiViet.noiDung))
            {
                loi.Add("Nội dung không được bỏ trống");
            }
            if (coKiemTra("NguoiTao", truong, kiemTra) && baiViet.nguoiTao == null)
            {
                loi.Add("Người tạo không được bỏ trống");
            }
            if (coKiemTra("KhoaHoc", truong, kiemTra) && baiViet.khoaHoc == null)
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

        public static void gan(ref BaiVietBaiGiangDTO baiViet, ref Form form)
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
                    case "TapTin":
                        baiViet.tapTin = TapTinBUS.chuyen("BaiVietBaiGiang_TapTin", form.layInt(key)).ketQua as TapTinDTO;
                        form[key] = baiViet.tapTin == null ? null : baiViet.tapTin.ma.ToString();
                        break;
                    case "ThoiDiemTao":
                        baiViet.thoiDiemTao = form.layDateTime(key);
                        break;
                    case "NguoiTao":
                        baiViet.nguoiTao = form.layDTO<NguoiDungDTO>(key);
                        break;
                    case "KhoaHoc":
                        baiViet.khoaHoc = form.layDTO<KhoaHocDTO>(key);
                        break;
                    default:
                        break;
                }
            }
        }

        public static BangCapNhat layBangCapNhat(string[] keys, BaiVietBaiGiangDTO baiViet)
        {
            BangCapNhat bangCapNhat = new BangCapNhat();
            foreach (string key in keys)
            {
                switch (key)
                {
                    case "TieuDe":
                        bangCapNhat.Add("TieuDe", baiViet.tieuDe, true);
                        break;
                    case "NoiDung":
                        bangCapNhat.Add("NoiDung", baiViet.noiDung, true);
                        break;
                    case "TapTin":
                        bangCapNhat.Add("MaTapTin", baiViet.tapTin == null ? null : baiViet.tapTin.ma.ToString(), false);
                        break;
                    default:
                        break;
                }
            }
            return bangCapNhat;
        }

        public static KetQua them(Dictionary<string, string> form)
        {
            KetQua ketQua = TapTinBUS.chuyen("BaiVietBaiGiang_TapTin", layInt(form, "TapTin"));

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            BaiVietBaiGiangDTO baiVietBaiGiang = new BaiVietBaiGiangDTO()
            {
                tieuDe = layString(form, "TieuDe"),
                noiDung = layString(form, "NoiDung"),
                tapTin = ketQua.ketQua as TapTinDTO,
                nguoiTao = layDTO<NguoiDungDTO>(Session["NguoiDung"] as int?),
                khoaHoc = layDTO<KhoaHocDTO>(form, "KhoaHoc")
            };
            
            ketQua = kiemTra(baiVietBaiGiang);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return BaiVietBaiGiangDAO.them(baiVietBaiGiang, new LienKet()
            {
                "TapTin"
            });
        }

        public static KetQua layTheoMaKhoaHoc(int maKhoaHoc)
        {
            return BaiVietBaiGiangDAO.layTheoMaKhoaHoc(maKhoaHoc, new LienKet()
            {
                "TapTin"
            });
        }

        public static KetQua layTheoMa(int ma)
        {
            return BaiVietBaiGiangDAO.layTheoMa(ma);
        }

        public static KetQua xoaTheoMa(int ma)
        {
            return BaiVietBaiGiangDAO.xoaTheoMa(ma);
        }

        public static KetQua capNhatTheoMa(Form form)
        {
            int? maBaiViet = layInt(form, "Ma");
            if (!maBaiViet.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 1
                };
            }

            KetQua ketQua = layTheoMa(maBaiViet.Value);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            BaiVietBaiGiangDTO baiViet = ketQua.ketQua as BaiVietBaiGiangDTO;

            gan(ref baiViet, ref form);

            ketQua = kiemTra(baiViet, form.Keys.ToArray());

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return BaiVietBaiGiangDAO.capNhatTheoMa(maBaiViet, layBangCapNhat(form.Keys.ToArray(), baiViet), new LienKet()
            {
                "NguoiTao",
                "TapTin"
            });
        }
    }
}
