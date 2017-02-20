//弹窗样式
//默认1.5s后消失
function my_alert(b, time, callback) {
	var div = document.createElement('div');
	$("body").append(div);
	var $div = $(div);
	div.className = "tanchuang";
	div.innerHTML = "<span>" + b + "</span>";
	$div.css("opacity", 1);

	$div.stop().animate({
		opacity: 1

	}, 1000, function() {

		$div.stop().animate({
			opacity: 0

		}, 500, function() {
			$div.hide();
			$div.css("opacity", 1);
			$(".tanchuang").remove();
		})

	});

	setTimeout(function() {

		if(typeof(callback) == 'function') {
			callback()
		} else {

		}

	}, time * 1000)
}




   //获取URL上参数
    $.getUrlParam = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)")
        var r = window.location.search.substr(1).match(reg)
        if (r != null) return unescape(r[2])
        return null
    }
    //移除LOADING
    $.RMLOAD = function () {

        (!$('.new-loading').length) || $('.new-loading').remove();
      
    }
    //添加LOADING
    $.ADDLOAD = function () {
        var html = '<div class="new-loading"><ul class="small-loading"><li></li><li></li><li></li><li></li><li></li><li></li><li></li><li></li></ul></div>'
        if (!$('.new-loading').length) {
            $('body').append(html);
        }
    }

//	前端表单逻辑逻辑验证
//	手机
function iphone(element) {

	if((/^1[3|4|5|7|8]\d{9}$/.test($(element).val()))) {
		return true;
	} else {
		return false;
	}
}
//		密码
function mima(element) {
	var myReg = /^[a-zA-Z0-9]{6,20}$/;
	if(myReg.test($(element).val())) {
		return true;
	} else {
		return false;
	}
}
//	重复密码	
function againPassword(element1, element2) {
	var myReg = /^[a-zA-Z0-9]{6,20}$/;
	var value = $(element1).val();
	var value1 = $(element2).val();
	if(myReg.test(value1) && (value1 == value)) {
		return true;
	} else {
		if(value1 != value) {
			$(".yan_cuo").show().html("两次密码不相同，请重新输入");
			$(".tishi").show().html("两次密码不相同，请重新输入");
//			my_alert("两次密码不相同，请重新输入")
			return false;
		} else {
			$(".yan_cuo").show().html("请输入六位字母或数字以上组合密码");
			$(".tishi").show().html("请输入六位字母或数字以上组合密码");
//			my_alert("请输入六位字母或数字以上组合密码");
			return false;
		}

	}
}
function againPassword1(element1, element2) {
	var myReg = /^[a-zA-Z0-9]{6,20}$/;
	var value = $(element1).val();
	var value1 = $(element2).val();
	if(myReg.test(value1) && (value1 == value)) {
		return true;
	} else {
		if(value1 != value) {
		
			my_alert("两次密码不相同，请重新输入")
			return false;
		} else {
	
			my_alert("请输入六位字母或数字以上组合密码");
			return false;
		}

	}
}
//	身份证
function shenfengzheng(element) {
	var myReg = /^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}([0-9]|X)$/;
	if(myReg.test($(element).val())) {
		return true;
	} else {
		return false;
	}
}

//		短信验证码
function yanzheng(element) {
	var value = $(element).val();
	if(value == "" || (/^\s+$/g.test(value))) {

		return false;

	} else {
		return true;
	}

}

//		图形验证码
function yanzheng_num(element) {
	var value = $(element).val();
	if(value == "" || (/^\s+$/g.test(value))) {

		return false;

	} else {
		return true;
	}

}

//		非空判断
function noNull(element) {
	var value = $(element).val();
	if(value == "" || (/^\s+$/g.test(value))) {

		return false;

	} else {
		return true;
	}

}
//		正整数与0
function positive(element) {

	var myReg = /^\d+$/;
	var value = $(element).val();
	if(myReg.test(value)) {

		return true;

	} else {

		return false;
	}

}




//启动和关闭遮罩
var loading;
var bntLoading = {
    show: function () {
        loading = layer.load(2, {
            shade: [0.1, '#aaaaaa'] //0.1透明度的白色背景
        });
    },
    hide: function () {
        layer.close(loading);
    }
};

//ajax全局监听，
var unload = false;
$(document).ajaxStart(function () {
    if (unload == undefined || unload == false) {
        bntLoading.show();
    }
});
$(document).ajaxComplete(function () {
    if (unload == undefined || unload == false) {
        bntLoading.hide();
    }
});
$(document).ajaxError(function () {
    if (unload == undefined || unload == false) {
        bntLoading.hide();
    }
});