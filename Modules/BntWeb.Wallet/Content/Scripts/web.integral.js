﻿











    //积分切换
     
        $(this).siblings().removeClass('active');
        var status = $(this).data('status');
        location.href = url_myIntagral + "?billType=" + status;
    });


