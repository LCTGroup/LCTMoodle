using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class HoiDapDAO : DAO
    {       
        public static KetQua them(CauHoiDataDTO cauHoi)
        {
            return layGiaTri<int>
            (
                "themCauHoi",
                new object[] 
                {
                    cauHoi.tieuDe,
                    cauHoi.noiDung,
                    cauHoi.thoiDiemTao,
                    cauHoi.maNguoiTao
                }
            );
        }
        public static KetQua lay(int maCauHoi)
        {
            return layDong<CauHoiViewDTO>
            (
                "layCauHoiTheoMa",
                new object[]
                {
                    maCauHoi
                }
            );
        }        
    }
}
