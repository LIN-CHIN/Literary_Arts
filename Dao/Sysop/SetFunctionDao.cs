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
        public SetFunctionDao(MemberUserModel loginUser = null) : base(loginUser) { }

        /// <summary>
        /// 根據mem_id取得 SET_FUNCTION 資料
        /// </summary>
        /// <returns></returns>
        public IList<SetFunctionModel> GetFunctionDataByID(string MEM_ID) {
            try
            {
                strSql = @"SELECT    mr.MEM_ID                      --會員帳號
	                                ,f.FUNCTION_ID                  --功能代號
	                                ,FUNCTION_NAME                  --功能中文名稱
	                                ,FUNCTION_URL                   --功能url
                                    ,f.CONTROLLER                     --Contrller
                                    ,f.ACTION                         --Action 
	                                ,PARENT_ID                      --父功能代號
	                                ,SORT_ID                        --父代號順序
                                    ,SORT_ITEM_ID                   --子代號順序(小項目)
                                    ,CLICK_FN                       --點擊事件function
	                                ,IS_STOP                        --是否停用
                                    ,IS_SPECIAL_FN                  --產生nav時是否需特殊處理 ex:小幫手(圖片)
                           FROM MAP_MEMBER_ROLE AS mr
	                           INNER JOIN MAP_ROLE_RIGHT AS rr
		                           ON mr.ROLE_ID = rr.ROLE_ID
	                           INNER JOIN MAP_RIGHT_FUNCTION AS rf
		                           ON rr.RIGHT_ID = rf.RIGHT_ID
	                           INNER JOIN SET_FUNCTION AS f
		                           ON rf.FUNCTION_ID = f.FUNCTION_ID
                           WHERE mr.MEM_ID = @mem_id ";

                //如果MEM_ID 為空 則抓 非會員角色
                if (string.IsNullOrEmpty(MEM_ID)) {
                    strSql += " OR mr.ROLE_ID = 'UserRole' ";
                }
                strSql += " ORDER BY SORT_ID , SORT_ITEM_ID  "; 
                

                objParam = new
                {
                    mem_id = MEM_ID
                };

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