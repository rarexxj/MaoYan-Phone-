$(function () {
    $.ADDLOAD();
    $.checkuser();
    var oid = $.getUrlParam('oid');
    ajax();

    function ajax() {
        $.ajax({
            url:URL+'/Api/v1/Order/Refund',
            type:'get',
            data:{
                orderId:oid
            }
        }).done(function (rs) {
            if (rs.returnCode == '200'){
                view(rs.data);
            }
        })
    }
    function view(rs) {
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
                url:URL+'/Api/v1/Order/Refund/'+rs.Datail.Id,
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