using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOLayer;
using DTOLayer;
using System.IO;
using Helpers;
using Data;

namespace BUSLayer
{
    public class TapTinBUS : BUS
    {
        public static KetQua lay(string loai, int ma)
        {
            return TapTinDAO.layTheoMa(loai, ma);
        }

        public static KetQua them(byte[] duLieu, string tenTapTin, int maNguoiTao, string contenttype)
        {
            TapTinDTO tapTin = new TapTinDTO()
            {
                ten = tenTapTin,
                loai = contenttype,
                duoi = Path.GetExtension(tenTapTin),
                nguoiTao = layDTO<NguoiDungDTO>(maNguoiTao),
            };

            KetQua ketQua = TapTinDAO.them(tapTin);

            if(ketQua.trangThai == 0)
            {
                TapTinDTO tapTinDaLuu = ketQua.ketQua as TapTinDTO;

                string duongDan = TapTinHelper.layDuongDanGoc() + "Tam/" + tapTinDaLuu.ma + tapTinDaLuu.duoi;

                //Lưu tập tin
                try
                {
                    File.WriteAllBytes(duongDan, duLieu);
                }
                catch (Exception loi)
                {
                    return new KetQua()
                    {
                        trangThai = 2,
                        ketQua = loi.Message
                    };
                }
            }
            return ketQua;
        }

        public static KetQua them(System.Web.HttpPostedFileBase tapTinLuu)
        {
            TapTinDTO tapTin = new TapTinDTO()
            {
                ten = tapTinLuu.FileName,
                loai = tapTinLuu.ContentType,
                duoi = Path.GetExtension(tapTinLuu.FileName),
                nguoiTao = layDTO<NguoiDungDTO>(System.Web.HttpContext.Current.Session["NguoiDung"] as int?)
            };

            KetQua ketQua = TapTinDAO.them(tapTin);

            if (ketQua.trangThai == 0)
            {
                TapTinDTO tapTinDaLuu = ketQua.ketQua as TapTinDTO;

                string duongDan = TapTinHelper.layDuongDanGoc() + "Tam/" + tapTinDaLuu.ma + tapTinDaLuu.duoi;

                //Lưu tập tin
                try
                {
                    tapTinLuu.SaveAs(duongDan);
                }
                catch (Exception loi)
                {
                    return new KetQua()
                    {
                        trangThai = 2,
                        ketQua = loi.Message
                    };
                }
            }

            return ketQua;
        }

        public static KetQua chuyen(string loai, int? ma)
        {
            if (!ma.HasValue)
            {
                return new KetQua()
                {
                    trangThai = 0
                };
            }

            KetQua ketQua = TapTinDAO.chuyen(loai, ma);

            if (ketQua.trangThai == 0)
            {
                string duongDanGoc = TapTinHelper.layDuongDanGoc();
                TapTinDTO tapTin = ketQua.ketQua as TapTinDTO;

                try
                {
                    File.Move(
                        duongDanGoc + "Tam/" + ma + tapTin.duoi,
                        duongDanGoc + loai + "/" + tapTin.ma + tapTin.duoi
                    );
                }
                catch (Exception loi)
                {
                    return new KetQua()
                    {
                        trangThai = 2,
                        ketQua = loi.Message
                    };
                }
            }

            return ketQua;
        }

        public static KetQua doc(string duongDan)
        {
            if (System.IO.File.Exists(duongDan))
            {
                string duoi = Path.GetExtension(duongDan);
                switch (duoi.Substring(1))
                {
                    case "jpg":
                    case "jpeg":
                    case "png":
                        return new KetQua
                            (
                                new string[]
                                {
                                    "image"
                                }
                            );
                    case "txt":
                        return new KetQua
                            (
                                new string[]
                                {
                                    "text",
                                    TapTinHelper.doc_string(duongDan)
                                }
                            );
                    case "cs":
                    case "css":
                    case "js":
                    case "rb":
                    case "php":
                    case "cshtml":
                    case "cpp":
                    case "html":
                    case "haml":
                        return new KetQua
                            (
                                new string[]
                                {
                                    "code",
                                    TapTinHelper.doc_string(duongDan)
                                }
                            );
                    default:
                        return new KetQua(3, "Không hỗ trợ đọc tập tin này");
                }
            }
            else
            {
                return new KetQua(1, "Tập tin không tồn tại");
            }
        }
    }
}
