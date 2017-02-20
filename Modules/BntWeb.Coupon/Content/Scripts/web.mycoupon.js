$(document).ready(function () {

    //导航点击
    $('.index-headnavmainul .index-navli').hover(function() {
        $(this).addClass('active');
        $(this).siblings().removeClass('active');
        $(this).children('.index-navtwo').stop().slideToggle();
    });

    //切换 使用、未使用、已失效
    $('.order_nav li').on('click', function () {

        var status = $(this).data("status");
        location.href = url_mycoupon + "?status=" + status;
       
    });
})