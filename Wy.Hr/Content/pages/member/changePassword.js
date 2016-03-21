define(function (require, exports, module) {
    require("pages/base.js");

    angular.module("app")
    .controller("changePasswordController", ['$scope', '$http', '$location', 'alertbox', function ($scope, $http, $location, alertbox) {
        $scope.reset = function () {
            $scope.oldPassword = "";
            $scope.newPassword = "";
            $scope.confirmPassword = "";
        }

        $scope.submit = function () {
            $http.post("/api/memberapi/changePasswordForMe", { oldPassword: $scope.oldPassword, newPassword: $scope.newPassword, confirmPassword: $scope.confirmPassword })
            .success(function (data, status, headers, config) {
                if (data.success) {
                    alert("修改成功,请牢记新密码！");
                    $scope.oldPassword = "";
                    $scope.newPassword = "";
                    $scope.confirmPassword = "";
                }
                else {
                    alert(data.message);
                }
            })
            .error(function (data, status, headers, config) {
                alert(status);
            });
        }
    } ]);
});