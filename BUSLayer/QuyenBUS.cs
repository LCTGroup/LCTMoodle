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
using Helpers;

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
            if (quyen == null || !string.IsNullOrWhiteSpace(quyen.giaTri))
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
        public static KetQua layTheoMaNguoiDungVaMaDoiTuong_MangGiaTri(int maNguoiDung, string phamVi, int maDoiTuong = 0)
        {
            if (maNguoiDung > 0)
            {
                KetQua ketQua = QuyenDAO.layTheoMaNguoiDung_MaDoiTuong_ChuoiGiaTri(maNguoiDung, phamVi, maDoiTuong);

                if (ketQua.trangThai == 0)
                {
                    ketQua.ketQua = ketQua.ketQua.ToString().Split('|');
                }

                return ketQua;
            }

            return new KetQua() 
            {
                trangThai = 1
            };
        }

        public static KetQua kiemTraQuyenNguoiDung(int maNguoiDung, string giaTri, string phamVi, int maDoiTuong = 0)
        {
            if (maNguoiDung > 0)
            {
                KetQua ketQua = QuyenDAO.layTheoMaNguoiDungVaGiaTriVaMaDoiTuong_KiemTra(giaTri, maNguoiDung, phamVi, maDoiTuong);

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

            return new KetQua(0, false);
        }

        public static KetQua danhSachQuyenCuaNguoiDung(int maNguoiDung)
        {
            var ketQua = QuyenDAO.layTheoMaNguoiDung_ToanBoQuyen(maNguoiDung);
            if (ketQua.trangThai != 0)
            {
                return new KetQua(1, "Người dùng không có quyền");
            }
            var ds = ketQua.ketQua as List<QuyenDTO>;

            //Phạm vi - mã đối tượng - dánh sách quyền
            var duLieu = new Dictionary<string, Dictionary<int, List<string>>>();

            string phamVi;
            int maDoiTuong;
            foreach (var q in ds)
            {
                //Phạm vi
                phamVi = q.phamVi;
                if (!duLieu.ContainsKey(phamVi))
                {
                    duLieu.Add(phamVi, new Dictionary<int,List<string>>());
                }

                //Đối tượng
                maDoiTuong = LCTHelper.layGiaTri<int>(q.duLieuThem, "MaDoiTuong", 0);
                if (!duLieu[phamVi].ContainsKey(maDoiTuong))
                {
                    duLieu[phamVi].Add(maDoiTuong, new List<string>());
                }

                duLieu[phamVi][maDoiTuong].Add(q.giaTri);
            }

            return new KetQua(duLieu);
        }

        public static KetQua layTheoMaNguoiDungVaPhamViQuyenVaGiaTriQuyen_MangMaDoiTuong(string phamViNhomNguoiDung, int maNguoiDung, string phamVi, string giaTri)
        {
            var ketQua = NhomNguoiDungDAO.layTheoMaNguoiDungVaPhamViQuyenVaGiaTriQuyen_ChuoiMaDoiTuong(phamViNhomNguoiDung, maNguoiDung, phamVi, giaTri);
            if (ketQua.trangThai != 0)
            {
                return new KetQua();
            }

            var chuoiMaDoiTuong = ketQua.ketQua as string;
            if (chuoiMaDoiTuong == "")
            {
                return new KetQua();
            }

            return new KetQua(chuoiMaDoiTuong.Split('|').Select(int.Parse).ToArray());
        }

        public static KetQua layTheoMaNguoiDungVaPhamViQuyenVaGiaTriQuyen_ChuoiMaDoiTuong(string phamViNhomNguoiDung, int maNguoiDung, string phamVi, string giaTri)
        {
            var ketQua = NhomNguoiDungDAO.layTheoMaNguoiDungVaPhamViQuyenVaGiaTriQuyen_ChuoiMaDoiTuong(phamViNhomNguoiDung, maNguoiDung, phamVi, giaTri);
            if (ketQua.trangThai != 0)
            {
                return new KetQua(1);
            }

            var chuoiMaDoiTuong = ketQua.ketQua as string;
            if (chuoiMaDoiTuong == "")
            {
                return new KetQua(1);
            }

            return ketQua;
        }
    }
}
