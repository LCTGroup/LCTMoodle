using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOLayer
{
    public class KhoaHocViewDTO : DTO
    {
        public int ma;
        public string ten;
        public string moTa;
        public ChuDeViewDTO chuDe;
        public TapTinViewDTO hinhDaiDien;
        public NguoiDungViewDTO nguoiTao;
        public DateTime thoiDiemTao;
        public bool canDangKy;
        public DateTime hanDangKy;
        public int phiThamGia;
        public CheDoRiengTu cheDoRiengTu;
        public bool coBangDiem;
        public bool coBangDiemDanh;
        public bool canDuyetBaiViet;

        public override void gan(System.Data.SqlClient.SqlDataReader dong)
        {
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        ma = layInt(dong, i);
                        break;
                    case "Ten":
                        ten = layString(dong, i);
                        break;
                    case "MoTa":
                        moTa = layString(dong, i);
                        break;
                    case "MaChuDe":
                        chuDe = new ChuDeViewDTO()
                            {
                                ma = layInt(dong, i)
                            };
                        break;
                    case "MaHinhDaiDien":
                        hinhDaiDien = new TapTinViewDTO()
                            {
                                ma = layInt(dong, i)
                            };
                        break;
                    case "MaNguoiTao":
                        nguoiTao = new NguoiDungViewDTO()
                            {
                                ma = layInt(dong, i)
                            };
                        break;
                    case "ThoiDiemTao":
                        thoiDiemTao = layDateTime(dong, i, DateTime.MinValue);
                        break;
                    case "CanDangKy":
                        canDangKy = layBool(dong, i, false);
                        break;
                    case "HanDangKy":
                        hanDangKy = layDateTime(dong, i, DateTime.MinValue);
                        break;
                    case "PhiThamGia":
                        phiThamGia = layInt(dong, i);
                        break;
                    case "CheDoRiengTu":
                        cheDoRiengTu = CheDoRiengTu.lay(layString(dong, i));
                        break;
                    case "CoBangDiem":
                        coBangDiem = layBool(dong, i, false);
                        break;
                    case "CoBangDiemDanh":
                        coBangDiemDanh = layBool(dong, i, false);
                        break;
                    case "CanDuyetBaiViet":
                        canDuyetBaiViet = layBool(dong, i, false);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}