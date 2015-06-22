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

        public static void gan(ref BaiTapNopDTO baiNop, Form form)
        {
            if (baiNop == null)
            {
                baiNop = new BaiTapNopDTO();
            }

            foreach (string key in form.Keys.ToArray())
            {
                switch (key)
                {
                    case "MaTapTin":
                        baiNop.tapTin = TapTinBUS.chuyen("BaiTapNop_TapTin", form.layInt(key)).ketQua as TapTinDTO;
                        break;
                    case "DuongDan":
                        baiNop.duongDan = form.layString(key);
                        break;
                    case "MaNguoiTao":
                        baiNop.nguoiTao = form.layDTO<NguoiDungDTO>(key);
                        break;
                    case "MaBaiVietBaiTap":
                        baiNop.baiVietBaiTap = form.layDTO<BaiVietBaiTapDTO>(key);
                        break;
                    default:
                        break;
                }
            }
        }

        public static KetQua themHoacCapNhat(Form form)
        {
            BaiTapNopDTO baiNop = new BaiTapNopDTO();

            gan(ref baiNop, form);

            KetQua ketQua = kiemTra(baiNop);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return BaiTapNopDAO.themHoacCapNhat(baiNop);
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
