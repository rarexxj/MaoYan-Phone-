
$(function () {
    //全局变量
    //单品Id
    var singleId = "";
    //单品属性
    var attribute = "";
    //当前商品数组
    var nowGoodArr = [];
    //加价购商品
    var purGoodArr = [];
    //导航点击
    $('.index-headnavmainul .index-navli').hover(function() {
        $(this).addClass('active');
        $(this).siblings().removeClass('active');
        $(this).children('.index-navtwo').stop().slideToggle();
    });


    imgShow($(".img-show"), $(".img-show-box"), 5, 83, "margin-left"); // 大事件

    setTimeout(function () {
        $(".jqzoom").jqueryzoom({
            xzoom: 496, //放大图的宽度
            yzoom: 496, //放大图的高度
            offset: 0, //离原图的距离
            position: "right",
            //放大图的定位(默认是 "right")
            preload: 1
        });
    }, 1000);

    //商品尺寸 分类
    $('.classificationul li').on('click', function() {
        $(this).addClass('active');
        $(this).siblings().removeClass('active');
        //点击每个分类其对应的库存要改变
       var stock= $(this).data("stock");
       var unit = $(this).data("unit");
       attribute = $(this).data("attribute");
       $("#unitId").html(unit);
       $("#inv-num").html(stock);
        //现价变化
       var nowprice = $(this).data("nowprice");
       $(".nowprice").html("￥" + nowprice);
       singleId = $(this).data("singleid");

       
    });
    //加
    $(".num-box .but-add").click(function() {
        var n = parseInt($(this).siblings('.but-num').val());
        var s = $("#inv-num").html();
        if (n < s) {
            $(this).siblings('.but-num').val(n + 1);
        } else {
            $(this).siblings('.but-num').val(s);
        }
       
    });
    //减
    $(".num-box .but-cut").click(function() {
        var n = parseInt($(this).parent().find(".but-num").val());
        if (n > 1) {
            $(this).siblings('.but-num').val(n - 1);
        } else {
            $(this).siblings('.but-num').val(1);
        }
    });

    // 遮罩
    $(".yiny").click(function() {
        $(this).stop().hide();
        $(".s-alert-sccess.s-car-alert").hide();
        $(".s-alert-sccess.s-collect-alert").hide();
    });

    //购物车
    $(".jjh").click(function () {
        if ($(".classificationul .attri.active").length < 1) {
            my_alert("请选择分类");
            return false;
        }
        //参数：商品Id，单品Id，商品数量 
        var goodId = $("#GoodId").val();
        //判断加价购商品是否被选中
        var isPurce ="0";
        var purchaseId = "";
        $('.jiajiabox').each(function() {
            if ($(this).attr('data-flag') == 1) {
                purchaseId = $(this).attr('data-id');
                isPurce = $(this).attr('data-flag');
            }
        });
   
        var quantity = $(".but-num").val();
        $.ajax({
            type: "POST",
            url: url_addMycart,
            data: { GoodsId: goodId, SingleGoodsId: singleId, Quantity: quantity, Flag: isPurce, PurchaseId: purchaseId }
        }).done(function (data) {
            if (data.Success) {
                $(".s-alert-sccess.s-car-alert").show();
                $(".yiny").show();

            } else {
                my_alert("未能成功加入购物车");
            }
        });
    });


    //立即购买
    $(".jjg").click(function () {
        if ($(".classificationul .attri.active").length < 1) {
            my_alert("请选择分类");
            return false;
        }
     
        bntLoading.show();
        //立即购买的商品Id
        var goodIds = "";
        //加价购商品ids
        var purIds = "";
        //判断加价购商品是否被选中
        var isPurce = "0";
        var purchaseId = "";
        purGoodArr=[];
        $('.jiajiabox').each(function () {
            if ($(this).attr('data-flag') == 1) {

                purchaseId = $(this).attr('data-id');
                isPurce = $(this).attr('data-flag');
                  //获得加价购商品的参数  Flag PurchaseId
                    var purGoods = { id: purchaseId, quantity: 1 };
                    purGoodArr.push(purGoods);
                    purIds = JSON.stringify(purGoodArr);
            }
        });
      
      
        var quantitys = $(".but-num").val();
        var goods = { id: singleId, quantity: quantitys }
        nowGoodArr.push(goods);
        goodIds = JSON.stringify(nowGoodArr);
        location.href = url_buyNow + "?singleIds=" + goodIds + "&purIds=" + purIds;

    });
   

    $(".s-alert-sccess.s-car-alert .close").click(function() {
        $(".s-alert-sccess.s-car-alert").hide();
        $(".yiny").hide();
    });

    //商品详情 累计评价
    $('.cheap-info-down-l .hd li').on('click', function() {
        var _index = $(this).index();
        $(this).addClass('cur');
        $(this).siblings().removeClass('cur');
        $('.cheap-info-down-l .bd>ul').hide();
        $('.cheap-info-down-l .bd>ul').eq(_index).show();
    });

    //加价购选择
    $('.jiajiabox').on('click', function() {
        $(this).children('.jiajiaquan').toggleClass('active');
        $(this).siblings('.jiajiabox').children('.jiajiaquan').removeClass('active');
        if ($(this).children('.jiajiaquan').hasClass('active')) {
            $(this).siblings('.jiajiabox').attr('data-flag', '0');
            $(this).attr('data-flag', '1');
            $("#flag").val($(this).data('flag'));
        } else {
            $(this).attr('data-flag', '0');
            $("#flag").val($(this).data('flag'));
        }

    });
    //评价类型切换
    $('.pjnav .font').on('click', function() {
        $(this).addClass('active');
        $(this).siblings().removeClass('active');
    });

    $(".s-alert-sccess.s-collect-alert .close").click(function() {
        $(".s-alert-sccess.s-collect-alert").hide();
        $(".yiny").hide();
    });

    $(".zuh-b button").click(function() {
        if ($(".zuh-m ul ").find("dd.cur").length == 0) {
            $(".xuanzgg").show();
        } else {
            location.href = "http://www.baidu.com"
        }
    });

    $(".cc-box .close").click(function() {
        $('.cc-box').hide();
        $(".yiny").hide();
    });
});