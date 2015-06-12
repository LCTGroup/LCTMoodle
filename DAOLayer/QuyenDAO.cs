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
                    case "PhamVi":
                        quyen.phamVi = layString(dong, i);
                        break;
                    default:
                        break;
                }
            }

            return quyen;
        }

        public static KetQua layTheoPhamVi(string phamVi)
        {
            return layDanhSachDong
                (
                    "layQuyenTheoPhamVi",
                    new object[]
                    {
                        phamVi
                    }
                );
        }
    }
}
