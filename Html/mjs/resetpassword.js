$(function () {
    $.ADDLOAD();
    $.checkuser();
    var id=$.get_user('Id');
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
                $.RMLOAD();
            })
        },
        methods: {
            infoajax: function () {
                var _this = this;
                var data = {
                    Password: $('#opw').val(),
                    NewPassword: $('#spw').val()
                };
                $.ajax({
                    url: '/Api/v1/Member/'+id+'/Password',
                    type: 'PATCH',
                    data: data
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data;
                        $.oppo('修改成功', 1);
                        setTimeout(function () {
                            window.location.href = "/Html/html/personalcenter/login.html"
                        },1000)
                    }
                })
            },
            submit: function () {
                var _this = this;
                $('.submit').on('click', function () {
                    if ($('#opw').val() == "") {
                        $.oppo('请输入旧密码', 1)
                        return false;
                    }
                    if ($('#npw').val() == "") {
                        $.oppo('请输入新密码', 1)
                        return false;
                    }
                    if ($('#spw').val() == "") {
                        $.oppo('请输入确认密码', 1)
                        return false;
                    }
                    if ($('#npw').val() != $('#spw').val()) {
                        $.oppo('两次密码不一致', 1)
                        return false;
                    }
                    _this.infoajax();
                })
            }
        }
    })
})