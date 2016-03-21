define(function (require, exports, module) {
    require("pages/base.js");

    angular.module("app")
    .controller("positionController", ['$scope', '$http', '$location', 'alertbox', '$modal', "hotkeys", function ($scope, $http, $location, alertbox, $modal, hotkeys) {
        //内容开始
        $scope.items = [];
        $scope.positionModel = {};
        $scope.selPositionIds = [];
        $scope.pageSizes = pageSizes;
        $scope.selItems = ['Z1', 'Z2', 'Z3', 'Z4', 'Z5', 'Z6', 'Z7'];

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
            $scope.searchModel.pageIndex = 1;
            $scope.load();
        };

        //  数据加载
        $scope.load = function () {
            var d = createDialog();
            var data = $scope.searchModel;
            $http.post("/api/positionAPI/getPagedList", data)
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

        $scope.openDialog = function (model) {
            if (model == undefined) {
                $scope.positionModel = {};
            }
            else {
                for (var i = 0; i < $scope.items.length; i++) {
                    if ($scope.items[i].id == model.id) {
                        var str = JSON.stringify($scope.items[i]);
                        $scope.positionModel = JSON.parse(str);
                        break;
                    }
                }
            }
            $('#positionEditModal').modal('show');
        }

        $scope.save = function () {
            var d = createDialog();
            var data = $scope.positionModel;
            var type = 'add';
            var typeStr = "添加";
            if (data.id != undefined) {
                typeStr = "修改";
                type = 'update';
            }
            $http.post("/api/positionAPI/" + type, data)
            .success(function (data, status, headers, config) {
                if (data.success) {
                    $scope.load();
                    if (type == 'add') {
                        $scope.items.splice(0, 0, data.content);
                    }
                    else {
                        for (var i = 0; i < $scope.items.length; i++) {
                            if ($scope.items[i].id == $scope.positionModel.id) {
                                $scope.items[i] = $scope.positionModel;
                                break;
                            }
                        }
                    }
                    $('#positionEditModal').modal('hide');
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

        $scope.selPosition = function (id) {
            if ($scope.selPositionIds.indexOf(id) == -1) {
                $scope.selPositionIds.push(id);
            }
            else {
                $scope.selPositionIds.remove(id);
            }
            $('button[name="delPositionBtn"]').attr('disabled', true);
            if ($scope.selPositionIds.length > 0) {
                $('button[name="delPositionBtn"]').removeAttr('disabled');
            }
        }

        //  批量删除
        $scope.batchRemove = function () {
            $http.post("/api/positionAPI/batchRemove", { Ids: $scope.selPositionIds })
            .success(function (data, status, headers, config) {
                if (data.success) {
                    var delList = [];
                    for (var i = 0; i < $scope.items.length; i++) {
                        if ($scope.selPositionIds.indexOf($scope.items[i].id) != -1) {
                            delList.push($scope.items[i]);
                        }
                    }
                    for (var i = 0; i < delList.length; i++) {
                        $scope.items.remove(delList[i]);
                    }
                    createDialog().showTip("删除成功", 1500);
                    $scope.selPositionIds = [];
                    $('button[name="delPositionBtn"]').attr('disabled', true);
                }
                else {
                    createDialog().showTip("删除失败," + data.message, 2500);
                }
            })
            .error(function (data, status, headers, config) {
                alert(status);
            });
        };

        $scope.init = function () {
            $scope.load();
        };

        $scope.init();
        //内容结束
    }]);
});