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
                    case "NguoiDung":
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
                    case "KhoaHoc":
                        maTam = layInt(dong, i);
                        
                        if (maTam.HasValue)
                        {
                            thanhVien.khoaHoc = LienKet.co(lienKet, "NguoiDung") ?
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
                    case "NguoiThem":
                        maTam = layInt(dong, i);

                        if (maTam.HasValue)
                        {
                            thanhVien.nguoiDung = LienKet.co(lienKet, "NguoiThem") ?
                                layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam)) :
                                new NguoiDungDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    default:
                        break;
                }
            }

            return thanhVien;
        }
    }
}
