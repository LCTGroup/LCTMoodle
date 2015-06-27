using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOLayer;
using DTOLayer;
using System.IO;
using Data;
using System.Web;

namespace BUSLayer
{
    public class BaiVietDienDanBUS : BUS
    {
        public static KetQua kiemTra(BaiVietDienDanDTO baiViet, string[] truong = null, bool kiemTra = true)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (coKiemTra("TieuDe", truong, kiemTra) && string.IsNullOrEmpty(baiViet.tieuDe))
            {
                loi.Add("Tiêu đề không được bỏ trống");
            }
            if (coKiemTra("NoiDung", truong, kiemTra) && string.IsNullOrEmpty(baiViet.noiDung))
            {
                loi.Add("Nội dung không được bỏ trống");
            }
            if (coKiemTra("NguoiTao", truong, kiemTra) && baiViet.nguoiTao == null)
            {
                loi.Add("Người tạo không được bỏ trống");
            }
            if (coKiemTra("KhoaHoc", truong, kiemTra) && baiViet.khoaHoc == null)
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

        public static void gan(ref BaiVietDienDanDTO baiViet, Form form)
        {
            if (baiViet == null)
            {
                baiViet = new BaiVietDienDanDTO();
            }

            foreach(string key in form.Keys.ToArray())
            {
                switch(key)
                {
                    case "TieuDe":
                        baiViet.tieuDe = form.layString(key);
                        break;
                    case "NoiDung":
                        baiViet.noiDung = form.layString(key);
                        break;
                    case "MaTapTin":
                        baiViet.tapTin = TapTinBUS.chuyen("BaiVietDienDan_TapTin", form.layInt(key)).ketQua as TapTinDTO;
                        break;
                    case "MaNguoiTao":
                        baiViet.nguoiTao = form.layDTO<NguoiDungDTO>(key);
                        break;
                    case "MaKhoaHoc":
                        baiViet.khoaHoc = form.layDTO<KhoaHocDTO>(key);
                        break;
                    default:
                        break;
                }
            }
        }

        public static BangCapNhat layBangCapNhat(BaiVietDienDanDTO baiViet, string[] keys)
        {
            BangCapNhat bangCapNhat = new BangCapNhat();
            foreach (string key in keys)
            {
                switch (key)
                {
                    case "TieuDe":
                        bangCapNhat.Add(key, baiViet.tieuDe, 2);
                        break;
                    case "NoiDung":
                        bangCapNhat.Add(key, baiViet.noiDung, 2);
                        break;
                    case "MaTapTin":
                        bangCapNhat.Add(key, baiViet.tapTin == null ? null : baiViet.tapTin.ma.ToString(), 1);
                        break;
                    default:
                        break;
                }
            }
            return bangCapNhat;
        }

        public static KetQua them(Form form)
        {
            BaiVietDienDanDTO baiViet = new BaiVietDienDanDTO();
            gan(ref baiViet, form);
            
            KetQua ketQua = kiemTra(baiViet);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return BaiVietDienDanDAO.them(baiViet, new LienKet()
            {
                "NguoiTao",
                "TapTin"
            });
        }

        public static KetQua layTheoMaKhoaHoc(int maKhoaHoc)
        {
            return BaiVietDienDanDAO.layTheoMaKhoaHoc(maKhoaHoc, new LienKet()
            {
                "NguoiTao",
                "TapTin"
            });
        }

        public static KetQua layTheoMa(int ma)
        {
            return BaiVietDienDanDAO.layTheoMa(ma);
        }

        public static KetQua xoaTheoMa(int ma)
        {
            return BaiVietDienDanDAO.xoaTheoMa(ma);
        }

        public static KetQua capNhatTheoMa(Form form)
        {
            int? maBaiViet = form.layInt("Ma");
            if (!maBaiViet.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 1
                };
            }

            KetQua ketQua = layTheoMa(maBaiViet.Value);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            BaiVietDienDanDTO baiViet = ketQua.ketQua as BaiVietDienDanDTO;

            gan(ref baiViet, form);

            ketQua = kiemTra(baiViet, form.Keys.ToArray());

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return BaiVietDienDanDAO.capNhatTheoMa(maBaiViet, layBangCapNhat(baiViet, form.Keys.ToArray()), new LienKet()
            {
                "NguoiTao",
                "TapTin"
            });
        }

        public static KetQua ghim(int ma, bool ghim)
        {
            #region Kiểm tra bài viết, lấy khóa học
            var ketQua = BaiVietDienDanDAO.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return new KetQua()
                {
                    trangThai = 1,
                    ketQua = "Bài viết không hợp lệ"
                };
            }

            var baiViet = ketQua.ketQua as BaiVietDienDanDTO;
            #endregion
            
            //Xóa ghim hiện tại nếu là ghim
            if (ghim)
            {
                BaiVietDienDanDAO.capNhatTheoMaKhoaHoc_XoaGhim(baiViet.khoaHoc.ma);
            }

            //Cập nhật ghim
            return BaiVietDienDanDAO.capNhatTheoMa_Ghim(ma, ghim);
        }
    }
}
