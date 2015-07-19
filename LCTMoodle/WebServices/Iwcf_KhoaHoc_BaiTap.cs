using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LCTMoodle.WebServices.Client_Model;

namespace LCTMoodle.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "Iwcf_KhoaHoc_BaiTap" in both code and config file together.
    [ServiceContract]
    public interface Iwcf_KhoaHoc_BaiTap
    {
        [OperationContract]
        byte[] layHinhAnh(string ten);

        [OperationContract]
        clientmodel_HinhAnh layHinhAnhChiSo(int chiSo, string ten);

        [OperationContract]
        clientmodel_KhoaHoc_BaiTap layTheoMa(int maBaiTap);

        [OperationContract]
        List<clientmodel_KhoaHoc_BaiTap> layDanhSachTheoMaKhoaHoc(int maKhoaHoc);

        [OperationContract]
        clientmodel_ThongBao_List themBaiTap(int maNguoiThem, int maKhoaHoc, string tieuDe, string noiDung, DateTime thoiHan, int loai = 0, int cachNop = 0, byte[] tapTin = null, string tenTapTin = null, string contenttype = null);
    }
}
