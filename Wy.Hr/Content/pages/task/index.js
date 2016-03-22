define(function (require, exports, module) {
    require("pages/base.js");

    angular.module("app")
    .controller("taskController", ['$scope', '$http', '$location', 'alertbox', '$modal', "hotkeys", function ($scope, $http, $location, alertbox, $modal, hotkeys) {
        //内容开始
        $scope.items = [];
        $scope.taskModel = {};
        $scope.pageSizes = pageSizes;
        $scope.selTaskItems = [];

        //  分页参数
        $scope.searchModel = {
            totalCount: 0,
            pageIndex: 0,
            pageSize: 10,
            totalPageCount: 0
        };

        $('#executor').typeahead({
            source: function (query, process) {
                var parameter = { query: query };
                $.post('/api/userAPI/autoComplate', parameter, function (data) {
                    process(data.content);
                });
            }
        });

        //  员工数据加载
        $scope.load = function () {
            var d = createDialog();
            var data = $scope.searchModel;
            $http.post("/api/taskAPI/getPagedList", data)
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

        $scope.selTask = function (item) {
            if ($scope.selTaskItems.indexOf(item) == -1) {
                $scope.selTaskItems.push(item);
            }
            else {
                $scope.selTaskItems.remove(item);
            }
            $('button[name="delTaskBtn"]').attr('disabled', true);
            if ($scope.selTaskItems.length > 0) {
                $('button[name="delTaskBtn"]').removeAttr('disabled');
            }
        }

        //  批量删除
        $scope.batchRemove = function () {
            var ids = [];
            for (var i = 0; i < $scope.selTaskItems.length; i++) {
                ids.push($scope.selTaskItems[i].id);
            }
            $http.post("/api/taskAPI/batchRemove", { Ids: ids })
            .success(function (data, status, headers, config) {
                if (data.success) {
                    var delList = [];
                    for (var i = 0; i < $scope.items.length; i++) {
                        if (ids.indexOf($scope.items[i].id) != -1) {
                            delList.push($scope.items[i]);
                        }
                    }
                    for (var i = 0; i < delList.length; i++) {
                        $scope.items.remove(delList[i]);
                    }
                    createDialog().showTip("删除成功", 1500);
                    $('button[name="delTaskBtn"]').attr('disabled', true);
                }
                else {
                    createDialog().showTip("删除失败," + data.message, 2500);
                }
            })
            .error(function (data, status, headers, config) {
                alert(status);
            });
        };

        $scope.openDialog = function (model) {
            if (model == undefined) {
                $scope.taskModel = {};
            }
            else {
                for (var i = 0; i < $scope.items.length; i++) {
                    if ($scope.items[i].id == model.id) {
                        var str = JSON.stringify($scope.items[i]);
                        $scope.taskModel = JSON.parse(str);
                        $scope.taskModel.expectedTime = new Date($scope.taskModel.expectedTime).format('yyyy-MM-dd');
                        break;
                    }
                }
            }
            $('#taskEditModal').modal('show');
        }

        $scope.save = function () {
            var d = createDialog();
            var data = $scope.taskModel;
            var type = 'add';
            var typeStr = "添加";
            console.log(data.id);
            if (data.id != undefined) {
                typeStr = "修改";
                type = 'update';
            }
            $http.post("/api/taskAPI/" + type, data)
            .success(function (data, status, headers, config) {
                if (data.success) {
                    $scope.load();
                    if (type == 'add') {
                        $scope.items.splice(0, 0, data.content);
                    }
                    else {
                        for (var i = 0; i < $scope.items.length; i++) {
                            if ($scope.items[i].id == $scope.taskModel.id) {
                                $scope.items[i] = $scope.taskModel;
                                break;
                            }
                        }
                    }
                    $('#taskEditModal').modal('hide');
                    d.showTip(typeStr + "成功", 1500);
                }
                else {
                    d.showTip(typeStr + "失败," + data.message, 2500);
                }
            })
            .error(function (data, status, headers, config) {
                alert(status);
            });
        }

        $scope.init = function () {
            $scope.load();
        };

        $scope.init();
        //内容结束
    }]);
});