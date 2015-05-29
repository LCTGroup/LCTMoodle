using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class BinhLuanViewDTO : DTO
    {
        public string noiDung;
        public NguoiDungViewDTO nguoiTao;
        public DTO doiTuong;
        public TapTinViewDTO tapTin;
        public DateTime? thoiDiemTao;
        public string loaiDoiTuong;

        public override void gan(System.Data.SqlClient.SqlDataReader dong)
        {
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        ma = layInt(dong, i);
                        break;
                    case "NoiDung":
                        noiDung = layString(dong, i);
                        break;
                    case "MaNguoiTao":
                        nguoiTao = new NguoiDungViewDTO() {
                            ma = layInt(dong, i)
                        };
                        break;
                    case "MaDoiTuong":
                        doiTuong = new DTO() {
                            ma = layInt(dong, i)
                        };
                        break;
                    case "MaTapTin":
                        tapTin = new TapTinViewDTO() {
                            ma = layInt(dong, i)
                        };
                        break;
                    case "ThoiDiemTao":
                        thoiDiemTao = layDateTime(dong, i);
                        break;
                    case "LoaiDoiTuong":
                        loaiDoiTuong = layString(dong, i);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
