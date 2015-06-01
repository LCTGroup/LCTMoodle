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
        public static KetQua kiemTra(BaiVietBaiGiangDataDTO baiViet)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (string.IsNullOrEmpty(baiViet.tieuDe))
            {
                loi.Add("Tiêu đề không được bỏ trống");
            }
            if (string.IsNullOrEmpty(baiViet.noiDung))
            {
                loi.Add("Nội dung không được bỏ trống");
            }
            if (baiViet.maNguoiTao == 0)
            {
                loi.Add("Người tạo không được bỏ trống");
            }
            if (baiViet.maKhoaHoc == 0)
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

        public static KetQua them(Dictionary<string, string> form)
        {
            KetQua ketQua = TapTinBUS.chuyen("BaiVietBaiGiang_TapTin", layInt(form, "TapTin"));

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            BaiVietBaiGiangDataDTO baiVietBaiGiang = new BaiVietBaiGiangDataDTO()
            {
                tieuDe = layString(form, "TieuDe"),
                noiDung = layString(form, "NoiDung"),
                maTapTin = (ketQua.ketQua as TapTinViewDTO).ma,
                maNguoiTao = (int)HttpContext.Current.Session["NguoiDung"],
                maKhoaHoc = layInt(form, "KhoaHoc")
            };
            
            ketQua = kiemTra(baiVietBaiGiang);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            BaiVietBaiGiangDAO.lienKet = new string[]
            {
                "TapTin"
            };
            return BaiVietBaiGiangDAO.them(baiVietBaiGiang);
        }

        public static KetQua layTheoMaKhoaHoc(int maKhoaHoc)
        {
            BaiVietBaiGiangDAO.lienKet = new string[]
            {
                "TapTin"
            };
            return BaiVietBaiGiangDAO.layTheoMaKhoaHoc(maKhoaHoc);
        }
    }
}
