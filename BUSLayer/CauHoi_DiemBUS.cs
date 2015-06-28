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
    public class CauHoi_DiemBUS : BUS
    {
        public static KetQua them(int maCauHoi, int? maNguoiTao, bool diem)
        {
            if (!maNguoiTao.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 4
                };
            }
            
            KetQua ketQua = CauHoiDAO.layTheoMa(maCauHoi);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }            

            CauHoiDTO cauHoi = ketQua.ketQua as CauHoiDTO;
            if (cauHoi.nguoiTao.ma == maNguoiTao)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền vote câu hỏi của mình"
                };
            }
            
            return CauHoi_DiemDAO.them(maCauHoi, maNguoiTao, diem);
        }

        public static KetQua xoa(int? maCauHoi, int? maNguoiTao)
        {
            if (!maNguoiTao.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 4
                };
            }
            
            return CauHoi_DiemDAO.xoaTheoMaCauHoiVaMaNguoiTao(maCauHoi, maNguoiTao);
        }
    }
}