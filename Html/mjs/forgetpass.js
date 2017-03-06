$(function () {
    //获取验证码
    getyanz();
    function getyanz() {
        $('.get').on('click', function () {
            if (!$('#ph').val()) {
                $.oppo('请输入手机号码', 1);
                return false;
            }
            var data = {
                PhoneNumber: $('#ph').val(),
                RequestType: 1
            }
            ajaxyanz(data)
        })
    }


    function ajaxyanz(data) {
        $.ajax({
            url: '/Api/v1/Member/SendCodeWeixin',
            type: 'post',
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
    var reg = /^1[3|4|5|7|8]\d{9}$/;
    next();
    function next() {
        $('.btn').on('click', function () {
            if ($('#ph').val() == '') {
                $.oppo("请输入手机号", 1);
                return false;
            } else if (!reg.test($('#ph').val())) {
                $.oppo("手机号格式有误", 1);
                return false;
            } else if ($('#yzm').val() == "") {
                $.oppo("请输入验证码", 1);
                return false;
            }else{
                $('.box2').show();
                $('.box1').hide();
                complete();
            }
        })
    }

    function complete() {
        $('.reg').on('click', '.btn2', function () {
            var data2 = {
                PhoneNumber: $('#ph').val(),
                Password: $('#pw').val(),
                SmsVerifyCode: $('#yzm').val()
            }
            ajaxforget(data2)
        })
    }

    function ajaxforget(data2) {
        $.ajax({
            url: '/Api/v1/Member/ResetPassword',
            type: 'patch',
            data: data2
        }).done(function (rs) {
            if (rs.returnCode == '200') {
                $('.box3').show();
                $('.box2').hide();
            }
        })
    }
})