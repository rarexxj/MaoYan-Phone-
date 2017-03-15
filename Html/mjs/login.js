$(function () {
    var openid = $.getUrlParam('openId');
    var _zh = $('#zh').val();
    var _mm = $('#mm').val();
    //点击登录
    $('.login-btn').on('click', function () {
        _zh = $('#zh').val();
        _mm = $('#mm').val();
        if (_zh == '') {
            $.oppo('请输入手机号', 1);
            return false;
        }
        if (_mm == '') {
            $.oppo('请输入密码', 1)
            return false;
        }

        ajax();
    })


    function ajax() {
        $.ajax({
            url: '/Api/v1/Login',
            type: 'post',
            dataType:'json',
            data: {
                PhoneNumber: _zh,
                Password: _mm,
                MobileDevice: ''
            }
        }).done(function (rs) {
            if (rs.returnCode == '200') {
                $.put_user(rs.data);
                localStorage.setItem('qy_loginToken', $.get_user('PhoneNumber') + ':' + $.get_user('DynamicToken'));

                // if (rs.data.Avatar != null) {
                //     localStorage['qy_head'] = rs.data.Id + '|' + rs.data.Avatar.SmallThumbnail;
                // }
                $.oppo('登录成功', 1, function () {
                    window.location.href = "/index.html"
                })
            }
        })
    }

})