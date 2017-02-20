$(function () {
    //效果js
    mySwiper();
    function mySwiper() {
        var mySwiper = new Swiper('.swiper-container', {
            direction: 'horizontal',
            loop: true,
            paginationClickable: true,
            autoplay: 2500,
            slidesPerView: 'auto',
            centeredSlides: true,
            grabCursor: true,
            autoplayDisableOnInteraction: false,
            // 如果需要分页器
            pagination: '.swiper-pagination'
        })
    }


    //选择
    choose();
    function choose() {
        $('.size').on('click', function () {
            $('.size-mask').show();
        })
        $('.size-mask .tc li').on('click', function () {
            $(this).addClass('cur').siblings().removeClass('cur');
        })
        //关闭
        $('.size-mask .close').on('click', function () {
            $('.size-mask').hide();
        })

    }

    //加价购
    jjgou()
    function jjgou() {
        $('.infoBox').on('click', '.jjbox', function () {
            $('.quan').removeClass('cur');
            $(this).children('.quan').addClass('cur');
        })
    }


    //加减
    Calculation();
    function Calculation() {
        //加
        $('.getnum .jia').on('click', function () {
            var max = parseInt($('.kc').html());
            var num = $(this).parents('.numbox').find('.amount').val();
            if (num >= max) {
                num = max;
            } else {
                num++;
            }
            $(this).parents('.numbox').find('.amount').val(num)
        })
        //减
        $('.getnum .jian').on('click', function () {
            var num = $(this).parents('.numbox').find('.amount').val();
            if (num <= 1) {
                num = 1;
            } else {
                num--;
            }
            $(this).parents('.numbox').find('.amount').val(num)
        })
    }


    //切换
    tab();
    function tab() {
        $('.information .btn').eq(0).on('click', function () {
            $(this).addClass('cur').siblings().removeClass('cur');
            $('.infoajax').show();
            $('.evaluateajax').hide();
        })
        $('.information .btn').eq(1).on('click', function () {
            $(this).addClass('cur').siblings().removeClass('cur');
            $('.infoajax').hide();
            $('.evaluateajax').show();
        })
    }

    //收藏
    coll();
    function coll() {
        $('.goshop .coll').on('click', function () {
            $('.goshop .coll').addClass('on')
            $.oppo('商品已加入收藏夹', 1)
        })
    }

    //用户评价
    // var flag = true;
    // $(window).scroll(function () {
    //     if ($('.eva-btn').hasClass('cur')) {
    //         var H = $('body,html').height();
    //         var h = $(window).height();
    //         var t = $('body').scrollTop();
    //         if (t >= H - h * 1.1 && flag == true) {
    //             flag = false;
    //             evadata.pageNo++;
    //             if (evadata.pageNo > colpage) {
    //                 $('.loading').hide();
    //             } else {
    //                 ajaxeva(function () {
    //                     setTimeout(function () {
    //                         flag = true;
    //                     }, 500)
    //                 }, function () {
    //                     $('.loading').show();
    //                 })
    //             }
    //         }
    //     }
    //
    // })

    //加入购物车
    $('.addshopc').on('click', function () {
        console.log(2312)
        $.oppo('商品已加入购物车', 1)
        // $('.size-mask').show();
    })

    $('.infoBox').on('click', '.pro-in-gocart', function () {
        $(this).addClass('on')
    })

    //购买
    $('.infoBox').on('click', '.gobuy', function () {
        $('.size-mask').show();
    })

})