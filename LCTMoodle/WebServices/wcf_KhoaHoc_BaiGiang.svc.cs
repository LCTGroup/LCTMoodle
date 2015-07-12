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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "wcf_KhoaHoc_BaiGiang" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select wcf_KhoaHoc_BaiGiang.svc or wcf_KhoaHoc_BaiGiang.svc.cs at the Solution Explorer and start debugging.
    public class wcf_KhoaHoc_BaiGiang : Iwcf_KhoaHoc_BaiGiang
    {
        public clientmodel_KhoaHoc_BaiGiang layTheoMa(int maBaiGiang)
        {
            KetQua ketQua = BaiVietBaiGiangBUS.layTheoMa(maBaiGiang, new LienKet { "NguoiTao" });
            BaiVietBaiGiangDTO dto_BaiGiang = ketQua.ketQua as BaiVietBaiGiangDTO;
            clientmodel_KhoaHoc_BaiGiang cm_BaiGiang = new clientmodel_KhoaHoc_BaiGiang();

            if(ketQua.trangThai == 0)
            {
                if (dto_BaiGiang.ma != null)
                {
                    cm_BaiGiang.ma = dto_BaiGiang.ma.Value;
                }

                if(dto_BaiGiang.nguoiTao.tenTaiKhoan != null)
                {
                    cm_BaiGiang.nguoiTao = dto_BaiGiang.nguoiTao.tenTaiKhoan;
                }

                if(dto_BaiGiang.tieuDe != null)
                {
                    cm_BaiGiang.tieuDe = dto_BaiGiang.tieuDe;
                }

                if(dto_BaiGiang.noiDung != null)
                {
                    cm_BaiGiang.noiDung = dto_BaiGiang.noiDung;
                }

                if(dto_BaiGiang.thoiDiemTao != null)
                {
                    cm_BaiGiang.ngayTao = dto_BaiGiang.thoiDiemTao.Value;
                }
            }
            return cm_BaiGiang;
        }
   
        /// <summary>
        /// Webservice lấy bài giàng theo mã khóa học
        /// </summary>
        /// <param name="maKhoaHoc"></param>
        /// <returns>List<clientmodel_KhoaHoc_BaiGiang></returns>
        public List<clientmodel_KhoaHoc_BaiGiang> layDanhSachTheoMaKhoaHoc(int maKhoaHoc)
        {
            KetQua ketQua = BaiVietBaiGiangBUS.layTheoMaKhoaHoc(maKhoaHoc, new LienKet{"NguoiTao"});
            List<clientmodel_KhoaHoc_BaiGiang> lst_BaiGiang = new List<clientmodel_KhoaHoc_BaiGiang>();

            if(ketQua.trangThai == 0)
            {
                foreach(var baiGiang in ketQua.ketQua as List<BaiVietBaiGiangDTO>)
                {
                    if(baiGiang.ma != null)
                    {
                        lst_BaiGiang.Add(new clientmodel_KhoaHoc_BaiGiang()
                        {
                            ma = baiGiang.ma.Value,
                        });
                    }

                    if(baiGiang.nguoiTao.tenTaiKhoan != null)
                    {
                        lst_BaiGiang[lst_BaiGiang.Count - 1].nguoiTao = baiGiang.nguoiTao.tenTaiKhoan;
                    }

                    if(baiGiang.tieuDe != null)
                    {
                        lst_BaiGiang[lst_BaiGiang.Count - 1].tieuDe = baiGiang.tieuDe;
                    }

                    if(baiGiang.tomTat != null)
                    {
                        lst_BaiGiang[lst_BaiGiang.Count - 1].tomTat = baiGiang.tomTat;
                    }

                    if(baiGiang.thoiDiemTao != null)
                    {
                        lst_BaiGiang[lst_BaiGiang.Count - 1].ngayTao = baiGiang.thoiDiemTao.Value;
                    }
                }
            }
            return lst_BaiGiang;
        }
    }
}
