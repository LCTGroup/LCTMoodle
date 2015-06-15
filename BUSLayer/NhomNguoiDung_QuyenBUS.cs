﻿using System;
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
        public static KetQua themHoacXoaTheoMaNhomNguoiDungVaMaQuyen(string phamVi, int maNhomNguoiDung, int maQuyen, int maDoiTuong, bool them, bool la)
        {
            return NhomNguoiDung_QuyenDAO.themHoacXoaTheoMaNhomNguoiDungVaMaQuyen(phamVi, maNhomNguoiDung, maQuyen, maDoiTuong, them, la);
        }

        public static KetQua layTheoMaNhomNguoiDungVaMaDoiTuong(string phamVi, int maNhomNguoiDung, int maDoiTuong)
        {
            return NhomNguoiDung_QuyenDAO.layTheoMaNhomNguoiDungVaMaDoiTuong(phamVi, maNhomNguoiDung, maDoiTuong, new LienKet()
                {
                    "Quyen"
                });
        }
    }
}