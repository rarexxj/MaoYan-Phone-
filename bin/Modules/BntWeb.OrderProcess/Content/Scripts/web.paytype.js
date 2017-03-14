$(function () {

    //导航点击
    $('.index-headnavmainul .index-navli').hover(function() {
        $(this).addClass('active');
        $(this).siblings().removeClass('active');
        $(this).children('.index-navtwo').stop().slideToggle();
    });

    //选择支付方式
    $('.paytydd').on('click', function() {
        $('.paytyquan').removeClass('active');
        $(this).find('.paytyquan').addClass('active');

        payInitData.PaymentCode = $(this).data("code");
    });


    function pay() {
        if (payInitData.PaymentCode == "") {
            my_alert('请选择支付方式');
            return false;
        }
        if (payInitData.OrderId == "") {
            my_alert('支付订单ID无效');
            return false;
        }
        if (payInitData.ReturnUrl == "") {
            my_alert('支付成功通知地址无效');
            return false;
        }

        var payUrl = "";
        if (payInitData.PaymentCode == "alipay") {
            payUrl =  "/Payment/Web" + "?PaymentCode=" + payInitData.PaymentCode + "&OrderId=" + payInitData.OrderId + "&UseBalance=0&ReturnUrl=" + payInitData.ReturnUrl;

            location.href = payUrl;
        } else if (payInitData.PaymentCode == "weixin") {
            payUrl =  "/Payment/Process/WeiXin/ScanPay?PaymentCode=" + payInitData.PaymentCode + "&OrderId=" + payInitData.OrderId + "&UseBalance=0&ReturnUrl=" + payInitData.ReturnUrl;

            location.href = payUrl;
        } else if (payInitData.PaymentCode == "chinabank") {

            my_alert('网银在线支付暂未开通，敬请期待');
            return false;
            //payUrl =  "/Payment/ChinaBank?PaymentCode=" + payInitData.PaymentCode + "&OrderId=" + payInitData.OrderId + "&UseBalance=0&ReturnUrl=" + payInitData.ReturnUrl;
            
        }
        else if (payInitData.PaymentCode == "payondelivery") {
            $.ajax({
                url: "/Payment/PayOnDelivery?orderId=" + payInitData.OrderId,
                type: "post",
                data: null,
                success: function (data) {
                    if (data.Success) {
                        location.href = payInitData.ReturnUrl;
                    } else {
                        alert(data.ErrorMessage);
                    }
                },
                error: function () { }
            });
        }
    }
    $("#btnPay").click(function () {
        pay();
    });



    var timer = null;
    CountDown("2016/12/28 15:50:10"); //时间调用
    function CountDown(time) {
        var endTime = new Date(time).getTime(); //time是后台上传的时间
        var nowTime = new Date().getTime();
        var allsecond = (endTime - nowTime) / 1000;
        var minute = Math.floor((allsecond / 60) % 60);
        var second = Math.floor(allsecond % 60);
        $('.min').html(minute < 10 ? "0" + minute : minute);
        $('.second').html(second < 10 ? "0" + second : second);

        timer = setInterval(function() {
            allsecond = allsecond - 1;
            if (allsecond > 0) {
                var minute = Math.floor((allsecond / 60) % 60);
                var second = Math.floor(allsecond % 60);
                $('.min').html(minute < 10 ? "0" + minute : minute);
                $('.second').html(second < 10 ? "0" + second : second);
            } else {
            }
        }, 1000);
    }



})