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
    public class CotDiemBUS : BUS
    {
        public static KetQua kiemTra(CotDiemDTO cotDiem)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (cotDiem.khoaHoc == null)
            {
                loi.Add("Khóa học không được bỏ trống");
            }
            if (string.IsNullOrEmpty(cotDiem.ten))
            {
                loi.Add("Tên không được bỏ trống");
            }
            if (cotDiem.heSo == 0)
            {
                loi.Add("Hệ số không được bỏ trống");
            }
            if (cotDiem.ngay == null)
            {
                loi.Add("Ngày không được bỏ trống");
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

        public static KetQua them(Dictionary<string, string> form)
        {
            CotDiemDTO cotDiem = new CotDiemDTO()
            {
                ten = layString(form, "Ten"),
                moTa = layString(form, "MoTa"),
                heSo = layInt(form, "HeSo"),
                ngay = layDate(form, "Ngay"),
                khoaHoc = layDTO<KhoaHocDTO>(form, "KhoaHoc")
            };

            KetQua ketQua = kiemTra(cotDiem);
            
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }
            
            return CotDiemDAO.them(cotDiem);
        }

        public static KetQua layTheoMaKhoaHoc(int maKhoaHoc)
        {
            return CotDiemDAO.layTheoMaKhoaHoc(maKhoaHoc);
        }

        public static KetQua xoaTheoMa(int ma)
        {
            return CotDiemDAO.xoaTheoMa(ma);
        }

        public static KetQua capNhatThuTu(int thuTuCu, int thuTuMoi, int maKhoaHoc)
        {
            return CotDiemDAO.capNhatThuTu(thuTuCu, thuTuMoi, maKhoaHoc);
        }
    }
}
