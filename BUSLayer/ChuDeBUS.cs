using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOLayer;
using DTOLayer;
using System.IO;
using Data;

namespace BUSLayer
{
    public class ChuDeBUS : BUS
    {
        public static KetQua kiemTra(ChuDeDTO chuDe)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (string.IsNullOrEmpty(chuDe.ten))
            {
                loi.Add("Tên không được bỏ trống");
            }
            if (string.IsNullOrEmpty(chuDe.moTa))
            {
                loi.Add("Mô tả không được bỏ trống");
            }
            if (chuDe.nguoiTao == null)
            {
                loi.Add("Người tạo không được bỏ trống");
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

        public static void gan(ref ChuDeDTO chuDe, Form form)
        {
            if (chuDe == null)
            {
                chuDe = new ChuDeDTO();
            }

            foreach (string key in form.Keys.ToArray())
            {
                switch (key)
                {
                    case "Ten":
                        chuDe.ten = form.layString(key);
                        break;
                    case "MoTa":
                        chuDe.moTa = form.layString(key);
                        break;
                    case "HinhDaiDien":
                        chuDe.hinhDaiDien = TapTinBUS.chuyen("ChuDe_HinhDaiDien", form.layInt(key)).ketQua as TapTinDTO;
                        break;
                    case "NguoiTao":
                        chuDe.nguoiTao = form.layDTO<NguoiDungDTO>(key);
                        break;
                    case "Cha":
                        chuDe.cha = form.layDTO<ChuDeDTO>(key);
                        break;
                    default:
                        break;
                }
            }
        }

        public static KetQua them(Form form)
        {
            ChuDeDTO chuDe = new ChuDeDTO();

            if (Session["NguoiDung"] != null)
            {
                form.Add("NguoiTao", Session["NguoiDung"].ToString());
            }

            gan(ref chuDe, form);

            KetQua ketQua = kiemTra(chuDe);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return ChuDeDAO.them(chuDe);
        }

        public static KetQua layTheoMa(int ma, LienKet lienKet = null)
        {
            return ChuDeDAO.layTheoMa(ma, lienKet);
        }

        public static KetQua layTheoMaCha(int maCha, LienKet lienKet = null)
        {
            return ChuDeDAO.layTheoMaCha(maCha, lienKet);
        }

        public static KetQua lay_TimKiem(string tuKhoa, LienKet lienKet = null)
        {
            return ChuDeDAO.lay_TimKiem(tuKhoa, lienKet);
        }
    }
}
