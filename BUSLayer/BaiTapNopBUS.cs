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
        public static KetQua them(Dictionary<string, string> form)
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
                maNguoiTao = 1,
                maBaiVietBaiTap = layInt(form, "BaiVietBaiTap")
            };

            ketQua = baiTapNop.kiemTra();

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return BaiTapNopDAO.them(baiTapNop);
        }
    }
}
