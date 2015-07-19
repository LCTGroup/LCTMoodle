using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LCTMoodle.WebServices.Client_Model;

namespace LCTMoodle.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "Iwcf_CaNhan" in both code and config file together.
    [ServiceContract]
    public interface Iwcf_CaNhan
    {
        [OperationContract]
        List<clientmodel_CaNhan> cauHoi(int maNguoiDung);

        [OperationContract]
        List<clientmodel_CaNhan> traLoi(int maNguoiDung);

        [OperationContract]
        List<clientmodel_KhoaHoc_BaiTap> khoaHoc(int maNguoiDung);
    }
}
