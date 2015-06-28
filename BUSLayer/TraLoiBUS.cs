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
using Helpers;

namespace BUSLayer
{
    public class TraLoiBUS : BUS
    {
        public static KetQua kiemTra(TraLoiDTO traLoi, string[] truong = null, bool kiemTra = true)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (coKiemTra("NoiDung", truong, kiemTra) && string.IsNullOrEmpty(traLoi.noiDung))
            {
                loi.Add("Nội dung không được bỏ trống");
            }
            if (coKiemTra("MaNguoiTao", truong, kiemTra) && traLoi.nguoiTao == null)
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
        
        public static void gan(ref TraLoiDTO traLoi, Form form)
        {
            if (traLoi == null)
            {
                traLoi = new TraLoiDTO();
            }

            foreach (string key in form.Keys.ToArray())
            {
                switch (key)
                {                    
                    case "NoiDung":
                        traLoi.noiDung = form.layString(key);
                        break;
                    case "MaNguoiTao":
                        traLoi.nguoiTao = form.layDTO<NguoiDungDTO>(key);
                        break;
                    case "MaCauHoi":
                        traLoi.cauHoi = form.layDTO<CauHoiDTO>(key);
                        break;
                    default:
                        break;
                }
            }
        }

        public static BangCapNhat layBangCapNhat(TraLoiDTO traLoi, string[] keys)
        {
            BangCapNhat bangCapNhat = new BangCapNhat();
            foreach (string key in keys)
            {
                switch (key)
                {
                    case "NoiDung":
                        bangCapNhat.Add("NoiDung", traLoi.noiDung, 2);
                        break;                    
                    default:
                        break;
                }
            }
            return bangCapNhat;
        }
        
        public static KetQua them(Form form)
        {
            TraLoiDTO traLoi = new TraLoiDTO();

            if (Session["NguoiDung"] != null)
            {
                form.Add("MaNguoiTao", Session["NguoiDung"].ToString());
            }
            
            gan(ref traLoi, form);

            KetQua ketQua = TraLoiBUS.kiemTra(traLoi);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }
            return TraLoiDAO.them(traLoi, new LienKet() { "NguoiTao" });
        }

        public static KetQua xoaTheoMa(int? ma)
        {
            return TraLoiDAO.xoaTheoMa(ma);
        }

        public static KetQua layTheoMaCauHoi(int maCauHoi)
        {
            return TraLoiDAO.layTheoMaCauHoi(maCauHoi, new LienKet() { "NguoiTao" });
        }

        public static KetQua layTheoMa(int? ma)
        {
            return TraLoiDAO.layTheoMa(ma);
        }

        public static KetQua laySoLuongTraLoiTrongCauHoi(int maCauHoi)
        {
            return TraLoiDAO.layTraLoiTheoMaCauHoi_SoLuong(maCauHoi);
        }

        public static KetQua capNhat(Form form)
        {
            int? maTraLoi = form.layInt("Ma");
            if (!maTraLoi.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 1
                };
            }

            KetQua ketQua = TraLoiBUS.layTheoMa(maTraLoi.Value);
            if (ketQua.trangThai != 0) 
            {
                return ketQua;
            }

            TraLoiDTO traLoi = ketQua.ketQua as TraLoiDTO;
            gan(ref traLoi, form);

            ketQua = TraLoiBUS.kiemTra(traLoi, form.Keys.ToArray());
            if (ketQua.trangThai != 0) 
            {
                return ketQua;
            }

            return TraLoiDAO.capNhatTheoMa(maTraLoi, layBangCapNhat(traLoi, form.Keys.ToArray()), new LienKet() { 
                "NguoiTao",
                "CauHoi"
            });
        }

        public static KetQua capNhatDuyetTheoMa(int? ma, bool duyet)
        {

            return TraLoiDAO.capNhatDuyetTheoMa(ma, duyet);
        }
    }
}