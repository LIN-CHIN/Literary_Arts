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

        /// <summary>
        /// Get SET_PARAMITEM
        /// </summary>
        /// <param name="setItem"></param>
        /// <returns></returns>
        //public SetParamItemModel ReadItem(string setItem)
        //{
        //    string strSql = @"SELECT set_item,
        //                             set_item_name,
        //                             memo,
        //                             editable
        //                      FROM   dbo.SET_PARAMITEM WHERE del_flg=0 AND set_item=@set_item";
        //    objParam = new
        //    {
        //        set_item = setItem
        //    };
        //    return ExecuteQuery<SetParamItemModel>(strSql, objParam).SingleOrDefault();
        //}

    }
}