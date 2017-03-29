$(function () {
    $.ADDLOAD();
    var data={
        KeyWord:decodeURIComponent(decodeURIComponent($.getUrlParam('key'))),
        SortType:'0',
        PageNo:'1',
        Limit:'5'
    }
    ajax();
    function ajax(callback,beforecallback) {
        $.ajax({
            url:URL+'/Api/v1/Mall/Goods',
            type:'post',
            data:data,
            beforeSend:function () {
                if(typeof beforecallback=='function'){beforecallback()}
            }
        }).done(function (rs) {
            if (rs.returnCode == '200'){
                window.allpage=Math.ceil(rs.data.TotalCount/data.Limit)
                rs.data.page=data.PageNo;
                view(rs.data);
                $('.loading').hide();
            }
        }).always(function () {
            if(typeof callback=='function'){callback()}
        })
    }

    var list;
    function view(rs) {
        if(rs.Goods.length == 0){
            $('.search-box').show();
            $('.nothing').show()
        }

        if(list){
            if (rs.page==1){
                list.Goods = rs.Goods;
            }else{
                list.Goods=list.Goods.concat(rs.Goods)
            }
        }else{
            list = new Vue({
                el:'#pro-list',
                data:rs,
                ready:function () {
                    //$('.loading').hide();
                    js();
                    $.RMLOAD();
                }
            })
        }
    }
    //下滑加载
    var flag = true;
    $(window).scroll(function () {
        var H = $('body,html').height();
        var h = $(window).height();
        var t = $('body').scrollTop();
        if (t >= H - h * 1.1 && flag == true) {
            flag = false;
            data.PageNo++;
            var nSortType = $('.typecur').attr('data-nSortType')

            if(data.PageNo>allpage){
                $('.loading').hide();

            }else{
                data['SortType']=nSortType
                ajax(function () {
                    setTimeout(function () {
                        flag = true;
                    }, 500)
                },function () {
                    $('.loading').show();
                })
            }
        }
    })

    //效果js
    function js() {
        //排序
        $('.sort').on('click',function () {
            $('.sort-mask').show();
            $('.screen-mask').hide();
        })
        $('.sort-mask li').on('click',function () {
            $.ADDLOAD();
            flag = true;
            $(this).addClass('typecur').siblings().removeClass('typecur');
            var nSortType=$(this).attr('data-nSortType');
            data['SortType']=nSortType;
            data['PageNo']=1;
            ajax();
            $('.products-nav .num').removeClass('typecur')
            $('.sort').addClass('cur').siblings().removeClass('cur');
            $('.sort-mask').hide();
        })
        $('.sort-mask').on('click',function () {
            $(this).hide();
        })
        //销量优先
        $('.products-nav .num').on('click',function () {
            $.ADDLOAD();
            flag = true;
            $(this).addClass('typecur').siblings().removeClass('cur');
            $('.sort-box li').removeClass('typecur');
            var nSortType=$(this).attr('data-nSortType');
            data['SortType']=nSortType;
            data['PageNo']=1;
            ajax();
            $('.sort-mask').hide();
            $('.screen-mask').hide();
        })

    }

    //获取商品标签
    tagajax();
    function tagajax() {
        $.ajax({
            url:URL+'/Api/v1/Goods/Tags/BntWeb-Mall',
            type:'get'
        }).done(function (rs) {
            if (rs.returnCode == '200'){
                tagview(rs);
            }else{
                $.oppo(rs.msg ,1)
            }
        })
    }
    function tagview(rs) {
        new Vue({
            el:'#search_box',
            data:rs,
            ready:function () {
                //$.RMLOAD();
                searchjs();
                searchdelete();
                link();
            }
        })
    }
    function searchjs() {
        //搜索
        $('.search-tit').on('click',function () {
            flag = true;
            $('.search-box').show();
            $('.sort-mask').hide();
            $('.screen-mask').hide();
        })
        $('.search-box .close').on('click',function () {
            window.history.back(-1);
        })
    }
    function searchdelete() {
        $('.search-box .text').on('keyup',function () {
            if($(this).val()==''){
                $('.search-box .delete').hide();
            }else{
                $('.search-box .delete').show();
            }
        })
        $('.search-box .delete').on('click',function () {
            $('.search-box .text').val('');
            $(this).hide();
        })
    }
    //跳转搜索页
    function link() {
        $('.search-box .btn').on('click',function () {
            var val = $('.search-box .text').val();
            if(val == ''){
                $.oppo('请输入关键字',1)
            }else{
                window.location.href="/Html/html/fenlei/searchlist.html?key="+encodeURIComponent(encodeURIComponent(val))
            }

        })
        $('.search-box .con li').on('click',function () {
            var val = $(this).attr('data-tag')
            window.location.href="/Html/html/fenlei/searchlist.html?key="+encodeURIComponent(encodeURIComponent(val))
        })
    }
})