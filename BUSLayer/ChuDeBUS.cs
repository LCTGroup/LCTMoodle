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
    public class ChuDeBUS : BUS
    {
        public static KetQua them(Dictionary<string, string> form)
        {
            KetQua ketQua = TapTinBUS.chuyen(layInt(form, "HinhDaiDien"), "ChuDe_HinhDaiDien");

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            ChuDeDataDTO chuDe = new ChuDeDataDTO()
            {
                ten = layString(form, "Ten"),
                moTa = layString(form, "MoTa"),
                maHinhDaiDien = (ketQua.ketQua as TapTinViewDTO).ma,
                maChuDeCha = layInt(form, "ChuDeCha"),
                phamVi = layString(form, "PhamVi"),
                maNguoiTao = 1 //Để tạm
            };

            ketQua = chuDe.kiemTra();

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return ChuDeDAO.them(chuDe);
        }
    }
}
