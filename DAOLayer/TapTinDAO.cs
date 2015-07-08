using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class TapTinDAO : DAO<TapTinDAO, TapTinDTO>
    {
        public static TapTinDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            TapTinDTO tapTin = new TapTinDTO();

            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        tapTin.ma = layInt(dong, i); break;
                    case "Ten":
                        tapTin.ten = layString(dong, i); break;
                    case "Loai":
                        tapTin.loai = layString(dong, i); break;
                    case "Duoi":
                        tapTin.duoi = layString(dong, i); break;
                    case "ThoiDiemTao":
                        tapTin.thoiDiemTao = layDateTime(dong, i); break;
                    case "MaNguoiTao":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            tapTin.nguoiTao = LienKet.co(lienKet, "NguoiTao") ?
                                layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam, lienKet["NguoiTao"])) :
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

            return tapTin;
        }

        public static KetQua them(TapTinDTO tapTin)
        {
            return layDong
                (
                    "themTapTin",
                    new object[] 
                    { 
                        tapTin.ten,
                        tapTin.loai,
                        tapTin.duoi,
                        layMa(tapTin.nguoiTao)
                    }
                );
        }

        public static KetQua chuyen(string loai, int? ma)
        {
            return layDong
                (
                    "chuyenTapTin",
                    new object[] 
                    { 
                        loai,
                        ma
                    }
                );
        }
       
        public static KetQua layTheoMa(string loai, int? ma, LienKet lienKet = null)
        {
            return layDong
                (
                    "layTapTinTheoMa",
                    new object[] 
                    { 
                        loai,
                        ma
                    },
                    lienKet
                );
        }
    }
}
