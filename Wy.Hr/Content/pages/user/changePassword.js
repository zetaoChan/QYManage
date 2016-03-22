define(function (require, exports, module) {
    require("pages/base.js");

    angular.module("app")
    .controller("changePasswordController", ['$scope', '$http', '$location', 'alertbox', '$modal', "hotkeys", function ($scope, $http, $location, alertbox, $modal, hotkeys) {
        //内容开始
        $scope.userModel = {};

        //  修改密码
        $scope.changePassword = function () {
            var data = $scope.userModel;
            if (data.cfgNewPassword != data.newPassword) {
                createDialog().showTip("密码不一致", 2500);
            }
            else{
                $http.post("/api/userAPI/changePassword", data)
                .success(function (data, status, headers, config) {
                    if (data.success) {
                        createDialog().showTip("修改成功", 1500);
                    }
                    else {
                        createDialog().showTip("修改失败," + data.message, 2500);
                    }
                })
                .error(function (data, status, headers, config) {
                    alert(status);
                });
            }
        };

        

        //内容结束
    }]);
});