using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DAOLayer;
using DTOLayer;
using System.IO;
using Data;

namespace BUSLayer
{
    public class CauHoiBUS : BUS
    {
        public static void gan(ref CauHoiDTO cauHoi, Form form)
        {
            if (cauHoi == null)
            {
                cauHoi = new CauHoiDTO();
            }

            foreach (string key in form.Keys.ToArray())
            {
                switch (key)
                {
                    case "TieuDe":
                        cauHoi.tieuDe = form.layString(key);
                        break;
                    case "NoiDung":
                        cauHoi.noiDung = form.layString(key);
                        break;
                    case "MaNguoiTao":
                        cauHoi.nguoiTao = form.layDTO<NguoiDungDTO>(key);
                        break;
                    case "MaChuDe":
                        cauHoi.chuDe = form.layDTO<ChuDeDTO>(key);
                        break;
                    default:
                        break;
                }
            }
        }
        
        public static KetQua kiemTra(CauHoiDTO cauHoi, string[] truong = null, bool kiemTra = true)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi
            if (coKiemTra("TieuDe", truong, kiemTra) && string.IsNullOrWhiteSpace(cauHoi.tieuDe))
            {
                loi.Add("Tiêu đề không được bỏ trống");
            }
            if (coKiemTra("NoiDung", truong, kiemTra) && string.IsNullOrWhiteSpace(cauHoi.noiDung))
            {
                loi.Add("Nội dung không được bỏ trống");                
            }
            if (coKiemTra("MaNguoiTao", truong, kiemTra) && cauHoi.nguoiTao == null)
            {
                loi.Add("Chưa đăng nhập");
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
        
        public static BangCapNhat layBangCapNhat(CauHoiDTO cauHoi, string[] keys)
        {
            BangCapNhat bangCapNhat = new BangCapNhat();
            foreach (string key in keys)
            {
                switch (key)
                {
                    case "TieuDe":
                        bangCapNhat.Add("TieuDe", cauHoi.tieuDe, 2);
                        break;
                    case "NoiDung":
                        bangCapNhat.Add("NoiDung", cauHoi.noiDung, 2);
                        break;
                    case "MaChuDe":
                        bangCapNhat.Add("MaChuDe", layMa_String(cauHoi.chuDe), 1);
                        break;
                    default:
                        break;
                }
            }
            return bangCapNhat;
        }
       
        public static KetQua them(Form form)
        {
            #region Kiểm tra điều kiện

            int? maNguoiTao = form.layInt("MaNguoiTao");
            if (!maNguoiTao.HasValue)
            {
                return new KetQua(4);
            }

            #endregion

            CauHoiDTO cauHoi = new CauHoiDTO();
            gan(ref cauHoi, form);

            KetQua ketQua = kiemTra(cauHoi);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }
            
            return CauHoiDAO.them(cauHoi);
        }

        public static KetQua xoaTheoMa(int? ma, int? maNguoiXoa)
        {
            #region Kiểm tra điều kiện

            //Lấy câu hỏi
            var ketQua = CauHoiBUS.layTheoMa(ma);
            if (ketQua.trangThai != 0)
            {
                return new KetQua(1, "Câu hỏi không tồn tại");
            }

            var cauHoi = ketQua.ketQua as CauHoiDTO;
            if (cauHoi.nguoiTao.ma != maNguoiXoa && !coQuyen("XoaCauHoi", "HD", cauHoi.ma.Value, maNguoiXoa))
            {
                return new KetQua(3, "Bạn không có quyền xóa câu hỏi");
            }

            #endregion

            return CauHoiDAO.xoaTheoMa(ma);
        }

        public static KetQua capNhat(Form form, LienKet lienKet = null)
        {
            #region Kiểm tra điều kiện

            int? maNguoiSua = form.layInt("MaNguoiSua");
            int? maCauHoi = form.layInt("Ma");

            var ketQua = CauHoiDAO.layTheoMa(maCauHoi);
            if (ketQua.trangThai != 0)
            {
                return new KetQua(1, "Câu hỏi không tồn tại");
            }

            var cauHoi = ketQua.ketQua as CauHoiDTO;
            if (cauHoi.nguoiTao.ma != maNguoiSua && !coQuyen("SuaCauHoi", "HD", cauHoi.ma.Value, maNguoiSua))
            {
                return new KetQua(3, "Bạn không có quyền sửa câu hỏi này");
            }

            #endregion            

            gan(ref cauHoi, form);

            ketQua = kiemTra(cauHoi, form.Keys.ToArray());
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return CauHoiDAO.capNhatTheoMa(maCauHoi, layBangCapNhat(cauHoi, form.Keys.ToArray()), lienKet);
        }

        public static KetQua duyetHienThiCauHoi(int? maCauHoi, bool trangThai)
        {
            return CauHoiDAO.capNhatTheoMa_DuyetHienThi(maCauHoi, trangThai);
        }

        public static KetQua layTheoMa(int? ma, LienKet lienKet = null)
        {            
            return CauHoiDAO.layTheoMa(ma, lienKet);
        }

        public static KetQua layDanhSach(int? soDong = null, LienKet lienKet = null, string tieuChiHienThi = null)
        {
            return CauHoiDAO.lay(soDong, tieuChiHienThi, lienKet);
        }

        public static KetQua layTheoMaNguoiDung(int maNguoiTao, LienKet lienKet = null)
        {
            return CauHoiDAO.layTheoMaNguoiTao(maNguoiTao, lienKet);
        }

        public static KetQua layTheoMaChuDe_TimKiem(int? ma, string tuKhoa, LienKet lienKet = null, string cachHienThi = null)
        {
            return CauHoiDAO.layTheoMaChuDe_TimKiem(ma, tuKhoa, lienKet, cachHienThi);
        }

        public static KetQua lay_TimKiem(string tuKhoa, LienKet lienKet = null, string cachHienThi = null)
        {
            return CauHoiDAO.lay_TimKiem(tuKhoa, lienKet, cachHienThi);
        }

        public static KetQua timKiemPhanTrang(int trang, int soDongMoiTrang, string where = null, string orderBy = null, LienKet lienKet = null)
        {
            return CauHoiDAO.lay_TimKiemPhanTrang(where, orderBy, trang, soDongMoiTrang, lienKet);
        }

    }
}