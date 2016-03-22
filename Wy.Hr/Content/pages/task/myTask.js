define(function (require, exports, module) {
    require("pages/base.js");

    angular.module("app")
    .controller("myTaskController", ['$scope', '$http', '$location', 'alertbox', '$modal', "hotkeys", function ($scope, $http, $location, alertbox, $modal, hotkeys) {
        //内容开始
        $scope.items = [];
        $scope.pageSizes = pageSizes;

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
            $http.post("/api/taskAPI/getMyTask", data)
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

        $scope.excuseTask = function (item) {
            $http.post("/api/taskAPI/excuseTask", {id : item.id})
            .success(function (data, status, headers, config) {
                if (data.success) {
                    item.status = data.content;
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