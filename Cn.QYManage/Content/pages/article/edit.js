define(function (require, exports, module) {
    require("pages/base.js");

    angular.module("app")
    .controller("articleEditController", ['$scope', '$http', '$location', 'alertbox', '$modal', "hotkeys", function ($scope, $http, $location, alertbox, $modal, hotkeys) {
        //内容开始
        $scope.articleModel = {
            type: 0
        };

        $scope.preview = function () {
            $scope.articleModel.contents = $('#editor').html();
            $('#previewModal').modal('show');
        }

        $scope.update = function () {
            var d = createDialog();
            $scope.articleModel.contents = $('#editor').html();
            var data = $scope.articleModel;
            $http.post("/api/articleAPI/update", data)
            .success(function (data, status, headers, config) {
                if (data.success) {
                    d.showTip('文章更新成功', 2000);
                }
                else {
                    d.showTip('文章更新失败，' + data.message, 2000);
                }
            })
            .error(function (data, status, headers, config) {
                alert(status);
            });
        }

        $scope.init = function (id) {
            $http.post("/api/articleAPI/getDetail", { id: id })
            .success(function (data, status, headers, config) {
                if (data.success) {
                    $scope.articleModel = data.content;
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