$(document).ready(function () {
    //导航点击
    $('.index-headnavmainul .index-navli').hover(function () {
        $(this).addClass('active');
        $(this).siblings().removeClass('active');
        $(this).children('.index-navtwo').stop().slideToggle();
    });


    wordlimit(".address_add p", 40);
    //地址多行超出字段隐藏
    function wordlimit(cname, wordlength) {

        var cname = $(cname);
        for (var i = 0; i < cname.length; i++) {
            var nowLength = cname[i].innerHTML.length;
            if (nowLength > wordlength) {
                cname[i].innerHTML = cname[i].innerHTML.substr(0, wordlength) + ' . . . ';
            }
        }
    }


    //	默认地址切换
    $(".she_tacityly").on("click", function () {
        var index = $(".address_list .she_tacityly").index($(this));
        var addressId = $(this).data("id");
        var memberId = $(this).data("memberid");
        $.ajax({
            url: url_defaultaddress,
            type: "post",
            data: { memberId: memberId, addressId: addressId }
        }).done(function (data) {
            if (data.Success) {
                $(".xuanzhong").removeClass("active");
                $(".she_tacityly").removeClass("hidden");
                $(".xuanzhong").eq(index).addClass("active");
                $(".she_tacityly").eq(index).addClass("hidden");
            } else {

                my_alert(data.ErrorMessage);

            }
        });



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

                    html += "<option value='" + value.Id + "'>" + value.FullName + "</option>";
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
                    if (value.Id == province) {
                        pid = value.Id;
                        html += "<option value='" + value.FullName + "' selected='selected' >" + value.FullName + "</option>";
                    } else {
                        html += "<option  value='" + value.FullName + "'>" + value.FullName + "</option>";
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
                        if (value.Id == city) {
                            cid = value.Id;
                            html1 += "<option value='" + value.FullName + "' selected='selected' >" + value.FullName + "</option>";
                        } else {
                            html1 += "<option value='" + value.FullName + "'>" + value.FullName + "</option>";
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
                            if (value.Id == district) {
                                did = value.Id;
                                html2 += "<option  value='" + value.FullName + "' selected='selected' >" + value.FullName + "</option>";
                            } else {
                                html2 += "<option  value='" + value.FullName + "'>" + value.FullName + "</option>";
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
        //var provinceId = ($(this).find("option:checked").attr("id"));
        var provinceId = $("#Province option:selected").val();
        var html = "<option value=''>市</option>";
        $.ajax({
            type: "get",
            dataType: "json",
            contentType: "application/json;charset=utf-8",
            url: 'Api/v1/Settings/District/' + provinceId + '/Child',
        }).done(function (data) {
            if (data.returnCode === "200") {
                $.each(data.data, function (index, value) {
                    html += "<option  value='" + value.Id + "'>" + value.FullName + "</option>";

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
        //var districtId = ($(this).find("option:checked").attr("id"));
        var districtId = $("#City option:selected").val();;
        $.ajax({
            type: "get",
            dataType: "json",
            contentType: "application/json;charset=utf-8",
            url: 'Api/v1/Settings/District/' + districtId + '/Child',
        }).done(function (data) {
            if (data.returnCode === "200") {
                $.each(data.data, function (index, value) {
                    html += "<option  value='" + value.Id + "'>" + value.FullName + "</option>";
                });
            } else {
                my_alert(result.ErroMessage);
            }
            $("#District").append(html);
        });

    });

    //	地址删除
    $(".address_list").on("click", ".add_detail", function () {
        var addressId = $(this).data("id");
        $.ajax({
            url: url_deleteaddress,
            type: "post",
            data: { addressId: addressId }
        }).done(function (data) {
            if (data.Success) {
                //添加成功后跳到收货地址列表页
                location.href = url_addressList;
            }
            else {
                my_alert(data.ErrorMessage);
            }
        });
        var index = $(".address_list .add_detail").index($(this));

        $(".address_list li").eq(index).remove();

    });


    //	添加收货地址

    var type;
    $(".add_shouhuo").on("click", function () {
        getProvince();
        type = 1; //添加收货
        $("#AddAddressForm").attr("action", url_addaddress);
        var div = document.createElement("div");
        $(".footer").after(div);
        $(".add_address").show();
        details_box(div); //取消弹窗
        queren(div); //确认收货地址

    });
    //编辑收货地址
    $(".bianji").on("click", function () {

        type = 2; //编辑收货
        var provinceId = $(this).data("province");
        var cityId = $(this).data("city");
        var districtId = $(this).data("district");
        getProvince1(provinceId, cityId, districtId);
        var div = document.createElement("div");
        $(".footer").after(div);
        $("#AddAddressForm").attr("action", url_editaddress);
        $(".shuohuo_add").html("编辑收货地址");
        var contacts = $(this).data("contacts");
        var phoneId = $(this).data("phone");
        var addressId = $(this).data("addressid");


        var isdefault = $(this).data("isdefault");
        $("#Contacts").val(contacts);
        $("#Phone").val(phoneId);
        $("#AddressId").val(addressId);


        $("#Address").val($(this).data("address"));

        if (isdefault == "True") {
            $("#DefaultAddress").val("True");
            $("#IsDefault").addClass("bg_orange");
        } else {
            $("#DefaultAddress").val("False");
            $("#IsDefault").removeClass("bg_orange");
        }
        $(".add_address").show();
        details_box(div); //取消弹窗
        queren(div); //确认收货地址


    });


    //默认切换


    $(".moren").on("click", function () {

        if ($(".moren").hasClass('bg_orange')) {

            $(".moren").removeClass('bg_orange');

        } else {

            $(".moren").addClass('bg_orange');
        }
    });


    //取消弹窗
    function details_box(div) {
        //取消
        $(".detail_x ,.no").on("click", function () {

            $(div).hide();
            btn = true;
            btn1 = true;
        });

    }
    //取消或确定
    $(".detail_x ,.no").on("click", function () {

        $(".add_address").hide();
        btn = true;
        btn1 = true;
    });

    //确认收货地址
    function queren(div) {

        $(".yes").on("click", function () {
            //			前端限制判断
            if (!noNull(".shouhuo_man")) {

                my_alert('请填写收货人');
                return false;
            }
            if (!iphone(".shuohuo_iphone")) {

                my_alert('手机号码格式错误');
                return false;
            }
            if (!noNull(".address_texa")) {

                my_alert('请填写详细地址');
                return false;
            }
            $("#DefaultAddress").val("false");
            if ($(".moren").hasClass('bg_orange')) {
                $("#DefaultAddress").val("true");
            }

            $(div).remove();
            var regionName = $("#Province option:selected").html() + "," + $("#City option:selected").text() + "," + $("#Province option:selected").html() + $("#District option:selected").html();
            $("#RegionName").val(regionName);
            //这块应该已经提交信息,列表展示信息应该从后端获取
            //现在只是展示用,后端可删除
            if (type == "1") {
                $.ajax({
                    type: 'POST',
                    url: $("#AddAddressForm").attr("action"),
                    data: $("#AddAddressForm").serialize()
                }).done(function (data) {
                    if (data.Success) {
                        //添加成功后跳到收货地址列表页
                        location.href = url_addressList;
                    }
                    else {
                        my_alert(data.ErrorMessage);
                    }
                });
            }
            if (type == "2") {
                $.ajax({
                    type: 'POST',
                    url: $("#AddAddressForm").attr("action"),
                    data: $("#AddAddressForm").serialize()
                }).done(function (data) {
                    if (data.Success) {
                        //添加成功后跳到收货地址列表页
                        location.href = url_addressList;
                    }
                    else {
                        my_alert(data.ErrorMessage);
                    }
                });
            }
            //增加收货地址
            //$(".address_list").append("<li class='clearfix'> <div class='address_name'>" + name1 + "</div> <div class='address_iphone color_999'>" + iphone1 + "</div> <div class='address_add'> <p>浙江省杭州市江省杭州市西湖区紫荆</p> </div> <div class='address_operate'><span class='color_orange bianji'>编辑</span><span class='color_orange add_detail'>删除</span></div> <div class='address_tacityly'><span class='color_orange she_tacityly'>设为默认地址</span> <div class='xuanzhong'>默认地址</div> </div> </li>");

        });
    }
})