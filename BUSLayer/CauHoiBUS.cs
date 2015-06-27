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
        public static void gan(ref CauHoiDTO cauHoi, Form form)
        {
            if (cauHoi == null)
            {
                cauHoi = new CauHoiDTO();
            }

            foreach (string key in form.Keys.ToArray())
            {
                switch (key)
                {
                    case "TieuDe":
                        cauHoi.tieuDe = form.layString(key);
                        break;
                    case "NoiDung":
                        cauHoi.noiDung = form.layString(key);
                        break;
                    case "MaNguoiTao":
                        cauHoi.nguoiTao = form.layDTO<NguoiDungDTO>(key);
                        break;
                    case "MaChuDe":
                        cauHoi.chuDe = form.layDTO<ChuDeDTO>(key);
                        break;
                    case "Diem":
                        cauHoi.diem = form.layInt(key);
                        break;
                    default:
                        break;
                }
            }
        }
        
        public static KetQua kiemTra(CauHoiDTO cauHoi, string[] truong = null, bool kiemTra = true)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (coKiemTra("TieuDe", truong, kiemTra) && string.IsNullOrEmpty(cauHoi.tieuDe))
            {
                loi.Add("Tiêu đề không được bỏ trống");
            }
            if (coKiemTra("NoiDung", truong, kiemTra) && string.IsNullOrEmpty(cauHoi.noiDung))
            {
                loi.Add("Nội dung không được bỏ trống");                
            }
            if (coKiemTra("MaNguoiTao", truong, kiemTra) && cauHoi.nguoiTao == null)
            {
                loi.Add("Chưa đăng nhập");
            }  
            if (coKiemTra("Diem",truong,kiemTra) && cauHoi.diem == null)
            {
                loi.Add("Chưa có điểm");
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
        
        public static BangCapNhat layBangCapNhat(CauHoiDTO cauHoi, string[] keys)
        {
            BangCapNhat bangCapNhat = new BangCapNhat();
            foreach (string key in keys)
            {
                switch (key)
                {
                    case "TieuDe":
                        bangCapNhat.Add("TieuDe", cauHoi.tieuDe, 2);
                        break;
                    case "NoiDung":
                        bangCapNhat.Add("NoiDung", cauHoi.noiDung, 2);
                        break;
                    case "MaChuDe":
                        bangCapNhat.Add("MaChuDe", layMa_String(cauHoi.chuDe), 1);
                        break;
                    case "Diem":
                        bangCapNhat.Add("Diem", cauHoi.diem.ToString(), 1);
                        break;
                    default:
                        break;
                }
            }
            return bangCapNhat;
        }
       
        public static KetQua them(Form form)
        {
            CauHoiDTO cauHoi = new CauHoiDTO();
            gan(ref cauHoi, form);

            KetQua ketQua = kiemTra(cauHoi);
            
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }
            
            return CauHoiDAO.them(cauHoi);
        }

        public static KetQua xoaTheoMa(int? ma)
        {
            return CauHoiDAO.xoaTheoMa(ma);
        }

        public static KetQua capNhat(Form form, LienKet lienKet = null)
        {
            int? maCauHoi = form.layInt("Ma");
            if (!maCauHoi.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 1
                };
            }

            KetQua ketQua = CauHoiBUS.layTheoMa(maCauHoi.Value, new LienKet()
            {
                "NguoiTao", 
                {
                    "TraLoi",
                    new LienKet() 
                    {
                        "NguoiTao"
                    }
                },
                "ChuDe"
            });
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            CauHoiDTO cauHoi = ketQua.ketQua as CauHoiDTO;

            gan(ref cauHoi, form);

            ketQua = kiemTra(cauHoi, form.Keys.ToArray());

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return CauHoiDAO.capNhatTheoMa(maCauHoi, layBangCapNhat(cauHoi, form.Keys.ToArray()), lienKet);
        }

        public static KetQua capNhatDiem(int? maCauHoi, bool diem)
        {
            KetQua ketQua = CauHoi_DiemBUS.layDiem(maCauHoi);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            int? soDiem = ketQua.ketQua as int?;

            //soDiem = soDiem + (diem == true ? 1 : -1);

            return CauHoiDAO.capNhatTheoMa_Diem(maCauHoi, soDiem);
        }

        public static KetQua layTheoMa(int? ma, LienKet lienKet = null)
        {            
            return CauHoiDAO.layTheoMa(ma, lienKet);
        }

        public static KetQua layDanhSach(int? soDong = null, LienKet lienKet = null)
        {
            return CauHoiDAO.lay(soDong, lienKet);
        }

        public static KetQua layTheoMaChuDe_TimKiem(int? ma, string tuKhoa, LienKet lienKet = null)
        {
            return CauHoiDAO.layTheoMaChuDe_TimKiem(ma, tuKhoa, lienKet);
        }

        public static KetQua lay_TimKiem(string tuKhoa, LienKet lienKet = null)
        {
            return CauHoiDAO.lay_TimKiem(tuKhoa, lienKet);
        }

    }
}