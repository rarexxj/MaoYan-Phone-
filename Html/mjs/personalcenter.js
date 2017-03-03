$(function () {
    $.ADDLOAD();
    $.checkuser();
    new Vue({
        el: '#percen',
        data: {
            info: [],
            state: []
        },
        ready: function () {
            var _this = this;
            // _this.xinx = $.get_user('');
            _this.infoajax();
            _this.prostate();
            _this.$nextTick(function () {
                _this.guanzhu();

            })
        },
        methods: {
            infoajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Member/GetMemberInfo',
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data
                    }
                })
            },
            prostate: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Member/CenterInfo',
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.state = rs.data;
                        $.RMLOAD();
                    }
                })
            },
            guanzhu: function () {
                $('.weixin .att').on('click', function () {
                    $('.mask').stop().fadeIn();
                })
                $('.mask').on('click', function () {
                    $(this).stop().fadeOut();
                })
            }
        }
    })
})