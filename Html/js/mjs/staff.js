$(function () {
    var data1 = {
        pageNo: '1',
        limit: '10',
        key: $('.top-bg').val()
    };
    new Vue({
        el: '#staff',
        data: {
            info: []
        },
        ready: function () {
            var _this = this;
            _this.searchclick();
            _this.$nextTick(function () {
                // _this.scrollload();
            })
        },
        methods: {
            infoajax: function () {
                var _this = this;
                data1.key = $('.top-bg').val();
                $.ajax({
                    url: '/Api/v1/GetSalesPerson',
                    type: 'get',
                    data: data1
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        if(rs.data.TotalCount==0){
                            $.oppo('无该营业员',4)
                        }
                        window.allpage = (rs.data.TotalCount) / (data1.limit);
                        _this.info = _this.info.concat(rs.data.SalesPerson);
                    }
                })
            },
            searchclick: function () {
                var _this = this;
                $('#staff').on('change','.top-bg', function () {
                    _this.infoajax();
                })
            },
            scrollload: function () {
                //下滑加载
                var _this = this;
                var flag = true;
                $(window).scroll(function () {
                    var H = $('.scrolljs')[0].getBoundingClientRect().top;
                    var h = $(window).height();
                    if (H - h < 0 && flag == true)  {
                        flag = false;
                        data1.pageNo++;
                        if (data1.pageNo > Math.ceil(allpage)) {
                            $.RMLOAD();
                        } else {
                            setTimeout(function () {
                                flag = true;
                            }, 500)
                            _this.infoajax();
                            $.ADDLOAD();
                        }
                    }
                })
            }
        }
    })
})