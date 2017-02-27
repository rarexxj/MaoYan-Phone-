$(document).ready(function() {

    //图形验证码
    SetCaptcha();
    $("#sendimg").on("click", function() {
        SetCaptcha();
    });

    //导航点击
    $('.index-headnavmainul .index-navli').hover(function() {
        $(this).addClass('active');
        $(this).siblings().removeClass('active');
        $(this).children('.index-navtwo').stop().slideToggle();
    });

    //注册模块
    $('#js_submit').click(function() {
        if (!iphone(".userIphone")) {
            my_alert("手机号格式错误");
            return false;
        }
        if (!mima(".password")) {
            my_alert("请输入6位数字或字母以上组合密码");
            return false;
        }
        if (!againPassword1(".password", ".again_password")) {
            return false;
        }
        if (!yanzheng_num(".pic_yan")) {
            my_alert("请输入图形验证码");
            return false;
        }
        if (!yanzheng_num(".yan")) {
            my_alert("请输入短信验证码");
            return false;
        }

        if (!$(".checked")[0].checked) {
            my_alert("请点击同意用户服务使用协议");
            return false;
        }

        $.ajax({
            type: 'POST',
            url: $("#RegisterForm").attr("action"),
            cache: false,
            data: $("#RegisterForm").serialize()
        }).done(function(data) {
            if (data.Success) {
                var card = data.Data.MemberCard;
                $(".color_red").html(card);
                $(".add_address").show(); //表示display:block; 

            } else {
                my_alert(data.ErrorMessage);

            }
        });
    });

   

    $("#sendbtn").on("click", function() {
        SendCodes(this);
    });

    $(".code-btn").on("click", function() {
        if (!iphone(".userIphone")) {

            my_alert("手机号码格式错误");
            return false;
        }
        chufa(); //倒计时

    });
});

//倒计时点击验证触发函数
function chufa() {
    sendCode($(".code-btn"));
}

//刷新不变
v = getCookieValue("secondsremained5"); //获取cookie值
if (v > 0) {
    settime($(".code-btn")); //开始倒计时
}

//验证码计时
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
    addCookie('secondsremained5', 60, 60); //添加cookie记录,有效时间60s
    settime(obj); //开始倒计时

}


//发送短信验证码
function SendCodes(e) {
    if ($("#imgcode".val() != $("#sendimg").val())) {
        my_alert("图形验证码输入不正确！");
        return false;

    }
    var _this = $(e.target);
    if (!_this.hasClass("disabled")) {
        var ophone = $("#RegisterForm input[name='PhoneNumber']");
        if (!iphone(".userIphone")) {
            my_alert("请输入正确的手机号码");
            ophone.focus();
            return false;
        } else {
            var verifyKey = $("input[name='ImageVerifyKey']");
            var verifyCode = $("input[name='ImageVerifyCode']");
            if (verifyKey !== "" && verifyCode !== "") {
                //if (isempty(verifyKey.val(), verifyKey) && isempty(verifyCode.val(), verifyCode)) {
                //请求发送 短信验证码
                $.ajax({
                    type: 'POST',
                    url: '/Api/v1/Member/SendCode',
                    data: { PhoneNumber: ophone.val(), RequestType: 0, ImageVerifyKey: verifyKey.val(), ImageVerifyCode: verifyCode.val() }
                }).done(function (data) {
                    if (data.returnCode == 200) {
                        //发送短信成功后执行
                        //time($(e));
                    }
                    else if (data.returnCode == "0000") {
                        my_alert("图形验证码不能为空");
                    } else if (data.returnCode == "0002") {
                        my_alert(data.msg);
                    }
                    else {
                        my_alert("未成功发送，请稍后再试");
                    }
                });
            }
        }
    }
};
//开始倒计时
function settime(obj) {
    countdown = getCookieValue("secondsremained5");
    if (countdown == 0) {
        obj.removeAttr("disabled");

        obj.val("获取验证码");
        return;
    } else {
        obj.attr("disabled", true);
        obj.val("重新发送(" + countdown + ")");
        countdown--;
        editCookie("secondsremained5", countdown, countdown + 1);
    }
    setTimeout(function() {
        settime(obj)
    }, 1000); //每1000毫秒执行一次
}
function SetCaptcha() {
    $.ajax({
        type: 'GET',
        url: '/Api/v1/Captcha'
    }).done(function (data) {
        if (data.returnCode == 200) {
            $("input[name='ImageVerifyKey']").val(data.data.Key);
            $("#sendimg").attr("src", data.data.Image);
        }
    });
}