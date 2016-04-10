define(function (require, exports, module) {
    //  js类库
    require("js/mousetrap.min");
    require("js/angular-hotkeys/hotkeys");
    require("dialog");
    require("js/json2.js");
    require("js/bootDirective/transition/transition.js?");
    require("js/bootDirective/position/position.js?");
    require("js/bootDirective/collapse/collapse.js?");
    require("js/bootDirective/alert/alert.js?");
    require("js/bootDirective/pagination/pagination.js?");
    require("js/bootDirective/modal/modal.js?");
    require("js/bootDirective/accordion/accordion.js?");
    require("js/bootDirective/typeahead/bootstrap-typeahead.js?");
    require("js/bootDirective/tabs/tabs.js?v=1.1");
    require("js/angular-contenteditable.js?v=1.0");
    require("js/angular-dragdrop.min.js");
    require("js/angular-tree-control-master/angular-tree-control.js");
    require("js/angular-route.min.js");
    require("js/My97DatePicker/WdatePicker.js");
    require("pages/parameters.js");
    require("js/select2.js");

    //  原生js拓展方法
    require("js/nativejs-extension-custom.js");

    var app = angular.module("app", ['ui.bootstrap', 'ui.bootstrap.modal', 'ui.bootstrap.collapse', 'ui.bootstrap.tabs'
        , 'cfp.hotkeys', 'contenteditable', 'ngDragDrop', 'treeControl']);

    //网上的例子有问题，factory不能注入$scope，只能注入$rootScope，可以理解为factory为一个全局变量
    app.factory('alertbox', ['$rootScope', function ($rootScope) {
        $rootScope.alerts = [];
        $rootScope.closeAlert = function (index) {
            $rootScope.alerts.splice(index, 1);
        };
        $rootScope.addAlert = function (alert) {
            $rootScope.alerts.push(alert);
        };
        return {
            addAlert: function (alert) {
                $rootScope.addAlert(alert);
            },
            clearAll: function () {
                $rootScope.alerts.length = 0
            },
            createAlert: function () {
                var alert = {
                    closeable: 'true',
                    info: 'info',
                    msg: '',
                    isshow: false,
                    showSuccess: function (msg) {
                        this.msg = msg;
                        this.type = 'success';
                        this.isshow = true;
                    },
                    showDanger: function (msg) {
                        this.msg = msg;
                        this.type = 'danger';
                        this.isshow = true;
                    },
                    showInfo: function (msg) {
                        this.msg = msg;
                        this.type = 'info';
                        this.isshow = true;
                    }
                };
                $rootScope.addAlert(alert);
                return alert;
            },
            closeAlert: function (index) {
                $rootScope.closeAlert(index);
            }
        };
    }]);

    app.directive('select2', function () {
        return {
            restrict: 'A',
            scope: {
                config: '=',
                ngModel: '=',
                select2Model: '='
            },
            link: function (scope, element, attrs) {
                // 初始化
                var tagName = element[0].tagName,
                    config = {
                        allowClear: true,
                        multiple: !!attrs.multiple,
                        placeholder: attrs.placeholder || ' '   // 修复不出现删除按钮的情况
                    };

                // 生成select
                if (tagName === 'SELECT') {
                    // 初始化
                    var $element = $(element);
                    delete config.multiple;

                    $element
                        .prepend('<option value=""></option>')
                        .val('')
                        .select2(config);

                    return false;
                }

            }
        }
    });


    app.directive('yearPicker', function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            scope: {
                minDate: '@'
            },
            link: function (scope, element, attr, ngModel) {

                element.val(ngModel.$viewValue);

                function onpicking(dp) {
                    var date = dp.cal.getNewDateStr();
                    scope.$apply(function () {
                        ngModel.$setViewValue(date);

                        //alert(ngModel.$viewValue);
                    });
                }

                element.bind('click', function () {
                    WdatePicker({
                        onpicking: onpicking,
                        dateFmt: 'yyyy'
                        //minDate: (scope.minDate || '%y-%M-%d')
                    })
                });
            }
        };
    });

    app.directive('monthPicker', function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            scope: {
                minDate: '@'
            },
            link: function (scope, element, attr, ngModel) {

                element.val(ngModel.$viewValue);

                function onpicking(dp) {
                    var date = dp.cal.getNewDateStr();
                    scope.$apply(function () {
                        ngModel.$setViewValue(date);

                        //alert(ngModel.$viewValue);
                    });
                }

                element.bind('click', function () {
                    WdatePicker({
                        onpicking: onpicking,
                        dateFmt: 'yyyy-MM'
                        //minDate: (scope.minDate || '%y-%M-%d')
                    })
                });
            }
        };
    });

    app.directive('dateFormat', ['$filter', function ($filter) {
        var dateFilter = $filter('date');
        return {
            require: 'ngModel',
            link: function (scope, elm, attrs, ctrl) {
                function formatter(value) {
                    return dateFilter(value, 'yyyy-MM-dd'); //format  
                }
                function parser() {
                    return ctrl.$modelValue;
                }
                ctrl.$formatters.push(formatter);
                ctrl.$parsers.unshift(parser);
            }
        };
    }]);

    app.directive('datePicker', function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            scope: {
                minDate: '@'
            },
            link: function (scope, element, attr, ngModel) {
                element.val(ngModel.$viewValue);
                function onpicking(dp) {
                    var date = dp.cal.getNewDateStr();
                    scope.$apply(function () {
                        ngModel.$setViewValue(date);
                    });
                }
                element.bind('click', function () {
                    WdatePicker({
                        onpicking: onpicking,
                        dateFmt: 'yyyy-MM-dd'
                        //minDate: (scope.minDate || '%y-%M-%d')
                    })
                });
                element.on('blur keyup change', function () {
                    var val = element.val();
                    ngModel.$setViewValue(val);
                });
            }
        };
    });

    app.directive('dateTimePicker', function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            scope: {
                minDate: '@'
            },
            link: function (scope, element, attr, ngModel) {

                element.val(ngModel.$viewValue);

                function onpicking(dp) {
                    var date = dp.cal.getNewDateStr();
                    scope.$apply(function () {
                        ngModel.$setViewValue(date);

                        //alert(ngModel.$viewValue);
                    });
                }

                element.bind('click', function () {
                    WdatePicker({
                        onpicking: onpicking,
                        dateFmt: 'yyyy-MM-dd HH:mm:ss'
                        //minDate: (scope.minDate || '%y-%M-%d')
                    })
                });
            }
        };
    });

    app.directive('clickShowBubble', ['$http', function ($http) {
        return {
            restrict: 'A',
            compile: function (tElem, attrs) {
                return function (scope, elem, attrs) {
                    //alert("a");
                    var d = null;
                    elem.click(function () {
                        if (d == null) {
                            var contentId = elem.attr("click-show-bubble");
                            var c = $(contentId);
                            var align = elem.attr("bubble-align");
                            var d = dialog({
                                align: align,
                                content: c,
                                quickClose: true,
                                cancelValue: '取消',
                                cancelDisplay: false,
                                cancel: function () {
                                    this.close();
                                    return false;
                                }
                            });
                        }
                        d.show(elem[0]);
                    });
                }
            }
        };
    }]);

    app.directive('clickOrder', ['$rootScope', function ($rootScope) {
        return {
            restrict: 'A',
            scope: {
                c: '@',
                orderName: '@clickOrder',
                clickOrderGroup: '=',
                orderType: "@"
            },
            replace: false,
            transclude: true,
            template: '<b ng-transclude></b><i class="fa {{c}} pull-right"></i>',
            compile: function (tElem, attrs) {
                tElem.css("cursor", "pointer");
                return function (scope, elem, attrs) {
                    //alert(scope.clickOrderGroup.cols.length);
                    var group = scope.clickOrderGroup;
                    scope.c = 'fa-sort';
                    group.cols.push(scope);
                    elem.click(function () {
                        if (scope.orderType == "desc") {
                            scope.c = "fa-sort-up";
                            scope.orderType = "asc";
                        }
                        else {
                            scope.c = "fa-sort-down";
                            scope.orderType = "desc";
                        }
                        angular.forEach(scope.clickOrderGroup.cols, function (value, key) {
                            if (value != scope) {
                                value.c = "fa-sort";
                                value.orderType = "";
                            }
                        });
                        scope.clickOrderGroup.orderName = scope.orderName;
                        scope.clickOrderGroup.orderType = scope.orderType;
                        scope.clickOrderGroup.order();
                        scope.$apply();
                    });
                };
            }
        };
    }]);

    app.directive('deleteConfirm', ['$http', function ($http) {
        return {
            restrict: 'A',
            compile: function (tElem, attrs) {
                return function (scope, elem, attrs) {
                    //alert("a");
                    elem.click(function () {
                        content = elem.attr("delete-confirm");
                        if (content.length <= 0) {
                            content = "确定要删除？";
                        }
                        var d = dialog({
                            align: 'left',
                            content: content,
                            quickClose: true,
                            okValue: '确定',
                            ok: function () {
                                var dataAttr = elem.data();
                                if (dataAttr.fnDelete) {
                                    scope.$eval(dataAttr.fnDelete);
                                }
                                //return false;
                            },
                            cancelValue: '取消',
                            cancel: function () { }
                        });
                        d.show(elem[0]);
                    });
                }
            }
        };
    }]);

    app.directive('scrollBox', ['$http', function ($http) {
        return {
            restrict: 'A',
            scope: {
                watchTarget: '=scrollBox'
            },
            compile: function (tElem, attrs) {
                return function (scope, elem, attrs) {
                    var isBottom = true;
                    elem.scroll(function () {
                        nDivHight = elem.height();
                        nScrollHight = elem[0].scrollHeight;
                        nScrollTop = elem[0].scrollTop;
                        if (nScrollTop + nDivHight >= nScrollHight) {
                            isBottom = true;
                        }
                        else {
                            isBottom = false;
                        }
                    });

                    elem.bind('DOMNodeInserted', function (e) {
                        $(e).find("img").load(function () {
                            if (isBottom) {
                                nDivHight = elem.height();
                                nScrollHight = elem[0].scrollHeight;
                                elem[0].scrollTop = nScrollHight - nDivHight;
                            }
                        });

                        if (isBottom) {
                            nDivHight = elem.height();
                            nScrollHight = elem[0].scrollHeight;
                            elem[0].scrollTop = nScrollHight - nDivHight;
                        }
                    })
                }
            }
        };
    }]);
    //directive end

    //filter 
    app.filter('to_trusted', ['$sce', function ($sce) {
        return function (text) {
            return $sce.trustAsHtml(text);
        }
    }]);

    app.filter('FeeFormat', function () {
        return function (num) {
            return Math.ceil(num);
        }
    });

    app.filter('DepartmentGrade', function () {
        return function (grade) {
            var g = "";
            switch (grade) {
                case 0:
                    g = "公司";
                    break;
                case 1:
                    g = "部门";
                    break;
                default:
                    g = "未知"
            }
            return g;
        }
    });

    app.filter('Sex', function () {
        return function (value) {
            var v = "";
            switch (value) {
                case 0:
                    v = "男";
                    break;
                case 1:
                    v = "女";
                    break;
                default:
                    v = "未知"
            }
            return v;
        }
    });

    app.filter('Education', function () {
        return function (value) {
            var v = "";
            switch (value) {
                case 0:
                    v = "高中";
                    break;
                case 1:
                    v = "大专";
                    break;
                case 2:
                    v = "本科";
                    break;
                case 3:
                    v = "硕士";
                    break;
                case 4:
                    v = "MBA";
                    break;
                case 5:
                    v = "EMBA";
                    break;
                default:
                    v = "未知"
            }
            return v;
        }
    });

    app.filter('Boolean', function () {
        return function (value) {
            var v = '否';
            if (value == true || value == 'true') {
                v = '是';
            }
            return v;
        }
    });

    app.filter('ArticleType', function () {
        return function (value) {
            var v = "";
            switch (value) {
                case 0:
                    v = "新闻";
                    break;
                case 1:
                    v = "通知";
                    break;
                default:
                    v = "未知"
            }
            return v;
        }
    });

    app.filter('TaskStatus', function () {
        return function (value) {
            var v = "";
            switch (value) {
                case 0:
                    v = "未开始";
                    break;
                case 1:
                    v = "进行中";
                    break;
                default:
                    v = "已完成"
            }
            return v;
        }
    });
    
    //filteend

});