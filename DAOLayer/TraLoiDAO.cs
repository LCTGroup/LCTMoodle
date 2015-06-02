using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class TraLoiDAO : DAO<TraLoiDAO, TraLoiViewDTO>
    {
        public static TraLoiViewDTO gan(System.Data.SqlClient.SqlDataReader dong)
        {
            TraLoiViewDTO traLoi = new TraLoiViewDTO();

            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        traLoi.ma = layInt(dong, i); break;
                    case "NoiDung":
                        traLoi.noiDung = layString(dong, i); break;
                    case "ThoiDiemTao":
                        traLoi.thoiDiemTao = layDateTime(dong, i); break;
                    case "Duyet":
                        traLoi.duyet = layBool(dong, i); break;
                    case "MaNguoiTao":
                        traLoi.NguoiTao = new NguoiDungViewDTO()
                        {
                            ma = layInt(dong, i)
                        };
                        break;
                    case "MaCauHoi":
                        traLoi.CauHoi = new CauHoiViewDTO()
                        {
                            ma = layInt(dong, i)
                        };
                        break;
                    default:
                        break;
                }
            }

            return traLoi;
        }  
        public static KetQua them(TraLoiDataDTO traLoi)
        {
            return layGiaTri<int>
            (
                "themTraLoi",
                new object[] 
                {
                    traLoi.noiDung,
                    traLoi.thoiDiemTao,
                    traLoi.duyet,
                    traLoi.maNguoiTao,
                    traLoi.maCauHoi
                }
            );
        }
    }
}
