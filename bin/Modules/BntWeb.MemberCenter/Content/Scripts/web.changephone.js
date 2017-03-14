$(document).ready(function () {
    //导航点击
    $('.index-headnavmainul .index-navli').hover(function () {
        $(this).addClass('active');
        $(this).siblings().removeClass('active');
        $(this).children('.index-navtwo').stop().slideToggle();
    });

    //根据历史记录切换模块
    function locationHashChanged() {
        if (location.hash === '') {
            //1.修改绑定手机模块
            $(".phonetwo").hide(); //表示display:none; 
            iphone_one();
        }
        if (location.hash === '#p2') {
            //1.绑定新手机模块
            $(".per_nav_right2").hide(); //表示display:none; 
            $(".phonetwo").show(); //表示display:block; 
            iphone_two();
        }
    }
    //前后记录
    window.onhashchange = locationHashChanged;
    //刷新记录
    locationHashChanged();


    //倒计时
    function chufa(a,b) {
    //点击验证触发函数
        sendCode($(b));
    
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
        addCookie(a, 60, 60); //添加cookie记录,有效时间60s
        settime(obj); //开始倒计时

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

    //开始倒计时
    function settime(obj) {
        countdown = getCookieValue(a);
        if (countdown == 0) {
            obj.removeAttr("disabled");
            obj.val("获取验证码");
            return;
        } else {
            obj.attr("disabled", true);
            obj.val("重新发送(" + countdown + ")");
            countdown--;
            editCookie(a, countdown, countdown + 1);
        }
        setTimeout(function () {
            settime(obj)
        }, 1000); //每1000毫秒执行一次
    }


    }

    ////获得短信验证码
    //$("#sendbtn").on("click", function () {
    //    SendCode(this);
    //});

    //1.修改绑定手机模块
    function iphone_one() {

        //下一步提交
        $(".next_bu").on("click", function () {
            if (!yanzheng(".yanzheng")) {
                $(".yan_cuo").show().html("验证码不能为空");
                return false;
            }
            //验证短信验证码是否正确
            if ($("#SmsVerifyCode").val() !== $("#smscode").val()) {
                my_alert("输入的验证码不正确！");
                return false;
            }
            //旧手机隐藏
            $(".iphone_one").hide();
            //新手机显示
            $(".phonetwo").show();
        });
        //实时消去提示
        details_tishi(".yanzheng", ".yan_cuo");
        //获取验证码
        $("#sendbtnOne").on("click", function () {
            sendCodeOne(this); //获取短信验证码
          
        });

        //旧手机获得短信验证码
        function sendCodeOne(e) {
          
                //旧手机号码
                var ophone = $("#PhoneOne").val();
                //请求发送 短信验证码
                $.ajax({
                    type: 'POST',
                    url: '/Api/v1/Member/SendCode',
                    data: { PhoneNumber: ophone, RequestType: 2 }
                }).done(function (data) {
                    if (data.returnCode === "200") {
                        //发送短信成功后执行
                        //数组转成字符串
                        var sms = data.data.smsvalue.join("");
                        $("#smscode").val(sms);
                       
                        chufa("yanzheng1", ".code1"); //倒计时
                    } else {
                        my_alert("未成功发送，请稍后再试");
                    }
                });
            
        };
    }

    //新手机绑定，获取验证码新手机验证（点击事件）
    $("#sendbtnTwo").on("click", function () {
        //判断新手机号格式
        if (!iphone("#NewPhoneNumber")) {

            $(".yan_cuo").show().html("手机格式错误");
            return false;
        }
        var _that = $(this);
        sendCodeTwo(_that); //倒计时

    });

    //新手机获得短信验证码
    function sendCodeTwo(_that) {
        var _this = _that;
       
            //旧手机号码
            var newPhone = $("#NewPhoneNumber").val();
            //请求发送 短信验证码
            $.ajax({
                type: 'POST',
                url: '/Api/v1/Member/SendCode',
                data: { PhoneNumber: newPhone, RequestType: 4}
            }).done(function (data) {
                if (data.returnCode === "200") {
                    //发送短信成功后执行
                    chufa("yangzheng2", ".code2"); //倒计时

                } else {
                    my_alert("未成功发送，请稍后再试");
                }
            });
        
    };

    //2.绑定新手机模块

    //下一步提交 第二步骤
    $("#finish").on("click", function () {

        if ($("#NewPhoneNumber").val() === "") {
            my_alert("手机号码不能为空！");
        }

     
        if (!yanzheng(".yanzheng1")) {

            $(".yan_cuo").show().html("验证码不能为空");
            return false;
        }
        $.ajax({
            url: $("#phoneform").attr("action"),
            cache: false,
            type: 'post',
            data: $("#phoneform").serialize()
        }).done(function (data) {
            if (data.Success) {
                location.href = url_personal;
            }

        });
    });


    details_tishi(".yanzheng1", ".yan_cuo");
    details_tishi(".iphone", ".yan_cuo");




    //实时,消去提示
    function details_tishi(element, elementbox) {
        $(element)[0].onpropertychange = function () {
            if (yanzheng(element)) {
                $(elementbox).hide();
            }
        }
        $(element)[0].oninput = function () {
            if (yanzheng(element)) {

                $(elementbox).hide();
            }
        }
    }
});