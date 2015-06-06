using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;
using System.Reflection;

namespace DAOLayer
{
    public class CotDiemDAO : DAO<CotDiemDAO, CotDiemDTO>
    {
        public static CotDiemDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            CotDiemDTO cotDiem = new CotDiemDTO();

            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        cotDiem.ma = layInt(dong, i);
                        break;
                    case "MaKhoaHoc":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            cotDiem.khoaHoc = LienKet.co(lienKet, "KhoaHoc") ?
                                layDTO<KhoaHocDTO>(KhoaHocDAO.layTheoMa(maTam.Value)) :
                                new KhoaHocDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "Ten":
                        cotDiem.ten = layString(dong, i);
                        break;
                    case "MoTa":
                        cotDiem.moTa = layString(dong, i);
                        break;
                    case "HeSo":
                        cotDiem.heSo = layInt(dong, i);
                        break;
                    case "ThoiDiem":
                        cotDiem.thoiDiem = layDateTime(dong, i);
                        break;
                    default:
                        break;
                }
            }

            return cotDiem;
        }
    }
}
