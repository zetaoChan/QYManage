define(function (require, exports, module) {
    require("pages/base.js");

    angular.module("app")
    .controller("articleDetailController", ['$scope', '$http', '$location', 'alertbox', '$modal', "hotkeys", function ($scope, $http, $location, alertbox, $modal, hotkeys) {
        //内容开始
        $scope.items = [];
        $scope.articleModel = {};

        $scope.init = function (id) {
            $http.post("/api/articleAPI/getDetailAndList", { id: id })
            .success(function (data, status, headers, config) {
                if (data.success) {
                    $scope.articleModel = data.content.article;
                    $scope.items = data.content.items;
                }
                else {
                    d.showError(data.message);
                }
            })
            .error(function (data, status, headers, config) {
                alert(status);
            });
        }

        //内容结束
    }]);
    
});