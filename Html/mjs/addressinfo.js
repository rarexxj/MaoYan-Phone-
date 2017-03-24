$(function () {
    $.checkuser();
    var id = $.getUrlParam('id');
    $.ADDLOAD();

    new Vue({
        el: '#addaddress',
        data: {
            info: []
        },
        ready: function () {
            var _this = this;
            _this.infoajax();
            _this.$nextTick(function () {
                _this.moren();
                _this.shanchu();
                $.RMLOAD()
            })
        },
        methods: {
            infoajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Member/Address/' + id,
                    dataType:'json',
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data;
                    }
                })
            },
            moren: function () {
                //设为默认地址
                var _this = this;
                $('.submit').on('click', function () {
                    $(this).addClass('gray');
                    _this.morenajax();
                })
            },
            morenajax: function () {
                $.ajax({
                    url: '/Api/v1/Member/Address/' + id + '/Default',
                    dataType:'json',
                    type: 'patch',
                    data: {
                        addressId: id
                    }
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        $.oppo('成功设为默认地址', 1, function () {
                            window.location.href("/Html/html/personalcenter/myinfo.html")
                        })
                    }
                })
            },
            shanchu: function () {
                //删除收货地址
                var _this = this;
                $('.delete').on('click', function () {
                    if ($(this).hasClass('on')) {
                        return false
                    } else {
                        $(this).addClass('on');
                        _this.shanchuajax();
                    }
                })
            },
            shanchuajax: function () {
                $.ajax({
                    url: '/Api/v1/Member/Address/' + id,
                    dataType:'json',
                    type: 'delete',
                    data: {
                        addressId: id
                    }
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        $.oppo('成功删除地址', 1, function () {
                            window.location.href("/Html/html/personalcenter/myinfo.html")
                        })
                    }
                })
            }

        }
    })
})
