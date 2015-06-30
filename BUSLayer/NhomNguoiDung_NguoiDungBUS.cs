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
    public class NhomNguoiDung_NguoiDungBUS : BUS
    {
        public static KetQua layNguoiDung_TimKiem(string tuKhoa, string phamVi, int maNhomNguoiDung, int maDoiTuong = 0)
        {
            KetQua ketQua;
            if (phamVi == "KH" && maDoiTuong != 0)
            {
                ketQua = NguoiDungDAO.layTheoMaKhoaHoc_TimKiem(maDoiTuong, tuKhoa);
            }
            else
            {
                ketQua = NguoiDungDAO.lay_TimKiem(tuKhoa);
            }

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            List<NguoiDungDTO> danhSachNguoiDung = ketQua.ketQua as List<NguoiDungDTO>;

            List<NguoiDungDTO> danhSachTrongNhom = new List<NguoiDungDTO>();
            List<NguoiDungDTO> danhSachNgoaiNhom = new List<NguoiDungDTO>();

            foreach(var nguoiDung in danhSachNguoiDung)
            {
                ketQua = NhomNguoiDung_NguoiDungDAO.layTheoMaNhomNguoiDungVaMaNguoiDung(phamVi, maNhomNguoiDung, nguoiDung.ma);

                if (ketQua.trangThai == 0)
                {
                    danhSachTrongNhom.Add(nguoiDung);
                }
                else
                {
                    danhSachNgoaiNhom.Add(nguoiDung);
                }
            }

            return new KetQua()
            {
                trangThai = 0,
                ketQua = new List<NguoiDungDTO>[]
                {
                    danhSachTrongNhom,
                    danhSachNgoaiNhom
                }
            };
        }

        public static KetQua them(string phamVi, int maNhomNguoiDung, int maNguoiDung, int maNguoiThem)
        {
            #region Kiểm tra điều kiện
            //Lấy nhóm người dùng
            var ketQua = NhomNguoiDungDAO.layTheoMa(phamVi, maNhomNguoiDung);
            if (ketQua.trangThai != 0)
            {
                return new KetQua(3, "Nhóm người dùng không tồn tại");
            }

            var nhomNguoiDung = ketQua.ketQua as NhomNguoiDungDTO;

            //Kiểm tra quyền
            if (!coQuyen("QLQuyen", phamVi, nhomNguoiDung.doiTuong == null ? 0 : nhomNguoiDung.doiTuong.ma.Value, maNguoiThem))
            {
                return new KetQua(3, "Bạn không có quyền thêm người dùng vào nhóm");
            }
            #endregion

            return NhomNguoiDung_NguoiDungDAO.them(phamVi, maNhomNguoiDung, maNguoiDung);
        }

        public static KetQua xoaTheoMaNhomNguoiDungVaMaNguoiDung(string phamVi, int maNhomNguoiDung, int maNguoiDung, int maNguoiXoa)
        {
            #region Kiểm tra điều kiện
            //Lấy nhóm người dùng
            var ketQua = NhomNguoiDungDAO.layTheoMa(phamVi, maNhomNguoiDung);
            if (ketQua.trangThai != 0)
            {
                return new KetQua(3, "Nhóm người dùng không tồn tại");
            }

            var nhomNguoiDung = ketQua.ketQua as NhomNguoiDungDTO;

            //Kiểm tra quyền
            if (!coQuyen("QLQuyen", phamVi, nhomNguoiDung.doiTuong == null ? 0 : nhomNguoiDung.doiTuong.ma.Value, maNguoiXoa))
            {
                return new KetQua(3, "Bạn không có quyền xóa người dùng khỏi nhóm");
            }
            #endregion

            return NhomNguoiDung_NguoiDungDAO.xoaTheoMaNhomNguoiDungVaMaNguoiDung(phamVi, maNhomNguoiDung, maNguoiDung);
        }
    }
}
