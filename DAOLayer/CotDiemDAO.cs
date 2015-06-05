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
    public class CotDiemDAO : DAO<CotDiemDAO, CotDiemViewDTO>
    {
        public static CotDiemViewDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            CotDiemViewDTO cotDiem = new CotDiemViewDTO();

            int maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        cotDiem.ma = layInt(dong, i);
                        break;
                    case "MaKhoaHoc":
                        maTam = layInt(dong, i);

                        if (maTam != 0)
                        {
                            if (LienKet.co(lienKet, "KhoaHoc"))
                            {
                                KetQua ketQua = KhoaHocDAO.layTheoMa(maTam);

                                if (ketQua.trangThai == 0)
                                {
                                    cotDiem.khoaHoc = ketQua.ketQua as KhoaHocViewDTO;
                                }
                            }
                            else
                            {
                                cotDiem.khoaHoc = new KhoaHocViewDTO()
                                {
                                    ma = maTam
                                };
                            }
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
