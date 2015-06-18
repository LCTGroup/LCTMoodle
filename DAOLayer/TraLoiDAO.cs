using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class TraLoiDAO : DAO<TraLoiDAO, TraLoiDTO>
    {
        public static TraLoiDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            TraLoiDTO traLoi = new TraLoiDTO();

            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        traLoi.ma = layInt(dong, i); break;
                    case "NoiDung":
                        traLoi.noiDung = layString(dong, i); break;
                    case "ThoiDiemTao":
                        traLoi.thoiDiemTao = layDateTime(dong, i); break;
                    case "ThoiDiemCapNhat":
                        traLoi.thoiDiemCapNhat = layDateTime(dong, i); break;
                    case "Duyet":
                        traLoi.duyet = layBool(dong, i); break;
                    case "MaNguoiTao":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            traLoi.nguoiTao = LienKet.co(lienKet, "MaNguoiTao") ?
                                layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam.Value)) :
                                new NguoiDungDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "MaCauHoi":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            traLoi.cauHoi = LienKet.co(lienKet, "MaCauHoi") ?
                                layDTO<CauHoiDTO>(CauHoiDAO.layTheoMa(maTam.Value)) :
                                new CauHoiDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    default:
                        break;
                }
            }

            return traLoi;
        }  
        
        public static KetQua them(TraLoiDTO traLoi, LienKet lienKet = null)
        {
            return layDong
            (
                "themTraLoi",
                new object[] 
                {
                    traLoi.noiDung,
                    layMa(traLoi.nguoiTao),
                    layMa(traLoi.cauHoi)
                },
                lienKet
            );
        }
        
        public static KetQua xoaTheoMa(int? ma, LienKet lienKet = null)
        {
            return khongTruyVan
            (
                "xoaTraLoiTheoMa",
                new object[]
                {
                    ma
                }
            );
        }
        
        public static KetQua layTheoMaCauHoi (int? maCauHoi, LienKet lienKet = null)
        {
            return layDanhSachDong
            (
                "layDanhSachTraLoiTheoMaCauHoi",
                new object[] 
                {
                    maCauHoi
                },
                lienKet
            );
        }
        
        public static KetQua layTheoMa(int? ma, LienKet lienKet = null)
        {
            return layDong
            (
                "layTraLoiTheoMa",
                new object[]
                {
                    ma
                },
                lienKet
            );
        }

        public static KetQua capNhatTheoMa(int? ma, BangCapNhat bangCapNhat, LienKet lienKet = null)
        {
            return layDong(
                "capNhatTraLoiTheoMa",
                new object[] {
                    ma, 
                    bangCapNhat.bang
                },
                lienKet
            );
        }

        /// <summary>
        /// Cập nhật duyệt trả lời theo mã
        /// </summary>
        /// <param name="ma">Mã trả lời</param>
        /// <param name="duyet">[true || false]</param>
        /// <returns>Trạng thái = 0: Duyệt thành công-- Trạng thái !=0: Duyệt thất bại</returns>
        public static KetQua capNhatDuyetTheoMa(int? ma, bool duyet)
        {
            return khongTruyVan
                (
                    "capNhatDuyetTraLoiTheoMa",
                    new object[]
                    {
                        ma,
                        duyet
                    }
                );
        }
    }
}
