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
        public string renderPartialViewToString(ControllerContext controllerContextontrollerContext, 
            string partialViewName, object model = null , ViewDataDictionary viewData = null, TempDataDictionary tempData = null)
        {
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
	}
}