$(function () {
    $.ADDLOAD();
    var qy_user1 = $.get_user()
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
                            var qy_IDCard = rs.data.IDCard;
                            var qy_user;
                            qy_user1.IDCard = qy_IDCard;
                            qy_user = JSON.stringify(qy_user1);
                           $.put_user(qy_user1)
                            //alert(JSON.stringify($.get_user()));
                            localStorage.setItem('qy_idcard', qy_IDCard);
                            if(id){
                                if(localStorage.qy_NickName){
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
                    // 身份证号码为15位或者18位，15位时全为数字，18位前17位为数字，最后一位是校验位，可能为数字或字符X
                    var reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
                    var card = $('.text').val();
                    if (reg.test(card) === false) {
                        $.oppo('身份证输入不合法',1);
                        return false;
                    };
                    var data = {
                        NickName: '',
                        Birthday: '',
                        Sex: '0',
                        IDCard: $('.text').val()
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
