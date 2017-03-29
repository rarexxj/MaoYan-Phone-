$(function () {
    $.ADDLOAD();
    new Vue({
        el: '#main',
        data: {
            banner: [],
            proinfo: [],
            hotinfo: [],
            prodata: {
                type: 3,
                pageNo: 1,
                limit: 10
            }
        },
        ready: function () {
            var _this = this;
            _this.bannerajax();
            _this.proinfoajax();
            _this.hotajax();
            _this.$nextTick(function () {
                _this.totop();
                _this.updownload();
                setTimeout(function () {
                    _this.swipe();
                }, 100)
            })
        },
        methods: {
            //banner
            bannerajax: function () {
                var _this = this
                $.ajax({
                    url: URL+'/Api/v1/Carousel/01',
                    type: 'get',
                    data: {
                        key: '03'
                    }
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.banner = rs.data;
                    }
                })
            },
            //banner滚动
            swipe: function () {
                //轮播
                new Swiper('.swiper-container', {
                    direction: 'horizontal',
                    paginationClickable: true,
                    autoplay: 2500,
                    loop: true,
                    autoplayDisableOnInteraction: false,
                    // 如果需要分页器
                    pagination: '.swiper-pagination'
                })
            },
            totop: function () {
                // scroll
                var obj = $(".scroll")[0];
                obj.onclick = function () {
                    var timer = setInterval(function () {
                        $("#index")[0].scrollTop -= 10;
                        if ($("#index")[0].scrollTop <= 0) {
                            clearInterval(timer);
                        }
                    }, 2);
                }

                // 窗口滚动检测
                $("#index")[0].onscroll = function () {
                    obj.style.display = ($("#index")[0].scrollTop >= 10) ? "block" : "none"
                }
            },
            others: function () {
                //左侧导航栏伸缩
                $('.box').on('click', '.toleft', function () {
                    $('.sidebarnav').addClass('left');
                    $('.sidebarnav').removeClass('right');
                    $(this).addClass('toright');
                    $(this).removeClass('toleft');
                })

                $('.box').on('click', '.toright', function () {
                    $('.sidebarnav').removeClass('left');
                    $('.sidebarnav').addClass('right');
                    $(this).addClass('toleft');
                    $(this).removeClass('toright');
                })

                var _height = $('.sidebarnav').height();
                var _height2 = $('.rightbar').height();
                console.log(_height);
                console.log(_height2);
                $('.sidebarnav').css('margin-top', -_height / 2)
                $('.rightbar').css('margin-top', -_height2 / 2)
            },
            //首页产品
            proinfoajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Mall/Home',
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.proinfo = rs.data;
                        _this.$nextTick(function () {
                            _this.others();
                        })
                    }
                })
            },
            hotajax: function () {
                var _this = this;
                $.ajax({
                    url: URL+'/Api/v1/Mall/Goods/Recommend',
                    type: 'get',
                    data: _this.prodata
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        window.allpage = Math.ceil(_this.hotinfo.TotalCount / _this.prodata.limit)
                        if (_this.prodata.pageNo == 1) {
                            _this.hotinfo = rs.data.Goods
                        } else {
                            _this.hotinfo = _this.hotinfo.concat(rs.data.Goods)
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
                        _this.prodata.pageNo++;
                        if (_this.prodata.pageNo > allpage) {
                            $.RMLOAD();
                        } else {
                            setTimeout(function () {
                                flag = true;
                            }, 500)
                            _this.hotajax();
                            $.ADDLOAD();
                        }
                    }
                })
            }
        }
    })
})