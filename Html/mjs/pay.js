/**
 * Created by admin on 2016/9/9.
 */
$(function () {
    $.ADDLOAD();
    var Id = $.getUrlParam('id');
    var OrderNo = $.getUrlParam('OrderNo');
    var money = $.getUrlParam('money');
    var time = $.getUrlParam('time');
    $('.orderno').html(OrderNo);
    $('#orderid').val(Id);
    cash();
    //金额
    var pricebox = {}
    if (money.toString().indexOf('.') > -1) {
        if (money.toString().split('.')[1].length == 1) {
            pricebox.prices2 = money.toString().split('.')[1] + '0'
        } else {
            pricebox.prices2 = money.toString().split('.')[1];
        }
        pricebox.prices1 = money.toString().split('.')[0];
        $('.npri .int').html(pricebox.prices1);
        $('.npri .dec').html(pricebox.prices2);
        $('.mpri .int').html(pricebox.prices1).attr('data-int', pricebox.prices1)
        $('.mpri .dec').html(pricebox.prices2).attr('data-dec', pricebox.prices2)
    } else {
        $('.npri .int').html(money);
        $('.npri .dec').html('00');
        $('.mpri .int').html(money).attr('data-int', money)
        $('.mpri .dec').html('00').attr('data-int', '00')
    }
    //计时
    // var str = time.toString().replace(/-/g,"/");

    // var date = new Date(time);
    // var deadline = date.getTime() + 30 * 60 * 1000;
    // var mytime = new Date()
    // var nowtime = mytime.getTime();
    // var last = (deadline - nowtime) / 1000;
    // var fen = parseInt(last / 60);
    // var miao = parseInt(last % 60);
    // if (fen < 10) {
    //     fen = '0' + fen
    // }
    // if (miao < 10) {
    //     miao = '0' + miao
    // }
    // $('.deadline .min').html(fen);
    // $('.deadline .min').attr('data-min', fen)
    // $('.deadline .sec').html(miao)
    // $('.deadline .sec').attr('data-sec', miao)
    // lasttime();
    //js();
    function wxp() {
        $('.pay-btn').on('click', function () {
            // if($(this).hasClass('cur')){
            //     $(this).removeClass('cur')
            // }else{
            //     $('.pay-btn').removeClass('cur');
            //     $(this).addClass('cur');
            // }
            $(this).addClass('cur').siblings('.pay-btn').removeClass('cur')
        })
    }

    function yep(rs) {
        $('.pay-mon').on('click', function () {
            if ($(this).hasClass('cur')) {
                $(this).removeClass('cur')
                $('#usebalance').val(0)
                $('.mpri .int').html($('.mpri .int').attr('data-int'))
                $('.mpri .dec').html($('.mpri .dec').attr('data-dec'))
            } else {
                $(this).addClass('cur');
                $('#usebalance').val(1);
                if (rs.moremoney > 0) {
                    oppo('余额不足，请再选择一个支付方式', 1);
                    $('.mpri .int').html(rs.prices3)
                    $('.mpri .dec').html(rs.prices4)
                } else {

                    $('.mpri .int').html('0')
                    $('.mpri .dec').html('00')
                }
            }
        })
    }

    $('.mask').on('click', function () {
        $(this).fadeOut()
    })
    //判断是否为微信
    function is_weixin() {
        var ua = navigator.userAgent.toLowerCase();
        if (ua.match(/micromessenger/i) == "micromessenger") {
            return true;
        } else {
            return false;
        }
    }

    if (is_weixin()) {
        //$('.mask').fadeIn()
    }
    //倒计时
    var time = time.toString().replace(/-/g, "/");
    console.log(time)
    countDown(time, '.deadline')
    function countDown(time, id) {
        var btn = true;
        var count = 0;
        var timer = null;
        // var day_elem = $(id).find('.day');
        // var hour_elem = $(id).find('.hour');
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
                    window.location.replace("/Html/Order/MyOrder.html?orderType=0")
                    clearInterval(index);
                    return; //停止下面代码执行
                }
            }, 1000)
        }
    }

    /*function lasttime() {
     var min = $('.deadline .min').attr('data-min');
     var sec = $('.deadline .sec').attr('data-sec');
     var time = setInterval(function () {
     if (sec == 0) {
     if (min == 0) {
     min = '00';
     sec = '00';
     window.location.replace("/Html/Order/MyOrder.html?orderType=0")
     clearInterval(time)
     } else {
     sec = 59;
     min--
     if (min < 10) {
     min = '0' + min
     }
     }
     } else {
     sec--
     if (sec < 10) {
     sec = '0' + sec
     }
     }
     $('.deadline .min').html(min);
     $('.deadline .sec').html(sec)
     }, 1000)

     }
     */
    //调取余额

    function cash() {
        $.ajax({
            url: "/Api/v1/Wallet/Cash",
            type: "get"
        }).done(function (rs) {
            if (rs.returnCode == '200') {
                view(rs.data)
            } else {
                if (rs.returnCode == '401') {
                    Backlog();
                } else {
                    oppo(rs.msg, 1)
                }
            }
        })
    }

    var je;

    function view(rs) {
        //可提现金额
        if (rs.Available.toString().indexOf('.') > -1) {
            if (rs.Available.toString().split('.')[1].length == 1) {
                rs.prices2 = rs.Available.toString().split('.')[1] + '0'
            } else {
                rs.prices2 = rs.Available.toString().split('.')[1];
            }
            rs.prices1 = rs.Available.toString().split('.')[0];

        } else {
            rs.prices1 = rs.Available;
            rs.prices2 = '00';
        }
        //剩余支付金额
        rs.moremoney = parseFloat(money - rs.Available).toFixed(2)
        if (rs.moremoney.toString().indexOf('.') > -1) {
            if (rs.moremoney.toString().split('.')[1].length == 1) {
                rs.prices4 = rs.moremoney.toString().split('.')[1] + '0'
            } else {
                rs.prices4 = rs.moremoney.toString().split('.')[1];
            }
            rs.prices3 = rs.moremoney.toString().split('.')[0];

        } else {
            rs.prices3 = rs.moremoney;
            rs.prices4 = '00';
        }
        je = new Vue({
            el: "#pay_mon",
            data: rs,
            ready: function () {
                $.RMLOAD();
                wxp();
                yep(rs);
                choosePay()
            }
        })
    }

    function choosePay() {
        $("#subimitButton").on("click", function () {
            var submitForm = $("#formid");
            if (is_weixin()) {
                //如果是选择的支付宝，显示遮罩
                if ($('.alipay').hasClass('cur')) {
                    $('.mask').fadeIn();
                    return false;
                }
                return true;
            } else {
                //判断支付类型
                if ($('.alipay').hasClass('cur')) {
                    $("#paymentCode").val("alipay");
                } else {
                    $("#paymentCode").val("weixin");
                }
                submitForm.attr("action", "/Payment/H5/Pay");
                return true;
            }

            return false;
        });
    }
})