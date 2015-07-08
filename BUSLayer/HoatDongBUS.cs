using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DAOLayer;
using DTOLayer;
using System.IO;
using Data;

namespace BUSLayer
{
    public class HoatDongBUS : BUS
    {
        public static KetQua them(HoatDongDTO hoatDong)
        {
            return HoatDongDAO.them(hoatDong);
        }

        public static KetQua them(HoatDongDTO hoatDong, GiaTriHoatDongDTO giaTriHoatDong)
        {
            var ketQua = HoatDongDAO.them(hoatDong);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return GiaTriHoatDongDAO.them(giaTriHoatDong);
        }

        public static KetQua them(HoatDongDTO hoatDong, List<GiaTriHoatDongDTO> dsGiaTriHoatDong)
        {
            var ketQua = HoatDongDAO.them(hoatDong);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            foreach(var giaTriHoatDong in dsGiaTriHoatDong)
            {
                ketQua = GiaTriHoatDongDAO.them(giaTriHoatDong);
                if (ketQua.trangThai != 0)
                {
                    return ketQua;
                }
            }
            
            return ketQua;
        }

        public static KetQua layTheoMa(int? maHoatDong, LienKet lienKet = null)
        {
            return HoatDongDAO.layTheoMa(maHoatDong, lienKet);
        }
    }
}