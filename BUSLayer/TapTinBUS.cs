using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOLayer;
using DTOLayer;
using System.IO;

namespace BUSLayer
{
    public class TapTinBUS : BUS
    {
        private static string layDuongDanGoc()
        {
            return System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/");
        }

        public static string layDuongDan(string thuMuc, string ten)
        {
            return layDuongDanGoc() + thuMuc + ten;
        }

        public static KetQua layTapTin(int maTapTin)
        {
            return TapTinDAO.layTapTinTheoMa(maTapTin);

        }

        public static KetQua themTapTin(System.Web.HttpPostedFileBase tapTinLuu, string thuMuc)
        {
            //Thêm vào CSDL
            var ketQua = TapTinDAO.themTapTin(
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

                var duongDan = layDuongDanGoc() + tapTinDaLuu.thuMuc + tapTinDaLuu.ma.ToString() + "_" + tapTinDaLuu.ten;

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
