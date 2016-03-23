define(function (require, exports, module) {
    require("pages/base.js?v=1.1");

    angular.module("app")
    .controller("messageController", ['$scope', '$http', '$location', 'alertbox', '$modal', "hotkeys", function ($scope, $http, $location, alertbox, $modal, hotkeys) {
        //内容开始
        $scope.items = [];
        $scope.msgModel = {};
        $scope.selMessages = [];
        $scope.detailModel = {};

        //  分页参数
        $scope.searchModel = {
            totalCount: 0,
            pageIndex: 0,
            pageSize: 10,
            totalPageCount: 0
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

        $scope.selMsg = function (item) {
            if ($scope.selMessages.indexOf(item) == -1) {
                $scope.selMessages.push(item);
            }
            else {
                $scope.selMessages.remove(item);
            }
            $('button[name="delMsgBtn"]').attr('disabled', true);
            if ($scope.selMessages.length > 0) {
                $('button[name="delMsgBtn"]').removeAttr('disabled');
            }
        }

        //  批量删除
        $scope.batchRemove = function () {
            var ids = [];
            for (var i = 0; i < $scope.selMessages.length; i++){
                ids.push($scope.selMessages[i].id);
            }
            $http.post("/api/messageAPI/batchRemove", { Ids: ids })
            .success(function (data, status, headers, config) {
                if (data.success) {
                    var delList = [];
                    for (var i = 0; i < $scope.items.length; i++) {
                        if ($scope.selMessages.indexOf($scope.items[i]) != -1) {
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

        $scope.seeDetail = function (item) {
            $scope.detailModel = item;
            $('#msgDetailModal').modal('show');
        };

        $scope.openDialog = function () {
            $scope.msgModel = {};
            $('#msgAddModal').modal('show');
        }

        $scope.send = function () {
            $http.post("/api/messageAPI/add", $scope.msgModel)
            .success(function (data, status, headers, config) {
                if (data.success) {
                    $scope.load();
                }
            })
            .error(function (data, status, headers, config) {
                alert(status);
            });
        };

        $('#recipientName').typeahead({
            source: function (query, process) {
                var parameter = { query: query };
                $.post('/api/userAPI/autoComplate', parameter, function (data) {
                    process(data.content);
                });
            }
        });

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