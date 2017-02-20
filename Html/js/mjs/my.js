$(function () {
    $.ADDLOAD()
    $.checkuser();
    new Vue({
        el: '#my',
        data: {
            info: {},
            xinx: {}
        },
        ready: function () {
            var _this = this;
            _this.xinx = $.get_user('');
            _this.infoajax();
            _this.$nextTick(function () {
                _this.xxtx();
                $.RMLOAD()
            })
        },
        methods: {
            infoajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Member/CenterInfo',
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode = '200') {
                        _this.info = rs.data;
                    }
                })
            },
            xxtx: function () {
                if (localStorage['qy_head']) {
                    if ($.get_user('Id') == localStorage['qy_head'].toString().split('|')[0]) {
                        $('.touximg').attr('src', localStorage['qy_head'].toString().split('|')[1]);
                    }
                };
                if($.get_user('NickName')){
                    $('.name').html(decodeURIComponent(decodeURIComponent($.get_user('NickName'))));
                }else{
                    $('.name').html(' ')
                }

            }
        }
    })
})