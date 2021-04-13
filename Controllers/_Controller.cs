using Literary_Arts.Dao;
using Literary_Arts.Dao.Sysop;
using Literary_Arts.Models;
using Literary_Arts.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace Literary_Arts.Controllers
{
    public class _Controller : Controller
    {
        //設定初始結果，有權限對各種文章做操作時，才能改變result
        //目的:減少每一段controller的 update / delete 區塊的重複代碼
        public RtnResultModel upd_result = new RtnResultModel(false, SysSet.GetParamItemValue("SYS_MESSAGE", "not_auth_upd"));  //無編輯權限
        public RtnResultModel upd_reply_result = new RtnResultModel(false, SysSet.GetParamItemValue("SYS_MESSAGE", "not_auth_upd_reply")); //無編輯留言權限
        public RtnResultModel del_result = new RtnResultModel(false, SysSet.GetParamItemValue("SYS_MESSAGE", "not_auth_del"));  //無刪除權限
        public RtnResultModel del_reply_result = new RtnResultModel(false, SysSet.GetParamItemValue("SYS_MESSAGE", "not_auth_del_reply")); //無刪除留言權限

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            ViewBag.GetLoginUser = new Func<MemberUserModel>(GetLoginUser);
        }

        /// <summary>
        /// 取得登入使用者
        /// </summary>
        /// <returns></returns>
        protected MemberUserModel GetLoginUser()
        {
            return Session == null || Session["loginUser"] == null ? new MemberUserModel() : (MemberUserModel)((MemberUserModel)Session["loginUser"]).Clone();
        }



        /// <summary>
        /// 參數檔 set_type互相對照的中英文 轉換
        /// </summary>
        /// <param name="set_item"> 目前的set_item (大項)代號 </param>
        /// <param name="target_item"> 要轉換的 set_item (大項) 代號</param>
        /// <param name="set_value"> 目前的值 </param>
        /// <returns> set_value 轉換後的值 </returns>
        /// ex：　英轉中  - ReverseParam("LITERARY_CLASS_ENG", "LITERARY_CLASS_CHI", arti_class)
        public string ReverseParam(string set_item, string target_item, string set_value)
        {
            //取得目前item的value 
            string class_type = SysSet.GetParamItemType(set_item, set_value);

            //取得轉換後的value
            set_value = SysSet.GetParamItemValue(target_item, class_type);

            return set_value;
        }

    }
}