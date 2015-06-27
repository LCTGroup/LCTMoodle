using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class TraLoi_DiemDAO : DAO<TraLoi_DiemDAO, TraLoi_DiemDTO>
    {
        public static TraLoi_DiemDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            TraLoi_DiemDTO traLoi_Diem = new TraLoi_DiemDTO();

            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "MaCauHoi":
                        maTam = layInt(dong, i);
                        
                        if (maTam.HasValue)
                        {
                            traLoi_Diem.traLoi = LienKet.co(lienKet, "TraLoi") ?
                                layDTO<TraLoiDTO>(TraLoiDAO.layTheoMa(maTam)) :
                                new TraLoiDTO() { ma = maTam };
                        }
                        break;
                    case "MaNguoiTao":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            traLoi_Diem.nguoiTao = LienKet.co(lienKet, "NguoiTao") ?
                                layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam)) :
                                new NguoiDungDTO() { ma = maTam };
                        }
                        break;
                    case "Diem":
                        traLoi_Diem.diem = layBool(dong, i); 
                        break;
                    default:
                        break;
                }                
            }
            return traLoi_Diem;
        }  
        
        public static KetQua them(int? maCauHoi, int? maNguoiTao, bool diem)
        {
            return khongTruyVan
            (
                "themtraLoi_Diem",
                new object[] 
                {
                    maCauHoi,
                    maNguoiTao,
                    diem
                }
            );
        }

        public static KetQua xoaTheoMa(int? maCauHoi, int? maNguoiTao)
        {
            return khongTruyVan
            (
                "xoaTraLoi_DiemTheoMaTraLoiVaMaNguoiTao",
                new object[] 
                {
                    maCauHoi,
                    maNguoiTao
                }
            );
        }        
    }
}
