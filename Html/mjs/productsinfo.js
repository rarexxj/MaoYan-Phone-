$(function () {
    $.ADDLOAD();
    window.TOKEN = localStorage.getItem('qy_loginToken');
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
            headinfo: []
        },
        ready: function () {
            var _this = this;
            _this.ajax();
            _this.$nextTick(function () {
                setTimeout(function () {
                    _this.swipe();
                }, 400);
                _this.guige();
                _this.Calculation();
                _this.tab();
                _this.collect();
                _this.evaajax();
                _this.gotobuy();
                _this.headimg();
                _this.putshopcar();
            })
        },

        methods: {
            ajax: function () {
                var _this = this;
                if (localStorage.getItem('qy_loginToken')) {
                    $.checkuser()
                }
                $.ajax({
                    url: '/Api/v1/Mall/Goods/Attribute',
                    type: 'get',
                    data: {
                        goodId: _this.goodId
                    }
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data;
                        _this.guigechioce();
                        $.RMLOAD()
                    }
                });
            },
            //banner滚动
            swipe: function () {
                var mySwiper =new Swiper('.swiper-container', {
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
                    url: '/Api/v1/Mall/Collect/' + id,
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
                            if ($.is_weixin()) {
                                window.location.href = '/WeiXin/Login';
                            } else {
                                window.location.href = '/Html/html/personalcenter/login.html';
                            }
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
                                    var listdata = {
                                        GoodsId: id,
                                        SingleGoodsId: $('.get-btn').attr('data-id'),
                                        Quantity: $('.getnum .amount').val()
                                    }
                                    _this.addshopcar(listdata);
                                }
                            }
                        }
                    }
                });
                $('.goshopc').on('click', function () {
                    window.location.href = '/Html/shopcar.html'
                })
            },
            addshopcar: function (listdata) {
                $.ajax({
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
                    console.log(1321)
                    if (!TOKEN) {
                        $.oppo('您还未登录', 1, function () {
                            if ($.is_weixin()) {
                                window.location.href = '/WeiXin/Login';
                            } else {
                                window.location.href = '/Html/login.html';
                            }
                        })
                    } else {
                        if ($(this).hasClass('on')) {
                            return false
                        } else {
                            if (!$('.get-btn').attr('data-id')) {
                                $.oppo('请选择规格', 1);
                                // $('.size-mask').show();
                            } else {
                                if ($('.kc').html() == 0) {
                                    $.oppo('库存不足', 1)
                                } else {
                                    window.location.href = "/Html/getmethod.html?gid=" + $('.get-btn').attr('data-id') + "|" + $('.getnum .amount').val() + '&type=' + _this.info.Type
                                }

                            }
                        }
                    }
                });
                //购买
                $('#main').on('click', '.gobuy', function () {
                    console.log(2312321)
                    if (window.TOKEN) {
                        if (!$('.get-btn').attr('data-id')) {
                            $('.size-mask').show();
                        } else {
                            if ($('.kc').html() == 0) {
                                $.oppo('库存不足', 1)
                            } else {
                                window.location.href = "/Html/getmethod.html?gid=" + $('.get-btn').attr('data-id') + "|" + $('.getnum .amount').val() + '&type=' + _this.info.Type
                            }

                        }
                    } else {
                        $.oppo('请先登录', 1)
                        setTimeout(function () {
                            window.location.href = '/Html/login.html'
                        }, 1000)
                    }
                })

            },
            //评价
            evaajax: function () {
                var _this = this;
                $.ajax({
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
            }
        }
    })
})


