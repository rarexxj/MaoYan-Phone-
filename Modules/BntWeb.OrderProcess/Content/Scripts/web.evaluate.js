$(document).ready(function () {

    //用来存放上传凭证图片的Id
    var imageArr = [];
    //定义一个对象
    var fileObj;
    var imageId = "";
    var imageId1 = "";
    var imageId2 = "";
 
    //导航点击
    $('.index-headnavmainul .index-navli').hover(function () {
        $(this).addClass('active');
        $(this).siblings().removeClass('active');
        $(this).children('.index-navtwo').stop().slideToggle();
    });

    //五星评分
    pinfen(".grade span");	//材料新鲜
    pinfen(".describe span");	//描述相符
    pinfen(".serve  span");	//物流服务
    pinfen(".kougan  span");	//口感
    function pinfen(element) {
        $(element).on("click", function () {

            var index = $(element).index($(this));
            var length = $(element).length;
            if (!$(this).hasClass("full_orange")) {
                $(element).removeClass("full_orange");
                for (var i = 0; i < index + 1; i++) {
                    $(element).eq(i).addClass("full_orange");
                }
            } else {
                for (var i = length; i > index - 1; i--) {
                    $(element).eq(i).removeClass("full_orange");
                }

            }
        });
    }
    //遍历综合评分是几个星   
    //口感满意评分
    function getOne() {
        var score = 0;
        $(".kougan span").each(function () {
            if ($(this).hasClass("full_orange")) {
                score = score + 1;
            }
            $("#GoodTasteScore").val(score);
        });
    }
    //描述相符
    function getTwo() {
        var score = 0;
        $(".describe span").each(function () {
          
            if ($(this).hasClass("full_orange")) {
                score = score + 1;
            }
            $("#DesMatchScore").val(score);
        });
    }
    //物流服务
    function getThree() {
        var score = 0;
        $(".serve  span").each(function () {
            if ($(this).hasClass("full_orange")) {
                score = score + 1;
            }
            $("#LogisticsScore").val(score);
        });
    }
    //材料新鲜
    function getFour() {
        var score = 0;
        $(".grade  span").each(function () {
            if ($(this).hasClass("full_orange")) {
                score = score + 1;
            }
            $("#FreshMaterialScore").val(score);
        });
    }

    //提交
    $(".tijiao").on('click', function() {
        getOne();
        getTwo();
        getThree();
        getFour();

        $('.upload img').each(function () {

            fileObj = { id: $(this).data("id") }
            imageArr.push(fileObj);
        });
        $("#EvaluateImageIds").val(JSON.stringify(imageArr));
        //取放订单Id
        $("#orderId").val($(this).data('id'));
        var url = $(this).data("url");
        if (confirm("您确定要提交评价吗？")) {
            $.ajax({
                url: $("#EvaluateForm").attr('action'),
                type: "post",
                data: $("#EvaluateForm").serialize(),
                success: function (data) {
                    if (data.Success) {
                        location.href = url;
                    } else {
                        alert(data.ErrorMessage);
                    }
                },
                error: function () { }
            });
        }
    });

    //上传图片
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
            if (rs.returnCode == "200") {
                var oimg = rs.data[0];
                if ($(".upload li").length >= 4) {
                } else {
                    $("#doc").parent().before('<li class="fl first_file"><img data-id="' + oimg.Id + '" src="' + oimg.MediumThumbnail + ' "/>  <div class="pic_datails"></div></li>');
                }

            }
        }
    });

    //图片删除
    $(".upload").on("click", ".pic_datails", function () {
        $(this).parent().remove();
    });
})