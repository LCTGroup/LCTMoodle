﻿using System;
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
                    case "ThoiDiemCapNhat":
                        bangCapNhat.Add("ThoiDiemCapNhat", cauHoi.thoiDiemCapNhat.HasValue ? cauHoi.thoiDiemCapNhat.Value.ToString("d/M/yyyy H:mm") : null, 1);
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

            if (Session["NguoiDung"] != null)
            {
                form.Add("MaNguoiTao", Session["NguoiDung"].ToString());
            }
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
        
        public static KetQua layTheoMa(int? ma)
        {            
            return CauHoiDAO.layTheoMa(ma, new LienKet()
            {
                "NguoiTao", 
                { 
                    "TraLoi", 
                    new LienKet()
                    {
                        "NguoiTao"
                    }
                }
            });
        }

        public static KetQua layToanBoCauHoi()
        {
            return CauHoiDAO.layDanhSachCauHoi(new LienKet() 
            { 
                "NguoiTao"
            });
        }

        public static KetQua capNhatTheoMa(Form form)
        {
            int? maCauHoi = form.layInt("Ma");
            if (!maCauHoi.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 1
                };
            }

            KetQua ketQua = CauHoiBUS.layTheoMa(maCauHoi.Value);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            CauHoiDTO cauHoi = ketQua.ketQua as CauHoiDTO;

            form.Add("ThoiDiemCapNhat", DateTime.Now.ToString());
            gan(ref cauHoi, form);

            ketQua = kiemTra(cauHoi, form.Keys.ToArray());

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return CauHoiDAO.capNhatTheoMa(maCauHoi, layBangCapNhat(cauHoi, form.Keys.ToArray()), new LienKet() 
            {
                "NguoiTao"
            });
        }
    }
}