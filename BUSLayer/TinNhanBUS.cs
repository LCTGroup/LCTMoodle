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
    public class TinNhanBUS : BUS
    {
        public static void gan(ref TinNhanDTO tinNhan, Form form)
        {
            if (tinNhan == null)
            {
                tinNhan = new TinNhanDTO();
            }
            foreach (var key in form.Keys.ToArray())
            {
                switch (key)
                {
                    case "MaNguoiGui":
                        tinNhan.nguoiGui = form.layDTO<NguoiDungDTO>(key);
                        break;
                    case "MaNguoiNhan":
                        tinNhan.nguoiNhan = form.layDTO<NguoiDungDTO>(key);
                        break;
                    case "NoiDung":
                        tinNhan.noiDung = form.layString(key);
                        break;                    
                    default:
                        break;
                }
            }
        }

        public static KetQua kiemTra(TinNhanDTO tinNhan, string[] truong = null, bool kiemTra = true)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi

            if (coKiemTra("MaNguoiGui", truong, kiemTra) && tinNhan.nguoiGui == null)
            {
                loi.Add("Người gửi không được bỏ trống");
            }
            if (coKiemTra("MaNguoiNhan", truong, kiemTra) && tinNhan.nguoiNhan == null)
            {
                loi.Add("Người nhận không được bỏ trống");
            }
            if (coKiemTra("NoiDung", truong, kiemTra) && string.IsNullOrWhiteSpace(tinNhan.noiDung))
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

        public static KetQua them(Form form, LienKet lienKet = null, int? maNguoiGui = null)
        {
            if (!maNguoiGui.HasValue)
            {
                return new KetQua(3, "Bạn chưa đăng nhập");
            }
            form.Add("MaNguoiGui", maNguoiGui.ToString());

            TinNhanDTO tinNhan = new TinNhanDTO();            
            gan(ref tinNhan, form);

            return TinNhanDAO.them(tinNhan, lienKet);
        }

        public static KetQua lay(int maNguoiGui, int maNguoiNhan, LienKet lienKet = null)
        {
            return TinNhanDAO.layTheoMaNguoiGuiVaMaNguoiNhan(maNguoiGui, maNguoiNhan, lienKet);
        }

        public static KetQua layDanhSachTinNhanTheoMaNguoiDung(int maNguoiDung, LienKet lienKet = null)
        {
            return TinNhanDAO.layDanhSachTinNhanTheoMaNguoiDung(maNguoiDung, lienKet);
        }
    }
}