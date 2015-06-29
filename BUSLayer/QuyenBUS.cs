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

        /// <summary>
        /// Lấy danh sách quyền
        /// </summary>
        /// <param name="phamVi">Phạm vi quyền ("HT", "ND", "CD", "HD", "KH") - !Không phải phạm vi nhóm quyền</param>
        /// <param name="maDoiTuong">Đối tượng tác động. Chỉ "CD", "KH" cần truyền</param>
        /// <param name="maNguoiDung">Mã người dùng cần kiểm tra (Mặc định là người dùng hiện tại)</param>
        /// <returns>string[]</returns>
        public static KetQua layTheoMaNguoiDungVaMaDoiTuong_MangGiaTri(string phamVi, int maDoiTuong = 0, int? maNguoiDung = null)
        {
            if (Session["NguoiDung"] != null || maNguoiDung.HasValue)
            {
                KetQua ketQua = QuyenDAO.layTheoMaNguoiDung_MaDoiTuong_ChuoiGiaTri(maNguoiDung.HasValue ? maNguoiDung.Value : (int)Session["NguoiDung"], phamVi, maDoiTuong);

                if (ketQua.trangThai == 0)
                {
                    ketQua.ketQua = ketQua.ketQua.ToString().Split('|');
                }

                return ketQua;
            }

            return new KetQua() 
            {
                trangThai = 4
            };
        }

        public static KetQua kiemTraQuyenNguoiDung(string giaTri, string phamVi, int maDoiTuong = 0, int? maNguoiDung = null)
        {
            if (Session["NguoiDung"] != null || maNguoiDung.HasValue)
            {
                KetQua ketQua = QuyenDAO.layTheoMaNguoiDungVaGiaTriVaMaDoiTuong_KiemTra(giaTri, maNguoiDung.HasValue ? maNguoiDung : Session["NguoiDung"] as int?, phamVi, maDoiTuong);

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
