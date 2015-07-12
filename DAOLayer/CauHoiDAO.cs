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
                                layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam.Value, lienKet["NguoiTao"])) :
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
                                layDTO<ChuDeDTO>(ChuDeDAO.layTheoMa(maTam.Value, lienKet["ChuDe"])) :
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
                    case "DuyetHienThi":
                        cauHoi.DuyetHienThi = layBool(dong, i); break;
                    default:
                        if (cauHoi.duLieuThem == null)
                        {
                            cauHoi.duLieuThem = new Dictionary<string, object>();
                        }
                        cauHoi.duLieuThem.Add(dong.GetName(i), dong[i]);
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

        public static KetQua capNhatTheoMa_DuyetHienThi(int? maCauHoi, bool trangThai)
        {
            return khongTruyVan
                (
                    "capNhatCauHoiTheoMa_DuyetHienThi",
                    new object[]
                    {
                        maCauHoi,
                        trangThai
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

        public static KetQua layCauHoi_ChuaDuyet()
        {
            return layDanhSachDong
                (
                    "layCauHoi_ChuaDuyet",
                    new object[] { }
                );
        }
        
        public static KetQua lay(int? soDong = null, string tieuChiHienThi = null, LienKet lienKet = null)
        {
            return layDanhSachDong
            (
                "layCauHoi",
                new object[] 
                {
                    soDong,
                    tieuChiHienThi
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

        public static KetQua layTheoMaChuDe_TimKiem(int? ma, string maChuDe, LienKet lienKet = null, string cachHienThi = null)
        {
            return layDanhSachDong(
                "layCauHoiTheoMaChuDe_TimKiem",
                new object[] 
                {
                    ma,
                    maChuDe,
                    cachHienThi
                },
                lienKet
            );
        }

        public static KetQua lay_TimKiem(string tuKhoa, LienKet lienKet = null, string cachHienThi = null)
        {
            return layDanhSachDong
                (
                    "layCauHoi_TimKiem",
                    new object[]
                    {
                        tuKhoa,
                        cachHienThi
                    },
                    lienKet                    
                );
        }

        public static KetQua lay_TimKiemPhanTrang(string where = null, string orderBy = null, int? trang = null, int? soDongMoiTrang = null, LienKet lienKet = null)
        {
            return layDanhSachDong
                (
                    "layCauHoi_TimKiemPhanTrang",
                    new object[]
                    {
                        where,
                        orderBy,
                        trang,
                        soDongMoiTrang
                    },
                    lienKet
                );
        }
    }
}
