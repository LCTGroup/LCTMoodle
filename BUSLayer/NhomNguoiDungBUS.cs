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
            if (coKiemTra("MaDoiTuong", truong, kiemTra) && nhomNguoiDung.phamVi != "HT" && nhomNguoiDung.doiTuong == null)
            {
                loi.Add("Đối tượng không được bỏ trống");
            }
            if (coKiemTra("MaNguoiTao", truong, kiemTra) && nhomNguoiDung.nguoiTao == null)
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
                    case "MaDoiTuong":
                        nhomNguoiDung.doiTuong = form.layDTO<DTO>(key);
                        break;
                    case "MaNguoiTao":
                        nhomNguoiDung.nguoiTao = form.layDTO<NguoiDungDTO>(key);
                        break;
                    default:
                        break;
                }
            }
        }

        public static KetQua them(Form form)
        {
            #region Kiểm tra điều kiện
            //Lấy mã người tạo
            var maNguoiTao = form.layInt("MaNguoiTao");
            if (!maNguoiTao.HasValue)
            {
                return new KetQua(3, "Người tạo không thể bỏ trống");
            }

            var phamVi = form.layString("PhamVi");
            var maDoiTuong = form.layInt("MaDoiTuong");
            if (phamVi == null || !maDoiTuong.HasValue)
            {
                return new KetQua(3, "Đối tượng không thể bỏ trống");
            }

            if (!coQuyen("QLQuyen", phamVi, maDoiTuong.Value, maNguoiTao))
            {
                return new KetQua(3, "Bạn không có quyền thêm nhóm người dùng");
            }
            #endregion

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

        public static KetQua xoaTheoMa(string phamVi, int ma, int maNguoiXoa)
        {
            #region Kiểm tra điều kiện
            //Lấy nhóm người dùng
            var ketQua = NhomNguoiDungDAO.layTheoMa(phamVi, ma);
            if (ketQua.trangThai != 0)
            {
                return new KetQua(1, "Nhóm người dùng không tồn tại");
            }
            var nhomNguoiDung = ketQua.ketQua as NhomNguoiDungDTO;

            if (!coQuyen("QLQuyen", phamVi, nhomNguoiDung.doiTuong.ma.Value, maNguoiXoa))
            {
                return new KetQua(3, "Bạn không có quyền xóa nhóm người dùng");
            }
            #endregion

            return NhomNguoiDungDAO.xoaTheoMa(phamVi, ma);
        }
    }
}
