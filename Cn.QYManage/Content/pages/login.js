define(function (require, exports, module) {
    require("pages/base.js");

    angular.module("app")
    .controller("loginController", ['$scope', '$http', function ($scope, $http) {
        $scope.isLoading = false;
        $scope.login = function () {
            $scope.isLoading = true;
            $http.post("/api/memberAPI/login", { username: $scope.username, password: $scope.password })
            .success(function (data, status, headers, config) {
                if (data.success) {
                    window.location = "/";
                }
                else {
                    alert(data.message);
                    $scope.isLoading = false;
                }
            })
            .error(function (data, status, headers, config) {
                alert(status);
                $scope.isLoading = false;
            });
        }

    } ]);
});