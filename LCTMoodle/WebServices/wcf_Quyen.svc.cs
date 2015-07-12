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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "wcf_Quyen" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select wcf_Quyen.svc or wcf_Quyen.svc.cs at the Solution Explorer and start debugging.
    public class wcf_Quyen : Iwcf_Quyen
    {
        public string[] layQuyenNguoiDungTheoDoiTuong(int maNguoiDung, string phamVi, int maDoiTuong)
        {
            KetQua ketQua = QuyenBUS.layTheoMaNguoiDungVaMaDoiTuong_MangGiaTri(maNguoiDung, phamVi, maDoiTuong);
            string[] lst_Quyen = null;

            if(ketQua.trangThai == 0)
            {
                lst_Quyen = ketQua.ketQua as string[];
            }

            return lst_Quyen;
        }
    }
}
