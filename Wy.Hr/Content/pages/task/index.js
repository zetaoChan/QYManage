define(function (require, exports, module) {
    require("pages/base.js");

    angular.module("app")
    .controller("taskController", ['$scope', '$http', '$location', 'alertbox', '$modal', "hotkeys", function ($scope, $http, $location, alertbox, $modal, hotkeys) {
        //内容开始
        $scope.items = [];

        //  分页参数
        $scope.searchModel = {
            totalCount: 0,
            pageIndex: 0,
            pageSize: 10,
            totalPageCount: 0
        };

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

        $scope.selTask = function (id) {
            if ($scope.selTaskIds.indexOf(id) == -1) {
                $scope.selTaskIds.push(id);
            }
            else {
                $scope.selTaskIds.remove(id);
            }
            $('button[name="delTaskBtn"]').attr('disabled', true);
            if ($scope.selTaskIds.length > 0) {
                $('button[name="delTaskBtn"]').removeAttr('disabled');
            }
        }

        //  批量删除
        $scope.batchRemove = function () {
            $http.post("/api/taskAPI/batchRemove", { Ids: $scope.selTaskIds })
            .success(function (data, status, headers, config) {
                if (data.success) {
                    var delList = [];
                    for (var i = 0; i < $scope.items.length; i++) {
                        if ($scope.selTaskIds.indexOf($scope.items[i].id) != -1) {
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

        $scope.openDialog = function (model) {
            if (model == undefined) {
                $scope.TaskModel = {};
            }
            else {
                for (var i = 0; i < $scope.items.length; i++) {
                    if ($scope.items[i].id == model.id) {
                        var str = JSON.stringify($scope.items[i]);
                        $scope.TaskModel = JSON.parse(str);
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

        //内容结束
    }]);
});