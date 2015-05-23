using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOLayer;
using DTOLayer;
using System.IO;

namespace BUSLayer
{
    public class BaiVietBaiTapBUS : BUS
    {
        public static KetQua them(Dictionary<string, string> form)
        {
            KetQua ketQua = TapTinBUS.chuyen(layInt(form, "TapTin"), "BaiVietBaiTap_TapTin");

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            BaiVietBaiTapDataDTO baiVietBaiTap = new BaiVietBaiTapDataDTO()
            {
                tieuDe = layString(form, "TieuDe"),
                noiDung = layString(form, "NoiDung"),
                maTapTin = (ketQua.ketQua as TapTinViewDTO).ma,
                thoiDiemHetHan = layDateTime_Full(form, "ThoiDiemHetHan_Ngay", "ThoiDiemHetHan_Gio"),
                maNguoiTao = 1, //Tạm
                maKhoaHoc = layInt(form, "KhoaHoc")
            };
            
            ketQua = baiVietBaiTap.kiemTra();

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return BaiVietBaiTapDAO.them(baiVietBaiTap);
        }
    }
}
