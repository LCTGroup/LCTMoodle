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
            KetQua ketQua = TapTinBUS.chuyen("BinhLuan_" + layString(form, "Loai") + "_TapTin", layInt(form, "TapTin"));

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            var binhLuan = new BinhLuanDataDTO()
            {
                noiDung = layString(form, "NoiDung"),
                maTapTin = (ketQua.ketQua as TapTinViewDTO).ma,
                maNguoiTao = (int)Session["NguoiDung"],
                maDoiTuong = layInt(form, "DoiTuong"),
                loaiDoiTuong = layString(form, "Loai")
            };

            ketQua = kiemTra(binhLuan);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return BinhLuanDAO.them(binhLuan, new LienKet()
            {
                "NguoiTao",
                "TapTin"
            });
        }

        public static KetQua layTheoDoiTuong(string loaiDoiTuong, int maDoiTuong)
        {
            return BinhLuanDAO.layTheoDoiTuong(loaiDoiTuong, maDoiTuong, new LienKet()
            {
                "NguoiTao",
                "TapTin"
            });
        }
    }
}
