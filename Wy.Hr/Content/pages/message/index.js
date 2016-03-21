define(function (require, exports, module) {
    require("pages/base.js?v=1.1");
    require("js/json2.js");

    angular.module("app")
    .controller("messageController", ['$scope', '$http', '$location', 'alertbox', '$modal', "hotkeys", function ($scope, $http, $location, alertbox, $modal, hotkeys) {
        //内容开始
        $scope.items = [];
        $scope.itemModel = {};
        $scope.viewType = "list";
        $scope.selMessageIds = [];

        //  分页参数
        $scope.searchModel = {
            totalCount: 0,
            pageIndex: 0,
            pageSize: 10,
            totalPageCount: 0,
            messageType: ''
        };

        //  查询方法
        $scope.find = function () {
            $scope.searchModel.pageIndex = 0;
            $scope.load();
        };

        //  员工数据加载
        $scope.load = function () {
            var d = createDialog();
            var data = $scope.searchModel;
            $http.post("/api/messageAPI/getPagedList", data)
            .success(function (data, status, headers, config) {
                if (data.success) {
                    $scope.items = data.content.items;
                    $scope.searchModel.totalCount = data.content.totalCount;
                    $scope.searchModel.pageIndex = data.content.pageIndex;
                    $scope.searchModel.pageSize = data.content.pageSize;
                    $scope.searchModel.totalPageCount = data.content.totalPageCount;
                    d.close();
                }
                else {
                    d.showTip("加载失败，" + data.message, 2500);
                }
            })
            .error(function (data, status, headers, config) {
                alert(status);
            });
        };

        $scope.selMessage = function (id) {
            if ($scope.selMessageIds.indexOf(id) == -1) {
                $scope.selMessageIds.push(id);
            }
            else {
                $scope.selMessageIds.remove(id);
            }
            $('button[name="delMessageBtn"]').attr('disabled', true);
            if ($scope.selMessageIds.length > 0) {
                $('button[name="delMessageBtn"]').removeAttr('disabled');
            }
        }

        //  批量删除
        $scope.batchRemove = function () {
            $http.post("/api/messageAPI/batchRemove", { Ids: $scope.selMessageIds })
            .success(function (data, status, headers, config) {
                if (data.success) {
                    var delList = [];
                    for (var i = 0; i < $scope.items.length; i++) {
                        if ($scope.selMessageIds.indexOf($scope.items[i].id) != -1) {
                            delList.push($scope.items[i]);
                        }
                    }
                    for (var i = 0; i < delList.length; i++) {
                        $scope.items.remove(delList[i]);
                    }
                    createDialog().showTip("删除成功", 1500);
                }
                else {
                    createDialog().showTip("删除失败," + data.message, 2500);
                }
            })
            .error(function (data, status, headers, config) {
                alert(status);
            });
        };

        $scope.back = function () {
            $scope.load();
            $scope.viewType = "list";
        }

        $scope.detail = function (id) {
            var d = createDialog();
            $http.post("/api/messageAPI/getDetail", { id: id })
            .success(function (data, status, headers, config) {
                if (data.success) {
                    $scope.itemModel = data.content;
                    $scope.viewType = "detail";
                    d.close();
                }
            })
            .error(function (data, status, headers, config) {
                alert(status);
            });
        };

        $scope.changeType = function (type) {
            $scope.searchModel.messageType = type;
            $scope.load();
        }

        $scope.init = function () {
            $scope.load();
        };

        $scope.init();
        //内容结束
    }])
    .filter('MessageType', function () {
        return function (value) {
            var v = "";
            switch (value) {
                case 0:
                    v = "系统消息";
                    break;
                case 1:
                    v = "用户消息";
                    break;
                default:
                    v = "未知"
            }
            return v;
        }
    });
});