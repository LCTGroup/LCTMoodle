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
            return TapTinDAO.lay(loai, ma);
        }

        public static KetQua them(System.Web.HttpPostedFileBase tapTinLuu)
        {
            TapTinDataDTO tapTin = new TapTinDataDTO()
            {
                ten = tapTinLuu.FileName,
                loai = tapTinLuu.ContentType,
                duoi = Path.GetExtension(tapTinLuu.FileName)
            };

            KetQua ketQua = TapTinDAO.them(tapTin);

            if (ketQua.trangThai == 0)
            {
                TapTinViewDTO tapTinDaLuu = ketQua.ketQua as TapTinViewDTO;

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

        public static KetQua chuyen(string loai, int ma)
        {
            if (ma == 0)
            {
                return new KetQua()
                {
                    trangThai = 0,
                    ketQua = new TapTinViewDTO()
                    {
                        ma = 0
                    }
                };
            }

            KetQua ketQua = TapTinDAO.chuyen(loai, ma);

            if (ketQua.trangThai == 0)
            {
                string duongDanGoc = TapTinHelper.layDuongDanGoc();
                TapTinViewDTO tapTin = ketQua.ketQua as TapTinViewDTO;

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
