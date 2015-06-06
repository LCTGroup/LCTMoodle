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
        public static KetQua kiemTra(BaiTapNopDTO baiNop)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (baiNop.tapTin == null && string.IsNullOrEmpty(baiNop.duongDan))
            {
                loi.Add("Nội dung không được bỏ trống");
            }
            if (baiNop.nguoiTao == null)
            {
                loi.Add("Người tạo không được bỏ trống");
            }
            if (baiNop.baiVietBaiTap == null)
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

            BaiTapNopDTO baiTapNop = new BaiTapNopDTO()
            {
                tapTin = ketQua.ketQua as TapTinDTO,
                duongDan = layString(form, "DuongDan"),
                nguoiTao = layDTO<NguoiDungDTO>(Session["NguoiDung"] as int?),
                baiVietBaiTap = layDTO<BaiVietBaiTapDTO>(form, "BaiVietBaiTap")
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
