$(function () {
    $.ADDLOAD()
    var id=$.getUrlParam('id')
    $.checkuser();
    new Vue({
        el: '#main',
        data: {
            info: []
        },
        ready: function () {
            var _this = this;
            _this.infoajax();
            _this.btnclick();
            _this.$nextTick(function () {
                $.RMLOAD();
            })
        },
        methods: {
            infoajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Mall/Order/'+id,
                    type: 'GET'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data
                    }
                })
            },
            btnclick: function () {
                var _this = this;
                //取消订单
                $('#main').on('click', '.quxiao', function () {
                    _this.cancleajax();
                })
                //确认收货
                $('#main').on('click', '.confirm-btn', function () {
                    _this.sureajax();
                })
                //确认删除订单
                $('#main').on('click', '.delete-btn', function () {
                    _this.deletajax();
                })
                //补足余款
                $('#main').on('click','.paymor-btn',function(){

                })
            },
            cancleajax: function () {
                $.ajax({
                    url: '/Api/v1/Order/' + id + '/Cancel',
                    type: 'PATCH'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        $.oppo('成功取消订单', 1, function () {
                            window.location.replace("/Html/costorder.html?orderType=0")
                        })
                    }
                })
            },
            sureajax: function () {
                $.ajax({
                    url: '/Api/v1/Order/' + id + '/Complete',
                    type: 'PATCH'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        $.oppo('成功确认收货', 1, function () {
                            window.location.replace("/Html/costorder.html?orderType=0")
                        })
                    }
                })
            },
            remindajax: function () {
                $.ajax({
                    url: '/Api/v1/Order/' + id + '/Remind',
                    type: 'post'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        $.oppo('提醒成功', 1)
                    }
                })
            },
            deletajax: function () {
                $.ajax({
                    url: '/Api/v1/Order/' + id,
                    type: 'DELETE'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        $.oppo('成功删除订单', 1, function () {
                            window.location.replace("/Html/costorder.html?orderType=0")
                        })
                    }
                })
            }

        }
    })
})