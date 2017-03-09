$(function () {
    $.ADDLOAD()
    $.checkuser();
    var id = $.getUrlParam('id');   //购物车id
    var gid = $.getUrlParam('gid'); //单品id和数量
    var addid = $.getUrlParam('addid');
    var jjid=$.getUrlParam('jjid');
    if (gid) {
        var gid2 = gid.split('|')[0]; //单品id
    }
    var ids = {}
    if (id) {
        if (id.indexOf('|') > 0) {
            ids.CartIds = id.split('|')
        } else {
            ids.CartIds = id
        }
    } else {
        ids.IsFromCart = false;
    }
    if (gid) {
        ids.SingleGoods = [];
        var a = {}
        a.SingleGoodsId = gid.split('|')[0];
        a.Quantity = gid.split('|')[1];
        ids.SingleGoods.push(a)
    }
    if (addid) {
        ids.AddressId = addid
    }
    console.log(jjid)
    if(jjid!=0){
        ids.PurchaseId=jjid
    }else{
        jjid=''
    }
    console.log(ids);
    new Vue({
        el: '#main',
        data: {
            info: [],
            data1: ids,
            yhqinfo:[]
        },
        ready: function () {
            var _this = this;
            _this.ajax();
            _this.$nextTick(function () {
                _this.link();
                _this.js();
                _this.getCoupon();
                _this.prosubmit();
                _this.youhq();
                _this.getsettime();
                // $.RMLOAD();
            })
        },
        methods: {
            ajax: function () {
                var _this = this;
                $.ajax({
                    url: "/Api/v1/Mall/OrderCalculation",
                    data: _this.data1,
                    type: 'post'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data;
                        _this.price();
                        //计算总价
                        rs.data.allprice = parseFloat(rs.data.GoodsAmount + rs.data.ShippingFee)
                        $.RMLOAD();
                    }
                })
            },
            link: function () {
                //选择地址
                if (id) {
                    $('#main').on('click', '.choadd .a', function () {
                        window.location.href = "/Html/html/shopcar/chooseaddress.html?id=" + id
                    })
                    $('#main').on('click', '.addadd .a', function () {
                        window.location.href = "/Html/html/shopcar/chooseaddaddress.html?id=" + id
                    })
                } else {
                    $('#main').on('click', '.choadd .a', function () {
                        window.location.href = "/Html/html/shopcar/chooseaddress.html?gid=" + gid
                    })
                    $('#main').on('click', '.addadd .a', function () {
                        window.location.href = "/Html/html/shopcar/chooseaddaddress.html?gid=" + gid
                    })
                }


                //进入商品明细
                $('#main').on('click', '.godeta', function () {
                    window.location.href = "/Html/html/shopcar/shopcardetails.html?id=" + id
                })
            },
            //价格计算
            price: function () {
                var _this = this;
                $('.need').html((_this.info.GoodsAmount) - (_this.info.GoodsDeposit));
            },
            //提交订单
            prosubmit: function () {
                var _this = this;
                $('.balance').on('click', function () {
                    if ($('.addadd').length) {
                        $.oppo('请选择收货地址', 1)
                    } else {
                        var prodata = []
                        for (var i = 0; i < _this.info.Goods.List.length; i++) {
                            var pro = {}
                            pro.Id = _this.info.Goods.List[i].SingleGoodsId;
                            pro.Quantity = _this.info.Goods.List[i].Quantity;
                            prodata.push(pro)
                        }
                        var num = 0
                        if ($('.weui_switch').is(':checked')) {
                            num = $('.weui_switch').attr('data-int')
                        } else {
                            num = 0
                        }
                        var message = {
                            Consignee: _this.info.Addresses.Contacts,
                            Province: _this.info.Addresses.Province,
                            City: _this.info.Addresses.City,
                            District: _this.info.Addresses.District,
                            Street: _this.info.Addresses.Street,
                            Address: _this.info.Addresses.Address,
                            Tel: _this.info.Addresses.Phone,
                            Memo: $('.bz').val(),
                            Goods: prodata,
                            CouponId: $('.youhq.active').attr('data-id') ? $('.youhq.active').attr('data-id') : null,
                            GoodId:jjid,
                            BestTime:$('.date').val()
                        }
                        console.log(message)
                        _this.inputajax(message);
                    }
                })
            },
            inputajax: function (message) {
                $.ajax({
                    url: '/Api/v1/Mall/Order',
                    data: message,
                    type: 'post'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        if ($('.car-list .amo').attr('data-price') == 0) {
                            window.location.replace("/Html/Member/PersonalCenter.html")
                        } else {
                            window.location.replace("/Html/html/shopcar/pay.html?id=" + rs.data.Id + '&OrderNo=' + rs.data.OrderNo + '&money=' + rs.data.PayFee + '&time=' + rs.data.CreateTime)
                        }
                    }
                })
            },
            js: function () {
                var _this=this;
                var n = $('.weui_switch').attr('data-money')
                if ($('.weui_switch').is(':checked')) {
                    $('.weui_switch').attr('data-price', n)
                } else {
                    $('.weui_switch').attr('data-price', 0)
                }
                _this.getLastPrice();
            },
            getCoupon: function () {
                var _this=this;
                $('#yhq').on('click', function () {
                    $('.confirm-order').hide();
                    $('.youhq-box').show();
                })
                $('.yhq-btn').on('click', function () {
                    $('.confirm-order').show();
                    $('.youhq-box').hide();
                })
                $('.youhq-box .yhq-btn').on('click', function () {  //确认选择优惠券
                    $('.confirm-order').show();
                    if ($('.youhq-box .youhq.active').length == 0) {
                        $('#yhq').attr('data-price', 0)
                    } else {
                        var price = $('.youhq-box .youhq.active').attr('data-price');
                        $('#yhq').attr('data-price', price)
                    }
                    _this.getLastPrice()
                })

                $('.youhq-box').on('click', '.youhq', function () {
                    if ($(this).hasClass('active')) {
                        $(this).removeClass('active');
                    } else {
                        $(this).addClass('active').siblings().removeClass('active');
                    }
                })
            },
            getLastPrice: function () {
                // var money = parseFloat($('.car-list .amo').attr('data-price2') - $('.weui_switch').attr('data-price') - $('#yhq').attr('data-price'));
                var money = parseFloat($('.car-list .amo').attr('data-price2') - $('#yhq').attr('data-price'));
                if (money < 0) {
                    money = 0
                }
                money = parseFloat(Number(money) + Number($('.postage').attr('data-postage'))).toFixed(2);
                $('.car-list .amo').attr('data-price', money);
                var price = $('.car-list').find('.amo').attr('data-price');
                $('.car-list').find('.amo').html(price)
            },
            getsettime:function () {
                $('.settime').on('click', function () {
                    $('.confirm-order').hide();
                    $('.timebox').show();
                })
                $('.time-btn').on('click', function () {
                    $('.confirm-order').show();
                    $('.timebox').hide();
                })
            },
            youhq:function () {
                var _this=this;
                $.ajax({
                    url: '/Api/v1/Coupon',
                    type: 'get',
                    dataType:'json',
                    data:{
                        isAvailable:0
                    }
                }).done(function (rs) {
                    if (rs.returnCode == 200) {
                        _this.yhqinfo=rs.data.Coupons;
                    }
                })

            }
        }
    })
})



