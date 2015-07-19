using Helpers;
using LCTMoodle.WebServices.Client_Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LCTMoodle.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "Iwcf_KhoaHoc_BaiTap_Nop" in both code and config file together.
    [ServiceContract]
    public interface Iwcf_KhoaHoc_BaiTap_Nop
    {
        [OperationContract]
        byte[] layHinhAnh(string ten);

        [OperationContract]
        clientmodel_HinhAnh layHinhAnhChiSo(int chiSo, string ten);

        [OperationContract]
        List<clientmodel_KhoaHoc_BaiTap_Nop> layDanhSachTheoMaBaiTap(int maBaiTap);
    }
}
