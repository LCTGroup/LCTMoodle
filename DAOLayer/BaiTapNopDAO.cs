using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class BaiTapNopDAO : DAO<BaiTapNopDAO, BaiTapNopViewDTO>
    {
        public static BaiTapNopViewDTO gan(System.Data.SqlClient.SqlDataReader dong)
        {
            BaiTapNopViewDTO baiTapNop = new BaiTapNopViewDTO();

            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        baiTapNop.ma = layInt(dong, i);
                        break;
                    case "MaTapTin":
                        baiTapNop.tapTin = new TapTinViewDTO()
                        {
                            ma = layInt(dong, i)
                        };
                        break;
                    case "DuongDan":
                        baiTapNop.duongDan = layString(dong, i);
                        break;
                    case "ThoiDiemTao":
                        baiTapNop.thoiDiemTao = layDateTime(dong, i);
                        break;
                    case "MaNguoiTao":
                        baiTapNop.nguoiTao = new NguoiDungViewDTO()
                        {
                            ma = layInt(dong, i)
                        };
                        break;
                    case "MaBaiVietBaiTap":
                        baiTapNop.baiVietBaiTap = new BaiVietBaiTapViewDTO()
                        {
                            ma = layInt(dong, i)
                        };
                        break;
                    default:
                        break;
                }
            }

            return baiTapNop;
        }

        public static KetQua them(BaiTapNopDataDTO baiTapNop)
        {
            return layDong
            (
                "themBaiTapNop",
                new object[] 
                { 
                    baiTapNop.maTapTin,
                    baiTapNop.duongDan,
                    baiTapNop.maNguoiTao,
                    baiTapNop.maBaiVietBaiTap
                }
            );
        }
    }
}
