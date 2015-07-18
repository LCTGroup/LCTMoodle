using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LCTMoodle.WebServices.Client_Model;
using DTOLayer;
using BUSLayer;
using Helpers;

namespace LCTMoodle.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "wcf_KhoaHoc_DienDan_BinhLuan" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select wcf_KhoaHoc_DienDan_BinhLuan.svc or wcf_KhoaHoc_DienDan_BinhLuan.svc.cs at the Solution Explorer and start debugging.
    public class wcf_KhoaHoc_DienDan_BinhLuan : Iwcf_KhoaHoc_DienDan_BinhLuan
    {

        public clientmodel_KhoaHoc_DienDan_BinhLuan layTheoMa(int maDienDan)
        {
            KetQua ketQua = BinhLuanBaiVietDienDanBUS.layTheoMa(maDienDan, new LienKet() { "TapTin", { "NguoiTao", new LienKet() { "HinhDaiDien" } } });
            BinhLuanBaiVietDienDanDTO dto_BinhLuan = ketQua.ketQua as BinhLuanBaiVietDienDanDTO;
            clientmodel_KhoaHoc_DienDan_BinhLuan cm_BinhLuan = new clientmodel_KhoaHoc_DienDan_BinhLuan();

            if(ketQua.trangThai == 0)
            {
                if(dto_BinhLuan.ma != null)
                {
                    cm_BinhLuan.ma = dto_BinhLuan.ma.Value;
                }


                if(dto_BinhLuan.diem != null)
                {
                    cm_BinhLuan.diem = dto_BinhLuan.diem.Value;
                }

                if(dto_BinhLuan.noiDung != null)
                {
                    cm_BinhLuan.noiDung = dto_BinhLuan.noiDung;
                }

                if(dto_BinhLuan.thoiDiemTao != null)
                {
                    cm_BinhLuan.ngayTao = dto_BinhLuan.thoiDiemTao.Value;
                }

                if(dto_BinhLuan.nguoiTao != null)
                {
                    if(dto_BinhLuan.nguoiTao.ma != null)
                    {
                        cm_BinhLuan.maNguoiTao = dto_BinhLuan.nguoiTao.ma.Value;
                    }

                    if(dto_BinhLuan.nguoiTao.tenLot != null)
                    {
                        cm_BinhLuan.tenNguoiTao = string.Format("{0} {1} {2}", dto_BinhLuan.nguoiTao.ho, dto_BinhLuan.nguoiTao.tenLot, dto_BinhLuan.nguoiTao.ten);
                    }
                    else
                    {
                        cm_BinhLuan.tenNguoiTao = string.Format("{0} {1}", dto_BinhLuan.nguoiTao.ho, dto_BinhLuan.nguoiTao.ten);
                    }

                    if(dto_BinhLuan.nguoiTao.tenTaiKhoan != null)
                    {
                        cm_BinhLuan.tenTKNguoiTao = dto_BinhLuan.nguoiTao.tenTaiKhoan;
                    }

                    if(dto_BinhLuan.nguoiTao.hinhDaiDien != null)
                    {
                        if(dto_BinhLuan.nguoiTao.hinhDaiDien.ma != null && dto_BinhLuan.nguoiTao.hinhDaiDien.duoi != null)
                        {
                            cm_BinhLuan.hinhAnh = dto_BinhLuan.nguoiTao.hinhDaiDien.ma.Value + dto_BinhLuan.nguoiTao.hinhDaiDien.duoi;
                        }
                    }
                }

                if(dto_BinhLuan.tapTin != null)
                {
                    if(dto_BinhLuan.tapTin.ma != null)
                    {
                        cm_BinhLuan.maTapTin = dto_BinhLuan.tapTin.ma.Value;
                    }

                    if(dto_BinhLuan.tapTin.ten != null)
                    {
                        cm_BinhLuan.tenTapTin = dto_BinhLuan.tapTin.ten;
                    }
                }
            }

            return cm_BinhLuan;
        }


        public List<clientmodel_KhoaHoc_DienDan_BinhLuan> layDanhSachTheoMaDienDan(int maDienDan)
        {
            throw new NotImplementedException();
        }
    }
}
