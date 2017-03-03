$(function () {
    $.ADDLOAD();
    $.checkuser();
    var id = $.getUrlParam('id');
    var sex;
    new Vue({
        el: '#myinfo',
        data: {
            info: [],
            btn: true
        },
        ready: function () {
            var _this = this;
            _this.infoajax();
            _this.$nextTick(function () {
                _this.Portrait();
                _this.exit();
                _this.hideexit();
            })
        },
        methods: {
            infoajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Member/GetMemberInfo',
                    type: 'get'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data;
                        if (rs.data.Birthday.toString().indexOf('1949') > -1) {
                            _this.btn = false;
                        }else{
                            $('.xingm').attr('href','javascript:;')
                        }
                        $.RMLOAD()

                    }
                })
            },
            //上传头像
            Portrait: function () {
                $('.head .file').on('change', function () {
                    $.ADDLOAD();
                    var imgData = {};
                    var file = $(this)[0].files[0];
                    //判断类型是不是图片
                    if (!/image\/\w+/.test(file.type)) {
                        //toolTips(0, "请确保文件为图像类型", 1);
                        $.oppo('请确保文件为图像类型', 1)
                        return false;
                    }
                    var reader = new FileReader();
                    reader.readAsDataURL(file);
                    reader.onload = function (e) {
                        imgData['fileName'] = file.name;
                        imgData['data'] = this.result; //就是base64
                        //console.log(imgData);
                        $.ajax({
                            //contentType: false,    //不可缺
                            //processData: false,    //不可缺
                            url: '/Api/v1/Member/' + $.get_user('Id') + '/Avatar',
                            data: imgData,
                            type: 'Patch'
                        }).done(function (json) {
                            if (json.returnCode == 200) {
                                var data = json.data;
                                // localStorage['qy_head'] = $.get_user('Id') + "|" + data.SmallThumbnail
                                $('.head img').attr('src', data.SmallThumbnail);
                                $.oppo('修改成功！', 1)
                                $.RMLOAD();
                            } else {
                                $.oppo(json.msg, 1);
                                $.RMLOAD();
                            }
                        })
                    }
                })
            },
            exit: function () {
                var _this = this;
                $('.submit').on('click', function () {
                    _this.exitajax();
                })

            },
            exitajax: function () {
                $.ajax({
                    url: '/Api/v1/Logout',
                    type: 'post'
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        localStorage.removeItem('qy_user');
                        localStorage.removeItem('qy_loginToken');
                        window.location.href = "/Html/login.html"
                    }
                })
            },
            hideexit: function () {
                if ($.is_weixin()) {
                    $('.submit').hide();
                }
            }
        }
    })
})