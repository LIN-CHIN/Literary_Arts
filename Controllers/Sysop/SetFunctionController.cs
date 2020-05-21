﻿using Literary_Arts.Dao.Sysop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Literary_Arts.Controllers.Sysop
{
    public class SetFunctionController : Controller
    {
        public JsonResult GetFunctionDate()
        {
            using (SetFunctionDao dao = new SetFunctionDao()) 
            {
                return Json(dao.GetFunctionData());
            }
        }
    }
}