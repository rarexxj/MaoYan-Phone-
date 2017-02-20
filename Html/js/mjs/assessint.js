$(function () {
    $.ADDLOAD()
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
        window.location.href = '/Html/innerlogin.html'
    }
    new Vue({
        el: '#int',
        data: {
            info: []
        },
        ready: function () {
            var _this = this;
            _this.intajax();
            _this.$nextTick(function () {
            })
        },
        methods: {
            intajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Page/02',
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data;
                        $.RMLOAD();
                    }
                })
            }
        }
    })
})