// $(function () {
//     //效果js
//     mySwiper();
//     function mySwiper() {
//         var mySwiper = new Swiper('.swiper-container', {
//             direction: 'horizontal',
//             loop: true,
//             paginationClickable: true,
//             autoplay: 2500,
//             slidesPerView: 'auto',
//             centeredSlides: true,
//             grabCursor: true,
//             autoplayDisableOnInteraction: false,
//             // 如果需要分页器
//             pagination: '.swiper-pagination'
//         })
//     }
//
//
//     //选择
//     choose();
//     function choose() {
//         $('.size').on('click', function () {
//             $('.size-mask').show();
//         })
//         $('.size-mask .tc li').on('click', function () {
//             $(this).addClass('cur').siblings().removeClass('cur');
//         })
//         //关闭
//         $('.size-mask .close').on('click', function () {
//             $('.size-mask').hide();
//         })
//
//     }
//
//     //加价购
//     jjgou()
//     function jjgou() {
//         $('.infoBox').on('click', '.jjbox', function () {
//             $('.quan').removeClass('cur');
//             $(this).children('.quan').addClass('cur');
//         })
//     }
//
//
//
//
//     //切换
//     tab();
//     function tab() {
//         $('.information .btn').eq(0).on('click', function () {
//             $(this).addClass('cur').siblings().removeClass('cur');
//             $('.infoajax').show();
//             $('.evaluateajax').hide();
//         })
//         $('.information .btn').eq(1).on('click', function () {
//             $(this).addClass('cur').siblings().removeClass('cur');
//             $('.infoajax').hide();
//             $('.evaluateajax').show();
//         })
//     }
//
//     //收藏
//     coll();
//     function coll() {
//         $('.goshop .coll').on('click', function () {
//             $('.goshop .coll').addClass('on')
//             $.oppo('商品已加入收藏夹', 1)
//         })
//     }
//
//     //用户评价
//     // var flag = true;
//     // $(window).scroll(function () {
//     //     if ($('.eva-btn').hasClass('cur')) {
//     //         var H = $('body,html').height();
//     //         var h = $(window).height();
//     //         var t = $('body').scrollTop();
//     //         if (t >= H - h * 1.1 && flag == true) {
//     //             flag = false;
//     //             evadata.pageNo++;
//     //             if (evadata.pageNo > colpage) {
//     //                 $('.loading').hide();
//     //             } else {
//     //                 ajaxeva(function () {
//     //                     setTimeout(function () {
//     //                         flag = true;
//     //                     }, 500)
//     //                 }, function () {
//     //                     $('.loading').show();
//     //                 })
//     //             }
//     //         }
//     //     }
//     //
//     // })
//
//     //加入购物车
//     $('.addshopc').on('click', function () {
//         console.log(2312)
//         $.oppo('商品已加入购物车', 1)
//         // $('.size-mask').show();
//     })
//
//     $('.infoBox').on('click', '.pro-in-gocart', function () {
//         $(this).addClass('on')
//     })
//
//     //购买
//     $('.infoBox').on('click', '.gobuy', function () {
//         $('.size-mask').show();
//     })
//
// })0


// //用户评价
// var eval;
// var evadata={
//     goodsId:id,
//     pageNo:'1',
//     limit:'5'
//
// }
// ajaxeva();
// function ajaxeva(callback,beforecallback) {
//     $.ajax({
//         url:'/Api/v1/Mall/GoodsEvaluates',
//         type:'get',
//         data:evadata,
//         beforeSend:function () {
//             if(typeof beforecallback=='function'){beforecallback()}
//         }
//     }).done(function (rs) {
//         if (rs.returnCode == '200'){
//             vieweva(rs.data);
//             window.colpage=Math.ceil(rs.data.TotalCount/evadata.limit)
//         }else{
//             if(rs.returnCode == '401'){
//                 Backlog();
//             }else{
//                 oppo(rs.msg ,1)
//             }
//         }
//     }).always(function () {
//         if(typeof callback=='function'){callback()}
//     })
// }
//
//
// //评价前9名头像
// ajaxhead();
// function ajaxhead() {
//     $.ajax({
//         url:'/Api/v1/Mall/GoodsEvaluates/top',
//         type:'get',
//         data:{
//             goodsId:id
//         }
//     }).done(function (rs) {
//         if (rs.returnCode == '200'){
//             viewhead(rs)
//         }else{
//             if(rs.returnCode == '401'){
//                 Backlog();
//             }else{
//                 oppo(rs.msg ,1)
//             }
//         }
//     })
// }