$(function () {
    $.ADDLOAD()
    if ($.is_weixin()) {
        var user = $.cookie('userInfo')
        if (user) {
            user = JSON.parse(plus.base64decodes(user))
            $.put_user(user)
            localStorage.setItem('qy_loginTokenyy', user.PhoneNumber + ':' + user.DynamicToken);
            //新增
            $.removeCookie('userInfo');
        }

    }
    window.TOKEN = localStorage.getItem('qy_loginTokenyy')
    if (window.TOKEN) {
        $.ajaxSetup({
            headers: {
                Authorization: 'Basic ' + window.base64encode(window.TOKEN)
            }
        })
    } else {
        window.location.href = '/Html/innerlogin.html'
    }

    new Vue({
        el: '#main',
        data: {
            TotalCount: '',
            price: '',
            info: [],
            beginTime: '',
            endTime: '',
            ul: [{month: '本月', dataId: '1'}, {month: '3个月', dataId: '3'}, {month: '6个月', dataId: '6'}, {
                month: '12个月',
                dataId: '12'
            }]
        },
        ready: function () {
            var _this = this;
            _this.$nextTick(function () {
                _this.searchajax();
                _this.timetab();
                $.RMLOAD()
            })
        },
        methods: {
            monthtab: function (index) {
                var _this = this;
                var data = {
                    beginTime: '',
                    endTime: '',
                    isPay: false,
                    pageNo: 1,
                    limit: 10,
                    months: _this.ul[index].dataId
                }
                $.ajax({
                    url: '/Api/v1/SalesPerson/MyChildsFee',
                    type: 'get',
                    data: data
                }).done(function (rs) {
                    if (rs.returnCode == 200) {
                        window.allpage = (rs.data.TotalCount) / (data.limit);
                        _this.info = rs.data.MemberFee
                        _this.price = rs.data.TotalFee;
                        _this.month=data.months;
                        var flag = true;
                        $(window).scroll(function () {
                            _this.info = _this.info.concat(rs.data.MemberFee);
                            var H = $('.scrolltop')[0].getBoundingClientRect().top;
                            var h = $(window).height();
                            if (H - h < 0 && flag == true) {
                                flag = false;
                                data.pageNo++;
                                if (data.pageNo > Math.ceil(allpage)) {
                                    $.RMLOAD();
                                } else {
                                    setTimeout(function () {
                                        flag = true;
                                    }, 500)
                                    _this.monthtab();
                                    $.ADDLOAD();
                                }
                            }
                        })
                    }
                })
            },
            searchajax: function () {
                var _this = this;
                $('.search').on('click', function () {
                    var t1 = new Date($('.time1').val()).getTime();
                    var t2 = new Date($('.time2').val()).getTime();
                    if (t1 && t2) {
                        if (t1 > t2) {
                            $.oppo('请输入正确的时间', 1)
                        } else {
                            var data2 = {
                                beginTime: _this.beginTime,
                                endTime: _this.endTime,
                                isPay: false,
                                pageNo: 1,
                                limit: 10
                            }
                            $.ajax({
                                url: '/Api/v1/SalesPerson/MyChildsFee',
                                type: 'get',
                                data: data2
                            }).done(function (rs) {
                                if (rs.returnCode == 200) {
                                    window.allpage = (rs.data.TotalCount) / (data2.limit);
                                    if (rs.data.TotalCount == 1) {
                                        _this.info = rs.data.MemberFee
                                    } else {
                                        _this.info = _this.info.concat(rs.data.MemberFee);
                                    }
                                    _this.price = rs.data.TotalFee
                                }
                            })
                            var flag = true;

                            $(window).scroll(function () {
                                var H = $('.scrolltop')[0].getBoundingClientRect().top;
                                var h = $(window).height();
                                if (H - h < 0 && flag == true) {
                                    flag = false;
                                    data2.pageNo++;
                                    if (data2.pageNo > Math.ceil(allpage)) {
                                        $.RMLOAD();
                                    } else {
                                        setTimeout(function () {
                                            flag = true;
                                        }, 500)
                                        _this.searchajax();
                                        $.ADDLOAD();
                                    }
                                }
                            })
                        }
                    } else {
                        $.oppo('请选择时间', 1)
                    }

                })
            },
            timetab: function () {
                $('.dateol li').on('click', function () {
                    $(this).addClass('active');
                    $(this).siblings().removeClass('active');
                })
            }
            // price: function () {
            //     var _price = 0;
            //     $('.mainnrdl .mainnrdd').each(function () {
            //         var dj = Number($(this).find('.je').html());
            //         console.log(dj);
            //         _price = _price + dj;
            //         $('.price').html(_price);
            //     })
            // },
            // fix: function () {
            //     $('.je').each(function () {
            //         $.fixed($(this));
            //     })
            //     $.fixed('.price');
            // }

        }
    })
})