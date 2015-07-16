using LCTMoodle.WebServices.Client_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LCTMoodle.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "Iwcf_TinNhan" in both code and config file together.
    [ServiceContract]
    public interface Iwcf_TinNhan
    {
        [OperationContract]
        List<clientmodel_TinNhan> layDanhSachTheoMaNguoiDung(int maNguoiDung);

        [OperationContract]
        List<clientmodel_TinNhan> layTinNhanChoNguoiDung(int maNguoiGui, int maNguoiNhan);

        //[OperationContract]
        //clientmodel_ThongBao themTinNhan(int maNguoiGui, int maNguoiNhan, string noiDung);
    }
}
