using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class KhoaHoc_NguoiDungDAO : DAO<KhoaHoc_NguoiDungDAO, KhoaHoc_NguoiDungDTO>
    {
        public static KhoaHoc_NguoiDungDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            KhoaHoc_NguoiDungDTO thanhVien = new KhoaHoc_NguoiDungDTO();
            
            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "MaNguoiDung":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            thanhVien.nguoiDung = LienKet.co(lienKet, "NguoiDung") ?
                                layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam)) :
                                new NguoiDungDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "MaKhoaHoc":
                        maTam = layInt(dong, i);
                        
                        if (maTam.HasValue)
                        {
                            thanhVien.khoaHoc = LienKet.co(lienKet, "KhoaHoc") ?
                                layDTO<KhoaHocDTO>(KhoaHocDAO.layTheoMa(maTam)) :
                                new KhoaHocDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "TrangThai":
                        thanhVien.trangThai = layInt(dong, i);
                        break;
                    case "ThoiDiemThamGia":
                        thanhVien.thoiDiemThamGia = layDateTime(dong, i);
                        break;
                    case "MaNguoiThem":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            thanhVien.nguoiThem = LienKet.co(lienKet, "NguoiThem") ?
                                layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam)) :
                                new NguoiDungDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "LaHocVien":
                        thanhVien.laHocVien = layBool(dong, i);
                        break;
                    default:
                        break;
                }
            }

            return thanhVien;
        }

        public static KetQua them(KhoaHoc_NguoiDungDTO thanhVien)
        {
            return khongTruyVan
                (
                    "themKhoaHoc_NguoiDung",
                    new object[]
                    {
                        layMa(thanhVien.khoaHoc),
                        layMa(thanhVien.nguoiDung),
                        thanhVien.trangThai,
                        layMa(thanhVien.nguoiThem)
                    }
                );
        }

        public static KetQua layTheoMaKhoaHocVaMaNguoiDung(int? maKhoaHoc, int? maNguoiDung, LienKet lienKet = null)
        {
            return layDong
                (
                    "layKhoaHoc_NguoiDungTheoMaKhoaHocVaMaNguoiDung",
                    new object[] 
                    { 
                        maKhoaHoc, maNguoiDung
                    },
                    lienKet
                );
        }

        public static KetQua xoaTheoMaKhoaHocVaMaNguoiDung(int? maKhoaHoc, int? maNguoiDung)
        {
            return khongTruyVan
                (
                    "xoaKhoaHoc_NguoiDungTheoMaKhoaHocVaMaNguoiDung",
                    new object[]
                    {
                        maKhoaHoc,
                        maNguoiDung
                    }
                );
        }

        public static KetQua layTheoMaKhoaHocVaTrangThai(int? maKhoaHoc, int? trangThai, LienKet lienKet = null)
        {
            return layDanhSachDong
                (
                    "layKhoaHoc_NguoiDungTheoMaKhoaHocVaTrangThai",
                    new object[]
                    {
                        maKhoaHoc,
                        trangThai
                    },
                    lienKet
                );
        }

        public static KetQua capNhatTheoMaKhoaHocVaMaNguoiDung_TrangThai(KhoaHoc_NguoiDungDTO thanhVien)
        {
            return khongTruyVan
                (
                    "capNhatKhoaHoc_NguoiDungTheoMaKhoaHocVaMaNguoiDung_TrangThai",
                    new object[]
                    {
                        layMa(thanhVien.khoaHoc),
                        layMa(thanhVien.nguoiDung),
                        thanhVien.trangThai,
                        layMa(thanhVien.nguoiThem)
                    }
                );
        }

        public static KetQua layTheoMaNguoiDung(int? maNguoiDung, LienKet lienKet = null)
        {
            return layDanhSachDong
                (
                    "layKhoaHoc_NguoiDungTheoMaNguoiDung",
                    new object[]
                    {
                        maNguoiDung
                    },
                    lienKet
                );
        }

        public static KetQua layTheoMaNguoiDungVaTrangThai(int? maNguoiDung, int? trangThai, LienKet lienKet = null)
        {
            return layDanhSachDong
                (
                    "layKhoaHoc_NguoiDungTheoMaNguoiDungVaTrangThai",
                    new object[]
                    {
                        maNguoiDung,
                        trangThai
                    },
                    lienKet
                );
        }

        public static KetQua capNhatTheoMaKhoaHocVaMaNguoiDung_LaHocVien(int? maKhoaHoc, int? maNguoiDung, bool laHocVien)
        {
            return khongTruyVan
                (
                    "capNhatKhoaHoc_NguoiDungTheoMaKhoaHocVaMaNguoiDung_LaHocVien",
                    new object[]
                    {
                        maKhoaHoc,
                        maNguoiDung,
                        laHocVien
                    }
                );
        }
    }
}
