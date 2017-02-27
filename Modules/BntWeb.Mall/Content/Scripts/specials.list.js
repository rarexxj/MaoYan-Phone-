jQuery(function ($) {
    var status = $("#Status").val();
    var name = $("#Name").val();
    var goodsNo = $("#GoodsNo").val();
    var specialType = $("#SpecialType").val();

    var specialGoodsTable = $('#SpecialGoodsTable').dataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": url_loadPage,
            "data": function (d) {
                //添加额外的参数传给服务器
                d.extra_search = { "Name": name, "Status": status, "GoodsNo": goodsNo, "SpecialType": specialType };
            }
        },
        "sorting": [[1, "desc"]],
        "aoColumns":
		[
		    {
		        "mData": "Id", 'sClass': ' center', "orderable": false,
		        "mRender": function (data, type, full) {
		            var render = '<label class="inline"><input type="checkbox" class="ace" name="single-goods" data-id="' + full.Id + '"><span class="lbl"></span></label>';
		            return render;
		        }
		    },
			{ "mData": "Name", 'sClass': 'left' },
			{ "mData": "GoodsNo", 'sClass': 'left' },
            { "mData": "ExchangeIntegral", 'sClass': 'left' },
			{ "mData": "ShopPrice", 'sClass': 'left' },
			{
			    "mData": "Status", 'sClass': 'center',
			    "mRender": function (data, type, full) {
			        if (data == 1) {
			            return '<span class="label label-sm label-success">在售</span>';
			        }
			        return '<span class="label label-sm label-danger">未上架</span>';
			    }
			},
			{ "mData": "Stock", 'sClass': 'left' },
			{
			    "mData": "SpecialType", 'sClass': 'center',
			    "mRender": function (data, type, full) {
			        if (data ===1) {
			            return '<span class="label label-sm label-success">自选</span>';
			        }
                    else if (data === 2) {
			            return '<span class="label label-sm label-danger">加价购</span>';
			        } else if(data===3) {
                        return '<span class="label label-sm label-danger">积分换购</span>';
			        }
			        else if (data === 4) {
			            return '<span class="label label-sm label-danger">鲜花专区</span>';
			        }
			    }
			},
		    {
		        "mData": "Id", 'sClass': ' center', "orderable": false,
		        "sWidth": "200px",
		        "mRender": function (data, type, full) {
		            var render = '<div class="visible-md visible-lg hidden-sm hidden-xs action-buttons">';
		            if (full.Status == 1) {
		                render += '<a class="red switch" data-id="' + full.Id + '" data-value="off" href="#" title="下架"><i class="icon-circle bigger-130"></i></a>';
		            }
		            else if (full.Status == 0) {
		                render += '<a class="green switch" data-id="' + full.Id + '" data-value="on" href="#"  title="上架"><i class="icon-circle-blank bigger-130"></i></a>';
		            }
		            render += '<a class="blue" href="' + url_editGoods + '?id=' + full.Id + '" title="编辑"><i class="icon-pencil bigger-130"></i></a>';
		            render += '<a class="red delete" data-id="' + full.Id + '" href="#" title="删除"><i class="icon-trash bigger-130"></i></a>';
		            render += '</div>';
		            return render;
		        }
		    }
		]
    });


    //查询
    $('#QueryButton').on("click", function () {
        name = $("#Name").val();
        goodsNo = $("#GoodsNo").val();
        status = $("#Status").val();
         specialType = $("#SpecialType").val();
        specialGoodsTable.api().ajax.reload();
    });

    $('#SpecialGoodsTable').on("click", ".delete", function (e) {
        var id = $(this).data("id");

        bntToolkit.confirm("删除商品会使产品强制下架，确定还要删除该商品吗？", function () {
            bntToolkit.post(url_deleteGoods, { goodsId: id }, function (result) {
                if (result.Success) {
                    $("#SpecialGoodsTable").dataTable().fnDraw();
                } else {
                    bntToolkit.error(result.ErrorMessage);
                }
            });
        });
    });

    $('#SpecialGoodsTable').on("click", ".switch", function (e) {
        var id = $(this).data("id");
        var value = $(this).data("value");
        var url = "";
        if (value == "off")
            url = url_NotInSaleGoods;
        else {
            url = url_InSaleGoods;
        }
        bntToolkit.post(url, { id: id }, function (result) {
            if (result.Success) {
                $("#SpecialGoodsTable").dataTable().fnDraw();
            } else {
                bntToolkit.error(result.ErrorMessage);
            }
        });
    });

    $("input[name='toggle-all']").on("click", function () {
        $('input[name="single-goods"]').prop("checked", $('input[name="toggle-all"]').prop("checked"));
    });

    //$('#SpecialGoodsTable').on("click", 'input[name="single-goods"]', function (e) {
    //    if ($('input[name="single-goods"]:checked').length == $('input[name="single-goods"]').length) {
    //        $("input[name='toggle-all']").prop("checked", true);
    //    } else {
    //        $("input[name='toggle-all']").prop("checked", false);
    //    }
    //});

    $("#batch-delete").on("click", function () {
        if ($('input[name="single-goods"]:checked').length === 0) {
            bntToolkit.error("请选择要删除的商品");
            return false;
        }

        //var ids = [];
        //$('input[name="single-goods"]:checked').each(function () {
        //    ids.push($(this).data("id"));
        //});

        bntToolkit.confirm("删除商品会使产品强制下架，确定还要删除该商品吗？", function () {
            bntToolkit.post(url_batchDeleteGoods, { goodsIds: ids }, function (result) {
                if (result.Success) {
                    $("#SpecialGoodsTable").dataTable().fnDraw();
                } else {
                    bntToolkit.error(result.ErrorMessage);
                }
            });
        });
    });
});