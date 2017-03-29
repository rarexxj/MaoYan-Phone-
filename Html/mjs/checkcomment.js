$(function () {
    $.ADDLOAD();
    $.checkuser();
    var id  = $.getUrlParam('id');
    console.log(id)
    ajax();
    function ajax() {
        $.ajax({
            url:URL+'/Api/v1/Order/'+id+'/Evaluate',
            dataType:'json',
            type:'get',
            data:{
                orderId:id
            }
        }).done(function (rs) {
            if (rs.returnCode == '200'){
                view(rs)
            }
        })
    }
    function view(rs) {
        new Vue({
            el:'#che_com',
            data:rs,
            ready:function () {
                $.RMLOAD();
            }
        })
    }
})