

$(document).ready(function() {

    //导航点击
    $('.index-headnavmainul .index-navli').hover(function() {
        $(this).addClass('active');
        $(this).siblings().removeClass('active');
        $(this).children('.index-navtwo').stop().slideToggle();
    });


    //积分切换    $('.order_nav li').on('click', function () {
     
        $(this).siblings().removeClass('active');        $(this).addClass('active');
        var status = $(this).data('status');
        location.href = url_myIntagral + "?billType=" + status;
    });


});