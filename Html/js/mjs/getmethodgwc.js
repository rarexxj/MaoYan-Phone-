$(function () {
    $.ADDLOAD()
    var id = $.getUrlParam('id');
    var type = $.getUrlParam('type');
    console.log(type);
    new Vue({
        el: '#main',
        data: {
            info: [],
            tipinfo:[]
        },
        ready: function () {
            var _this = this;
            _this.$nextTick(function () {
                _this.tipajax();
                _this.yemian();
                _this.click();
                $.RMLOAD()
            })
        },
        methods: {
            click: function () {
                $('.zt').on('click', function () {
                    window.location.href = '/Html/shopchioce.html?id=' + id + '&type=' + type+'&mode='+0
                });
                $('.yj').on('click', function () {
                    window.location.href = '/Html/settlement.html?id=' + id + '&type=' + type+'&mode='+1
                });
            },
            yemian: function () {
                if (type == 1) {
                    $('.yj').hide();
                }
            },
            tipajax:function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Page/05',
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.tipinfo = rs.data;
                    }
                })
            }
        }
    })
})