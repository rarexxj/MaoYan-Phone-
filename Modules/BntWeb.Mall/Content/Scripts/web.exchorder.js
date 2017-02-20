
// 对Date的扩展，将 Date 转化为指定格式的String   
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符，   
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字)   
// 例子：   
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423   
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18   
Date.prototype.Format = function (fmt) { //author: meizz   
    var o = {
        "M+": this.getMonth() + 1,                 //月份   
        "d+": this.getDate(),                    //日   
        "h+": this.getHours(),                   //小时   
        "m+": this.getMinutes(),                 //分   
        "s+": this.getSeconds(),                 //秒   
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度   
        "S": this.getMilliseconds()             //毫秒   
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

$(function () {
    //全局变量 其他收货地址
    var n = "";
    //导航点击
    $('.index-headnavmainul .index-navli').hover(function () {
        $(this).addClass('active');
        $(this).siblings().removeClass('active');
        $(this).children('.index-navtwo').stop().slideToggle();
    });

    //省市区的下拉加载
    function getProvince() {

        $("#Contacts").val("");
        $("#Phone").val("");
        $("#Address").val("");
        $("#Province").val("");
        $("#City").empty();
        $("#District").empty();
        $("#City").append("<option value=''>市</option>");
        $("#District").append("<option value=''>区/县</option>");
        var html = "<option value=''>省</option>";

        $.ajax({
            type: "get",
            url: '/Api/v1/Settings/District/' + 0 + '/Child',
            data: { id: 0 }
        }).done(function (data) {
            if (data.returnCode === "200") {
                $.each(data.data, function (index, value) {

                    html += "<option id='" + value.Id + "' value='" + value.FullName + "'>" + value.FullName + "</option>";
                });
            }
            else {
                my_alert(data.ErrorMessage);
            }
            $("#Province").append(html);
        });

    }


    function getProvince1(province, city, district) {
        var pid = "", cid = "", did = "";

        var html = "<option value=''>省</option>";

        $.ajax({
            type: "get",
            url: '/Api/v1/Settings/District/' + 0 + '/Child',
            data: { id: 0 }
        }).done(function (data) {
            if (data.returnCode === "200") {
                $.each(data.data, function (index, value) {
                    if (value.FullName == province) {
                        pid = value.Id;
                        html += "<option id='" + value.Id + "' value='" + value.FullName + "' selected='selected' >" + value.FullName + "</option>";
                    } else {
                        html += "<option id='" + value.Id + "' value='" + value.FullName + "'>" + value.FullName + "</option>";
                    }
                });
            }
            else {
                my_alert(data.ErrorMessage);
            }
            $("#Province").append(html);


            var html1 = "<option value=''>市</option>";
            $.ajax({
                type: "get",
                dataType: "json",
                contentType: "application/json;charset=utf-8",
                url: 'Api/v1/Settings/District/' + pid + '/Child',
            }).done(function (data) {
                if (data.returnCode === "200") {
                    $.each(data.data, function (index, value) {
                        if (value.FullName == city) {
                            cid = value.Id;
                            html1 += "<option id='" + value.Id + "' value='" + value.FullName + "' selected='selected' >" + value.FullName + "</option>";
                        } else {
                            html1 += "<option id='" + value.Id + "' value='" + value.FullName + "'>" + value.FullName + "</option>";
                        }

                    });
                } else {
                    my_alert(result.ErroMessage);
                    //async:false
                }
                $("#City").append(html1);


                var html2 = "<option value=''>区/县</option>";
                //$("#City option").remove();
                $("#District option").remove();
                var districtId = ($(this).find("option:checked").attr("id"));
                $.ajax({
                    type: "get",
                    dataType: "json",
                    contentType: "application/json;charset=utf-8",
                    url: 'Api/v1/Settings/District/' + cid + '/Child',
                }).done(function (data) {
                    if (data.returnCode === "200") {
                        $.each(data.data, function (index, value) {
                            if (value.FullName == district) {
                                did = value.Id;
                                html2 += "<option id='" + value.Id + "' value='" + value.FullName + "' selected='selected' >" + value.FullName + "</option>";
                            } else {
                                html2 += "<option id='" + value.Id + "' value='" + value.FullName + "'>" + value.FullName + "</option>";
                            }
                        });
                    } else {
                        my_alert(result.ErroMessage);
                    }
                    $("#District").append(html2);
                });
            });
        });

    }
    //市的下拉
    $(".Province").change(function () {

        $("#City").empty();
        $("#District").empty();
        var provinceId = ($(this).find("option:checked").attr("id"));
        var html = "<option value=''>市</option>";
        $.ajax({
            type: "get",
            dataType: "json",
            contentType: "application/json;charset=utf-8",
            url: 'Api/v1/Settings/District/' + provinceId + '/Child',
        }).done(function (data) {
            if (data.returnCode === "200") {
                $.each(data.data, function (index, value) {
                    html += "<option id='" + value.Id + "' value='" + value.FullName + "'>" + value.FullName + "</option>";

                });
            } else {
                my_alert(result.ErroMessage);
                //async:false
            }
            $("#City").append(html);
        });

        $("#District").append("<option value=''>区/县</option>");
    });

    //区的下拉
    $(".City").change(function () {
        var html = "<option value=''>区/县</option>";
        //$("#City option").remove();
        $("#District option").remove();
        var districtId = ($(this).find("option:checked").attr("id"));
        $.ajax({
            type: "get",
            dataType: "json",
            contentType: "application/json;charset=utf-8",
            url: 'Api/v1/Settings/District/' + districtId + '/Child',
        }).done(function (data) {
            if (data.returnCode === "200") {
                $.each(data.data, function (index, value) {
                    html += "<option id='" + value.Id + "' value='" + value.FullName + "'>" + value.FullName + "</option>";
                });
            } else {
                my_alert(result.ErroMessage);
            }
            $("#District").append(html);
        });

    });
    //添加收货地址
    var btn = true;
    $(".adaddr").on("click", function () {
        getProvince();
        var div = document.createElement("div");
        $(".footer").after(div);
        $("#add").show();
        details_box(div); //取消弹窗
        queren(div); //确认收货地址
        //默认切换
        if (btn) {
            one();
            btn = false;
        }
    });
    //选择其他收货地址
    $('.otheraddr').on('click', function () {
        var div = document.createElement("div");
        //$(".footer").after(div);
        //选择其他收货地址弹框显示
        $("#other").show();
        //otherqueren(div);//确认其他收货地址


    });

    details_box(); //取消弹窗
    //取消弹窗
    function details_box() {
        //取消
        $(".detail_x ,.no,.quxiao,.sure").on("click", function () {
            $('.add_address').hide();
            btn = true;
            btn1 = true;
        });
    }

    chioceAddress1();
    //地址选择
    function chioceAddress1() {
        $('.add_address').on('click', '.add-chioce', function () {
            $(this).addClass('active');
            $(this).attr('data-id', '1');
            $(this).siblings().removeClass('active');
            $(this).siblings().attr('data-id', '0');
            n = $(this).attr('data-id');
            $("#d1").html("其他收货地址");
            $("#d2").html("收货人 :" + $(this).find("#a1").html());
            $("#d3").html("联系方式 : " + $(this).find("#a2").html());
            $("#d4").html("收货地址 :" + $(this).find("#a3").html());
            $("#c1").html($(this).find("#a1").html());
            $("#c2").html($(this).find("#a2").html());
            $(".zongjibox3").html($(this).find("#a3").html());
            $("#AddressId").val($(this).data("id"));
        });
    };
    //选择其他收货地址确定按钮

    $('.sure').on('click', function () {
        $("#other").hide();
    });

    //添加收货地址的默认切换
    function one() {
        //设为默认
        $(".acquiesce").on("click", function () {

            if ($(".moren").hasClass('bg_orange')) {
                $(".moren").removeClass('bg_orange');
            } else {
                $(".moren").addClass('bg_orange');
            }
        });
    }
    //加
    $(".num-box .but-add").click(function () {
        var n = parseInt($(this).siblings('.but-num').val()) + 1;
        var danjia = $(this).parents('.numout').siblings('.danjia').children(' .danjia-xx').html();
        $(this).siblings('.but-num').val(n);
        //点击加号 变换价格
        $(this).parents('.numout').siblings('.peice-danjian').find('.peice-danjianxx').html((n * danjia).toFixed(2));

        allprice();
        priceall();
    });
    //减
    $(".num-box .but-cut").click(function () {
        var n = parseInt($(this).parent().find(".but-num").val()) - 1;
        var danjia = $(this).parents('.numout').siblings('.danjia').children(' .danjia-xx').html();
        console.log(danjia);
        if (n > 0) {
            $(this).siblings('.but-num').val(n);
            n--;
        } else {
            $(this).siblings('.but-num').val(1);
        }
        $(this).parents('.numout').siblings('.peice-danjian').find('.peice-danjianxx').html(((n + 1) * danjia).toFixed(2));

        allprice();
        priceall();
    });

    //单价保留两位小数
    $('.danjia-xx').each(function () {
        var _price = Number($(this).html());
        $(this).html(_price.toFixed(2));
    });
    //确认收货地址
    function queren(div) {
        $(".yes").on("click", function () {
            //前端限制判断
            if (!noNull(".shouhuo_man")) {

                my_alert('请填写收货人');
                return false;
            }
            if (!iphone(".shuohuo_iphone")) {

                my_alert('手机号码格式错误');
                return false;
            }
            if (!noNull(".Address1_texa")) {

                my_alert('请填写详细地址');
                return false;
            }
            $.ajax({
                type: 'POST',
                url: $("#AddAddressForm").attr("action"),
                data: $("#AddAddressForm").serialize()
            }).done(function (data) {
                if (data.Success) {
                    //给隐藏控件省市区赋值
                    $("#Province1").val($("#Province").val());
                    $("#City1").val($("#City").val());
                    $("#District1").val($("#District").val());
                    //添加成功后跳到收货地址列表页   
                    $("#d1").html("新收货地址");
                    $("#d2").html("收货人 :" + $("#Contacts").val());
                    $("#d3").html("联系方式 : " + $("#Phone").val());
                    $("#d4").html("收货地址 :" + $("#Address").val());
                    $("#c1").html($("#Contacts").val());
                    $("#c2").html($("#Phone").val());
                    $(".zongjibox3").html($("#Address").val());
                    //给隐藏控件地址id赋值
                    $("#AddressId").val(data.Data.Id);
                    $("#add").hide();
                }
                else {
                    my_alert(data.ErrorMessage);
                }
            });
        });
        $(div).remove();
    }
    //页面加载显示价格
    show();
    function show() {
        var sumPrice = 0;
        $('.bixuanpro .peice-danjianxx').each(function () {
            var priceOne = Number($(this).html());
            sumPrice = sumPrice + priceOne;
        });
        $('.price-all span').html(sumPrice.toFixed(2));
    }

    //提交订单Form表单提交
    $(".tijiao").on('click', function () {
        //判断积分够不够支付
        if (eval(Number($("#integra").val())) < eval(Number($(".zongjiprice").html()))) {
            my_alert("可用积分不足！");
            return false;
        }
        $("#Integral").val(Number($(".zongjiprice").html()));
        if ($("#BestTime").val() == "") {
            my_alert("请填写最佳送货呢时间！");
            return false;
        }
        //判断送货时间函数
        kongztime();
        $.ajax({
            type: 'POST',
            url: $("#ConfirmForm").attr("action"),
            data: $("#ConfirmForm").serialize()
        }).done(function (data) {
            if (data.Success) {
                //到我的订单
                location.href = url_myOrder;
            }
            else {
                my_alert(data.ErrorMessage);
            }
        });
    });

    $(".marginleft172").on('click', function () {
        $("#coupon").hide();
        priceall();
    });
    //商品最下面总价格
    priceall();
    function priceall() {
        var _price1 = Number($('.price-all span').html());
        var _price4 = Number($('.yunf span').html());
        var allprice = _price1- _price4;
        $('.zongjiprice').html(allprice.toFixed(2));
    }
    //datetimeppicker
    $('#BestTime').datetimepicker({

        dayOfWeekStart: 1,
        lang: 'en',
        minDate: 'true'

    });
    //计算总价格方法
    function allprice() {
        var sumPrice = 0;
        var otherprice = 0;
        var btn = false;
        $('.bixuanpro .peice-danjianxx').each(function () {
            var priceOne = Number($(this).html());
            sumPrice = sumPrice + priceOne;
        });
        $('.zixuanpro .peice-danjianxx').each(function () {
            if ($(this).parents().siblings('.nocenter').children('input')[0].checked) {
                var priceTwo = Number($(this).html());
                otherprice += priceTwo;
                btn = true;
            }
            if (!btn) {
                otherprice = 0;
            }
        });
        var _allprice = sumPrice + otherprice;
        //console.log(_allprice);
        $('.price-all span').html(_allprice.toFixed(2));
    
    }

    kongztime();
    function kongztime() {
        $('#BestTime').bind('blur', function () {
            var _time = $(this).val();
            var date = _time.split(' ')[0];
            //送货的 时分
            var time = _time.split(' ')[1];
            var mydate = new Date();
            //获得现在的时分
            var timenow = (mydate.getHours() + 5) + ":" + mydate.getMinutes();
            var nowdate = mydate.Format("yyyy/MM/dd");

            //如果现在的日期和送货日期相等 判断送货的时分是否在距现在5小时后
            if (date === nowdate) {

                if (time < timenow)
                    my_alert("送货时间要在5小时后！");
            }
        });
    };

    //取消弹窗
    $(".quxiao ,.no").on("click", function () {
        $("#add").hide();
        $("#coupon").hide();
        $("#other").hide();
    });
   
});