using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOLayer;
using Data;

namespace DAOLayer
{
    public class TinNhanDAO : DAO<TinNhanDAO, TinNhanDTO>
    {
        public static TinNhanDTO gan(System.Data.SqlClient.SqlDataReader dong, LienKet lienKet = null)
        {
            TinNhanDTO tinNhan = new TinNhanDTO();

            int? maTam;
            for(int i=0; i < dong.FieldCount; i++)
            {
                switch(dong.GetName(i))
                {
                    case "Ma":
                        tinNhan.ma = layInt(dong, i); break;
                    case "MaNguoiGui":
                        maTam = layInt(dong, i);
                        
                        if (maTam.HasValue)
                        {
                            tinNhan.nguoiGui = LienKet.co(lienKet, "NguoiDung") ?
                                layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam, new LienKet() { "HinhDaiDien" })) :
                                new NguoiDungDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "MaNguoiNhan":
                        maTam = layInt(dong, i);
                        
                        if (maTam.HasValue)
                        {
                            tinNhan.nguoiNhan = LienKet.co(lienKet, "NguoiDung") ?
                                layDTO<NguoiDungDTO>(NguoiDungDAO.layTheoMa(maTam, new LienKet() { "HinhDaiDien" })) :
                                new NguoiDungDTO()
                                {
                                    ma = maTam
                                };
                        }
                        break;
                    case "NoiDung":
                        tinNhan.noiDung = layString(dong, i); break;
                    case "ThoiDiemGui":
                        tinNhan.thoiDiemGui = layDateTime(dong, i); break;
                    case "DaDoc":
                        tinNhan.daDoc = layBool(dong, i); break;
                    default:
                        break;
                };
            }

            return tinNhan;
        }  

        public static KetQua them(TinNhanDTO tinNhan, LienKet lienKet = null)
        {
            return layDong
                (
                    "themTinNhan",
                    new object[]
                    {
                        tinNhan.nguoiGui.ma,
                        tinNhan.nguoiNhan.ma,
                        tinNhan.noiDung
                    },
                    lienKet
                );
        }

        public static KetQua capNhatTheoMa_DaDoc(int maTinNhan, bool daDoc)
        {
            return khongTruyVan
                (
                    "capNhatTinNhanTheoMa_DaDoc",
                    new object[] 
                    {
                        maTinNhan,
                        daDoc
                    }
                );
        }

        public static KetQua capNhatTheoMaNguoiGuiVaMaNguoiNhan_DaDoc(int maNguoiGui, int maNguoiNhan, bool daDoc)
        {
            return khongTruyVan
                (
                    "capNhatTinNhanTheoMaNguoiGuiVaMaNguoiNhan_DaDoc",
                    new object[] 
                    {
                        maNguoiGui,
                        maNguoiNhan,
                        daDoc
                    }
                );
        }

        public static KetQua layTheoMaNguoiGuiVaMaNguoiNhan(int? maNguoiGui, int? maNguoiNhan, LienKet lienKet = null)
        {
            return layDanhSachDong
                (
                    "layTinNhanTheoMaNguoiGuiVaMaNguoiNhan",
                    new object[]
                    {
                        maNguoiGui,
                        maNguoiNhan
                    },
                    lienKet
                );
        }

        public static KetQua layDanhSachTinNhanTheoMaNguoiDung(int? maNguoiNhan, LienKet lienKet = null)
        {
            return layDanhSachDong
                (
                    "layTinNhanTheoMaNguoiDung",
                    new object[] 
                    {
                        maNguoiNhan
                    },
                    lienKet
                );
        }

        public static KetQua laySoLuongTinNhanChuaDocTheoMaNguoiNhan(int? maNguoiNhan)
        {
            return layGiaTri<int>
                (
                    "laySoLuongTinNhanChuaDocTheoMaNguoiNhan",
                    new object[]
                    {
                        maNguoiNhan
                    }
                );
        }

    }
}
