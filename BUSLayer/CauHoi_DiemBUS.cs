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
            #region Kiểm tra điều kiện

            if (!maNguoiTao.HasValue)
            {
                return new KetQua(4);
            }            

            var ketQua = CauHoiDAO.layTheoMa(maCauHoi);
            if (ketQua.trangThai != 0)
            {
                return new KetQua(3, "Câu hỏi không tồn tại");
            }

            CauHoiDTO cauHoi = ketQua.ketQua as CauHoiDTO;
            if (cauHoi.nguoiTao.ma == maNguoiTao)
            {
                return new KetQua()
                {
                    trangThai = 3,
                    ketQua = "Bạn không được cho điểm câu hỏi của mình"
                };
            }

            ketQua = NguoiDungBUS.layTheoMa(maNguoiTao.Value);
            if (ketQua.trangThai != 0)
            {
                return new KetQua(4, "Người dùng không tồn tại");
            }
            var nguoiVote = ketQua.ketQua as NguoiDungDTO;
            if (nguoiVote.diemHoiDap < 10)
            {
                return new KetQua(3, "Tham gia tạo hoặc trả lời " + (10 - nguoiVote.diemHoiDap).ToString() + " câu hỏi nữa, bạn mới đủ quyền cho điểm");
            }

            #endregion
            
            ketQua = CauHoi_DiemDAO.them(maCauHoi, maNguoiTao, diem);

            //Thêm hoạt động
            if (ketQua.trangThai == 0)
            {
                HoatDongBUS.them(new HoatDongDTO()
                    {
                        maNguoiTacDong = maNguoiTao,
                        loaiDoiTuongBiTacDong = "CH",
                        maDoiTuongBiTacDong = maCauHoi,
                        hanhDong = layDTO<HanhDongDTO>(diem ? 400 : 401),
                        duongDan = "/HoiDap/" + maCauHoi
                    });
            }

            return ketQua;
        }

        public static KetQua xoa(int maCauHoi, int? maNguoiVote)
        {
            #region Kiểm tra điều kiện

            if (!maNguoiVote.HasValue)
            {
                return new KetQua(4);
            }

            var ketQua = CauHoiBUS.layTheoMa(maCauHoi);
            if (ketQua.trangThai != 0)
            {
                return new KetQua(3, "Câu hỏi chưa được cho điểm");
            }
            var cauHoi = ketQua.ketQua as CauHoiDTO;

            if (cauHoi.nguoiTao.ma == maNguoiVote)
            {
                return new KetQua(3, "Bạn không được cập nhật điểm cho câu hỏi của mình");
            }

            ketQua = NguoiDungBUS.layTheoMa(maNguoiVote.Value);
            if (ketQua.trangThai != 0)
            {
                return new KetQua(4, "Người dùng không tồn tại");
            }
            var nguoiVote = ketQua.ketQua as NguoiDungDTO;
            if (nguoiVote.diemHoiDap < 10)
            {
                return new KetQua(3, "Tham gia tạo hoặc trả lời " + (10 - nguoiVote.diemHoiDap).ToString() + " câu hỏi nữa, bạn mới đủ quyền cho điểm");
            }

            #endregion

            ketQua = CauHoi_DiemDAO.xoaTheoMaCauHoiVaMaNguoiTao(maCauHoi, maNguoiVote);
            if (ketQua.trangThai == 0)
            {
                HoatDongBUS.them(new HoatDongDTO()
                {
                    maNguoiTacDong = maNguoiVote,
                    loaiDoiTuongBiTacDong = "CH",
                    maDoiTuongBiTacDong = maCauHoi,
                    hanhDong = layDTO<HanhDongDTO>(402),
                    duongDan = "/HoiDap/"
                });
            }

            return ketQua;
        }

        public static int trangThaiVoteCuaNguoiDungTrongCauHoi(int? maCauHoi, int? maNguoiDungHienTai)
        {            
            if (!maNguoiDungHienTai.HasValue)
            {
                return 0;
            }
            //0: chưa vote | 1: vote cộng | 2: vote trừ            
            int trangThaiVote = 0;

            KetQua ketQua = CauHoi_DiemDAO.layTheoMaCauHoiVaMaNguoiTao_Diem(maCauHoi, maNguoiDungHienTai);
            if (ketQua.trangThai == 0)
            {
                bool tam = (bool)ketQua.ketQua;
                trangThaiVote = tam == true ? 1 : -1;
            }
            return trangThaiVote;
        }
    }
}