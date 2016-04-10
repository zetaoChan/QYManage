define(function (require, exports, module) {
    require("pages/base.js");

    angular.module("app")
    .controller("articleListController", ['$scope', '$http', '$location', 'alertbox', '$modal', "hotkeys", function ($scope, $http, $location, alertbox, $modal, hotkeys) {
        //内容开始
        $scope.items = [];
        $scope.pageSizes = pageSizes;

        //  分页参数
        $scope.searchModel = {
            totalCount: 0,
            pageIndex: 0,
            pageSize: 10,
            totalPageCount: 0,
            type: '',
            title: ''
        };

        //  设置每次记录数
        $scope.changeType = function (type) {
            $scope.searchModel.type = type;
            $('.articleTypeBtn button').removeClass('active');
            $('.articleTypeBtn button[name="' + $scope.searchModel.type + '"]').addClass('active');
            $scope.load();
        };

        //  数据加载
        $scope.load = function () {
            var d = createDialog();
            var data = $scope.searchModel;
            $http.post("/api/articleAPI/getPagedList", data)
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


        $scope.init = function (type) {
            $scope.searchModel.type = type;
            $scope.load();
        };

        //内容结束
    }]);
    
});