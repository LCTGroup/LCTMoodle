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
                                layDTO<TraLoiDTO>(TraLoiDAO.layTheoMa(maTam, lienKet["TraLoi"])) :
                                new TraLoiDTO() { ma = maTam };
                        }
                        break;
                    case "MaNguoiTao":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            traLoi_Diem.nguoiTao = LienKet.co(lienKet, "NguoiTao") ?
                                layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam, lienKet["NguoiTao"])) :
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
        
        public static KetQua them(int? maTraLoi, int? maNguoiTao, bool diem)
        {
            return khongTruyVan
            (
                "themTraLoi_Diem",
                new object[] 
                {
                    maTraLoi,
                    maNguoiTao,
                    diem
                }
            );
        }

        public static KetQua xoaTheoMaTraLoiVaMaNguoiTao(int? maTraLoi, int? maNguoiTao)
        {
            return khongTruyVan
            (
                "xoaTraLoi_DiemTheoMaTraLoiVaMaNguoiTao",
                new object[] 
                {
                    maTraLoi,
                    maNguoiTao
                }
            );
        }

        public static KetQua layTheoMaTraLoiVaMaNguoiTao_Diem(int? maTraLoi, int? maNguoiTao)
        {
            return layGiaTri<bool>
                (
                    "layTraLoi_DiemTheomMaTraLoiVaMaNguoiTao_Diem",
                    new object[]
                    {
                        maTraLoi,
                        maNguoiTao
                    }
                );
        }

        public static KetQua layTheoMaTraLoi_Diem(int? maTraLoi)
        {
            return layGiaTri<int>
                (
                    "layTraLoi_DiemTheoMaTraLoi_Diem",
                    new object[]
                    {
                        maTraLoi
                    }
                );
        }
    }
}
