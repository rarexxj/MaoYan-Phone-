$(function () {
    $.checkuser();
    var id = $.getUrlParam('id'); //购物车id
    var gid = $.getUrlParam('gid');
    var hgid=$.getUrlParam('hgid');//积分换购id
    $.ADDLOAD();

    new Vue({
        el: '#address',
        data: {
            info: {}
        },
        ready: function () {
            var _this = this;
            _this.infoajax();
            _this.$nextTick(function () {
                _this.link();

            })
        },
        methods: {
            infoajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Member/Address',
                    dataType:'json',
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data;
                        $.RMLOAD()
                    }
                })
            },
            link: function () {
                $('#address').on('click', '.addlist a', function () {
                    var addid = $(this).parents('.addlist').attr('data-addid');
                    if (id) {
                        window.location.href = '/Html/html/shopcar/settlement.html?id=' + id + '&addid=' + addid
                    } else if(gid){
                        window.location.href = '/Html/html/shopcar/settlement.html?gid=' + gid + '&addid=' + addid
                    }else if(hgid){
                        window.location.href = '/Html/html/personalcenter/jifensettlement.html?hgid=' + hgid + '&addid=' + addid
                    }
                });
                $('.submit').on('click', function () {
                    if (id) {
                        window.location.href = '/Html/html/shopcar/chooseaddaddress.html?id=' + id
                    } else if(gid){
                        window.location.href = '/Html/html/shopcar/chooseaddaddress.html?gid=' + gid
                    }else if(hgid){
                        window.location.href = '/Html/html/shopcar/chooseaddaddress.html?hgid=' + hgid
                    }
                })
            }
        }
    })
})