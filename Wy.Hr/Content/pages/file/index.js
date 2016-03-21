define(function (require, exports, module) {
    require("pages/base.js");

    angular.module("app")
    .controller("fileController", ['$scope', '$http', '$location', 'alertbox', '$modal', "hotkeys", function ($scope, $http, $location, alertbox, $modal, hotkeys) {
        //内容开始
        $scope.items = [];
        $scope.selFileIds = [];

        //  数据加载
        $scope.load = function () {
            var d = createDialog();
            $http.post("/api/fileAPI/GetList")
            .success(function (data, status, headers, config) {
                if (data.success) {
                    $scope.items = data.content;
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

        $scope.selFile = function (id) {
            if ($scope.selFileIds.indexOf(id) == -1) {
                $scope.selFileIds.push(id);
            }
            else {
                $scope.selFileIds.remove(id);
            }
            $('button[name="delFileBtn"]').attr('disabled', true);
            if ($scope.selFileIds.length > 0) {
                $('button[name="delFileBtn"]').removeAttr('disabled');
            }
        }

        //  批量删除
        $scope.batchRemove = function () {
            $http.post("/api/fileAPI/batchRemove", { Ids: $scope.selFileIds })
            .success(function (data, status, headers, config) {
                if (data.success) {
                    var delList = [];
                    for (var i = 0; i < $scope.items.length; i++) {
                        if ($scope.selFileIds.indexOf($scope.items[i].id) != -1) {
                            delList.push($scope.items[i]);
                        }
                    }
                    for (var i = 0; i < delList.length; i++) {
                        $scope.items.remove(delList[i]);
                    }
                    createDialog().showTip("删除成功", 1500);
                    $scope.selFileIds = [];
                    $('button[name="delFileBtn"]').attr('disabled', true);
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