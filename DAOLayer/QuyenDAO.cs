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
    public class QuyenDAO : DAO<QuyenDAO, QuyenDTO>
    {
        public static QuyenDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            QuyenDTO quyen = new QuyenDTO();

            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        quyen.ma = layInt(dong, i);
                        break;
                    case "Ten":
                        quyen.ten = layString(dong, i);
                        break;
                    case "MoTa":
                        quyen.moTa = layString(dong, i);
                        break;
                    case "GiaTri":
                        quyen.giaTri = layString(dong, i);
                        break;
                    case "PhamVi":
                        quyen.phamVi = layString(dong, i);
                        break;
                    case "MaCha":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            quyen.cha = layDTO<QuyenDTO>(QuyenDAO.layTheoMa(maTam));
                        }
                        break;
                    default:
                        break;
                }
            }

            return quyen;
        }

        public static KetQua layTheoPhamViVaMaCha(string phamVi, int? maCha)
        {
            return layDanhSachDong
                (
                    "layQuyenTheoPhamViVaMaCha",
                    new object[]
                    {
                        phamVi,
                        maCha
                    }
                );
        }

        public static KetQua layTheoMa(int? ma)
        {
            return layDong
                (
                    "layQuyenTheoMa",
                    new object[]
                    {
                        ma
                    }
                );
        }

        public static KetQua layTheoMaDoiTuongVaMaNguoiDung_ChuoiGiaTri(string phamVi, int? maDoiTuong, int? maNguoiDung) 
        {
            return layGiaTri<string>
                (
                    "layQuyenTheoMaDoiTuongVaMaNguoiDung_ChuoiGiaTri",
                    new object[]
                    {
                        phamVi,
                        maDoiTuong,
                        maNguoiDung
                    }
                );
        }
    }
}
