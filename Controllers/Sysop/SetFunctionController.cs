using Literary_Arts.Dao.Sysop;
using Literary_Arts.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Literary_Arts.Controllers.Sysop
{
    public class SetFunctionController : _Controller
    {
        public JsonResult GetFunctionData(string MEM_ID)
        {
            using (SetFunctionDao dao = new SetFunctionDao()) 
            {
                return Json(dao.GetFunctionDataByID(HttpUtility.HtmlEncode(MEM_ID)));
            }
        }
    }
}