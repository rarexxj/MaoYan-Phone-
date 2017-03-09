$(function () {
    $.ADDLOAD()
    var OrderNo=$.getUrlParam('OrderNo');
    var Id = $.getUrlParam('id'); //订单id
    var money = $.getUrlParam('money');
    money=Number(money).toFixed(2);
    var time = $.getUrlParam('time');
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
            OrderNo:OrderNo,
            money:money,
            needmore: ' ',
            data:{
                paymentCode:'alipay',
                orderId:Id,
                useBalance:0
            }
        },
        ready: function () {
            var _this = this;
            // _this.infoajax();
            _this.$nextTick(function () {
                _this.countDown(time, '.deadline');
                _this.choosePay();
                setTimeout(function () {
                    _this.paymode();
                }, 100)
                $.RMLOAD();
            })
        },
        methods: {
            infoajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Payment/'+_this.data.paymentCode+'/SignInfo/'+_this.data.orderId,
                    type: 'post',
                    dataType:'json',
                    data:_this.data
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data;
                        _this.needmore = (Number(rs.data.GoodsAmount) - Number(rs.data.Deposit));
                        console.log(Number(rs.data.GoodsAmount))
                        $.RMLOAD();
                    }
                })
            },
            countDown: function (time, id) {
                var btn = true;
                var minute_elem = $(id).find('.min');
                var second_elem = $(id).find('.sec');
                var end_time = new Date(time).getTime() + 30 * 60 * 1000, //月份是实际月份-1
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
                            window.location.replace("/Html/my.html")
                            clearInterval(index);
                            return; //停止下面代码执行
                        }
                    }, 1000)
                }
            },
            // moreprice: function () {
            //     var needmore = $('.shifu').html() - $('.dingj').html()
            //     $('.haixu').html(Number(needmore).toFixed(2))
            // },
            choosePay: function () {
                $('.pay-btn').on('click', function () {
                    $(this).addClass('cur').siblings('.pay-btn').removeClass('cur')
                })
            },
            paymode: function () {
                var _this = this;
                $(".shoppay").on("click", '#subimitButton', function () {
                    if ($('.weixin').hasClass('cur')) {
                        var submitForm = $("#formid");
                        if (type == 2) {
                            $('#paytype').attr('value', 1)
                        }
                        if (type == 1) {
                            if (fkzt == 2) {
                                $('#paytype').attr('value', 1)
                            } else {
                                $('#paytype').attr('value', 0)
                            }
                        }
                        if ($.is_weixin()) {
                            //如果是选择的银行卡，显示遮罩
                            submitForm.attr("action", "/Payment/Process/WeiXin/")
                            return true;
                        } else {
                            //判断支付类型
                            submitForm.attr("action", "/Payment/H5/Pay");
                            return true;
                        }
                        return false;
                    }
                    if ($('.bankpay').hasClass('cur')) {
                        $('#subimitButton').attr('type', 'button')
                        window.location.href = '/Html/voucher.html?orderid=' + Id + '&type=' + type + '&fkzt=' + fkzt + '&OrderNo=' + _this.info.OrderNo

                    }
                });
            }
        }
    })
})