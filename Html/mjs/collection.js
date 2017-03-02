$(function () {
    $.ADDLOAD();
    $.checkuser();
    new Vue({
        el: '#main',
        data: {
            info: [],
            ajaxdata: {
                pageNo: '1',
                limit: '5'
            }
        },
        ready: function () {
            var _this = this;
            _this.infoajax();
            _this.$nextTick(function () {
                _this.updownload();
                _this.deletecol();

            })
        },
        methods: {
            infoajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Mall/Collect',
                    type: 'get',
                    data: _this.ajaxdata
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
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
                        if ( _this.ajaxdata.pageNo > Math.ceil(allpage)) {
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
            //删除收藏
            deletecol: function () {
                var _this=this;
                $('.collection').on('click', '.box .delete', function (event) {
                    event.stopPropagation();
                    var id = $(this).parents('.box').attr('data-id');
                    _this.ajaxcancel(id);
                })
            },
            ajaxcancel: function (id) {
                $.ajax({
                    url: '/Api/v1/Mall/Collect/' + id,
                    type: 'DELETE'
                }).done(function (rs) {
                    if (rs.returnCode = '200') {
                        $.oppo('您已取消收藏', 1, function () {
                            window.location.href = "/Html/html/personalcenter/collection.html"
                        })
                    }
                })
            }
        }
    })
})