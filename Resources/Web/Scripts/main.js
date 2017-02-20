/*
 
*/
$(function () {

});
// ����
var regp = /^1(3|4|5|7|8)\d{9}$/; // /^1\d{10}$/;//�ֻ�����
var regnum = /^\d+\.?\d*$/; //����
var regw = /^\w{6,18}$/; //����


// �����۽�
function navfocus(num1, num2) {
    $(".header .bel li").eq(num1).addClass("cur").siblings("li").removeClass("cur");
}

// �����Ŀ�۽�
function menufocus(num1, num2) {
    var opar = $(".sm-lst dl").eq(num1);
    opar.addClass("cur");
    opar.find("dd").stop(true, true).slideDown();
    opar.find("dd a").eq(num2).addClass("cur");
}

//���ظ���
function loadDataByBtn(url, param, pageIndex, $dom, $more, isRefresh, itemClassName, totalCount) {
    $.post(url, $.extend({}, param, { "pageIndex": pageIndex }), function (data) {
        if (isRefresh) {
            $dom.empty();
            $more.attr("pageindex", "1");
        }
        var className = "." + itemClassName;
        if (data == "") {
            $more.hide();
            return;
        }
        $dom.append(data);
        var itemLength = $dom.find(className).length;
        if (itemLength >= totalCount) {
            $more.hide();
        }
        else {
            $more.show();
        }
    });
}

// �Ƿ��ѵ�¼
function islogin() {
    var token = getcookie("token");
    if (getcookie("token") != "") {
        return true;
    } else {
        return false;
    }
}

// �˳���¼
function exit() {
    $.MsgBox.Confirm("��ʾ", "ȷ��Ҫ�˳���", function () {
        $.ajax({
            url: '/Member/LogOff',
            cache: false,
            type: 'post'
        }).done(function (data) {
            if (data.Success) {
                clearcookie();
                location.href = "/login.html";
            }
        }).fail(function (data) {

        });
    });
}

//setcookie
function setcookie(name, value, flag) {
    if (flag) {
        $.cookie(name, value, { expires: 7, path: '/' });
    } else {
        var expiresTime = new Date();
        expiresTime.setTime(expiresTime.getTime() + (2 * 60 * 60 * 1000));//Ĭ��2Сʱ
        $.cookie(name, value, { expires: expiresTime, path: '/' });
    }
}
//gecookie
function getcookie(name) {
    var val = $.cookie(name);
    if (val && val != "null") {
        return val;
    } else {
        return "";
    }
}

//���cookie
function clearcookie() {
    $.cookie("UserName", null, { path: "/" });
    $.cookie("PhoneNumber", null, { path: "/" });
    $.cookie("Id", null, { path: "/" });
    $.cookie("token", '', { path: "/" });
    $.cookie("UnReadMessageCount", "", { path: "/" });
    $.cookie("CreateTime", "", { path: "/" });
    $.cookie("NickName", "", { path: "/" });
    $.cookie("Sex", "", { path: "/" });
    $.cookie("QQ", "", { path: "/" });
    $.cookie("Level", "", { path: "/" });
    $.cookie("RealName", "", { path: "/" });
    $.cookie("MediumThumbnail", "", { path: "/" });
    $.cookie("Avatar", "", { path: "/" });
    $.cookie("RelativePath", "", { path: "/" });
    $.cookie("SmallThumbnail", "", { path: "/" });
}

// �Ƿ�Ϊ��
function isempty(str, obj) {
    if (!$.trim(str)) {
        if (obj) {
            obj.parent().addClass("err");
            obj.focus();
        }
        return false;
    } else {
        if (obj) {
            obj.parent().removeClass("err");
        }
    }
    return true;
}
// select ѡ�е�ֵ�Ƿ�Ϊ�գ�objΪѡ�е���option
function isSelectEmpty(str, obj) {
    if (!$.trim(str)) {
        if (obj) {
            obj.parent().parent().addClass("err");
            obj.focus();
        }
        return false;
    } else {
        if (obj) {
            obj.parent().parent().removeClass("err");
        }
    }
    return true;
}
// ��֤
function testd(str, reg, obj) {
    if (!reg.test(str)) {
        if (obj) {
            obj.parent().addClass("err");
            obj.focus();
            return false;
        }
    } else {
        if (obj) {
            obj.parent().removeClass("err");
        }
    }
    return true;
}

//��Ȩʧ�ܺ�ִ��
function afterfail(url) {
    $.MsgBox.Tip("��ʾ", "���¼", true);
    clearcookie();
    setTimeout(function () {
        if (url) {
            location.href = '/login.html?returnUrl=' + url;
        } else {
            location.href = '/login.html';
        }
    }, 1000);
}

function getTimeago(datetime) {
    var minute = 1000 * 60;
    var hour = minute * 60;
    var day = hour * 24;
    var halfamonth = day * 15;
    var month = day * 30;


    var dateTimeStamp = Date.parse(datetime.replace(/-/gi, "/"));

    var now = new Date().getTime();
    var diffValue = now - dateTimeStamp;
    if (diffValue < 0) {
        //�����ڲ����򵯳����ڸ�֮
        //alert("�������ڲ���С�ڿ�ʼ���ڣ�");
    }
    var monthC = diffValue / month;
    var weekC = diffValue / (7 * day);
    var dayC = diffValue / day;
    var hourC = diffValue / hour;
    var minC = diffValue / minute;
    var result = datetime;
    if (monthC >= 1) {
        result = parseInt(monthC) + "����ǰ";
    } else if (weekC >= 1) {
        result = parseInt(weekC) + "��ǰ";
    } else if (dayC >= 1) {
        result = parseInt(dayC) + "��ǰ";
    } else if (hourC >= 1) {
        result = parseInt(hourC) + "��Сʱǰ";
    } else if (minC >= 1) {
        result = parseInt(minC) + "����ǰ";
    } else
        result = "�ո�";
    return result;

}