using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class CauHoiDAO : DAO<CauHoiDAO, CauHoiViewDTO>
    {
        public static CauHoiViewDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            CauHoiViewDTO cauHoi = new CauHoiViewDTO();

            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        cauHoi.ma = layInt(dong, i); break;
                    case "TieuDe":
                        cauHoi.tieuDe = layString(dong, i); break;
                    case "NoiDung":
                        cauHoi.noiDung = layString(dong, i); break;
                    case "ThoiDiemTao":
                        cauHoi.thoiDiemTao = layDateTime(dong, i); break;
                    case "MaNguoiTao":
                        int maTam = layInt(dong, i);
                        {
                            if (maTam != 0)
                            {
                                if (LienKet.co(lienKet, "NguoiDung"))
                                {
                                    KetQua ketQua = NguoiDungDAO.layTheoMa(maTam);
                                    if (ketQua.trangThai == 0)
                                    {
                                        cauHoi.nguoiTao = ketQua.ketQua as NguoiDungViewDTO;
                                    }                                    
                                }
                                else
                                {
                                    cauHoi.nguoiTao = new NguoiDungViewDTO()
                                    {
                                        ma = maTam
                                    };
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            return cauHoi;
        }  
        public static KetQua them(CauHoiDataDTO cauHoi)
        {
            return layGiaTri<int>
            (
                "themCauHoi",
                new object[] 
                {
                    cauHoi.tieuDe,
                    cauHoi.noiDung,
                    cauHoi.thoiDiemTao,
                    cauHoi.maNguoiTao
                }
            );
        }
        public static KetQua lay(int maCauHoi, LienKet lienKet = null)
        {
            return layDong
            (
                "layCauHoiTheoMa",
                new object[]
                {
                    maCauHoi
                },
                lienKet
            );
        }        
        public static KetQua layDanhSachCauHoi(LienKet lienKet = null)
        {
            return layDanhSachDong
            (
                "layToanBoCauHoi",
                new object[] 
                {
                },
                lienKet
            );
        }
    }
}
