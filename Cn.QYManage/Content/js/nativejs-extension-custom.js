Date.prototype.format = function (fmt) { //author: meizz 
    //console.log(this);
    var o = {
        "M+": this.getMonth() + 1,                 //�·�   
        "d+": this.getDate(),                    //��   
        "h+": this.getHours(),                   //Сʱ   
        "m+": this.getMinutes(),                 //��   
        "s+": this.getSeconds(),                 //��   
        "q+": Math.floor((this.getMonth() + 3) / 3), //����   
        "S": this.getMilliseconds()             //����   
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

Date.prototype.getDateYouWant = function (type, i) { //author: pmaxloo 
    switch (type) {
        case 'y': this.setFullYear(this.getFullYear() + i); break;
        case 'm': this.setMonth(this.getMonth() + i); break;
        case 'd': this.setDate(this.getDate() + i); break;
    }
    return this;
}

Number.prototype.formatRateShow = function () {
    var a = parseFloat(this) * 8;
    if (a > 1024 * 1024)//G
    {
        return (a / 1024 / 1024).toFixed(2) + "G";
    }
    else if (a > 1024) {
        return (a / 1024).toFixed(2) + "M";
    }
    else {
        return a + "k";;
    }
}

// ��ʽ��������ʱ���ʽ
Number.prototype.formatTime = function () {
    // ����
    var h = 0, i = 0, s = parseInt(this);
    if (s > 60) {
        i = parseInt(s / 60);
        s = parseInt(s % 60);
        if (i > 60) {
            h = parseInt(i / 60);
            i = parseInt(i % 60);
        }
    }
    // ����
    var zero = function (v) {
        return (v >> 0) < 10 ? "0" + v : v;
    };
    return [zero(h), zero(i), zero(s)].join(":");
};

//  ����ɾ��Ԫ��
Array.prototype.remove = function (val) {
    var index = this.indexOf(val);
    if (index > -1) {
        this.splice(index, 1);
    }
};

window.showErrorDialog = function (title, content) {
    if (title.length <= 0) title = "�д�����";

    content = "<i class=\"fa fa-warning fa-fw\"></i>" + content;

    var d = dialog({
        title: title,
        content: content
    });
    d.show();
    return d;
}

window.createDialog = function () {
    var d = dialog({
        width: 60,
        height: 60
    });
    d.show();
    d.showError = function (title, content) {
        if (title.length <= 0) title = "�д�����";
        content = "<i class=\"fa fa-times-circle\"></i>&nbsp;" + content;

        this.width('auto');
        this.height('auto');
        this.title(title);
        this.content(content);
        this.show();
        return this;
    }
    d.showTip = function (content, timeout) {
        content = "<i class=\"fa fa-info\"></i>&nbsp;" + content;
        this.width('auto');
        this.height('auto');
        this.content(content);
        this.show();
        var d = this;
        if (timeout != null && timeout != undefined) {
            setTimeout(function () {
                d.close().remove();
            }, timeout);
        }
        return this;
    }
    return d;
}