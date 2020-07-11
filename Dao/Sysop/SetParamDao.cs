using Literary_Arts.Models;
using Literary_Arts.Models.Sysop;
using Literary_Arts.Web_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts.Dao.Sysop
{
    /// <summary>
    /// 使用參數檔 
    /// </summary>
    public class SetParamDao : _Dao
    {
        public SetParamDao(MemberUserModel loginUser = null) : base(loginUser) { }

        /// <summary>
        /// 取得參數檔 (細項) 值
        /// </summary>
        /// <param name="set_item"> 項目(大)代號 </param>
        /// <param name="set_type"> 項目(小)代號 </param>
        /// <returns></returns>
        public string GetParamItemValue(string set_item, string set_type)
        {
            try
            {
                string strSql = @"SELECT SET_VALUE			-- 根據代號對應的值
                                  FROM SET_PARAM_ITEM 
                                  WHERE IS_STOP = '0' 
                                        AND SET_ITEM = @set_item
                                        AND SET_TYPE = @set_type ";
                objParam = new
                {
                    set_item = set_item,
                    set_type = set_type
                };

                return ExecuteQuery<string>(strSql, objParam).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogSet.LogError(ex.ToString());
                return "";
            }
            
        }

        /// <summary>
        /// 取得 參數檔(細項) 的 type (代號)
        /// </summary>
        /// <param name="set_item"> 項目(大)代號 </param>
        /// <param name="set_value"> 項目(細項) 值 </param>
        public string GetParamItemType(string set_item, string set_value)
        {
            try
            {
                string strSql = @"SELECT SET_TYPE			-- 根據value對應type
                                  FROM SET_PARAM_ITEM 
                                  WHERE IS_STOP = '0' 
                                        AND SET_ITEM = @set_item
                                        AND SET_VALUE = @set_value ";
                objParam = new
                {
                    set_item = set_item,
                    set_value = set_value
                };

                return ExecuteQuery<string>(strSql, objParam).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogSet.LogError(ex.ToString());
                return "";
            }

        }

        /// <summary>
        /// 取得參數檔(細項) 的所有項目
        /// </summary>
        /// <param name="set_item"> 項目代號 </param>
        /// <returns></returns>
        public IList<SetParamItemModel> GetParamItem(string set_item) {
            try
            {
                string strSql = @"SELECT SET_ITEM		    -- 參數檔的代號
	                                    ,SET_TYPE			-- item 的代號
	                                    ,SET_VALUE			-- item 代號對應的值
	                                    ,MEMO				-- 說明註解
	                                    ,SORT				-- item 順序
	                                    ,IS_STOP			-- 是否停用 0 = 不停用
                                   FROM SET_PARAM_ITEM 
                                   WHERE IS_STOP = '0' AND SET_ITEM = 'LITERARY_CLASS_CHI'
                                   ORDER BY SORT ";
                objParam = new
                {
                    set_item = set_item
                };

                return ExecuteQuery<SetParamItemModel>(strSql, objParam);
            }
            catch (Exception ex)
            {
                LogSet.LogError(ex.ToString());
                return new List<SetParamItemModel>();
            }
        }


    }
}