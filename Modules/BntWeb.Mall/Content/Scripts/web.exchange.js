$(document).ready(function () {
    //单价和数量
  
    //导航点击
    $('.index-headnavmainul .index-navli').hover(function() {
        $(this).addClass('active');
        $(this).siblings().removeClass('active');
        $(this).children('.index-navtwo').stop().slideToggle();
    });

    //积分兑换
    $('.jfdh-mainr').on('click', function () {
        //当前可用积分
        var mm = $(".integral").html();
        //当前商品所需积分
        var u = $(this).data("nowinteg");
        if (eval(Number(mm)) < eval(Number(u))) {
            my_alert("当前所用积分不够！");
            return false;
        }
        //商品Id
        var goodId = $(this).data("id");
        //产生订单 确认订单页面
        location.href = url_exchangeIn + "?goodId=" + goodId;

    });


})