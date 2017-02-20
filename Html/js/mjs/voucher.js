$(function () {
    // $.ADDLOAD()
    $.checkuser();
    var OrderNo=$.getUrlParam('OrderNo');
    var type = $.getUrlParam('type');
    var fkzt = $.getUrlParam('fkzt');
    var orderid = $.getUrlParam('orderid');
    var data1 = {}
    new Vue({
        el: '#main',
        data: {
            bankinfo: {},
            info: {},
            OrderNo:OrderNo
        },
        ready: function () {
            var _this = this;
            _this.bankajax();
            _this.click();
            _this.$nextTick(function () {
                $.RMLOAD()
            })
        },
        methods: {
            bankajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Config/Bank',
                    type: 'GET'
                }).done(function (rs) {
                    if (rs.returnCode == 200) {
                        _this.bankinfo = rs.data
                    }
                })
            },
            infoajax: function (data1) {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Proof/Upload',
                    type: 'POST',
                    data: data1
                }).done(function (rs) {
                    if (rs.returnCode == 200) {
                        if(type==1){
                            window.location.href = '/Html/costorder.html?orderType=0'
                        }
                        if(type==2){
                            window.location.href = '/Html/costorder.html?orderType=0'
                        }

                    }
                })
            },
            click: function () {
                var _this = this;
                $('#main').on('click', '.btn',function () {
                    data1 = {
                        OrderId: orderid,
                        PayName: $('.name').val(),
                        PayFee: $('.price').val(),
                        BankName: $('.bank').val(),
                        Account: $('.num').val(),
                        Note: $('.liuyan').val(),
                        ProofType: 0, //凭证类型（0定金1余款）
                    }
                    if (type == 2) {
                        data1.ProofType = 1
                    }
                    if (type == 1) {
                        if (fkzt == 2) {
                            data1.ProofType = 1    //余款
                        } else {
                            data1.ProofType = 0 //定金
                        }
                    }
                    _this.infoajax(data1);
                })
            }
        }
    })
})