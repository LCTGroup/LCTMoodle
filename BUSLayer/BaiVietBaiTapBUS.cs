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
    public class BaiVietBaiTapBUS : BUS
    {
        public static KetQua kiemTra(BaiVietBaiTapDTO baiViet, string[] truong = null, bool kiemTra = true)
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

        public static void gan(ref BaiVietBaiTapDTO baiViet, Form form)
        {
            if (baiViet == null)
            {
                baiViet = new BaiVietBaiTapDTO();
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
                        baiViet.tapTin = TapTinBUS.chuyen("BaiVietBaiTap_TapTin", form.layInt(key)).ketQua as TapTinDTO;
                        form[key] = baiViet.tapTin == null ? null : baiViet.tapTin.ma.ToString();
                        break;
                    case "CoThoiDiemHetHan":
                        baiViet.thoiDiemHetHan = form.layBool("CoThoiDiemHetHan") ? form.layDateTime("ThoiDiemHetHan") : null;
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

        public static BangCapNhat layBangCapNhat(BaiVietBaiTapDTO baiViet, string[] keys)
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
                    case "CoThoiDiemHetHan":
                        bangCapNhat.Add("ThoiDiemHetHan", baiViet.thoiDiemHetHan.HasValue ? baiViet.thoiDiemHetHan.ToString() : null, true);
                        break;
                    default:
                        break;
                }
            }
            return bangCapNhat;
        }

        public static KetQua them(Dictionary<string, string> form)
        {
            KetQua ketQua = TapTinBUS.chuyen("BaiVietBaiTap_TapTin", layInt(form, "TapTin"));

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            BaiVietBaiTapDTO baiViet = new BaiVietBaiTapDTO()
            {
                tieuDe = layString(form, "TieuDe"),
                noiDung = layString(form, "NoiDung"),
                tapTin = ketQua.ketQua as TapTinDTO,
                thoiDiemHetHan = layDateTime(form, "ThoiDiemHetHan"),
                nguoiTao = layDTO<NguoiDungDTO>(Session["NguoiDung"] as int?),
                khoaHoc = layDTO<KhoaHocDTO>(form, "KhoaHoc")
            };
            
            ketQua = kiemTra(baiViet);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return BaiVietBaiTapDAO.them(baiViet, new LienKet()
                {
                    "NguoiTao",
                    "TapTin"
                });
        }

        public static KetQua layTheoMaKhoaHoc(int maKhoaHoc)
        {
            return BaiVietBaiTapDAO.layTheoMaKhoaHoc(maKhoaHoc, new LienKet()
            {
                "NguoiTao",
                "TapTin",
                "BaiTapNop"
            });
        }

        public static KetQua layTheoMa(int ma)
        {
            return BaiVietBaiTapDAO.layTheoMa(ma);
        }

        public static KetQua xoaTheoMa(int ma)
        {
            return BaiVietBaiTapDAO.xoaTheoMa(ma);
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

            BaiVietBaiTapDTO baiViet = ketQua.ketQua as BaiVietBaiTapDTO;

            gan(ref baiViet, form);

            ketQua = kiemTra(baiViet, form.Keys.ToArray());

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return BaiVietBaiTapDAO.capNhatTheoMa(maBaiViet, layBangCapNhat(baiViet, form.Keys.ToArray()), new LienKet()
            {
                "NguoiTao",
                "TapTin"
            });
        }
    }
}
