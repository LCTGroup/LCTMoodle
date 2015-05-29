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
    public class ChuDeBUS : BUS
    {
        public static KetQua them(Dictionary<string, string> form)
        {
            KetQua ketQua = TapTinBUS.chuyen("ChuDe_HinhDaiDien", layInt(form, "HinhDaiDien"));

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            ChuDeDataDTO chuDe = new ChuDeDataDTO()
            {
                ten = layString(form, "Ten"),
                moTa = layString(form, "MoTa"),
                maHinhDaiDien = (ketQua.ketQua as TapTinViewDTO).ma,
                maChuDeCha = layInt(form, "ChuDeCha"),
                phamVi = layString(form, "PhamVi"),
                maNguoiTao = 1 //Để tạm
            };

            ketQua = chuDe.kiemTra();

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            return ChuDeDAO.them(chuDe);
        }

        public static KetQua layTheoMa(string phamVi, int ma)
        {
            //Lấy chủ đề
            KetQua ketQua = ChuDeDAO.layTheoMa(phamVi, ma);

            if (ketQua.trangThai != 0)
            {
                return ketQua;
            }

            ChuDeViewDTO chuDe = ketQua.ketQua as ChuDeViewDTO;

            layLienKet(ref chuDe);

            return new KetQua()
            {
                trangThai = 0,
                ketQua = chuDe
            };
        }

        private static void layLienKet(ref ChuDeViewDTO chuDe)
        {
            KetQua ketQua;
            //Lấy cha
            if (chuDe.chuDeCha != null)
            {
                ketQua = ChuDeDAO.layTheoMa(chuDe.phamVi, chuDe.chuDeCha.ma);
                if (ketQua.trangThai == 0)
                {
                    ChuDeViewDTO chuDeCha = ketQua.ketQua as ChuDeViewDTO;
                    layLienKet(ref chuDeCha);

                    chuDe.chuDeCha = chuDeCha;
                }
            }

            //Lấy con
            ketQua = ChuDeDAO.layTheoMaChuDeCha(chuDe.phamVi, chuDe.ma);
            if (ketQua.trangThai == 0)
            {
                chuDe.danhSachChuDeCon = ketQua.ketQua as List<ChuDeViewDTO>;
            }
        }
    }
}
