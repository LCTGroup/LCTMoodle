using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LCTMoodle.Controllers
{
    public class LCTController : Controller
    {
        [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult LayContent(string tapTin, string dinhDang, string thuMuc = null)
        {
            string loaiTapTin;

            switch (dinhDang)
            {
                case "png":
                    thuMuc = "images/" + thuMuc;
                    loaiTapTin = "image/png";
                    break;

                case "jpg":
                case "jpeg":
                    thuMuc = "images/" + thuMuc;
                    loaiTapTin = "image/jpeg";
                    break;

                case "js":
                    thuMuc = "scripts/" + thuMuc;
                    loaiTapTin = "text/javascript";
                    break;

                case "css":
                    thuMuc = "styles/" + thuMuc;
                    loaiTapTin = "text/css";
                    break;

                case "woff":
                case "ttf":
                case "eot":
                case "otf":
                case "svg":
                    thuMuc = "fonts/" + thuMuc;
                    loaiTapTin = "application/octet-stream";
                    break;

                default:
                    loaiTapTin = "text/plain";
                    break;
            }

            string duongDan = string.Format (
                "{0}/{1}/{2}.{3}",
                Server.MapPath("~/Content/"),
                thuMuc,
                tapTin,
                dinhDang
            );

            if (System.IO.File.Exists(duongDan)) {
                return File (
                    duongDan,
                    loaiTapTin,
                    tapTin + "." + dinhDang
                );
            }

            return null;

        }

        #region Helper
        [NonAction]
        public Dictionary<string, string> chuyenDuLieuForm(FormCollection formCollection)
        {
            return formCollection.AllKeys.ToDictionary(k => k, v => formCollection[v]);
        }

        [NonAction]
        public string renderPartialViewToString(ControllerContext controllerContextontrollerContext,
            string partialViewName, object model = null, ViewDataDictionary viewData = null, TempDataDictionary tempData = null)
        {
            partialViewName = "~/Views/" + partialViewName;

            if (viewData == null)
            {
                viewData = new ViewDataDictionary();
            }
            if (tempData == null)
            {
                tempData = new TempDataDictionary();
            }

            viewData.Model = model;

            var sw = new StringWriter();
            var viewResult = ViewEngines.Engines.FindPartialView(controllerContextontrollerContext, partialViewName);
            var viewContext = new ViewContext(controllerContextontrollerContext, viewResult.View, viewData, tempData, sw);
            viewResult.View.Render(viewContext, sw);

            var s = sw.GetStringBuilder().ToString();
            return s;
        } 
        #endregion
	}
}