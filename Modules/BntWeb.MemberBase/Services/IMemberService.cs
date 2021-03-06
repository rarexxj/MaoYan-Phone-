﻿/* 
    ======================================================================== 
        File name：        IMemberService
        Module:                
        Author：            Luce
        Create Time：    2016/6/7 14:30:49
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BntWeb.FileSystems.Media;
using BntWeb.MemberBase.Models;
using BntWeb.Security.Identity;
using Microsoft.AspNet.Identity;

namespace BntWeb.MemberBase.Services
{
    public interface IMemberService : IDependency
    {
        /// <summary>
        /// 创建一个会员
        /// </summary>
        /// <param name="member"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        IdentityResult CreateMember(Member member, string password);

        /// <summary>
        /// 更新会员信息
        /// </summary>
        /// <param name="oldMember"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        IdentityResult UpdateMember(Member oldMember, string oldPassword = null, string newPassword = null);

        /// <summary>
        /// 根据Name获取用户完整信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        User FindUserByName(string name);


        /// <summary>
        /// 获取会员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Member FindMemberById(string id);

        /// <summary>
        /// 根据邀请码找会员
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Member FindMemberByInvitationCode(string code);

        /// <summary>
        /// 判断昵称是否存在
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        bool NickNameIsExists(string nickName);
        
        /// <summary>
        /// 获取会员
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Member FindMember(User user);
        /// <summary>
        /// 根据手机号或者用户名获取用户信息
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        User FindUserByNameOrPhone(string phoneNumber);
        /// <summary>
        /// 分页获取会员
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="expression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="isDesc"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<Member> GetListPaged<TKey>(int pageIndex, int pageSize, Expression<Func<Member, bool>> expression,
        Expression<Func<Member, TKey>> orderByExpression, bool isDesc, out int totalCount);
        /// <summary>
        /// 通过手机获得用户信息
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        User FindUserByPhone(string phone);
        /// <summary>
        /// 解锁和锁定会员
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        IdentityResult SetLockoutEnabled(string userId, bool enabled);

        /// <summary>
        /// 删除会员
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IdentityResult> Delete(Member user);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newPassword"></param>
        void ResetPassword(User user, string newPassword);

        /// <summary>
        /// 获取头像文件
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        StorageFile GetAvatarFile(string userId);

        void SetDefaultAddress(string memberId, Guid addressId);
        /// <summary>
        /// 获得最大的会员卡号
        /// </summary>
        /// <returns></returns>
        string GetMaxMemberCard();
        /// <summary>
        /// 保存会员
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        int SaveMemberExtension(List<MemberExtension> member);
    }
}
