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
    public class TraLoi_DiemBUS : BUS
    {
        public static KetQua them(int maTraLoi, int? maNguoiTao, bool diem)
        {
            if (!maNguoiTao.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 4
                };
            }

            KetQua ketQua = TraLoiDAO.layTheoMa(maTraLoi);
            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }            

            TraLoiDTO traLoi = ketQua.ketQua as TraLoiDTO;
            if (traLoi.nguoiTao.ma == maNguoiTao)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không có quyền cho điểm trả lời của mình"
                };
            }
            
            return TraLoi_DiemDAO.them(maTraLoi, maNguoiTao, diem);
        }

        public static KetQua xoa(int? maTraLoi, int? maNguoiTao)
        {
            if (!maNguoiTao.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 4
                };
            }

            return TraLoi_DiemDAO.xoaTheoMaTraLoiVaMaNguoiTao(maTraLoi, maNguoiTao);
        }

        public static int trangThaiVoteCuaNguoiDungTrongTraLoi(int? maTraLoi, int? maNguoiDung)
        {
            if (!maNguoiDung.HasValue)
            {
                return 0;
            }
            //0: chưa vote | 1: vote cộng | 2: vote trừ            
            int trangThaiVote = 0;

            KetQua ketQua = TraLoi_DiemDAO.layTheoMaTraLoiVaMaNguoiTao_Diem(maTraLoi, maNguoiDung);
            if (ketQua.trangThai == 0)
            {
                bool tam = (bool)ketQua.ketQua;
                trangThaiVote = tam == true ? 1 : -1;
            }
            return trangThaiVote;
        }
    }
}