using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class BaiTapNopViewDTO : DTO
    {
        public TapTinViewDTO tapTin;
        public string duongDan;
        public DateTime? thoiDiemTao;
        public NguoiDungViewDTO nguoiTao;
        public BaiVietBaiTapViewDTO baiVietBaiTap;

        public override void gan(System.Data.SqlClient.SqlDataReader dong)
        {
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        ma = layInt(dong, i);
                        break;
                    case "MaTapTin":
                        tapTin = new TapTinViewDTO()
                        {
                            ma = layInt(dong, i)
                        };
                        break;
                    case "DuongDan":
                        duongDan = layString(dong, i);
                        break;
                    case "ThoiDiemTao":
                        thoiDiemTao = layDateTime(dong, i);
                        break;
                    case "MaNguoiTao":
                        nguoiTao = new NguoiDungViewDTO()
                        {
                            ma = layInt(dong, i)
                        };
						break;
                    case "MaBaiVietBaiTap":
						baiVietBaiTap = new BaiVietBaiTapViewDTO()
						{
							ma = layInt(dong, i)
						};
						break;
                    default:
                        break;
                }
            }
        }
    }
}
