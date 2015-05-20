using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class ChuDeViewDTO : DTO
    {
        public string ten;
        public string moTa;
        public int maNguoiTao;
        public DateTime? thoiDiemTao;
        public string phamVi;
        public ChuDeViewDTO chuDeCha;
        public TapTinViewDTO hinhDaiDien;

        public override void gan(System.Data.SqlClient.SqlDataReader dong)
        {
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        ma = layInt(dong, i); break;
                    case "Ten":
                        ten = layString(dong, i); break;
                    case "MoTa":
                        moTa = layString(dong, i); break;
                    case "ThoiDiemTao":
                        thoiDiemTao = layDateTime(dong, i); break;
                    case "MaNguoiTao":
                        maNguoiTao = layInt(dong, i); break;
                    case "PhamVi":
                        phamVi = layString(dong, i); break;
                    case "MaChuDeCha":
                        chuDeCha = new ChuDeViewDTO()
                        {
                            ma = layInt(dong, i)
                        };
                        break;
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
