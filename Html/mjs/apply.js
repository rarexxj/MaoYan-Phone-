/**
 * Created by admin on 2016/9/8.
 */
$(function () {
    var oid = $.getUrlParam('oid');
    var gid = $.getUrlParam('gid');
    var mp = $.getUrlParam('mp');
    var RefundType = $.getUrlParam('RefundType');
    //退款类型
    if (RefundType==0){
        $('.type').html('仅退款')
    }else if(RefundType==1){
        $('.type').html('退货并退款')
    }else if(RefundType==1){
        $('.type').html('换货')
    }
    //默认退款价格
    $('.priceinput').val(mp);
    $('.priceinput').attr('data-max',mp)
    js();
    function js() {
        $('.textarea').on('keyup',function () {
            $('.line .cal span').html($(this).val().length)
        })
        $('.priceinput').on('keyup',function () {
            var money = parseFloat($(this).val());
            if(money > mp){
                $(this).val(mp)
            }
        })
    }
    //提交
    $('.submit').on('click',function () {
        if($(this).hasClass('gray')){
            return false
        }else{
            if($('.priceinput').val()==''){
                $.oppo('请输入退款金额',1)
            }else if($('.textarea').val()==''){
                $.oppo('请输入退款原因',1)
            }else{
                $('.submit').addClass('gray');
                var datas={
                    OrderId:oid,
                    SingleGoodsId:gid,
                    RefundType:RefundType,
                    RefundAmount:parseFloat($('.priceinput').val()),
                    Reason:$('.textarea').val()
                }
                ajax(datas)
            }
        }
    })
    function ajax(datas) {
        $.ajax({
            url:'/Api/v1/Order/Refund',
            type:'post',
            data:datas
        }).done(function (rs) {
            if (rs.returnCode == '200'){
                $.oppo('提交成功',1,function () {
                    window.location.replace("/Html/html/AfterSales/Info.html?oid="+oid+"&gid="+gid)

                })
            }
        }).always(function () {
            $('.submit').removeClass('gray');
        })
    }
})