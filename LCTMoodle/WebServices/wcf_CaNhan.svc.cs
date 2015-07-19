using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LCTMoodle.WebServices.Client_Model;
using DTOLayer;
using BUSLayer;

namespace LCTMoodle.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "wcf_CaNhan" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select wcf_CaNhan.svc or wcf_CaNhan.svc.cs at the Solution Explorer and start debugging.
    public class wcf_CaNhan : Iwcf_CaNhan
    {
        public string cauHoi(int maNguoiDung)
        {
            KetQua ketQuaDS = CauHoiBUS.layCauHoi_DanhSachMaLienQuan(maNguoiDung);
            KetQua ketQua = HoatDongBUS.lay_CuaDanhSachDoiTuong("CH", ketQuaDS.ketQua as string, 1, 5, new LienKet() { "HanhDong" });
            List<HoatDongDTO> dto_HoatDong = ketQua.ketQua as List<HoatDongDTO>;
            return dto_HoatDong[0].hanhDong.loiNhan;
        }
    }
}
