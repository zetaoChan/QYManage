define(function (require, exports, module) {
    require("pages/base.js");

    angular.module("app")
    .controller("roleController", ['$scope', '$http', '$location', 'alertbox', '$modal', "hotkeys", function ($scope, $http, $location, alertbox, $modal, hotkeys) {
        //内容开始
        $scope.items = [];
        $scope.roleModel = {};
        $scope.selRoleIds = [];
        $scope.pageSizes = pageSizes;
        $scope.permissionItems = [];


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
            $http.post("/api/roleAPI/getPagedList", data)
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

        $scope.loadPermissions = function () {
            $http.post("/api/permissionAPI/getAllPermission")
            .success(function (data, status, headers, config) {
                if (data.success) {
                    $scope.permissionItems = data.content;
                }
            })
            .error(function (data, status, headers, config) {
                alert(status);
            });
        };
        
        $scope.selPermission = function (item) {
            if ($scope.selRoleIds.indexOf(id) == -1) {
                $scope.selRoleIds.push(id);
            }
            else {
                $scope.selRoleIds.remove(id);
            }
            $('button[name="delRoleBtn"]').attr('disabled', true);
            if ($scope.selRoleIds.length > 0) {
                $('button[name="delRoleBtn"]').removeAttr('disabled');
            }
        }

        $scope.selRole = function (id) {
            if ($scope.selRoleIds.indexOf(id) == -1) {
                $scope.selRoleIds.push(id);
            }
            else {
                $scope.selRoleIds.remove(id);
            }
            $('button[name="delRoleBtn"]').attr('disabled', true);
            if ($scope.selRoleIds.length > 0) {
                $('button[name="delRoleBtn"]').removeAttr('disabled');
            }
        }

        //  批量删除
        $scope.batchRemove = function () {
            $http.post("/api/roleAPI/batchRemove", { Ids: $scope.selRoleIds })
            .success(function (data, status, headers, config) {
                if (data.success) {
                    var delList = [];
                    for (var i = 0; i < $scope.items.length; i++) {
                        if ($scope.selRoleIds.indexOf($scope.items[i].id) != -1) {
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
                $scope.roleModel = {};
            }
            else {
                for (var i = 0; i < $scope.items.length; i++) {
                    if ($scope.items[i].id == model.id) {
                        var str = JSON.stringify($scope.items[i]);
                        $scope.roleModel = JSON.parse(str);
                        break;
                    }
                }
            }
            $('#roleEditModal').modal('show');
        }

        $scope.save = function () {
            var d = createDialog();
            var data = $scope.roleModel;
            var type = 'add';
            var typeStr = "添加";
            if (data.id != undefined) {
                typeStr = "修改";
                type = 'update';
            }
            $http.post("/api/roleAPI/" + type, data)
            .success(function (data, status, headers, config) {
                if (data.success) {
                    $scope.load();
                    if (type == 'add') {
                        $scope.items.splice(0, 0, data.content);
                    }
                    else {
                        for (var i = 0; i < $scope.items.length; i++) {
                            if ($scope.items[i].id == $scope.roleModel.id) {
                                $scope.items[i] = $scope.roleModel;
                                break;
                            }
                        }
                    }
                    $('#roleEditModal').modal('hide');
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
            $scope.loadPermissions();
        };

        $scope.init();
        //内容结束
    }]);
});