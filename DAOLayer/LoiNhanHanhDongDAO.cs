using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class LoiNhanHanhDongDAO : DAO<LoiNhanHanhDongDAO, LoiNhanHanhDongDTO>
    {
        public static LoiNhanHanhDongDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            LoiNhanHanhDongDTO loiNhanHanhDong = new LoiNhanHanhDongDTO();

            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "MaHanhDong":
                        loiNhanHanhDong.maHanhDong = layInt(dong, i); break;
                    case "ChuDong":
                        loiNhanHanhDong.chuDong = layString(dong, i); break;
                    case "BiDong":
                        loiNhanHanhDong.biDong = layString(dong, i); break;
                    case "ChuDongNgoai":
                        loiNhanHanhDong.chuDongNgoai = layString(dong, i); break;
                    case "BiDongNgoai":
                        loiNhanHanhDong.biDongngoai = layString(dong, i); break;
                    default:
                        break;
                }
            }
            return loiNhanHanhDong;
        }

    }
}
