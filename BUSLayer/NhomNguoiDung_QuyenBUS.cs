using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOLayer;
using DTOLayer;
using System.IO;
using System.Web;
using Data;

namespace BUSLayer
{
    public class NhomNguoiDung_QuyenBUS : BUS
    {
        public static KetQua themHoacXoaTheoMaNhomNguoiDungVaMaQuyen(string phamVi, int maNhomNguoiDung, int maQuyen, int maDoiTuong, bool la, bool them)
        {
            return them ? 
                NhomNguoiDung_QuyenDAO.themTheoMaNhomNguoiDungVaMaQuyen(phamVi, maNhomNguoiDung, maQuyen, maDoiTuong, la) :
                NhomNguoiDung_QuyenDAO.xoaTheoMaNhomNguoiDungVaMaQuyen(phamVi, maNhomNguoiDung, maQuyen, maDoiTuong, la);
        }

        public static KetQua layTheoMaNhomNguoiDung(string phamVi, int maNhomNguoiDung)
        {
            return NhomNguoiDung_QuyenDAO.layTheoMaNhomNguoiDung(phamVi, maNhomNguoiDung, new LienKet()
                {
                    "Quyen"
                });
        }
    }
}
