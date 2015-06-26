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
    public class ChuongTrinhBUS : BUS
    {
        public static KetQua kiemTra(ChuongTrinhDTO chuongTrinh)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (chuongTrinh.khoaHoc == null)
            {
                loi.Add("Khóa học không được bỏ trống");
            }
            if (string.IsNullOrEmpty(chuongTrinh.baiHoc))
            {
                loi.Add("Bài học không được bỏ trống");
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
            return ChuongTrinhDAO.layTheoMaKhoaHoc(maKhoaHoc);
        }

        public static KetQua them(Dictionary<string, string> form)
        {
            ChuongTrinhDTO chuongTrinh = new ChuongTrinhDTO()
            {
                khoaHoc = layDTO<KhoaHocDTO>(form, "KhoaHoc"),
                baiHoc = layString(form, "BaiHoc"),
                noiDung = layString(form, "NoiDung"),
                thoiGian = layString(form, "ThoiGian")
            };

            KetQua ketQua = kiemTra(chuongTrinh);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return ChuongTrinhDAO.them(chuongTrinh);
        }

        public static KetQua xoaTheoMa(int ma)
        {
            return ChuongTrinhDAO.xoaTheoMa(ma);
        }

        public static KetQua capNhatThuTu(int thuTuCu, int thuTuMoi, int maKhoaHoc)
        {
            return ChuongTrinhDAO.capNhatThuTu(thuTuCu, thuTuMoi, maKhoaHoc);
        }
    }
}
