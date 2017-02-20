jQuery(function ($) {
    //用来存放上传凭证图片的Id
    var imageId = "";
    var imageId1 = "";
    var imageId2 = "";
   //退款申请提交
    $('.service-btn').on('click', function () {
        alert("你好！");
        $.ajax({
                type: 'POST',
                url: $("#RefundForm").attr("action"),
                cache: false,
                data: $("#RefundForm").serialize()
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
            $.MsgBox.Confirm("提示", "您的浏览器没有安装flash插件，需安装后才能上传，现在安装？", function () {
                window.location.href = "http://get.adobe.com/cn/flashplayer/";
            });
        },
        'onUploadSuccess': function (file, data, response) {
            var rs = JSON.parse(data);
            //console.log(rs);
            if (rs.returnCode == "200") {
                var oimg = rs.data[0];
                setimgurl(oimg);
            }
        }
    });

    //退款上传凭证：图片上传
    $("#doc").uploadify({
        'swf': '/Resources/Web/js/update/uploadify.swf',
        'fileSizeLimit': '5120KB',
        'fileTypeExts': '*.jpg;*.jpeg;*.png,*.gif',
        'uploader': '/Api/v1/File/Upload',
        'formData': { 'mediumThumbnailWidth': 200, 'mediumThumbnailHeight': 200 },
        width: 100,
        buttonText: '',
        multi: false,
        'onFallback': function () {
            $.MsgBox.Confirm("提示", "您的浏览器没有安装flash插件，需安装后才能上传，现在安装？", function () {
                window.location.href = "http://get.adobe.com/cn/flashplayer/";
            });
        },
        'onUploadSuccess': function (file, data, response) {
            var rs = JSON.parse(data);
            //console.log(rs);
            if (rs.returnCode == "200") {
                var oimg = rs.data[0];
                setimgurl(oimg);
            }
        }
    });


    function setimgurl(oimg) {
        if (oimg) {
            $("#preview").attr("src", oimg.MediumThumbnail);
            imageId = oimg.Id;
         
        }
    }
    //第二张图片
    $("#doc1").uploadify({
        'swf': '/Resources/Web/js/update/uploadify.swf',
        'fileSizeLimit': '5120KB',
        'fileTypeExts': '*.jpg;*.jpeg;*.png,*.gif',
        'uploader': '/Api/v1/File/Upload',
        'formData': { 'mediumThumbnailWidth': 200, 'mediumThumbnailHeight': 200 },
        width: 100,
        buttonText: '',
        multi: false,
        'onFallback': function () {
            $.MsgBox.Confirm("提示", "您的浏览器没有安装flash插件，需安装后才能上传，现在安装？", function () {
                window.location.href = "http://get.adobe.com/cn/flashplayer/";
            });
        },
        'onUploadSuccess': function (file, data, response) {
            var rs = JSON.parse(data);
            //console.log(rs);
            if (rs.returnCode == "200") {
                var oimg = rs.data[0];
                setimgurl1(oimg);
            }
        }
    });
    function setimgurl1(oimg) {
        if (oimg) {
            $("#preview1").attr("src", oimg.MediumThumbnail);
            imageId1= oimg.Id;
        }
    }
    //第三张图片
    $("#doc2").uploadify({
        'swf': '/Resources/Web/js/update/uploadify.swf',
        'fileSizeLimit': '5120KB',
        'fileTypeExts': '*.jpg;*.jpeg;*.png,*.gif',
        'uploader': '/Api/v1/File/Upload',
        'formData': { 'mediumThumbnailWidth': 200, 'mediumThumbnailHeight': 200 },
        width: 100,
        buttonText: '',
        multi: false,
        'onFallback': function () {
            $.MsgBox.Confirm("提示", "您的浏览器没有安装flash插件，需安装后才能上传，现在安装？", function () {
                window.location.href = "http://get.adobe.com/cn/flashplayer/";
            });
        },
        'onUploadSuccess': function (file, data, response) {
            var rs = JSON.parse(data);
            //console.log(rs);
            if (rs.returnCode == "200") {
                var oimg = rs.data[0];
                setimgurl2(oimg);
            }
        }
    });
    function setimgurl2(oimg) {
        if (oimg) {
            $("#preview2").attr("src", oimg.MediumThumbnail);
            imageId2 = oimg.Id;
        }

    }

    var allImageIds = "";
    //把上传凭证的个图片Id拼接成以逗号的字符串
    if (imageId !== "" && imageId1 !=="" && imageId2 !=="") {
         allImageIds = imageId + ',' + imageId1 + ',' + imageId2;
    }
    else if (imageId !== "" && imageId1 !== "") {
        allImageIds = imageId + ',' + imageId1;
    }
    else if (imageId !== "") {
        allImageIds = imageId;
    }
    else { 
        allImageIds = "";
    }
    $("#RefundImageIds").val(allImageIds);
});