$(function () {
    $.ADDLOAD()
    $.checkuser();
    var id = $.getUrlParam('id');   //购物车id
    var gid = $.getUrlParam('gid'); //单品id和数量
    var addid = $.getUrlParam('addid');
    var type = $.getUrlParam('type');           //	1投资品2消费品
    var mode = $.getUrlParam('mode');          //0自提1邮寄
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
    ids.OrderType = '1';
    console.log(ids);
    new Vue({
        el: '#main',
        data: {
            info: [],
            data1: ids,
            type: type,
            mode: mode
        },
        ready: function () {
            var _this = this;
            _this.ajax();
            _this.$nextTick(function () {
                _this.link();
                _this.prosubmit();
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
                        $.RMLOAD();
                    }
                })
            },
            link: function () {

                //选择地址
                if (id) {
                    $('#main').on('click', '.choadd .a', function () {

                        window.location.href = "/Html/chooseaddress.html?id=" + id + '&type=' + type + '&mode=' + mode
                    })
                    $('#main').on('click', '.addadd .a', function () {
                        window.location.href = "/Html/chooseaddaddress.html?id=" + id + '&type=' + type + '&mode=' + mode
                    })
                } else {

                    $('#main').on('click', '.choadd .a', function () {
                        window.location.href = "/Html/chooseaddress.html?gid=" + gid + '&type=' + type + '&mode=' + mode
                    })
                    $('#main').on('click', '.addadd .a', function () {
                        window.location.href = "/Html/chooseaddaddress.html?gid=" + gid + '&type=' + type + '&mode=' + mode
                    })
                }


                //进入商品明细
                $('#main').on('click', '.godeta', function () {
                    window.location.href = "/Html/shopcardetails.html?id=" + id
                })
            },
            //价格计算
            price: function () {
                console.log(1213)
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
                        var message = {
                            Consignee: _this.info.Addresses.Contacts,
                            Province: _this.info.Addresses.Province,
                            City: _this.info.Addresses.City,
                            District: _this.info.Addresses.District,
                            Street: _this.info.Addresses.Street,
                            RegionName: _this.info.Addresses.RegionName,
                            Address: _this.info.Addresses.Address,
                            Tel: _this.info.Addresses.Phone,
                            Memo: $('.bz').val(),
                            Goods: prodata,
                            Type: type,
                            PickUp: mode
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
                        if (mode == 1) {
                            window.location.replace("/Html/pay.html?id=" + rs.data.Id + '&time=' + rs.data.CreateTime + '&type=' + type)
                        } else {

                        }

                    }
                })
            }

        }
    })
})