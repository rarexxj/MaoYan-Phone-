$(function () {
    var i = $.getUrlParam('type');
    console.log(i);
    $.ADDLOAD()
    new Vue({
        el: '#infoList',
        data: {
            info: [],
            data1:{
                pageNo: 1,
                limit: 10,
                type: i
            }
        },
        ready: function () {
            var _this = this;
            _this.infoajax();
            _this.$nextTick(function () {
                _this.xxload();
                $.RMLOAD();
            })
        },
        methods: {
            infoajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Article',
                    type: 'get',
                    data: _this.data1
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data;
                        window.allpage=_this.info.TotalCount/_this.data1.limit;
                        // 判断类型
                        $('.navul li').eq(i).find('a').addClass('active');
                    }
                })
            },
            xxload: function () {
                var _this = this;
                var flag = true;
                $(window).scroll(function () {
                    var H = $('.scrolltop')[0].getBoundingClientRect().top;
                    var h = $(window).height();
                    if (H - h < 0 && flag == true) {
                        flag = false;
                        _this.data1.pageNo++;
                        if (_this.data1.pageNo > Math.ceil(allpage)) {
                            $.RMLOAD();
                        } else {
                            setTimeout(function () {
                                flag = true;
                            }, 500)
                            _this.infoajax();
                            $.ADDLOAD();
                        }
                    }
                })
            }
        }
    })
})