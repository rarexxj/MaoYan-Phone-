$(function () {
    $.ADDLOAD();
    $.checkuser();
    new Vue({
        el: '#shopcar',
        data: {
            info: []
        },
        ready: function () {
            var _this = this;
            _this.infoajax();
            _this.$nextTick(function () {
                _this.oneclick();
                _this.jisuanjs();
                _this.clicom();
                _this.shixiao();
                _this.tijiao();
                _this.bianj();

            })
        },
        methods: {
            infoajax: function () {
                var _this = this;
                $.ajax({
                    url: URL+'/Api/v1/Mall/Cart',
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        if (rs.data.Carts.length == 0) {
                            $('.search-box').show();
                            $('.nothing').show();
                        }
                        var shixiao = [];
                        for (i in rs.data.Carts) {
                            rs.data.Carts[i].allprice = rs.data.Carts[i].Price * rs.data.Carts[i].Quantity;
                            //获取属性数组
                            rs.data.Carts[i].shuxi = []
                            // if (rs.data.Carts[i].GoodsAttribute.split(',')) {
                            //     for (j in rs.data.Carts[i].GoodsAttribute.split(',')) {
                            //         rs.data.Carts[i].shuxi[j] = rs.data.Carts[i].GoodsAttribute.split(',')[j]
                            //     }
                            // }
                            //获取是否有失效商品
                            shixiao.push(rs.data.Carts[i].Status)
                        }
                        if (shixiao.indexOf(0) <= -1) {
                            $('.clearsc').hide();
                        }
                        //最多选择库存
                        // for (var i = 0; i < rs.data.Carts.length; i++) {
                        //     if (rs.data.Carts[i].Quantity >= rs.data.Carts[i].Stock) {
                        //         rs.data.Carts[i].Quantity = rs.data.Carts[i].Stock;
                        //         console.log(rs.data)
                        //     }
                        // }
                        _this.info = rs.data;
                        $.RMLOAD()
                    }
                })
            },
            oneclick: function () {
                var _this = this;
                //单选
                $('#shopcar').on('click', '.box .ccheck', function () {
                    if ($(this).hasClass('cur')) {
                        $(this).removeClass('cur');
                    } else {
                        $(this).addClass('cur');
                    }
                    _this.TotalMoney()
                })
                //全选
                $('#shopcar').on('click', '.car-list .choose', function () {
                    if ($(this).find('.pcheck').hasClass('cur')) {
                        $(this).find('.pcheck').removeClass('cur');
                        $('.box .ccheck').removeClass('cur');
                    } else {
                        $(this).find('.pcheck').addClass('cur');
                        $('.box .ccheck').addClass('cur');
                    }
                    _this.TotalMoney()
                })
            },
            jisuanjs: function () {
                //加
                $('#shopcar').on('click', '.box .jia', function () {
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
                $('#shopcar').on('click', '.box .jian', function () {
                    var num = $(this).parents('.numbox').find('.amount').val();
                    if (num <= 1) {
                        num = 1;
                    } else {
                        num--;
                    }
                    $(this).parents('.numbox').find('.amount').val(num)
                })
            },
            bianj: function () {

                //编辑
                $('#shopcar').on('click', '.box .edit', function () {

                    console.log(231312)
                    $(this).parents('.box').find('.editbox').show();
                })
                //删除
                $('#shopcar').on('click', '.box .delete', function () {
                    var id = $(this).parents('.box').attr('data-id');

                    $(this).parents('.box').remove();
                    // TotalMoney()
                    $.ajax({

                        url: URL+'/Api/v1/Mall/Cart/' + id,
                        type: 'DELETE',
                        data: {
                            CartId: id
                        }
                    }).done(function (rs) {
                        if (rs.returnCode == '200') {
                            $.oppo('删除成功', 1, function () {
                                //window.location.href="/Html/ShopCar/ShopCar.html"
                            })
                        }
                    })
                })
            },
            clicom: function () {
                var _this = this;
                //完成
                $('#shopcar').on('click', '.box .ok', function () {
                    var num = $(this).parents('.editbox').find('.amount').val();
                    var unit = parseFloat($(this).parents('.box').find('.amo').attr('data-unit'));
                    var sum = parseFloat(num * unit);
                    $(this).parents('.box').find('.price .amo').attr('data-price', sum.toFixed(2));
                    // GetPrice($(this).parents('.box').find('.price .amo'))
                    $(this).parents('.box').find('.num span').html(num);
                    $(this).parents('.editbox').hide();
                    _this.TotalMoney()
                    //编辑购物车
                    var id = $(this).parents('.box').attr('data-id');
                    $.ajax({
                        url: URL+'/Api/v1/Mall/Cart',
                        type: 'patch',
                        data: {
                            CartId: id,
                            Quantity: num
                        }
                    }).done(function (rs) {
                        if (rs.returnCode != '200') {
                            $.oppo(rs.msg, 1)
                        }
                    })

                })
            },
            shixiao: function () {
                //清楚失效商品
                $('#shopcar').on('click', '.clearsc', function () {
                    $.ajax({
                        url: URL+'/Api/v1/Mall/Cart/Clear',
                        type: 'DELETE'
                    }).done(function (rs) {
                        if (rs.returnCode == '200') {
                            $.oppo('清除成功', 1, function () {
                                window.location.href = "/Html/html/shopcar/shopcar.html"
                            })
                        }
                    })
                })
            },
            tijiao: function () {
                var _this = this;
                //提交
                $('#shopcar').on('click', '.balance', function () {
                    var ids = {};
                    ids.CartIds = [];
                    $('.box').each(function () {
                        if ($(this).find('.ccheck').hasClass('cur')) {
                            ids.CartIds.push($(this).attr('data-id'))
                        }
                    })
                    _this.link()
                })
            },
            TotalMoney: function () {
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
                $('.car-list').find('.total .amo').html($('.car-list').find('.total .amo').attr('data-price'))
                // GetPrice($('.car-list').find('.total .amo'))
                $('.car-list').find('.balance span').html(i)
            },
            link: function () {
                var idstr = '';
                if ($('.ccheck.cur').length == 0) {
                    $.oppo('请选择商品', 1)
                } else {
                    $('.ccheck.cur').each(function (index) {
                        if (index != 0) {
                            idstr += '|'
                        }
                        idstr += $(this).parent().attr('data-id')

                    })
                    window.location.href = "/Html/html/shopcar/settlement.html?id=" + idstr
                }
            }

        }
    })
})


