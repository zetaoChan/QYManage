define(function (require, exports, module) {
    require("pages/base.js");

    angular.module("app")
    .controller("indexController", ['$scope', '$http', '$location', 'alertbox', '$modal', "hotkeys", function ($scope, $http, $location, alertbox, $modal, hotkeys) {
        $scope.newsList = [];
        $scope.noticeList = [];

        $scope.loadNews = function (params) {
            $http.post("/api/articleAPI/getTopArticles", params)
            .success(function (data, status, headers, config) {
                if (data.success) {
                    $scope.newsList = data.content;
                }
                else {
                    createDialog().showTip("加载失败，" + data.message, 2500);
                }
            })
            .error(function (data, status, headers, config) {
                alert(status);
            });
        };

        $scope.loadNotice = function (params) {
            $http.post("/api/articleAPI/getTopArticles", params)
            .success(function (data, status, headers, config) {
                if (data.success) {
                    $scope.noticeList = data.content;
                }
                else {
                    createDialog().showTip("加载失败，" + data.message, 2500);
                }
            })
            .error(function (data, status, headers, config) {
                alert(status);
            });
        };

        $scope.init = function () {
            $scope.loadNews({ type: 0, top: 10 });
            $scope.loadNotice({ type: 1, top: 10 });
        }

        $scope.init();
        //内容结束
    }]);
});