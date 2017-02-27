$(function () {
    // $.ADDLOAD();
    var yaoqingma = $.getUrlParam('code');
    var openid = $.getUrlParam('openId');
    if (yaoqingma) {
        $('#inv').val(yaoqingma)
    }
    //获取验证码
    getyanz();
    function getyanz() {
        $('.get').on('click', function () {
            if (!$('#ph').val()) {
                $.oppo('请输入手机号码', 1)
                return false;
            }
            var data = {
                PhoneNumber: $('#ph').val(),
                RequestType: 0
            }
            ajaxyanz(data)
        })
    }


    function ajaxyanz(data) {
        $.ajax({
            url: '/Api/v1/Member/SendCodeWeixin',
            type: 'POST',
            data: data
        }).done(function (rs) {
            if (rs.returnCode == '200') {
                $.oppo('验证码已经发送', 1)
                CountDown($('.get'));
            }
        })
    }

    //注册
    //手机号验证匹配
    var reg = /^1[3|4|5|7|8]\d{9}$/
    zhuc();
    function zhuc() {
        $('.btn').on('click', function () {

            if ($('#ph').val() == "") {
                $.oppo("请输入手机号", 1);
                return false;
            } else if (!reg.test($('#ph').val())) {
                $.oppo("手机号格式有误", 1);
                return false;
            } else if ($('#yzm').val() == "") {
                $.oppo("请输入验证码", 1);
                return false;
            } else if ($('#pw').val() == "") {
                $.oppo("请输入密码", 1);
                return false;
            } else if ($('#pw').val() != $('#pw2').val()) {
                $.oppo('您输入的密码不同，请重新输入', 1)
            } else {
                $(this).addClass('gray');
                var data2 = {
                    PhoneNumber: $('#ph').val(),
                    Password: $('#pw').val(),
                    SmsVerifyCode: $('#yzm').val(),
                    InvitationCode: $('#inv').val(),
                    ChannelId: '',
                    openId: openid
                }
                ajaxzhc(data2)
            }
        })
    }

    function ajaxzhc(data2) {
        $.ajax({
            url: '/Api/v1/Member',
            type: 'post',
            data: data2
        }).done(function (rs) {
            if (rs.returnCode == '200') {
                $.oppo('注册成功', 1, function () {
                    window.location.href = "/Html/my.html";
                })
            }
        })
    }

    //微信手机绑定
    $('.gowxb').on('click', function () {
        window.location.href = "/Html/html/personalcenter/signup.html?openId=" + openid
    })
})