using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class BinhLuanDAO : DAO
    {
        public static KetQua them(BinhLuanDataDTO binhLuan)
        {
            return layDong<BinhLuanViewDTO>(
                "themBinhLuan",
                new object[] { 
                    binhLuan.noiDung,
                    binhLuan.maTapTin,
                    binhLuan.maDoiTuong,
                    binhLuan.maNguoiTao,
                    binhLuan.loaiDoiTuong
                }
            );
        }

        public static KetQua layTheoDoiTuong(string loaiDoiTuong, int maDoiTuong)
        {
            return layDanhSachDong<BinhLuanViewDTO>(
                "layBinhLuanTheoDoiTuong",
                new object[] {
                    loaiDoiTuong,
                    maDoiTuong
                }
            );
        }

        public static KetQua xoaTheoMa(string loaiDoiTuong, int ma)
        {
            return khongTruyVan(
                "xoaBinhLuanTheoMa",
                new object[] 
                { 
                    loaiDoiTuong,
                    ma
                }
            );
        }
    }
}
