using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class BaiVietDienDanViewDTO : DTO
    {
        public int ma;
        public string tieuDe;
        public string noiDung;
        public TapTinViewDTO tapTin;
        public DateTime? thoiDiemTao;
        public NguoiDungViewDTO nguoiTao;
        public KhoaHocViewDTO khoaHoc;

        public override void gan(System.Data.SqlClient.SqlDataReader dong)
        {
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        ma = layInt(dong, i);
                        break;
                    case "TieuDe":
                        tieuDe = layString(dong, i);
                        break;
                    case "NoiDung":
                        noiDung = layString(dong, i);
                        break;
                    case "MaTapTin":
                        tapTin = new TapTinViewDTO()
                        {
                            ma = layInt(dong, i)
                        };
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
                    case "MaKhoaHoc":
						khoaHoc = new KhoaHocViewDTO()
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
