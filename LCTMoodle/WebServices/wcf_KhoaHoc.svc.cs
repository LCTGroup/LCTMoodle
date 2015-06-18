using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BUSLayer;
using DTOLayer;
using Helpers;

namespace LCTMoodle.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "wcf_KhoaHoc" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select wcf_KhoaHoc.svc or wcf_KhoaHoc.svc.cs at the Solution Explorer and start debugging.
    public class wcf_KhoaHoc : Iwcf_KhoaHoc
    {
        public List<KhoaHocDTO> layKhoaHoc()
        {
            KetQua ketQua = KhoaHocBUS.lay();
            List<KhoaHocDTO> lst_KhoaHoc = ketQua.ketQua as List<KhoaHocDTO>;
            return lst_KhoaHoc;
        }

        public KhoaHocDTO layKhoaHocTheoMa(int _Ma)
        {
            KetQua ketQua = KhoaHocBUS.layTheoMa(_Ma);
            KhoaHocDTO dto_KhoaHoc = ketQua.ketQua as KhoaHocDTO;
            return dto_KhoaHoc;
        }


        public string layLK(int _Ma)
        {
            //return TapTinHelper.layDuongDan("KhoaHoc_HinhDaiDien", "1 jpg");

            //KetQua ketQua = KhoaHocBUS.lay(new LienKet() { "HinhDaiDien" });
            //List<KhoaHocDTO> lstKhoaHoc = ketQua.ketQua as List<KhoaHocDTO>;
            //return lstKhoaHoc;

            KetQua ketQua = KhoaHocBUS.layTheoMa(_Ma, new LienKet() { "HinhDaiDien" });
            KhoaHocDTO dto_KhoaHoc = ketQua.ketQua as KhoaHocDTO;
            return dto_KhoaHoc.hinhDaiDien.ten;
        }
    }
}
