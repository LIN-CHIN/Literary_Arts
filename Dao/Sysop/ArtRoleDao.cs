using Literary_Arts.Models;
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
        public IList<ArtRoleDao> GetRoleByMemID(string MEM_ID, string ROLE_ID) 
        {
            strSql = @"SELECT mr.MEM_ID
	                        ,rf.FUNCTION_ID
                       FROM MAP_MEMBER_ROLE AS mr
	                       INNER JOIN MAP_ROLE_RIGHT AS rr
		                       ON mr.ROLE_ID = rr.ROLE_ID
	                       INNER JOIN MAP_RIGHT_FUNCTION AS rf
		                       ON rr.RIGHT_ID = rf.RIGHT_ID
                       WHERE mr.MEM_ID = 'chin' ";
            objParam = new
            {
                mem_id = MEM_ID,
                role_id = ROLE_ID
            };

            return ExecuteQuery<ArtRoleDao>(strSql, objParam);
        }

    }
}