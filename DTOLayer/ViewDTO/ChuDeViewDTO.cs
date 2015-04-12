using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class ChuDeViewDTO : DTO
    {
        public int ma;
        public string ten;
        public string moTa;
        public int maNguoiTao;
        public DateTime thoiDiemTao;
        public string phamVi;
        public int maChuDeCha;
        public TapTinViewDTO hinhDaiDien;

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
                    case "MoTa":
                        moTa = layString(dong, i); break;
                    case "ThoiDiemTao":
                        thoiDiemTao = layDateTime(dong, i, DateTime.MinValue); break;
                    case "MaNguoiTao":
                        maNguoiTao = layInt(dong, i); break;
                    case "PhamVi":
                        phamVi = layString(dong, i); break;
                    case "MaChuDeCha":
                        maChuDeCha = layInt(dong, i); break;
                    case "MaHinhDaiDien":
                        hinhDaiDien = new TapTinViewDTO()
                        {
                            ma = layInt(dong, i)
                        };
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
