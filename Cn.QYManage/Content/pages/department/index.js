define(function (require, exports, module) {
    require("pages/base.js");

    angular.module("app")
    .controller("departmentController", ['$scope', '$http', '$location', 'alertbox', '$modal', "hotkeys", function ($scope, $http, $location, alertbox, $modal, hotkeys) {
        //内容开始
        $scope.items = [];
        $scope.itemModel = {};
        $scope.treedata = [];
        $scope.selDeptIds = [];
        $scope.pageSizes = pageSizes;
        $scope.selDepts = [];

        //  分页参数
        $scope.searchModel = {
            totalCount: 0,
            pageIndex: 0,
            pageSize: 10,
            totalPageCount: 0,
            departmentName: "",
            parentId: null
        };

        //  设置每次记录数
        $scope.find = function () {
            $scope.searchModel.pageIndex = 1;
            $scope.load();
        };

        //  数据加载
        $scope.load = function () {
            var d = createDialog();
            var data = $scope.searchModel;
            $http.post("/api/DepartmentAPI/GetPagedList", data)
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
                    d.showError(data.message);
                }
            })
            .error(function (data, status, headers, config) {
                alert(status);
            });
        };

        $scope.SelTreeLoad = function () {
            $http.post("/api/departmentAPI/getSelTreeList")
            .success(function (data, status, headers, config) {
                if (data.success) {
                    $scope.selDepts = data.content;
                }
                else {
                    d.showError(data.message);
                }
            })
            .error(function (data, status, headers, config) {
                alert(status);
            });
        };

        $scope.TreeLoad = function () {
            $http.post("/api/DepartmentAPI/GetDepartmentTree")
            .success(function (data, status, headers, config) {
                if (data.success) {
                    $scope.treedata = data.content;
                }
                else {
                    d.showError(data.message);
                }
            })
            .error(function (data, status, headers, config) {
                alert(status);
            });
        };

        $scope.showSelected = function (sel) {
            if ($scope.searchModel.parentId == sel.id) {
                $scope.searchModel.parentId = null;
            }
            else {
                $scope.searchModel.parentId = sel.id;
            }
            $scope.load();
        };

        $scope.selDept = function (id) {
            if ($scope.selDeptIds.indexOf(id) == -1) {
                $scope.selDeptIds.push(id);
            }
            else {
                $scope.selDeptIds.remove(id);
            }
            $('button[name="delDeptBtn"]').attr('disabled', true);
            if ($scope.selDeptIds.length > 0) {
                $('button[name="delDeptBtn"]').removeAttr('disabled');
            }
        }

        //  批量删除
        $scope.batchRemove = function () {
            $http.post("/api/departmentAPI/batchRemove", { Ids: $scope.selDeptIds })
            .success(function (data, status, headers, config) {
                if (data.success) {
                    var delList = [];
                    for (var i = 0; i < $scope.items.length; i++) {
                        if ($scope.selDeptIds.indexOf($scope.items[i].id) != -1) {
                            delList.push($scope.items[i]);
                        }
                    }
                    for (var i = 0; i < delList.length; i++) {
                        $scope.items.remove(delList[i]);
                    }
                    createDialog().showTip("删除成功", 1500);
                    $scope.selDeptIds = [];
                    $('button[name="delDeptBtn"]').attr('disabled', true);
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
                $scope.deptModel = {};
            }
            else {
                for (var i = 0; i < $scope.items.length; i++) {
                    if ($scope.items[i].id == model.id) {
                        var str = JSON.stringify($scope.items[i]);
                        $scope.deptModel = JSON.parse(str);
                        break;
                    }
                }
            }
            $('#departmentEditModal').modal('show');
        }

        $scope.save = function () {
            var d = createDialog();
            var data = $scope.deptModel;
            var type = 'add';
            var typeStr = "添加";
            if (data.id != undefined) {
                typeStr = "修改";
                type = 'update';
            }
            $http.post("/api/departmentAPI/" + type, data)
            .success(function (data, status, headers, config) {
                if (data.success) {
                    $scope.load();
                    if (type == 'add') {
                        $scope.items.splice(0, 0, data.content);
                    }
                    else {
                        for (var i = 0; i < $scope.items.length; i++) {
                            if ($scope.items[i].id == $scope.deptModel.id) {
                                $scope.items[i] = $scope.deptModel;
                                break;
                            }
                        }
                    }
                    $('#departmentEditModal').modal('hide');
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
            $scope.TreeLoad();
            $scope.SelTreeLoad();
        };

        $scope.init();
        //内容结束
    }]);
});