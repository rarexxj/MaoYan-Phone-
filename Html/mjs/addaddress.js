$(function () {
    $.ADDLOAD();
    var reg =/^1[3|4|5|7|8]\d{9}$/;
    $.checkuser();
    new Vue({
        el: '#main',
        data: {
            proinfo: [],
            cityinfo: [],
            xianinfo: [],
            streetinfo: []
        },
        ready: function () {
            var _this = this;
            _this.province(); //省
            _this.$nextTick(function () {
                _this.submit();
                $.RMLOAD();
            })
        },
        methods: {
            province: function () {
                //获得省
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Settings/District/0/Child',
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.proinfo = rs.data
                        _this.getcity();
                    }
                })
            },
            getcity: function () {
                var _this = this;
                $('#province').on('change', function () {
                    var id = $(this).find('option:selected').attr('data-id');
                    _this.city(id)
                })
            },
            city: function (id) {
                //得到市
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Settings/District/' + id + '/Child',
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.cityinfo = rs.data
                        _this.getxian();
                    }
                })
            },
            getxian: function () {
                var _this = this;
                $('#City').on('change', function () {
                    var id = $(this).find('option:selected').attr('data-id');
                    _this.xian(id);
                })
            },
            xian: function (id) {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Settings/District/' + id + '/Child',
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.xianinfo = rs.data;
                        _this.getstreet();
                    }
                })
            },
            getstreet: function () {
                var _this = this;
                $('#District').on('change', function () {
                    var id = $(this).find('option:selected').attr('data-id');
                    _this.street(id);
                })
            },
            street: function (id) {
                //得到街道
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Settings/District/' + id + '/Child',
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.streetinfo = rs.data
                    }
                })
            },
            submit: function () {
                var _this = this;
                $('.submit').on('click', function () {

                    if ($('.use').val() == '') {
                        $.oppo('收货人不能为空', 1)
                        return false;
                    } else if ($('.ph').val() == '') {
                        $.oppo('手机号不能为空', 1)
                        return false;
                    } else if(!reg.test($('.ph').val())){
                        $.oppo("手机号格式有误", 1);
                        return false;
                    }else if ($('.postcode').val() == '') {
                        $.oppo('邮政编码不能为空', 1)
                        return false;
                    } else if ($('#province').val() == '') {
                        $.oppo('省份不能为空', 1)
                        return false;
                    } else if ($('#City').val() == '') {
                        $.oppo('市区不能为空', 1)
                        return false;
                    } else if ($('#District').val() == '') {
                        $.oppo('地区不能为空', 1)
                        return false;
                    } else if ($('.textarea').val() == '') {
                        $.oppo('详细地址不能为空', 1)
                        return false;
                    } else {
                        var RegionName = $('#province').val() + ',' + $('#City').val() + ',' + $('#District').val() + ',' + $('#Street').val()
                        var data = {
                            Address: $('.textarea').val(),
                            Contacts: $('.use').val(),
                            Phone: $('.ph').val(),
                            Province: $('#province option:selected').attr('data-id'),
                            City: $('#City option:selected').attr('data-id'),
                            District: $('#District option:selected').attr('data-id'),
                            Street: $('#Street option:selected').attr('data-id'),
                            RegionName: RegionName,
                            Postcode: $('.postcode').val()
                        }
                        _this.subajax(data);
                    }
                })

            },
            subajax: function (data) {
                $.ajax({
                    url: '/Api/v1/Member/Address',
                    type: 'post',
                    data: data
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        $.oppo('保存成功', 1);
                        window.location.replace("/Html/my.html")
                    }
                })
            }
        }
    })
})
