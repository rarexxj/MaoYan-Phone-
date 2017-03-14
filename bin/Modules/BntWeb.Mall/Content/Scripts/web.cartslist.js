$(function () {
 

    //全局变量：用逗号拼接的字符串 用来全部删除时
    var arrList = "";
    //商品数组
    var goodArr = [];
    //自选商品数组
    var optGoodArr = [];
    //导航点击
    $('.index-headnavmainul .index-navli').hover(function() {
        $(this).addClass('active');
        $(this).siblings().removeClass('active');
        $(this).children('.index-navtwo').stop().slideToggle();
    });
    //删除
    $('.shanchu').on('click', function () {
        //当前购物车Id
        var carts = $(this).data("cartid");
        $.ajax({
            type: "POST",
            url: url_delCart,
            data: { cartIds: carts }
        }).done(function (data) {
            if (data.Success) {
                
                location.href = url_myCarts;
            }
            else {
                my_alert("购物车删除失败");
            }
        });
    });

   
    //全删除
    $('.shopC-shanc').on('click', function () {
      
        $('.checkdp').each(function() {
            if ($(this).is(':checked')) {
                var c = $(this).data("cartids");
                if (c !== undefined)
                { arrList += c + ","; }
            }
        });
        $.ajax({
            type: "POST",
            url: url_delCart,
            data: { cartIds: arrList }
        }).done(function (data) {
            if (data.Success) {
                location.href = url_myCarts;
            }
            else {
                my_alert("购物车删除失败");
            }
        });

    });


    //加
    $(".num-box .but-add").click(function() {
        var n = parseInt($(this).siblings('.but-num').val()) + 1;
        var danjia = $(this).parents('.numout').siblings('.danjia').children(' .danjia-xx').html();
        $(this).siblings('.but-num').val(n);
        //点击加号 变换价格
        $(this).parents('.numout').siblings('.peice-danjian').find('.peice-danjianxx').html((n * danjia).toFixed(2));
        allprice();
    });

    //减
    $(".num-box .but-cut").click(function() {
        var n = parseInt($(this).parent().find(".but-num").val()) - 1;
        var danjia = Number($(this).parents('.numout').siblings('.danjia').children(' .danjia-xx').html());
        if (n > 0) {
            $(this).siblings('.but-num').val(n);
            n--;
        } else {
            $(this).siblings('.but-num').val(1);
        }
        $(this).parents('.numout').siblings('.peice-danjian').find('.peice-danjianxx').html((n + 1) * danjia.toFixed(2));
        allprice();
    });
    //清除失效
    $('.shopCar-shixiao').on('click', function() {

        $.ajax({
            url: url_clearCart,
            type: "POST"
        }).done(function(data) {
            if (data.Success) {
                $('.allchioce .shixbox').each(function() {
                    $(this).parents('.allchioceli').remove();
                });
            } else {
                my_alert("购物车清除失效商品失败！");
            }
        });
    });

    //全选
    $('.selectall').on('click', function() {
        if ($('input[type=checkbox]')[0].checked) {
            var length = $('.bixuanpro input[type=checkbox]').length;
            for (var i = 0; i < length; i++) {
                $('.bixuanpro input[type=checkbox]')[i].checked = true;
            }
        } else {
            var length = $('.bixuanpro input[type=checkbox]').length;
            for (var i = 0; i < length; i++) {
                $('.bixuanpro input[type=checkbox]')[i].checked = false;
            }
        }
    });

    $('.selectall1').on('click', function() {
        if ($('input[type=checkbox]').last()[0].checked) {
            var length = $('input[type=checkbox]').length;
            for (var i = 0; i < length; i++) {
                $('input[type=checkbox]')[i].checked = true;
            }

        } else {
            console.log(1);
            var length = $('input[type=checkbox]').length;
            for (var i = 0; i < length; i++) {
                $('input[type=checkbox]')[i].checked = false;
            }
        }
    });

    $('input[type=checkbox]').on('click', function() {
        allprice();
    });
    $('.allchioceli').each(function() {
        var _num = $(this).find('.but-num').val();
        var _danjia = $(this).find('.danjia-xx').html();
        $(this).find('.peice-danjianxx').html((_num * _danjia).toFixed(2));
    });
    $('.danjia-xx').each(function() {
        $(this).html(Number($(this).html()).toFixed(2));
    });
    ////自选商品点击
    //$("#optmodel").on('click', function() {
    //    $(this)
        


    //});
    //计算总价格
    function allprice() {
        var sumPrice = 0;
        var btn = false;
        $('.checkdp').each(function () {
            if ($(this).is(':checked')) {
                var priceOne = Number($(this).parents('.nocenter').siblings('.peice-danjian').children('.peice-danjianxx').html());
                sumPrice = sumPrice + priceOne;
                $('.shopCar-price').html(sumPrice.toFixed(2));
                btn = true;
            }
        });
    
        if (!btn) {
            $('.shopCar-price').html("0");
            sumPrice = 0;
        }
    }

    //去结算

    $(".jiesuan").on('click', function () {

        if ($('.bixuanpro .checkdp').is(':checked')) {
            //自选商品Id
            var optIds = "";
            //购物车商品Ids
            var myCartsId = "";
            optGoodArr.splice(0, optGoodArr.length);
            goodArr.splice(0, goodArr.length);
            $('.checkdp').each(function() {
                if ($(this).data('cartids') === "") {
                    if ($(this).is(":checked")) {
                        var number = $(this).parent().parent().find('.number').val();
                        //自选商品
                        var optnum = $(this).parent().parent().find('.but-num').val();
                        //json对象
                        var optGoods = { id: $(this).data("optids"), quantity: optnum, number: number };
                        //放到数组里
                        optGoodArr.push(optGoods);
                    }
                    //序列化 后台用string 接受 再序列化成List
                     optIds= JSON.stringify(optGoodArr);
                }
                if ($(this).data('optids') === "") {
                    if ($(this).is(":checked")) {
                        //商品
                        var num = $(this).parent().parent().find('.but-num').val();
                        //json对象
                        var goodsObj = { id: $(this).data("cartids"), quantity: num };
                        //放到数组里
                        goodArr.push(goodsObj);
                        //序列化 后台用string 接受 再序列化成List
                        myCartsId = JSON.stringify(goodArr);
                    }
                }
            });
            window.location.href = url_confirmOrder + "?myCartsId=" + myCartsId + "&optIds="+optIds;
        } else {
            my_alert('请选择商品');
        }
    });

});