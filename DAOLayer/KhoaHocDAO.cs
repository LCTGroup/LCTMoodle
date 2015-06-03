using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class KhoaHocDAO : DAO<KhoaHocDAO, KhoaHocViewDTO>
    {
        public static KhoaHocViewDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            KhoaHocViewDTO khoaHoc = new KhoaHocViewDTO();

            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        khoaHoc.ma = layInt(dong, i);
                        break;
                    case "Ten":
                        khoaHoc.ten = layString(dong, i);
                        break;
                    case "MoTa":
                        khoaHoc.moTa = layString(dong, i);
                        break;
                    case "MaChuDe":
                        khoaHoc.chuDe = new ChuDeViewDTO()
                        {
                            ma = layInt(dong, i)
                        };
                        break;
                    case "MaHinhDaiDien":
                        khoaHoc.hinhDaiDien = new TapTinViewDTO()
                        {
                            ma = layInt(dong, i)
                        };
                        break;
                    case "MaNguoiTao":
                        khoaHoc.nguoiTao = new NguoiDungViewDTO()
                        {
                            ma = layInt(dong, i)
                        };
                        break;
                    case "ThoiDiemTao":
                        khoaHoc.thoiDiemTao = layDateTime(dong, i);
                        break;
                    case "CanDangKy":
                        khoaHoc.canDangKy = layBool(dong, i, false);
                        break;
                    case "HanDangKy":
                        khoaHoc.hanDangKy = layDateTime(dong, i);
                        break;
                    case "PhiThamGia":
                        khoaHoc.phiThamGia = layInt(dong, i);
                        break;
                    case "CheDoRiengTu":
                        khoaHoc.cheDoRiengTu = CheDoRiengTu.lay(layString(dong, i));
                        break;
                    case "CoBangDiem":
                        khoaHoc.coBangDiem = layBool(dong, i, false);
                        break;
                    case "CoBangDiemDanh":
                        khoaHoc.coBangDiemDanh = layBool(dong, i, false);
                        break;
                    case "CanDuyetBaiViet":
                        khoaHoc.canDuyetBaiViet = layBool(dong, i, false);
                        break;
                    default:
                        break;
                }
            }

            return khoaHoc;
        }

        public static KetQua layTheoMa(int ma)
        {
            return layDong
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
            return layGiaTri<int>
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
