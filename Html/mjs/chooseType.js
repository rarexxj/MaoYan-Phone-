$(function () {
    var oid=$.getUrlParam('oid');
    var gid=$.getUrlParam('gid');
    var mp = $.getUrlParam('mp');
    $('.box1').on('click',function () {
        window.location.href='/Html/html/personalcenter/apply.html?oid='+oid+'&gid='+gid+'&mp='+mp+'&RefundType='+0
    })
    $('.box2').on('click',function () {
        window.location.href='/Html/html/personalcenter/apply.html?oid='+oid+'&gid='+gid+'&mp='+mp+'&RefundType='+1
    })
    $('.box3').on('click',function () {
        window.location.href='/Html/html/personalcenter/apply.html?oid='+oid+'&gid='+gid+'&mp='+mp+'&RefundType='+2
    })
})
