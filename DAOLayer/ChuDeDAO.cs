using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class ChuDeDAO : DAO
    {
        public static KetQua them(ChuDeDataDTO chuDe)
        {
            return layDong<ChuDeViewDTO>
            (
                "themChuDe",
                new object[] 
                {
                    chuDe.ten,
                    chuDe.moTa,
                    chuDe.maNguoiTao,
                    chuDe.phamVi,
                    chuDe.maChuDeCha,
                    chuDe.maHinhDaiDien
                }
            );
        }

        public static KetQua layTheoMaChuDeCha(string phamVi, int maChuDeCha)
        {
            return layDanhSachDong<ChuDeViewDTO>
            (
                "layChuDeTheoMaChuDeCha",
                new object[] 
                { 
                    phamVi,
                    maChuDeCha
                }
            );
        }
        
        public static KetQua layTheoMa(string phamVi, int ma)
        {
            return layDong<ChuDeViewDTO>
            (
                "layChuDeTheoMa",
                new object[] 
                { 
                    phamVi,
                    ma
                }
            );
        }

        public static KetQua xoaTheoMa(int ma)
        {
            return khongTruyVan
            (
                "xoaChuDeTheoMa",
                new object[] 
                { 
                    ma
                }
            );
        }
    }
}
