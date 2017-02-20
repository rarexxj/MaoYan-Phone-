$(document).ready(function () {

    //导航点击
    $('.index-headnavmainul .index-navli').hover(function () {
        $(this).addClass('active');
        $(this).siblings().removeClass('active');
        $(this).children('.index-navtwo').stop().slideToggle();
    });

    //保存
    $('.sub_baocun').on('click', function () {

        var chkRadio = $('input:radio[name="a1"]:checked').val();
        $("#Sex").val(chkRadio);
        $.ajax({
            type: 'POST',
            url: $("#PersonalForm").attr("action"),
            cache: false,
            data: $("#PersonalForm").serialize()
        }).done(function(data) {
            if (data.Success) {
                setimgurl(data.data);
                history.go(0);
                my_alert("个人信息提交成功");
            } else {
                my_alert(data.ErrorMessage);

            }
        });

    });

    $("#uploadify").uploadify({
        'swf': '/Resources/Web/js/update/uploadify.swf',
        'fileSizeLimit': '5120KB',
        'fileTypeExts': '*.jpg;*.jpeg;*.png,*.gif',
        'uploader': '/Api/v1/File/Upload',
        'formData': { 'mediumThumbnailWidth': 200, 'mediumThumbnailHeight': 200 },
        width: 100,
        buttonText: '',
        multi: false,
        'onFallback': function () {
            debugger;
            $.MsgBox.Confirm("提示", "您的浏览器没有安装flash插件，需安装后才能上传，现在安装？", function() {
                window.location.href = "http://get.adobe.com/cn/flashplayer/";
            });
        },
        'onUploadSuccess': function(file, data, response) {
            debugger;
            var rs = JSON.parse(data);
         
            if (rs.returnCode ===  "200") {
                var oimg = rs.data[0];
                console.log(oimg);
                setimgurl(oimg);
            }
        }
    });
   
    function setimgurl(oimg) {
        if (oimg) {
            $("#preview").attr("src", oimg.MediumThumbnail);
            $(".img_user").css("background", "url("+oimg.MediumThumbnail+") no-repeat center");
            $("#AvatarId").val(oimg.Id);
        }
    }
});
        
