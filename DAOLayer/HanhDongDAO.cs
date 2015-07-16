using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class HanhDongDAO : DAO<HanhDongDAO, HanhDongDTO>
    {
        public static HanhDongDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            HanhDongDTO hanhDong = new HanhDongDTO();

            //int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        hanhDong.ma = layInt(dong, i); 
                        break;
                    case "LoiNhan":
                        hanhDong.loiNhan = layString(dong, i); 
                        break;
                    default:
                        break;
                }
            }
            return hanhDong;
        }

        public static KetQua layTheoMa(int? ma, LienKet lienKet = null)
        {
            return layDong
                (
                    "layHanhDongTheoMa",
                    new object[]
                    {
                        ma
                    },
                    lienKet
                );
        }
    }
}
