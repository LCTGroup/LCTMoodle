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
        public static KetQua kiemTra(TraLoiDataDTO traLoi)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (string.IsNullOrEmpty(traLoi.noiDung))
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

        public static KetQua them(Dictionary<string, string> form)
        {
            TraLoiDataDTO traLoi = new TraLoiDataDTO()
            {
                maCauHoi = layInt(form, "KhoaHoc"),
                noiDung = layString(form, "NoiDung"),
                maNguoiTao = (int)Session["NguoiDung"],
                thoiDiemTao = DateTime.Now,
            };
            
            KetQua ketQua = TraLoiBUS.kiemTra(traLoi);

            if (ketQua.trangThai != 0)
                ketQua = TraLoiDAO.them(traLoi);
            return ketQua;
        }

        public static KetQua layDanhSachTraLoiTheoCauHoi(int maCauHoi)
        {
            return TraLoiDAO.layDanhSachTraLoiTheoCauHoi(maCauHoi);
        }

        public static KetQua layTraLoiTheoMa(int ma)
        {
            return TraLoiDAO.layTraLoiTheoMa(ma);
        }
    }
}