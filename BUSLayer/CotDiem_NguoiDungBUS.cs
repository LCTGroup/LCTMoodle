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
        public static KetQua kiemTra(CotDiem_NguoiDungDTO diem)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (diem.cotDiem == null)
            {
                loi.Add("Cột điểm không được bỏ trống");
            }
            if (diem.nguoiDung == null)
            {
                loi.Add("Người dùng không được bỏ trống");
            }
            if (!diem.diem.HasValue && (diem.diem.Value < 0 || 10 < diem.diem.Value))
            {
                loi.Add("Điểm không hợp lệ");
            }
            if (diem.nguoiTao == null)
            {
                loi.Add("Người chấm không được bỏ trống");
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

        public static DataTable toDataTable(List<CotDiem_NguoiDungDTO> dsDiem)
        {
            DataTable bang = new DataTable();
            bang.Columns.AddRange(new DataColumn[] {
                new DataColumn("MaCotDiem"),
                new DataColumn("MaNguoiDung"),
                new DataColumn("Diem"),
                new DataColumn("MaNguoiTao")
            });

            foreach (var diem in dsDiem)
            {
                bang.Rows.Add(new object[]
                {
                    layMa(diem.cotDiem),
                    layMa(diem.nguoiDung),
                    diem.diem.Value,
                    layMa(diem.nguoiTao)
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

        public static KetQua capNhat(List<dynamic> ds, int maNguoiSua)
        {
            #region Kiểm tra điều kiện
            if (ds.Count == 0)
            {
                return new KetQua()
                {
                    trangThai = 0
                };
            }
            #endregion

            List<CotDiem_NguoiDungDTO> dsDiem = new List<CotDiem_NguoiDungDTO>();
            CotDiemDTO cotDiem;
            CotDiem_NguoiDungDTO diem;
            KetQua ketQua;

            List<int> dsMaKhoaHoc = new List<int>();
            foreach(var item in ds)
            {
                diem = new CotDiem_NguoiDungDTO()
                {
                    cotDiem = layDTO<CotDiemDTO>(Convert.ToInt32(item.maCotDiem)),
                    nguoiDung = layDTO<NguoiDungDTO>(Convert.ToInt32(item.maNguoiDung)),
                    diem = Math.Round(Convert.ToDouble(item.diem), 2),
                    nguoiTao = layDTO<NguoiDungDTO>(maNguoiSua)
                };

                ketQua = kiemTra(diem);
                if (ketQua.trangThai != 0)
                {
                    return ketQua;
                }

                ketQua = CotDiemDAO.layTheoMa(diem.cotDiem.ma);
                if (ketQua.trangThai != 0)
                {
                    return new KetQua()
                    {
                        trangThai = 1,
                        ketQua = "Có cột điểm không tồn tại"
                    };
                }

                cotDiem = ketQua.ketQua as CotDiemDTO;
                if (!dsMaKhoaHoc.Exists(x => x == cotDiem.khoaHoc.ma.Value))
                {
                    dsMaKhoaHoc.Add(cotDiem.khoaHoc.ma.Value);
                    if (!coQuyen("QLBangDiem", "KH", cotDiem.khoaHoc.ma.Value))
                    {
                        return new KetQua()
                        {
                            trangThai = 3,
                            ketQua = "Bạn không có quyền sửa điểm"
                        };
                    }
                }

                dsDiem.Add(diem);
            }

            return CotDiem_NguoiDungDAO.capNhat_Nhieu(toDataTable(dsDiem));
        }

        public static KetQua chuyenDiemBaiTapNop(int maBaiTap, int maNguoiSua)
        {
            #region Kiểm tra điều kiện
            //Lấy bài tập
            var ketQua = BaiVietBaiTapDAO.layTheoMa(maBaiTap);
            if (ketQua.trangThai != 0)
            {
                return new KetQua(1, "Bài tập không tồn tại");
            }
            var baiTap = ketQua.ketQua as BaiVietBaiTapDTO;

            //Kiểm tra bài tập
            if (baiTap.loai != 1 && baiTap.loai != 2)
            {
                return new KetQua(3, "Bài tập không phù hợp");
            }

            //Lấy cột điểm
            ketQua = CotDiemDAO.layTheoLoaiDoiTuongVaMaDoiTuong("BaiTap", baiTap.ma);
            if (ketQua.trangThai != 0)
            {
                return new KetQua(1, "Không có cột điểm phù hợp");
            }
            var cotDiem = ketQua.ketQua as CotDiemDTO;

            //Kiểm tra quyền
            if (!coQuyen("QLBangDiem", "KH", baiTap.khoaHoc.ma.Value, maNguoiSua))
            {
                return new KetQua(3, "Bạn không có quyền chuyển điểm vào bảng điểm");
            }
            #endregion

            //Lấy danh sách bài nộp chưa chuyển điểm
            ketQua = BaiTapNopDAO.layTheoMaBaiVietBaiTapVaDaChuyenDiem(baiTap.ma, false);
            if (ketQua.trangThai == 1)
            {
                return new KetQua(0);
            }
            if (ketQua.trangThai != 0)
            {
                return new KetQua(1, "Không tồn tại bài nộp");
            }
            var dsBaiNop = ketQua.ketQua as List<BaiTapNopDTO>;

            //Tạo danh sách điểm từ danh sách bài nộp
            var dsDiem = new List<CotDiem_NguoiDungDTO>();
            CotDiem_NguoiDungDTO diem;
            var nguoiSua = layDTO<NguoiDungDTO>(maNguoiSua);

            foreach (var baiNop in dsBaiNop)
            {
                
                diem = new CotDiem_NguoiDungDTO()
                {
                    cotDiem = cotDiem,
                    nguoiDung = baiNop.nguoiTao,
                    diem = baiNop.diem,
                    nguoiTao = nguoiSua
                };

                ketQua = kiemTra(diem);
                if (ketQua.trangThai != 0)
                {
                    return ketQua;
                }

                dsDiem.Add(diem);
            }

            ketQua = CotDiem_NguoiDungDAO.capNhat_Nhieu(toDataTable(dsDiem));
            
            if (ketQua.trangThai == 0)
            {
                //Cập nhật trạng thái
                BaiTapNopDAO.capNhatTheoMaBaiVietBaiTap_DaChuyenDiem(baiTap.ma);
            }

            return ketQua;
        }
    }
}
