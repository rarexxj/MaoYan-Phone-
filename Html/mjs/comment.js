$(function () {
    $.ADDLOAD();
    $.checkuser();
    var id  = $.getUrlParam('id');
    ajaxinfo();
    $('.submit').on('click',function () {
        var data1=[]
        $('.box').each(function () {
            var data2={}
            if ($(this).find('.pf li.cur').length == '0'){
                $.oppo('请评分',1);
            }else if($(this).find('.textarea').val() == ''){
                $.oppo('请填写评价',1);
            }else{
                data2={
                    // orderId:id,
                    SingleGoodsId:$(this).attr('data-id'),
                    Score:$(this).find('.pf li.cur').length,
                    Content:$(this).find('.textarea').val(),
                    IsAnonymity:$(this).find('.weui_switch').is(':checked')
                }
                data1.push(data2)
            }
        })
        if(data1.length){

            ajax(data1);
        }
        //alert(1)
    })


    function ajax(data1) {
        console.log(data1)
        var datas={
            // 'orderId':id,
            'evaluates':data1
        }
        $.ajax({
            url:'/Api/v1/Order/'+id+'/Evaluate',
            data:datas,
            type:"post"
        }).done(function (rs) {
            if (rs.returnCode == '200'){
                $.oppo('提交成功' ,1,function () {
                    window.location.replace("/Html/Order/MyOrder.html?orderType=0")
                })
            }
        })
    }
    function ajaxinfo() {
        $.ajax({
            url:'/Api/v1/Mall/Order/'+id,
            tpye:'get',
            data:{
                id:id
            }
        }).done(function (rs) {
            if (rs.returnCode == '200'){
                view(rs.data);
            }
        })
    }
    function view(rs) {
        //转换价格
        new Vue({
            el:'#comment',
            data:rs,
            ready:function () {
                $.RMLOAD();
                fen();
            }
        })
    }
    function fen() {
        //评分
        $('.line li').on('click',function () {
            var n = $(this).index()+1;
            for (var i=0;i<n;i++){
                $(this).parents('.line').find('li').eq(i).addClass('cur');
            }
            for (var j=n;j<$('.line li').length;j++){
                $(this).parents('.line').find('li').eq(j).removeClass('cur');
            }
        })
        $('.textarea').on('keyup',function () {
            $('.line .cal span').html($(this).val().length)
        })
    }
})