$(function () {
    $.ADDLOAD();
    window.TOKEN = localStorage.getItem('qy_loginToken');
    console.log(TOKEN)
    var id = $.getUrlParam('id');
    new Vue({
        el: '#main',
        data: {
            goodId: id,
            info: [],
            imgs: '',
            lei: [],
            tipinfo: [],
            evadata: {
                goodsId: id,
                limit: 10,
                pageNo: 1
            },
            evainfo: [],
            headinfo: [],
            banner:[]
        },
        ready: function () {
            var _this = this;
            _this.ajax();
            _this.$nextTick(function () {
                // _this.bannerajax();
                _this.guige();
                _this.Calculation();
                _this.tab();
                _this.collect();
                _this.evaajax();

                _this.gotobuy();
                _this.headimg();
                _this.jjgou();
                _this.putshopcar();
            })
        },

        methods: {
            //banner
            bannerajax: function () {
                var _this = this;
                $.ajax({
                    // url: URL+'/Api/v1/Carousel/01',
                    url: '/Api/v1/Carousel/01',
                    type: 'get',
                    data: {
                        key: '04'
                    }
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.banner = rs.data;
                    }
                })
            },
            ajax: function () {
                var _this = this;
                if (localStorage.getItem('qy_loginToken')) {
                    $.checkuser();
                    _this.liuljl();
                }
                $.ajax({
                    // url: URL+'/Api/v1/Mall/Goods/Attribute',
                    url:'/Api/v1/Mall/Goods/Attribute',
                    type: 'get',
                    data: {
                        goodId: _this.goodId
                    }
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data;
                        _this.guigechioce();
                        _this.$nextTick(function () {
                            _this.swipe();
                        })
                        $.RMLOAD()
                    }
                });
            },
            //banner滚动
            swipe: function () {
                var mySwiper = new Swiper('.swiper-container', {
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
            guige: function () {
                //
                var _this = this;
                $('.size').on('click', function () {
                    $('.size-mask').show();
                })
                //关闭
                $('#main').on('click', '.size-mask .close', function () {
                    $('.size-mask').hide();
                })
            },
            Calculation: function () {
                //加
                $('#main').on('click', '.getnum .jia', function () {

                    var max = parseInt($('.kc').html());
                    var num = $(this).parents('.numbox').find('.amount').val();
                    console.log(num)
                    if (num >= max) {
                        num = max;
                        $.oppo('已没有更多库存', 1)
                    } else {
                        num++;
                    }
                    $(this).parents('.numbox').find('.amount').val(num)
                })
                //减
                $('#main').on('click', '.getnum .jian', function () {
                    var num = $(this).parents('.numbox').find('.amount').val();
                    if (num <= 1) {
                        num = 1;
                    } else {
                        num--;
                    }
                    $(this).parents('.numbox').find('.amount').val(num)
                })
            },
            //切换
            tab: function () {
                $('.information .btn').eq(0).on('click', function () {
                    $(this).addClass('cur').siblings().removeClass('cur');
                    $('.infoajax').show();
                    $('.evaluateajax').hide();
                })
                $('.information .btn').eq(1).on('click', function () {
                    $(this).addClass('cur').siblings().removeClass('cur');
                    $('.infoajax').hide();
                    $('.evaluateajax').show();
                })
            },
            //点击收藏
            collect: function () {
                var _this = this;
                $('#main .collect').on('click', function () {
                    if (!window.TOKEN) {
                        $.oppo('您还未登陆', 1);
                    } else {
                        $.checkuser();
                        _this.ajaxadd();
                        // if ($(this).hasClass('cur')) {
                        //     _this.ajaxcancel()
                        // } else {
                        //     _this.ajaxadd();
                        // }
                    }
                })
            },
            //取消收藏
            ajaxcancel: function () {
                $.ajax({
                    // url: URL+'/Api/v1/Mall/Collect/' + id,
                    url:'/Api/v1/Mall/Collect/' + id,
                    type: 'DELETE'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        $.oppo('您已取消收藏', 2)
                        $('.goshop .coll').removeClass('cur');
                    }
                })
            },
            //添加收藏
            ajaxadd: function () {
                $.ajax({
                    // url: URL+'/Api/v1/Mall/Collect/' + id,
                    url: '/Api/v1/Mall/Collect/' + id,
                    type: 'post'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        $.oppo('您已添加收藏', 2)
                        $('.goshop .coll').addClass('cur');
                    }
                })
            },
            guigechioce: function () {
                var _this = this;
                window.proimg = _this.info.MainImage.MediumThumbnail;
                for (var i in _this.info.AttributeValues) {
                    if (!_this.info.AttributeValues[i].list) {
                        _this.info.AttributeValues[i].list = []
                    }
                    _this.info.AttributeValues[i].list = _this.info.AttributeValues[i].AttributeValues.split(',')
                }
                if (_this.info.MainImage[0]) {
                    _this.imgs = _this.info.MainImage[0].MediumThumbnail
                }
                for (var i = 0; i < _this.info.SingleGoogsList.length; i++) {
                    var datas1 = _this.info.SingleGoogsList[i]
                    if (!datas1['zuhe']) {
                        datas1['zuhe'] = {}
                    }
                    for (var j = 0; j < datas1.Attributes.length; j++) {
                        if (!datas1['zuhe']['id']) {
                            datas1['zuhe']['id'] = []
                        }
                        if (!datas1['zuhe']['name']) {
                            datas1['zuhe']['name'] = []
                        }
                        datas1['zuhe']['id'].push(datas1.Attributes[j].AttributeId)
                        datas1['zuhe']['name'].push(datas1.Attributes[j].AttributeValue)
                    }

                }
                //规格属性
                var czuhe = [];
                var lei = {};

                for (var i = 0; i < _this.info.AttributeValues[0].AttributeValues.toString().split(',').length; i++) {
                    var zn = _this.info.AttributeValues[0].AttributeValues.toString().split(',')[i];
                    var ci = []
                    for (var j = 0; j < _this.info.SingleGoogsList.length; j++) {
                        if (zn.toString() == _this.info.SingleGoogsList[j].Attributes[0].AttributeValue.toString()) {
                            if (_this.info.SingleGoogsList[j].Attributes[1]) {
                                ci.push(_this.info.SingleGoogsList[j].Attributes[1].AttributeValue.toString())
                            }
                        }
                    }
                    lei = {
                        zhu: zn,
                        ci: ci
                    }
                    czuhe.push(lei)
                }
                _this.info.czuhe = czuhe;
                _this.chooses();


            },
            chooses: function () {
                var _this = this;
                $('#main').on('click', '.choose-box>li', function () {
                    $('.getnum .amount').val(1)
                    $(this).addClass('choose-cur').siblings().removeClass('choose-cur')
                    _this.choose_zuhe();
                })
                $('.first .choose-box>li').on('click', function () {
                    _this.choose_gg()
                })
            },
            choose_zuhe: function () {
                var _this = this;
                var zuhe = []
                $('.choose-cur').each(function (index) {
                    var px = $(this).attr('data-px'),
                        id = $(this).attr('data-id'),
                        vals = $.trim($(this).text())
                    zuhe.push(vals)
                })
                console.log(zuhe)
                for (var i in _this.info.SingleGoogsList) {
                    var datas = _this.info.SingleGoogsList[i]
                    var addj = JSON.stringify(datas.zuhe.name.sort());
                    var bddj = JSON.stringify(zuhe.sort());
                    if (addj == bddj) {
                        // console.log(zuhe.sort())
                        _this.info.Price = datas.Price
                        _this.info.Stock = datas.Stock
                        if (datas.Image) {
                            _this.imgs = datas.Image.MediumThumbnail;
                            console.log(_this.info)
                        } else {
                            _this.imgs = proimg
                        }
                        $('.get-btn').attr('data-id', datas.SingleGoodsId)
                    }
                }

            },
            choose_gg: function () {
                var _this = this;
                var gg = [];
                $('.first .choose-cur').each(function (index) {
                    var px = $(this).attr('data-px'),
                        id = $(this).attr('data-id'),
                        vals = $(this).text()
                    gg.push(vals)
                })
                for (var i in _this.info.czuhe) {
                    var datas = _this.info.czuhe[i]
                    if (datas.zhu.toString() == gg.toString()) {
                        //datas.ci.join(',');
                        //console.log(datas.ci.join(','))
                        var html = '';
                        for (var i = 0; i < datas.ci.length; i++) {
                            html += '<li class="choose" data-px="' + i + '">';
                            html += datas.ci[i];
                            html += '</li>'
                        }
                        $('.tc.other ul').html(html)
                        _this.chooses()
                    }
                }
            },
            //加入购物车
            putshopcar: function () {
                var _this = this;
                $('.addshopc').on('click', function () {
                    $('.size-mask').show();
                })
                $('#main').on('click', '.pro-in-gocart', function () {
                    if (!TOKEN) {
                        $.oppo('您还未登录', 1, function () {
                            window.location.href = '/Html/html/personalcenter/login.html';
                            // if ($.is_weixin()) {
                            //     window.location.href = '/WeiXin/Login';
                            // } else {
                            //     window.location.href = '/Html/html/personalcenter/login.html';
                            // }
                        })
                    } else {
                        if ($(this).hasClass('on')) {
                            return false
                        } else {
                            if (!$('.get-btn').attr('data-id')) {
                                $.oppo('请选择规格', 1);
                            } else {
                                if ($('.kc').html() == 0) {
                                    $.oppo('库存不足', 1)
                                } else {
                                    $.checkuser();
                                    $(this).addClass('on')
                                    if($('.jiajg').attr('data-jjid')==0){
                                        $('.jiajg').attr('data-jjid','')
                                    }
                                    var listdata = {
                                        GoodsId: id,
                                        SingleGoodsId: $('.get-btn').attr('data-id'),
                                        Quantity: $('.getnum .amount').val(),
                                        PurchaseId:$('.jiajg').attr('data-jjid')
                                    }
                                    _this.addshopcar(listdata);
                                }
                            }
                        }
                    }
                });
                $('.goshopc').on('click', function () {
                    // window.location.href = '/Html/shopcar.html'
                })
            },
            addshopcar: function (listdata) {
                $.ajax({
                    // url: URL+'/Api/v1/Mall/Cart',
                    url: '/Api/v1/Mall/Cart',
                    type: 'post',
                    data: listdata
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        $('.size-mask').hide();
                        $.oppo('成功加入购物车', 1)
                    }
                }).always(function () {
                    $('.addshopc,.pro-in-gocart').removeClass('on')
                })
            },
            gotobuy: function () {
                //立即购买
                var _this = this;
                $('#main').on('click', '.pro-in-gobuy', function () {
                    if (!TOKEN) {
                        $.oppo('您还未登录', 1, function () {
                            window.location.href = '/Html/html/personalcenter/login.html';
                            // if ($.is_weixin()) {
                            //     window.location.href = '/WeiXin/Login';
                            // } else {
                            //     window.location.href = '/Html/html/personalcenter/login.html';
                            // }
                        })
                    } else {
                        if ($(this).hasClass('on')) {
                            return false
                        } else {
                            if (!$('.get-btn').attr('data-id')) {
                                $.oppo('请选择规格', 1);
                                // $('.size-mask').show();
                            } else if($('.jiajg').length>0&&(!$('.jiajg').attr('data-jjid'))){
                                $.oppo('请选择加价购', 1);
                            }else {
                                if ($('.kc').html() == 0) {
                                    $.oppo('库存不足', 1)
                                } else {
                                    window.location.href = "/Html/html/shopcar/settlement.html?gid=" + $('.get-btn').attr('data-id') + "|" + $('.getnum .amount').val()+"&jjid="+$('.jiajg').attr('data-jjid')
                                }

                            }
                        }
                    }
                });
                //购买
                $('#main').on('click', '.gobuy', function () {
                    if (window.TOKEN) {
                        if (!($('.get-btn').attr('data-id')&&$('.jiajg').attr('data-jjid'))) {
                            $('.size-mask').show();
                        } else {
                            if ($('.kc').html() == 0) {
                                $.oppo('库存不足', 1)
                            } else {
                                window.location.href = "/Html/html/shopcar/settlement.html?gid=" + $('.get-btn').attr('data-id') + "|" + $('.getnum .amount').val()+"&jjid="+$('.jiajg').attr('data-jjid')
                            }

                        }
                    } else {
                        $.oppo('请先登录', 1)
                        setTimeout(function () {
                            window.location.href = '/Html/html/personalcenter/login.html'
                        }, 1000)
                    }
                })

            },
            //评价
            evaajax: function () {
                var _this = this;
                $.ajax({
                    // url: URL+'/Api/v1/Mall/GoodsEvaluates',
                    url: '/Api/v1/Mall/GoodsEvaluates',
                    type: 'get',
                    data: _this.evadata
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.evainfo = rs.data;
                    }
                })
            },
            //前9名头像
            headimg: function () {
                var _this = this;
                $.ajax({
                    // url: URL+'/Api/v1/Mall/GoodsEvaluates/Top',
                    url: '/Api/v1/Mall/GoodsEvaluates/Top',
                    type: 'get',
                    data: {
                        goodsId: _this.goodId
                    }
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.headinfo = rs.data;
                    }
                })
            },
            liuljl: function () {
                var _this = this;
                $.ajax({
                    // url: URL+'/Api/v1/Mall/Browse/' + id,
                    url: '/Api/v1/Mall/Browse/' + id,
                    type: 'POST'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                    }
                })

            },
            //加价购
            jjgou: function () {
                $('.infoBox').on('click', '.jjbox', function () {
                    $('.quan').removeClass('cur');
                    $(this).children('.quan').addClass('cur');
                    var id=$(this).attr('data-jjid');
                    console.log(id)
                    $('.jiajg').attr('data-jjid',id)
                })
            }
        }
    })
})