$(function () {
    $.ADDLOAD();
    $.checkuser();
    var id = $.getUrlParam('id');   //购物车id
    var gid = $.getUrlParam('gid'); //单品id和数量
    var type = $.getUrlParam('type');           //	1投资品2消费品
    var mode = $.getUrlParam('mode');          //0自提1邮寄
    var shopid = $.getUrlParam('shopid');
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
    if (mode) {
        $('.submit').show()
    } else {
        $('.submit').hide()
    }
    new Vue({
        el: '#main',
        data: {
            info: [],
            data1: {},
            numinfo: []
        },
        ready: function () {
            var _this = this;
            _this.infoajax();
            _this.numajax();
            _this.$nextTick(function () {
                _this.swipe();
                _this.zitsubmit();
                $.RMLOAD();
            })
        },
        methods: {
            infoajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Merchant/' + shopid,
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data;
                    }
                })
            },
            numajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Mall/OrderCalculation',
                    type: 'post',
                    data: ids
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.numinfo = rs.data;


                        var prodata = []
                        for (var i = 0; i < _this.numinfo.Goods.List.length; i++) {
                            var pro = {}
                            pro.Id = _this.numinfo.Goods.List[i].SingleGoodsId;
                            pro.Quantity = _this.numinfo.Goods.List[i].Quantity;
                            prodata.push(pro)
                        }
                        _this.data1 = {
                            MerchantId: shopid,
                            PickUp: 0,
                            Type: type,
                            Goods: prodata
                        }
                    }
                })
            },
            //banner滚动
            swipe: function () {
                new Swiper('.swiper-container', {
                    direction: 'horizontal',
                    loop: true,

                    // 如果需要分页器
                    pagination: '.swiper-pagination'
                })
            },
            zitsubmit: function () {
                var _this = this;
                $('#main').on('click', '.submit', function () {
                    $.ajax({
                        url: '/Api/v1/Mall/Order',
                        type: 'POST',
                        data: _this.data1
                    }).done(function (rs) {
                        if (rs.returnCode == '200') {
                            window.location.replace("/Html/pay.html?id=" + rs.data.Id + '&time=' + rs.data.CreateTime + '&type=' + type)
                        }
                    })
                })
            }
        }
    })
})