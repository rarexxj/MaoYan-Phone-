$(function () {
    $.checkuser();
    new Vue({
        el: '#main',
        data: {
        },
        methods: {
            ajax: function () {
                $.ajax({
                    url: URL+'/Api/v1/Member/' +$.get_user('Id') +'/Update',
                    dataType:'json',
                    type: 'put',
                    data: {
                        NickName: '',
                        Birthday: $('.date').val(),
                        Sex: '',
                        TastesType: ''
                    }
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        $.oppo('修改成功', 1, function () {
                            window.location.replace("/Html/html/personalcenter/myinfo.html");
                        })
                    }
                })
            }

        }
    })
})