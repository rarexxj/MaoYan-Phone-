$(function () {
    $.ADDLOAD();
    var id = $.getUrlParam('id');
    var data1 = {
        PageNo: '1',
        Limit: '10',
        CategoryId: id,
        GoodsType: '2',
        SortType:'0'
    }
    new Vue({
        el: '#main',
        data: {
            info: []
        },
        ready: function () {
            var _this = this;
            _this.zongheajax();
            _this.$nextTick(function () {
                _this.zonghpx();
                _this.xiaolpx();
                _this.shaixox();
                _this.updownload();
            })
        },
        methods: {
            zongheajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Mall/Goods',
                    type: 'post',
                    data: data1
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        window.allpage = (rs.data.TotalCount) / (data1.Limit);
                        if (data1.PageNo == 1) {
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
                    $('.chioce').hide();
                })
                $('#main').on('click', '.sort-mask li', function () {
                    // flag = true;
                    $('.sort').addClass('cur');
                    $('.num').removeClass('typecur');
                    $('.screen').removeClass('typecur');
                    $(this).addClass('typecur').siblings().removeClass('typecur');
                    var nSortType = $(this).attr('data-type');
                    data1.SortType = nSortType;
                    data1.PageNo = 1;
                    _this.zongheajax();
                    console.log(data1.SortType)
                })
                $('.sort-mask').on('click', function () {
                    $(this).hide();
                    $('.screen-mask').hide();
                    $('.chioce').hide();
                })
            },
            xiaolpx: function () {
                //销量排序
                var _this = this;
                $('#main').on('click', '.num', function () {
                    $(this).addClass('typecur').siblings('.chobtn').removeClass('cur');
                    $('.sort-mask li').removeClass('typecur');
                    $('.screen').removeClass('typecur');
                    $('.sort-mask').hide();
                    $('.chioce').hide();
                    $('.screen-mask').hide();
                    data1.SortType = 3;
                    data1.PageNo = 1;
                    _this.zongheajax();
                    console.log(data1.SortType)
                })
            },

            //筛选
            shaixox: function () {
                var _this = this;
                $('.screen-mask').on('click', function () {
                    $(this).hide();
                    $('.chioce').hide();
                    $('.sort-mask').hide();
                })
                $('#main').on('click', '.screen', function () {
                    $(this).addClass('typecur');
                    $('.num').removeClass('typecur');
                    $('.sort').removeClass('cur');
                    $('.sort-box li').removeClass('typecur');
                    $('.screen-mask').show();
                    $('.chioce').show();
                })
                $('#main').on('click', '.chiocebtn', function () {
                    var cha = $('.text1').val() - $('.text2').val();
                    if ($('.text1').val() == '' && $('.text2').val() == '') {
                        $.oppo('输入的价格不能为空', 1);
                        return false
                    }
                    if (cha > 0) {
                        $.oppo('输入的价格区间有误', 1);
                        return false
                    }
                    data1.MinPrice = $('.text1').val();
                    data1.MaxPrice = $('.text2').val();
                    data1.SortType = '';
                    data1.PageNo = 1;
                    $('.screen-mask').hide();
                    $('.chioce').hide();
                    _this.zongheajax();
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
                        data1.PageNo++;
                        data1.SortType = $('.typecur').attr('data-type');
                        if (data1.PageNo > Math.ceil(allpage)) {
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
            }
        }
    })
})