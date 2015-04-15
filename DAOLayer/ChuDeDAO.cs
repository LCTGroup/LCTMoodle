using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;

namespace DAOLayer
{
    public class ChuDeDAO : DAO
    {
        public static KetQua themChuDe(ChuDeDataDTO chuDe)
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

        public static KetQua layChuDeTheoMaChuDeChaVaPhamVi(int maChuDeCha, string phamVi)
        {
            return layDanhSachDong<ChuDeViewDTO>
            (
                "layChuDeTheoMaChuDeChaVaPhamVi",
                new object[] 
                { 
                    maChuDeCha,
                    phamVi
                }
            );
        }
    }
}
