using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DTOLayer;
using LCTMoodle.WebServices.Client_Model;

namespace LCTMoodle.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "Iwcf_NguoiDung" in both code and config file together.
    [ServiceContract]
    public interface Iwcf_NguoiDung
    {
        [OperationContract]
        int kiemTraDangNhap(string _TenDN, string _MatKhau);

        [OperationContract]
        clientmodel_DangNhap themNguoiDung();

        [OperationContract]
        clientmodel_NguoiDung layNguoiDungTheoMa(int ma);

        [OperationContract]
        clientmodel_DangNhap dangNhap(string tenDN, string matKhau);

        [OperationContract]
        clientmodel_DangNhap dangKy(string tenDN, string matKhau, string email, string hoTen, DateTime ngaySinh, int maHinh);
    }
}
