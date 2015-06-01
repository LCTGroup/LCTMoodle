using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOLayer;
using DTOLayer;
using System.IO;
using Data;
using System.Web;

namespace BUSLayer
{
    public class BaiVietDienDanBUS : BUS
    {
        public static KetQua kiemTra(BaiVietDienDanDataDTO baiViet)
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
            KetQua ketQua = TapTinBUS.chuyen("BaiVietDienDan_TapTin", layInt(form, "TapTin"));

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }
            
            BaiVietDienDanDataDTO baiViet = new BaiVietDienDanDataDTO()
            {
                tieuDe = layString(form, "TieuDe"),
                noiDung = layString(form, "NoiDung"),
                maTapTin = (ketQua.ketQua as TapTinViewDTO).ma,
                maNguoiTao = (int)HttpContext.Current.Session["NguoiDung"],
                maKhoaHoc = layInt(form, "KhoaHoc")
            };
            
            ketQua = kiemTra(baiViet);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            BaiVietDienDanDAO.lienKet = new string[] 
            {
                "NguoiTao",
                "TapTin"
            };
            return BaiVietDienDanDAO.them(baiViet);
        }

        public static KetQua layTheoMaKhoaHoc(int maKhoaHoc)
        {
            BaiVietDienDanDAO.lienKet = new string[] 
            {
                "NguoiTao",
                "TapTin"
            };
            return BaiVietDienDanDAO.layTheoMaKhoaHoc(maKhoaHoc);
        }
    }
}
