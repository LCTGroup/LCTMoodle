using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOLayer;
using DTOLayer;
using System.IO;
using Data;
using System.Data;

namespace BUSLayer
{
    public class CotDiem_NguoiDungBUS : BUS
    {
        public static DataTable toDataTable(List<CotDiem_NguoiDungDTO> dsDiem)
        {
            DataTable bang = new DataTable();
            bang.Columns.AddRange(new DataColumn[] {
                new DataColumn("MaCotDiem"),
                new DataColumn("MaNguoiDung"),
                new DataColumn("MaNguoiTao"),
                new DataColumn("Diem")
            });

            foreach (var diem in dsDiem)
            {
                bang.Rows.Add(new object[]
                {
                    layMa(diem.cotDiem),
                    layMa(diem.nguoiDung),
                    layMa(diem.nguoiTao),
                    diem.diem
                });
            }

            return bang;
        }

        /// <summary>
        /// Lấy bảng điểm
        /// </summary>
        /// <param name="maKhoaHoc">Mã khóa học</param>
        /// <returns>
        /// Mảng object[]
        /// [0]: Danh sách cột điểm ---
        /// [1]: Danh sách người dùng ---
        /// [2]: Danh sách điểm
        /// </returns>
        public static KetQua layTheoMaKhoaHoc(int maKhoaHoc)
        {
            #region Lấy danh sách cột điểm
            var ketQua = CotDiemDAO.layTheoMaKhoaHoc(maKhoaHoc);
            if (ketQua.trangThai != 0)
            {
                if (ketQua.trangThai == 1)
                {
                    ketQua.ketQua = "Chưa có danh sách cột điểm";
                }
                return ketQua;
            }
            var dsCotDiem = ketQua.ketQua as List<CotDiemDTO>;
            #endregion

            //Lấy chuỗi người dùng _ điểm
            //Tách người dùng _ điểm
            //Tách người dùng, điểm
            //Lấy người dùng
            #region Lấy danh sách học viên, điểm của học viên
            ketQua = CotDiem_NguoiDungDAO.layTheoMaKhoaHoc_ChuoiNguoiDung_Diem(maKhoaHoc);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }
            var mangNguoiDung_Diem = (ketQua.ketQua as string).Split('|');

            var dsMaNguoiDung = mangNguoiDung_Diem[0].Split(',');
            List<NguoiDungDTO> dsNguoiDung = new List<NguoiDungDTO>();
            foreach (var ma in dsMaNguoiDung)
            {
                dsNguoiDung.Add(layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(int.Parse(ma))));
            }
            #endregion

            return new KetQua()
            {
                trangThai = 0,
                ketQua = new object[] 
                { 
                    dsCotDiem,
                    dsNguoiDung,
                    mangNguoiDung_Diem[1].Split(',')
                }
            };
        }

        public static KetQua capNhat(List<CotDiem_NguoiDungDTO> dsDiem)
        {
            DataTable bang = toDataTable(dsDiem);
            return CotDiem_NguoiDungDAO.capNhat(bang);
        }
    }
}
