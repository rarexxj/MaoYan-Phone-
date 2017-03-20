$(function () {
    $.ADDLOAD();
    new Vue({
        el: '#main',
        data: {
            banner: [],
            proinfo: [],
            telinfo:[]
        },
        ready: function () {
            var _this = this;
            _this.bannerajax();
            _this.proinfoajax();
            _this.$nextTick(function () {
                _this.totop();
                _this.others();
                _this.tel();
                setTimeout(function () {

                }, 300)

            })
        },
        methods: {
            //banner
            bannerajax: function () {
                var _this = this
                $.ajax({
                    url: '/Api/v1/Carousel/01',
                    type: 'get',
                    data: {
                        key: '01'
                    }
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.banner = rs.data;
                        _this.$nextTick(function () {
                            _this.swipe();
                        })
                        _this.urltype();
                    }
                })
            },
            urltype: function () {
                var _this = this;
                for (i = 0; i < _this.banner.length; i++) {
                    if (_this.banner[i].ShotUrl) {
                        // _this.bannerimg.push(_this.banner[i].ShotUrl)
                        if (_this.banner[i].ShotUrl.indexOf('|') > -1) {
                            _this.banner[i].url = 'detail.html?id=' + _this.banner[i].ShotUrl.split('|')[1];
                            // img.push({url:'detail.html?id='+_this.banner[i].ShotUrl.split('|')[1]});
                        } else {
                            _this.banner[i].url = _this.banner[i].ShotUrl;
                            // img.push({url:_this.banner[i].ShotUrl})
                        }
                    } else {

                    }
                }
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


            },
            poheight:function () {
                var _height = $('.sidebarnav').height();
                var _height2 = $('.rightbar').height();
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
                        for (i=0;i<rs.data.OneLevelCategorys.length;i++){
                            rs.data.OneLevelCategorys[i].maodian=i;
                        }
                        for(j=0;j<rs.data.Categories.length;j++){
                            rs.data.Categories[i].maodian=i;
                        }
                        _this.$nextTick(function(){
                            _this.poheight();
                            $.RMLOAD();
                        })

                    }
                })
            },
            tel:function () {
                var _this=this;
                $.ajax({
                    url: '/Api/v1/CustomPhone',
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.telinfo = rs;
                    }
                })
            }
        }
    })
})