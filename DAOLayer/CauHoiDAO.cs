using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class CauHoiDAO : DAO<CauHoiDAO, CauHoiDTO>
    {
        public static CauHoiDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            CauHoiDTO cauHoi = new CauHoiDTO();

            int? maTam;
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
                    case "ThoiDiemCapNhat":
                        cauHoi.thoiDiemCapNhat = layDateTime(dong, i); break;
                    case "MaNguoiTao":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            cauHoi.nguoiTao = LienKet.co(lienKet, "NguoiTao") ?
                                layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam.Value)) :
                                new NguoiDungDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "MaChuDe":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            cauHoi.chuDe = LienKet.co(lienKet, "ChuDe") ?
                                layDTO<ChuDeDTO>(ChuDeDAO.layTheoMa(maTam.Value)) :
                                new ChuDeDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "Diem":
                        cauHoi.diem = layInt(dong, i); break;
                    case "SoLuongTraLoi":
                        cauHoi.soLuongTraLoi = layInt(dong, i); break;
                    default:
                        break;
                }                
            }

            if (LienKet.co(lienKet, "TraLoi"))
            {
                cauHoi.danhSachTraLoi = layDanhSachDTO<TraLoiDTO>(TraLoiDAO.layTheoMaCauHoi(cauHoi.ma, lienKet["TraLoi"]));
            }

            return cauHoi;
        }  
        
        public static KetQua them(CauHoiDTO cauHoi)
        {
            return layGiaTri<int>
            (
                "themCauHoi",
                new object[] 
                {
                    cauHoi.tieuDe,
                    cauHoi.noiDung,
                    layMa(cauHoi.nguoiTao),
                    layMa(cauHoi.chuDe)
                }
            );
        }

        public static KetQua xoaTheoMa(int? ma)
        {
            return khongTruyVan
            (
                "xoaCauHoiTheoMa",
                new object[] 
                {
                    ma
                }
            );
        }
   
        public static KetQua capNhatTheoMa(int? ma, BangCapNhat bangCapNhat, LienKet lienKet = null)
        {
            return layDong
                (
                    "capNhatCauHoiTheoMa",
                    new object[] 
                    {
                        ma,
                        bangCapNhat.bang
                    },
                    lienKet
                );
        }
        
        public static KetQua capNhatTheoMa_Diem(int? maCauHoi, int? soDiem)
        {
            return layGiaTri<int>
                (
                    "capNhatCauHoi_Diem",
                    new object[]
                    {
                        maCauHoi,
                        soDiem
                    }
                );
        }

        public static KetQua layTheoMa(int? maCauHoi, LienKet lienKet = null)
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
        
        public static KetQua lay(int? soDong = null, LienKet lienKet = null)
        {
            return layDanhSachDong
            (
                "layCauHoi",
                new object[] 
                {
                    soDong
                },
                lienKet
            );
        }

        public static KetQua layTheoMaNguoiTao(int? maNguoiTao, LienKet lienKet = null)
        {
            return layDanhSachDong
                (
                    "layCauHoiTheoMaNguoiTao",
                    new object[]
                    {
                        maNguoiTao
                    },
                    lienKet
                );
        }

        public static KetQua layTheoMaChuDe_TimKiem(int? ma, string maChuDe, LienKet lienKet = null)
        {
            return layDanhSachDong(
                "layCauHoiTheoMaChuDe_TimKiem",
                new object[] 
                {
                    ma,
                    maChuDe
                },
                lienKet
            );
        }

        public static KetQua lay_TimKiem(string tuKhoa, LienKet lienKet = null)
        {
            return layDanhSachDong
                (
                    "layCauHoi_TimKiem",
                    new object[]
                    {
                        tuKhoa
                    },
                    lienKet
                );
        }
    }
}
