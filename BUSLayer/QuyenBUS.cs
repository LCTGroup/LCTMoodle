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
    public class QuyenBUS : BUS
    {
        public static KetQua layTheoPhamViVaMaCha(string phamVi, int maCha, bool laQuyenChung = false)
        {
            return QuyenDAO.layTheoPhamViVaMaChaVaLaQuyenChung(phamVi, maCha, laQuyenChung);
        }

        public static KetQua layTheoPhamVi_Cay(string phamVi, bool laQuyenChung = false)
        {
            KetQua ketQua = QuyenDAO.layTheoPhamViVaMaChaVaLaQuyenChung(phamVi, 0, laQuyenChung);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            var danhSachQuyen = ketQua.ketQua as List<QuyenDTO>;

            foreach(var quyen in danhSachQuyen)
            {
                quyen.con = layCon(quyen);
            }

            return new KetQua()
            {
                trangThai = 0,
                ketQua = danhSachQuyen
            };
        }

        private static List<QuyenDTO> layCon(QuyenDTO quyen)
        {
            if (quyen == null || !string.IsNullOrEmpty(quyen.giaTri))
            {
                return null;
            }

            List<QuyenDTO> danhSachCon = layDanhSachDTO<QuyenDTO>(QuyenDAO.layTheoPhamViVaMaChaVaLaQuyenChung(quyen.phamVi, quyen.ma, quyen.laQuyenChung));

            if (danhSachCon != null)
            {
                foreach (var con in danhSachCon)
                {
                    con.con = layCon(con);
                }
            }

            return danhSachCon;
        }

        public static KetQua layTheoMaNguoiDungVaMaDoiTuong_MangGiaTri(int? maNguoiDung = null, string phamVi = "HT", int maDoiTuong = 0)
        {
            if (Session["NguoiDung"] != null || maNguoiDung.HasValue)
            {
                KetQua ketQua = QuyenDAO.layTheoMaNguoiDung_MaDoiTuong_ChuoiGiaTri(maNguoiDung.HasValue ? maNguoiDung.Value : (int)Session["NguoiDung"], phamVi, maDoiTuong);

                if (ketQua.trangThai == 0)
                {
                    ketQua.ketQua = ketQua.ketQua.ToString().Split(',');
                }

                return ketQua;
            }

            return new KetQua() 
            {
                trangThai = 4
            };
        }

        public static KetQua kiemTraQuyenNguoiDung(string giaTri, string phamVi, int? maNguoiDung = null, int maDoiTuong = 0)
        {
            if (Session["NguoiDung"] != null || maNguoiDung.HasValue)
            {
                KetQua ketQua = QuyenDAO.layTheoMaNguoiDungVaGiaTriVaMaDoiTuong_KiemTra(maNguoiDung.HasValue ? maNguoiDung.Value : (int)Session["NguoiDung"], giaTri, phamVi, maDoiTuong);

                if (ketQua.trangThai > 1)
                {
                    return ketQua;
                }
                if (ketQua.trangThai == 1)
                {
                    ketQua.trangThai = 0;
                    ketQua.ketQua = false;
                }

                return ketQua;
            }

            return new KetQua() 
            { 
                trangThai = 4
            };
        }
    }
}
