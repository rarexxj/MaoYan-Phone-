using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.Data;
using BntWeb.Data.Services;
using BntWeb.Logging;
using System.Data.Entity;
using BntWeb.Coupon.Models;
using BntWeb.Coupon.ViewModels;
using BntWeb.Security;

namespace BntWeb.Coupon.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICurrencyService _currencyService;

        public CouponService(ICurrencyService currencyService)
        {
            _currencyService = currencyService;

            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        #region 优惠券

        public Models.Coupon GetCouponByCode(string code)
        {
            using (var dbContext = new CouponDbContext())
            {
                return dbContext.Coupons.FirstOrDefault(c => c.Code == code);
            }
        }
        #endregion

        #region 用户优惠券
        public Models.CouponRelation GetCouponRelationById(Guid id)
        {
            using (var dbContext = new CouponDbContext())
            {
                return dbContext.CouponRelations.Include(c => c.Coupon).FirstOrDefault(c => c.Id == id);
            }
        }
        /// <summary>
        /// 获得我的所有领取的优惠券
        /// </summary>
        /// <param name="memberId">会员Id</param>
        /// <param name="status">优惠券状态</param>
        /// <returns></returns>
        public List<CouponRelation> GetMyUseCoupon(string memberId, CouponStatus status)
        {
            var couponList=
                _currencyService.GetList<CouponRelation>(a => a.Status == status && a.MemberId == memberId).ToList();
            return couponList;

        }
        public List<MyCoupsModel> GetMyCouponList(string memberId, CouponStatus status)
        {
            using (var dbContext = new CouponDbContext())
            {
                var query = from cr in dbContext.CouponRelations
                    join c in dbContext.Coupons on cr.CouponId equals c.Id
                    where cr.MemberId == memberId && cr.Status == status
                    orderby cr.BeginTime descending
                    select new MyCoupsModel
                    {
                        Id = c.Id,
                        CouponType = c.CouponType,
                        Minimum = c.Minimum,
                        Money = c.Money,
                        Term = c.Term,
                        Title = c.Title,
                        BeginTime = cr.BeginTime,
                        EndTime = cr.EndTime

                    };
                return query.ToList();
            }
        }
        /// <summary>
        /// 分页获得我的优惠券
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="status"></param>
        /// <param name="totalCount"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<MyCoupsModel> GetCoupons(string memberId, CouponStatus status, out int totalCount, int pageIndex=1,int pageSize=9)
        {
            using (var dbContext = new CouponDbContext())
            {
                var query = from cr in dbContext.CouponRelations
                            join c in dbContext.Coupons on cr.CouponId equals c.Id
                            where cr.MemberId == memberId && cr.Status == status
                            orderby cr.BeginTime descending
                            select new MyCoupsModel
                            {
                                Id=c.Id,
                                CouponType=c.CouponType,
                                Minimum =c.Minimum,
                                Money=c.Money,
                                Term=c.Term,
                                Title=c.Title,
                                BeginTime=cr.BeginTime,
                                EndTime=cr.EndTime

                            };

                totalCount = query.Count();
                return query.Skip((pageIndex - 1)*pageSize).Take(pageSize).ToList();

            }
            
        }

        /// <summary>
        /// 领取优惠券
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="code"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool AddMemberCoupon(string memberId, string code, CouponType type)
        {
            using (var dbContext = new CouponDbContext())
            {
                var coupon = dbContext.Coupons.FirstOrDefault(c => c.Code.Equals(code, StringComparison.OrdinalIgnoreCase) && c.CouponType == type && c.Enabled);
                if (coupon == null)
                    return false;//throw new Exception("优惠券类型不合法");

               CouponRelation model = new CouponRelation
                {
                    Id = KeyGenerator.GetGuidKey(),
                    CouponId = coupon.Id,
                    MemberId = memberId,
                    CodeNo = KeyGenerator.GenerateRandom(14),
                    BeginTime = DateTime.Now.Date
                };
                if (coupon.Term > 0)
                {
                    model.EndTime = model.BeginTime.AddMonths(coupon.Term);
                }

                dbContext.CouponRelations.Add(model);
                return dbContext.SaveChanges() > 0;
            }
        }
        /// <summary>
        /// 领取优惠券
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="code"></param>
        /// <param name="money"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool AddMemberCoupon(string memberId, string code, int money, CouponType type)
        {
            using (var dbContext = new CouponDbContext())
            {
                var coupon = dbContext.Coupons.FirstOrDefault(c => c.Code.Equals(code, StringComparison.OrdinalIgnoreCase) && c.Money == money && c.CouponType == type && c.Enabled);
                if (coupon == null)
                    return false;//throw new Exception("优惠券类型不合法");

                CouponRelation model = new CouponRelation
                {
                    Id = KeyGenerator.GetGuidKey(),
                    CouponId = coupon.Id,
                    MemberId = memberId,
                    CodeNo = KeyGenerator.GenerateRandom(14),
                    BeginTime = DateTime.Now.Date
                };
                if (coupon.CouponType == CouponType.FullCut && coupon.Term > 0)
                {
                    model.EndTime = model.BeginTime.AddMonths(coupon.Term);
                }

                dbContext.CouponRelations.Add(model);

                return dbContext.SaveChanges() > 0;
            }
        }

/// <summary>
/// 领取优惠券
/// </summary>
/// <param name="memberId"></param>
/// <param name="code"></param>
/// <param name="money"></param>
/// <param name="type"></param>
/// <param name="count"></param>
/// <returns></returns>
        public bool AddMemberCoupon(string memberId, string code, int money, CouponType type, int count)
        {
            using (var dbContext = new CouponDbContext())
            {
                var coupon = dbContext.Coupons.FirstOrDefault(c => c.Code.Equals(code, StringComparison.OrdinalIgnoreCase) && c.Money == money && c.CouponType == type && c.Enabled);
                if (coupon == null)
                    return false;//throw new Exception("优惠券类型不合法");

                if (count > 0)
                {

                    bool hasEndTime = coupon.CouponType == CouponType.FullCut && coupon.Term > 0;
                    List<CouponRelation> list = new List<CouponRelation>();

                    for (var i = 0; i < count; i++)
                    {
                        CouponRelation model = new CouponRelation
                        {
                            Id = KeyGenerator.GetGuidKey(),
                            CouponId = coupon.Id,
                            MemberId = memberId,
                            CodeNo = KeyGenerator.GenerateRandom(14),
                            BeginTime = DateTime.Now.Date
                        };
                        if (hasEndTime)
                        {
                            model.EndTime = model.BeginTime.AddMonths(coupon.Term);
                        }

                        list.Add(model);

                    }

                    dbContext.CouponRelations.AddRange(list);
                }

                return dbContext.SaveChanges() > 0;
            }
        }

        public List<CouponRelation> GetMemberCoupons(string memberId, bool isAvailable, int pageIndex, int pageSize, out int totalCount)
        {
            using (var dbContext = new CouponDbContext())
            {
                var query = dbContext.CouponRelations.Include(c => c.Coupon).Where(c => c.MemberId == memberId);
                if (isAvailable)
                {
                    query = query.Where(c => c.Status == CouponStatus.Unused);
                }
                else
                {
                    query = query.Where(c => c.Status != CouponStatus.Unused);
                }
                query = query.OrderBy(c => c.BeginTime);
                totalCount = query.Count();
                return query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public List<CouponRelation> GetAvailableMemberCoupons(string memberId, CouponType couponType, int minimum = 0)
        {
            using (var dbContext = new CouponDbContext())
            {
                var query = dbContext.CouponRelations.Include(c => c.Coupon).Where(c => c.MemberId == memberId && c.Status == CouponStatus.Unused && c.Coupon.CouponType == couponType);

                if (couponType == CouponType.FullCut && minimum > 0)
                {
                    query = query.Where(c => minimum >= c.Coupon.Minimum);
                }
                query = query.OrderBy(c => c.BeginTime);
                return query.ToList();
            }
        }

        public List<CouponRelation> GetMemberWithdrawalCoupons(string memberId)
        {
            using (var dbContext = new CouponDbContext())
            {
                var withdrawalTime = DateTime.Now.Date.AddDays(-90);
                var query = dbContext.CouponRelations.Include(c => c.Coupon).Where(c => c.MemberId == memberId && c.Status == CouponStatus.Unused && c.Coupon.CouponType == CouponType.Minus && c.BeginTime < withdrawalTime);

                query = query.OrderBy(c => c.BeginTime);
                return query.ToList();
            }
        }

        #endregion

        #region 优惠券使用记录
        public bool AddCouponUseRecord(CouponUseRecord recordInfo)
        {
            recordInfo.Id = KeyGenerator.GetGuidKey();
            recordInfo.CreateTime = DateTime.Now;
            recordInfo.IsUsed = true;
            var result = _currencyService.Create<CouponUseRecord>(recordInfo);
            return result;
        }


        public List<CouponRelation> GetCouponRelationBySource(Guid sourceId, string moduleKey, string sourceType)
        {
            using (var dbContext = new CouponDbContext())
            {

                var query = from a in dbContext.CouponUseRecords
                            join b in dbContext.CouponRelations on a.CouponRelationId equals b.Id
                            where a.SourceId == sourceId && a.ModuleKey == moduleKey && a.SourceType == sourceType
                            select b;

                return query.ToList();
            }
        }
        #endregion

        #region 优惠券过期处理

        public int ProcessTimeOutCoupon()
        {
            using (var dbContext = new CouponDbContext())
            {
                var outTime = DateTime.Now.Date;
                var query = dbContext.CouponRelations.Where(c => c.EndTime < outTime && c.Status == CouponStatus.Unused).ToList();
                query.ForEach(c => c.Status = CouponStatus.Expired);
                return dbContext.SaveChanges();
            }
        }
        #endregion

        /// <summary>
        /// 创建优惠券
        /// </summary>
        /// <param name="coupon"></param>
        /// <returns></returns>
        public bool CreateCoupon(Models.Coupon coupon)
        {

            var result = _currencyService.Create<Models.Coupon>(coupon);
        
            Logger.Operation($"创建优惠券-{coupon.Title}:{coupon.Id}", CouponModule.Instance, SecurityLevel.Normal);

            return result;
        }
        /// <summary>
        /// 更新优惠券
        /// </summary>
        /// <param name="coupon"></param>
        /// <returns></returns>
        public bool UpdateCoupon(Models.Coupon coupon)
        {
          
            var result = _currencyService.Update<Models.Coupon>(coupon);
            Logger.Operation($"更新优惠券-{coupon.Title}:{coupon.Id}", CouponModule.Instance, SecurityLevel.Normal);
            return result;
        }
        /// <summary>
        /// 删除优惠券
        /// </summary>
        /// <param name="coupon"></param>
        /// <returns></returns>
        public bool Delete(Models.Coupon coupon)
        {
           
            var result = _currencyService.Delete(coupon);

            if (result)
                Logger.Operation($"删除联系人优惠券-{coupon.Title}:{coupon.Id}", CouponModule.Instance,
                    SecurityLevel.Warning);

            return result;
        }
        /// <summary>
        /// 修改优惠券状态
        /// </summary>
        /// <param name="couponId"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public bool Switch(Guid couponId, string enabled)
        {

            var coupon = _currencyService.GetSingleById<Models.Coupon>(couponId);
            var result = false;
            if (coupon != null)
            {
                if(enabled=="false")
                coupon.Enabled = true;
                if (enabled == "true")
                    coupon.Enabled = false;
                result = _currencyService.Update(coupon);
            }
            if (result)
                Logger.Operation($"修改优惠券状态-{coupon.Enabled}:{coupon.Id}", CouponModule.Instance,
                    SecurityLevel.Warning);

            return result;
        }


    }
}