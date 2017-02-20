$(function () {
    $.ADDLOAD();
    var qy_user1 = $.get_user();
    var id=$.getUrlParam('id');
    $.checkuser();
    new Vue({
        el: '#main',
        data: {
            info: []
        },
        ready: function () {
            var _this = this;
            _this.nsubmit();
            _this.$nextTick(function () {
                $.RMLOAD();
            })
        },
        methods: {
            infoajax: function (data) {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Member/' + $.get_user('Id'),
                    type: 'put',
                    data: data
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        $.oppo('修改成功', 1, function () {
                            var  qy_NickName = encodeURIComponent(encodeURIComponent(rs.data.NickName));
                            qy_user1.NickName=qy_NickName;
                            // qy_user = JSON.stringify(qy_user1);
                            $.put_user(qy_user1)
                            localStorage.setItem('qy_NickName', qy_NickName);
                            if(id){
                                if(localStorage.qy_idcard){
                                    window.location.replace('/Html/prodetails.html?id='+id);
                                }else{
                                    window.location.replace("/Html/myinfo.html?id="+id);
                                }

                            }else{
                                window.location.replace("/Html/myinfo.html");
                            }
                        });
                        _this.info = rs.data;
                    }
                }).always(function () {
                    $('.submit').removeClass('gray')
                })
            },
            nsubmit: function () {
                var _this = this;
                $('.submit').on('click', function () {
                    var data = {
                        NickName: $('.text').val(),
                        Birthday: '',
                        Sex: '0',
                        IDCard:''
                    };
                    if ($(this).hasClass('gray')) {
                        return false;
                    } else {
                        $(this).addClass('gray')
                        _this.infoajax(data);
                    }
                })
            }
        }
    })
})

