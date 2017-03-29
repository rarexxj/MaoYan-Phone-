$(function () {
    $.ADDLOAD();
    $.checkuser();
    new Vue({
        el: '#main',
        data: {
            info: [],
            ajaxdata: {
                pageNo: 1,
                limit: 5,
                walletType: 2
            },
            bdinfo:[]
        },
        ready: function () {
            var _this = this;
            _this.infoajax();
            _this.$nextTick(function () {
                _this.bill();
                _this.updownload();
            })
        },
        methods: {
            infoajax: function () {
                //积分渲染
                var _this = this;
                $.ajax({
                    url: URL+'/Api/v1/Wallet/Integral',
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode = '200') {
                        _this.info = rs.data;

                    }
                })
            },
            bill: function () {
                var _this=this;
                //资金变动记录渲染
                $.ajax({
                    url: URL+'/Api/v1/WalletBill',
                    type: 'get',
                    data: _this.ajaxdata
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        if (_this.ajaxdata.pageNo == 1) {
                            _this.bdinfo = rs.data.Bills;
                        } else {
                            _this.bdinfo = _this.bdinfo.concat(rs.data.Bills);
                        }
                        window.allpage = Math.ceil(rs.data.TotalCount / _this.ajaxdata.limit);
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
                        _this.ajaxdata.pageNo++;
                        if ( _this.ajaxdata.pageNo > allpage) {
                            $.RMLOAD();
                        } else {
                            setTimeout(function () {
                                flag = true;
                            }, 500)
                            _this.bill();
                            $.ADDLOAD();
                        }
                    }
                })
            }
        }
    })
})
