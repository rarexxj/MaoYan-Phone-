$(function () {
    $.ADDLOAD();
    var id = $.getUrlParam('id');
    var data1 = {

    }
    new Vue({
        el: '#mainlist',
        data: {
            info: [],
            telinfo:[],
            ajaxdata:{
                PageNo: '1',
                Limit: '10',
                CategoryId: id,
                SortType: '0'
            },
            taginfo:[]
        },
        ready: function () {
            var _this = this;
            _this.zongheajax();
            _this.$nextTick(function () {
                _this.tel();
                _this.totop();
                _this.zonghpx();
                _this.xiaolpx();
                _this.searchlink();
                _this.tagajax();
                // _this.shaixox();
                _this.updownload();
            })
        },
        methods: {
            zongheajax: function () {
                var _this = this;
                $.ajax({
                    // url: URL+'/Api/v1/Mall/Goods',
                    url:'/Api/v1/Mall/Goods',
                    type: 'post',
                    data: _this.ajaxdata
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        window.allpage = (rs.data.TotalCount) / (_this.ajaxdata.Limit);
                        if ( _this.ajaxdata.PageNo == 1) {
                            _this.info = rs.data.Goods;
                        } else {
                            _this.info = _this.info.concat(rs.data.Goods);
                        }
                        $.RMLOAD();
                    }
                })
            },
            zonghpx: function () {
                //综合排序
                var _this = this;
                $('.sort').on('click', function () {
                    $('.sort-mask').show();
                    $('.screen-mask').hide();
                })
                $('.box').on('click', '.sort-mask li', function () {
                    $('.sort').addClass('cur');
                    $('.num').removeClass('cur');
                    $('.screen').removeClass('cur');
                    $(this).addClass('typecur').siblings().removeClass('typecur');
                    var nSortType = $(this).attr('data-type');
                    _this.ajaxdata.SortType = nSortType;
                    _this.ajaxdata.PageNo = 1;
                    _this.zongheajax();
                    console.log( _this.ajaxdata.SortType)
                })
                $('.sort-mask').on('click', function () {
                    $(this).hide();
                    $('.screen-mask').hide();
                })
            },
            xiaolpx: function () {
                //销量排序
                var _this = this;
                $('.box').on('click', '.num', function () {
                    $(this).addClass('cur').siblings('.chobtn').removeClass('cur');
                    $('.sort-mask li').removeClass('typecur');
                    $('.sort-mask').hide();
                    $('.screen-mask').hide();
                    _this.ajaxdata.SortType = 3;
                    _this.ajaxdata.PageNo = 1;
                    _this.zongheajax();
                    console.log( _this.ajaxdata.SortType)
                })
            },
            //筛选
            // shaixox: function () {
            //     //高级筛选
            //     $('.screen').on('click', function () {
            //         $('.screen-mask').show();
            //     })
            //
            //     $('.box').on('click', '.screen-box .outbox li', function () {
            //         if ($(this).hasClass('cur')) {
            //             $(this).removeClass('cur');
            //         } else {
            //             $(this).addClass('cur');
            //         }
            //     })
            //     $('.box').on('click', '.shaixtitle', function () {
            //         $('.screen-mask').hide();
            //     })
            //     $('.box').on('click', '.screen-btn', function () {
            //         var cha = $('.text1').val() - $('.text2').val();
            //         if ($('.text1').val() == '' && $('.text2').val() == '') {
            //             $.oppo('输入的价格不能为空', 1);
            //             return false
            //         }
            //         if (cha > 0) {
            //             $.oppo('输入的价格区间有误', 1);
            //             return false
            //         }
            //         _this.ajaxdata.MinPrice = $('.text1').val();
            //         _this.ajaxdata.MaxPrice = $('.text2').val();
            //         _this.ajaxdata.SortType = '';
            //         _this.ajaxdata.PageNo = 1;
            //         $('.screen-mask').hide();
            //         $('.chioce').hide();
            //         _this.zongheajax();
            //     })
            // },
            updownload: function () {
                var _this = this;
                var flag = true;
                $('#index').scroll(function () {
                    var H = $('.scrolltop')[0].getBoundingClientRect().top;
                    var h = $(window).height();
                    if (H - h < 0 && flag == true) {
                        flag = false;
                        _this.ajaxdata.PageNo++;
                        _this.ajaxdata.SortType = $('.typecur').attr('data-type');
                        if ( _this.ajaxdata.PageNo > Math.ceil(allpage)) {
                            $.RMLOAD();
                        } else {
                            setTimeout(function () {
                                flag = true;
                            }, 500)
                            _this.zongheajax();
                            $.ADDLOAD();
                        }
                    }
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

                var _height2 = $('.rightbar').height();
                $('.rightbar').css('margin-top', -_height2)
            },
            searchlink: function () {
                $('.products-nav .btn').on('click', function () {
                    $('.search-box').show(100, function () {
                        setTimeout(function () {
                            $('#search_alert').trigger('focus')
                        }, 2000)
                    });
                })
                $('.search-box .close').on('click', function () {
                    $('.search-box').hide();
                })

                $('#mainlist').on('click', '.search-box .btn', function () {
                    var val = $('.search-box .text').val()
                    if (val == '') {
                        $.oppo('请输入关键字', 1)
                    } else {
                        window.location.href = "/Html/html/fenlei/searchlist.html?key=" + encodeURIComponent(encodeURIComponent(val))
                    }
                })
                $('#mainlist').on('click', '.search-box .con li',function () {
                    var val = $(this).attr('data-tag')
                    window.location.href = "/Html/html/fenlei/searchlist.html?key=" + encodeURIComponent(encodeURIComponent(val))
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
                    // url: URL+'/Api/v1/Goods/Tags/BntWeb-Mall',
                    url: '/Api/v1/Goods/Tags/BntWeb-Mall',
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.taginfo=rs.data
                    }
                })
            },
            tel:function () {
                var _this=this;
                $.ajax({
                    // url: URL+'/Api/v1/CustomPhone',
                    url: '/Api/v1/CustomPhone',
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.telinfo = rs;
                        $.RMLOAD();
                    }
                })
            }
        }
    })
})
