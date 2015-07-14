using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class HoatDongDAO : DAO<HoatDongDAO, HoatDongDTO>
    {
        public static HoatDongDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            HoatDongDTO hoatDong = new HoatDongDTO();

            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        hoatDong.ma = layInt(dong, i); 
                        break;
                    case "MaNguoiTacDong":
                        hoatDong.maNguoiTacDong = layInt(dong, i);
                        break;
                    case "LoaiDoiTuongTacDong":
                        hoatDong.loaiDoiTuongTacDong = layString(dong, i); 
                        break;
                    case "MaDoiTuongTacDong":
                        hoatDong.maDoiTuongTacDong = layInt(dong, i); 
                        break;
                    case "LoaiDoiTuongBiTacDong":
                        hoatDong.loaiDoiTuongBiTacDong = layString(dong, i); 
                        break;
                    case "MaDoiTuongBiTacDong":
                        hoatDong.maDoiTuongBiTacDong = layInt(dong, i); 
                        break;
                    case "MaHanhDong":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            hoatDong.hanhDong = LienKet.co(lienKet, "HanhDong") ?
                                layDTO<HanhDongDTO>(HanhDongDAO.layTheoMa(maTam, lienKet["HanhDong"])) :
                                new HanhDongDTO()
                                {
                                    ma = maTam
                                }; 
                        }
                        break;
                    case "DuongDan":
                        hoatDong.duongDan = layString(dong, i);
                        break;
                    case "ThoiDiem":
                        hoatDong.thoiDiem = layDateTime(dong, i); 
                        break;                        
                    default:
                        break;
                }                
            }

            if (LienKet.co(lienKet, "GiaTriHoatDong"))
            {
                var ketQua = layDanhSachDTO<GiaTriHoatDongDTO>(GiaTriHoatDongDAO.layTheoMaHoatDong(hoatDong.ma, lienKet["GiaTriHoatDong"]));
            }

            return hoatDong;
        }

        public static KetQua them(HoatDongDTO hoatDong)
        {
            return khongTruyVan
                (
                    "themHoatDong",
                    new object[]
                    {
                        hoatDong.maNguoiTacDong,
                        hoatDong.loaiDoiTuongTacDong,
                        hoatDong.maDoiTuongTacDong,
                        hoatDong.loaiDoiTuongBiTacDong,
                        hoatDong.maDoiTuongBiTacDong,
                        layMa(hoatDong.hanhDong),
                        hoatDong.duongDan
                    }
                );
        }

        public static KetQua layTheoMa(int? ma, LienKet lienKet = null)
        {
            return layDong
                (
                    "layHoatDongTheoMa",
                    new object[]
                    {
                        ma
                    },
                    lienKet
                );
        }

        public static KetQua lay_CuaDoiTuong(string loaiDoiTuong, int? maDoiTuong, int? trang = null, int? soLuongMoiTrang = null, LienKet lienKet = null)
        {
            return layDanhSachDong
                (
                    "layHoatDong_CuaDoiTuong",
                    new object[]
                    {
                        loaiDoiTuong, 
                        maDoiTuong,
                        trang,
                        soLuongMoiTrang
                    },
                    lienKet
                );
        }

        public static KetQua lay_CuaDanhSachDoiTuong(string loaiDoiTuong, string dsMaDoiTuong, int? trang = null, int? soLuongMoiTrang = null, LienKet lienKet = null)
        {
            return layDanhSachDong
                (
                    "layHoatDong_CuaDanhSachDoiTuong",
                    new object[]
                    {
                        loaiDoiTuong, 
                        dsMaDoiTuong,
                        trang,
                        soLuongMoiTrang
                    },
                    lienKet
                );
        }
    }
}
