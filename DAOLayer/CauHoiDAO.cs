using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class CauHoiDAO : DAO<CauHoiDAO, CauHoiDTO>
    {
        public static CauHoiDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            CauHoiDTO cauHoi = new CauHoiDTO();

            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        cauHoi.ma = layInt(dong, i); break;
                    case "TieuDe":
                        cauHoi.tieuDe = layString(dong, i); break;
                    case "NoiDung":
                        cauHoi.noiDung = layString(dong, i); break;
                    case "ThoiDiemTao":
                        cauHoi.thoiDiemTao = layDateTime(dong, i); break;
                    case "MaNguoiTao":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            cauHoi.nguoiTao = LienKet.co(lienKet, "NguoiTao") ?
                                layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam.Value)) :
                                new NguoiDungDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    default:
                        break;
                }
            }

            return cauHoi;
        }  
        public static KetQua them(CauHoiDTO cauHoi)
        {
            return layGiaTri<int>
            (
                "themCauHoi",
                new object[] 
                {
                    cauHoi.tieuDe,
                    cauHoi.noiDung,
                    cauHoi.thoiDiemTao,
                    layMa(cauHoi.nguoiTao)
                }
            );
        }
        public static KetQua layTheoMa(int? maCauHoi, LienKet lienKet = null)
        {
            return layDong
            (
                "layCauHoiTheoMa",
                new object[]
                {
                    maCauHoi
                },
                lienKet
            );
        }        
        public static KetQua layDanhSachCauHoi(LienKet lienKet = null)
        {
            return layDanhSachDong
            (
                "layToanBoCauHoi",
                new object[] 
                {
                },
                lienKet
            );
        }
    }
}
