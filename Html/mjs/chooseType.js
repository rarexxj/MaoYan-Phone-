$(function () {
    var oid=$.getUrlParam('oid');
    var filesid=$.getUrlParam('filesid');
    var mp = $.getUrlParam('mp');
    $('.box1').on('click',function () {
        window.location.href='/Html/html/personalcenter/apply.html?oid='+oid+'&filesid='+filesid+'&mp='+mp+'&RefundType='+0
    })
    $('.box2').on('click',function () {
        window.location.href='/Html/html/personalcenter/apply.html?oid='+oid+'&filesid='+filesid+'&mp='+mp+'&RefundType='+1
    })
    $('.box3').on('click',function () {
        window.location.href='/Html/html/personalcenter/apply.html?oid='+oid+'&filesid='+filesid+'&mp='+mp+'&RefundType='+2
    })
})
