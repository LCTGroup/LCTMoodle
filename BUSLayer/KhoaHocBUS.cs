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
    public class KhoaHocBUS : BUS
    {
        public static KetQua them(Dictionary<string, string> form)
        {
            KhoaHocDataDTO khoaHoc  = new KhoaHocDataDTO()
            {
                ten = layString(form, "Ten"),
                moTa = layString(form, "MoTa"),
                maChuDe = layInt(form, "ChuDe"),
                maNguoiTao = 1, //Tạm
                maHinhDaiDien = layInt(form, "HinhDaiDien"),
                canDangKy = layBool(form, "CanDangKy"),
                phiThamGia = layInt(form, "PhiThamGia"),
                cheDoRiengTu = layString(form, "CheDoRiengTu")
            };

            if (layBool(form, "CoHan"))
            {
                khoaHoc.han = layDateTime_Full(form, "Han_Ngay", "Han_Gio");
            }

            if (khoaHoc.canDangKy && layBool(form, "CoHanDangKy"))
            {
                khoaHoc.hanDangKy = layDateTime_Full(form, "HanDangKy_Ngay", "HanDangKy_Gio");
            }

            KetQua ketQua = khoaHoc.kiemTra();

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return KhoaHocDAO.them(khoaHoc);
        }
    }
}
