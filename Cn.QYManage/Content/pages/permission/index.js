define(function (require, exports, module) {
    require("pages/base.js");

    angular.module("app")
    .controller("permissionController", ['$scope', '$http', '$location', 'alertbox', '$modal', "hotkeys", function ($scope, $http, $location, alertbox, $modal, hotkeys) {
        //内容开始
        $scope.items = [];
        $scope.permissionModel = {};
        $scope.selPermissionIds = [];
        $scope.pageSizes = pageSizes;


        //  分页参数
        $scope.searchModel = {
            totalCount: 0,
            pageIndex: 0,
            pageSize: 10,
            totalPageCount: 0,
            name: ''
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
            $http.post("/api/permissionAPI/getPagedList", data)
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

        $scope.selPermission = function (id) {
            if ($scope.selPermissionIds.indexOf(id) == -1) {
                $scope.selPermissionIds.push(id);
            }
            else {
                $scope.selPermissionIds.remove(id);
            }
            $('button[name="delPermissionBtn"]').attr('disabled', true);
            if ($scope.selPermissionIds.length > 0) {
                $('button[name="delPermissionBtn"]').removeAttr('disabled');
            }
        }

        //  批量删除
        $scope.batchRemove = function () {
            $http.post("/api/permissionAPI/batchRemove", { Ids: $scope.selPermissionIds })
            .success(function (data, status, headers, config) {
                if (data.success) {
                    var delList = [];
                    for (var i = 0; i < $scope.items.length; i++) {
                        if ($scope.selPermissionIds.indexOf($scope.items[i].id) != -1) {
                            delList.push($scope.items[i]);
                        }
                    }
                    for (var i = 0; i < delList.length; i++) {
                        $scope.items.remove(delList[i]);
                    }
                    createDialog().showTip("删除成功", 1500);
                    $scope.selPermissionIds = [];
                    $('button[name="delPermissionBtn"]').attr('disabled', true);
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
                $scope.permissionModel = {};
            }
            else {
                for (var i = 0; i < $scope.items.length; i++) {
                    if ($scope.items[i].id == model.id) {
                        var str = JSON.stringify($scope.items[i]);
                        $scope.permissionModel = JSON.parse(str);
                        break;
                    }
                }
            }
            $('#permissionEditModal').modal('show');
        }

        $scope.save = function () {
            var d = createDialog();
            var data = $scope.permissionModel;
            var type = 'add';
            var typeStr = "添加";
            if (data.id != undefined) {
                typeStr = "修改";
                type = 'update';
            }
            $http.post("/api/permissionAPI/" + type, data)
            .success(function (data, status, headers, config) {
                if (data.success) {
                    $scope.load();
                    if (type == 'add') {
                        $scope.items.splice(0, 0, data.content);
                    }
                    else {
                        for (var i = 0; i < $scope.items.length; i++) {
                            if ($scope.items[i].id == $scope.permissionModel.id) {
                                $scope.items[i] = $scope.permissionModel;
                                break;
                            }
                        }
                    }
                    $('#permissionEditModal').modal('hide');
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