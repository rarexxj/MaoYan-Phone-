$(function () {
    $.ADDLOAD();
    $.checkuser();
    var sfId=$.get_user('Id')
    new Vue({
        el: '#main',
        data: {
            info: []
        },
        ready: function () {
            var _this = this;
            _this.nsubmit();
            _this.$nextTick(function () {
                $.RMLOAD();
            })
        },
        methods: {
            infoajax: function (data) {
                var _this = this;
                $.ajax({
                    url: URL+'/Api/v1/Member/' + sfId +'/Update',
                    type: 'put',
                    data: data
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        $.oppo('修改成功', 1, function () {
                            window.location.href("/Html/html/personalcenter/myinfo.html");
                        });
                    }
                }).always(function () {
                    $('.submit').removeClass('gray')
                })
            },
            nsubmit: function () {
                var _this = this;
                $('.submit').on('click', function () {
                    var data = {
                        NickName: $('.text').val(),
                        Birthday: '',
                        Sex: '',
                        TastesType:''
                    };
                    if ($(this).hasClass('gray')) {
                        return false;
                    } else {
                        $(this).addClass('gray')
                        _this.infoajax(data);
                    }
                })
            }
        }
    })
})

