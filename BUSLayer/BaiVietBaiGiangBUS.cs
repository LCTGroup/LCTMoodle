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
    public class BaiVietBaiGiangBUS : BUS
    {
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
                maNguoiTao = 1, //Tạm
                maKhoaHoc = layInt(form, "KhoaHoc")
            };
            
            ketQua = baiVietBaiGiang.kiemTra();

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return BaiVietBaiGiangDAO.them(baiVietBaiGiang);
        }
    }
}
