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
    public class CauHoi_DiemBUS : BUS
    {
        public static void gan(ref CauHoi_DiemDTO cauHoi_Diem, Form form)
        {
            if (cauHoi_Diem == null)
            {
                cauHoi_Diem = new CauHoi_DiemDTO();
            }

            foreach (string key in form.Keys.ToArray())
            {
                switch (key)
                {
                    case "MaCauHoi":
                        cauHoi_Diem.cauHoi = form.layDTO<CauHoiDTO>(key);
                        break;
                    case "MaNguoiTao":
                        cauHoi_Diem.nguoiTao = form.layDTO<NguoiDungDTO>(key);
                        break;
                    case "Diem":
                        cauHoi_Diem.diem = form.layBool(key);
                        break;                   
                    default:
                        break;
                }
            }
        }
        
        public static KetQua kiemTra(CauHoi_DiemDTO cauHoi_Diem, string[] truong = null, bool kiemTra = true)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (coKiemTra("MaCauHoi", truong, kiemTra) && cauHoi_Diem.cauHoi == null)
            {
                loi.Add("Câu hỏi không được bỏ trống");
            }
            if (coKiemTra("MaNguoiTao", truong, kiemTra) && cauHoi_Diem.nguoiTao == null)
            {
                loi.Add("Chưa đăng nhập");
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
        
        public static BangCapNhat layBangCapNhat(CauHoi_DiemDTO cauHoi_Diem, string[] keys)
        {
            BangCapNhat bangCapNhat = new BangCapNhat();
            foreach (string key in keys)
            {
                switch (key)
                {
                    case "MaCauHoi":
                        bangCapNhat.Add("MaNguoiTao", layMa_String(cauHoi_Diem.cauHoi), 1);
                        break;
                    case "MaNguoiTao":
                        bangCapNhat.Add("MaNguoiTao", layMa_String(cauHoi_Diem.nguoiTao), 1);
                        break;
                    default:
                        break;
                }
            }
            return bangCapNhat;
        }
       
        public static KetQua them(int? maCauHoi, int? maNguoiTao, bool diem)
        {
            return CauHoi_DiemDAO.them(maCauHoi, maNguoiTao, diem);
        }

        public static KetQua xoaTheoMa(int? maCauHoi, int? maNguoiTao)
        {
            return CauHoi_DiemDAO.xoaTheoMa(maCauHoi, maNguoiTao);
        }

        public static KetQua layDiemVoteNguoiDung(int? maNguoiTao)
        {
            return CauHoi_DiemDAO.layCauHoi_DiemTheoMaNguoiTao(maNguoiTao);
        }

        public static KetQua layDiem(int? maCauHoi)
        {
            return CauHoi_DiemDAO.layTheoMaCauHoi_Diem(maCauHoi);
        }
    }
}