$(function () {
    $.ADDLOAD()
    var OrderNo = $.getUrlParam('OrderNo');
    var Id = $.getUrlParam('id'); //订单id
    var money = $.getUrlParam('money');
    money = Number(money).toFixed(2);
    var time = $.getUrlParam('time');
    var yhq = $.getUrlParam('yhq');
    yhq = Number(yhq).toFixed(2);
    if (time) {
        time = time.toString().replace(/-/g, "/");
    }
    var GoodsDeposit = $.getUrlParam('GoodsDeposit');
    // $('.orderno').html(OrderNo);
    // $('.jiage').html(Number(money).toFixed(2))
    $.checkuser();
    new Vue({
        el: '#pay_mon',
        data: {
            info: [],
            OrderNo: OrderNo,
            money: money,
            needmore: ' ',
            paymentCode: '',
            yhq: yhq,
            haix: (money - yhq).toFixed(2)
        },
        ready: function () {
            var _this = this;
            // _this.infoajax();
            _this.$nextTick(function () {
                _this.countDown(time, '.deadline');
                _this.choosePay();
                _this.paymode();
                $.RMLOAD();
            })
        },
        methods: {
            infoajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Payment/' + _this.paymentCode + '/SignInfo/' + Id,
                    type: 'post',
                    dataType: 'json'
                    // data:_this.data
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data;
                        $.RMLOAD();
                    }
                })
            },
            countDown: function (time, id) {
                var btn = true;
                var minute_elem = $(id).find('.min');
                var second_elem = $(id).find('.sec');
                var end_time = new Date(time).getTime() + 60 * 60 * 1000, //月份是实际月份-1
                    sys_second = (end_time - new Date().getTime()) / 1000;
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
            },
            choosePay: function () {
                $('.pay-btn').on('click', function () {
                    $(this).addClass('cur').siblings('.pay-btn').removeClass('cur')
                })

                $('.main').on('click', '.mask', function () {
                    $(this).fadeOut()
                })
            },
            paymode: function () {
                var _this = this;
                $(".shoppay").on("click", '#subimitButton', function () {
                    if ($.is_weixin()) {
                        //如果是选择的支付宝，显示遮罩
                        if ($('.alipay').hasClass('cur')) {
                            $('.mask').fadeIn();
                            return false;
                        }
                        return true;
                    } else {
                        //判断支付类型
                        if ($('.alipay').hasClass('cur')) {
                            _this.paymentCode = 'alipay';
                            _this.infoajax();
                        } else if ($('.weixin').hasClass('cur')) {
                            _this.paymentCode = 'weixin';
                            _this.infoajax();
                        }else if($('.huodao').hasClass('cur')){
                            window.location.replace("/Html/html/personalcenter/personalcenter.html")
                        }
                        return true;
                    }
                });
            }
        }
    })
})