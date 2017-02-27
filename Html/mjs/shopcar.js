$(function () {
    $.ADDLOAD();
    ajax();
    function ajax() {
        $.ajax({
            url: '/Api/v1/Mall/Cart',
            type: 'get'
        }).done(function (rs) {
            if (rs.returnCode == '200') {
                for (var i = 0; i < rs.data.Carts.length; i++) {
                    if (rs.data.Carts[i].Quantity >= rs.data.Carts[i].Stock) {
                        rs.data.Carts[i].Quantity = rs.data.Carts[i].Stock;
                        console.log(rs.data)
                    }
                }
                view(rs.data);
            } else {
                if (rs.returnCode == '401') {
                    Backlog();
                } else {
                    oppo(rs.msg, 1)
                }
            }
        })
    }

    function view(rs) {
        if (rs.Carts.length == 0) {
            $('.search-box').show();
            $('.nothing').show();
        }
        //获取总价 转换
        var shixiao = []
        for (i in rs.Carts) {
            rs.Carts[i].allprice = rs.Carts[i].Price * rs.Carts[i].Quantity;
            if (rs.Carts[i].allprice.toString().indexOf('.') > -1) {
                if (rs.Carts[i].allprice.toString().split('.')[1].length == 1) {
                    rs.Carts[i].prices2 = rs.Carts[i].allprice.toString().split('.')[1] + '0'
                } else {
                    rs.Carts[i].prices2 = rs.Carts[i].allprice.toString().split('.')[1];
                }
                rs.Carts[i].prices1 = rs.Carts[i].allprice.toString().split('.')[0];

            } else {
                rs.Carts[i].prices1 = rs.Carts[i].allprice;
                rs.Carts[i].prices2 = '00';
            }
            //获取属性数组
            rs.Carts[i].shuxi = []
            for (j in rs.Carts[i].GoodsAttribute.split(',')) {
                rs.Carts[i].shuxi[j] = rs.Carts[i].GoodsAttribute.split(',')[j]
            }
            //获取是否有失效商品
            shixiao.push(rs.Carts[i].Status)
        }
        if (shixiao.indexOf(0) <= -1) {
            $('.clearsc').hide();
        }
        new Vue({
            el: '#shopcar',
            data: rs,
            ready: function () {
                $.RMLOAD();
                ocho();
                acho();
                bj();
                com();
                js();

            }
        })
    }


    function TotalMoney() {
        var money = 0.00;
        var i = 0
        $('.box').each(function () {
            if ($(this).find('.ccheck').hasClass('cur')) {
                var num = parseFloat($(this).find('.price .amo').attr('data-price'))
                money = parseFloat(money + num);
                i++;
            }
        })

        $('.car-list').find('.total .amo').attr('data-price', money.toFixed(2))
        GetPrice($('.car-list').find('.total .amo'))
        $('.car-list').find('.balance span').html(i)
    }

    function ocho() {
        //单选
        $('.box .ccheck').on('click', function () {
            if ($(this).hasClass('cur')) {
                $(this).removeClass('cur');
            } else {
                $(this).addClass('cur');
            }
            TotalMoney()
        })
    }

    function acho() {
        //全选
        $('.car-list .choose').on('click', function () {
            if ($(this).find('.pcheck').hasClass('cur')) {
                $(this).find('.pcheck').removeClass('cur');
                $('.box .ccheck').removeClass('cur');
            } else {
                $(this).find('.pcheck').addClass('cur');
                $('.box .ccheck').addClass('cur');
            }
            TotalMoney()
        })
    }

    function js() {
        //加
        $('.box .jia').on('click', function () {
            var stock = $(this).parents('.numbox').attr('data-max');
            // console.log(stock);
            var num = $(this).parents('.numbox').find('.amount').val();

            if (Number(num) >= Number(stock)) {
                num = stock;
            } else {
                num++;
            }
            // console.log(num);
            $(this).parents('.numbox').find('.amount').val(num)
        })
        //减
        $('.box .jian').on('click', function () {
            var num = $(this).parents('.numbox').find('.amount').val();
            if (num <= 1) {
                num = 1;
            } else {
                num--;
            }
            $(this).parents('.numbox').find('.amount').val(num)
        })
    }

    function bj() {
        //编辑
        $('.box .edit').on('click', function () {
            $(this).parents('.box').find('.editbox').show();
        })
        //删除
        $('.box .delete').on('click', function () {
            var id = $(this).parents('.box').attr('data-id');

            $(this).parents('.box').remove();
            TotalMoney()
            $.ajax({

                url: '/Api/v1/Mall/Cart/' + id,
                type: 'DELETE',
                data: {
                    CartId: id
                }
            }).done(function (rs) {
                if (rs.returnCode == '200') {
                    oppo('删除成功', 1, function () {
                        //window.location.href="/Html/ShopCar/ShopCar.html"
                    })
                } else {
                    if (rs.returnCode == '401') {
                        Backlog();
                    } else {
                        oppo(rs.msg, 1)
                    }
                }
            })
        })
    }

    //删除购物车商品

    function com() {
        //完成
        $('.box .ok').on('click', function () {
            var num = $(this).parents('.editbox').find('.amount').val();
            var unit = parseFloat($(this).parents('.box').find('.amo').attr('data-unit'));
            var sum = parseFloat(num * unit);
            $(this).parents('.box').find('.price .amo').attr('data-price', sum.toFixed(2));
            GetPrice($(this).parents('.box').find('.price .amo'))
            $(this).parents('.box').find('.num span').html(num);
            $(this).parents('.editbox').hide();
            TotalMoney()
            //编辑购物车
            var id = $(this).parents('.box').attr('data-id');
            $.ajax({
                url: '/Api/v1/Mall/Cart',
                type: 'patch',
                data: {
                    CartId: id,
                    Quantity: num
                }
            }).done(function (rs) {
                if (rs.returnCode != '200') {
                    oppo(rs.msg, 1)
                }
            })

        })
    }

    //清楚失效商品
    $('.clearsc').on('click', function () {
        $.ajax({
            url: '/Api/v1/Mall/Cart/Clear',
            type: 'DELETE'
        }).done(function (rs) {
            if (rs.returnCode == '200') {
                oppo('清除成功', 1, function () {
                    window.location.href = "/Html/ShopCar/ShopCar.html"
                })
            } else {
                if (rs.returnCode == '401') {
                    Backlog();
                } else {
                    oppo(rs.msg, 1)
                }
            }
        })
    })
    //提交
    $('.balance').on('click', function () {

        var ids = {};
        ids.CartIds = [];

        $('.box').each(function () {
            if ($(this).find('.ccheck').hasClass('cur')) {
                ids.CartIds.push($(this).attr('data-id'))
            }
        })
        //console.log(ids)
        link()
        //submitajax(ids);
    })
    function submitajax(ids) {
        $.ajax({
            url: '/Api/v1/Mall/OrderCalculation',
            data: ids,
            type: 'post'
        }).done(function (rs) {
            if (rs.returnCode == '200') {

                oppo('提交成功', 1, function () {

                })
            } else {
                if (rs.returnCode == '401') {
                    Backlog();
                } else {
                    oppo(rs.msg, 1)
                }
            }
        })
    }

    //确认订单链接
    function link() {
        var idstr = '';
        if ($('.ccheck.cur').length == 0) {
            oppo('请选择商品', 1)
        } else {
            $('.ccheck.cur').each(function (index) {

                if (index != 0) {
                    idstr += '|'
                }
                idstr += $(this).parent().attr('data-id')

            })
            window.location.href = "/Html/ShopCar/Confirm.html?id=" + idstr + "&type=0"
        }

    }
})