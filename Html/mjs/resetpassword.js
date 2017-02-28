$(function () {
    $.ADDLOAD()
    new Vue({
        el: '#main',
        data: {
            info: [],
            yanz: []
        },
        ready: function () {
            var _this = this;
            _this.$nextTick(function () {
                _this.submit();
                _this.yzclick();
                $.RMLOAD();
            })
        },
        methods: {
            infoajax: function () {
                var _this = this;
                var data = {
                    PhoneNumber: $.get_user('PhoneNumber'),
                    Password: $('#ntpw').val(),
                    SmsVerifyCode: $('#yzm').val()
            }
                $.ajax({
                    url: '/Api/v1/Member/ResetPassword',
                    type: 'PATCH',
                    data: data
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data;
                        // localStorage.removeItem('qy_loginToken');
                        $.oppo('修改成功',1)
                        window.location.href = "/Html/login.html"
                    }
                })
            },
            yzclick: function () {
                var _this = this;
                $('.yanzhengcode').on('click', function () {
                    $.oppo('验证码已发送')
                    CountDown($('.yanzhengcode'));
                    _this.yzajax();
                })
            },
            yzajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Member/SendCode',
                    type: 'post',
                    data: {
                        PhoneNumber: $.get_user('PhoneNumber'),
                        RequestType: '1'
                    }
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.yanz = rs.data
                    }
                })
            },
            submit: function () {
                var _this = this;
                $('.submit').on('click', function () {
                    if ($('#yzm').val() == "") {
                        $.oppo('请输入验证码', 1)
                        return false;
                    }
                    if ($('#npw').val() == "") {
                        $.oppo('请输入新密码', 1)
                        return false;
                    }
                    if ($('#ntpw').val() == "") {
                        $.oppo('请输入确认密码', 1)
                        return false;
                    }
                    if ($('#npw').val() != $('#ntpw').val()) {
                        $.oppo('两次密码不一致', 1)
                        return false;
                    }
                    _this.infoajax();
                })
            }
        }
    })
})