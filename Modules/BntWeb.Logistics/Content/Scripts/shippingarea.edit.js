

jQuery(function ($) {
    bntToolkit.initForm($("#ShippingAreaForm"), {
        Name: {
            required: true,
            maxlength: 120
        }
    }, null, success);

    $("#dllDrovince").change(function () {
        var val = $(this).val();
        if (val != "0") {
            $.ajax({
                type: "Get",
                url: url_loadDistrict + "?parentId=" + val,
                dataType: "json",
                success: function (data) {
                    $("#dllCity").html("<option value=\"0\">请选择市</option>" + data);
                }
            });

        } else {
            $("#dllCity").html("<option value=\"0\">请选择市</option>");
            //$("#dllArea").html("<option value=\"0\">请选择区</option>");
        }
    });

    //$("#dllCity").change(function () {
    //    var val = $(this).val();
    //    if (val != "0") {
    //        $.ajax({
    //            type: "Get",
    //            url: url_loadDistrict + "?parentId=" + val,
    //            dataType: "json",
    //            success: function (data) {
    //                $("#dllArea").html("<option value=\"0\">请选择区</option>" + data);
    //            }
    //        });
    //    } else {
    //        $("#dllArea").html("<option value=\"0\">请选择区</option>");
    //    }
    //});

    $('#ShippingAreaForm').on("click", "#addArea", function (e) {
        var areaId;
        var areaName;
        //if ($("#dllArea").val() != "0") {
        //    areaId = $("#dllArea").val();
        //    areaName = $("#dllArea").find("option:selected").text();
        //} else
        if ($("#dllCity").val() != "0") {
            areaId = $("#dllCity").val();
            areaName = $("#dllCity").find("option:selected").text();
        }
        else if ($("#dllDrovince").val() != "0") {
            areaId = $("#dllDrovince").val();
            areaName = $("#dllDrovince").find("option:selected").text();
        } else {
            bntToolkit.error("请选择区域");
            return false;
        }
       
        $("#AreaBox").append("<label><input name=\"AreaId\" type=\"checkbox\" class=\"ace\" checked value=\"" + areaId + "\"><span class=\"lbl\"> " + areaName + "</span></label>");
    });


});

function success(result, statusText, xhr, $form) {
    if (!result.Success) {
        bntToolkit.error(result.ErrorMessage);
    } else {
        location.href = url_loadPage;
    }
}
