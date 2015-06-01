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
    public class BinhLuanBUS : BUS
    {
        public static KetQua kiemTra(BinhLuanDataDTO binhLuan)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (string.IsNullOrEmpty(binhLuan.noiDung))
            {
                loi.Add("Nội dung không được bỏ trống");
            }
            if (binhLuan.maNguoiTao == 0)
            {
                loi.Add("Người tạo không được bỏ trống");
            }
            if (binhLuan.maDoiTuong == 0)
            {
                loi.Add("Đối tượng bình luận không được bỏ trống");
            }
            if (!new string[] { "BaiVietDienDan" }.Contains(binhLuan.loaiDoiTuong))
            {
                loi.Add("Loại đối tượng không hợp lệ");
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
            var binhLuan = new BinhLuanDataDTO()
            {
                noiDung = layString(form, "NoiDung"),
                maTapTin = 0,
                maNguoiTao = 1,
                maDoiTuong = layInt(form, "DoiTuong"),
                loaiDoiTuong = layString(form, "Loai")
            };

            KetQua ketQua = kiemTra(binhLuan);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return BinhLuanDAO.them(binhLuan);
        }

        public static KetQua layTheoDoiTuong(string loaiDoiTuong, int maDoiTuong)
        {
            BinhLuanDAO.lienKet = new string[]
            {
                "NguoiTao"
            };
            return BinhLuanDAO.layTheoDoiTuong(loaiDoiTuong, maDoiTuong);
        }
    }
}
