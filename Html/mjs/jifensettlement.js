$(function () {
    $.ADDLOAD()
    $.checkuser();
    var addid = $.getUrlParam('addid');
    var hgid = $.getUrlParam('hgid'); //积分换购商品id
    var ids = {}
    if (addid) {
        ids.AddressId = addid
    }else{
        ids.AddressId =''
    }
    ids.GoodsId=hgid;
    new Vue({
        el: '#main',
        data: {
            info: [],
            data1: ids,
            yhqinfo: []
        },
        ready: function () {
            var _this = this;
            _this.ajax();
            _this.$nextTick(function () {
                _this.link();
                _this.getsettime();
                _this.prosubmit();
                // $.RMLOAD();
            })
        },
        methods: {
            ajax: function () {
                var _this = this;
                $.ajax({
                    url: "/Api/v1/Mall/IntegralOrderCalculation",
                    data: ids,
                    type: 'post',
                    dataType: 'json'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data;
                        $.RMLOAD();
                    }
                })
            },
            link: function () {
                //选择地址
                if (hgid) {
                    $('#main').on('click', '.choadd .a', function () {
                        window.location.href = "/Html/html/shopcar/chooseaddress.html?hgid=" + hgid
                    })
                }
            },
            getsettime: function () {
                // $('.settime').on('click', function () {
                //     $('.confirm-order').hide();
                //     $('.timebox').show();
                // })
                // $('.time-btn').on('click', function () {
                //     $('.confirm-order').show();
                //     $('.timebox').hide();
                // })

                var currYear = (new Date()).getFullYear();
                var opt = {};
                opt.date = {preset: 'date'};
                opt.datetime = {preset: 'datetime'};
                opt.time = {preset: 'time'};
                opt.default = {
                    theme: 'android-ics light', //皮肤样式
                    display: 'modal', //显示方式
                    mode: 'scroller', //日期选择模式
                    dateFormat: 'yyyy-mm-dd',
                    lang: 'zh',
                    showNow: true,
                    nowText: "今天",
                    // startYear: currYear, //开始年份
                    endYear: currYear + 10, //结束年份
                    minDate: new Date(),
                    stepMinute: 15
                };
                var optDateTime = $.extend(opt['datetime'], opt['default']);
                $("#peistime").mobiscroll(optDateTime).datetime(optDateTime);

            },
            //提交订单
            prosubmit: function () {
                var _this = this;
                console.log($('#peistime').val())
                $('.balance').on('click', function () {
                    if ($('.addadd').length) {
                        $.oppo('请选择收货地址', 1);
                    } else {
                        var message = {
                            AddressId: _this.info.DefaultAddress.Id,
                            GoodId: _this.info.Exchangegood[0].Id,
                            Integral: _this.info.Exchangegood[0].ExchangeIntegral,
                            BestTime: $('.peit').val(),
                            Memo: $('.bz').val()
                        }
                        _this.inputajax(message);
                    }
                })
            },
            inputajax: function (message) {
                $.ajax({
                    url: '/Api/v1/Mall/ConfirmExchangeSubmit',
                    data: message,
                    type: 'post'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        if(rs.data.PayFee==0){
                            window.location.href="/Html/html/personalcenter/personalcenter.html"
                        }else{
                            window.location.replace("/Html/html/shopcar/pay.html?id=" + rs.data.Id + '&OrderNo=' + rs.data.OrderNo + '&money=' + rs.data.PayFee + '&time=' + rs.data.CreateTime)
                        }
                    }
                })
            }
        }
    })
})



