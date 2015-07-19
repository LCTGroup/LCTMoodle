using LCTMoodle.WebServices.Client_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LCTMoodle.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "Iwcf_KhoaHoc_DienDan" in both code and config file together.
    [ServiceContract]
    public interface Iwcf_KhoaHoc_DienDan
    {
        [OperationContract]
        byte[] layHinhAnh(string ten);

        [OperationContract]
        clientmodel_HinhAnh layHinhAnhChiSo(int chiSo, string ten);

        [OperationContract]
        clientmodel_KhoaHoc_DienDan layTheoMa(int maDienDan);

        [OperationContract]
        List<clientmodel_KhoaHoc_DienDan> layDanhSachTheoMaKhoaHoc(int maKhoaHoc);

        [OperationContract]
        clientmodel_ThongBao themBaiVietDienDan(int maNguoiThem, int maKhoaHoc , string tieuDe , string noiDung, byte[] tapTin = null, string tenTapTin = null, string contenttype = null);

        [OperationContract]
        clientmodel_ThongBao xoaBaiVietDienDan(int maNguoiXoa, int maBaiViet);

        [OperationContract]
        clientmodel_ThongBao suaBaiVietDienDan(int maNguoiSua, int maBaiViet, string tieuDe, string noiDung, byte[] tapTin = null, string tenTapTin = null, string contenttype = null);
    }
}
