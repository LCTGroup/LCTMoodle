using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class TapTinViewDTO : DTO
    {
        public string ten;
        public string loai;
        public string thuMuc;
        public DateTime? thoiDiemTao;

        public override void gan(System.Data.SqlClient.SqlDataReader dong)
        {
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        ma = (dong.IsDBNull(i)) ? 0 : dong.GetInt32(i); break;
                    case "Ten":
                        ten = layString(dong, i); break;
                    case "Loai":
                        loai = layString(dong, i); break;
                    case "ThuMuc":
                        thuMuc = layString(dong, i); break;
                    case "ThoiDiemTao":
                        thoiDiemTao = layDateTime(dong, i); break;
                    default:
                        break;
                }
            }
        }
    }
}