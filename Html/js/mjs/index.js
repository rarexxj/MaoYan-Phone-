$(function () {
    //轮播
    var mySwiper = new Swiper('.swiper-container', {
        direction: 'horizontal',
        paginationClickable: true,
        autoplay: 2500,
        loop:true,
        autoplayDisableOnInteraction: false,
        // 如果需要分页器
        pagination: '.swiper-pagination'
    })


    // scroll
    var obj = $(".scroll")[0];
    obj.onclick = function () {
        var timer = setInterval(function () {
            $("#index")[0].scrollTop -= 10;
            if ($("#index")[0].scrollTop <= 0) {
                clearInterval(timer);
            }
        }, 2);
    }

    // 窗口滚动检测
    $("#index")[0].onscroll = function () {
        obj.style.display = ($("#index")[0].scrollTop >= 10) ? "block" : "none"
    }


    //左侧导航栏伸缩
    $('.box').on('click','.toleft',function(){
        $('.sidebarnav').addClass('left');
        $('.sidebarnav').removeClass('right');
        $(this).addClass('toright');
        $(this).removeClass('toleft');
    })

    $('.box').on('click','.toright',function(){
        $('.sidebarnav').removeClass('left');
        $('.sidebarnav').addClass('right');
        $(this).addClass('toleft');
        $(this).removeClass('toright');
    })

    var _height=$('.sidebarnav').height();
    var _height2=$('.rightbar').height();
    $('.sidebarnav').css('margin-top',-_height/2)
    $('.rightbar').css('margin-top',-_height2/2)
})