$(function () {
    $.ADDLOAD();
    new Vue({
        el: '#main',
        data: {
            info: []
        },
        ready: function () {
            var _this = this;
            _this.mainajax();
            // _this.nextTick(function () {
            //
            // })
        },
        methods: {
            mainajax: function () {
                var _this=this;
                $.ajax({
                    url: URL+'/Api/v1/Mall/GoodsCategory',
                    type: 'get',
                    data: {}
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data;
                        $.RMLOAD()
                    }
                })
            }
        }
    })


})