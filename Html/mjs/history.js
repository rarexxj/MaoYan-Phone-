/**
 * Created by admin on 2016/8/27.
 */
$(function () {
    $.ADDLOAD();

    ajax();
    function ajax() {
        $.ajax({
            url:'/Api/v1/Mall/Browse',
            type:'get',
        }).done(function (rs) {
            if (rs.returnCode = '200'){
                view(rs.data);
            }else{
                if(rs.returnCode == '401'){
                    Backlog();
                }else{
                    oppo(rs.msg ,1)
                }
            }
        })
    }
    function view(rs) {
        if(rs.Goods.length == 0){
            $('.search-box').show();
            $('.nothing').show();
        }
        //转换价格
        for(var i in rs.Goods) {
            if (rs.Goods[i].ShopPrice.toString().indexOf('.') > -1) {
                if (rs.Goods[i].ShopPrice.toString().split('.')[1].length == 1) {
                    rs.Goods[i].prices2 = rs.Goods[i].ShopPrice.toString().split('.')[1] + '0'
                } else {
                    rs.Goods[i].prices2 = rs.Goods[i].ShopPrice.toString().split('.')[1]
                }
                rs.Goods[i].prices1 = rs.Goods[i].ShopPrice.toString().split('.')[0]

            } else {
                rs.Goods[i].prices1 = rs.Goods[i].ShopPrice;
                rs.Goods[i].prices2 = '00'
            }
            //时间
            rs.Goods[i].time=rs.Goods[i].BrowseTime.split('T')[0];
        }
        new Vue({
            el:'#history',
            data:rs,
            ready:function () {
                $.RMLOAD();
            }
        })
    }
    
    //删除浏览记录
    $('.history .delete').on('click',function () {
        deleteajax();
    })

    function deleteajax() {
        $.ajax({
            url:"/Api/v1/Mall/Browse",
            type:'delete'
        }).done(function (rs) {
            if (rs.returnCode = '200'){
                oppo('成功清理浏览记录',1,function () {
                    window.location.href="/Html/Member/PersonalCenter.html"
                })
            }
        })
    }
})