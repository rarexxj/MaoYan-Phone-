$(function () {
    $.ADDLOAD()
    window.TOKEN = localStorage.getItem('qy_loginToken');
    var id = $.getUrlParam('id')
    new Vue({
        el: '#infodetails',
        data: {
            info: []
        },
        ready: function () {
            var _this = this;
            _this.infoajax();
            _this.$nextTick(function () {
                _this.shoucang();
                $.RMLOAD();
            })
        },
        methods: {
            infoajax: function () {
                var _this = this;
                if (localStorage.getItem('qy_loginToken')) {
                    $.checkuser();
                }
                $.ajax({
                    url: '/Api/v1/ArticleInfo',
                    type: 'get',
                    data: {
                        id: id
                    }
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data;
                        if (_this.info.HasCollect) {
                            $('.shouclogo').addClass('cur');
                        }

                    }
                })
            },
            shoucang: function () {
                var _this = this;
                $('.infoList').on('click', '.shouclogo',function () {
                    if (!window.TOKEN) {
                        $.oppo('您还未登录', 1);
                        setTimeout(function () {
                            window.location.href='/Html/login.html'
                        },1000)
                    } else {
                        if ($(this).hasClass('cur')) {
                            _this.shanchu();
                        } else {
                            _this.tianjia();
                        }
                    }
                })
            },
            tianjia: function () {
                $.ajax({
                    url: '/Api/v1/Article/Collect/'+id,
                    type: 'post'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        $.oppo('您已添加收藏', 1)
                        $('.shouclogo').addClass('cur');
                    }


                })
            },
            shanchu: function () {
                $.ajax({
                    url: '/Api/v1/Article/CancleCollect/'+id,
                    type: 'DELETE'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        $.oppo('您已取消收藏', 1)
                        $('.shouclogo').removeClass('cur');
                    }
                })
            }
        }
    })
})