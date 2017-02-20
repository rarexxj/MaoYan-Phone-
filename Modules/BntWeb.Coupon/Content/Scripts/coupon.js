jQuery(function ($) {
   
    var title = $("#Title").val();
    var money = $("#Money").val();
    var minimum = $("#Minimum").val();
    var couponType = $("#CouponType").val();
  
    var loadTable = $('#CouponTable').dataTable({
        "processing": true,
        "serverSide": true,
        "sorting": [[3, "desc"]],
        "ajax": {
            "url": url_loadPage,
            "data": function (d) {
                //添加额外的参数传给服务器
                d.extra_search = {
                    "Title": title, "Money": money, "Minimum": minimum, "CouponType": couponType
                  
                };
            }
        },
        "aoColumns":
        [
            { "mData": "Title", 'sClass': 'left' },
            { "mData": "Describe", 'sClass': 'left' },
            { "mData": "Money", 'sClass': 'left' },
            { "mData": "Term", 'sClass': 'left' },
            { "mData": "Minimum", 'sClass': 'left' },
            { "mData": "Code", 'sClass': 'left' },
             {
                 "mData": "Enabled", 'sClass': 'center',
                 "mRender": function (data, type, full) {
                     if (data) {
                         return '<span class="label label-sm label-success">启用</span>';
                     }
                     return '<span class="label label-sm label-danger">禁用</span>';
                 }
             },

           {
               "mData": "CouponType", 'sClass': 'left',
               "mRender": function(data, type, full) {
                   if (data === 0) return "满减";
                   if (data === 1) return "立减";
               }

           },
                        {
                            "mData": "CreateTime", 'sClass': 'left',
                            "mRender": function (data, type, full) {
                                if (data != null && data.length > 0) {
                                    return eval('new ' + data.replace(/\//g, '')).Format("yyyy-MM-dd hh:mm");
                                }
                                return "";
                            }
                        },
                          {
                              "mData": "Enabled",
                              'sClass': 'center',
                              "sWidth": "30px",
                              "orderable": false,
                              "mRender": function (data, type, full) {
                                  var render = '<div class="visible-md visible-lg hidden-sm hidden-xs action-buttons">';

                                  if (data) {
                                      render += '<a class="red switch" data-id="' + full.Id + '" data-value="off" href="#" title="禁用">禁用</a>';
                                  } else {
                                      render += '<a class="green switch" data-id="' + full.Id + '" data-value="on" href="#" title="启用">启用</a>';

                                  }
                                  render += '<a class="blue" href="' + url_editCoupon + '?id=' + full.Id + '" title="编辑">编辑</a>';
                                      render += '<a class="red delete" data-id="' + full.Id + '" href="#" title="删除"><i class="icon-trash bigger-130"></i></a>';

                                  render += '</div>';
                                  return render;
                              }
                          }

        ]
    });
    //查询
    $('#QueryButton').on("click", function () {

         title = $("#Title").val();
         money = $("#Money").val();
         minimum = $("#Minimum").val();
         couponType = $("#CouponType").val();
        loadTable.api().ajax.reload();
    });

    $('#CouponTable').on("click", ".switch", function (e) {
        var id = $(this).data("id");
        var val = $(this).data("value");
        bntToolkit.post(url_switchCoupon, { couponId: id, enabled: val === "off" }, function (result) {
            if (result.Success) {
                $("#CouponTable").dataTable().fnDraw();
            } else {
                bntToolkit.error(result.ErrorMessage);
            }
        });
    });
    $('#CouponTable').on("click", ".delete", function (e) {
        var id = $(this).data("id");

        bntToolkit.confirm("删除后将无法恢复，确定还要删除吗？", function () {
            bntToolkit.post(url_deleteCoupon, { couponId: id }, function (result) {
                if (result.Success) {
                    $("#CouponTable").dataTable().fnDraw();
                } else {
                    bntToolkit.error(result.ErrorMessage);
                }
            });
        });
    });

});