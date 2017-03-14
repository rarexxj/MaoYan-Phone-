
    $(document).ready(function() {

        //导航点击
        $('.index-headnavmainul .index-navli').hover(function() {
            $(this).addClass('active');
            $(this).siblings().removeClass('active');
            $(this).children('.index-navtwo').stop().slideToggle();
        });
        //	样式更改
        $(".order_feilei li").each(function () {
        
            $(this).find(".daifu").height($(this).find(".order_xinxi").height());
        });

        //取消订单
        $(".order_word").on('click', function() {
            var obj = $(this);
            var orderId = obj.data("id");
            var url_cacelOrder = $(this).data('url');
            if (confirm("您确定要取消订单吗？")) {
                $.ajax({
                    url: url_cacelOrder,
                    type: "post",
                    data: { orderId: orderId, d: Date.now() },
                    success: function(data) {
                        if (data.Success) {
                          
                            location.href = url_myorder;

                        } else {
                            alert(data.ErrorMessage);
                        }
                    },
                    error: function() {}
                });
            }

        });
        //搜索
        $(".order_seach").on('click', function () {
            var key = $("#search").val();
            location.href = url_myorder + "?keywords=" + key;
        });
        //订单详情
        $(".order_info").on('click', function() {

            location.href = $(this).data('url');
        });
        //确认收货
        $("#Delivery").on('click', function() {
            var url = $(this).data('url');
            var obj = $(this);
            var orderId = obj.data("id");
            if (confirm("您确定要确认收货订单吗？")) {
                $.ajax({
                    url: url,
                    type: "post",
                    data: { orderId: orderId, d: Date.now() },
                    success: function(data) {
                        if (data.Success) {
                            location.href = url;
                        } else {
                            alert(data.ErrorMessage);
                        }
                    },
                    error: function() {}
                });
            }
    });
      
        //订单模块切换
        $('.order_nav li').on('click', function () {
          
            var value = $(this).data('status');
            if (value === 0) {
                location.href = url_myorder;
            }
            if (value === 1) {
                location.href = url_myorder + "?status=0";
            }
            if (value === 2) {
                location.href = url_myorder + "?status=2";
            }
            if (value === 3) {
                location.href = url_myorder + "?status=3&evaluateStatus=0";
            }
            if (value === 4) {
                location.href = url_myorder + "?refundStatus=1";
            }
            $(this).siblings().removeClass('active');
            $(this).addClass('active');
        });
      
    });
