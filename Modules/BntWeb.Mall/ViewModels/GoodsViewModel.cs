/* 
    ======================================================================== 
        File name：        GoodsViewModel
        Module:                
        Author：            罗嗣宝
        Create Time：    2016/7/5 15:18:59
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Autofac;
using BntWeb.Environment;
using BntWeb.FileSystems.Media;
using BntWeb.Mall.Models;
using BntWeb.Mall.Services;

namespace BntWeb.Mall.ViewModels
{
    public class GoodsViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        
        public Guid? BrandId { get; set; }

        public Guid CategoryId { get; set; }

        public string GoodsNo { get; set; }

        public decimal OriginalPrice { get; set; }

        public string Description { get; set; }

        public Guid GoodsType { get; set; }

        public string SingleGoodsJson { get; set; }
        /// <summary>
        /// 关联的加价购商品
        /// </summary>
        public string RelationOpt { get; set; }
        public List<SingleGoodsViewModel> SingleGoods { get; set; }

        /// <summary>
        /// 扩展分类Ids，英文逗号分隔
        /// </summary>
        public string ExtendCategoryIds { set; get; }
        /// <summary>
        /// 主图
        /// </summary>
        public string MainImage { set; get; }

        public bool IsNew { set; get; }

        public bool IsBest { set; get; }

        public bool IsHot { set; get; }

        public bool FreeShipping { set; get; }

        public decimal[] Commission { get; set; }
    }
    public class SpecialGoodsViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string GoodsNo { get; set; }

        public decimal ShopPrice { get; set; }
        public int Stock { get; set; }

        public string Description { get; set; }
        public bool FreeShipping { get; set; }
        /// <summary>
        /// 所需积分
        /// </summary>
        public int ExchangeIntegral { get; set; }
        /// <summary>
        /// 主图
        /// </summary>
        public string MainImage { set; get; }
        /// <summary>
        /// 商品特殊类型
        /// </summary>
     public SpecialType SpecialType { get; set; }
    }
    public class SingleGoodsViewModel
    {
        public Guid Id { get; set; }

        public int Stock { get; set; }

        public string Unit { get; set; }

        public decimal Price { get; set; }
     

        public List<AttrViewModel> Attrs { get; set; }

        public SingleGoodsImage Image { get; set; }
    }
    public class SingleGoodsImage
    {
        public Guid Id { get; set; }

        public string RelativePath { get; set; }
        /// <summary>
        /// 中等尺寸缩略图，图片类型可用
        /// 
        /// </summary>
        public string MediumThumbnail { get; set; }
        /// <summary>
        /// 小尺寸缩略图，图片类型可用
        /// 
        /// </summary>
        public string SmallThumbnail { get; set; }
    }

    public class AttrViewModel
    {
        public Guid Id { get; set; }

        public string Val { get; set; }
    }
    public class SelectedAttrViewModel
    {
        public Guid Id { get; set; }

        public List<string> Vals { get; set; }
    }

    /// <summary>
    /// 商品浏览记录模型
    /// </summary>
    public class BrowseViewModel
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 售价，如果有多个规格的商品，取最低价格
        /// </summary>
        public decimal ShopPrice { get; set; }

        /// <summary>
        /// 商品状态 0-失效 1-正常
        /// </summary>
        public Models.GoodsStatus Status { get; set; }

        /// <summary>
        /// 浏览时间
        /// </summary>
        public DateTime BrowseTime { get; set; }

        public string MinePicture { get; set; }
        public string Days { get; set; }
        public Guid GoodsId  { get; set; }

        public Guid SourceId { get; set; }

        public DateTime CreateTime { get; set; }

    }

    public class ExtendGoodsViewModelnew
    {
        public GoodsView GoodsView { get; set; }
        /// <summary>
        /// 扩展属性
        /// </summary>
        public Guid? ExtendCategoryId { get; set; }
    }
   
    public class ExchangeModel
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 积分兑换 所需积分
        /// </summary>
        public int ExchangeIntegral { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [MaxLength(200)]
        public string Name { get; set; }
        public SimplifiedStorageFile GoodsImage { set; get; }
         public ExchangeModel(Goods model)
    {
        Id = model.Id;
        ExchangeIntegral = model.ExchangeIntegral;
        Name = model.Name;
        var fileService = HostConstObject.Container.Resolve<IStorageFileService>();
        var mainImage = fileService.GetFiles(model.Id, MallModule.Key, "MainImage").FirstOrDefault();
        GoodsImage = mainImage?.Simplified();
    }

    }
}