$(function () {
    $.ADDLOAD()
    var OrderNo = $.getUrlParam('OrderNo');
    var Id = $.getUrlParam('id'); //订单id
    var money = $.getUrlParam('money');
    money = Number(money).toFixed(2);
    var time = $.getUrlParam('time');
    var yhq = $.getUrlParam('yhq');
    if(yhq){
        yhq = Number(yhq).toFixed(2);
    }else{
        yhq=0
    }

    if (time) {
        time = time.toString().replace(/-/g, "/");
    }
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
            zongji:Number(yhq)+Number(money),
            haix: (money - yhq).toFixed(2),
            orderid: $('#orderid').val(Id)
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
                        } else if ($('.huodao').hasClass('cur')) {
                            $.ajax({
                                url:'/Payment/PayOnDelivery/'+Id,
                                type:'post',
                                dataType:'json'
                            }).done(function (rs) {
                                if(rs.returnCode=='200'){
                                    // window.location.replace("/Html/html/personalcenter/personalcenter.html");
                                }
                            })
                            return false;
                        }
                        return true;
                    } else {
                        //判断支付类型
                        if ($('.alipay').hasClass('cur')) {
                            $('#paymentCode').val('alipay');
                        } else if ($('.weixin').hasClass('cur')) {
                            $('#paymentCode').val('weixin');
                        } else if ($('.huodao').hasClass('cur')) {
                            $.ajax({
                                url:'/Payment/PayOnDelivery/'+Id,
                                type:'post',
                                dataType:'json'
                            }).done(function (rs) {
                                if(rs.returnCode=='200'){
                                    // window.location.replace("/Html/html/personalcenter/personalcenter.html");
                                }
                            })
                            return false;
                        }
                        $('#formid').attr('action', '/Payment/H5/Pay')
                        return true;
                    }
                });
            }
        }
    })
})