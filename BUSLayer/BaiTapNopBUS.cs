using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOLayer;
using DTOLayer;
using System.IO;
using Data;

namespace BUSLayer
{
    public class BaiTapNopBUS : BUS
    {
        public static KetQua kiemTra(BaiTapNopDataDTO baiNop)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (baiNop.maTapTin == 0 && string.IsNullOrEmpty(baiNop.duongDan))
            {
                loi.Add("Nội dung không được bỏ trống");
            }
            if (baiNop.maNguoiTao == 0)
            {
                loi.Add("Người tạo không được bỏ trống");
            }
            if (baiNop. maBaiVietBaiTap == 0)
            {
                loi.Add("Bài tập được nộp không được bỏ trống");
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

        public static KetQua themHoacCapNhat(Dictionary<string, string> form)
        {
            KetQua ketQua = TapTinBUS.chuyen("BaiTapNop_TapTin", layInt(form, "TapTin"));

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            BaiTapNopDataDTO baiTapNop = new BaiTapNopDataDTO()
            {
                maTapTin = (ketQua.ketQua as TapTinViewDTO).ma,
                duongDan = layString(form, "DuongDan"),
                maNguoiTao = (int)Session["NguoiDung"],
                maBaiVietBaiTap = layInt(form, "BaiVietBaiTap")
            };

            ketQua = kiemTra(baiTapNop);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return BaiTapNopDAO.themHoacCapNhat(baiTapNop);
        }

        public static KetQua layTheoMaBaiVietBaiTap(int maBaiVietBaiTap)
        {
            return BaiTapNopDAO.layTheoMaBaiVietBaiTap(maBaiVietBaiTap, new LienKet()
            {
                "NguoiTao",
                "TapTin"
            });
        }
    }
}
