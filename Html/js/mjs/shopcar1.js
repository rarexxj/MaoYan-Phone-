$(function () {
    $.ADDLOAD()
    $.checkuser();
    new Vue({
        el: '#main',
        data: {
            info: []
        },
        ready: function () {
            var _this = this;
            _this.infoajax();
            _this.$nextTick(function () {
                _this.quanclick();
                _this.chioceclick();
                _this.jisuan();
                _this.shanchu();
                _this.submit();
                setTimeout(function () {
                    _this.chushi();
                }, 300)
            })
        },
        methods: {
            infoajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Mall/Cart',
                    type: 'get',
                    data: {}
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data;
                        if(rs.data.ConsumerGoods.length==0){
                            $('.nothing').show();
                            $('.infoList').hide();
                        }
                        $.RMLOAD();
                    }
                })
            },
            //单选
            quanclick: function () {
                var _this = this;
                $('#main').on('click', '.quan', function () {
                    $(this).toggleClass('active');
                    _this.price();
                })
            },
            //计算价格
            price: function () {
                var sumprice = 0;
                var btn = false;
                $('.quan').each(function () {
                    var _price = Number($(this).siblings('a').find('.dprice').html());
                    if ($(this).hasClass('active')) {
                        sumprice = sumprice + _price;
                        btn = true;
                    }
                    if (!btn) {
                        _price = 0
                        sumprice = sumprice + _price;
                    }
                })
                $('.pricejs').html(sumprice.toFixed(2))
            },
            chushi: function () {
                $('.dprice').each(function () {
                    var dprice;
                    var num;
                    dprice = $(this).attr('data-price');
                    num = Number($(this).parents('.main').find('.num').val());
                    _price = dprice * num;
                    $(this).html(_price.toFixed(2));
                })
            },
            chioceclick: function () {
                var btn = true;
                var _this = this;
                $('.all').on('click', function () {
                    if (btn) {
                        $('.listmainbox .quan').each(function () {
                            $(this).addClass('active');
                            $('.all').addClass('active');
                            btn = false;
                        })
                    } else {
                        $('.listmainbox .quan').each(function () {
                            $(this).removeClass('active');
                            $('.all').removeClass('active');
                            btn = true;
                        })
                    }
                    _this.price();
                })
            },
            jisuan: function () {
                var _this = this;
                $('#main').on('click', '.add', function () {
                    var dprice = $(this).parents('.zl').attr('data-price');
                    var _val = Number($(this).siblings('.num').val());
                    if (_val > 0) {
                        _val = _val + 1;
                        $(this).siblings('.num').val(_val);
                    }
                    $(this).parents('.main').find('.dprice').html((dprice * _val).toFixed(2));
                    _this.price();
                    var id = $(this).parents('.main').attr('data-id');
                    $.ajax({
                        url: '/Api/v1/Mall/Cart',
                        type: 'patch',
                        data: {
                            CartId: id,
                            Quantity: _val
                        }
                    }).done(function (rs) {
                        // if (rs.returnCode != '200') {
                        //     $.oppo(rs.msg, 1)
                        // }
                    })
                });
                $('#main').on('click', '.reduce', function () {
                    var dprice = $(this).parents('.zl').attr('data-price');
                    var _val = Number($(this).siblings('.num').val());
                    if (_val > 1) {
                        _val = _val - 1;
                        $(this).siblings('.num').val(_val);
                    }
                    else {
                        _val = 1;
                        $(this).siblings('.num').val(_val);
                    }
                    $(this).parents('.main').find('.dprice').html((dprice * _val).toFixed(2));
                    _this.price();
                    var id = $(this).parents('.main').attr('data-id');
                    $.ajax({
                        url: '/Api/v1/Mall/Cart',
                        type: 'patch',
                        data: {
                            CartId: id,
                            Quantity: _val
                        }
                    }).done(function (rs) {
                        // if (rs.returnCode != '200') {
                        //     $.oppo(rs.msg, 1)
                        // }
                    })
                });
                $('#main').on('change', '.num', function () {
                    var dprice = $(this).parents('.zl').attr('data-price');
                    var _val = Number($(this).val());
                    var id = $(this).parents('.main').attr('data-id');
                    $(this).parents('.main').find('.dprice').html(dprice * _val.toFixed(2));
                    _this.price();
                    $.ajax({
                        url: '/Api/v1/Mall/Cart',
                        type: 'patch',
                        data: {
                            CartId: id,
                            Quantity: _val
                        }
                    }).done(function (rs) {
                        // if (rs.returnCode != '200') {
                        //     $.oppo(rs.msg, 1)
                        // }
                    })
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
                            _this.price();
                        }
                    });
                    if (sid.length) {
                        $.ajax({
                            url: '/Api/v1/Mall/Cart/DELETE',
                            type: 'DELETE',
                            data: {
                                cartIds: sid
                            }
                        }).done(function (rs) {
                            if (rs.returnCode == '200') {
                                $.oppo('删除成功', 1, function () {
                                    window.location.href = "/Html/shopcar1.html"
                                })
                            }
                        })
                    } else {
                        $.oppo('请选择删除的商品', 1);
                        return false;
                    }
                })
            },
            submit: function () {
                var _this = this;
                //提交
                $('.infoList').on('click', '.submit',function () {
                    var ids = {};   //购物车id
                    ids.CarIds = [];
                    $('.main').each(function () {
                        if ($(this).find('.quan').hasClass('active')) {
                            ids.CarIds.push($(this).attr('data-id'))
                        }
                    })
                    console.log(ids)
                    _this.link()
                })
            },
            link: function () {
                //确认订单链接
                var idstr = '';
                if ($('.quan.active').length == 0) {
                    $.oppo('请选择商品', 1)
                } else {
                    $('.quan.active').each(function (index) {
                        if (index != 0) {
                            idstr=idstr+'|'
                        }
                        idstr += $(this).parents('.main').attr('data-id')
                    })
                    window.location.href = "/Html/getmethodgwc.html?id=" + idstr+'&type='+2
                    // window.location.href = "/Html/settlement.html?id=" + idstr
                }
            }
        }
    })
})