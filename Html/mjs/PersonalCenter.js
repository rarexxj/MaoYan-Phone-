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
    //
    // function view(rs) {
    //     if (!rs.Money || rs.Money == 'null') {
    //         rs.Money = 0
    //     }
    //     if (!rs.Integral || rs.Integral == 'null') {
    //         rs.Integral = 0
    //     }
    //     localStorage['qy_MemberType'] = rs.MemberType;
    //     new Vue({
    //         el: '#per-cen',
    //         data: rs,
    //         ready: function () {
    //             $.RMLOAD();
    //             js();
    //         }
    //     })
    // }
    //
    // // function base64_encode(){
    // //     var str=CryptoJS.enc.Utf8.parse($("#source").val());
    // //     var base64=CryptoJS.enc.Base64.stringify(str);
    // //     $("#result").val(base64);
    // // }
    // function base64_decode(str) {
    //     var words = CryptoJS.enc.Base64.parse(str);
    //     words = words.toString(CryptoJS.enc.Utf8);
    //     return words
    // }

})