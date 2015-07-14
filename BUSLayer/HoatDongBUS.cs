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
        public static KetQua kiemTra(HoatDongDTO hoatDong)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (!hoatDong.maNguoiTacDong.HasValue)
            {
                loi.Add("Người tác động không thể bỏ trống");
            }
            if (string.IsNullOrWhiteSpace(hoatDong.loaiDoiTuongTacDong))
            {
                loi.Add("Loại đối tượng tác động không thể bỏ trống");
            }
            if (!hoatDong.maDoiTuongTacDong.HasValue)
            {
                loi.Add("Đối tượng tác động không thể bỏ trống");
            }
            if (string.IsNullOrWhiteSpace(hoatDong.loaiDoiTuongBiTacDong))
            {
                loi.Add("Loại đối tượng bị tác động không thể bỏ trống");
            }
            if (!hoatDong.maDoiTuongBiTacDong.HasValue)
            {
                loi.Add("Đối tượng bị tác động không thể bỏ trống");
            }
            if (hoatDong.hanhDong == null)
            {
                loi.Add("Hành động không thể bỏ trống");
            }
            #endregion

            if (loi.Count > 0)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = loi
                };
            }
            else
            {
                return new KetQua()
                {
                    trangThai = 0
                };
            }
        }

        public static KetQua them(HoatDongDTO hoatDong)
        {
            if (string.IsNullOrWhiteSpace(hoatDong.loaiDoiTuongTacDong) || !hoatDong.maDoiTuongTacDong.HasValue)
            {
                hoatDong.loaiDoiTuongTacDong = "ND";
                hoatDong.maDoiTuongTacDong = hoatDong.maNguoiTacDong;
            }

            var ketQua = kiemTra(hoatDong);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return HoatDongDAO.them(hoatDong);
        }

        public static KetQua them(HoatDongDTO hoatDong, GiaTriHoatDongDTO giaTriHoatDong)
        {
            if (string.IsNullOrWhiteSpace(hoatDong.loaiDoiTuongTacDong) || !hoatDong.maDoiTuongTacDong.HasValue)
            {
                hoatDong.loaiDoiTuongTacDong = "ND";
                hoatDong.maDoiTuongTacDong = hoatDong.maNguoiTacDong;
            }

            var ketQua = kiemTra(hoatDong);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            ketQua = HoatDongDAO.them(hoatDong);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return GiaTriHoatDongDAO.them(giaTriHoatDong);
        }

        public static KetQua them(HoatDongDTO hoatDong, List<GiaTriHoatDongDTO> dsGiaTriHoatDong)
        {
            if (string.IsNullOrWhiteSpace(hoatDong.loaiDoiTuongTacDong) || !hoatDong.maDoiTuongTacDong.HasValue)
            {
                hoatDong.loaiDoiTuongTacDong = "ND";
                hoatDong.maDoiTuongTacDong = hoatDong.maNguoiTacDong;
            }

            var ketQua = kiemTra(hoatDong);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            ketQua = HoatDongDAO.them(hoatDong);
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

        public static KetQua lay_CuaDoiTuong(string loaiDoiTuong, int maDoiTuong, int? trang = null, int? soLuongMoiTrang = null, LienKet lienKet = null)
        {
            return HoatDongDAO.lay_CuaDoiTuong(loaiDoiTuong, maDoiTuong, trang, soLuongMoiTrang, lienKet);
        }

        public static KetQua lay_CuaDanhSachDoiTuong(string loaiDoiTuong, string dsMaDoiTuong, int? trang = null, int? soLuongMoiTrang = null, LienKet lienKet = null)
        {
            return HoatDongDAO.lay_CuaDanhSachDoiTuong(loaiDoiTuong, dsMaDoiTuong, trang, soLuongMoiTrang, lienKet);
        }
    }
}