$(document).ready(function () {

    //导航点击
    $('.index-headnavmainul .index-navli').hover(function() {
        $(this).addClass('active');
        $(this).siblings().removeClass('active');
        $(this).children('.index-navtwo').stop().slideToggle();
    });
    //图形验证码
    SetCaptcha();
    $("#sendimg").on("click", function () {
        SetCaptcha();
    });
    function SetCaptcha() {
        $.ajax({
            type: 'GET',
            url: '/Api/v1/Captcha'
        }).done(function (data) {
            if (data.returnCode ==200) {
                $("#ImageVerifyKey").val(data.data.Key);
                $("#sendimg").attr("src", data.data.Image);
            }
        });
    }
    //根据历史记录切换模块
    function locationHashChanged() {

      
        if (location.hash == '#f3') {
            //		3.修改密码
            $(".forget_step").html("");
            $(".forget_step").load("../../module/forget/forget_three.html", function() {

                forget_three();
            });
        }
        if (location.hash == '#f4') {
            //		3.修改密码
            $(".forget_step").html("");
            $(".forget_step").load("../../module/forget/forget_four.html", function() {

            });
        }
    }

    //前后记录
    window.onhashchange = locationHashChanged;
    //刷新记录
    locationHashChanged();


    $(".forget_first").on("click", function () {
        //vailPhone();
        imageYanZhe();
        var phone = $("#PhoneNumberOne").val();
        $("#PhoneNumber1").val(phone);
        $("#PhoneNumber").val(phone);
        
      
    });

    details_tishi(".userIphone", ".tishi");
    details_tishi(".pic_yan", ".tishi");
    //图形验证码
    function imageYanZhe() {

        var verifyKey = $("#ImageVerifyKey").val();
        var verifyCode = $("#ImageVerifyCode").val();

        $.ajax({
            type: 'POST',
            url: url_imageVerify,
            data: { imageVerifyCode: verifyKey, imageVerifyKey: verifyCode }
        }).done(function (data) {
            if (data) {
                $(".passwordOne").hide(); //表示display:none; 
                $(".passwordTwo").show(); //表示display:block; 

            }
            else {
                flag = false;
            }
        });
    }
    //手机验证
    function vailPhone() {
        var phone = $("#PhoneNumberOne").val();
        var flag = false;
       
        var myreg = /^(((13[0-9]{1})|(14[0-9]{1})|(17[0]{1})|(15[0-3]{1})|(15[5-9]{1})|(18[0-9]{1}))+\d{8})$/;
        if (phone == '') {
            my_alert("手机号码不能为空");
            return false;s
        } else if (phone.length != 11) {
            my_alert("请输入有效的手机号码！");
            return false;
        } else if (!myreg.test(phone)) {
            my_alert("请输入有效的手机号码");
            return false;
         
        } else if (!checkPhoneIsExist()) {
            my_alert("该手机号码已经被绑定");
            return false;

        } else {
            flag = true;
        }
       
        return flag;
    }
    //验证该手机是否存在
    function checkPhoneIsExist() {
        var phone = $("#PhoneNumberOne").val();
        var flag = true;
        $.ajax({
            type: 'GET',
            url: url_phoneIsExist,
            data: { phone: phone }
        }).done(function (data) {
            if (data) {
             
            }
           
            else {
                flag=false;
            }
        });
        return flag;
    }


    //发送短信验证码
    $("#sendbtn").on("click", function () {
        SendCode(this);
    });

    function SendCode(e) {
        var _this = $(e.target);
        if (!_this.hasClass("disabled")) {
            var ophone = $("#PhoneNumber1").val();
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
                        data: { PhoneNumber: ophone, RequestType: 1, ImageVerifyKey: verifyKey.val(), ImageVerifyCode: verifyCode.val() }
                    }).done(function (data) {
                        if (data.returnCode == 200) {
                            //发送短信成功后执行
                            var sms = data.data.smsvalue.join("");
                            $("#smscode").val(sms);
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

    //第二步
    $(".forget_two").on("click", function() {
        //vailPhone();
        //验证短信验证码是否正确
        if ($("#SmsVerifyCode").val() !== $("#smscode").val()) {
            my_alert("输入的验证码不正确！");
            return false;
        }
        $(".passwordOne").hide(); //表示display:none; 
        $(".passwordTwo").hide(); //表示display:none; 
        $(".passwordThree").show(); //表示display:block;
     
    });


        details_tishi(".pic_yan", ".tishi");

    //第三部
        $(".forget_three").on("click", function () {
            ////判断两次密码是否一致
            //if ($("#secondPassword").val() != $("#Password").val()) {
            //    my_alert("两次密码输入不一致！");
            //}
        vailPhone();
        if (!mima(".password")) {

            $(".tishi").html("请输入6位数字或字母以上组合密码").show();
            return false;
        }
        if (!againPassword(".password", ".again_password")) {

            return false;
        }
        $.ajax({
            type: 'POST',
            url: $("#ForgetForm").attr("action"),
            cache: false,
            data: $("#ForgetForm").serialize(),
            success: function(data) {
                if (data.Success) {
                    //成功后跳到登陆
                    location.href = url_longin;
                } else {
                    my_alert(data.ErrorMessage);

                }
            }
        });
    });


        details_tishi(".password", ".tishi");
        details_tishi(".again_password", ".tishi");

 
  

    //	 实时,消去提示
    function details_tishi(element, elementbox) {
        $(element)[0].onpropertychange = function () {
            if (yanzheng(element)) {

                $(elementbox).hide()
            }

        }
        $(element)[0].oninput = function () {
            if (yanzheng(element)) {

                $(elementbox).hide()
            }
        }
    }
})