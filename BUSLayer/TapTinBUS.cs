using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOLayer;
using DTOLayer;
using System.IO;
using Helpers;

namespace BUSLayer
{
    public class TapTinBUS : BUS
    {
        public static KetQua them(System.Web.HttpPostedFileBase tapTinLuu)
        {
            TapTinDataDTO tapTin = new TapTinDataDTO()
            {
                ten = tapTinLuu.FileName,
                loai = tapTinLuu.ContentType
            };

            KetQua ketQua = TapTinDAO.them(tapTin);

            if (ketQua.trangThai == 0)
            {
                TapTinViewDTO tapTinDaLuu = ketQua.ketQua as TapTinViewDTO;

                string duongDan = TapTinHelper.layDuongDanGoc() + "Tam/" + tapTinDaLuu.ma + Path.GetExtension(tapTinDaLuu.ten);

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

        public static KetQua chuyen(int maTapTin, string loai)
        {
            KetQua ketQua = TapTinDAO.chuyen(maTapTin, loai);

            if (ketQua.trangThai == 0)
            {
                string duongDanGoc = TapTinHelper.layDuongDanGoc();
                TapTinViewDTO tapTin = ketQua.ketQua as TapTinViewDTO;
                string duoi = Path.GetExtension(tapTin.ten);

                try
                {
                    File.Move(
                        duongDanGoc + "Tam/" + maTapTin + duoi,
                        duongDanGoc + loai + "/" + tapTin.ma + duoi
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

        public static KetQua lay(int maTapTin, string loai)
        {
            return TapTinDAO.lay(maTapTin, loai);
        }
    }
}
