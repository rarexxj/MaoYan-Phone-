$(function () {
    $.ADDLOAD();
    $.checkuser();
    var key = $.getUrlParam('key');
    new Vue({
        el: '#singlepage',
        data: {
            info: []
        },
        ready: function () {
            var _this = this;
            _this.infoajax();
            $.RMLOAD();
        },
        methods: {
            infoajax: function () {
                var _this=this;
                $.ajax({
                    url: "/Api/v1/Page/" + key,
                    type: "get",
                    data: {
                        key: key
                    }
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data;
                    }
                })
            }
        }
    })
})