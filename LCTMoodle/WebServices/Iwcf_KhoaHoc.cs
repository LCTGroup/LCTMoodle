﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LCTMoodle.WebServices.Client_Model;
using DTOLayer;
using System.Threading.Tasks;

namespace LCTMoodle.WebServices
{
    [ServiceContract]
    public interface Iwcf_KhoaHoc
    {
        [OperationContract]
        byte[] layHinhAnh(string ten);

        [OperationContract]
        clientmodel_HinhAnh layHinhAnhChiSo(int chiSo, string ten);

        [OperationContract]
        KhoaHocDTO layTheoMa(int ma);

        [OperationContract]
        List<KhoaHocDTO> lay();

        [OperationContract]
        List<clientmodel_KhoaHoc> layTheoMaChuDe(int maChuDe);

        [OperationContract]
        List<clientmodel_KhoaHoc> layKhoaHocThamGiaTheoMaNguoiDung(int maNguoiDung);

        [OperationContract]
        string layTheoMaNguoiDungVaTrangThai(int maNguoiDung, int trangThai);

        [OperationContract]
        List<clientmodel_KhoaHoc> layTheoTieuChi(int soKhoaHoc, string tieuChi);

        [OperationContract]
        List<clientmodel_KhoaHoc> timKiem(string tuKhoa);

        [OperationContract]
        List<KhoaHocDTO> timKiemTheoMaChuDe(int maChuDe, string tuKhoa);
    }
}
