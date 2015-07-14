using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUSLayer;
using DTOLayer;
using DAOLayer;
using Helpers;
using Data;

namespace LCTMoodle.Controllers
{
    public class TapTinController : LCTController
    {
        public ActionResult Lay(string loai, int ma)
        {
            KetQua ketQua = TapTinBUS.lay(loai, ma);

            if (ketQua.trangThai == 0)
            {
                TapTinDTO tapTin = ketQua.ketQua as TapTinDTO;

                string duongDan = TapTinHelper.layDuongDan(loai, tapTin.ma + tapTin.duoi);
                if (System.IO.File.Exists(duongDan))
                {
                    return File(duongDan, tapTin.loai, tapTin.ten);
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        public ActionResult LayHinh(string loai, int ma)
        {
            var ketQua = TapTinBUS.lay(loai, ma);

            var duoi = "file";
            var tapTin = ketQua.ketQua as TapTinDTO;
            string duongDan;

            if (ketQua.trangThai == 0)
            {
                duoi = tapTin.duoi.Substring(1);
                
                if (Array.IndexOf(new string[] { "png", "jpg", "jpeg" }, duoi) != -1)
                {
                    duongDan = TapTinHelper.layDuongDan(loai, tapTin.ma + tapTin.duoi);
                    if (System.IO.File.Exists(duongDan))
                    {
                        return File(duongDan, tapTin.loai, tapTin.ten);
                    }
                }
            }

            duoi = "file";

            duongDan = "~/Content/images/TapTin/" + duoi + ".png";
                    
            return File (
                duongDan,
                "image/png",
                duoi + ".png"
            );
        }

        public ActionResult Doc(string loai, int ma)
        {
            KetQua ketQua = TapTinBUS.lay(loai, ma);

            if (ketQua.trangThai == 0)
            {
                TapTinDTO tapTin = ketQua.ketQua as TapTinDTO;

                string duongDan = TapTinHelper.layDuongDan(loai, tapTin.ma + tapTin.duoi);
                if (System.IO.File.Exists(duongDan))
                {
                    switch (tapTin.duoi.Substring(1))
                    {
                        case "jpg":
                        case "jpeg":
                        case "png":
                            return Json
                                (
                                    new KetQua
                                    (
                                        new
                                        {
                                            loai = "hinh"
                                        }
                                    ), 
                                    JsonRequestBehavior.AllowGet
                                );
                        case "txt":
                            return Json
                                (
                                    new KetQua
                                    (
                                        new
                                        {
                                            loai = "text",
                                            giaTri = TapTinHelper.doc_string(duongDan)
                                        }
                                    ),
                                    JsonRequestBehavior.AllowGet
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
                            return Json
                                (
                                    new KetQua
                                    (
                                        new 
                                        {
                                            loai = "code",
                                            giaTri = TapTinHelper.doc_string(duongDan)
                                        }
                                    ),
                                    JsonRequestBehavior.AllowGet
                                );
                        default:
                            return Json(new KetQua(1, "Không hỗ trợ đọc tập tin này"), JsonRequestBehavior.AllowGet);
                    }
                }

                return Json(new KetQua(1), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(ketQua, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Popup(string loai, int ma)
        {
            ViewData["Loai"] = loai;
            ViewData["Ma"] = ma;
            return Json(new KetQua(0, renderPartialViewToString(ControllerContext, "TapTin/_Popup.cshtml", null, ViewData)), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult XuLyThem(FormCollection form)
        {
            KetQua ketQua = TapTinBUS.them(Request.Files["TapTin"]);

            return Json(ketQua);
        }
	}
}