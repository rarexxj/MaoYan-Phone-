$(function () {
    $.checkuser();
    var aid = $.getUrlParam('aid');
    var oid = $.getUrlParam('oid');
    var gid = $.getUrlParam('gid');
    var money = $.getUrlParam('money');
    var RefundType = $.getUrlParam('RefundType');
    //退款类型
    if (RefundType==0){
        $('.type').html('仅退款')
    }else if(RefundType==1){
        $('.type').html('退货并退款')
    }else if(RefundType==2){
        $('.type').html('换货')
    }
    //默认退款价格
    $('.priceinput').val(Number(money).toFixed(2));
    // $('.priceinput').attr('data-max',mp)
    js();
    function js() {
        $('.textarea').on('keyup',function () {
            $('.line .cal span').html($(this).val().length)
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
                    Id:aid,
                    RefundType:RefundType,
                    RefundAmount:money,
                    Reason:$('.textarea').val()
                }
                ajax(datas)
            }
        }
    })
    function ajax(datas) {
        $.ajax({
            url:URL+'/Api/v1/Order/Refund',
            type:'patch',
            data:datas
        }).done(function (rs) {
            if (rs.returnCode == '200'){
                $.oppo('提交成功',1,function () {
                    window.location.replace("/Html/html/personalcenter/tuikinfo.html?oid="+oid)
                })
            }
        }).always(function () {
            $('.submit').removeClass('gray');
        })
    }
})