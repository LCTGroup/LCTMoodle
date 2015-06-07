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
    public class GiaoTrinhBUS : BUS
    {
        public static KetQua kiemTra(GiaoTrinhDTO giaoTrinh)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (giaoTrinh.khoaHoc == null)
            {
                loi.Add("Khóa học không được bỏ trống");
            }
            if (string.IsNullOrEmpty(giaoTrinh.congViec))
            {
                loi.Add("Công việc không được bỏ trống");
            }
            if (string.IsNullOrEmpty(giaoTrinh.moTa))
            {
                loi.Add("Mô tả không được bỏ trống");
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

        public static KetQua layTheoMaKhoaHoc(int maKhoaHoc)
        {
            return GiaoTrinhDAO.layTheoMaKhoaHoc(maKhoaHoc);
        }

        public static KetQua them(Dictionary<string, string> form)
        {
            GiaoTrinhDTO giaoTrinh = new GiaoTrinhDTO()
            {
                khoaHoc = layDTO<KhoaHocDTO>(form, "KhoaHoc"),
                congViec = layString(form, "CongViec"),
                moTa = layString(form, "MoTa"),
                thoiGian = layString(form, "ThoiGian")
            };

            KetQua ketQua = kiemTra(giaoTrinh);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return GiaoTrinhDAO.them(giaoTrinh);
        }

        public static KetQua xoaTheoMa(int ma)
        {
            return GiaoTrinhDAO.xoaTheoMa(ma);
        }

        public static KetQua capNhatThuTu(int ma, int thuTu, int maKhoaHoc)
        {
            return GiaoTrinhDAO.capNhatThuTu(ma, thuTu, maKhoaHoc);
        }
    }
}
