﻿jQuery(function ($) {

    var options = $.datepicker.regional["zh-CN"];
    options["dateFormat"] = "yy-mm-dd";
    $("#CreateTimeBegin").datepicker(options);
    $("#CreateTimeEnd").datepicker(options);

    var orderNo = $("#OrderNo").val();
    var consignee = $("#Consignee").val();
    var phone = $("#Phone").val();
    var orderStatus = $("#OrderStatus").val();
    var refundStatus = $("#RefundStatus").val();
    var payStatus = $("#PayStatus").val();
    var shippingStatus = $("#ShippingStatus").val();
    var createTimeBegin = $("#CreateTimeBegin").val();
    var createTimeEnd = $("#CreateTimeEnd").val();

    var goodsTable = $('#OrdersTable').dataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": url_loadPage,
            "data": function (d) {
                //添加额外的参数传给服务器
                d.extra_search = { "OrderNo": orderNo, "Consignee": consignee, "Phone": phone, "OrderStatus": orderStatus, "RefundStatus": refundStatus, "PayStatus": payStatus, "ShippingStatus": shippingStatus, "CreateTimeBegin": createTimeBegin, "CreateTimeEnd": createTimeEnd };
            }
        },
        "sorting": [[1, "desc"]],
        "aoColumns":
		[
			{ "mData": "OrderNo", 'sClass': 'left' },
			{
			    "mData": "CreateTime", 'sClass': 'left',
			    "mRender": function (data, type, full) {
			        var render = '<strong>' + full.MemberName + "</strong><br/>";
			        if (data != null && data.length > 0) {
			            render += eval('new ' + data.replace(/\//g, '')).Format("yyyy-MM-dd hh:mm:ss");
			        }
			        return render;
			    }
			},
			{
			    "mData": "Consignee", 'sClass': 'left', "mRender": function (data, type, full) {
			        var render = '<strong>' + full.Consignee + '</strong> [Tel:' + full.Tel + ']<br/>' + (full.PCDS != null ? (full.PCDS + '<br/>') : '') + full.Address ;
			        if (full.BestTime != null) {
			            render += '<br/><strong>最佳送货时间:</strong>' + full.BestTime;
			        }
			        return render;
			    }
			},
			{ "mData": "OrderAmount", 'sClass': 'center', "sWidth": "200px", "orderable": false, "mRender": function(data, type, full) {
			    return '商品总价：￥' + data.toFixed(2) + '<br/> + 物流费用：￥' + full.ShippingFee.toFixed(2) + '<br/> - 积分折抵：￥' + full.IntegralMoney.toFixed(2);
			} },
			{ "mData": "PayFee", 'sClass': 'center', "sWidth": "110px", "mRender": function (data, type, full) { return '￥' + data.toFixed(2); } },
			{
			    "mData": "OrderStatus", 'sClass': 'center', "sWidth": "250px", "orderable": false, "mRender": function (data, type, full) {
			        var render = '';
			        switch (full.OrderStatus) {
			            case 0:
			                render += '待付款';
			                break;
			            case 1:
			                render += '待发货';
			                break;
			            case 2:
			                render += '待收货';
			                break;
			            case 3:
			                render += '已完成';
			                break;
			            case 4:
			                render += '<span class="red">已关闭</span>';
			                break;
			            case 5:
			                render += '退款中';
			                break;

			            default:
			        }

			        switch (full.RefundStatus) {
			            case 1:
			                render += '<br/><span class="red">退款中</span>';
			                break;
			            case 2:
			                render += '<br/><span class="red">已退款</span>';
			                break;
			            default:
			        }

			        switch (full.EvaluateStatus) {
			            case 1:
			                render += '<br/><span class="red">已评价</span>';
			                break;
			            case 2:
			                render += '<br/><span class="red">已回复</span>';
			                break;
			            default:
			                break;
			        }
			        

			        //switch (full.PayStatus) {
			        //    case 0:
			        //        render += '未付款,';
			        //        break;
			        //    case 1:
			        //        render += '付款中,';
			        //        break;
			        //    case 2:
			        //        render += '已付款,';
			        //        break;

			        //    default:
			        //}

			        //switch (full.ShippingStatus) {
			        //    case 0:
			        //        render += '未发货';
			        //        break;
			        //    case 1:
			        //        render += '已发货';
			        //        break;
			        //    case 2:
			        //        render += '已取消';
			        //        break;

			        //    default:
			        //}

			        return render;
			    }
			},
		    {
		        "mData": "Id", 'sClass': ' center', "orderable": false,
		        "sWidth": "200px",
		        "mRender": function (data, type, full) {
		            var render = '<div class="visible-md visible-lg hidden-sm hidden-xs action-buttons">';

		            render += '<a class="green" href="' + url_detail + '?orderId=' + full.Id + '" title="查看"><i class="icon-eye-open bigger-130"></i></a>';
		            if (full.OrderStatus == 3|| full.OrderStatus ==4) {
		                render += '<a class="red delete" data-id="' + full.Id + '" href="#" title="删除"><i class="icon-trash bigger-130"></i></a>';
		                //render += '<a class="red" href="' + url_delete + '?orderId=' + full.Id + '" title="删除"><i class="icon-trash bigger-130"></i></a>';
		            }
		            render += '</div>';
		            return render;
		        }

		    }
		]
    });

    //查询
    $('#QueryButton').on("click", function () {
        orderNo = $("#OrderNo").val();
        consignee = $("#Consignee").val();
        phone = $("#Phone").val();
        orderStatus = $("#OrderStatus").val();
        refundStatus = $("#RefundStatus").val();
        payStatus = $("#PayStatus").val();
        shippingStatus = $("#ShippingStatus").val();
        createTimeBegin = $("#CreateTimeBegin").val();
        createTimeEnd = $("#CreateTimeEnd").val();
        goodsTable.api().ajax.reload();
    });

    $('#OrdersTable').on("click", ".delete", function (e) {
        var id = $(this).data("id");

        bntToolkit.confirm("确定还要删除该订单吗？", function () {
            bntToolkit.post(url_delete, { orderId: id }, function (result) {
                if (result.Success) {
                    $("#OrdersTable").dataTable().fnDraw();
                } else {
                    bntToolkit.error(result.ErrorMessage);
                }
            });
        });
    });
});