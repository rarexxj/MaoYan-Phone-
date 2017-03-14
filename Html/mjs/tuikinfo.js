$(function () {
    $.ADDLOAD();
    $.checkuser();
    var oid = $.getUrlParam('oid');
    var gid = $.getUrlParam('gid');
    ajax();

    function ajax() {
        $.ajax({
            url:'/Api/v1/Order/Refund',
            type:'get',
            data:{
                orderId:oid,
                singleGoodsId:gid
            }
        }).done(function (rs) {
            if (rs.returnCode == '200'){
                view(rs.data);
            }
        })
    }
    function view(rs) {
        //金额
        if(rs.Datail.RefundAmount.toString().indexOf('.')>-1){
            if(rs.Datail.RefundAmount.toString().split('.')[1].length == 1){
                rs.Datail.prices2=rs.Datail.RefundAmount.toString().split('.')[1]+'0'
            }else{
                rs.Datail.prices2=rs.Datail.RefundAmount.toString().split('.')[1];
            }
            rs.Datail.prices1=rs.Datail.RefundAmount.toString().split('.')[0];

        }else{
            rs.Datail.prices1=rs.Datail.RefundAmount;
            rs.Datail.prices2='00';
        }
        //申请id
        //商品金额
        if(rs.Goods.Price.toString().indexOf('.')>-1){
            if(rs.Goods.Price.toString().split('.')[1].length == 1){
                rs.Goods.prices2=rs.Goods.Price.toString().split('.')[1]+'0'
            }else{
                rs.Goods.prices2=rs.Goods.Price.toString().split('.')[1];
            }
            rs.Goods.prices1=rs.Goods.Price.toString().split('.')[0];

        }else{
            rs.Goods.prices1=rs.Goods.Price;
            rs.Goods.prices2='00';
        }
        //商品属性
        rs.Goods.shuxi=[]
        for (i in rs.Goods.GoodsAttribute.split(',')){
            rs.Goods.shuxi[i]=rs.Goods.GoodsAttribute.split(',')[i]
        }
        new Vue({
            el:'#as_info',
            data:rs,
            ready:function () {
                $.RMLOAD();
                cancelapply(rs);
                js();
            }
        })
    }
    function js() {
        $('.pop .no').on('click',function () {
            $('.pop').hide();
        })
    }
    //撤销申请
    function cancelapply(rs) {
        $('.can-btns').on('click',function () {
            $('.pop-can').show();
        })
        $('.can-btn').on('click',function () {

            $.ajax({
                url:'/Api/v1/Order/Refund/'+rs.Datail.Id,
                type:'delete',
                data:{
                    applyId:rs.Id
                }
            }).done(function (rs) {
                if (rs.returnCode == '200'){
                    $('.pop-can').hide();
                    $.oppo('退款撤销成功成功',1,function () {
                        window.location.replace("/Html/Order/MyOrder.html?orderType=0")
                    })
                }
            })
        })
    }
})