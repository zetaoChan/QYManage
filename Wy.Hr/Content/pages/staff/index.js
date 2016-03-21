define(function (require, exports, module) {
    require("pages/base.js");

    angular.module("app")
    .controller("staffController", ['$scope', '$http', '$location', 'alertbox', '$modal', "hotkeys", function ($scope, $http, $location, alertbox, $modal, hotkeys) {
        //内容开始
        $scope.items = [];
        $scope.selEducations = education;
        $scope.selMaritalStatus = maritalStatus;
        $scope.selNativePlaces = nativePlace;
        $scope.selNations = nation;
        $scope.staffModel = {
            sex: 0,
            maritalStatus: 0
        };
        $scope.treedata = [];
        $scope.selStaffIds = [];
        $scope.selPositions = [];
        $scope.pageSizes = pageSizes;
        $scope.selDepts = [];

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

        $scope.PositionLoad = function () {
            $http.post("/api/positionAPI/GetSelList")
            .success(function (data, status, headers, config) {
                if (data.success) {
                    $scope.selPositions = data.content;
                }
                else {
                    d.showError(data.message);
                }
            })
            .error(function (data, status, headers, config) {
                alert(status);
            });
        };

        //  分页参数
        $scope.searchModel = {
            totalCount: 0,
            pageIndex: 0,
            pageSize: 10,
            totalPageCount: 0
        };

        //  分页参数
        $scope.find = function() {
            $scope.searchModel.pageIndex = 1;
            $scope.load();
        };

        //  员工数据加载
        $scope.load = function () {
            var d = createDialog();
            var data = $scope.searchModel;
            $http.post("/api/staffAPI/getPagedList", data)
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

        $scope.selStaff = function (id) {
            if ($scope.selStaffIds.indexOf(id) == -1) {
                $scope.selStaffIds.push(id);
            }
            else {
                $scope.selStaffIds.remove(id);
            }
            $('button[name="delStaffBtn"]').attr('disabled', true);
            if ($scope.selStaffIds.length > 0) {
                $('button[name="delStaffBtn"]').removeAttr('disabled');
            }
        }

        //  批量删除
        $scope.batchRemove = function () {
            $http.post("/api/staffAPI/batchRemove", { Ids: $scope.selStaffIds })
            .success(function (data, status, headers, config) {
                if (data.success) {
                    var delList = [];
                    for (var i = 0; i < $scope.items.length; i++) {
                        if ($scope.selStaffIds.indexOf($scope.items[i].id) != -1) {
                            delList.push($scope.items[i]);
                        }
                    }
                    for (var i = 0; i < delList.length; i++) {
                        $scope.items.remove(delList[i]);
                    }
                    createDialog().showTip("删除成功", 1500);
                    $('button[name="delStaffBtn"]').attr('disabled', true);
                    $scope.selStaffIds = [];
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
                $scope.staffModel = {
                    sex: 0,
                    maritalStatus: 0
                };
            }
            else {
                for (var i = 0; i < $scope.items.length; i++) {
                    if ($scope.items[i].id == model.id) {
                        var str = JSON.stringify($scope.items[i]);
                        $scope.staffModel = JSON.parse(str);
                        $scope.staffModel.entryTime = new Date($scope.staffModel.entryTime).format('yyyy-MM-dd');
                        $scope.staffModel.birthDate = new Date($scope.staffModel.birthDate).format('yyyy-MM-dd');
                        break;
                    }
                }
            }
            $('#staffEditModal').modal('show');
        }

        $scope.save = function () {
            var d = createDialog();
            var data = $scope.staffModel;
            var type = 'add';
            var typeStr = "添加";
            if (data.id != undefined) {
                typeStr = "修改";
                type = 'update';
            }
            $http.post("/api/staffAPI/" + type, data)
            .success(function (data, status, headers, config) {
                if (data.success) {
                    $scope.load();
                    if (type == 'add') {
                        $scope.items.splice(0, 0, data.content);
                    }
                    else {
                        for (var i = 0; i < $scope.items.length; i++) {
                            if ($scope.items[i].id == $scope.staffModel.id) {
                                $scope.items[i] = $scope.staffModel;
                                break;
                            }
                        }
                    }
                    $('#staffEditModal').modal('hide');
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

        $scope.showSelected = function (sel) {
            if ($scope.searchModel.departmentId == sel.id) {
                $scope.searchModel.departmentId = null;
            }
            else {
                $scope.searchModel.departmentId = sel.id;
            }
            $scope.load();
        };

        $scope.init = function () {
            $scope.load();
            $scope.TreeLoad();
            $scope.PositionLoad();
            $scope.SelTreeLoad();
        };

        $scope.init();
        //内容结束
    }]);
});