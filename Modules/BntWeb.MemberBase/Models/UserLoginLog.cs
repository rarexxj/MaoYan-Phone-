using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using BntWeb.Data;

namespace BntWeb.MemberBase.Models
{
    [Table(KeyGenerator.TablePrefix + "User_Login_Logs")]
    public class UserLoginLog
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 会员Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 登录Ip
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}