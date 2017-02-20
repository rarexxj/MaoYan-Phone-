$(function () {
    $.ADDLOAD()
    new Vue({
        el: '#search',
        data: {
            info: []
        },
        ready: function () {
            var _this = this;
            _this.infoajax();

            _this.$nextTick(function () {
                _this.leixclick();
            })

        },
        methods: {
            infoajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Mall/GoodsCategory',
                    type: 'get',
                    data: {}
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data;
                        $.RMLOAD();
                    }
                })
            },
            leixclick: function () {
                $('.leix').on('click', function () {
                    $(this).addClass('active');
                    $(this).siblings().removeClass('active')
                })
            }
        }
    })


})