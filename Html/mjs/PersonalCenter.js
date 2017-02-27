/**
 * Created by admin on 2016/8/24.
 */
$(function () {
    $.ADDLOAD();
    var userinfo = $.getUrlParam('userInfo');
    if(userinfo){
        //userinfo=decodeURIComponent(userinfo);
        userinfo=base64_decode(userinfo);
        // alert(userinfo)
        userinfo=eval("("+userinfo+")");
        localStorage.setItem('qy_loginToken',userinfo.PhoneNumber+':'+userinfo.DynamicToken);
        localStorage['qy_Identity']=userinfo.Id;
        localStorage['qy_UserName']=userinfo.UserName;
        //localStorage['qy_CreateTime']=rs.data.CreateTime;
        localStorage['qy_NickName']=encodeURIComponent(encodeURIComponent(userinfo.NickName));
        localStorage['qy_Sex']=userinfo.Sex;
        localStorage['qy_Birthday']=userinfo.Birthday;
        localStorage['qy_PhoneNumber']=userinfo.PhoneNumber;
        localStorage['qy_Province']=userinfo.Province;
        localStorage['qy_City']=userinfo.City;
        localStorage['qy_InvitationCode']=userinfo.InvitationCode;
        if(userinfo.Avatar != null){
            localStorage['qy_head']=userinfo.Id+'|'+userinfo.Avatar.SmallThumbnail;
        }
    }

    window.TOKEN=localStorage.getItem('qy_loginToken')
    if(window.TOKEN&&location.pathname.indexOf('/Html/Member/Login')<=-1){
        $.ajaxSetup({
            headers:{
                Authorization:'Basic '+base64encode(window.TOKEN)
            } 
        })
    }else if((window.TOKEN&&location.pathname.indexOf('/Html/Member/Login')>-1) || (!window.TOKEN&&location.pathname.indexOf('/Html/Products')>-1) || (!window.TOKEN&&location.pathname.indexOf('/Index.html')>-1) || (!window.TOKEN&&location.pathname.indexOf('/Html/Member/Register.html')>-1) || (!window.TOKEN&&location.pathname.indexOf('/Html/Member/Forget.html')>-1) || (!window.TOKEN&&location.pathname.indexOf('/Html/Member/Login.html')>-1) || (!window.TOKEN&&location.pathname.indexOf('/Html/Share')>-1)){
        //console.log('不用跳转登录页')
    }else{
        if(is_weixin()){
            window.location.replace('/WeiXin/Login');
        }else{
            window.location.replace('/Html/Member/Login.html');
        }
    }
    function is_weixin() {
        var ua = navigator.userAgent.toLowerCase();
        if (ua.match(/micromessenger/i) == "micromessenger") {
            return true;
        } else {
            return false;
        }
    }
    $('.head .name').html(decodeURIComponent(decodeURIComponent(localStorage['qy_NickName'])));
    $('.head .ph').html(localStorage['qy_PhoneNumber']);
    if(localStorage['qy_head']){
        if(localStorage['qy_Identity']==localStorage['qy_head'].toString().split('|')[0]){
            $('.head .img img').attr('src',localStorage['qy_head'].toString().split('|')[1]);
        }
    }
 
    ajax();
    function ajax() {
        $.ajax({
            url:'/Api/v1/Member/CenterInfo',
            type:'get'
        }).done(function (rs) {
            if (rs.returnCode == '200'){
                view(rs.data);
            }else {
                if (rs.returnCode == '401'){
                    Backlog();
                }else{
                    oppo(rs.msg ,1)
                }
            }
        })
    }

    function view(rs) {
        if (!rs.Money||rs.Money=='null'){
            rs.Money = 0
        }
        if (!rs.Integral||rs.Integral=='null'){
            rs.Integral = 0
        }
        localStorage['qy_MemberType']=rs.MemberType;
        new Vue({
            el:'#per-cen',
            data:rs,
            ready:function () {
                $.RMLOAD();
                js();
            }
        })
    }

    function js() {
        $('.weixin .att').on('click',function () {
            $('.mask').stop().fadeIn();
        })
        $('.mask').on('click',function () {
            $(this).stop().fadeOut();
        })
    }

    // function base64_encode(){
    //     var str=CryptoJS.enc.Utf8.parse($("#source").val());
    //     var base64=CryptoJS.enc.Base64.stringify(str);
    //     $("#result").val(base64);
    // }
    function base64_decode(str){
        var words  = CryptoJS.enc.Base64.parse(str);
        words = words.toString(CryptoJS.enc.Utf8);
        return words
    }

})