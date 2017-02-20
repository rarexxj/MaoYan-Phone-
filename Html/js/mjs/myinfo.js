$(function () {
    $.ADDLOAD();
    $.checkuser();
    var id=$.getUrlParam('id');
    var sex;
    new Vue({
        el: '#myinfo',
        data: {
            info: []
        },
        ready: function () {
            var _this = this;
            _this.infoajax();
            _this.$nextTick(function () {
                _this.Portrait();
                _this.exit();
                _this.hideexit();
                _this.bitlink();
                if (localStorage['qy_head']) {
                    if ($.get_user('Id') == localStorage['qy_head'].toString().split('|')[0]) {
                        $('.head img').attr('src', localStorage['qy_head'].toString().split('|')[1]);
                    }
                }

            })
        },
        methods: {
            infoajax: function () {
                var _this = this;
                $.ajax({
                    url: '/Api/v1/Member/' + $.get_user('Id'),
                    type: 'PUT',
                    data: {
                        NickName: '',
                        Birthday:'',
                        Sex: '0',
                        IDCard:''
                    }
                }).done(function (rs) {
                    if (rs.returnCode == '200') {
                        _this.info = rs.data;
                        _this.locationinfo();

                    }
                })
            },
            locationinfo: function () {
                var _this=this;
                sex=_this.info.Sex;
                console.log(sex);
                // var sex = $.get_user('Sex');
                // var tel = $.get_user('PhoneNumber');
                // var name = $.get_user('NickName');
                // var cdcard = $.get_user('IDCard ');
                var sexval;
                if (sex == 0) {
                    sexval = '请选择性别'
                } else if (sex == 1) {
                    sexval = '男'
                } else if (sex == 2) {
                    sexval = '女'
                }
                $('.sex').html(sexval);
                // $('.telenum').html(tel);
                // $('.cdcard').html(cdcard);
                // $('.name').html(decodeURIComponent(decodeURIComponent(name)));
                $.RMLOAD()
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
                            localStorage['qy_head'] = $.get_user('Id') + "|" + data.SmallThumbnail
                                // $('.head').attr('data-id', data.Id);
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
                    if ($(this).hasClass('gray')) {
                        return false
                    } else {
                        $(this).addClass('gray');
                        _this.exitajax();
                    }
                })

            },
            exitajax: function () {
                $.ajax({
                    url: '/Api/v1/Logout',
                    type: 'post'
                }).done(function (rs) {
                    console.log(123)
                    if (rs.returnCode == '200') {
                        localStorage.removeItem('qy_user');
                        localStorage.removeItem('qy_loginToken');
                        localStorage.removeItem('qy_idcard');
                        localStorage.removeItem('qy_NickName');
                        window.location.href = "/Html/login.html"
                    }
                }).always(function () {
                    $('.submit').removeClass('gray')
                })

            },
            hideexit:function () {
                if($.is_weixin()){
                    $('.submit').hide();
                }
            },
            bitlink:function () {
                $('.xingm').on('click',function () {
                    if(id){
                        window.location.href='/Html/name.html?id='+id;
                    }else{
                        window.location.href='/Html/name.html'
                    }
                })

                $('.shenfz').on('click',function () {
                    if(id){
                        window.location.href='/Html/cdcard.html?id='+id;
                    }else{
                        window.location.href='/Html/cdcard.html'
                    }
                })
            }
        }
    })
})