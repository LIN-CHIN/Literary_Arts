using Literary_Arts.Models;
using Literary_Arts.Web_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts.Dao.Sysop
{
    public class SetFunctionDao : _Dao
    {
        /// <summary>
        /// 取得 SET_FUNCTION 資料
        /// </summary>
        /// <returns></returns>
        public IList<SetFunctionModel> GetFunctionData() {
            try
            {
                strSql = @"SELECT FUNCTION_ID
	                         ,FUNCTION_NAME
	                         ,FUNCTION_URL
	                         ,PARENT_ID
	                         ,SORT_ID
                             ,SORT_ITEM_ID 
                             ,CLICK_FN
	                         ,IS_STOP
                             ,IS_SPECIAL_FN
	                         ,CRT_DATE
	                         ,CRT_MEMID
	                         ,CRT_MEMNAME
	                         ,MDF_DATE
	                         ,MDF_MEMID
	                         ,MDF_MEMNAME
                       FROM SET_FUNCTION 
                       ORDER BY SORT_ID , SORT_ITEM_ID";
                return ExecuteQuery<SetFunctionModel>(strSql, objParam);
            }

            catch (Exception ex)
            {
                LogSet.LogError(ex.ToString());
                return new List<SetFunctionModel>();
            }
        }

    }
}