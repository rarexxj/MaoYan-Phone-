$(function () {
    $.ADDLOAD();
    $.checkuser();
    new Vue({
        el:'#main',
        data:{
            info:[]
        },
        ready:function () {
            var _this=this;
            _this.ajax();
        },
        methods:{
            ajax:function () {
                var _this=this;
                $.ajax({
                    url:'/Api/v1/Mall/ConfirmExchangeGoodsList',
                    dataType:'json',
                    type:'GET',
                    data:{},
                    success:function (rs) {
                        if(rs.returnCode=='200'){
                            _this.info=rs.data;
                            $.RMLOAD();
                        }
                    }
                })
            }
        }
    })
})