using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;

namespace DAOLayer
{
    public class KhoaHocDAO : DAO
    {
        public static KetQua layTheoMa(int ma)
        {
            return layDong<KhoaHocViewDTO>
                (
                    "layKhoaHocTheoMa",
                    new object[]
                    {
                        ma
                    }
                );
        }

        public static KetQua them(KhoaHocDataDTO khoaHoc)
        {
            return layDong<KhoaHocViewDTO>
                (
                    "themKhoaHoc",
                    new object[] 
                    {
                        khoaHoc.ten,
                        khoaHoc.moTa,
                        khoaHoc.maHinhDaiDien,
                        khoaHoc.maChuDe,
                        khoaHoc.maNguoiTao,
                        khoaHoc.han,
                        khoaHoc.canDangKy,
                        khoaHoc.hanDangKy,
                        khoaHoc.phiThamGia,
                        khoaHoc.cheDoRiengTu
                    }
                );
        }
    }
}
