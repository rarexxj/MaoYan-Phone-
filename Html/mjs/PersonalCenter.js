$(function () {
    // $.ADDLOAD();
    $.checkuser();

    new Vue({
        el: 'percen',
        data: {
            info:[]
        },
        ready: function () {
            var _this = this;
            // _this.xinx = $.get_user('');
            _this.infoajax();
            _this.$nextTick(function () {

            })
        },
        methods: {
            infoajax: function () {
                $.ajax({
                    url: '/Api/v1/Member/CenterInfo',
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info=rs.data
                    }
                })
            }
        }
    })

    function view(rs) {
        if (!rs.Money || rs.Money == 'null') {
            rs.Money = 0
        }
        if (!rs.Integral || rs.Integral == 'null') {
            rs.Integral = 0
        }
        localStorage['qy_MemberType'] = rs.MemberType;
        new Vue({
            el: '#per-cen',
            data: rs,
            ready: function () {
                $.RMLOAD();
                js();
            }
        })
    }

    function js() {
        $('.weixin .att').on('click', function () {
            $('.mask').stop().fadeIn();
        })
        $('.mask').on('click', function () {
            $(this).stop().fadeOut();
        })
    }

    // function base64_encode(){
    //     var str=CryptoJS.enc.Utf8.parse($("#source").val());
    //     var base64=CryptoJS.enc.Base64.stringify(str);
    //     $("#result").val(base64);
    // }
    function base64_decode(str) {
        var words = CryptoJS.enc.Base64.parse(str);
        words = words.toString(CryptoJS.enc.Utf8);
        return words
    }

})