$(document).ready(function () {


    $(".reg_title_1 h3").on("click", function () {
        var index = $(".reg_title_1 h3").index($(this));
        $(".reg_title_1 h3").addClass("deng_type");
        $(this).removeClass("deng_type");
        if (index === 0) {
            $(".denglu_first").show();
            $(".denglu_second").hide();
        } else {
            $(".denglu_first").hide();
            $(".denglu_second").show();
        }
    });

    //普通登录模块
    $(".submit_1").on("click", function () {
        if (!noNull(".userIphone1")) {
            my_alert("用户名不能为空");
            return false;
        }
        if (!mima(".password")) {
            my_alert("请输入6位数字或字母以上组合密码");
            return false;
        }
        $.ajax({
            url: '/Member/LogOn',
            cache: false,
            type: 'post',
            data: $("#loginform").serialize()
        }).done(function (data) {

            if (data.Success) {
                //登录成功后跳转到编辑个人信息页面
                location.href = url_Personal;
            }
            else {
                my_alert(data.ErrorMessage);
            }

        });
    });


    //动态登录
    $(".submit_2").on("click", function () {
     
        if (!noNull(".yan")) {
            my_alert("动态密码不能为空");
            return false;
        }
        $.ajax({
            url: url_LoginWithSms,
            cache: false,
            type: 'post',
            data: $("#loginform").serialize()
        }).done(function (data) {
            if (data.Success) {
                //登录成功后跳转到编辑个人信息页面
                editCookie("secondsremained4", 0, 0);
               
                location.href = url_Personal;
            }
            else {
                my_alert(data.ErrorMessage);
            }
        });
    });

    $(".code-btn").on("click", function () {
        chufa(); //倒计时

    });

    //击验证触发函数
    function chufa() {
        sendCode($(".code-btn"));
    }

    //刷新不变
    v = getCookieValue("secondsremained4"); //获取cookie值
    if (v > 0) {
        settime($(".code-btn")); //开始倒计时
    }

    //  验证码计时
    function addCookie(name, value, expiresHours) {
        var cookieString = name + "=" + escape(value);
        //判断是否设置过期时间,0代表关闭浏览器时失效
        if (expiresHours > 0) {
            var date = new Date();
            date.setTime(date.getTime() + expiresHours * 1000);
            cookieString = cookieString + ";expires=" + date.toUTCString();
        }
        document.cookie = cookieString;
    }

    //修改cookie的值
    function editCookie(name, value, expiresHours) {
        var cookieString = name + "=" + escape(value);
        if (expiresHours > 0) {
            var date = new Date();
            date.setTime(date.getTime() + expiresHours * 1000); //单位是毫秒
            cookieString = cookieString + ";expires=" + date.toGMTString();
        }
        document.cookie = cookieString;
    }

    //根据名字获取cookie的值
    function getCookieValue(name) {
        var strCookie = document.cookie;
        var arrCookie = strCookie.split("; ");
        var value = "";
        for (var i = 0; i < arrCookie.length; i++) {
            var arr = arrCookie[i].split("=");
            if (arr[0] == name) {
                value = unescape(arr[1]);
            }
        }
        return value;
    }

    //发送验证码
    function sendCode(obj) {

        //doPostBack(a_url);//发送请求
        addCookie('secondsremained4', 60, 60); //添加cookie记录,有效时间60s
        settime(obj); //开始倒计时

    }
    //开始倒计时
    function settime(obj) {
        countdown = getCookieValue("secondsremained4");
        if (countdown == 0) {
            obj.removeAttr("disabled");
            obj.val("获取验证码");
            return;
        } else {
            obj.attr("disabled", true);
            obj.val("重新发送(" + countdown + ")");
            countdown--;
            editCookie("secondsremained4", countdown, countdown + 1);
        }
        setTimeout(function () {
            settime(obj);
        }, 1000); //每1000毫秒执行一次
    }



    //获得短信验证码
    $("#sendbtn").on("click", function () {
        SendCode1(this);
    });

    function SendCode1(e) {
        var _this = $(e.target);
        if (!_this.hasClass("disabled")) {
            var ophone = $("#loginform input[name='DPhoneNumber']");
            var dPhone = $("#DPhoneNumber").val();
            if (!iphone(".userIphone2")) {
                my_alert("请输入正确的手机号码");
                ophone.focus();
                return false;
            } else {
               
                    //if (isempty(verifyKey.val(), verifyKey) && isempty(verifyCode.val(), verifyCode)) {
                    //请求发送 短信验证码
                    $.ajax({
                        type: 'POST',
                        url: '/Api/v1/Member/SendCode',
                        data: { PhoneNumber: dPhone, RequestType: 3 }
                    }).done(function (data) {
                        if (data.returnCode === "200") {
                            //发送短信成功后执行
                            //数组转成字符串
                            //var sms = data.data.smsvalue.join("");
                            //$("#smscode").val(sms);
                            my_alert("成功发送");
                        }else {
                            my_alert("未成功发送，请稍后再试");
                        }
                    });
                }
            }
        
    };

  
});
