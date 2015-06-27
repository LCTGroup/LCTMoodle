using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class BinhLuanDAO : DAO<BinhLuanDAO, BinhLuanDTO>
    {
        public static BinhLuanDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            BinhLuanDTO binhLuan = new BinhLuanDTO();

            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        binhLuan.ma = layInt(dong, i);
                        break;
                    case "LoaiDoiTuong":
                        binhLuan.loaiDoiTuong = layString(dong, i);
                        break;
                    case "NoiDung":
                        binhLuan.noiDung = layString(dong, i);
                        break;
                    case "MaNguoiTao":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            binhLuan.nguoiTao = LienKet.co(lienKet, "NguoiTao") ?
                                layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam.Value)) :
                                new NguoiDungDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "MaDoiTuong":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            //Tạm - Bổ sung thêm truyền đối tượng DTO (xài <T>)
                            binhLuan.doiTuong = new DTO()
                            {
                                ma = layInt(dong, i)
                            };
                        }
                        break;
                    case "MaTapTin":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            binhLuan.tapTin = LienKet.co(lienKet, "TapTin") ?
                                layDTO<TapTinDTO>(TapTinDAO.layTheoMa("BinhLuan_" + binhLuan.loaiDoiTuong + "_TapTin", maTam.Value)) :
                                new TapTinDTO()
                                {
                                    ma = layInt(dong, i)
                                };
                        }
                        break;
                    case "ThoiDiemTao":
                        binhLuan.thoiDiemTao = layDateTime(dong, i);
                        break;
                    default:
                        break;
                }
            }

            return binhLuan;
        }

        public static KetQua them(BinhLuanDTO binhLuan, LienKet lienKet = null)
        {
            return layDong
            (
                "themBinhLuan",
                new object[] { 
                    binhLuan.noiDung,
                    layMa(binhLuan.tapTin),
                    layMa(binhLuan.doiTuong),
                    layMa(binhLuan.nguoiTao),
                    binhLuan.loaiDoiTuong
                },
                lienKet
            );
        }

        public static KetQua layTheoDoiTuong(string loaiDoiTuong, int? maDoiTuong, LienKet lienKet = null)
        {
            return layDanhSachDong
            (
                "layBinhLuanTheoDoiTuong",
                new object[] {
                    loaiDoiTuong,
                    maDoiTuong
                },
                lienKet
            );
        }

        public static KetQua xoaTheoMa(string loaiDoiTuong, int? ma)
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

        public static KetQua capNhatTheoMa(string loaiDoiTuong, int? ma, BangCapNhat bangCapNhat, LienKet lienKet = null)
        {
            return layDong
            (
                "capNhatBinhLuanTheoMa",
                new object[]
                {
                    loaiDoiTuong,
                    ma,
                    bangCapNhat.bang
                },
                lienKet
            );
        }

        public static KetQua layTheoMa(string loaiDoiTuong, int? ma, LienKet lienKet = null)
        {
            return layDong
                (
                    "layBinhLuanTheoMa",
                    new object[]
                    {
                        loaiDoiTuong,
                        ma
                    },
                    lienKet
                );
        }
    }
}
