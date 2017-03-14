using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using BntWeb.Data;

namespace BntWeb.Coupon
{
    public class CouponDbContext: BaseDbContext
    {
        public DbSet<Models.Coupon> Coupons { get; set; }
        public DbSet<Models.CouponRelation> CouponRelations { get; set; }
        public DbSet<Models.CouponUseRecord> CouponUseRecords { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}