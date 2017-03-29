$(function () {
    $.ADDLOAD();
    $.checkuser();
    new Vue({
        el: '#main',
        data: {
            info: [],
            ajaxdata:{
                pageNo: '1',
                limit: '10'
            }
        },
        ready: function () {
            var _this = this;
            _this.infoajax();
            _this.$nextTick(function () {
                _this.deletejilu();
            })
        },
        methods: {
            infoajax: function () {
                var _this = this;
                $.ajax({
                    url: URL+'/Api/v1/Mall/Browse',
                    type: 'get',
                    data: _this.ajaxdata
                }).done(function (rs) {
                    if (rs.returnCode = '200') {
                        window.allpage = Math.ceil(rs.data.TotalCount / _this.ajaxdata.limit)
                        if (_this.ajaxdata.pageNo == 1) {
                            _this.info = rs.data.Goods;
                        } else {
                            _this.info = _this.info.concat(rs.data.Goods);
                        }
                        $.RMLOAD();
                    }
                })
            },
            updownload: function () {
                var _this = this;
                var flag = true;
                $(window).scroll(function () {
                    var H = $('.scrolltop')[0].getBoundingClientRect().top;
                    var h = $(window).height();
                    if (H - h < 0 && flag == true) {
                        flag = false;
                        _this.ajaxdata.pageNo++;
                        if ( _this.ajaxdata.pageNo > allpage) {
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
            },
            deletejilu: function () {
                //删除浏览记录
                var _this = this;
                $('#main').on('click','.history .delete', function () {
                    _this.deleteajax();
                })
            },
            deleteajax: function () {
                $.ajax({
                    url: URL+'/Api/v1/Mall/Browse',
                    type: 'delete'
                }).done(function (rs) {
                    if (rs.returnCode = '200') {
                        $.oppo('成功清理浏览记录', 1, function () {
                            window.location.href = "/Html/html/personalcenter/personalcenter.html"
                        })
                    }
                })
            }
        }
    })
})