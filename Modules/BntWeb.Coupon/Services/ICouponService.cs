using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.Coupon.Models;
using BntWeb.Coupon.ViewModels;
using BntWeb.Security;

namespace BntWeb.Coupon.Services
{
    public interface ICouponService : IDependency
    {
        #region 优惠券

        Models.Coupon GetCouponByCode(string code);
        #endregion

        #region 用户优惠券
        /// <summary>
        /// 获取用户优惠券完整信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Models.CouponRelation GetCouponRelationById(Guid id);
        /// <summary>
        /// 获得我的优惠券
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        List<MyCoupsModel> GetMyCouponList(string memberId, CouponStatus status);
        /// <summary>
        /// 分也获得我的优惠券
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="status"></param>
        /// <param name="totalCount"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        List<MyCoupsModel> GetCoupons(string memberId, CouponStatus status, out int totalCount, int pageIndex = 1,
            int pageSize = 9);
        /// <summary>
        /// 获得我的所有领取的优惠券
        /// </summary>
        /// <param name="memberId">会员Id</param>
        /// <param name="status">优惠券状态</param>
        /// <returns></returns>
        List<CouponRelation> GetMyUseCoupon(string memberId, CouponStatus status);
        /// <summary>
        /// 添加用户优惠券
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="code"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        bool AddMemberCoupon(string memberId, string code, Models.CouponType type);

        /// <summary>
        /// 添加会员优惠券 支持优惠券标示和金额查询
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="code"></param>
        /// <param name="money"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        bool AddMemberCoupon(string memberId, string code, int money, Models.CouponType type);

        /// <summary>
        /// 添加会员优惠券 按张数插入
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="code"></param>
        /// <param name="money"></param>
        /// <param name="type"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        bool AddMemberCoupon(string memberId, string code, int money, Models.CouponType type, int count);

        /// <summary>
        /// 获取会员优惠券列表
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="isAvailable">是否可用</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<Models.CouponRelation> GetMemberCoupons(string memberId, bool isAvailable, int pageIndex,
            int pageSize, out int totalCount);

        /// <summary>
        /// 获取会员有效优惠券、现金券
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="couponType"></param>
        /// <param name="minimum"></param>
        /// <returns></returns>
        List<Models.CouponRelation> GetAvailableMemberCoupons(string memberId, Models.CouponType couponType, int minimum = 0);

        /// <summary>
        /// 获取会员可提现现金券列表
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        List<Models.CouponRelation> GetMemberWithdrawalCoupons(string memberId);

        #endregion

        #region 优惠券使用记录
        /// <summary>
        /// 添加优惠券使用记录
        /// </summary>
        /// <param name="recordInfo"></param>
        /// <returns></returns>
        bool AddCouponUseRecord(Models.CouponUseRecord recordInfo);

        /// <summary>
        /// 根据来源获取会员优惠券列表
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="moduleKey"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        List<Models.CouponRelation> GetCouponRelationBySource(Guid sourceId, string moduleKey, string sourceType);

        #endregion

        #region 优惠券过期处理
        /// <summary>
        /// 修改超过时间未使用的优惠券状态为已过期
        /// </summary>
        /// <returns></returns>
        int ProcessTimeOutCoupon();

        #endregion
        /// <summary>
        /// 创建优惠券
        /// </summary>
        /// <param name="coupon"></param>
        /// <returns></returns>
        bool CreateCoupon(Models.Coupon coupon);
       /// <summary>
       /// 更新优惠券
       /// </summary>
       /// <param name="coupon"></param>
       /// <returns></returns>
        bool UpdateCoupon(Models.Coupon coupon);
        /// <summary>
        /// 删除优惠券
        /// </summary>
        /// <param name="coupon"></param>
        /// <returns></returns>
        bool Delete(Models.Coupon coupon);
        /// <summary>
        /// 修改优惠券状态
        /// </summary>
        /// <param name="couponId"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        bool Switch(Guid couponId, string enabled);

    }
}