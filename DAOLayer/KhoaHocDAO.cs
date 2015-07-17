using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class KhoaHocDAO : DAO<KhoaHocDAO, KhoaHocDTO>
    {
        public static KhoaHocDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            KhoaHocDTO khoaHoc = new KhoaHocDTO();

            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        khoaHoc.ma = layInt(dong, i);
                        break;
                    case "Ten":
                        khoaHoc.ten = layString(dong, i);
                        break;
                    case "MoTa":
                        khoaHoc.moTa = layString(dong, i);
                        break;
                    case "MaChuDe":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            khoaHoc.chuDe = LienKet.co(lienKet, "ChuDe") ?
                                layDTO<ChuDeDTO>(ChuDeDAO.layTheoMa(maTam, lienKet["ChuDe"])) :
                                new ChuDeDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "MaHinhDaiDien":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            khoaHoc.hinhDaiDien = LienKet.co(lienKet, "HinhDaiDien") ?
                                layDTO<TapTinDTO>(TapTinDAO.layTheoMa("KhoaHoc_HinhDaiDien", maTam.Value, lienKet["HinhDaiDien"])) :
                                new TapTinDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "MaNguoiTao":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            khoaHoc.nguoiTao = LienKet.co(lienKet, "NguoiTao") ?
                                layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam.Value, lienKet["NguoiTao"])) :
                                new NguoiDungDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "ThoiDiemTao":
                        khoaHoc.thoiDiemTao = layDateTime(dong, i);
                        break;
                    case "ThoiDiemHetHan":
                        khoaHoc.thoiDiemHetHan = layDateTime(dong, i);
                        break;
                    case "CanDangKy":
                        khoaHoc.canDangKy = layBool(dong, i);
                        break;
                    case "HanDangKy":
                        khoaHoc.hanDangKy = layDateTime(dong, i);
                        break;
                    case "PhiThamGia":
                        khoaHoc.phiThamGia = layInt(dong, i);
                        break;
                    case "CheDoRiengTu":
                        khoaHoc.cheDoRiengTu = layString(dong, i);
                        break;
                    case "CoBangDiem":
                        khoaHoc.coBangDiem = layBool(dong, i);
                        break;
                    case "CoBangDiemDanh":
                        khoaHoc.coBangDiemDanh = layBool(dong, i);
                        break;
                    case "CanDuyetBaiViet":
                        khoaHoc.canDuyetBaiViet = layBool(dong, i);
                        break;
                    case "SoLuongThanhVien":
                        khoaHoc.soLuongThanhVien = layInt(dong, i);
                        break;
                    default:
                        if (khoaHoc.duLieuThem == null)
                        {
                            khoaHoc.duLieuThem = new Dictionary<string, object>();
                        }
                        khoaHoc.duLieuThem.Add(dong.GetName(i), dong[i]);
                        break;
                }
            }

            if (LienKet.co(lienKet, "GiangVien"))
            {
                khoaHoc.danhSachGiangVien = layDanhSachDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMaDoiTuongNhomNguoiDungVaGiaTriNhomNguoiDung("KH", khoaHoc.ma, "GiangVien", lienKet["GiangVien"]));
            }

            return khoaHoc;
        }

        public static KetQua layTheoMa(int? ma, LienKet lienKet = null)
        {
            return layDong
                (
                    "layKhoaHocTheoMa",
                    new object[]
                    {
                        ma
                    },
                    lienKet
                );
        }

        public static KetQua them(KhoaHocDTO khoaHoc, LienKet lienKet = null)
        {
            return layDong
            (
                "themKhoaHoc",
                new object[] 
                {
                    khoaHoc.ten,
                    khoaHoc.moTa,
                    layMa(khoaHoc.hinhDaiDien),
                    layMa(khoaHoc.chuDe),
                    layMa(khoaHoc.nguoiTao),
                    khoaHoc.thoiDiemHetHan,
                    khoaHoc.canDangKy,
                    khoaHoc.hanDangKy,
                    khoaHoc.phiThamGia,
                    khoaHoc.cheDoRiengTu
                },
                lienKet
            );
        }

        public static KetQua capNhatTheoMa(int? ma, BangCapNhat bangCapNhat, LienKet lienKet = null)
        {
            return layDong
            (
                "capNhatKhoaHocTheoMa",
                new object[]
                {
                    ma,
                    bangCapNhat.bang
                },
                lienKet
            );
        }

        public static KetQua lay(LienKet lienKet = null)
        {
            return layDanhSachDong
                (
                    "layKhoaHoc",
                    new object[0],
                    lienKet
                );
        }

        public static KetQua lay_TimKiem(string tuKhoa, LienKet lienKet = null)
        {
            return layDanhSachDong
                (
                    "layKhoaHoc_TimKiem",
                    new object[] 
                    { 
                        tuKhoa
                    },
                    lienKet
                );
        }

        public static KetQua layTheoMaChuDe(int? maChuDe, LienKet lienKet = null)
        {
            return layDanhSachDong
                (
                    "layKhoaHocTheoMaChuDe",
                    new object[]
                    {
                        maChuDe
                    },
                    lienKet
                );
        }

        public static KetQua layTheoMaChuDe_TimKiem(int? maChuDe, string tuKhoa, LienKet lienKet = null)
        {
            return layDanhSachDong
                (
                    "layKhoaHocTheoMaChuDe_TimKiem",
                    new object[] 
                    { 
                        maChuDe,
                        tuKhoa
                    },
                    lienKet
                );
        }

        public static KetQua xoaTheoMa(int? ma)
        {
            return khongTruyVan
                (
                    "xoaKhoaHocTheoMa",
                    new object[]
                    {
                        ma
                    }
                );
        }

        public static KetQua lay_TimKiemPhanTrang(string where = null, string orderBy = null, int? trang = null, int? soDongMoiTrang = null, LienKet lienKet = null)
        {
            return layDanhSachDong
                (
                    "layKhoaHoc_TimKiemPhanTrang",
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
