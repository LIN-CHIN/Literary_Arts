using Literary_Arts.Models;
using Literary_Arts.Web_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts.Dao
{
    public class MemberDao : _Dao
    {
        public MemberDao (MemberUserModel loginUser) : base (loginUser) { }

        /// <summary>
        /// 登入檢查
        /// 檢查使用者是否為會員 (只檢查帳號是否存在和啟用)
        /// </summary>
        /// <returns></returns>
        public MemberUserModel CheckLoginUserID(string MEM_ID) 
        {
            try
            {
                strSql = @"SELECT   MEM_ID              --會員帳號
                                    ,LINE_ID            --Line帳號
                                    ,MEM_BIRTH          --會員生日
                                    ,MEM_NAME           --會員暱稱
                                    ,MEM_MAIL           --會員信箱
                                    ,MEM_GENDER         --性別
                                    ,MEM_NOTI           --會員通知
                                    ,MEM_ADDR           --會員地址
                                    ,IS_STOP            --是否停用
                           FROM ART_MEMBER
                           WHERE IS_STOP = '0'      
                                AND MEM_ID = @mem_id  ";
                objParam = new
                {
                    mem_id = MEM_ID
                };

                MemberUserModel model = ExecuteQuery<MemberUserModel>(strSql, objParam).FirstOrDefault();

                return model == null ? new MemberUserModel() : model ; 
            }
            catch (Exception ex) 
            {
                LogSet.LogError(ex.ToString());
                return new MemberUserModel();
            }
        }

        /// <summary>
        /// 檢查登入密碼是否正確
        /// </summary>
        /// <param name="MEM_PASS"></param>
        /// <returns></returns>
        public MemberUserModel CheckLoginUserPass(string MEM_ID, string MEM_PASS) 
        {
            try
            {
                strSql = @"SELECT   MEM_ID              --會員帳號
                                    ,LINE_ID            --Line帳號
                                    ,MEM_BIRTH          --會員生日
                                    ,MEM_NAME           --會員暱稱
                                    ,MEM_MAIL           --會員信箱
                                    ,MEM_GENDER         --性別
                                    ,MEM_NOTI           --會員通知
                                    ,MEM_ADDR           --會員地址
                                    ,IS_STOP            --是否停用
                           FROM ART_MEMBER
                           WHERE IS_STOP = '0'      
                                AND MEM_ID = @mem_id AND MEM_PASS = @mem_pass ";
                objParam = new
                {
                    mem_id = MEM_ID,
                    mem_pass = MEM_PASS
                };

                MemberUserModel model = ExecuteQuery<MemberUserModel>(strSql, objParam).FirstOrDefault();

                return model == null ? new MemberUserModel() : model;
            }
            catch (Exception ex)
            {
                LogSet.LogError(ex.ToString());
                return new MemberUserModel();
            }
        }
    }
}