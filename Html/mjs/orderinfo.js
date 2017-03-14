$(function () {
    $.ADDLOAD()
    var id = $.getUrlParam('id')
    $.checkuser();
    new Vue({
        el: '#main',
        data: {
            info: [],
            time: '',
            id:'',
            money:'',
            filesid:''
        },
        ready: function () {
            var _this = this;
            _this.infoajax();
            _this.btnclick();
            _this.$nextTick(function () {

            })
        },
        methods: {
            infoajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Mall/Order/' + id,
                    type: 'GET'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data;
                        _this.time = rs.data.CreateTime.toString().replace(/-/g, "/");
                        ;
                        _this.id=rs.data.Id;
                        _this.money=rs.data.PayFee;
                        console.log(_this.time);
                        _this.$nextTick(function () {
                            if (rs.data.OrderStatus == 0) {
                                _this.countDown(_this.time, '.deadline');
                            }
                        })
                        var a = [];
                        for (i = 0; i < rs.data.OrderGoods.length; i++) {
                            a.push(rs.data.OrderGoods[i].GoodsImage.Id);
                        }
                        b=a.join('|');
                        _this.filesid=b;
                        $.RMLOAD();
                    }
                })
            },
            btnclick: function () {
                var _this = this;
                //取消订单
                $('#main').on('click', '.can-btns', function () {
                    _this.cancleajax();
                })
                //确认收货
                $('#main').on('click', '.confirm-btn', function () {
                    _this.sureajax();
                })
                //提醒发货
                $('#main').on('click', '.remind-btn', function () {
                    var id = $('.orderdetails').attr('data-orderId');
                    _this.remindajax(id)
                })
                //确认删除订单
                $('#main').on('click', '.delete-btn', function () {
                    _this.deletajax();
                })

                $('#main').on('click', '.back', function () {
                    _this.tuikclick();
                })
            },
            tuikclick: function () {
                var _this=this;
                window.location.href='/Html/html/personalcenter/tuiktype.html?oid='+_this.id+'&mp='+_this.money+'&filesid='+_this.filesid
            },
            cancleajax: function () {
                $.ajax({
                    url: '/Api/v1/Order/' + id + '/Cancel',
                    type: 'PATCH'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        $.oppo('成功取消订单', 1, function () {
                            window.location.replace("/Html/html/personalcenter/myorder.html?orderType=0")
                        })
                    }
                })
            },
            sureajax: function () {
                $.ajax({
                    url: '/Api/v1/Order/' + id + '/Complete',
                    type: 'PATCH'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        $.oppo('成功确认收货', 1, function () {
                            window.location.replace("/Html/html/personalcenter/myorder.html?orderType=0")
                        })
                    }
                })
            },
            remindajax: function () {
                $.ajax({
                    url: '/Api/v1/Order/' + id + '/Remind',
                    type: 'post'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        $.oppo('提醒成功', 1)
                    }
                })
            },
            deletajax: function () {
                $.ajax({
                    url: '/Api/v1/Order/' + id,
                    type: 'DELETE'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        $.oppo('成功删除订单', 1, function () {
                            window.location.replace("/Html/html/personalcenter/myorder.html?orderType=0")
                        })
                    }
                })
            },
            countDown: function (time, id) {
                var btn = true;
                var minute_elem = $(id).find('.min');
                var second_elem = $(id).find('.sec');
                var end_time = new Date(time).getTime() + 60 * 60 * 1000; //月份是实际月份-1
                var sys_second = (end_time - new Date().getTime()) / 1000;
                if (btn) {
                    var minute = Math.floor((sys_second / 60) % 60);
                    var second = Math.floor(sys_second % 60);
                    $(minute_elem).html(minute < 10 ? "0" + minute : minute); //计算分钟
                    $(second_elem).html(second < 10 ? "0" + second : second); //计算秒

                    var index = setInterval(function () {
                        if (sys_second > 1) {
                            sys_second = sys_second - 1;
                            var minute = Math.floor((sys_second / 60) % 60);
                            var second = Math.floor(sys_second % 60);
                            $(minute_elem).html(minute < 10 ? "0" + minute : minute); //计算分钟
                            $(second_elem).html(second < 10 ? "0" + second : second); //计算秒杀
                        } else {
                            window.location.replace("/Html/html/personalcenter/personalcenter.html")
                            clearInterval(index);
                            return; //停止下面代码执行
                        }
                    }, 1000)
                }
            }

        }
    })
})