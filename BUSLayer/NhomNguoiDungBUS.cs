using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOLayer;
using DTOLayer;
using System.IO;
using System.Web;
using Data;

namespace BUSLayer
{
    public class NhomNguoiDungBUS : BUS
    {
        public static KetQua kiemTra(NhomNguoiDungDTO nhomNguoiDung, string[] truong = null, bool kiemTra = true)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (coKiemTra("Ten", truong, kiemTra) && string.IsNullOrEmpty(nhomNguoiDung.ten))
            {
                loi.Add("Tên không được bỏ trống");
            }
            if (coKiemTra("PhamVi", truong, kiemTra) && nhomNguoiDung.phamVi == null)
            {
                loi.Add("Phạm vi không được bỏ trống");
            }
            if (coKiemTra("DoiTuong", truong, kiemTra) && nhomNguoiDung.phamVi != "HT" && nhomNguoiDung.doiTuong == null)
            {
                loi.Add("Đối tượng không được bỏ trống");
            }
            if (coKiemTra("NguoiTao", truong, kiemTra) && nhomNguoiDung.nguoiTao == null)
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

        public static void gan(ref NhomNguoiDungDTO nhomNguoiDung, Form form)
        {
            if (nhomNguoiDung == null)
            {
                nhomNguoiDung = new NhomNguoiDungDTO();
            }

            foreach (string key in form.Keys.ToArray())
            {
                switch (key)
                {
                    case "Ten":
                        nhomNguoiDung.ten = form.layString(key);
                        break;
                    case "MoTa":
                        nhomNguoiDung.moTa = form.layString(key);
                        break;
                    case "PhamVi":
                        nhomNguoiDung.phamVi = form.layString(key);
                        break;
                    case "DoiTuong":
                        nhomNguoiDung.doiTuong = form.layDTO<DTO>(key);
                        break;
                    case "NguoiTao":
                        nhomNguoiDung.nguoiTao = form.layDTO<NguoiDungDTO>(key);
                        break;
                    default:
                        break;
                }
            }
        }

        public static KetQua them(Form form)
        {
            if (Session["NguoiDung"] != null)
            {
                form.Add("NguoiTao", Session["NguoiDung"].ToString());
            }

            NhomNguoiDungDTO nhomNguoiDung = new NhomNguoiDungDTO();

            gan(ref nhomNguoiDung, form);

            KetQua ketQua = kiemTra(nhomNguoiDung);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return NhomNguoiDungDAO.them(nhomNguoiDung);
        }

        public static KetQua layTheoMaDoiTuong(string phamVi, int maDoiTuong)
        {
            return NhomNguoiDungDAO.layTheoMaDoiTuong(phamVi, maDoiTuong);
        }

        public static KetQua xoaTheoMa(string phamVi, int ma)
        {
            return NhomNguoiDungDAO.xoaTheoMa(phamVi, ma);
        }

        public static KetQua capNhatQuyenTheoMaNhomNguoiDungVaMaQuyen(string phamVi, int maNhomNguoiDung, int maQuyen, bool co)
        {
            return NhomNguoiDungDAO.capNhatQuyenTheoMaNhomNguoiDungVaMaQuyen(phamVi, maNhomNguoiDung, maQuyen, co);
        }
    }
}
