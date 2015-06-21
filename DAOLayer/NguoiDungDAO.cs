using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class NguoiDungDAO : DAO<NguoiDungDAO, NguoiDungDTO>
    {
        public static NguoiDungDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            NguoiDungDTO nguoiDung = new NguoiDungDTO();

            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        nguoiDung.ma = layInt(dong, i); break;
                    case "TenTaiKhoan":
                        nguoiDung.tenTaiKhoan = layString(dong, i); break;
                    case "MatKhau":
                        nguoiDung.matKhau = layString(dong, i); break;
                    case "Email":
                        nguoiDung.email = layString(dong, i); break;
                    case "GioiTinh":
                        nguoiDung.gioiTinh = layInt(dong, i); break;
                    case "Ho":
                        nguoiDung.ho = layString(dong, i); break;
                    case "TenLot":
                        nguoiDung.tenLot = layString(dong, i); break;
                    case "Ten":
                        nguoiDung.ten = layString(dong, i); break;
                    case "NgaySinh":
                        nguoiDung.ngaySinh = layDateTime(dong, i); break;
                    case "DiaChi":
                        nguoiDung.diaChi = layString(dong, i); break;
                    case "SoDienThoai":
                        nguoiDung.soDienThoai = layString(dong, i); break;
                    case "MaHinhDaiDien":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            nguoiDung.hinhDaiDien = LienKet.co(lienKet, "HinhDaiDien") ?
                                layDTO<TapTinDTO>(TapTinDAO.layTheoMa("NguoiDung_HinhDaiDien", maTam.Value)) :
                                new TapTinDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    default:
                        break;
                }
            }

            return nguoiDung;
        }  

        public static KetQua them(NguoiDungDTO nguoiDung)
        {
            return layGiaTri<int>
            (
                "themNguoiDung",
                new object[] 
                {
                    nguoiDung.tenTaiKhoan,
                    nguoiDung.matKhau,
                    nguoiDung.email,
                    nguoiDung.ho,
                    nguoiDung.ten,
                    nguoiDung.ngaySinh,
                    nguoiDung.diaChi,
                    nguoiDung.soDienThoai,
                    layMa(nguoiDung.hinhDaiDien)
                }
            );
        }

        public static KetQua capNhat(int? ma, BangCapNhat bangCapNhat)
        {
            return khongTruyVan
                (
                    "capNhatNguoiDungTheoMa",
                    new object[]
                    {
                        ma,
                        bangCapNhat.bang
                    }
                );
        }
        
        public static KetQua layTheoTenTaiKhoan(string tenTaiKhoan)
        {
            return layDong
            (
                "layNguoiDungTheoTenTaiKhoan",
                new object[]
                {
                    tenTaiKhoan
                }
            );
        }
        
        public static KetQua layTheoMa(int? ma, LienKet lienKet = null)
        {
            return layDong
            (
                "layNguoiDungTheoMa",
                new object[]
                {
                    ma
                },
                lienKet
            );
        }

        public static KetQua lay_TimKiem(string tuKhoa, LienKet lienKet = null)
        {
            return layDanhSachDong
                (
                    "layNguoiDung_TimKiem",
                    new object[]
                    {
                        tuKhoa
                    },
                    lienKet
                );
        }

        public static KetQua layTheoMaKhoaHoc_TimKiem(int? maKhoaHoc, string tuKhoa, LienKet lienKet = null)
        {
            return layDanhSachDong
                (
                    "layNguoiDungTheoMaKhoaHoc_TimKiem",
                    new object[]
                    {
                        maKhoaHoc,
                        tuKhoa
                    },
                    lienKet
                );
        }

        public static KetQua layTheoMaNhomNguoiDung(string phamVi, int? maNhomNguoiDung, LienKet lienKet = null)
        {
            return layDanhSachDong
                (
                    "layNguoiDungTheoMaNhomNguoiDung",
                    new object[]
                    {
                        phamVi,
                        maNhomNguoiDung
                    },
                    lienKet
                );
        }
    }
}
