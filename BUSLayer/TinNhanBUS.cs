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
using Helpers;

namespace BUSLayer
{
    public class TinNhanBUS : BUS
    {
        public static void gan(ref TinNhanDTO tinNhan, Form form)
        {
            if (tinNhan == null)
            {
                tinNhan = new TinNhanDTO();
            }
            foreach (var key in form.Keys.ToArray())
            {
                switch (key)
                {
                    case "MaNguoiGui":
                        tinNhan.nguoiGui = form.layDTO<NguoiDungDTO>(key);
                        break;
                    case "MaNguoiNhan":
                        tinNhan.nguoiNhan = form.layDTO<NguoiDungDTO>(key);
                        break;
                    case "NoiDung":
                        tinNhan.noiDung = form.layString(key);
                        break;                    
                    default:
                        break;
                }
            }
        }

        public static KetQua kiemTra(TinNhanDTO tinNhan, string[] truong = null, bool kiemTra = true)
        {
            List<string> loi = new List<string>();

            #region Bắt lỗi

            if (coKiemTra("MaNguoiGui", truong, kiemTra) && tinNhan.nguoiGui == null)
            {
                loi.Add("Người gửi không được bỏ trống");
            }
            if (coKiemTra("MaNguoiNhan", truong, kiemTra) && tinNhan.nguoiNhan == null)
            {
                loi.Add("Người nhận không được bỏ trống");
            }
            if (coKiemTra("NoiDung", truong, kiemTra) && string.IsNullOrWhiteSpace(tinNhan.noiDung))
            {
                loi.Add("Nội dung không được bỏ trống");
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

        #region Thêm

        public static KetQua them(Form form, LienKet lienKet = null)
        {
            int? maNguoiGui = form.layInt("MaNguoiGui");
            if (!maNguoiGui.HasValue)
            {
                return new KetQua(3, "Bạn chưa đăng nhập");
            }

            TinNhanDTO tinNhan = new TinNhanDTO();            
            gan(ref tinNhan, form);

            return TinNhanDAO.them(tinNhan, lienKet);
        }

	    #endregion
        
        #region Xóa

	    #endregion

        #region Sửa

		/// <summary>
		 /// Cập nhật trạng thái đã đọc tin nhắn
		 /// </summary>
		 /// <param name="maTinNhan">Mã tin nhắn đã đọc</param>
		 /// <param name="daDoc">Trạng thái
         /// true: đã đọc | false: chưa đọc
         /// </param>
		 /// <returns>KetQua</returns>
        public static KetQua capNhatTrangThaiDaDocTinNhan(int maTinNhan, bool daDoc)
        {
            return TinNhanDAO.capNhatTheoMa_DaDoc(maTinNhan, daDoc);
        }

        /// <summary>
        /// Cập nhật tin nhắn theo mã người gửi và mã người nhận
        /// </summary>
        /// <param name="maNguoiGui">Mã người gửi</param>
        /// <param name="maNguoiNhan">Mã người nhận</param>
        /// <param name="daDoc">Trạng thái
        /// true: đã đọc | false: chưa đọc
        /// </param>
        /// <returns>KetQua</returns>
        public static KetQua capNhatTrangThaiDaDocTinNhan(int maNguoiGui, int maNguoiNhan, bool daDoc)
        {
            return TinNhanDAO.capNhatTheoMaNguoiGuiVaMaNguoiNhan_DaDoc(maNguoiGui, maNguoiNhan, daDoc);
        }

	    #endregion

        #region Lấy

        public static KetQua lay(int maNguoiGui, int maNguoiNhan, LienKet lienKet = null)
        {
            return TinNhanDAO.layTheoMaNguoiGuiVaMaNguoiNhan(maNguoiGui, maNguoiNhan, lienKet);
        }

        public static KetQua layDanhSachTinNhanTheoMaNguoiDung(int maNguoiDung, LienKet lienKet = null)
        {
            return TinNhanDAO.layDanhSachTinNhanTheoMaNguoiDung(maNguoiDung, lienKet);
        }

        public static KetQua laySoLuongTinNhanChuaDocTheoMaNguoiNhan(int maNguoiNhan)
        {
            return TinNhanDAO.laySoLuongTinNhanChuaDocTheoMaNguoiNhan(maNguoiNhan);
        }

        #endregion
    }
}