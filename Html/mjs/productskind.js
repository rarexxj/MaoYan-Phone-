$(function () {
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

    var _height2=$('.rightbar').height();
    $('.rightbar').css('margin-top',-_height2)



    //综合排序
    $('.sort').on('click', function () {
        $('.sort-mask').show();
        $('.screen-mask').hide();
    })
    $('.box').on('click', '.sort-mask li', function () {
        $('.sort').addClass('cur');
        $('.num').removeClass('cur');
        $('.screen').removeClass('cur');
        $(this).addClass('typecur').siblings().removeClass('typecur');
    })
    $('.sort-mask').on('click', function () {
        $(this).hide();
        $('.screen-mask').hide();
    })

    //销量排序
    $('.box').on('click', '.num', function () {
        $(this).addClass('cur').siblings('.chobtn').removeClass('cur');
        $('.sort-mask li').removeClass('typecur');
        $('.sort-mask').hide();
        $('.screen-mask').hide();
    })
    //高级筛选
    $('.screen').on('click',function () {
        $('.screen-mask').show();
    })

    $('.box').on('click', '.screen-box .outbox li',function () {
        if ($(this).hasClass('cur')) {
            $(this).removeClass('cur');
        } else {
            $(this).addClass('cur');
        }
    })
    $('.box').on('click','.shaixtitle',function () {
        $('.screen-mask').hide();
    })


    //确定
    $('.box').on('click','.screen-btn',function () {
    })
})