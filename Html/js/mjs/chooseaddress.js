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
                $.RMLOAD()
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
                    }
                })
            },
            link: function () {
                $('#address').on('click', '.addlist a', function () {
                    var addid = $(this).parents('.addlist').attr('data-addid');
                    if (id) {
                        window.location.href = '/Html/settlement.html?id=' + id + '&addid=' + addid + "&type=" + type + '&mode=' + mode
                    } else {
                        window.location.href = '/Html/settlement.html?gid=' + gid + '&addid=' + addid + "&type=" + type + '&mode=' + mode
                    }
                });
                $('.submit').on('click', function () {
                    if (id) {
                        window.location.href = '/Html/chooseaddaddress.html?id=' + id + "&type=" + type + '&mode=' + mode
                    } else {
                        window.location.href = '/Html/chooseaddaddress.html?gid=' + gid + "&type=" + type + '&mode=' + mode
                    }
                })
            }
        }
    })


    function view(rs) {
        //得到返回id
        rs.backid = id
        new Vue({
            el: '#address',
            data: rs,
            ready: function () {
                $.RMLOAD()

            }
        })
    }


})