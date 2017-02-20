$(function () {
    $.ADDLOAD()
    var qy_user1 = JSON.parse(localStorage.qy_user)
    $.checkuser();
    new Vue({
        el: '#main',
        data: {
            info: []
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
                    PhoneNumber: $('.ophone').val(),
                    NewPhoneNumber: $('.newphone').val()
                }
                $.ajax({
                    url: '/Api/v1/Member/' + $.get_user('Id') + '/PhoneNumber',
                    type: 'PATCH',
                    data: data
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        $.oppo('修改成功', 1, function () {
                            var qy_PhoneNumber = $('.newphone').val();
                            qy_user1.PhoneNumber = qy_PhoneNumber;
                            qy_user = JSON.stringify(qy_user1);
                            localStorage.setItem('qy_user', qy_user);
                            window.location.replace("/Html/login.html");
                        });
                        _this.info = rs.data
                    }
                })
            },
            submit: function () {
                var _this = this;
                //手机号验证匹配
                var reg = /^1[3|4|5|7|8]\d{9}$/
                $('.submit').on('click', function () {
                    if (!reg.test($('.text').val())) {
                        $.oppo("手机号格式有误", 1);
                        return false;
                    } else {
                        var phonenum=$.get_user('PhoneNumber');
                        $('.success .p i').html(phonenum);
                        $('.success').show();
                    }
                });
                $('.myinfo').on('click', '.nobang', function () {
                    $('.success').hide();
                })

                $('.myinfo').on('click', '.bangd', function () {
                    _this.infoajax();
                })
            }
        }
    })
})