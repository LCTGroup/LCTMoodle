using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;
using System.Reflection;

namespace DAOLayer
{
    public class BaiTapNopDAO : DAO<BaiTapNopDAO, BaiTapNopViewDTO>
    {
        public static BaiTapNopViewDTO gan(System.Data.SqlClient.SqlDataReader dong)
        {
            BaiTapNopViewDTO baiTapNop = new BaiTapNopViewDTO();

            int maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        baiTapNop.ma = layInt(dong, i);
                        break;
                    case "MaTapTin":
                        maTam = layInt(dong, i);

                        if (maTam != 0)
                        {
                            if (coLienKet("TapTin"))
                            {
                                KetQua ketQua = TapTinDAO.layTheoMa("BaiTapNop_TapTin", maTam);
                                
                                if (ketQua.trangThai == 0)
                                {
                                    baiTapNop.tapTin = ketQua.ketQua as TapTinViewDTO;
                                }
                            }
                            else
                            {
                                baiTapNop.tapTin = new TapTinViewDTO()
                                {
                                    ma = maTam
                                };
                            }
                        }
                        break;
                    case "DuongDan":
                        baiTapNop.duongDan = layString(dong, i);
                        break;
                    case "ThoiDiemTao":
                        baiTapNop.thoiDiemTao = layDateTime(dong, i);
                        break;
                    case "MaNguoiTao":
                        maTam = layInt(dong, i);

                        if (maTam != 0)
                        {
                            if (coLienKet("NguoiTao"))
                            {
                                KetQua ketQua = NguoiDungDAO.layTheoMa(maTam);

                                if (ketQua.trangThai == 0)
                                {
                                    baiTapNop.nguoiTao = ketQua.ketQua as NguoiDungViewDTO;
                                }
                            }
                            else
                            {
                                baiTapNop.nguoiTao = new NguoiDungViewDTO()
                                {
                                    ma = maTam
                                };
                            }
                        }
                        break;
                    case "MaBaiVietBaiTap":
                        
                        baiTapNop.baiVietBaiTap = new BaiVietBaiTapViewDTO()
                        {
                            ma = layInt(dong, i)
                        };
                        break;
                    default:
                        break;
                }
            }

            lienKet = null;
            return baiTapNop;
        }

        public static KetQua them(BaiTapNopDataDTO baiTapNop)
        {
            return layDong
            (
                "themBaiTapNop",
                new object[] 
                { 
                    baiTapNop.maTapTin,
                    baiTapNop.duongDan,
                    baiTapNop.maNguoiTao,
                    baiTapNop.maBaiVietBaiTap
                }
            );
        }
    }
}
