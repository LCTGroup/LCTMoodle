using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOLayer;
using DTOLayer;
using System.IO;
using Helpers;
using Data;

namespace BUSLayer
{
    public class TapTinBUS : BUS
    {
        public static KetQua lay(string loai, int ma)
        {
            return TapTinDAO.layTheoMa(loai, ma);
        }

        public static KetQua them(System.Web.HttpPostedFileBase tapTinLuu)
        {
            TapTinDTO tapTin = new TapTinDTO()
            {
                ten = tapTinLuu.FileName,
                loai = tapTinLuu.ContentType,
                duoi = Path.GetExtension(tapTinLuu.FileName),
                nguoiTao = layDTO<NguoiDungDTO>(Session["NguoiDung"] as int?)
            };

            KetQua ketQua = TapTinDAO.them(tapTin);

            if (ketQua.trangThai == 0)
            {
                TapTinDTO tapTinDaLuu = ketQua.ketQua as TapTinDTO;

                string duongDan = TapTinHelper.layDuongDanGoc() + "Tam/" + tapTinDaLuu.ma + tapTinDaLuu.duoi;

                //Lưu tập tin
                try
                {
                    tapTinLuu.SaveAs(duongDan);
                }
                catch (Exception loi)
                {
                    return new KetQua()
                    {
                        trangThai = 2,
                        ketQua = loi.Message
                    };
                }
            }

            return ketQua;
        }

        public static KetQua chuyen(string loai, int? ma)
        {
            if (!ma.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 0
                };
            }

            KetQua ketQua = TapTinDAO.chuyen(loai, ma);

            if (ketQua.trangThai == 0)
            {
                string duongDanGoc = TapTinHelper.layDuongDanGoc();
                TapTinDTO tapTin = ketQua.ketQua as TapTinDTO;

                try
                {
                    File.Move(
                        duongDanGoc + "Tam/" + ma + tapTin.duoi,
                        duongDanGoc + loai + "/" + tapTin.ma + tapTin.duoi
                    );
                }
                catch (Exception loi)
                {
                    return new KetQua()
                    {
                        trangThai = 2,
                        ketQua = loi.Message
                    };
                }
            }

            return ketQua;
        }
    }
}
