$(function () {
    $.ADDLOAD()
    $.checkuser();
    new Vue({
        el: '#collect',
        data: {
            info: [],
            data1:{
                pageNo: 1,
                limit: 8,
                type:1
            }
        },
        ready: function () {
            var _this = this;
            _this.collectajax();
            _this.$nextTick(function () {
                _this.quanclick();
                _this.allclick();
                _this.shanchu();
                _this.updownload();

            })
        },
        methods: {
            collectajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Mall/Collect',
                    type: 'get',
                    data: _this.data1
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data;
                        if(rs.data.Goods.length==0){
                            $('.nothing').show();
                            $('.infoList').hide();
                        }
                        window.allpage=_this.info.TotalCount/_this.data1.limit;
                        $.RMLOAD();
                    }
                })
            },
            //单选
            quanclick: function () {
                $('.infoList').on('click', '.quan', function () {
                    $(this).toggleClass('active');
                })
            },
            //全选
            allclick: function () {
                var btn = true;
                $('.all').on('click', function () {
                    if (btn) {
                        $(this).addClass('active');
                        for (i = 0; i < $('.listmainbox .main').length; i++) {
                            $('.main').eq(i).find('.quan').addClass('active')
                        }
                        btn = false;

                    } else {
                        $(this).removeClass('active');
                        for (i = 0; i < $('.listmainbox .main').length; i++) {
                            $('.main').eq(i).find('.quan').removeClass('active')
                        }
                        btn = true;
                    }

                })
            },
            shanchu: function () {
                //删除
                var _this = this;
                var sid = [];
                $('.delete').on('click', function () {
                    $('.listmainbox .quan').each(function () {
                        if ($(this).hasClass('active')) {
                            console.log(123)
                            var id = $(this).parents('.main').attr('data-id');
                            sid.push(id);
                            // $(this).parents('.main').remove();
                            // xid = sid.join();
                        }
                    });
                    if (sid.length) {
                        $.ajax({
                            url: '/Api/v1/Mall/DeleteCollect',
                            type: 'DELETE',
                            data: {
                                Ids: sid
                            }
                        }).done(function (rs) {
                            if (rs.returnCode == '200') {
                                $.oppo('删除成功', 1, function () {
                                    window.location.href = "/Html/collectTz.html"
                                })
                            }
                        })
                    } else {
                        $.oppo('请选择删除的商品', 1);
                        return false;
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
                        _this.data1.pageNo++;
                        if (_this.data1.pageNo > Math.ceil(allpage)) {
                            $.RMLOAD();
                        } else {
                            setTimeout(function () {
                                flag = true;
                            }, 500)
                            _this.collectajax();
                            $.ADDLOAD();
                        }
                    }
                })
            }
        }
    })
})