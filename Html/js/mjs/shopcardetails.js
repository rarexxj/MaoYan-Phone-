$(function () {
    $.checkuser();
    var id = $.getUrlParam('id');
    var ids = {}
    if (id) {
        if (id.indexOf('|') > 0) {
            ids.CartIds = id.split('|')
        } else {
            ids.CartIds = id
        }
    }
    console.log(ids)
    // $.ADDLOAD();

    new Vue({
        el: '#main',
        data: {
            info: {},
            data1:ids
        },
        ready: function () {
            var _this = this;
            _this.infoajax();
            _this.$nextTick(function () {
                // _this.link();
                // $.RMLOAD()
            })
        },
        methods: {
            infoajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Mall/OrderCalculation',
                    data:_this.data1,
                    type: 'post'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data;
                    }
                })
            }
        }
    })
})
