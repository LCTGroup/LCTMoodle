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
        public static KetQua lay(int maTapTin)
        {
            return TapTinDAO.layTheoMa(maTapTin);

        }

        public static KetQua them(System.Web.HttpPostedFileBase tapTinLuu, string thuMuc)
        {
            //Thêm vào CSDL
            var ketQua = TapTinDAO.them(
                new TapTinDataDTO()
                {
                    ten = tapTinLuu.FileName,
                    loai = tapTinLuu.ContentType,
                    thuMuc = thuMuc
                }
            );

            if (ketQua.trangThai == 0)
            {
                var tapTinDaLuu = ketQua.ketQua as TapTinViewDTO;

                var duongDan = TapTinHelper.layDuongDanGoc() + tapTinDaLuu.thuMuc + "/" + tapTinDaLuu.ma.ToString() + "_" + tapTinDaLuu.ten;

                //Lưu tập tin
                tapTinLuu.SaveAs(duongDan);

                return new KetQua()
                {
                    trangThai = 0,
                    ketQua = tapTinDaLuu
                };
            }
            else
            {
                return new KetQua()
                {
                    trangThai = 2,
                    ketQua = "Xử lý thêm tập tin thất bại (themTapTin - TapTinBUS)"
                };
            }
        }
    }
}
