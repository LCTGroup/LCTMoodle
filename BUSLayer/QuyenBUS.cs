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
        public static KetQua layTheoPhamViVaMaCha(string phamVi, int maCha)
        {
            return QuyenDAO.layTheoPhamViVaMaCha(phamVi, maCha);
        }

        public static KetQua layTheoPhamVi_Cay(string phamVi)
        {
            KetQua ketQua = QuyenDAO.layTheoPhamViVaMaCha(phamVi, 0);

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

            List<QuyenDTO> danhSachCon = layDanhSachDTO<QuyenDTO>(QuyenDAO.layTheoPhamViVaMaCha(quyen.phamVi, quyen.ma));

            if (danhSachCon != null)
            {
                foreach (var con in danhSachCon)
                {
                    con.con = layCon(con);
                }
            }

            return danhSachCon;
        }

        public static string[] layTheoMaDoiTuongVaMaNguoiDung_MangGiaTri(string phamVi, int maDoiTuong, int? maNguoiDung = null)
        {
            if (Session["NguoiDung"] != null || maNguoiDung.HasValue)
            {
                KetQua ketQua = QuyenDAO.layTheoMaDoiTuongVaMaNguoiDung_ChuoiGiaTri(phamVi, maDoiTuong, maNguoiDung.HasValue ? maNguoiDung.Value : (int)Session["NguoiDung"]);

                if (ketQua.trangThai != 0)
                {
                    return new string[0];
                }

                return ketQua.ketQua.ToString().Split(',');
            }

            return new string[0];
        }
    }
}
