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
    public class BinhLuanBUS : BUS
    {
        public static KetQua them(Dictionary<string, string> form)
        {
            var binhLuan = new BinhLuanDataDTO()
            {
                noiDung = layString(form, "NoiDung"),
                maTapTin = 0,
                maNguoiTao = 1,
                maDoiTuong = layInt(form, "DoiTuong"),
                loaiDoiTuong = layString(form, "Loai")
            };

            KetQua ketQua = binhLuan.kiemTra();

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return BinhLuanDAO.them(binhLuan);
        }
    }
}
