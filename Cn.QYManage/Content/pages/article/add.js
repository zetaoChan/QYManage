define(function (require, exports, module) {
    require("pages/base.js");

    angular.module("app")
    .controller("articleAddController", ['$scope', '$http', '$location', 'alertbox', '$modal', "hotkeys", function ($scope, $http, $location, alertbox, $modal, hotkeys) {
        //内容开始
        $scope.articleModel = {};

        $scope.preview = function () {
            $scope.articleModel.contents = $('#editor').html();
            $('#previewModal').modal('show');
        }

        $scope.save = function () {
            $scope.articleModel.contents = $('#editor').html();
            var data = $scope.articleModel;
            $http.post("/api/articleAPI/add", data)
            .success(function (data, status, headers, config) {
                if (data.success) {
                    $scope.articleModel = {};
                    $('#editor').html('输入内容');
                    createDialog().showTip('文章添加成功', 2000);
                }
                else {
                    createDialog().showTip('文章添加失败，' + data.message, 2500);
                }
            })
            .error(function (data, status, headers, config) {
                alert(status);
            });
        }

        //内容结束
    }]);
    
});