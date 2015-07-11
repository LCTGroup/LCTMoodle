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
    public class QuyenDAO : DAO<QuyenDAO, QuyenDTO>
    {
        public static QuyenDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet)
        {
            QuyenDTO quyen = new QuyenDTO();

            int? maTam;
            for (int i = 0; i < dong.FieldCount; i++)
            {
                switch (dong.GetName(i))
                {
                    case "Ma":
                        quyen.ma = layInt(dong, i);
                        break;
                    case "Ten":
                        quyen.ten = layString(dong, i);
                        break;
                    case "MoTa":
                        quyen.moTa = layString(dong, i);
                        break;
                    case "GiaTri":
                        quyen.giaTri = layString(dong, i);
                        break;
                    case "PhamVi":
                        quyen.phamVi = layString(dong, i);
                        break;
                    case "MaCha":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            quyen.cha = LienKet.co(lienKet, "Cha") ?
                                layDTO<QuyenDTO>(QuyenDAO.layTheoMa(maTam, lienKet["Cha"])) :
                                new QuyenDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "LaQuyenChung":
                        quyen.laQuyenChung = layBool(dong, i);
                        break;
                    default:
                        if (quyen.duLieuThem == null)
                        {
                            quyen.duLieuThem = new Dictionary<string, object>();
                        }
                        quyen.duLieuThem.Add(dong.GetName(i), dong[i]);
                        break;
                }
            }

            return quyen;
        }

        public static KetQua layTheoPhamViVaMaChaVaLaQuyenChung(string phamVi, int? maCha, bool laQuyenChung = false)
        {
            return layDanhSachDong
                (
                    "layQuyenTheoPhamViVaMaChaVaLaQuyenChung",
                    new object[]
                    {
                        phamVi,
                        maCha,
                        laQuyenChung
                    }
                );
        }

        public static KetQua layTheoMa(int? ma, LienKet lienKet = null)
        {
            return layDong
                (
                    "layQuyenTheoMa",
                    new object[]
                    {
                        ma
                    },
                    lienKet
                );
        }

        public static KetQua layTheoMaNguoiDung_MaDoiTuong_ChuoiGiaTri(int? maNguoiDung, string phamVi, int? maDoiTuong) 
        {
            return layGiaTri<string>
                (
                    "layQuyenTheoMaNguoiDungVaMaDoiTuong_ChuoiGiaTri",
                    new object[]
                    {
                        maNguoiDung,
                        phamVi,
                        maDoiTuong
                    }
                );
        }
        
        public static KetQua layTheoMaNguoiDungVaGiaTriVaMaDoiTuong_KiemTra(string giaTri, int? maNguoiDung, string phamVi, int? maDoiTuong)
        {
            return layGiaTri<bool>
                (
                    "layQuyenTheoMaNguoiDungVaGiaTriVaMaDoiTuong_KiemTra",
                    new object[]
                    {
                        maNguoiDung,
                        phamVi,
                        maDoiTuong,
                        giaTri
                    }
                );
        }

        public static KetQua layTheoMaNguoiDung_ToanBoQuyen(int? maNguoiDung, LienKet lienKet = null)
        {
            return layDanhSachDong
                (
                    "layQuyenTheoMaNguoiDung_ToanBoQuyen",
                    new object[]
                    {
                        maNguoiDung
                    },
                    lienKet
                );
        }
    }
}
