using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class TraLoiDAO : DAO<TraLoiDAO, TraLoiDTO>
    {
        public static TraLoiDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            TraLoiDTO traLoi = new TraLoiDTO();

            int? maTam;
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
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            traLoi.nguoiTao = LienKet.co(lienKet, "NguoiTao") ?
                                layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam.Value)) :
                                new NguoiDungDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "MaCauHoi":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            traLoi.cauHoi = LienKet.co(lienKet, "CauHoi") ?
                                layDTO<CauHoiDTO>(CauHoiDAO.layTheoMa(maTam.Value)) :
                                new CauHoiDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    default:
                        break;
                }
            }

            return traLoi;
        }  
        public static KetQua them(TraLoiDTO traLoi)
        {
            return layGiaTri<int>
            (
                "themTraLoi",
                new object[] 
                {
                    traLoi.noiDung,
                    traLoi.thoiDiemTao,
                    traLoi.duyet,
                    layMa(traLoi.nguoiTao),
                    layMa(traLoi.cauHoi)
                }
            );
        }
        public static KetQua layDanhSachTraLoiTheoCauHoi (int? maCauHoi, LienKet lienKet = null)
        {
            return layDanhSachDong
            (
                "layDanhSachTraLoiTheoCauHoi",
                new object[] 
                {
                    maCauHoi
                },
                lienKet
            );
        }
        public static KetQua layTraLoiTheoMa(int? ma, LienKet lienKet = null)
        {
            return layDong
            (
                "layTraLoiTheoMa",
                new object[]
                {
                    ma
                },
                lienKet
            );
        }
    }
}
