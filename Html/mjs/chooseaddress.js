$(function () {
    $.checkuser();
    var id = $.getUrlParam('id');
    var gid = $.getUrlParam('gid');
    var type = $.getUrlParam('type');
    var mode = $.getUrlParam('mode');
    $.ADDLOAD();

    new Vue({
        el: '#address',
        data: {
            info: {}
        },
        ready: function () {
            var _this = this;
            _this.infoajax();
            _this.$nextTick(function () {
                _this.link();

            })
        },
        methods: {
            infoajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Member/Address',
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data;
                        $.RMLOAD()
                    }
                })
            },
            link: function () {
                $('#address').on('click', '.addlist a', function () {
                    var addid = $(this).parents('.addlist').attr('data-addid');
                    if (id) {
                        window.location.href = '/Html/html/shopcar/settlement.html?id=' + id + '&addid=' + addid
                    } else {
                        window.location.href = '/Html/html/shopcar/settlement.html?gid=' + gid + '&addid=' + addid
                    }
                });
                $('.submit').on('click', function () {
                    if (id) {
                        window.location.href = '/Html/html/shopcar/chooseaddaddress.html?id=' + id
                    } else {
                        window.location.href = '/Html/html/shopcar/chooseaddaddress.html?gid=' + gid
                    }
                })
            }
        }
    })
})