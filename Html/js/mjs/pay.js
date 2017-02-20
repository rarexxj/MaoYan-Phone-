$(function () {
    $.ADDLOAD()
    var Id = $.getUrlParam('id'); //订单id
    var money = $.getUrlParam('money');
    var time = $.getUrlParam('time');
    var type = $.getUrlParam('type');
    var fkzt = $.getUrlParam('fkzt');
    if (!fkzt) {
        fkzt = 0
    }
    if (time) {
        time = time.toString().replace(/-/g, "/");
    }
    var GoodsDeposit = $.getUrlParam('GoodsDeposit');
    // $('.orderno').html(OrderNo);
    // $('.jiage').html(Number(money).toFixed(2))
    $('#orderid').val(Id);
    $.checkuser();
    new Vue({
        el: '#pay_mon',
        data: {
            info: [],
            telinfo:[],
            tipinfo:[],
            data1: {
                pageNo: 1,
                limit: 8
            }
        },
        ready: function () {
            var _this = this;
            _this.infoajax();
            _this.tipajax();
            _this.$nextTick(function () {
                if(type!=1&&fkzt!=2){
                    _this.countDown(time, '.deadline');
                }
                if(type==1&&fkzt==2){
                    $('.deadline').hide()
                }
                _this.choosePay();
                setTimeout(function () {
                    _this.paymode();
                }, 100)
                setTimeout(function () {
                    _this.moreprice();
                    $.RMLOAD();
                }, 200)

            })
        },
        methods: {
            infoajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Mall/Order/' + Id,
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data
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
            moreprice: function () {
                var needmore = $('.shifu').html() - $('.dingj').html()
                console.log($('.shifu').html())
                $('.haixu').html(Number(needmore).toFixed(2))
            },
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
                            $('#paytype').attr('value',1)
                        }
                        if (type == 1) {
                            if (fkzt == 2) {
                                $('#paytype').attr('value',1)
                            } else {
                                $('#paytype').attr('value',0)
                            }
                        }
                        if ($.is_weixin()) {
                            //如果是选择的银行卡，显示遮罩
                            submitForm.attr("action","/Payment/Process/WeiXin/")
                            return true;
                        } else {
                            //判断支付类型
                            console.log(123)
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
            },
            tipajax:function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Page/04',
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.tipinfo = rs.data;
                    }
                })
            }
        }
    })
})