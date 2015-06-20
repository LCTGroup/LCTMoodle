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
    public class NhomNguoiDung_NguoiDungBUS : BUS
    {
        public static KetQua layNguoiDung_TimKiem(string tuKhoa, string phamVi, int maNhomNguoiDung, int maDoiTuong = 0)
        {
            KetQua ketQua;
            if (phamVi == "KH")
            {
                ketQua = NguoiDungDAO.layTheoMaKhoaHoc_TimKiem(maDoiTuong, tuKhoa);
            }
            else
            {
                ketQua = NguoiDungDAO.lay_TimKiem(tuKhoa);
            }

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            List<NguoiDungDTO> danhSachNguoiDung = ketQua.ketQua as List<NguoiDungDTO>;

            List<NguoiDungDTO> danhSachTrongNhom = new List<NguoiDungDTO>();
            List<NguoiDungDTO> danhSachNgoaiNhom = new List<NguoiDungDTO>();

            foreach(var nguoiDung in danhSachNguoiDung)
            {
                ketQua = NhomNguoiDung_NguoiDungDAO.layTheoMaNhomNguoiDungVaMaNguoiDung(phamVi, maNhomNguoiDung, nguoiDung.ma);

                if (ketQua.trangThai == 0)
                {
                    danhSachTrongNhom.Add(nguoiDung);
                }
                else
                {
                    danhSachNgoaiNhom.Add(nguoiDung);
                }
            }

            return new KetQua()
            {
                trangThai = 0,
                ketQua = new List<NguoiDungDTO>[]
                {
                    danhSachTrongNhom,
                    danhSachNgoaiNhom
                }
            };
        }

        public static KetQua them(string phamVi, int maNhomNguoiDung, int maNguoiDung)
        {
            return NhomNguoiDung_NguoiDungDAO.them(phamVi, maNhomNguoiDung, maNguoiDung);
        }

        public static KetQua xoaTheoMaNhomNguoiDungVaMaNguoiDung(string phamVi, int maNhomNguoiDung, int maNguoiDung)
        {
            return NhomNguoiDung_NguoiDungDAO.xoaTheoMaNhomNguoiDungVaMaNguoiDung(phamVi, maNhomNguoiDung, maNguoiDung);
        }
    }
}
