using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using BntWeb.Environment;
using BntWeb.Evaluate;
using BntWeb.FileSystems.Media;
using BntWeb.MemberBase;
using BntWeb.Utility.Extensions;

namespace BntWeb.Mall.ApiModels
{
    public class GoodsEvaluateModel
    {
        /// <summary>
        ///  口感满意评分
        /// </summary>
        public int GoodTasteScore { get; set; }

        /// <summary>
        ///  材料新鲜评分
        /// </summary>
        public int FreshMaterialScore { get; set; }
        /// <summary>
        /// 物流服务评分
        /// </summary>
        public int LogisticsScore { get; set; }
      
        /// <summary>
        /// 描述相符评分
        /// </summary>
        public int DesMatchScore { get; set; }
        /// <summary>
        /// 评价内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 是否匿名
        /// </summary>
        public bool IsAnonymity { get; set; }

        /// <summary>
        /// 评价人名称
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// 评价时间
        /// </summary>
        public DateTime EvaluateTime { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        public string ReplyContent { get; set; }

        /// <summary>
        /// 回复时间
        /// </summary>
        public DateTime? ReplyTime { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public SimplifiedStorageFile Avatar { get; set; }

        /// <summary>
        /// 评价图片
        /// </summary>
        public List<SimplifiedStorageFile> Files { get; set; }
        /// <summary>
        /// 平均分
        /// </summary>
        public int Score { get; set; }
        public GoodsEvaluateModel(Evaluate.Models.Evaluate model)
        {
            GoodTasteScore = model.GoodTasteScore;
            FreshMaterialScore = model.FreshMaterialScore;
            LogisticsScore = model.FreshMaterialScore;
            DesMatchScore =model.FreshMaterialScore;
            Score = (int) Math.Round((decimal)(model.GoodTasteScore + model.FreshMaterialScore + model.FreshMaterialScore +
                                               model.FreshMaterialScore)/4,0);
            Content = model.Content;
            IsAnonymity = model.IsAnonymity;
            MemberName = model.MemberName;
            EvaluateTime = model.CreateTime;
            ReplyContent = model.ReplyContent;
            ReplyTime = model.ReplyTime;
            var fileService = HostConstObject.Container.Resolve<IStorageFileService>();
            var avatar = fileService.GetFiles(model.MemberId.ToGuid(), MemberBaseModule.Key, "Avatar").FirstOrDefault();
            Avatar = avatar?.Simplified();
            Files = fileService.GetFiles(model.Id, EvaluateModule.Key, "Evaluate").Select(x => x.Simplified()).ToList();
        }
    }

    public class EvaluateMemberAvatarModel
    {
        public SimplifiedStorageFile Avatar { get; set; }
        public EvaluateMemberAvatarModel(Evaluate.Models.Evaluate model)
        {
            var fileService = HostConstObject.Container.Resolve<IStorageFileService>();
            var avatar = fileService.GetFiles(model.MemberId.ToGuid(), MemberBaseModule.Key, "Avatar").FirstOrDefault();
            Avatar = avatar?.Simplified();
        }
    }
}