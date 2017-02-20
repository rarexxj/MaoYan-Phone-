$(function () {
    $.ADDLOAD();
    new Vue({
        el: '#index',
        data: {
            banner: [],
            goldprice: [],
            protype: [],
            adver: [],
            pro: [],
            phone: [],
            title: [],
            address: [],
            mapinfo: [],
            taginfo:[]
        },
        ready: function () {
            var t = this;
            t.bannerajax();
            t.goldpriceajax();
            t.protypeajax();
            t.adajax();
            t.hotajax();
            t.phoneajax();
            t.indtitajax();
            t.searchlink();
            t.tagajax();
            // t.pimg();
            t.$nextTick(function () {
                setTimeout(function () {
                    t.swipe();
                    t.bannerlink();
                }, 100)
            })
        },
        methods: {
            //banner
            bannerajax: function () {
                var t = this
                $.ajax({
                    url: '/Api/v1/Carousel/01',
                    type: 'get',
                    data: {
                        key: '01'
                    }
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        t.banner = rs.data;
                        $.RMLOAD();
                    }
                })
            },
            //黄金价格ajax
            goldpriceajax: function () {
                var t = this
                $.ajax({
                    url: '/Api/v1/Mall/Gold/GetPrice',
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        t.goldprice = rs.data;
                    }
                })
            },
            //2大分类
            protypeajax: function () {
                var t = this
                $.ajax({
                    url: '/Api/v1/Mall/GoodsCategory',
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        t.protype = rs
                    }
                })
            },
            //明星产品广告
            adajax: function () {
                var t = this
                $.ajax({
                    url: '/Api/v1/Advert/01',
                    type: 'get',
                    data: {
                        Key: '01'
                    }
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        t.adver = rs
                    }
                })
            },
            //畅销产品
            hotajax: function () {
                var t = this
                $.ajax({
                    url: '/Api/v1/Mall/Goods',
                    type: 'post',
                    data: {
                        KeyWord: '',
                        SortType: '3',
                        PageNo: '1',
                        Limit: '10',
                        CategoryId: '',
                        MinPrice: '',
                        MaxPrice: '',
                        Type: 'Hot',
                        GoodsType: '2'
                    }
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        t.pro = rs.data
                    }
                })
            },
            //电话
            phoneajax: function () {
                var t = this
                $.ajax({
                    url: '/Api/v1/GetPhone',
                    type: 'get',
                    data: {}
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        t.phone = rs
                    }
                })
            },
            indtitajax: function () {
                var t = this;
                $.ajax({
                    url: '/Api/v1/Tag/Tags/BntWeb-Mall',
                    type: 'get',
                    data: {}
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        t.title = rs
                    }
                })
            },
            //banner滚动
            swipe: function () {
                new Swiper('.swiper-container', {
                    direction: 'horizontal',
                    loop: true,
                    paginationClickable: true,
                    autoplay: 2500,
                    slidesPerView: 'auto',
                    centeredSlides: true,
                    grabCursor: true,
                    autoplayDisableOnInteraction: false,
                    // 如果需要分页器
                    pagination: '.swiper-pagination'
                })
            },
            //链接
            bannerlink: function () {
                var _this = this;
                $('.goods').on('click', function () {
                    window.location.href = '/Html/Prodetails.html?id=' + $(this).attr('data-id')
                })
                $('.goodscate').on('click', function () {
                    window.location.href = '/Html/informationdetails.html?id=' + $(this).attr('data-id')
                })
                $('.mingxadver').on('click', function () {
                    window.location.href = 'Html/prodetails.html?id=' + $(this).attr('data-id')
                })
            },
            searchlink: function () {
                $('.index-banner-r').on('click', function () {
                    $('.search-box').show(100, function () {
                        setTimeout(function () {
                            $('#search_alert').trigger('focus')
                        }, 2000)
                    });


                })
                $('.search-box .close').on('click', function () {
                    $('.search-box').hide();
                })
                $('#index').on('click', '.search-box .btn', function () {
                    var val = $('.search-box .text').val()
                    if (val == '') {
                        console.log(312)
                        $.oppo('请输入关键字', 1)
                    } else {
                        window.location.href = "/Html/searchlist.html?key=" + encodeURIComponent(encodeURIComponent(val))
                    }
                })
                $('#index').on('click', '.search-box .con li',function () {
                    var val = $(this).attr('data-tag')
                    window.location.href = "/Html/searchlist.html?key=" + encodeURIComponent(encodeURIComponent(val))
                })
                $('.search-box .text').on('keyup', function () {
                    if ($(this).val() == '') {
                        $('.search-box .delete').hide();
                    } else {
                        $('.search-box .delete').show();
                    }
                })
                $('.search-box .delete').on('click', function () {
                    $('.search-box .text').val('');
                    $(this).hide();
                })
            },
            //获取商品标签
            tagajax: function () {
                var _this=this;
                $.ajax({
                    url: '/Api/v1/Goods/Tags/BntWeb-Mall',
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                       _this.taginfo=rs.data
                    }
                })
            }
            // pimg:function () {
            //     var _this=this;
            //     var map = new BMap.Map("allmap");
            //     var point = new BMap.Point(116.331398,39.897445);
            //     map.centerAndZoom(point,12);
            //     var geolocation = new BMap.Geolocation();
            //     geolocation.getCurrentPosition(function(r){
            //         _this.mapinfo=r.address;
            //     },{enableHighAccuracy: true})
            // }

        }
    })

})