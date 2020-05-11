using Literary_Arts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts.Dao
{
    public class ArticleDao : _Dao
    {
        public IList<ArticleModel> Query()
        {
            strSql = @"SELECT *
                       FROM ARTICLE";

            return ExecuteQuery<ArticleModel>(strSql);

        }
    }
}