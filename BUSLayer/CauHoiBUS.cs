using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DAOLayer;
using DTOLayer;
using System.IO;
using Data;

namespace BUSLayer
{
    public class CauHoiBUS : BUS
    {
        public static KetQua kiemTra(CauHoiDataDTO cauHoi)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (string.IsNullOrEmpty(cauHoi.tieuDe))
            {
                loi.Add("Tiêu đề không được bỏ trống");
            }
            if (string.IsNullOrEmpty(cauHoi.noiDung))
            {
                loi.Add("Nội dung không được bỏ trống");                
            }
            #endregion

            if (loi.Count > 0)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = loi
                };
            }
            else
            {
                return new KetQua()
                {
                    trangThai = 0
                };
            }
        }

        public static KetQua them(Dictionary<string,string> formCauHoi)
        {
            CauHoiDataDTO cauHoi = new CauHoiDataDTO()
            {
                tieuDe = layString(formCauHoi, "TieuDe"),
                noiDung = layString(formCauHoi, "NoiDung"),
                thoiDiemTao = DateTime.Now,
                maNguoiTao = (int)Session["NguoiDung"]
            };
            
            KetQua ketQua = kiemTra(cauHoi);
            
            if (ketQua.trangThai == 0)
                return CauHoiDAO.them(cauHoi);
            return ketQua;
        }
        
        public static KetQua layCauHoi(int ma)
        {            
            return CauHoiDAO.lay(ma, new LienKet()
            {
                "NguoiDung"
            });
        }
    }
}