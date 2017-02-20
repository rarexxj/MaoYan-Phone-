$(function () {
    $.ADDLOAD()
    if ($.is_weixin()) {
        var user = $.cookie('userInfo1')
        if (user) {
            user = JSON.parse(window.base64decodes(user))
            $.put_user(user)
            localStorage.setItem('qy_loginTokenyy', user.PhoneNumber + ':' + user.DynamicToken);
            //新增
            $.removeCookie('userInfo1');
        }

    }
    window.TOKEN = localStorage.getItem('qy_loginTokenyy')
    if (window.TOKEN) {
        $.ajaxSetup({
            headers: {
                Authorization: 'Basic ' + window.base64encode(window.TOKEN)
            }
        })
    } else {
        window.location.href = '/Html/innerlogin.html'
    }
    var beginTime = $.getUrlParam('beginTime');
    var endTime = $.getUrlParam('endTime');
    var month = $.getUrlParam('month')
    var id = $.getUrlParam('id');
    var ispay = $.getUrlParam('isPay');
    if (beginTime) {
        beginTime = beginTime
    } else {
        beginTime = ''
    }
    if (endTime) {
        endTime = endTime
    } else {
        endTime = ''
    }
    if (month) {
        month = month
    } else {
        month = ''
    }
    new Vue({
        el: '#my',
        data: {
            info: []
        },
        ready: function () {
            var _this = this;
            _this.infoajax();
            setTimeout(function () {
                _this.tabclick();
            },100)
            _this.$nextTick(function () {
                $.RMLOAD();
            })
        },
        methods: {
            infoajax: function () {
                var _this = this;
                var data = {
                    memberId: id,
                    isPay: ispay,
                    beginTime: beginTime,
                    endTime: endTime,
                    type: 2,
                    month: month
                }
                $.ajax({
                    url: '/Api/v1/Mall/MemberFeeDetail',
                    type: 'GET',
                    data: data
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data;
                    }
                })
            },
            tabclick: function () {
                var _this = this;
                $('.tab').on('click', function () {
                    var typeid = $(this).attr('data-id');
                    console.log(typeid)
                    $(this).children().addClass('active');
                    $(this).siblings().children().removeClass('active');
                    var data = {
                        memberId: id,
                        isPay: ispay,
                        beginTime: beginTime,
                        endTime: endTime,
                        type: typeid,
                        month: month
                    }
                    $.ajax({
                        url: '/Api/v1/Mall/MemberFeeDetail',
                        type: 'GET',
                        data: data
                    }).done(function (rs) {
                        if (rs.returnCode == '200') {
                            _this.info = rs.data
                        }
                    })
                })
            }
        }
    })
})