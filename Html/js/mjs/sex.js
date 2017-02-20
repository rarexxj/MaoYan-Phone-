$(function () {
    // $.ADDLOAD()
    var qy_user1 = JSON.parse(localStorage.qy_user)
    $.checkuser();
    var num = $.get_user('Sex');
    if (num == 1) {
        $('.sexman').addClass('active')
    }
    if (num == 2) {
        $('.sexwoman').addClass('active')
    }
    new Vue({
        el: '#main',
        data: {
            info: []
        },
        ready: function () {
            var _this = this;
            _this.$nextTick(function () {
                _this.chioce();
                _this.submit();
                $.RMLOAD();
            })
        },
        methods: {
            infoajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Member/' + $.get_user('Id'),
                    type: 'put',
                    data: {
                        NickName: '',
                        Birthday: '',
                        Sex: num,
                        IDCard: ''
                    }
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        $.oppo('修改成功', 1, function () {
                            var qy_sex = rs.data.Sex;
                            qy_user1.Sex = qy_sex;
                            qy_user = JSON.stringify(qy_user1);
                            localStorage.setItem('qy_user', qy_user);
                            window.location.replace("/Html/myinfo.html");
                        });
                        _this.info = rs.data
                    }
                }).always(function () {
                    $('.submit').removeClass('gray')
                })
            },
            submit: function () {
                var _this = this;
                $('.submit').on('click', function () {
                    if ($(this).hasClass('gray')) {
                        return false;
                    } else {
                        $(this).addClass('gray')
                        _this.infoajax();
                    }
                })
            },
            chioce: function () {
                $('.sex').on('click', function () {
                    $(this).addClass('active').siblings().removeClass('active');
                    if ($('.sexman').hasClass('active')) {
                        num = 1;
                    }
                    if ($('.sexwoman').hasClass('active')) {
                        num = 2;
                    }
                })
            }
        }
    })
})
