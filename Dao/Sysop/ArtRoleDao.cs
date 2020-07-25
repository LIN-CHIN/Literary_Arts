using Literary_Arts.Models;
using Literary_Arts.Web_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts.Dao.Sysop
{
    public class ArtRoleDao : _Dao
    {
        public ArtRoleDao(MemberUserModel loginUser) : base(loginUser) { }


        /// <summary>
        /// 取得 會員帳號 擁有的所有角色代碼 
        /// </summary>
        /// <param name="MEM_ID"></param>
        /// <param name="ROLE_ID"></param>
        /// <returns></returns>
        public IList<ArtRoleModel> GetRoleByMemID(string MEM_ID) 
        {
            try 
            {
                strSql = @"SELECT MEM_ID
	                            , ROLE_ID
                            FROM MAP_MEMBER_ROLE
                            WHERE MEM_ID = @mem_id ";
                objParam = new
                {
                    mem_id = MEM_ID
                };

                return ExecuteQuery<ArtRoleModel>(strSql, objParam);

            }
            catch(Exception ex) 
            {
                LogSet.LogError(ex.ToString());
                return new List<ArtRoleModel>();
            }
            
        }

    }
}