$(function() {

    //导航点击
    $('.index-headnavmainul .index-navli').hover(function() {
        $(this).addClass('active');
        $(this).siblings().removeClass('active');
        $(this).children('.index-navtwo').stop().slideToggle();
    });

    //banner图轮播
    jQuery(".bannerbox").slide({
        mainCell: ".bd ul",
        autoPlay: true,
        mouseOverStop: false
    });


    //分页
    //根据总页数判断，如果小于5页，则显示所有页数，如果大于5页，则显示5页。根据当前点击的页数生成
    var pageCount = 11; //模拟后台总页数
    //生成分页按钮
    //if (pageCount > 5) {
    //    page_icon(1, 5, 0);
    //} else {
    //    page_icon(1, pageCount, 0);
    //}

    ////点击分页按钮触发
    //$("#pageGro").on("click", ".pageList li", function() {
    //    console.log(1231);
    //    if (pageCount > 5) {
    //        var pageNum = parseInt($(this).html()); //获取当前页数
    //        pageGroup(pageNum, pageCount);
    //    } else {
    //        $(this).addClass("on");
    //        $(this).siblings("li").removeClass("on");
    //    }
    //});

    //点击上一页触发
    $("#pageGro .pageUp").click(function() {
        if (pageCount > 5) {
            var pageNum = parseInt($("#pageGro li.on").html()); //获取当前页
            pageUp(pageNum, pageCount);
        } else {
            var index = $("#pageGro ul li.on").index(); //获取当前页
            if (index > 0) {
                $("#pageGro li").removeClass("on"); //清除所有选中
                $("#pageGro ul li").eq(index - 1).addClass("on"); //选中上一页
            }
        }
    });

    //点击下一页触发
    $("#pageGro .pageDown").click(function() {
        if (pageCount > 5) {
            var pageNum = parseInt($("#pageGro li.on").html()); //获取当前页
            pageDown(pageNum, pageCount);
        } else {
            var index = $("#pageGro ul li.on").index(); //获取当前页
            if (index + 1 < pageCount) {
                $("#pageGro li").removeClass("on"); //清除所有选中
                $("#pageGro ul li").eq(index + 1).addClass("on"); //选中上一页
            }
        }
    });

    //优惠券弹窗关闭
    $('.youhqlayer').on('click', '.youhqlayer-close', function() {
        $('.youhqlayerbox').hide();
    });
    $('.youhqlayer1').on('click', '.youhqlayer-close', function() {
        $('.youhqlayerbox1').hide();
    });

    //领取优惠券
    $('.coupon .couponr').on('click', function () {
        //优惠券类型及标识优惠券Id
        var type = $(this).data("type");
        var code = $(this).data("code");
        var couponId = $(this).data("id");
        $.ajax({
            type: "POST",
            url: url_addcoupon,
            data: { Code: code, CouponType: type,Id:couponId}
        }).done(function (data) {
            if (data.Success) {
                //所减金额
                $("#d1").html(data.Data.Money);
                //门槛
                if (data.Data.CouponType === 0) {

                    $("#d2").html("满" + data.Data.Minimum);
                } else {
                    $("#d2").html("立减" + data.Data.Money);
                }
                //当前年月日
                var myDate = new Date();
                var timeNow = myDate.toLocaleDateString();
                //获得当前时间
                var mydate = new Date();
                //当前时间加上月数
                var str = "" + mydate.getFullYear() + "/";
                if (data.Data.Term!==0)
                    str += (mydate.getMonth() + 1 + data.Data.Term) + "/";
                str += (mydate.getMonth() + 1) + "/";
                str += mydate.getDate();
               
                //有效时间
                $("#d3").html(timeNow + "——" + str);

                $('.youhqlayerbox').show();

            } else {
                $('.youhqlayerbox1').show();
            }
        });
    
      
    });

    
});