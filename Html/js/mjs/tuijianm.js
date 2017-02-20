$(function () {
    // $.ADDLOAD()
    if ($.is_weixin()) {
        var user = $.cookie('userInfo1')
        if (user) {
            user = JSON.parse(window.base64decodes(user))
            $.put_user(user)
            localStorage.setItem('qy_loginTokenyy', user.PhoneNumber + ':' + user.DynamicToken);
            //新增
            $.removeCookie('userInfo1');
        }

    }
    window.TOKEN = localStorage.getItem('qy_loginTokenyy')
    if (window.TOKEN) {
        $.ajaxSetup({
            headers: {
                Authorization: 'Basic ' + window.base64encode(window.TOKEN)
            }
        })
    } else {
        window.location.href='/Html/innerlogin.html'
    }
    var code=$.get_user('InvitationCode');
    new Vue({
        el: '#main',
        data: {
            info: [],
            code:code
        },
        ready: function () {
            var _this = this;
            _this.qrcode();
            _this.$nextTick(function () {
                $.RMLOAD();
            })
        },
        methods: {
            qrcode: function () {
            // 设置参数方式
                var qrcode = new QRCode('qrcode', {
                    text: location.origin + "/Html/register.html?code=" + code,
                    colorDark : '#000000',
                    colorLight : '#ffffff',
                    correctLevel : QRCode.CorrectLevel.H
                });

            }
        }
    })
})