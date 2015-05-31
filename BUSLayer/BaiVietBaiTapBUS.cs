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
    public class BaiVietBaiTapBUS : BUS
    {
        public static KetQua kiemTra(BaiVietBaiTapDataDTO baiViet)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (string.IsNullOrEmpty(baiViet.tieuDe))
            {
                loi.Add("Tiêu đề không được bỏ trống");
            }
            if (string.IsNullOrEmpty(baiViet.noiDung))
            {
                loi.Add("Nội dung không được bỏ trống");
            }
            if (baiViet.maNguoiTao == 0)
            {
                loi.Add("Người tạo không được bỏ trống");
            }
            if (baiViet.maKhoaHoc == 0)
            {
                loi.Add("Khóa học không được bỏ trống");
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
            //Chuyển tập tin
            KetQua ketQua = TapTinBUS.chuyen("BaiVietBaiTap_TapTin", layInt(form, "TapTin"));

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            //Tạo bài viết
            BaiVietBaiTapDataDTO baiViet = new BaiVietBaiTapDataDTO()
            {
                tieuDe = layString(form, "TieuDe"),
                noiDung = layString(form, "NoiDung"),
                maTapTin = (ketQua.ketQua as TapTinViewDTO).ma,
                thoiDiemHetHan = layDateTime_Full(form, "ThoiDiemHetHan_Ngay", "ThoiDiemHetHan_Gio"),
                maNguoiTao = 1, //Tạm
                maKhoaHoc = layInt(form, "KhoaHoc")
            };
            
            ketQua = kiemTra(baiViet);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            //Thêm bài viết
            ketQua = BaiVietBaiTapDAO.them(baiViet);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            BaiVietBaiTapViewDTO baiVietMoi = ketQua.ketQua as BaiVietBaiTapViewDTO;

            //Lấy tập tin
            if (baiVietMoi.tapTin != null)
            {
                ketQua = TapTinBUS.lay("BaiVietBaiTap_TapTin", baiVietMoi.tapTin.ma);

                if (ketQua.trangThai == 0)
                {
                    baiVietMoi.tapTin = ketQua.ketQua as TapTinViewDTO;
                }
            }

            return new KetQua()
            {
                trangThai = 0,
                ketQua = baiVietMoi
            };
        }

        public static KetQua layTheoMaKhoaHoc(int maKhoaHoc)
        {
            KetQua ketQua = BaiVietBaiTapDAO.layTheoMaKhoaHoc(maKhoaHoc);
            
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            List<BaiVietBaiTapViewDTO> danhSachBaiViet = ketQua.ketQua as List<BaiVietBaiTapViewDTO>;

            foreach (BaiVietBaiTapViewDTO baiViet in danhSachBaiViet)
            {
                if (baiViet.tapTin != null)
                {
                    ketQua = TapTinDAO.lay("BaiVietBaiTap_TapTin", baiViet.tapTin.ma);

                    if (ketQua.trangThai == 0)
                    {
                        baiViet.tapTin = ketQua.ketQua as TapTinViewDTO;
                    }
                }
            }

            return new KetQua()
            {
                trangThai = 0,
                ketQua = danhSachBaiViet
            };
        }
    }
}
