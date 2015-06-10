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
    public class TraLoiBUS : BUS
    {
        public static KetQua kiemTra(TraLoiDTO traLoi)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (string.IsNullOrEmpty(traLoi.noiDung))
            {
                loi.Add("Nội dung không được bỏ trống");
            }
            if (traLoi.nguoiTao == null)
            {
                loi.Add("Người dùng không được bỏ trống");
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

        public static KetQua them(Dictionary<string, string> form)
        {
            TraLoiDTO traLoi = new TraLoiDTO()
            {
                cauHoi = layDTO<CauHoiDTO>(layInt(form, "CauHoi")),
                noiDung = layString(form, "NoiDung"),
                nguoiTao = layDTO<NguoiDungDTO>(Session["NguoiDung"] as int?)
            };
            
            KetQua ketQua = TraLoiBUS.kiemTra(traLoi);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }
            return TraLoiDAO.them(traLoi, new LienKet() { "NguoiTao" });
        }

        public static KetQua layTheoMaCauHoi(int maCauHoi)
        {
            return TraLoiDAO.layTheoMaCauHoi(maCauHoi, new LienKet() { "NguoiTao" });
        }

        public static KetQua layTraLoiTheoMa(int ma)
        {
            return TraLoiDAO.layTraLoiTheoMa(ma);
        }
    }
}