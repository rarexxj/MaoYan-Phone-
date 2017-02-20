$(function () {
    $.ADDLOAD()
    var orderType = $.getUrlParam('orderType');
    $.checkuser();
    new Vue({
        el: '#main',
        data: {
            info: {},
            data1: {
                orderType: orderType,
                pageNo: 1,
                limit: 10,
                type: 2
                // keywords:''
            }
        },
        ready: function () {
            var _this = this;
            // if(_this.info.TotalCount==0){
            //     $('.orderbox').hide();
            //     $('.nothingbox').show();
            // }
            _this.infoajax();
            _this.navlick();
            _this.btnclick();
            _this.link();
            _this.swip();
            _this.$nextTick(function () {
                $.RMLOAD()
            })
        },
        methods: {
            infoajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Mall/Order',
                    data: _this.data1,
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data;
                        if(rs.data.Orders.length==0){
                            $('.nothing').show();
                            $('.orderbox').hide();
                        }
                    }
                })
            },
            navlick: function () {
                $('.ordernav a').eq(orderType).addClass('active').siblings().removeClass('active')
            },
            btnclick: function () {
                var _this = this;
                //遮罩
                $('#main').on('click', '.delete-btns', function () {
                    //获取id
                    var dataorderId = $(this).parents('.box').attr('data-orderId');
                    $('.pop-del').show().find('.box').attr('data-orderId', dataorderId)
                })
                $('#main').on('click', '.canclebtn', function () {
                    //获取id
                    var dataorderId = $(this).parents('.box').attr('data-orderId');
                    $('.pop-can').show().find('.box').attr('data-orderId', dataorderId)
                })
                $('#main').on('click', '.confirm-btns', function () {
                    //获取id
                    var dataorderId = $(this).parents('.box').attr('data-orderId');
                    $('.pop-com').show().find('.box').attr('data-orderId', dataorderId)
                })

                $('#main').on('click', '.pop .no', function () {
                    $('.pop').hide();
                })
                //确认删除订单
                $('#main').on('click', '.delete-btn', function () {
                    var id = $(this).parents('.box').attr('data-orderId');
                    _this.deletajax(id);
                })
                //取消订单
                $('#main').on('click', '.cancle-btn', function () {
                    var id = $(this).parents('.box').attr('data-orderId');
                    _this.cancleajax(id);
                })
                //确认收货
                $('#main').on('click', '.confirm-btn', function () {
                    var id = $(this).parents('.box').attr('data-orderId');
                    _this.sureajax(id);
                })
                //提醒发货
                $('#main').on('click', '.remind-btn', function () {
                    var id = $(this).parents('.box').attr('data-orderId');
                    _this.remindajax(id)
                })
                //付款
                $('#main').on('click','.fukuan',function(){

                })
            },
            deletajax: function (id) {
                $.ajax({
                    url: '/Api/v1/Order/' + id,
                    type: 'DELETE'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        $('.pop-del').hide();
                        $.oppo('成功删除订单', 1, function () {
                            window.location.replace("/Html/costorder.html?orderType=0")
                        })
                    }
                })
            },
            cancleajax: function (id) {
                $.ajax({
                    url: '/Api/v1/Order/' + id + '/Cancel',
                    type: 'PATCH'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        $('.pop-can').hide();
                        $.oppo('成功取消订单', 1, function () {
                            window.location.replace("/Html/costorder.html?orderType=0")
                        })
                    }
                })
            },
            sureajax: function (id) {
                $.ajax({
                    url: '/Api/v1/Order/' + id + '/Complete',
                    type: 'PATCH'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        $('.pop-com').hide();
                        $.oppo('成功确认收货', 1, function () {
                            window.location.replace("/Html/costorder.html?orderType=0")
                        })
                    }
                })
            },
            remindajax: function (id) {
                $.ajax({
                    url: '/Api/v1/Order/' + id + '/Remind',
                    type: 'post'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        $.oppo('提醒成功', 1)
                    }
                })
            },
            link: function () {
                $('.my-order').on('click', '.normal>a', function () {
                    var id = $(this).parent().attr('data-orderId');
                    window.location.href = "/Html/costorderdetails.html?id=" + id;
                })
                // $('.my-order').on('click', '.normal .back', function (event) {
                //
                //     event.stopPropagation();
                //     var id = $(this).parents('.box').attr('data-id');
                //     var gid = $(this).parents('.pro-style').attr('data-id');
                //     window.location.href = "/Html/AfterSales/Info.html?oid=" + id + "&gid=" + gid;
                // })

            },
            swip:function () {
                var mySwiper = new Swiper ('.swiper-container', {
                    slidesPerView : 'auto'
                })
            }
        }
    })
})