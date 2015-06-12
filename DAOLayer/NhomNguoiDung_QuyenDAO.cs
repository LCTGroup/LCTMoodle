using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class NhomNguoiDung_QuyenDAO : DAO<NhomNguoiDung_QuyenDAO, NhomNguoiDung_QuyenDTO>
    {
        public static NhomNguoiDung_QuyenDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            NhomNguoiDung_QuyenDTO nhomNguoiDung_Quyen = new NhomNguoiDung_QuyenDTO();

            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "MaQuyen":
                        nhomNguoiDung_Quyen.maQuyen = layInt(dong, i);
                        break;
                    case "MaNhomNguoiDung":
                        nhomNguoiDung_Quyen.maNhomNguoiDung = layInt(dong, i);
                        break;
                    case "MaDoiTuong":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue) {
                            nhomNguoiDung_Quyen.doiTuong = new DTO()
                            {
                                ma = maTam
                            };
                        }
                        break;
                    case "PhamViQuyen":
                        nhomNguoiDung_Quyen.phamViQuyen = layString(dong, i);
                        break;
                    default:
                        break;
                }
            }

            return nhomNguoiDung_Quyen;
        }
        public static KetQua themHoacXoaTheoMaNhomNguoiDungVaMaQuyen(string phamVi, int? maNhomNguoiDung, int? maQuyen, int? maDoiTuong, bool them)
        {
            return khongTruyVan
                (
                    "themHoacXoaNhomNguoiDung_QuyenTheoMaNhomNguoiDungVaMaQuyen",
                    new object[] 
                    { 
                        phamVi,
                        maNhomNguoiDung,
                        maQuyen,
                        maDoiTuong,
                        them
                    }
                );
        }

        public static KetQua layTheoMaNhomNguoiDung(string phamVi, int? maNhomNguoiDung)
        {
            return layDanhSachDong
                (
                    "layNhomNguoiDung_QuyenTheoMaNhomNguoiDung",
                    new object[]
                    {
                        phamVi,
                        maNhomNguoiDung
                    }
                );
        }
    }
}
